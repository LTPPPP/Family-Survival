using UnityEngine;
using TMPro;

namespace Vampire
{
    /// <summary>
    /// Component để hiển thị điểm cao nhất
    /// Tự động cập nhật khi có kỷ lục mới
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HighScoreDisplay : MonoBehaviour
    {
        [Header("Display Settings")]
        [SerializeField] private string prefix = "High Score: ";
        [SerializeField] private bool showPrefix = true;

        private TextMeshProUGUI scoreText;

        void Start()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
            UpdateDisplay();

            // Đăng ký event để tự động cập nhật khi high score thay đổi
            PlayerDataManager.OnHighScoreChanged += OnHighScoreChanged;
        }

        void OnDestroy()
        {
            // Hủy đăng ký event khi object bị destroy
            PlayerDataManager.OnHighScoreChanged -= OnHighScoreChanged;
        }

        private void OnHighScoreChanged(int newHighScore)
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            int highScore = PlayerDataManager.Instance.GetHighScore();
            if (showPrefix)
            {
                scoreText.text = prefix + highScore.ToString();
            }
            else
            {
                scoreText.text = highScore.ToString();
            }
        }

        /// <summary>
        /// Set prefix text cho display
        /// </summary>
        public void SetPrefix(string newPrefix)
        {
            prefix = newPrefix;
            UpdateDisplay();
        }

        /// <summary>
        /// Bật/tắt hiển thị prefix
        /// </summary>
        public void SetShowPrefix(bool show)
        {
            showPrefix = show;
            UpdateDisplay();
        }
    }
}
