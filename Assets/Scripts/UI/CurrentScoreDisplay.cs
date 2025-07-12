using UnityEngine;
using TMPro;

namespace Vampire
{
    /// <summary>
    /// Component để hiển thị điểm số hiện tại trong game
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CurrentScoreDisplay : MonoBehaviour
    {
        [Header("Display Settings")]
        [SerializeField] private string prefix = "Score: ";
        [SerializeField] private bool showPrefix = true;
        [SerializeField] private float updateInterval = 0.1f; // Cập nhật mỗi 0.1 giây để không lag

        private TextMeshProUGUI scoreText;
        private float lastUpdateTime;
        private int lastScore = -1;

        void Start()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
            lastUpdateTime = Time.time;
            UpdateDisplay();
        }

        void Update()
        {
            // Chỉ cập nhật theo interval để tối ưu performance
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                UpdateDisplay();
                lastUpdateTime = Time.time;
            }
        }

        /// <summary>
        /// Cập nhật hiển thị điểm số
        /// </summary>
        public void UpdateDisplay()
        {
            int currentScore = GetCurrentScore();

            // Chỉ cập nhật text nếu score thay đổi
            if (currentScore != lastScore)
            {
                if (showPrefix)
                {
                    scoreText.text = prefix + currentScore.ToString();
                }
                else
                {
                    scoreText.text = currentScore.ToString();
                }
                lastScore = currentScore;
            }
        }

        /// <summary>
        /// Lấy điểm số hiện tại từ các nguồn khác nhau
        /// </summary>
        private int GetCurrentScore()
        {
            // Thử lấy từ LevelManager nếu có
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                // Sử dụng reflection để gọi CalculateCurrentScore nếu là public
                // Hoặc có thể tính toán lại ở đây
                StatsManager statsManager = FindObjectOfType<StatsManager>();
                if (statsManager != null)
                {
                    float levelTime = Time.timeSinceLevelLoad;
                    return statsManager.MonstersKilled * 100 +
                           Mathf.FloorToInt(levelTime * 10) +
                           statsManager.CoinsGained * 5;
                }
            }

            return 0;
        }

        /// <summary>
        /// Set prefix text cho display
        /// </summary>
        public void SetPrefix(string newPrefix)
        {
            prefix = newPrefix;
            lastScore = -1; // Force update
            UpdateDisplay();
        }

        /// <summary>
        /// Bật/tắt hiển thị prefix
        /// </summary>
        public void SetShowPrefix(bool show)
        {
            showPrefix = show;
            lastScore = -1; // Force update
            UpdateDisplay();
        }

        /// <summary>
        /// Set update interval
        /// </summary>
        public void SetUpdateInterval(float interval)
        {
            updateInterval = Mathf.Max(0.01f, interval); // Minimum 0.01s
        }
    }
}
