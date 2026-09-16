using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameCollection : MonoBehaviour
{
    readonly string[] names = { "Tap Rush", "Color Match", "Stack It", "Dodge Line", "Coin Catch", "Memory Flip", "One Tap Jump", "Ball Sort", "Parking Puzzle", "Merge 2048" };
    readonly string[] tips = { "Tap the target", "Match the color", "Build the stack", "Survive the run", "Catch the coins", "Find every pair", "Tap to jump", "Sort every tube", "Clear the parking path", "Merge the tiles" };
    readonly Color[] palette = { Color.red, Color.green, Color.blue, Color.yellow };

    Canvas canvas;
    Transform root;
    Text title;
    Text info;
    int mode = -1;
    int score;
    float time;
    float tick;
    bool gameOver;
    readonly List<GameObject> items = new List<GameObject>();
    readonly List<GameObject> gameplayItems = new List<GameObject>();

    // Tap Rush
    Button tapTarget;

    // Stack It
    RectTransform stackBlock;
    float stackX;
    float stackDirection = 1f;
    readonly List<RectTransform> stackedBlocks = new List<RectTransform>();

    // Dodge Line / Coin Catch / Jump
    RectTransform player;
    readonly List<RectTransform> fallingObjects = new List<RectTransform>();
    readonly List<int> fallingTypes = new List<int>();
    float playerX = 0.5f;
    bool jumping;
    float jumpVelocity;

    // Memory Flip
    readonly List<Button> memoryCards = new List<Button>();
    readonly List<int> memoryValues = new List<int>();
    int firstMemory = -1;
    bool memoryBusy;
    int matchedMemory;

    // Ball Sort
    readonly List<List<int>> tubes = new List<List<int>>();
    int selectedTube = -1;

    // Parking Puzzle
    readonly int[] parking = new int[20];
    int carCell;
    int parkingMoves;

    // Merge 2048
    readonly int[] mergeGrid = new int[16];
    readonly List<Button> mergeButtons = new List<Button>();

    void Start()
    {
        Setup();
        Home();
    }

    void Update()
    {
        if (mode < 0 || gameOver) return;

        time -= Time.deltaTime;
        tick += Time.deltaTime;
        UpdateGame(Time.deltaTime);

        if (time <= 0f)
        {
            time = 0f;
            FinishGame();
        }
        else
        {
            UpdateInfo();
        }
    }

    void Setup()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null) canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        if (GetComponent<GraphicRaycaster>() == null) gameObject.AddComponent<GraphicRaycaster>();

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler == null) scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        GameObject background = new GameObject("GameUI", typeof(RectTransform), typeof(Image));
        background.transform.SetParent(canvas.transform, false);
        root = background.transform;
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = bgRect.offsetMax = Vector2.zero;
        background.GetComponent<Image>().color = new Color(0.055f, 0.065f, 0.09f, 1f);

        title = MakeText("MY INDIE GAME", 52, 0.9f, 0.08f, 0.5f, 0.94f);
        info = MakeText("", 25, 0.92f, 0.06f, 0.5f, 0.86f);
    }

    void ClearGameplay()
    {
        for (int i = gameplayItems.Count - 1; i >= 0; i--)
            if (gameplayItems[i] != null) Destroy(gameplayItems[i]);
        gameplayItems.Clear();
        fallingObjects.Clear();
        fallingTypes.Clear();
        memoryCards.Clear();
        mergeButtons.Clear();
        stackedBlocks.Clear();
        stackBlock = null;
        tapTarget = null;
        player = null;
    }

    void ClearAll()
    {
        for (int i = items.Count - 1; i >= 0; i--)
            if (items[i] != null) Destroy(items[i]);
        items.Clear();
        gameplayItems.Clear();
        fallingObjects.Clear();
        fallingTypes.Clear();
        memoryCards.Clear();
        mergeButtons.Clear();
        stackedBlocks.Clear();
        stackBlock = null;
        tapTarget = null;
        player = null;
    }

    Text MakeText(string text, int size, float w, float h, float x, float y)
    {
        GameObject g = new GameObject("Text", typeof(RectTransform), typeof(Text));
        g.transform.SetParent(root, false);
        Text t = g.GetComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = text;
        t.fontSize = size;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleCenter;
        SetRect(g, w, h, x, y);
        items.Add(g);
        return t;
    }

    GameObject MakePanel(string name, float w, float h, float x, float y, Color color, bool gameplay = true)
    {
        GameObject g = new GameObject(name, typeof(RectTransform), typeof(Image));
        g.transform.SetParent(root, false);
        g.GetComponent<Image>().color = color;
        SetRect(g, w, h, x, y);
        if (gameplay) gameplayItems.Add(g);
        else items.Add(g);
        return g;
    }

    void SetRect(GameObject g, float w, float h, float x, float y)
    {
        RectTransform r = g.GetComponent<RectTransform>();
        r.anchorMin = r.anchorMax = new Vector2(x, y);
        r.sizeDelta = new Vector2(1080f * w, 1920f * h);
        r.anchoredPosition = Vector2.zero;
    }

    Button Btn(string text, UnityEngine.Events.UnityAction action, float w = 0.7f, float h = 0.09f, float x = 0.5f, float y = 0.5f, bool gameplay = true)
    {
        GameObject g = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button));
        g.transform.SetParent(root, false);
        Image image = g.GetComponent<Image>();
        image.color = new Color(0.16f, 0.19f, 0.25f, 1f);
        SetRect(g, w, h, x, y);
        g.GetComponent<Button>().onClick.AddListener(action);

        Text t = MakeChildText(g.transform, text, 28);
        t.color = Color.white;
        if (gameplay) gameplayItems.Add(g); else items.Add(g);
        return g.GetComponent<Button>();
    }

    Text MakeChildText(Transform parent, string text, int size)
    {
        GameObject g = new GameObject("Label", typeof(RectTransform), typeof(Text));
        g.transform.SetParent(parent, false);
        Text t = g.GetComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.text = text;
        t.fontSize = size;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        RectTransform r = g.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.offsetMin = r.offsetMax = Vector2.zero;
        return t;
    }

    void Home()
    {
        StopAllCoroutines();
        mode = -1;
        gameOver = false;
        ClearAll();
        title.text = "MY INDIE GAME";
        info.text = "10 playable mini-games";
        for (int i = 0; i < names.Length; i++)
        {
            int n = i;
            Btn((i + 1) + "  " + names[i], () => Play(n), 0.78f, 0.068f, 0.5f, 0.75f - i * 0.071f, false);
        }
    }

    void Play(int n)
    {
        StopAllCoroutines();
        mode = n;
        score = 0;
        time = n == 5 || n == 7 || n == 8 || n == 9 ? 60f : 30f;
        tick = 0f;
        gameOver = false;
        firstMemory = -1;
        matchedMemory = 0;
        selectedTube = -1;
        parkingMoves = 0;
        ClearAll();
        title.text = names[n];
        info.text = tips[n];
        Btn("HOME", Home, 0.2f, 0.055f, 0.13f, 0.055f, false);

        switch (n)
        {
            case 0: StartTapRush(); break;
            case 1: StartColorMatch(); break;
            case 2: StartStack(); break;
            case 3: StartDodge(); break;
            case 4: StartCoinCatch(); break;
            case 5: StartMemory(); break;
            case 6: StartJump(); break;
            case 7: StartBallSort(); break;
            case 8: StartParking(); break;
            case 9: StartMerge(); break;
        }
    }

    void UpdateInfo()
    {
        if (mode >= 0 && !gameOver)
            info.text = tips[mode] + "  •  Score: " + score + "  •  " + Mathf.CeilToInt(time) + "s";
    }

    void FinishGame()
    {
        if (gameOver) return;
        gameOver = true;
        int finishedMode = mode;
        ClearGameplay();
        title.text = "TIME UP!";
        info.text = "Final score: " + score;
        Btn("RETRY", () => Play(finishedMode), 0.45f, 0.1f, 0.5f, 0.42f, false);
        Btn("HOME", Home, 0.3f, 0.08f, 0.5f, 0.29f, false);
    }

    void WinGame(string message)
    {
        if (gameOver) return;
        gameOver = true;
        int finishedMode = mode;
        ClearGameplay();
        title.text = message;
        info.text = "Score: " + score;
        Btn("PLAY AGAIN", () => Play(finishedMode), 0.45f, 0.1f, 0.5f, 0.42f, false);
        Btn("HOME", Home, 0.3f, 0.08f, 0.5f, 0.29f, false);
    }

    void UpdateGame(float dt)
    {
        if (mode == 0) return;
        if (mode == 1) return;
        if (mode == 2) UpdateStack(dt);
        if (mode == 3) UpdateDodge(dt);
        if (mode == 4) UpdateCoinCatch(dt);
        if (mode == 6) UpdateJump(dt);
    }

    // ---------------------------------------------------------------------
    // 1. Tap Rush
    // ---------------------------------------------------------------------
    void StartTapRush()
    {
        tapTarget = CreateTarget("TAP", Random.Range(0.2f, 0.8f), Random.Range(0.27f, 0.7f), 0.24f, 0.13f);
    }

    Button CreateTarget(string text, float x, float y, float w, float h)
    {
        Button b = Btn(text, () =>
        {
            if (gameOver) return;
            score++;
            MoveTapTarget();
        }, w, h, x, y);
        b.GetComponent<Image>().color = new Color(0.1f, 0.65f, 0.95f, 1f);
        return b;
    }

    void MoveTapTarget()
    {
        if (tapTarget == null) return;
        RectTransform r = tapTarget.GetComponent<RectTransform>();
        r.anchorMin = r.anchorMax = new Vector2(Random.Range(0.16f, 0.84f), Random.Range(0.25f, 0.72f));
        r.anchoredPosition = Vector2.zero;
        tapTarget.transform.SetAsLastSibling();
    }

    // ---------------------------------------------------------------------
    // 2. Color Match
    // ---------------------------------------------------------------------
    void StartColorMatch()
    {
        CreateColorRound();
    }

    void CreateColorRound()
    {
        int wanted = Random.Range(0, 4);
        title.text = "MATCH " + new[] { "RED", "GREEN", "BLUE", "YELLOW" }[wanted];
        for (int i = 0; i < 4; i++)
        {
            int choice = i;
            Button b = Btn(new[] { "RED", "GREEN", "BLUE", "YELLOW" }[i], () =>
            {
                if (gameOver) return;
                if (choice == wanted) score += 2;
                else score = Mathf.Max(0, score - 1);
                ClearGameplay();
                CreateColorRound();
                UpdateInfo();
            }, 0.3f, 0.12f, 0.3f + (i % 2) * 0.4f, 0.62f - (i / 2) * 0.23f);
            b.GetComponent<Image>().color = palette[i];
        }
    }

    // ---------------------------------------------------------------------
    // 3. Stack It
    // ---------------------------------------------------------------------
    void StartStack()
    {
        RectTransform baseBlock = MakeGameplayRect("Base", 0.22f, 0.07f, 0.5f, 0.18f, new Color(0.2f, 0.8f, 0.95f, 1f));
        stackedBlocks.Add(baseBlock);
        stackX = 0.5f;
        CreateMovingStackBlock();
        Btn("TAP TO STACK", DropStackBlock, 0.55f, 0.11f, 0.5f, 0.27f);
    }

    void CreateMovingStackBlock()
    {
        stackX = 0.3f;
        stackDirection = 1f;
        float y = 0.18f + stackedBlocks.Count * 0.055f;
        stackBlock = MakeGameplayRect("MovingBlock", 0.22f, 0.055f, stackX, y, new Color(0.95f, 0.55f, 0.15f, 1f));
    }

    void UpdateStack(float dt)
    {
        if (stackBlock == null) return;
        stackX += stackDirection * dt * 0.5f;
        if (stackX > 0.8f) { stackX = 0.8f; stackDirection = -1f; }
        if (stackX < 0.2f) { stackX = 0.2f; stackDirection = 1f; }
        stackBlock.anchorMin = stackBlock.anchorMax = new Vector2(stackX, stackBlock.anchorMin.y);
        stackBlock.anchoredPosition = Vector2.zero;
    }

    void DropStackBlock()
    {
        if (stackBlock == null) return;
        RectTransform last = stackedBlocks[stackedBlocks.Count - 1];
        float overlap = Mathf.Abs(stackX - last.anchorMin.x);
        if (overlap > 0.19f)
        {
            FinishEarly("STACK MISSED");
            return;
        }
        score++;
        stackedBlocks.Add(stackBlock);
        if (stackedBlocks.Count >= 12)
        {
            WinGame("TOWER COMPLETE!");
            return;
        }
        CreateMovingStackBlock();
        UpdateInfo();
    }

    // ---------------------------------------------------------------------
    // 4. Dodge Line
    // ---------------------------------------------------------------------
    void StartDodge()
    {
        playerX = 0.5f;
        player = MakeGameplayRect("Player", 0.08f, 0.08f, playerX, 0.23f, Color.white);
        Btn("◀", () => MovePlayer(-0.08f), 0.28f, 0.1f, 0.3f, 0.1f);
        Btn("▶", () => MovePlayer(0.08f), 0.28f, 0.1f, 0.7f, 0.1f);
    }

    void MovePlayer(float amount)
    {
        playerX = Mathf.Clamp01(playerX + amount);
        if (player != null)
        {
            player.anchorMin = player.anchorMax = new Vector2(playerX, 0.23f);
            player.anchoredPosition = Vector2.zero;
        }
    }

    void UpdateDodge(float dt)
    {
        if (player == null) return;
        if (Random.value < dt * 1.8f) SpawnFallingObject(0.8f, 0.72f, Color.red, 0);
        for (int i = fallingObjects.Count - 1; i >= 0; i--)
        {
            RectTransform obj = fallingObjects[i];
            if (obj == null) { RemoveFallingAt(i); continue; }
            float y = obj.anchorMin.y - dt * 0.34f;
            obj.anchorMin = obj.anchorMax = new Vector2(obj.anchorMin.x, y);
            obj.anchoredPosition = Vector2.zero;
            if (RectOverlap(player, obj, 0.04f))
            {
                FinishEarly("HIT!");
                return;
            }
            if (y < 0.18f) { score++; RemoveFallingAt(i); }
        }
    }

    // ---------------------------------------------------------------------
    // 5. Coin Catch
    // ---------------------------------------------------------------------
    void StartCoinCatch()
    {
        playerX = 0.5f;
        player = MakeGameplayRect("Catcher", 0.14f, 0.08f, playerX, 0.19f, new Color(0.2f, 0.85f, 0.55f, 1f));
        Btn("◀", () => MovePlayer(-0.08f), 0.28f, 0.1f, 0.3f, 0.1f);
        Btn("▶", () => MovePlayer(0.08f), 0.28f, 0.1f, 0.7f, 0.1f);
    }

    void UpdateCoinCatch(float dt)
    {
        if (player == null) return;
        if (Random.value < dt * 2.4f)
        {
            bool bomb = Random.value < 0.18f;
            SpawnFallingObject(Random.Range(0.15f, 0.85f), 0.78f, bomb ? Color.red : Color.yellow, bomb ? 1 : 0);
        }
        for (int i = fallingObjects.Count - 1; i >= 0; i--)
        {
            RectTransform obj = fallingObjects[i];
            if (obj == null) { RemoveFallingAt(i); continue; }
            float y = obj.anchorMin.y - dt * 0.3f;
            obj.anchorMin = obj.anchorMax = new Vector2(obj.anchorMin.x, y);
            obj.anchoredPosition = Vector2.zero;
            if (RectOverlap(player, obj, 0.05f))
            {
                if (fallingTypes[i] == 1) { FinishEarly("BOMB!"); return; }
                score++;
                RemoveFallingAt(i);
                continue;
            }
            if (y < 0.12f) RemoveFallingAt(i);
        }
    }

    void SpawnFallingObject(float x, float y, Color color, int type)
    {
        RectTransform obj = MakeGameplayRect(type == 1 ? "Bomb" : "Coin", 0.065f, 0.065f, x, y, color);
        fallingObjects.Add(obj);
        fallingTypes.Add(type);
    }

    void RemoveFallingAt(int index)
    {
        if (index < 0 || index >= fallingObjects.Count) return;
        if (fallingObjects[index] != null) Destroy(fallingObjects[index].gameObject);
        fallingObjects.RemoveAt(index);
        fallingTypes.RemoveAt(index);
    }

    // ---------------------------------------------------------------------
    // 6. Memory Flip
    // ---------------------------------------------------------------------
    void StartMemory()
    {
        List<int> values = new List<int>();
        for (int i = 0; i < 8; i++) { values.Add(i); values.Add(i); }
        for (int i = values.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = values[i]; values[i] = values[j]; values[j] = tmp;
        }
        memoryValues.AddRange(values);
        for (int i = 0; i < 16; i++)
        {
            int index = i;
            Button b = Btn("?", () => FlipMemory(index), 0.18f, 0.12f, 0.23f + (i % 4) * 0.18f, 0.68f - (i / 4) * 0.15f);
            memoryCards.Add(b);
        }
    }

    void FlipMemory(int index)
    {
        if (memoryBusy || index < 0 || index >= memoryCards.Count) return;
        Text label = memoryCards[index].GetComponentInChildren<Text>();
        if (label == null || label.text != "?") return;
        label.text = (memoryValues[index] + 1).ToString();
        if (firstMemory < 0) { firstMemory = index; return; }
        if (memoryValues[firstMemory] == memoryValues[index])
        {
            memoryCards[firstMemory].interactable = false;
            memoryCards[index].interactable = false;
            memoryCards[firstMemory].GetComponent<Image>().color = new Color(0.2f, 0.65f, 0.35f, 1f);
            memoryCards[index].GetComponent<Image>().color = new Color(0.2f, 0.65f, 0.35f, 1f);
            score += 2;
            matchedMemory += 2;
            firstMemory = -1;
            if (matchedMemory == 16) WinGame("ALL PAIRS FOUND!");
        }
        else
        {
            score = Mathf.Max(0, score - 1);
            int old = firstMemory;
            firstMemory = -1;
            StartCoroutine(HideMemory(old, index));
        }
    }

    IEnumerator HideMemory(int a, int b)
    {
        memoryBusy = true;
        yield return new WaitForSeconds(0.55f);
        if (a >= 0 && a < memoryCards.Count) memoryCards[a].GetComponentInChildren<Text>().text = "?";
        if (b >= 0 && b < memoryCards.Count) memoryCards[b].GetComponentInChildren<Text>().text = "?";
        memoryBusy = false;
    }

    // ---------------------------------------------------------------------
    // 7. One Tap Jump
    // ---------------------------------------------------------------------
    void StartJump()
    {
        playerX = 0.25f;
        player = MakeGameplayRect("Runner", 0.08f, 0.08f, playerX, 0.2f, Color.white);
        Btn("TAP / JUMP", JumpNow, 0.55f, 0.11f, 0.5f, 0.1f);
    }

    void JumpNow()
    {
        if (!jumping) { jumping = true; jumpVelocity = 1.7f; }
    }

    void UpdateJump(float dt)
    {
        if (player == null) return;
        float ground = 0.2f;
        if (jumping)
        {
            float y = player.anchorMin.y + jumpVelocity * dt;
            jumpVelocity -= 4.2f * dt;
            if (y <= ground) { y = ground; jumping = false; }
            player.anchorMin = player.anchorMax = new Vector2(0.25f, y);
            player.anchoredPosition = Vector2.zero;
        }
        if (Random.value < dt * 1.35f) SpawnFallingObject(0.85f, ground, Color.red, 0);
        for (int i = fallingObjects.Count - 1; i >= 0; i--)
        {
            RectTransform obj = fallingObjects[i];
            if (obj == null) { RemoveFallingAt(i); continue; }
            float x = obj.anchorMin.x - dt * 0.42f;
            obj.anchorMin = obj.anchorMax = new Vector2(x, obj.anchorMin.y);
            obj.anchoredPosition = Vector2.zero;
            if (RectOverlap(player, obj, 0.04f)) { FinishEarly("CRASH!"); return; }
            if (x < 0.08f) { score++; RemoveFallingAt(i); }
        }
    }

    // ---------------------------------------------------------------------
    // 8. Ball Sort
    // ---------------------------------------------------------------------
    void StartBallSort()
    {
        tubes.Clear();
        for (int i = 0; i < 4; i++) tubes.Add(new List<int>());
        List<int> balls = new List<int>();
        for (int c = 0; c < 4; c++) for (int k = 0; k < 4; k++) balls.Add(c);
        for (int i = balls.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = balls[i]; balls[i] = balls[j]; balls[j] = tmp;
        }
        for (int i = 0; i < 16; i++) tubes[i / 4].Add(balls[i]);
        tubes.Add(new List<int>());
        tubes.Add(new List<int>());
        DrawTubes();
    }

    void DrawTubes()
    {
        ClearGameplay();
        for (int t = 0; t < tubes.Count; t++)
        {
            int tube = t;
            float x = 0.16f + (t % 3) * 0.34f;
            float y = t < 3 ? 0.63f : 0.27f;
            Button b = Btn("Tube " + (t + 1) + "\n" + TubeText(t), () => SelectTube(tube), 0.27f, 0.24f, x, y);
            if (selectedTube == t) b.GetComponent<Image>().color = new Color(0.1f, 0.55f, 0.85f, 1f);
        }
        info.text = "Sort each color into one tube  •  Moves: " + score;
    }

    string TubeText(int index)
    {
        string s = "";
        for (int i = tubes[index].Count - 1; i >= 0; i--) s += "●" + (tubes[index][i] + 1) + " ";
        return string.IsNullOrEmpty(s) ? "EMPTY" : s;
    }

    void SelectTube(int index)
    {
        if (selectedTube < 0) { selectedTube = index; DrawTubes(); return; }
        if (selectedTube == index) { selectedTube = -1; DrawTubes(); return; }
        List<int> from = tubes[selectedTube];
        List<int> to = tubes[index];
        if (from.Count == 0 || to.Count >= 4) { selectedTube = -1; DrawTubes(); return; }
        int color = from[from.Count - 1];
        if (to.Count > 0 && to[to.Count - 1] != color) { selectedTube = -1; DrawTubes(); return; }
        to.Add(from[from.Count - 1]);
        from.RemoveAt(from.Count - 1);
        score++;
        selectedTube = -1;
        if (IsSorted()) WinGame("SORT COMPLETE!"); else DrawTubes();
    }

    bool IsSorted()
    {
        for (int i = 0; i < tubes.Count; i++)
        {
            if (tubes[i].Count == 0) continue;
            if (tubes[i].Count != 4) return false;
            int c = tubes[i][0];
            for (int j = 1; j < 4; j++) if (tubes[i][j] != c) return false;
        }
        return true;
    }

    // ---------------------------------------------------------------------
    // 9. Parking Puzzle
    // ---------------------------------------------------------------------
    void StartParking()
    {
        for (int i = 0; i < parking.Length; i++) parking[i] = 0;
        // 0 empty, 1 wall, 2 car, 3 goal.
        int[] walls = { 1, 2, 6, 7, 11, 13, 14, 18 };
        foreach (int w in walls) parking[w] = 1;
        carCell = 15;
        parking[carCell] = 2;
        parking[4] = 3;
        DrawParking();
    }

    void DrawParking()
    {
        ClearGameplay();
        for (int i = 0; i < 20; i++)
        {
            int cell = i;
            float x = 0.2f + (i % 5) * 0.15f;
            float y = 0.66f - (i / 5) * 0.14f;
            string label = parking[i] == 1 ? "■" : parking[i] == 2 ? "CAR" : parking[i] == 3 ? "P" : "";
            Button b = Btn(label, () => MoveCar(cell), 0.12f, 0.11f, x, y);
            if (parking[i] == 1) b.GetComponent<Image>().color = new Color(0.32f, 0.34f, 0.4f, 1f);
            if (parking[i] == 2) b.GetComponent<Image>().color = new Color(0.1f, 0.7f, 0.95f, 1f);
            if (parking[i] == 3) b.GetComponent<Image>().color = new Color(0.2f, 0.65f, 0.35f, 1f);
        }
        info.text = "Tap an adjacent empty space to move the car  •  Moves: " + parkingMoves;
    }

    void MoveCar(int target)
    {
        int rowA = carCell / 5, colA = carCell % 5;
        int rowB = target / 5, colB = target % 5;
        if (Mathf.Abs(rowA - rowB) + Mathf.Abs(colA - colB) != 1) return;
        if (parking[target] == 1) return;
        parking[carCell] = 0;
        carCell = target;
        parking[carCell] = 2;
        parkingMoves++;
        score = Mathf.Max(0, 100 - parkingMoves);
        if (carCell == 4) WinGame("PARKED!"); else DrawParking();
    }

    // ---------------------------------------------------------------------
    // 10. Merge 2048
    // ---------------------------------------------------------------------
    void StartMerge()
    {
        for (int i = 0; i < 16; i++) mergeGrid[i] = 0;
        AddMergeTile();
        AddMergeTile();
        DrawMerge();
    }

    void DrawMerge()
    {
        ClearGameplay();
        mergeButtons.Clear();
        for (int i = 0; i < 16; i++)
        {
            int index = i;
            float x = 0.23f + (i % 4) * 0.18f;
            float y = 0.67f - (i / 4) * 0.13f;
            Button b = Btn(mergeGrid[i] == 0 ? "" : mergeGrid[i].ToString(), () => { }, 0.16f, 0.11f, x, y);
            b.interactable = false;
            mergeButtons.Add(b);
            if (mergeGrid[index] > 0) b.GetComponent<Image>().color = TileColor(mergeGrid[index]);
        }
        Btn("◀", () => MoveMerge(-1), 0.2f, 0.09f, 0.28f, 0.12f);
        Btn("▶", () => MoveMerge(1), 0.2f, 0.09f, 0.72f, 0.12f);
        Btn("▲", () => MoveMerge(-4), 0.2f, 0.09f, 0.5f, 0.2f);
        Btn("▼", () => MoveMerge(4), 0.2f, 0.09f, 0.5f, 0.08f);
        info.text = "Merge matching tiles  •  Best target: 2048  •  Score: " + score;
    }

    Color TileColor(int value)
    {
        int p = Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(value, 2f)) - 1, 0, 7);
        Color[] c = { new Color(0.3f,0.5f,0.9f), new Color(0.3f,0.7f,0.5f), new Color(0.9f,0.6f,0.2f), new Color(0.8f,0.3f,0.4f), new Color(0.6f,0.4f,0.8f), new Color(0.2f,0.7f,0.8f), new Color(0.9f,0.4f,0.2f), new Color(0.9f,0.8f,0.2f) };
        return c[p];
    }

    void MoveMerge(int direction)
    {
        bool vertical = Mathf.Abs(direction) == 4;
        bool reverse = direction > 0;
        bool moved = false;
        if (!vertical)
        {
            for (int row = 0; row < 4; row++)
            {
                int[] line = new int[4];
                for (int k = 0; k < 4; k++) line[k] = mergeGrid[row * 4 + (reverse ? 3 - k : k)];
                int[] result = MergeLine(line);
                for (int k = 0; k < 4; k++)
                {
                    int idx = row * 4 + (reverse ? 3 - k : k);
                    if (mergeGrid[idx] != result[k]) moved = true;
                    mergeGrid[idx] = result[k];
                }
            }
        }
        else
        {
            for (int col = 0; col < 4; col++)
            {
                int[] line = new int[4];
                for (int k = 0; k < 4; k++) line[k] = mergeGrid[(reverse ? 3 - k : k) * 4 + col];
                int[] result = MergeLine(line);
                for (int k = 0; k < 4; k++)
                {
                    int idx = (reverse ? 3 - k : k) * 4 + col;
                    if (mergeGrid[idx] != result[k]) moved = true;
                    mergeGrid[idx] = result[k];
                }
            }
        }
        if (!moved) return;
        AddMergeTile();
        score += 1;
        for (int i = 0; i < 16; i++) if (mergeGrid[i] >= 2048) { WinGame("2048 REACHED!"); return; }
        if (!HasMergeMoves()) { WinGame("NO MOVES LEFT"); return; }
        DrawMerge();
    }

    int[] MergeLine(int[] input)
    {
        List<int> values = new List<int>();
        for (int i = 0; i < input.Length; i++) if (input[i] != 0) values.Add(input[i]);
        List<int> result = new List<int>();
        for (int i = 0; i < values.Count; i++)
        {
            if (i + 1 < values.Count && values[i] == values[i + 1])
            {
                result.Add(values[i] * 2);
                score += values[i] * 2;
                i++;
            }
            else result.Add(values[i]);
        }
        while (result.Count < 4) result.Add(0);
        return result.ToArray();
    }

    void AddMergeTile()
    {
        List<int> empty = new List<int>();
        for (int i = 0; i < 16; i++) if (mergeGrid[i] == 0) empty.Add(i);
        if (empty.Count == 0) return;
        int cell = empty[Random.Range(0, empty.Count)];
        mergeGrid[cell] = Random.value < 0.9f ? 2 : 4;
    }

    bool HasMergeMoves()
    {
        for (int i = 0; i < 16; i++)
        {
            if (mergeGrid[i] == 0) return true;
            int r = i / 4, c = i % 4;
            if (c < 3 && mergeGrid[i] == mergeGrid[i + 1]) return true;
            if (r < 3 && mergeGrid[i] == mergeGrid[i + 4]) return true;
        }
        return false;
    }

    // ---------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------
    RectTransform MakeGameplayRect(string name, float w, float h, float x, float y, Color color)
    {
        GameObject g = MakePanel(name, w, h, x, y, color, true);
        return g.GetComponent<RectTransform>();
    }

    bool RectOverlap(RectTransform a, RectTransform b, float padding)
    {
        if (a == null || b == null) return false;
        return Mathf.Abs(a.anchorMin.x - b.anchorMin.x) < (a.rect.width / 1080f + b.rect.width / 1080f) * 0.5f + padding &&
               Mathf.Abs(a.anchorMin.y - b.anchorMin.y) < (a.rect.height / 1920f + b.rect.height / 1920f) * 0.5f + padding;
    }

    void FinishEarly(string message)
    {
        if (gameOver) return;
        gameOver = true;
        int finishedMode = mode;
        ClearGameplay();
        title.text = message;
        info.text = "Score: " + score;
        Btn("RETRY", () => Play(finishedMode), 0.45f, 0.1f, 0.5f, 0.42f, false);
        Btn("HOME", Home, 0.3f, 0.08f, 0.5f, 0.29f, false);
    }
}
