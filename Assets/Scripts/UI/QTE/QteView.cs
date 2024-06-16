using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.QTE
{
    public class QteView : UiView, IPointerClickHandler
    {
        [SerializeField] private RectTransform backGround;
        [SerializeField] private Image successZone;
        [SerializeField] private Image slider;

        private float _bgWidth;
        private Rect _successZoneRect;
        
        public event Action Clicked;
         
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }

        public void InitializeSuccessZone(float normalizedStart, float normalizedWidth)
        {
            _bgWidth = backGround.rect.width;
            _successZoneRect = successZone.rectTransform.rect;
            
            successZone.rectTransform.sizeDelta = new Vector2(_bgWidth * normalizedWidth, _successZoneRect.height);
            successZone.rectTransform.anchoredPosition = new Vector2(_bgWidth * normalizedStart, 0f);
        }
        
        public void SetSliderValue(float normalizedOffset)
        {
            var sliderWidth = slider.rectTransform.rect.width;
            var recPosition = _bgWidth * normalizedOffset;
            recPosition = Mathf.Clamp(recPosition, 0f, _bgWidth - sliderWidth);
            
            slider.rectTransform.anchoredPosition = new Vector2(recPosition, 0f);
        }
    }
}