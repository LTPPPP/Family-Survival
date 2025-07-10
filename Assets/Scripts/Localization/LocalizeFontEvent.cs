using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Vampire
{
    [Serializable]
    public class UnityEventTMPFont : UnityEvent<TMP_FontAsset> { }

    [AddComponentMenu("Localization/Asset/Localize Font Event")]
    public class LocalizeFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTmpFont, UnityEventTMPFont>
    {
        [SerializeField] private TextMeshProUGUI[] _tmpUITexts;
        [SerializeField] private TextMeshPro[] _tmpTexts;

        protected override void UpdateAsset(TMP_FontAsset font)
        {
            base.UpdateAsset(font);

            foreach (var tmp in _tmpUITexts)
            {
                if (tmp != null)
                    tmp.font = font;
                else
                    Debug.LogWarning($"[LocalizeFontEvent] One of the _tmpUITexts is null in {gameObject.name}");
            }

            foreach (var tmp in _tmpTexts)
            {
                if (tmp != null)
                    tmp.font = font;
                else
                    Debug.LogWarning($"[LocalizeFontEvent] One of the _tmpTexts is null in {gameObject.name}");
            }
        }
    }
}