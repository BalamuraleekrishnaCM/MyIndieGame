using System;using UnityEngine;
namespace MyIndieGame.Core
{
 public enum ConsentState{Unknown,Granted,Denied}
 public static class ConsentService
 {
  const string AnalyticsKey="mig.consent.analytics";const string AdsKey="mig.consent.ads";
  public static ConsentState Analytics=>Read(AnalyticsKey);public static ConsentState Ads=>Read(AdsKey);public static event Action Changed;
  public static void SetAnalytics(bool value){Write(AnalyticsKey,value);Changed?.Invoke();}
  public static void SetAds(bool value){Write(AdsKey,value);Changed?.Invoke();}
  static ConsentState Read(string key){int v=PlayerPrefs.GetInt(key,-1);return v<0?ConsentState.Unknown:(v==1?ConsentState.Granted:ConsentState.Denied);}
  static void Write(string key,bool value){PlayerPrefs.SetInt(key,value?1:0);PlayerPrefs.Save();}
 }
}