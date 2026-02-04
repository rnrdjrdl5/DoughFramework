using System;
// 통화 정의 데이터를 보관한다
public sealed class CurrencyDefinition
{
    public int Id => id;
    public string DisplayName => displayName;

    int id;
    string displayName;

    // 통화 정의를 초기화한다
    public CurrencyDefinition(int id, string displayName)
    {
        if (id <= 0)
        {
            // 잘못된 통화 식별자
            throw new ArgumentException("Currency id must be positive.", nameof(id));
        }

        this.id = id;
        this.displayName = displayName ?? id.ToString();
    }
}
