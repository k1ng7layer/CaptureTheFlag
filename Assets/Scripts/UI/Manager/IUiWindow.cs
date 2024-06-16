using System;
using System.Collections.Generic;

namespace UI.Manager
{
    public interface IUiWindow
    {
        List<Type> Controllers { get; }
        void Setup();
    }
}