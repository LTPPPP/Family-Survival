using UnityEngine;
using TMPro;
using UnityEngine.Localization;

namespace Vampire
{
    public class GameOverDialog : DialogBox
    {
        [Header("Main Stats")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI coinsGained;
        [SerializeField] private TextMeshProUGUI enemiesRouted;
        [SerializeField] private TextMeshProUGUI damageDealt;
        [SerializeField] private TextMeshProUGUI damageTaken;

        [Header("Score & Records")]
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI newRecordText; // Hiển thị "NEW RECORD!" nếu có

        [Header("Time Stats")]
        [SerializeField] private TextMeshProUGUI survivalTimeText;
        [SerializeField] private TextMeshProUGUI bestTimeText;
        [SerializeField] private TextMeshProUGUI newTimeRecordText; // Hiển thị "NEW TIME RECORD!" nếu có

        [Header("UI Elements")]
        [SerializeField] private GameObject background;
        [SerializeField] private LocalizedString levelPassedLocalization, levelLostLocalization;
        [SerializeField] private LocalizedString newRecordLocalization, newTimeRecordLocalization;

        [Header("Visual Effects")]
        [SerializeField] private Color newRecordColor = Color.yellow;
        [SerializeField] private Color normalColor = Color.white;

        public void Open(bool levelPassed, StatsManager statsManager)
        {
            Open(levelPassed, statsManager, null);
        }

        public void Open(bool levelPassed, StatsManager statsManager, SessionResult sessionResult)
        {
            // Status text
            statusText.text = levelPassed ? levelPassedLocalization.GetLocalizedString() : levelLostLocalization.GetLocalizedString();

            // Basic stats
            coinsGained.text = "+" + statsManager.CoinsGained;
            enemiesRouted.text = statsManager.MonstersKilled.ToString();
            damageDealt.text = statsManager.DamageDealt.ToString("F0");
            damageTaken.text = statsManager.DamageTaken.ToString("F0");

            // Score and records (chỉ hiển thị nếu có sessionResult)
            if (sessionResult != null)
            {
                // Current score
                if (currentScoreText != null)
                    currentScoreText.text = sessionResult.finalScore.ToString();

                // High score và new record
                if (highScoreText != null)
                    highScoreText.text = sessionResult.highScore.ToString();

                if (newRecordText != null)
                {
                    if (sessionResult.isNewHighScore)
                    {
                        newRecordText.text = newRecordLocalization.GetLocalizedString();
                        newRecordText.color = newRecordColor;
                        newRecordText.gameObject.SetActive(true);

                        // Highlight high score text
                        if (highScoreText != null)
                            highScoreText.color = newRecordColor;
                    }
                    else
                    {
                        newRecordText.gameObject.SetActive(false);
                        if (highScoreText != null)
                            highScoreText.color = normalColor;
                    }
                }

                // Survival time và best time
                if (survivalTimeText != null)
                    survivalTimeText.text = PlayerDataManager.Instance.FormatTime(sessionResult.finalTime);

                if (bestTimeText != null)
                    bestTimeText.text = PlayerDataManager.Instance.FormatTime(sessionResult.bestTime);

                if (newTimeRecordText != null)
                {
                    if (sessionResult.isNewBestTime)
                    {
                        newTimeRecordText.text = newTimeRecordLocalization.GetLocalizedString();
                        newTimeRecordText.color = newRecordColor;
                        newTimeRecordText.gameObject.SetActive(true);

                        // Highlight best time text
                        if (bestTimeText != null)
                            bestTimeText.color = newRecordColor;
                    }
                    else
                    {
                        newTimeRecordText.gameObject.SetActive(false);
                        if (bestTimeText != null)
                            bestTimeText.color = normalColor;
                    }
                }
            }
            else
            {
                // Fallback cho backward compatibility - ẩn các element mới
                if (currentScoreText != null) currentScoreText.gameObject.SetActive(false);
                if (highScoreText != null) highScoreText.gameObject.SetActive(false);
                if (newRecordText != null) newRecordText.gameObject.SetActive(false);
                if (survivalTimeText != null) survivalTimeText.gameObject.SetActive(false);
                if (bestTimeText != null) bestTimeText.gameObject.SetActive(false);
                if (newTimeRecordText != null) newTimeRecordText.gameObject.SetActive(false);
            }

            background.SetActive(true);
            base.Open();
        }

        public override void Close()
        {
            base.Close();
            background.SetActive(false);
        }
    }
}
