using TMPro;
using UnityEngine;

namespace UI.QteResult
{
    public class QteResultView : UiView
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        public void DisplayResult(string result)
        {
            _resultText.text = result;
        }
    }
}