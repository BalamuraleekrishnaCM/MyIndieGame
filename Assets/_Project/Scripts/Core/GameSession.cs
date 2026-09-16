using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class GameSession : MonoBehaviour
    {
        public static GameSession Instance { get; private set; }
        public int Coins { get; private set; }
        public int TotalGamesPlayed { get; private set; }
        public event Action Changed;

        const string CoinsKey = "mig.coins";
        const string PlayedKey = "mig.played";

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Coins = PlayerPrefs.GetInt(CoinsKey, 0);
            TotalGamesPlayed = PlayerPrefs.GetInt(PlayedKey, 0);
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            Coins += amount;
            Save();
        }

        public void RecordGamePlayed()
        {
            TotalGamesPlayed++;
            Save();
        }

        void Save()
        {
            PlayerPrefs.SetInt(CoinsKey, Coins);
            PlayerPrefs.SetInt(PlayedKey, TotalGamesPlayed);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }
    }
}
