using System;using UnityEngine;
namespace MyIndieGame.Core
{
 public static class AccessibilitySettings
 {
  const string ReducedMotionKey="mig.accessibility.reduced_motion"; const string LargeTextKey="mig.accessibility.large_text";
  public static bool ReducedMotion{get=>PlayerPrefs.GetInt(ReducedMotionKey,0)==1;set{PlayerPrefs.SetInt(ReducedMotionKey,value?1:0);PlayerPrefs.Save();Changed?.Invoke();}}
  public static bool LargeText{get=>PlayerPrefs.GetInt(LargeTextKey,0)==1;set{PlayerPrefs.SetInt(LargeTextKey,value?1:0);PlayerPrefs.Save();Changed?.Invoke();}}
  public static event Action Changed;
 }
}