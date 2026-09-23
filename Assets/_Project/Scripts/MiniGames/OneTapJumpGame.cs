using System;
using MyIndieGame.Core;
using UnityEngine;
namespace MyIndieGame.MiniGames
{
 public sealed class OneTapJumpGame:MonoBehaviour,IMiniGame
 {
  const string Id="one-tap-jump"; const int Index=6; MiniGameSession session; bool initialized;
  public string GameId=>Id; public bool IsRunning=>session!=null&&session.IsActive; public int Score=>session?.Score??0;
  public event Action<int> ScoreChanged; public event Action<int> Completed;
  void Awake()=>Initialize();
  public void Initialize(){if(initialized)return;if(!GameCatalog.TryGet(GameId,out var d))throw new InvalidOperationException($"Missing game definition: {GameId}");session=new MiniGameSession(d,Index);session.ScoreChanged+=v=>ScoreChanged?.Invoke(v);session.Completed+=v=>Completed?.Invoke(v);initialized=true;}
  public void StartGame(){Initialize();session.Start();AnalyticsService.RecordGameStarted(GameId);} public void PauseGame()=>session?.Pause(); public void ResumeGame()=>session?.Resume(); public void EndGame()=>session?.End();
  public void ClearObstacle(int points=1){if(IsRunning&&points>0)session.AddScore(points);} public void CompleteGame(){if(IsRunning)session.Complete();}
 }
}