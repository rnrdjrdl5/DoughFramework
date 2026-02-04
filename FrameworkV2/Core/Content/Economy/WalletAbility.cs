using System;
using System.Collections.Generic;

// 통화 잔액을 관리하는 지갑 Ability
public sealed class WalletAbility : Ability
{
    // 수량 변경 이벤트
    public event Action<EconomyEventPayload> AmountChanged;

    ICurrencyDefinitionProvider provider;
    readonly Dictionary<int, int> amounts = new();

    // 통화 정의 공급자를 주입한다
    public void Configure(ICurrencyDefinitionProvider provider)
    {
        this.provider = provider;
    }

    // 통화 수량을 조회한다
    public bool TryGetAmount(int id, out int amount)
    {
        amount = 0;
        if (!TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        return amounts.TryGetValue(resolvedId, out amount);
    }

    // 통화 수량을 없으면 0으로 반환한다
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

    // 통화 수량을 증가시킨다
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

    // 통화 수량을 차감한다
    public bool TrySpend(int id, int amount, string reason = null)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (!TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        amounts.TryGetValue(resolvedId, out var previous);
        if (previous < amount)
        {
            // 잔액 부족
            return false;
        }

        var current = previous - amount;
        amounts[resolvedId] = current;
        EmitChanged(resolvedId, previous, -amount, current, reason);
        return true;
    }

    // 통화 수량을 지정값으로 설정한다
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

    // 통화 id를 검증하고 반환한다
    bool TryResolveId(int id, out int resolvedId)
    {
        resolvedId = 0;
        return provider != null && provider.TryResolveId(id, out resolvedId);
    }

    // 잔액 변경 이벤트를 발행한다
    void EmitChanged(int id, int previous, int delta, int current, string reason)
    {
        AmountChanged?.Invoke(new EconomyEventPayload(id, previous, delta, current, reason));
    }
}
