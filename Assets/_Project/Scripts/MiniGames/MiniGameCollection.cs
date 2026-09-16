using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniGameCollection : MonoBehaviour
{
    string[] names={"Tap Rush","Color Match","Stack It","Dodge Line","Coin Catch","Memory Flip","One Tap Jump","Ball Sort","Parking Puzzle","Merge 2048"};
    string[] tips={"Tap the target","Match the color","Build the stack","Dodge obstacles","Catch coins","Find pairs","Tap to jump","Sort colors","Park the car","Merge tiles"};
    Canvas canvas; Transform root; Text title,info; int mode=-1,score; float time; List<GameObject> items=new();
    void Start(){Setup();Home();}
    void Setup(){canvas=GetComponent<Canvas>()??gameObject.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;gameObject.AddComponent<GraphicRaycaster>();var s=GetComponent<CanvasScaler>()??gameObject.AddComponent<CanvasScaler>();s.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;s.referenceResolution=new Vector2(1080,1920);var g=new GameObject("GameUI",typeof(RectTransform),typeof(Image));g.transform.SetParent(canvas.transform,false);root=g.transform;var r=g.GetComponent<RectTransform>();r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;g.GetComponent<Image>().color=new Color(.07f,.08f,.1f,1);title=MakeText("MY INDIE GAME",52,.9f,.08f,.5f,.94f);info=MakeText("",26,.9f,.06f,.5f,.86f);}
    void Clear(){foreach(var x in items)if(x)Destroy(x);items.Clear();}
    Text MakeText(string s,int size,float w,float h,float x,float y){var g=new GameObject("Text",typeof(RectTransform),typeof(Text));g.transform.SetParent(root,false);var t=g.GetComponent<Text>();t.font=Resources.GetBuiltinResource<Font>("Arial.ttf");t.text=s;t.fontSize=size;t.color=Color.white;t.alignment=TextAnchor.MiddleCenter;Set(g,w,h,x,y);items.Add(g);return t;}
    void Set(GameObject g,float w,float h,float x,float y){var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=new Vector2(x,y);r.sizeDelta=new Vector2(1080*w,1920*h);r.anchoredPosition=Vector2.zero;}
    Button Btn(string text,UnityEngine.Events.UnityAction a,float w=.7f,float h=.09f,float x=.5f,float y=.5f){var g=new GameObject("Button",typeof(RectTransform),typeof(Image),typeof(Button));g.transform.SetParent(root,false);g.GetComponent<Image>().color=new Color(.18f,.2f,.25f);Set(g,w,h,x,y);g.GetComponent<Button>().onClick.AddListener(a);var t=MakeText(text,28,1,1,.5f,.5f);t.transform.SetParent(g.transform,false);Set(t.gameObject,1,1,.5f,.5f);return g.GetComponent<Button>();}
    void Home(){mode=-1;Clear();title.text="MY INDIE GAME";info.text="10 quick casual games";for(int i=0;i<10;i++){int n=i;Btn((i+1)+"  "+names[i],()=>Play(n),.76f,.068f,.5f,.75f-i*.071f);}}
    void Play(int n){mode=n;score=0;time=30;Clear();title.text=names[n];info.text=tips[n]+"  •  Score: 0";Btn("HOME",Home,.2f,.055f,.15f,.055f);if(n==0)Tap();if(n==1)Colors();if(n==2)Stack();if(n==3)Dodge();if(n==4)Coins();if(n==5)Memory();if(n==6)Jump();if(n==7)Sort();if(n==8)Park();if(n==9)Merge();}
    void UpdateInfo(){info.text=tips[mode]+"  •  Score: "+score+"  •  "+Mathf.CeilToInt(time)+"s";}
    void Tap(){Target();}void Target(){Btn("TAP!",()=>{score++;Target();UpdateInfo();},.22f,.13f,Random.Range(.2f,.8f),Random.Range(.25f,.72f));}
    void Colors(){int wanted=Random.Range(0,4);string[] c={"RED","GREEN","BLUE","YELLOW"};title.text="MATCH "+c[wanted];for(int i=0;i<4;i++){int x=i;var b=Btn(c[i],()=>{score+=x==wanted?2:-1;Colors();UpdateInfo();},.3f,.12f,.3f+(i%2)*.4f,.6f-(i/2)*.23f);b.GetComponent<Image>().color=new[]{Color.red,Color.green,Color.blue,Color.yellow}[i];}}
    void Stack(){MakeText("▰\n▰▰\n▰▰▰",55,.5f,.3f,.5f,.6f);Btn("TAP TO STACK",()=>{score++;UpdateInfo();},.55f,.12f,.5f,.25f);}
    void Dodge(){MakeText("⬜   🟥   ⬜\n\n🟥     ⬜\n\n⬜   🟥",48,.8f,.4f,.5f,.55f);Btn("DODGE LEFT",()=>{score++;UpdateInfo();},.4f,.1f,.28f,.2f);Btn("DODGE RIGHT",()=>{score++;UpdateInfo();},.4f,.1f,.72f,.2f);}
    void Coins(){MakeText("🪙   💣   🪙\n\n   🪙\n\n   🧺",55,.7f,.4f,.5f,.58f);Btn("CATCH COIN",()=>{score++;UpdateInfo();},.45f,.1f,.5f,.25f);Btn("HIT BOMB",()=>{score=Mathf.Max(0,score-2);UpdateInfo();},.35f,.08f,.5f,.14f);}
    void Memory(){for(int i=0;i<8;i++){int n=i;Btn("?",()=>{score++;UpdateInfo();if(score>=8)title.text="ALL PAIRS FOUND!";},.2f,.12f,.27f+(i%4)*.15f,.62f-(i/4)*.2f);}}
    void Jump(){MakeText("🏃     🟥    🟥\n────────────",48,.8f,.3f,.5f,.6f);Btn("TAP / JUMP",()=>{score++;UpdateInfo();},.55f,.12f,.5f,.25f);}
    void Sort(){MakeText("🔴🔵     🟢🔴     🔵🟢\n  🧪          🧪          🧪",42,.9f,.3f,.5f,.58f);Btn("SORT MOVE",()=>{score++;UpdateInfo();},.5f,.11f,.5f,.25f);}
    void Park(){MakeText("🚗  →  🅿",65,.7f,.2f,.5f,.65f);Btn("MOVE CAR",()=>{score++;UpdateInfo();if(score>=5)title.text="PARKED!";},.5f,.12f,.5f,.3f);Btn("RESET",()=>{score=0;UpdateInfo();},.3f,.08f,.5f,.18f);}
    void Merge(){MakeText("2   4   8   16\n4   8   16  •\n2   4   8   16\n•   2   4   8",38,.7f,.4f,.5f,.58f);Btn("MERGE / MOVE",()=>{score++;UpdateInfo();},.55f,.12f,.5f,.22f);}
    void LateUpdate(){if(mode<0)return;time-=Time.deltaTime;if(time<=0){time=0;title.text="TIME UP!";Clear();Btn("RETRY",()=>Play(mode),.45f,.1f,.5f,.4f);Btn("HOME",Home,.3f,.08f,.5f,.27f);mode=-1;}else UpdateInfo();}
}