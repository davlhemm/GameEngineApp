using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class GameLoop : IGameLoop
    {
        public static int LoopCount = 0;
        public static bool GameLooping = true;
        public event EventHandler<GameLoopedEventArgs>? GameLooped;
        public event EventHandler? GameRedraw;

        public Thread GameLoopThread { get; protected set; }

        public GameLoop() 
        {
            GameLoopThread = new Thread(Loop);
            GameLoopThread.Start();
        }

        public void Loop()
        {
            while (GameLooping && GameLoopThread.IsAlive)
            {
                //Fire events for having looped
                GameLooped?.Invoke(this, new GameLoopedEventArgs(LoopCount));
                GameRedraw?.Invoke(this, new EventArgs());
                Debug.WriteLine(String.Format("Loop {0}", LoopCount));
                LoopCount++;
                Thread.Sleep(100);
            }
            //GameLoopThread?.Join();
        }

        public void StopLoop()
        {
            GameLooping = false;
        }
    }

    public interface IGameLoop
    {
        Thread GameLoopThread { get; }
        void Loop();
        void StopLoop();
        public event EventHandler<GameLoopedEventArgs> GameLooped;
        public event EventHandler GameRedraw;
    }

    public class GameLoopedEventArgs : EventArgs 
    {
        public int LoopCount { get; protected set; }
        public GameLoopedEventArgs(int loopCount) 
        {
            LoopCount = loopCount;
        }
    }
}
