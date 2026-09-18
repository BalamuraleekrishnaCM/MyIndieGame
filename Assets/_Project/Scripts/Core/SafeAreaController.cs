using UnityEngine;
namespace MyIndieGame.Core
{
 public sealed class SafeAreaController:MonoBehaviour
 {
  [SerializeField] RectTransform target; Rect lastSafeArea;
  void Awake(){Apply();}
  void OnEnable(){Apply();}
  void Update(){if(Screen.safeArea!=lastSafeArea)Apply();}
  void Apply(){if(target==null)return;lastSafeArea=Screen.safeArea;var area=lastSafeArea;var min=new Vector2(area.x/Screen.width,area.y/Screen.height);var max=new Vector2((area.x+area.width)/Screen.width,(area.y+area.height)/Screen.height);target.anchorMin=min;target.anchorMax=max;target.offsetMin=Vector2.zero;target.offsetMax=Vector2.zero;}
 }
}