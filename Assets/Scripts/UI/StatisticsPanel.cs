using UnityEngine;
using TMPro;

namespace Vampire
{
    /// <summary>
    /// Panel hiển thị thống kê tổng quát của người chơi
    /// Có thể sử dụng trong main menu hoặc settings
    /// </summary>
    public class StatisticsPanel : MonoBehaviour
    {
        [Header("Score Statistics")]
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI totalCoinsText;

        [Header("Combat Statistics")]
        [SerializeField] private TextMeshProUGUI totalEnemiesKilledText;
        [SerializeField] private TextMeshProUGUI totalDamageDealtText;

        [Header("Time Statistics")]
        [SerializeField] private TextMeshProUGUI bestSurvivalTimeText;
        [SerializeField] private TextMeshProUGUI totalPlaytimeText;
        [SerializeField] private TextMeshProUGUI totalGamesPlayedText;

        [Header("Calculated Statistics")]
        [SerializeField] private TextMeshProUGUI averageEnemiesPerGameText;
        [SerializeField] private TextMeshProUGUI averagePlaytimePerGameText;

        void Start()
        {
            UpdateAllStats();

            // Đăng ký events để tự động cập nhật
            PlayerDataManager.OnCoinsChanged += OnDataChanged;
            PlayerDataManager.OnHighScoreChanged += OnDataChanged;
        }

        void OnDestroy()
        {
            // Hủy đăng ký events
            PlayerDataManager.OnCoinsChanged -= OnDataChanged;
            PlayerDataManager.OnHighScoreChanged -= OnDataChanged;
        }

        private void OnDataChanged(int newValue)
        {
            UpdateAllStats();
        }

        /// <summary>
        /// Cập nhật tất cả thống kê
        /// </summary>
        public void UpdateAllStats()
        {
            PlayerDataManager playerData = PlayerDataManager.Instance;

            // Score statistics
            if (highScoreText != null)
                highScoreText.text = playerData.GetHighScore().ToString();

            if (totalCoinsText != null)
                totalCoinsText.text = playerData.GetCoins().ToString();

            // Combat statistics
            if (totalEnemiesKilledText != null)
                totalEnemiesKilledText.text = playerData.GetTotalEnemiesKilled().ToString();

            if (totalDamageDealtText != null)
                totalDamageDealtText.text = playerData.GetTotalDamageDealt().ToString("F0");

            // Time statistics
            if (bestSurvivalTimeText != null)
                bestSurvivalTimeText.text = playerData.FormatTime(playerData.GetBestSurvivalTime());

            if (totalPlaytimeText != null)
                totalPlaytimeText.text = FormatLongTime(playerData.GetTotalPlaytime());

            if (totalGamesPlayedText != null)
                totalGamesPlayedText.text = playerData.GetTotalGamesPlayed().ToString();

            // Calculated statistics
            UpdateCalculatedStats();
        }

        /// <summary>
        /// Cập nhật các thống kê được tính toán
        /// </summary>
        private void UpdateCalculatedStats()
        {
            PlayerDataManager playerData = PlayerDataManager.Instance;
            int totalGames = playerData.GetTotalGamesPlayed();

            if (totalGames > 0)
            {
                // Average enemies per game
                if (averageEnemiesPerGameText != null)
                {
                    float avgEnemies = (float)playerData.GetTotalEnemiesKilled() / totalGames;
                    averageEnemiesPerGameText.text = avgEnemies.ToString("F1");
                }

                // Average playtime per game
                if (averagePlaytimePerGameText != null)
                {
                    float avgTime = playerData.GetTotalPlaytime() / totalGames;
                    averagePlaytimePerGameText.text = playerData.FormatTime(avgTime);
                }
            }
            else
            {
                // Nếu chưa chơi game nào
                if (averageEnemiesPerGameText != null)
                    averageEnemiesPerGameText.text = "0";

                if (averagePlaytimePerGameText != null)
                    averagePlaytimePerGameText.text = "00:00";
            }
        }

        /// <summary>
        /// Format thời gian dài (có thể nhiều giờ) thành string dễ đọc
        /// </summary>
        private string FormatLongTime(float seconds)
        {
            int hours = Mathf.FloorToInt(seconds / 3600);
            int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
            int secs = Mathf.FloorToInt(seconds % 60);

            if (hours > 0)
                return string.Format("{0:0}h {1:00}m {2:00}s", hours, minutes, secs);
            else
                return string.Format("{0:00}m {1:00}s", minutes, secs);
        }

        /// <summary>
        /// Reset tất cả thống kê (để test hoặc reset game)
        /// </summary>
        public void ResetAllStats()
        {
            if (Application.isEditor)
            {
                // Chỉ cho phép reset trong editor để tránh việc vô tình xóa dữ liệu
                PlayerDataManager.Instance.ResetAllData();
                UpdateAllStats();
                Debug.Log("All player statistics have been reset!");
            }
            else
            {
                Debug.LogWarning("Reset stats is only allowed in editor mode!");
            }
        }

        /// <summary>
        /// Force refresh all stats (public method cho button v.v.)
        /// </summary>
        public void RefreshStats()
        {
            UpdateAllStats();
        }
    }
}
