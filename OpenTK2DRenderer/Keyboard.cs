using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Snake.Common;

namespace Snake.Gl2DRenderer
{
    public sealed class Keyboard
    {
        private readonly IReadOnlyDictionary<Keys, ControlCallback> _callbacks;
        
        public Keyboard(in Window window, in ControlList controls)
        {
            window.KeyCallback += WindowOnKeyCallback;
            Dictionary<Keys, ControlCallback> callbacks = new Dictionary<Keys, ControlCallback>();
            callbacks.Add(Keys.W, controls[Control.TurnUp]);
            callbacks.Add(Keys.A, controls[Control.TurnLeft]);
            callbacks.Add(Keys.S, controls[Control.TurnDown]);
            callbacks.Add(Keys.D, controls[Control.TurnRight]);
            _callbacks = callbacks;
        }

        private void WindowOnKeyCallback(Keys key, InputAction action)
        {
            if (_callbacks.TryGetValue(key, out ControlCallback callback))
            {
                callback.Invoke();
            }
        }
    }
}