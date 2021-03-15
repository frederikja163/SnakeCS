using System;
using System.Collections.Generic;

namespace Snake.Common
{
    public delegate void ControlCallback();

    public enum Control
    {
        TurnUp,
        TurnRight,
        TurnDown,
        TurnLeft,
        Close,
    }

    public sealed class ControlList
    {
        private readonly Dictionary<Control, ControlCallback> _controlItems = new Dictionary<Control, ControlCallback>();

        public ControlList()
        {
            
        }

        public ControlCallback this[Control control]
        {
            get
            {
                if (_controlItems.TryGetValue(control, out var callback))
                {
                    return callback;
                }
                _controlItems.Add(control, () => { });
                return _controlItems[control];
            }
            set
            {
                if (!_controlItems.TryGetValue(control, out var callback))
                {
                    _controlItems.Add(control, value);
                }
                else
                {
                    _controlItems[control] = (ControlCallback)Delegate.Combine(value, callback);
                }
            }
        }
    }
    
}