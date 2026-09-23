using System;using System.Collections.Generic;
namespace MyIndieGame.Core
{
 public static class RemoteConfigService
 {
  static readonly Dictionary<string,string> Values=new Dictionary<string,string>(); public static bool IsInitialized{get;private set;} public static event Action Changed;
  public static void Initialize(IDictionary<string,string> values=null){Values.Clear();if(values!=null)foreach(var p in values)Values[p.Key]=p.Value;IsInitialized=true;Changed?.Invoke();}
  public static string GetString(string key,string fallback=""){return key!=null&&Values.TryGetValue(key,out var v)?v:fallback;}
  public static int GetInt(string key,int fallback=0){return int.TryParse(GetString(key,null),out var v)?v:fallback;}
  public static float GetFloat(string key,float fallback=0f){return float.TryParse(GetString(key,null),out var v)?v:fallback;}
  public static bool GetBool(string key,bool fallback=false){var s=GetString(key,null);return bool.TryParse(s,out var v)?v:fallback;}
 }
}