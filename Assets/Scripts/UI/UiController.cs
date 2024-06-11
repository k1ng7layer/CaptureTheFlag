using UnityEngine;
using Zenject;

namespace UI
{
    public class UiController<T> where T : MonoBehaviour
    {
       [Inject] protected T View { get; }

        // protected T View
        // {
        //     get
        //     {
        //         if (_view == null)
        //             _view = Object.FindObjectOfType<T>();
        //
        //         return _view;
        //     }
        // }
    }
}