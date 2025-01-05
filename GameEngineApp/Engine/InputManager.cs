using GameEngineApp.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class InputMapping
    {
        public Dictionary<Keys, InputAction> InputMap { get; set; } = new()
        {
            { Keys.Up, InputAction.Up },
            { Keys.Left, InputAction.Left },
            { Keys.Right, InputAction.Right },
            { Keys.Down, InputAction.Down },
            { Keys.Space, InputAction.Jump },
            { Keys.Escape, InputAction.Escape },
        };
    }

    public class InputManager : IInputManager
    {
        public InputMapping InputMapping { get; set; } = new();

        public InputAction CurrentInputAction { get; set; }

        public InputManager(ref Canvas canvas) 
        {
            canvas.KeyDown += KeyDown;
            canvas.KeyUp += KeyUp;
            canvas.MouseMove += MouseMove;
        }

        public void KeyDown(object? sender, KeyEventArgs e)
        {
            Debug.WriteLine("Key Down: " + e.KeyData);
            Logger.WhatThread("KeyDownCallback");
            KeyMap(e, true);
        }

        public void KeyUp(object? sender, KeyEventArgs e)
        {
            Debug.WriteLine("Key Up: " + e.KeyData);
            Logger.WhatThread("KeyUpCallback");
            KeyMap(e, false);
        }

        public void MouseMove(object? sender, MouseEventArgs e)
        {
            Debug.WriteLine("Mouse Location: " + e.Location);
            Logger.WhatThread("MouseMoveCallback");
        }

        //TODO: Dict/HashMap of key input(s) to action(s)
        public void KeyMap(KeyEventArgs e, bool setTo)
        {
            //Keys flagUp = Keys.Up | Keys.W;
            var keyData = e.KeyData;
            var currentSwitch = InputAction.None;
            var foundKey = InputMapping.InputMap.TryGetValue(e.KeyData, out currentSwitch);
            if (foundKey)
            {
                if(setTo)
                {
                    CurrentInputAction = CurrentInputAction | currentSwitch;
                } else
                {
                    CurrentInputAction = CurrentInputAction & ~currentSwitch;
                }
            }
        }
    }

    // Flag for singular storage of all simultaneous inputs (probably stupid)
    [Flags]
    public enum InputAction
    {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        Jump = 1 << 4,
        Escape = 1 << 5,
    }

    public interface IInputManager
    {
        InputAction CurrentInputAction { get; }
        void KeyDown(object? sender, KeyEventArgs e);
        void KeyUp(object? sender, KeyEventArgs e);
        void MouseMove(object? sender, MouseEventArgs e);
    }
}
