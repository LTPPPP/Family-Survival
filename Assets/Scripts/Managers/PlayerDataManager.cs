using UnityEngine;
using System;

namespace Vampire
{
    /// <summary>
    /// Quản lý dữ liệu người chơi bao gồm điểm cao nhất, tổng coin và các thống kê khác
    /// Sử dụng PlayerPrefs để lưu trữ dữ liệu bền vững
    /// </summary>
    public class PlayerDataManager : MonoBehaviour
    {
        private static PlayerDataManager instance;
        public static PlayerDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PlayerDataManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("PlayerDataManager");
                        instance = go.AddComponent<PlayerDataManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return instance;
            }
        }

        [Header("Player Data Keys")]
        private const string COINS_KEY = "Coins";
        private const string HIGH_SCORE_KEY = "HighScore";
        private const string TOTAL_ENEMIES_KILLED_KEY = "TotalEnemiesKilled";
        private const string TOTAL_DAMAGE_DEALT_KEY = "TotalDamageDealt";
        private const string TOTAL_GAMES_PLAYED_KEY = "TotalGamesPlayed";
        private const string TOTAL_PLAYTIME_KEY = "TotalPlaytime";
        private const string BEST_SURVIVAL_TIME_KEY = "BestSurvivalTime";

        [Header("Current Session Data")]
        [SerializeField] private int currentSessionScore = 0;
        [SerializeField] private float currentSessionTime = 0f;

        // Events cho UI updates
        public static event Action<int> OnCoinsChanged;
        public static event Action<int> OnHighScoreChanged;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #region Coin Management
        /// <summary>
        /// Lấy số coin hiện tại
        /// </summary>
        public int GetCoins()
        {
            return PlayerPrefs.GetInt(COINS_KEY, 0);
        }

        /// <summary>
        /// Thêm coin vào tổng số coin
        /// </summary>
        public void AddCoins(int amount)
        {
            int currentCoins = GetCoins();
            int newAmount = currentCoins + amount;
            PlayerPrefs.SetInt(COINS_KEY, newAmount);
            PlayerPrefs.Save();
            OnCoinsChanged?.Invoke(newAmount);
        }

        /// <summary>
        /// Trừ coin (dùng khi mua đồ)
        /// </summary>
        public bool SpendCoins(int amount)
        {
            int currentCoins = GetCoins();
            if (currentCoins >= amount)
            {
                int newAmount = currentCoins - amount;
                PlayerPrefs.SetInt(COINS_KEY, newAmount);
                PlayerPrefs.Save();
                OnCoinsChanged?.Invoke(newAmount);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set coin trực tiếp (chỉ dùng cho debug)
        /// </summary>
        public void SetCoins(int amount)
        {
            PlayerPrefs.SetInt(COINS_KEY, amount);
            PlayerPrefs.Save();
            OnCoinsChanged?.Invoke(amount);
        }
        #endregion

        #region High Score Management
        /// <summary>
        /// Lấy điểm cao nhất
        /// </summary>
        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }

        /// <summary>
        /// Cập nhật điểm cao nhất nếu điểm mới cao hơn
        /// </summary>
        public bool UpdateHighScore(int newScore)
        {
            int currentHighScore = GetHighScore();
            if (newScore > currentHighScore)
            {
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, newScore);
                PlayerPrefs.Save();
                OnHighScoreChanged?.Invoke(newScore);
                return true; // Là kỷ lục mới
            }
            return false; // Không phải kỷ lục mới
        }

        /// <summary>
        /// Set điểm cao nhất trực tiếp (chỉ dùng cho debug)
        /// </summary>
        public void SetHighScore(int score)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(score);
        }
        #endregion

        #region Session Management
        /// <summary>
        /// Bắt đầu session mới
        /// </summary>
        public void StartNewSession()
        {
            currentSessionScore = 0;
            currentSessionTime = 0f;
            IncrementTotalGamesPlayed();
        }

        /// <summary>
        /// Cập nhật điểm trong session hiện tại
        /// </summary>
        public void UpdateSessionScore(int score)
        {
            currentSessionScore = score;
        }

        /// <summary>
        /// Cập nhật thời gian trong session hiện tại
        /// </summary>
        public void UpdateSessionTime(float time)
        {
            currentSessionTime = time;
        }

        /// <summary>
        /// Kết thúc session và lưu dữ liệu
        /// </summary>
        public SessionResult EndSession(StatsManager statsManager)
        {
            SessionResult result = new SessionResult();

            // Cập nhật điểm cao nhất
            result.isNewHighScore = UpdateHighScore(currentSessionScore);
            result.finalScore = currentSessionScore;
            result.highScore = GetHighScore();

            // Cập nhật thời gian sống sót tốt nhất
            result.isNewBestTime = UpdateBestSurvivalTime(currentSessionTime);
            result.finalTime = currentSessionTime;
            result.bestTime = GetBestSurvivalTime();

            // Thêm coin từ session
            AddCoins(statsManager.CoinsGained);
            result.coinsEarned = statsManager.CoinsGained;
            result.totalCoins = GetCoins();

            // Cập nhật thống kê tổng
            IncrementTotalEnemiesKilled(statsManager.MonstersKilled);
            IncrementTotalDamageDealt(statsManager.DamageDealt);
            IncrementTotalPlaytime(currentSessionTime);

            return result;
        }
        #endregion

        #region Statistics
        /// <summary>
        /// Lấy tổng số enemy đã giết
        /// </summary>
        public int GetTotalEnemiesKilled()
        {
            return PlayerPrefs.GetInt(TOTAL_ENEMIES_KILLED_KEY, 0);
        }

        /// <summary>
        /// Thêm vào tổng số enemy đã giết
        /// </summary>
        public void IncrementTotalEnemiesKilled(int amount)
        {
            int current = GetTotalEnemiesKilled();
            PlayerPrefs.SetInt(TOTAL_ENEMIES_KILLED_KEY, current + amount);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Lấy tổng damage đã gây
        /// </summary>
        public float GetTotalDamageDealt()
        {
            return PlayerPrefs.GetFloat(TOTAL_DAMAGE_DEALT_KEY, 0f);
        }

        /// <summary>
        /// Thêm vào tổng damage đã gây
        /// </summary>
        public void IncrementTotalDamageDealt(float amount)
        {
            float current = GetTotalDamageDealt();
            PlayerPrefs.SetFloat(TOTAL_DAMAGE_DEALT_KEY, current + amount);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Lấy tổng số game đã chơi
        /// </summary>
        public int GetTotalGamesPlayed()
        {
            return PlayerPrefs.GetInt(TOTAL_GAMES_PLAYED_KEY, 0);
        }

        /// <summary>
        /// Tăng số game đã chơi
        /// </summary>
        public void IncrementTotalGamesPlayed()
        {
            int current = GetTotalGamesPlayed();
            PlayerPrefs.SetInt(TOTAL_GAMES_PLAYED_KEY, current + 1);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Lấy tổng thời gian chơi (giây)
        /// </summary>
        public float GetTotalPlaytime()
        {
            return PlayerPrefs.GetFloat(TOTAL_PLAYTIME_KEY, 0f);
        }

        /// <summary>
        /// Thêm vào tổng thời gian chơi
        /// </summary>
        public void IncrementTotalPlaytime(float seconds)
        {
            float current = GetTotalPlaytime();
            PlayerPrefs.SetFloat(TOTAL_PLAYTIME_KEY, current + seconds);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Lấy thời gian sống sót tốt nhất
        /// </summary>
        public float GetBestSurvivalTime()
        {
            return PlayerPrefs.GetFloat(BEST_SURVIVAL_TIME_KEY, 0f);
        }

        /// <summary>
        /// Cập nhật thời gian sống sót tốt nhất
        /// </summary>
        public bool UpdateBestSurvivalTime(float time)
        {
            float currentBest = GetBestSurvivalTime();
            if (time > currentBest)
            {
                PlayerPrefs.SetFloat(BEST_SURVIVAL_TIME_KEY, time);
                PlayerPrefs.Save();
                return true; // Là kỷ lục mới
            }
            return false; // Không phải kỷ lục mới
        }
        #endregion

        #region Utility
        /// <summary>
        /// Reset tất cả dữ liệu người chơi
        /// </summary>
        public void ResetAllData()
        {
            PlayerPrefs.DeleteKey(COINS_KEY);
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            PlayerPrefs.DeleteKey(TOTAL_ENEMIES_KILLED_KEY);
            PlayerPrefs.DeleteKey(TOTAL_DAMAGE_DEALT_KEY);
            PlayerPrefs.DeleteKey(TOTAL_GAMES_PLAYED_KEY);
            PlayerPrefs.DeleteKey(TOTAL_PLAYTIME_KEY);
            PlayerPrefs.DeleteKey(BEST_SURVIVAL_TIME_KEY);
            PlayerPrefs.Save();

            OnCoinsChanged?.Invoke(0);
            OnHighScoreChanged?.Invoke(0);
        }

        /// <summary>
        /// Format time thành string dễ đọc
        /// </summary>
        public string FormatTime(float seconds)
        {
            int minutes = Mathf.FloorToInt(seconds / 60);
            int secs = Mathf.FloorToInt(seconds % 60);
            return string.Format("{0:00}:{1:00}", minutes, secs);
        }
        #endregion
    }

    /// <summary>
    /// Kết quả của một session chơi
    /// </summary>
    [System.Serializable]
    public class SessionResult
    {
        public int finalScore;
        public int highScore;
        public bool isNewHighScore;

        public float finalTime;
        public float bestTime;
        public bool isNewBestTime;

        public int coinsEarned;
        public int totalCoins;
    }
}
