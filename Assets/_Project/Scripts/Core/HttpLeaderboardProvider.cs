using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MyIndieGame.Core
{
    [Serializable]
    sealed class LeaderboardResponse
    {
        public string gameId;
        public string seasonId;
        public int limit;
        public int offset;
        public int totalCount;
        public LeaderboardEntry[] entries;
    }

    [Serializable]
    sealed class LeaderboardSubmitRequest
    {
        public string gameId;
        public string seasonId;
        public int score;
        public string idempotencyKey;
    }

    public sealed class HttpLeaderboardProvider : ILeaderboardProvider
    {
        readonly string baseUrl;
        readonly Func<string> accessTokenProvider;

        public HttpLeaderboardProvider(string baseUrl, Func<string> accessTokenProvider = null)
        {
            this.baseUrl = (baseUrl ?? string.Empty).TrimEnd('/');
            this.accessTokenProvider = accessTokenProvider;
        }

        public bool IsAvailable => !string.IsNullOrWhiteSpace(baseUrl);

        public void GetTop(string gameId, string seasonId, int limit, Action<LeaderboardPage, string> completed)
        {
            Get($"/leaderboards/{UnityWebRequest.EscapeURL(gameId)}?seasonId={UnityWebRequest.EscapeURL(seasonId ?? string.Empty)}&limit={Mathf.Clamp(limit, 1, 100)}", completed);
        }

        public void GetAroundPlayer(string gameId, string seasonId, int limit, Action<LeaderboardPage, string> completed)
        {
            Get($"/leaderboards/{UnityWebRequest.EscapeURL(gameId)}/around-me?seasonId={UnityWebRequest.EscapeURL(seasonId ?? string.Empty)}&limit={Mathf.Clamp(limit, 1, 100)}", completed);
        }

        public void SubmitScore(string gameId, string seasonId, int score, string idempotencyKey, Action<bool, string> completed)
        {
            var requestBody = new LeaderboardSubmitRequest
            {
                gameId = gameId,
                seasonId = seasonId ?? string.Empty,
                score = score,
                idempotencyKey = idempotencyKey
            };
            string json = JsonUtility.ToJson(requestBody);
            var request = new UnityWebRequest(baseUrl + "/leaderboards/submit", UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");
            ApplyAuth(request);
            var operation = request.SendWebRequest();
            operation.completed += _ =>
            {
                using (request)
                {
                    if (request.result == UnityWebRequest.Result.Success && request.responseCode >= 200 && request.responseCode < 300)
                    {
                        completed?.Invoke(true, string.Empty);
                    }
                    else
                    {
                        completed?.Invoke(false, FormatError(request));
                    }
                }
            };
        }

        void Get(string path, Action<LeaderboardPage, string> completed)
        {
            if (!IsAvailable)
            {
                completed?.Invoke(null, "Leaderboard endpoint is not configured.");
                return;
            }

            var request = UnityWebRequest.Get(baseUrl + path);
            ApplyAuth(request);
            var operation = request.SendWebRequest();
            operation.completed += _ =>
            {
                using (request)
                {
                    if (request.result != UnityWebRequest.Result.Success || request.responseCode < 200 || request.responseCode >= 300)
                    {
                        completed?.Invoke(null, FormatError(request));
                        return;
                    }

                    try
                    {
                        var response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
                        if (response == null)
                        {
                            completed?.Invoke(null, "Leaderboard response was empty.");
                            return;
                        }

                        completed?.Invoke(new LeaderboardPage
                        {
                            GameId = response.gameId,
                            SeasonId = response.seasonId,
                            Limit = response.limit,
                            Offset = response.offset,
                            TotalCount = Mathf.Max(0, response.totalCount),
                            Entries = response.entries == null
                                ? new System.Collections.Generic.List<LeaderboardEntry>()
                                : new System.Collections.Generic.List<LeaderboardEntry>(response.entries)
                        }, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        completed?.Invoke(null, "Invalid leaderboard response: " + ex.Message);
                    }
                }
            };
        }

        void ApplyAuth(UnityWebRequest request)
        {
            string token = accessTokenProvider?.Invoke();
            if (!string.IsNullOrWhiteSpace(token))
                request.SetRequestHeader("Authorization", "Bearer " + token);
        }

        static string FormatError(UnityWebRequest request)
        {
            if (request.responseCode > 0)
                return "Leaderboard request failed (HTTP " + request.responseCode + ").";
            return string.IsNullOrWhiteSpace(request.error) ? "Leaderboard request failed." : request.error;
        }
    }
}
