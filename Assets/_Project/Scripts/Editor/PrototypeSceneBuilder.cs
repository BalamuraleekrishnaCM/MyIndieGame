#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public static class PrototypeSceneBuilder
{
    [MenuItem("MyIndieGame/Build 10-Game Prototype")]
    public static void Build()
    {
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        var app=new GameObject("MiniGameApp",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster),typeof(MiniGameCollection));
        var canvas=app.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        var scaler=app.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1080,1920);
        if(!Object.FindFirstObjectByType<EventSystem>())new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));
        Directory.CreateDirectory("Assets/_Project/Scenes");
        EditorSceneManager.SaveScene(scene,"Assets/_Project/Scenes/Prototype.unity");
        AssetDatabase.SaveAssets();
        Selection.activeGameObject=app;
        Debug.Log("MyIndieGame prototype created: Assets/_Project/Scenes/Prototype.unity");
    }
}
#endif
