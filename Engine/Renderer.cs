using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngineApp.Engine
{
    public class Canvas : Form
    {
        public Canvas() 
        {
            this.DoubleBuffered = true;
        }

        public void Redraw(object? sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Redraw callback in canvas.");
                var result = BeginInvoke((MethodInvoker)delegate { Refresh(); });
            } 
            catch
            {
                Debug.WriteLine("Redraw fired while control doesn't exist.");
            }
        }
    }

    public class Renderer : RendererBase { }

    public abstract class RendererBase : IRenderer
    {
        public static int FramesRendered { get; set; } = 0;
        public static long PrevFrameTime { get; set; } = DateTime.Now.Ticks;
        public static TimeSpan FPSDeltaTime { get; set; } = TimeSpan.Zero;

        private static readonly Rectangle TestBox = new Rectangle(new Point(32, 32), new Size(32, 32));
        private static readonly Pen TestPen = new Pen(Color.White, 8);
        private static readonly FontFamily TestFontFam = new FontFamily(GenericFontFamilies.Monospace);
        private static readonly Font TestFont = new Font(TestFontFam, 16.0f, FontStyle.Bold);

        public void Renderer(object? sender, PaintEventArgs e)
        {

            //Get graphics from paint event
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.Black);
            graphics.DrawRectangle(TestPen, TestBox);
            var currFrameTime = DateTime.Now.Ticks;
            if (FramesRendered % 10 == 0)
            {
                var deltaTimeSpan = new TimeSpan(currFrameTime - PrevFrameTime);
                FPSDeltaTime = deltaTimeSpan;
            }
            PrevFrameTime = currFrameTime;
            FramesRendered++;
            DrawFPS(ref graphics, FPSDeltaTime);
        }

        static void DrawFPS(ref Graphics graphics, TimeSpan deltaTimeSpan)
        {
            graphics.DrawString(((int)((1.0f / deltaTimeSpan.Milliseconds) * 1000.0f)).ToString() + "fps",
                TestFont,
                new SolidBrush(Color.Blue),
                new PointF(420.0f, 0.0f));
        }
    }

    public interface IRenderer
    {
        public static int FramesRendered { get; set; }
        public static long PrevFrameTime { get; set; }
        public void Renderer(object? sender, PaintEventArgs e);
    }
}
