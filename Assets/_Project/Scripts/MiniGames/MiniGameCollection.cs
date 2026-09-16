using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Lightweight playable hub for the first ten MyIndieGame mini-games.
/// Attach to a Canvas in an otherwise empty Unity scene and press Play.
/// Uses only built-in Unity UI so the prototype has no package dependencies.
/// </summary>
public class MiniGameCollection : MonoBehaviour
{
    readonly string[] names = { "Tap Rush", "Color Match", "Stack It", "Dodge Line", "Coin Catch", "Memory Flip", "One Tap Jump", "Ball Sort", "Parking Puzzle", "Merge 2048" };
    readonly string[] tips = { "Tap the moving target", "Choose the requested color", "Stop the block in the stack", "Switch lanes to dodge", "Catch coins, avoid bombs", "Match every pair", "Jump over the obstacle", "Sort balls into tubes", "Clear the parking route", "Merge equal tiles" };
    readonly Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };

    Canvas canvas; Transform root; Text title; Text info; int mode = -1; int score; float time;
    readonly List<GameObject> items = new List<GameObject>();
    readonly List<int> memoryCards = new List<int>();
    readonly List<int> mergeBoard = new List<int>();
    int memoryFirst = -1; bool memoryBusy; int lane = 1; float obstacleLane; float targetX; int stackWidth = 3;
    int[] tube = { 0, 0, 1, 2, 1, 2 }; int[] parking = { 1, 0, 2 };

    void Start() { Setup(); Home(); }

    void Setup()
    {
        canvas = GetComponent<Canvas>();
        if (!canvas) canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        if (!GetComponent<GraphicRaycaster>()) gameObject.AddComponent<GraphicRaycaster>();
        var scaler = GetComponent<CanvasScaler>() ?? gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        var bg = new GameObject("GameUI", typeof(RectTransform), typeof(Image));
        bg.transform.SetParent(canvas.transform, false); root = bg.transform;
        var br = bg.GetComponent<RectTransform>(); br.anchorMin = Vector2.zero; br.anchorMax = Vector2.one; br.offsetMin = br.offsetMax = Vector2.zero;
        bg.GetComponent<Image>().color = new Color(.06f, .07f, .10f, 1);
        title = MakeText("MY INDIE GAME", 52, .9f, .08f, .5f, .94f);
        info = MakeText("", 25, .92f, .06f, .5f, .86f);
    }

    void Clear()
    {
        for (int i = 0; i < items.Count; i++) if (items[i]) Destroy(items[i]);
        items.Clear();
    }

    Text MakeText(string text, int size, float w, float h, float x, float y)
    {
        var g = new GameObject("Text", typeof(RectTransform), typeof(Text)); g.transform.SetParent(root, false);
        var t = g.GetComponent<Text>(); t.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); t.text = text; t.fontSize = size; t.color = Color.white; t.alignment = TextAnchor.MiddleCenter;
        Set(g, w, h, x, y); items.Add(g); return t;
    }

    Button Btn(string text, UnityEngine.Events.UnityAction action, float w = .7f, float h = .09f, float x = .5f, float y = .5f)
    {
        var g = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button)); g.transform.SetParent(root, false);
        var image = g.GetComponent<Image>(); image.color = new Color(.18f, .20f, .25f); Set(g, w, h, x, y);
        g.GetComponent<Button>().onClick.AddListener(action);
        var t = MakeText(text, 27, 1, 1, .5f, .5f); t.transform.SetParent(g.transform, false); Set(t.gameObject, 1, 1, .5f, .5f);
        return g.GetComponent<Button>();
    }

    void Set(GameObject g, float w, float h, float x, float y)
    {
        var r = g.GetComponent<RectTransform>(); r.anchorMin = r.anchorMax = new Vector2(x, y); r.sizeDelta = new Vector2(1080 * w, 1920 * h); r.anchoredPosition = Vector2.zero;
    }

    void Home()
    {
        mode = -1; Clear(); title.text = "MY INDIE GAME"; info.text = "10 quick casual games";
        for (int i = 0; i < names.Length; i++) { int n = i; Btn((i + 1) + "  " + names[i], () => Play(n), .76f, .067f, .5f, .76f - i * .071f); }
    }

    void Play(int n)
    {
        mode = n; score = 0; time = 30; memoryFirst = -1; memoryBusy = false; stackWidth = 3; lane = 1; obstacleLane = 0; targetX = .5f;
        Clear(); title.text = names[n]; AddHome();
        switch (n) { case 0: TapRush(); break; case 1: ColorMatch(); break; case 2: StackIt(); break; case 3: DodgeLine(); break; case 4: CoinCatch(); break; case 5: MemoryFlip(); break; case 6: OneTapJump(); break; case 7: BallSort(); break; case 8: ParkingPuzzle(); break; case 9: Merge2048(); break; }
        UpdateInfo();
    }

    void AddHome() { Btn("HOME", Home, .20f, .055f, .14f, .055f); }
    void UpdateInfo() { if (mode >= 0) info.text = tips[mode] + "  •  Score: " + score + "  •  " + Mathf.CeilToInt(time) + "s"; }
    void End(string message)
    {
        int finished = mode; mode = -1; Clear(); title.text = message; info.text = "Final score: " + score;
        Btn("RETRY", () => Play(finished), .42f, .10f, .5f, .40f); Btn("HOME", Home, .30f, .08f, .5f, .27f);
    }

    void TapRush() { targetX = Random.Range(.18f, .82f); TargetButton(); }
    void TargetButton()
    {
        var b = Btn("TAP!", () => { score++; if (score >= 20) { End("20 TAPS!"); return; } ClearGameOnly(); TapRush(); UpdateInfo(); }, .22f, .13f, targetX, Random.Range(.25f, .72f));
        b.GetComponent<Image>().color = Color.Lerp(Color.white, Color.cyan, .5f);
    }

    void ColorMatch()
    {
        int wanted = Random.Range(0, 4); title.text = "MATCH: " + new[] { "RED", "GREEN", "BLUE", "YELLOW" }[wanted];
        for (int i = 0; i < 4; i++) { int pick = i; var b = Btn(new[] { "RED", "GREEN", "BLUE", "YELLOW" }[i], () => { if (pick == wanted) score += 2; else score = Mathf.Max(0, score - 1); ClearGameOnly(); ColorMatch(); UpdateInfo(); }, .32f, .12f, .30f + (i % 2) * .40f, .62f - (i / 2) * .22f); b.GetComponent<Image>().color = colors[i]; }
    }

    void StackIt()
    {
        MakeText("STACK\n" + new string('■', stackWidth), 55, .65f, .20f, .5f, .64f);
        float moving = Mathf.PingPong(Time.time * (1.2f + score * .04f), .6f) + .2f;
        Btn("STOP BLOCK", () => { float error = Mathf.Abs(moving - .5f); if (error < .10f) { score += 3; stackWidth = Mathf.Max(1, stackWidth); } else if (error < .22f) score++; else score = Mathf.Max(0, score - 1); if (score >= 20) End("STACK MASTER!"); else { ClearGameOnly(); StackIt(); UpdateInfo(); } }, .55f, .12f, .5f, .28f);
    }

    void DodgeLine()
    {
        MakeText("LANE " + (lane + 1) + "\n\n  ■   ■   ■\n      ●", 45, .7f, .40f, .5f, .58f);
        Btn("← LEFT", () => { lane = Mathf.Max(0, lane - 1); DodgeCheck(); }, .35f, .10f, .28f, .23f);
        Btn("RIGHT →", () => { lane = Mathf.Min(2, lane + 1); DodgeCheck(); }, .35f, .10f, .72f, .23f);
    }
    void DodgeCheck() { obstacleLane = Random.Range(0, 3); if (lane == obstacleLane) score = Mathf.Max(0, score - 2); else score += 2; if (score >= 20) End("DODGE CLEAR!"); else { ClearGameOnly(); DodgeLine(); UpdateInfo(); } }

    void CoinCatch()
    {
        MakeText("🪙      💣      🪙\n\n       🧺", 48, .8f, .35f, .5f, .62f);
        Btn("CATCH COIN", () => { score += 2; if (score >= 20) End("COIN COLLECTOR!"); else { ClearGameOnly(); CoinCatch(); UpdateInfo(); } }, .45f, .10f, .30f, .28f);
        Btn("DODGE BOMB", () => { score++; ClearGameOnly(); CoinCatch(); UpdateInfo(); }, .45f, .10f, .70f, .28f);
    }

    void MemoryFlip()
    {
        memoryCards.Clear(); for (int i = 0; i < 4; i++) { memoryCards.Add(i); memoryCards.Add(i); }
        for (int i = 0; i < memoryCards.Count; i++) { int j = Random.Range(i, memoryCards.Count); int t = memoryCards[i]; memoryCards[i] = memoryCards[j]; memoryCards[j] = t; }
        for (int i = 0; i < 8; i++) { int index = i; Btn("?", () => MemoryPick(index), .20f, .12f, .27f + (i % 4) * .15f, .64f - (i / 4) * .20f); }
    }
    void MemoryPick(int index)
    {
        if (memoryBusy) return;
        var buttons = root.GetComponentsInChildren<Button>();
        int cardValue = memoryCards[index];
        if (memoryFirst < 0) { memoryFirst = index; score++; return; }
        if (memoryFirst == index) return;
        if (memoryCards[memoryFirst] == cardValue) { score += 3; memoryFirst = -1; if (score >= 16) End("MEMORY COMPLETE!"); }
        else { score = Mathf.Max(0, score - 1); memoryFirst = -1; }
        UpdateInfo();
    }

    void OneTapJump()
    {
        MakeText("🏃       ■\n────────────", 52, .75f, .28f, .5f, .62f);
        Btn("TAP / JUMP", () => { bool safe = Random.value > .28f; if (safe) score += 2; else score = Mathf.Max(0, score - 2); if (score >= 20) End("PERFECT RUN!"); else { ClearGameOnly(); OneTapJump(); UpdateInfo(); } }, .55f, .12f, .5f, .26f);
    }

    void BallSort()
    {
        MakeText("🔴 🔵     🟢 🔴     🔵 🟢\n  TUBE 1     TUBE 2     TUBE 3", 36, .90f, .25f, .5f, .64f);
        Btn("MOVE BALL", () => { score++; if (score >= 12) End("SORTED!"); else { ClearGameOnly(); BallSort(); UpdateInfo(); } }, .50f, .11f, .5f, .28f);
    }

    void ParkingPuzzle()
    {
        MakeText("🚗 → 🚙 → 🅿\n\nClear the route", 48, .75f, .30f, .5f, .62f);
        Btn("MOVE CAR", () => { int a = parking[0]; parking[0] = parking[1]; parking[1] = a; score++; if (score >= 5) End("PARKED!"); else { ClearGameOnly(); ParkingPuzzle(); UpdateInfo(); } }, .50f, .11f, .5f, .29f);
    }

    void Merge2048()
    {
        if (mergeBoard.Count == 0) { for (int i = 0; i < 16; i++) mergeBoard.Add(i < 2 ? 2 : 0); }
        string board = ""; for (int i = 0; i < 16; i++) board += (mergeBoard[i] == 0 ? "·" : mergeBoard[i].ToString()) + ((i % 4 == 3) ? "\n" : "   ");
        MakeText(board, 40, .75f, .42f, .5f, .60f);
        Btn("MERGE", () => { for (int i = 0; i < 15; i++) if (mergeBoard[i] == mergeBoard[i + 1] && mergeBoard[i] > 0) { mergeBoard[i] *= 2; mergeBoard[i + 1] = 0; score += mergeBoard[i]; break; } if (score >= 64) End("2048 IN PROGRESS!"); else { ClearGameOnly(); Merge2048(); UpdateInfo(); } }, .50f, .11f, .5f, .25f);
    }

    void ClearGameOnly()
    {
        // Preserve title/info and the HOME button by destroying all generated controls except the first three objects.
        for (int i = items.Count - 1; i >= 0; i--)
        {
            GameObject g = items[i];
            if (!g || g == title.gameObject || g == info.gameObject) continue;
            Destroy(g); items.RemoveAt(i);
        }
        // HOME is intentionally recreated by Play; remove any stale HOME as well.
        var buttons = root.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) if (buttons[i] && buttons[i].gameObject != title.gameObject) Destroy(buttons[i].gameObject);
        items.RemoveAll(x => x == null);
        AddHome();
    }

    void Update()
    {
        if (mode < 0) return;
        time -= Time.deltaTime;
        if (time <= 0) { time = 0; End("TIME UP!"); return; }
        UpdateInfo();
    }
}
