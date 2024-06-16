using TMPro;
using UnityEngine;

namespace UI.GameResult
{
    public class GameResultView : UiView
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        public void ShowResult(string text)
        {
            _resultText.text = text;
        }
    }
}