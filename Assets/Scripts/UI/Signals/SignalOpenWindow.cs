using System;

namespace UI.Signals
{
    public readonly struct SignalOpenWindow
    {
        public readonly Type WindowType;
        
        public SignalOpenWindow(Type viewType)
        {
            WindowType = viewType;
        }
    }
}