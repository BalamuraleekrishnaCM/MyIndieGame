using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public enum StoreProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }

    [Serializable]
    public sealed class StoreProductDefinition
    {
        public string Id;
        public StoreProductType Type;
        public int Coins;
    }

    public static class StoreCatalog
    {
        public const string RemoveAds = "remove_ads";
        public const string CoinsSmall = "coins_small";
        public const string CoinsMedium = "coins_medium";
        public const string CoinsLarge = "coins_large";

        public static readonly StoreProductDefinition[] Products =
        {
            new StoreProductDefinition { Id = CoinsSmall, Type = StoreProductType.Consumable, Coins = 100 },
            new StoreProductDefinition { Id = CoinsMedium, Type = StoreProductType.Consumable, Coins = 550 },
            new StoreProductDefinition { Id = CoinsLarge, Type = StoreProductType.Consumable, Coins = 1200 },
            new StoreProductDefinition { Id = RemoveAds, Type = StoreProductType.NonConsumable, Coins = 0 }
        };

        public static StoreProductDefinition Find(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return null;
            foreach (StoreProductDefinition product in Products)
                if (product.Id == productId) return product;
            return null;
        }

        public static bool GrantCoins(string productId)
        {
            StoreProductDefinition product = Find(productId);
            if (product == null || product.Coins <= 0 || GameSession.Instance == null) return false;
            GameSession.Instance.AddCoins(product.Coins);
            return true;
        }
    }
}
