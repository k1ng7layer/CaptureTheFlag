using UnityEngine;

namespace UI
{
    public class UiController<T> where T : MonoBehaviour
    {
        private T _view;

        protected T View
        {
            get
            {
                if (_view == null)
                    _view = Object.FindObjectOfType<T>();

                return _view;
            }
        }
    }
}