using System;

// 아이템 정의 데이터를 보관한다
public sealed class ItemDefinition
{
    public int Id => id;
    public string DisplayName => displayName;
    public string Description => description;
    public int MaxStack => maxStack;

    int id;
    string displayName;
    string description;
    int maxStack;

    // 아이템 정의를 초기화한다
    public ItemDefinition(
        int id,
        string displayName,
        string description,
        int maxStack = 1)
    {
        if (id <= 0)
        {
            // 잘못된 아이템 식별자
            throw new ArgumentException("Item id must be positive.", nameof(id));
        }

        if (maxStack <= 0)
        {
            // 잘못된 최대 스택 수량
            throw new ArgumentException("MaxStack must be positive.", nameof(maxStack));
        }

        this.id = id;
        this.displayName = displayName ?? id.ToString();
        this.description = description ?? string.Empty;
        this.maxStack = maxStack;
    }
}
