using System.Collections.Generic;
using UnityEngine;

// InventoryAbility 사용 예시를 보여주는 참고 스크립트
public static class ExampleInventoryAbility
{
    // Example: 아이템 정의 등록 -> 룰 구성 -> InventoryAbility 사용
    public static void RunExample()
    {
        var provider = new StaticItemDefinitionProvider();
        provider.Register(new ItemDefinition(1001, "포션", "체력을 회복한다", maxStack: 99));
        provider.Register(new ItemDefinition(2001, "철 검", "기본 공격력이 증가한다", maxStack: 1));

        var rules = new List<IItemRule>
        {
            new MaxStackRule(provider)
        };

        var hostObject = new GameObject("InventoryHost");
        var host = hostObject.AddComponent<Entity>();
        var inventoryAbility = host.AddAbility<InventoryAbility>();
        inventoryAbility.Configure(provider, rules);

        host.Initialize();
        host.Ready();

        inventoryAbility.AmountChanged += payload =>
        {
            // 변경된 아이템 수량을 수신한다
        };

        inventoryAbility.TryAdd(1001, 3);
        inventoryAbility.TryAdd(2001, 1);
        inventoryAbility.TrySetAmount(1001, 50);
    }

    // 아이템 최대 스택 규칙 예시
    sealed class MaxStackRule : IItemRule
    {
        readonly IItemDefinitionProvider provider;

        // 아이템 정의 제공자를 설정합니다.
        public MaxStackRule(IItemDefinitionProvider provider)
        {
            this.provider = provider;
        }

        // 아이템 추가 가능 여부를 판단합니다.
        public bool CanAdd(int id, int amount, out string reason)
        {
            reason = null;
            return provider != null && provider.TryGetDefinition(id, out _);
        }

        // 아이템 제거 가능 여부를 판단합니다.
        public bool CanRemove(int id, int amount, out string reason)
        {
            reason = null;
            return true;
        }

        // 아이템 수량 설정 가능 여부를 판단합니다.
        public bool CanSetAmount(int id, int amount, out string reason)
        {
            reason = null;
            if (provider == null || !provider.TryGetDefinition(id, out var definition))
            {
                reason = "Item definition not found.";
                return false;
            }

            if (amount > definition.MaxStack)
            {
                reason = "Exceeds MaxStack.";
                return false;
            }

            return true;
        }
    }
}
