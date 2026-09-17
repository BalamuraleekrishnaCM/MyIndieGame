using System;
using System.Collections.Generic;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Runtime lookup facade for the canonical mini-game catalog.
    /// Accepts both stable public ids (kebab-case) and legacy catalog ids (snake_case).
    /// </summary>
    public static class GameCatalog
    {
        static readonly Dictionary<string, MiniGameDefinition> Lookup = BuildLookup();

        public static bool TryGet(string gameId, out MiniGameDefinition definition)
        {
            definition = null;
            if (string.IsNullOrWhiteSpace(gameId)) return false;
            return Lookup.TryGetValue(Normalize(gameId), out definition) && definition != null && definition.IsAvailable;
        }

        static Dictionary<string, MiniGameDefinition> BuildLookup()
        {
            var map = new Dictionary<string, MiniGameDefinition>(StringComparer.OrdinalIgnoreCase);
            foreach (var definition in MiniGameCatalog.All)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.Id)) continue;
                map[Normalize(definition.Id)] = definition;
            }
            return map;
        }

        static string Normalize(string value) => value.Trim().Replace('-', '_');
    }
}
