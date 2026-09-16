using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyIndieGame.Core
{
    public sealed class ProgressionHUD : MonoBehaviour
    {
        static readonly string[] GameIds =
        {
            "tap_rush", "color_match", "stack_it", "dodge_line", "coin_catch",
            "memory_flip", "one_tap_jump", "ball_sort", "parking_puzzle", "merge_2048"
        };

        static readonly string[] GameNames =
        {
            "Tap Rush", "Color Match", "Stack It", "Dodge Line", "Coin Catch",
            "Memory Flip", "One Tap Jump", "Ball Sort", "Parking Puzzle", "Merge 2048"
        };

        Canvas canvas;
        Text stats;
        Text best;
        Button pauseButton;
        Text pauseLabel;
        bool paused;
        string currentGameId;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Bootstrap()
        {
            if (FindFirstObjectByType<ProgressionHUD>() != null) return;
            GameObject go = new GameObject("V05ProgressionHUD");
            DontDestroyOnLoad(go);
            go.AddComponent<ProgressionHUD>();
        }

        void Awake()
        {
            Build();
            EnsureSystems();
        }

        void OnDestroy()
        {
            if (paused) Time.timeScale = 1f;
        }

        void Update()
        {
            RefreshGame();
            RefreshStats();
        }

        void EnsureSystems()
        {
            GameObject systems = GameObject.Find("GameSystems");
            if (systems == null)
            {
                systems = new GameObject("GameSystems");
                DontDestroyOnLoad(systems);
            }

            if (GameSession.Instance == null) systems.AddComponent<GameSession>();
            if (GameFeel.Instance == null) systems.AddComponent<GameFeel>();
            if (AudioFeedback.Instance == null) systems.AddComponent<AudioFeedback>();
        }

        void Build()
        {
            GameObject root = new GameObject("HUDCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            root.transform.SetParent(transform, false);
            canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            CanvasScaler scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            stats = MakeText("Coins: 0  •  Games: 0", 24, 0.5f, 0.94f, 0.36f);
            best = MakeText("", 20, 0.5f, 0.895f, 0.36f);

            pauseButton = MakeButton("PAUSE", TogglePause, 0.16f, 0.055f, 0.88f, 0.94f);
            pauseLabel = pauseButton.GetComponentInChildren<Text>();
        }

        Text MakeText(string value, int size, float x, float y, float width)
        {
            GameObject go = new GameObject("HUDText", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(canvas.transform, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(x, y);
            rect.sizeDelta = new Vector2(1080f * width, 70f);
            rect.anchoredPosition = Vector2.zero;

            Text text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = size;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.text = value;
            return text;
        }

        Button MakeButton(string label, UnityEngine.Events.UnityAction action, float width, float height, float x, float y)
        {
            GameObject go = new GameObject("HUDBtn", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(canvas.transform, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(x, y);
            rect.sizeDelta = new Vector2(1080f * width, 1920f * height);
            rect.anchoredPosition = Vector2.zero;
            go.GetComponent<Image>().color = new Color(0.12f, 0.15f, 0.2f, 0.94f);
            Button button = go.GetComponent<Button>();
            button.onClick.AddListener(action);

            Text text = MakeText(label, 22, 0.5f, 0.5f, 1f);
            text.transform.SetParent(go.transform, false);
            RectTransform child = text.rectTransform;
            child.anchorMin = Vector2.zero;
            child.anchorMax = Vector2.one;
            child.offsetMin = child.offsetMax = Vector2.zero;
            return button;
        }

        void RefreshStats()
        {
            if (GameSession.Instance == null) return;
            stats.text = "Coins: " + GameSession.Instance.Coins + "  •  Games: " + GameSession.Instance.TotalGamesPlayed;
        }

        void RefreshGame()
        {
            string detectedId = DetectGameId();
            if (detectedId == currentGameId) return;
            currentGameId = detectedId;
            best.text = string.IsNullOrEmpty(currentGameId)
                ? ""
                : "Best: " + BestScoreStore.Get(currentGameId) + "  •  Difficulty: " + ProgressionService.GetEffectiveDifficulty(currentGameId);
        }

        string DetectGameId()
        {
            Text[] texts = FindObjectsByType<Text>(FindObjectsSortMode.None);
            for (int i = 0; i < texts.Length; i++)
            {
                string value = texts[i].text;
                for (int g = 0; g < GameNames.Length; g++)
                    if (string.Equals(value, GameNames[g], StringComparison.Ordinal)) return GameIds[g];
            }
            return null;
        }

        void TogglePause()
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
            pauseLabel.text = paused ? "RESUME" : "PAUSE";
            if (AudioFeedback.Instance != null) AudioFeedback.Instance.Click();
        }
    }
}
