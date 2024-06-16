using System.Collections;
using TMPro;
using UI.Manager;
using UnityEngine;

namespace UI.QteResult
{
    public class QteResultView : UiView
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        private Coroutine _runningCoroutine;

        public void DisplayPopupResult(string result, float time)
        {
            _resultText.text = result;

            if (_runningCoroutine != null)
                StopCoroutine(_runningCoroutine);
            
            _runningCoroutine = StartCoroutine(ShowTextWithTime(time));
        }

        public void DisplayText(string text)
        {
            _resultText.text = text;
        }

        private IEnumerator ShowTextWithTime(float sec)
        {
            while (sec >= 0)
            {
                yield return null;
                sec -= Time.deltaTime;
            }

            _resultText.text = "";
        }
    }
}