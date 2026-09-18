using UnityEngine;
namespace MyIndieGame.Core
{
 public static class PerformanceBudget
 {
  public const int TargetFrameRate=60; public const int MaxTrackedGameObjects=2500;
  public static void Apply(){Application.targetFrameRate=TargetFrameRate;QualitySettings.vSyncCount=0;}
  public static bool IsMobile()=>Application.platform==RuntimePlatform.Android||Application.platform==RuntimePlatform.IPhonePlayer;
 }
}