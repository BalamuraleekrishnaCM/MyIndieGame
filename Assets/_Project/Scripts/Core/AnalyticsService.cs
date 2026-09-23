using System;
using System.Collections.Generic;
namespace MyIndieGame.Core
{
 public static class AnalyticsService
 {
  public static event Action<string,IReadOnlyDictionary<string,object>> EventRecorded;
  public static void Record(string eventName,Dictionary<string,object> parameters=null)
  {
   if(string.IsNullOrWhiteSpace(eventName)||ConsentService.Analytics!=ConsentState.Granted)return;
   EventRecorded?.Invoke(eventName,parameters??new Dictionary<string,object>());
  }
  public static void RecordGameStarted(string gameId)=>Record("game_started",new Dictionary<string,object>{{"game_id",gameId}});
  public static void RecordGameCompleted(string gameId,int score,int reward)=>Record("game_completed",new Dictionary<string,object>{{"game_id",gameId},{"score",score},{"reward",reward}});
  public static void RecordRewardClaimed(string rewardId)=>Record("reward_claimed",new Dictionary<string,object>{{"reward_id",rewardId}});
 }
}