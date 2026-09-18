using System;
using MyIndieGame.Core;
using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class MemoryFlipGame : MonoBehaviour, IMiniGame
    {
        const string StableGameId = "memory-flip"; const int GameIndex = 5;
        MiniGameSession session; bool initialized;
        public string GameId=>StableGameId; public bool IsRunning=>session!=null&&session.IsActive; public int Score=>session?.Score??0;
        public event Action<int> ScoreChanged; public event Action<int> Completed;
        void Awake()=>Initialize();
        public void Initialize(){if(initialized)return;if(!GameCatalog.TryGet(GameId,out var d))throw new InvalidOperationException($"Missing game definition: {GameId}");session=new MiniGameSession(d,GameIndex);session.ScoreChanged+=OnScore;session.Completed+=OnCompleted;initialized=true;}
        public void StartGame(){Initialize();session.Start();AnalyticsService.RecordGameStarted(GameId);} public void PauseGame()=>session?.Pause(); public void ResumeGame()=>session?.Resume(); public void EndGame()=>session?.End();
        public void MatchPair(bool matched,int points=1){if(IsRunning&&matched&&points>0)session.AddScore(points);} public void CompleteGame(){if(IsRunning)session.Complete();}
        void OnScore(int v)=>ScoreChanged?.Invoke(v);void OnCompleted(int v){AnalyticsService.RecordGameCompleted(GameId,v,0);Completed?.Invoke(v);}void OnDestroy(){if(session==null)return;session.ScoreChanged-=OnScore;session.Completed-=OnCompleted;}
    }
}
