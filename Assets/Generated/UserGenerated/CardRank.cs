using System.Linq;

namespace Tables
{
    public partial class CardRank
    {
        // Add your custom logic here.

        public static CardRank MaxCardRankData => Table.OrderByDescending(kv => kv.Value.level).First().Value;

        public static CardRank GetCardRankFromRank(int cardLevel)
        {
            return Table.All(kv => kv.Value.level != cardLevel) ? null : Table.First(kv => kv.Value.level == cardLevel).Value;
        }
    }
}
