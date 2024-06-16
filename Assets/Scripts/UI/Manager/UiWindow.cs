using System;
using System.Collections.Generic;

namespace UI.Manager
{
    public abstract class UiWindow : IUiWindow
    {
        public List<Type> Controllers { get; } = new();

        public abstract void Setup();
        
        protected void AddController<T>() where T : IUiController
        {
            Controllers.Add(typeof(T));
        } 
    }
}