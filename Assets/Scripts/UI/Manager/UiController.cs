using Zenject;

namespace UI.Manager
{
    public abstract class UiController<T> : IUiController where T : UiView
    {
        [Inject] protected T View { get; }

        public void Open()
       {
           View.Show();
           OnShow();
       }

        public void Close()
       {
           OnHide();
           View.Hide();
       }

        protected virtual void OnShow(){}
        protected virtual void OnHide(){}
    }
}