using GameEngineApp.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class InputManager : IInputManager
    {
        public bool Up {  get; set; }

        public bool Down { get; set; }

        public bool Left { get; set; }

        public bool Right { get; set; }

        public bool Escape { get; set; }

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
            switch (e.KeyData)
            {
                case Keys.Up:
                    Up = setTo;
                    break;
                case Keys.Down:
                    Down = setTo;
                    break;
                case Keys.Left:
                    Left = setTo;
                    break;
                case Keys.Right:
                    Right = setTo;
                    break;
                case Keys.Escape: 
                    Escape = setTo;
                    break;
            }
        }
    }

    public interface IInputManager
    {
        bool Up { get; } 
        bool Down { get; }
        bool Left { get; }
        bool Right { get; }
        bool Escape { get; }
        void KeyDown(object? sender, KeyEventArgs e);
        void KeyUp(object? sender, KeyEventArgs e);
        void MouseMove(object? sender, MouseEventArgs e);
    }
}
