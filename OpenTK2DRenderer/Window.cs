using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace Snake.Gl2DRenderer
{
    public sealed class Window : IDisposable
    {
        private readonly unsafe GlfwWindow* _handle;
        public event Action<Keys, InputAction> KeyCallback;

        public Window(int width, int height, string title)
        {
            unsafe
            {
                _handle = GLFW.CreateWindow(width, height, title, null, null);
                GLFW.SetKeyCallback(_handle, OnKey);
            }
        }

        private unsafe void OnKey(GlfwWindow* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
        {
            KeyCallback?.Invoke(key, action);
        }

        public void SwapBuffers()
        {
            unsafe
            {
                GLFW.SwapBuffers(_handle);
            }
        }

        public void Dispose()
        {
            unsafe
            {
                GLFW.DestroyWindow(_handle);
            }
        }
    }
}