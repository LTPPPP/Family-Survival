using UnityEngine;
using TMPro;

namespace Vampire
{
    /// <summary>
    /// Debug panel để test các tính năng PlayerDataManager
    /// Chỉ hoạt động trong editor hoặc development build
    /// </summary>
    public class PlayerDataDebugPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField coinsInputField;
        [SerializeField] private TMP_InputField highScoreInputField;
        [SerializeField] private TextMeshProUGUI currentStatsText;

        [Header("Test Values")]
        [SerializeField] private int testCoinsAmount = 100;
        [SerializeField] private int testScore = 5000;
        [SerializeField] private int testEnemies = 50;

        void Start()
        {
            // Chỉ hiển thị trong editor hoặc development build
            if (!Debug.isDebugBuild && !Application.isEditor)
            {
                gameObject.SetActive(false);
                return;
            }

            UpdateStatsDisplay();
        }

        /// <summary>
        /// Cập nhật hiển thị thống kê hiện tại
        /// </summary>
        public void UpdateStatsDisplay()
        {
            if (currentStatsText != null)
            {
                PlayerDataManager playerData = PlayerDataManager.Instance;
                string stats = $"Current Stats:\n" +
                              $"Coins: {playerData.GetCoins()}\n" +
                              $"High Score: {playerData.GetHighScore()}\n" +
                              $"Total Enemies Killed: {playerData.GetTotalEnemiesKilled()}\n" +
                              $"Total Damage Dealt: {playerData.GetTotalDamageDealt():F0}\n" +
                              $"Total Games Played: {playerData.GetTotalGamesPlayed()}\n" +
                              $"Best Survival Time: {playerData.FormatTime(playerData.GetBestSurvivalTime())}\n" +
                              $"Total Playtime: {FormatLongTime(playerData.GetTotalPlaytime())}";

                currentStatsText.text = stats;
            }
        }

        /// <summary>
        /// Thêm coin từ input field
        /// </summary>
        public void AddCoinsFromInput()
        {
            if (coinsInputField != null && int.TryParse(coinsInputField.text, out int amount))
            {
                PlayerDataManager.Instance.AddCoins(amount);
                UpdateStatsDisplay();
                Debug.Log($"Added {amount} coins");
            }
        }

        /// <summary>
        /// Set high score từ input field
        /// </summary>
        public void SetHighScoreFromInput()
        {
            if (highScoreInputField != null && int.TryParse(highScoreInputField.text, out int score))
            {
                PlayerDataManager.Instance.SetHighScore(score);
                UpdateStatsDisplay();
                Debug.Log($"Set high score to {score}");
            }
        }

        /// <summary>
        /// Thêm coin test
        /// </summary>
        public void AddTestCoins()
        {
            PlayerDataManager.Instance.AddCoins(testCoinsAmount);
            UpdateStatsDisplay();
            Debug.Log($"Added {testCoinsAmount} test coins");
        }

        /// <summary>
        /// Test update high score
        /// </summary>
        public void TestHighScore()
        {
            bool isNewRecord = PlayerDataManager.Instance.UpdateHighScore(testScore);
            UpdateStatsDisplay();
            Debug.Log($"Tested score {testScore}, New record: {isNewRecord}");
        }

        /// <summary>
        /// Test thêm enemies killed
        /// </summary>
        public void TestAddEnemies()
        {
            PlayerDataManager.Instance.IncrementTotalEnemiesKilled(testEnemies);
            UpdateStatsDisplay();
            Debug.Log($"Added {testEnemies} enemies to total");
        }

        /// <summary>
        /// Test thêm playtime
        /// </summary>
        public void TestAddPlaytime()
        {
            PlayerDataManager.Instance.IncrementTotalPlaytime(300f); // 5 minutes
            UpdateStatsDisplay();
            Debug.Log("Added 5 minutes to total playtime");
        }

        /// <summary>
        /// Test session complete
        /// </summary>
        public void TestCompleteSession()
        {
            // Tạo mock StatsManager
            GameObject mockObj = new GameObject("MockStatsManager");
            StatsManager mockStats = mockObj.AddComponent<StatsManager>();

            // Set mock values using reflection hoặc public setters nếu có
            // Ở đây ta sẽ simulate bằng cách gọi các method public
            for (int i = 0; i < testEnemies; i++)
            {
                mockStats.IncrementMonstersKilled();
            }
            for (int i = 0; i < testCoinsAmount; i++)
            {
                mockStats.IncreaseCoinsGained(1);
            }
            mockStats.IncreaseDamageDealt(1000f);

            // Test session
            PlayerDataManager.Instance.StartNewSession();
            PlayerDataManager.Instance.UpdateSessionScore(testScore);
            PlayerDataManager.Instance.UpdateSessionTime(300f); // 5 minutes
            SessionResult result = PlayerDataManager.Instance.EndSession(mockStats);

            Debug.Log($"Session completed - New high score: {result.isNewHighScore}, New time record: {result.isNewBestTime}");

            // Cleanup
            DestroyImmediate(mockObj);
            UpdateStatsDisplay();
        }

        /// <summary>
        /// Reset tất cả dữ liệu
        /// </summary>
        public void ResetAllData()
        {
            PlayerDataManager.Instance.ResetAllData();
            UpdateStatsDisplay();
            Debug.Log("All player data reset!");
        }

        /// <summary>
        /// Format thời gian dài
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
        /// Save PlayerPrefs manually
        /// </summary>
        public void SaveData()
        {
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs saved manually");
        }
    }
}
