using System;
using System.Collections.Generic;

// 아이템 수량을 관리하는 인벤토리 Ability
public sealed class InventoryAbility : Ability
{
    // 수량 변경 이벤트
    public event Action<ItemEventPayload> AmountChanged;

    IItemDefinitionProvider provider;
    IReadOnlyList<IItemRule> rules;
    readonly Dictionary<int, int> amounts = new();

    // 아이템 정의 공급자와 규칙을 주입한다
    public void Configure(IItemDefinitionProvider provider, IReadOnlyList<IItemRule> rules = null)
    {
        this.provider = provider;
        this.rules = rules ?? new List<IItemRule>();
    }

    // 아이템 수량을 조회한다
    public bool TryGetAmount(int id, out int amount)
    {
        amount = 0;
        if (!TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        return amounts.TryGetValue(resolvedId, out amount);
    }

    // 아이템 수량을 없으면 0으로 반환한다
    public int GetAmountOrZero(int id)
    {
        if (TryGetAmount(id, out var amount))
        {
            return amount;
        }
        return 0;
    }

    // 특정 수량 이상 보유 여부를 확인한다
    public bool HasAtLeast(int id, int requiredAmount)
    {
        if (requiredAmount <= 0)
        {
            return false;
        }

        return TryGetAmount(id, out var currentAmount) && currentAmount >= requiredAmount;
    }

    // 아이템 수량을 증가시킨다
    // 흐름: 입력 검증 -> ID 검증 -> 룰 검증 -> 수량 변경 -> 이벤트 발행
    public bool TryAdd(int id, int amount, string reason = null)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (!TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        if (!CanAdd(resolvedId, amount))
        {
            return false;
        }

        amounts.TryGetValue(resolvedId, out var previous);
        if (previous > int.MaxValue - amount)
        {
            // 오버플로우 방지
            return false;
        }

        var current = previous + amount;

        amounts[resolvedId] = current;
        EmitChanged(resolvedId, previous, amount, current, reason);
        return true;
    }

    // 아이템 수량을 차감한다
    // 흐름: 입력 검증 -> ID 검증 -> 룰 검증 -> 보유 수량 확인 -> 수량 변경 -> 이벤트 발행
    public bool TryRemove(int id, int amount, string reason = null)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (!TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        if (!CanRemove(resolvedId, amount))
        {
            return false;
        }

        amounts.TryGetValue(resolvedId, out var previous);
        if (previous < amount)
        {
            // 보유 수량 부족
            return false;
        }

        var current = previous - amount;
        amounts[resolvedId] = current;
        EmitChanged(resolvedId, previous, -amount, current, reason);
        return true;
    }

    // 아이템 수량을 지정값으로 설정한다
    // 흐름: 입력 검증 -> ID 검증 -> 룰 검증 -> 수량 변경 -> 이벤트 발행
    public bool TrySetAmount(int id, int newAmount, string reason = null)
    {
        if (newAmount < 0)
        {
            return false;
        }

        if (!TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        if (!CanSetAmount(resolvedId, newAmount))
        {
            return false;
        }

        amounts.TryGetValue(resolvedId, out var previous);
        if (previous == newAmount)
        {
            // 변경이 없으면 성공 처리
            return true;
        }

        amounts[resolvedId] = newAmount;
        var delta = newAmount - previous;
        EmitChanged(resolvedId, previous, delta, newAmount, reason);
        return true;
    }

    // 전체 수량 사본을 반환한다
    public IReadOnlyDictionary<int, int> GetAmounts()
    {
        // 외부 변경을 막기 위해 복사본 반환
        return new Dictionary<int, int>(amounts);
    }

    // 아이템 id를 검증하고 반환한다
    bool TryResolveId(int id, out int resolvedId)
    {
        resolvedId = 0;
        return provider != null && provider.TryResolveId(id, out resolvedId);
    }

    bool CanAdd(int id, int amount)
    {
        if (rules == null)
        {
            return true;
        }

        for (int i = 0; i < rules.Count; i++)
        {
            if (!rules[i].CanAdd(id, amount, out _))
            {
                return false;
            }
        }

        return true;
    }

    bool CanRemove(int id, int amount)
    {
        if (rules == null)
        {
            return true;
        }

        for (int i = 0; i < rules.Count; i++)
        {
            if (!rules[i].CanRemove(id, amount, out _))
            {
                return false;
            }
        }

        return true;
    }

    bool CanSetAmount(int id, int amount)
    {
        if (rules == null)
        {
            return true;
        }

        for (int i = 0; i < rules.Count; i++)
        {
            if (!rules[i].CanSetAmount(id, amount, out _))
            {
                return false;
            }
        }

        return true;
    }

    // 수량 변경 이벤트를 발행한다
    void EmitChanged(int id, int previous, int delta, int current, string reason)
    {
        AmountChanged?.Invoke(new ItemEventPayload(id, previous, delta, current, reason));
    }
}
