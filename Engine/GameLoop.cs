using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public sealed class GameLoop : IGameLoop
    {
        public static int LoopCount = 0;
        public static bool GameLooping = false;
        public event EventHandler<GameLoopedEventArgs>? GameLooped;
        public event EventHandler? GameRedraw;
        public event EventHandler? GameUpdate;

        public Thread GameLoopThread { get; set; }

        private static readonly GameLoop instance = new GameLoop();

        public static GameLoop Instance
        {
            get { return instance; }
        }

        private GameLoop() 
        {
            GameLoopThread = new Thread(Loop);
        }

        public void Loop()
        {
            while (GameLooping && GameLoopThread.IsAlive)
            {
                //Fire events for having looped
                //OnDraw -> Refresh -> OnUpdate
                GameLooped?.Invoke(this, new GameLoopedEventArgs(LoopCount));
                GameRedraw?.Invoke(this, new EventArgs());
                GameUpdate?.Invoke(this, new EventArgs());

                Debug.WriteLine(String.Format("Loop {0}", LoopCount));
                LoopCount++;
                Thread.Sleep(10);
            }
        }

        public void Stop()
        {
            GameLooping = false;
            //TODO: Real thread management...
            //GameLoopThread?.Suspend();
        }

        public void Start() 
        {
            GameLooping = true;
            GameLoopThread.Start(); 
        }
    }

    public interface IGameLoop
    {
        Thread GameLoopThread { get; }
        void Loop();
        void Stop();
        void Start();
        public event EventHandler<GameLoopedEventArgs> GameLooped;
        public event EventHandler? GameRedraw;
        public event EventHandler? GameUpdate;
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
