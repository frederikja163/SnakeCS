using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Snake.Common;

namespace Snake.Gl2DRenderer
{
    internal interface IRenderer
    {
        public void Render(in SnakeData data);
    }
    
    public sealed class Gl2DPlatform : IPlatform<SnakeData>, IDisposable
    {
        private readonly bool _initialized = false;

        private readonly IReadOnlyCollection<IRenderer> _renderers;

        private readonly Keyboard _keyboard;
        private readonly Window _window;
        
        public Gl2DPlatform(in ControlList controls, in SnakeData data)
        {
            _initialized = GLFW.Init();
            if (!_initialized)
            {
                return;
            }
            GLLoader.LoadBindings(new GLFWBindingsContext());

            _window = new Window(800, 600, "OpenGL Snake 2D");
            _keyboard = new Keyboard(_window, controls);
            
            _renderers = new IRenderer[] { };
        }

        public void Tick(in SnakeData data)
        {
            foreach (var renderer in _renderers)
            {
                renderer.Render(data);
            }
            GLFW.PollEvents();
        }

        public void Dispose()
        {
            if (!_initialized)
            {
                return;
            }
            GLFW.Terminate();
        }
    }
}