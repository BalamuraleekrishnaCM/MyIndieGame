using System;
using MyIndieGame.Core;

namespace MyIndieGame.Core
{
    /// <summary>Compatibility facade for the production game catalog.</summary>
    public static class GameCatalog
    {
        public static bool TryGet(string gameId, out MiniGameDefinition definition)
        {
            definition = null;
            if (string.IsNullOrWhiteSpace(gameId)) return false;
            foreach (var item in MiniGameCatalog.All)
            {
                if (string.Equals(item.Id, gameId.Trim(), StringComparison.Ordinal))
                {
                    definition = item;
                    return true;
                }
            }
            return false;
        }
    }
}
