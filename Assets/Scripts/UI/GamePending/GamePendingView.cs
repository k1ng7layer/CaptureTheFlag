using TMPro;
using UI.Manager;
using UnityEngine;

namespace UI.GamePending
{
    public class GamePendingView : UiView
    {
        [SerializeField] private TextMeshProUGUI _joinInfo;

        public void SetPendingInfo(string text)
        {
            _joinInfo.text = text;
        }
    }
}