using UnityEngine;
using TMPro;

namespace Vampire
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinDisplay : MonoBehaviour
    {
        private TextMeshProUGUI coinText;

        void Start()
        {
            coinText = GetComponent<TextMeshProUGUI>();
            UpdateDisplay();

            // Đăng ký event để tự động cập nhật khi coin thay đổi
            PlayerDataManager.OnCoinsChanged += OnCoinsChanged;
        }

        void OnDestroy()
        {
            // Hủy đăng ký event khi object bị destroy
            PlayerDataManager.OnCoinsChanged -= OnCoinsChanged;
        }

        private void OnCoinsChanged(int newCoinAmount)
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            coinText.text = PlayerDataManager.Instance.GetCoins().ToString();
        }
    }
}
