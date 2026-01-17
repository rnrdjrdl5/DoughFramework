using System.Collections.Generic;
using System.Linq;

namespace Tables
{
    public partial class Pack
    {
        // Add your custom logic here.

        public List<Card> GetCards()
        {
            return Card.Table
                .Where(kv => kv.Key == Key)
                .Select(kv => kv.Value)
                .ToList();
        }
    }
}
