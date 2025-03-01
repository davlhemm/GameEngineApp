using System.Diagnostics;
using System.Drawing.Text;

namespace GameEngineApp.Engine
{
    public class Canvas : Form
    {
        public Canvas() 
        {
            DoubleBuffered = true;
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        /// <summary>
        /// TODO: Send a timestep or something to indicate whether we need to actually call draw...
        /// going to default here to a step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Refresh(object? sender, DrawFrameEventArgs e)
        {
            try
            {
                if(e.DrawFrame)
                {
                    var result = Invoke((MethodInvoker)delegate { Refresh(); });
                    Thread.Sleep(new TimeSpan(1));
                }
            } 
            catch
            {
                Debug.WriteLine("Refresh fired while control doesn't exist.");
            }
        }
    }

    public class Renderer : RendererBase { }

    public abstract class RendererBase : IRenderer
    {
        //TODO: Manage with entity-component-system, this is stupid
        private static readonly FontFamily TestFontFam = new FontFamily(GenericFontFamilies.Monospace);
        private static readonly Font TestFont = new Font(TestFontFam, 16.0f, FontStyle.Bold);

        public virtual void Render(object? sender, PaintEventArgs e, IList<IShape2D> shapes)
        {
            //Get graphics from paint event
            Graphics graphics = e.Graphics;
            if (true)
            { 
                graphics.Clear(Color.Black); 
            }

            //Draw all entities
            DrawShapes(ref graphics, ref shapes);
#if DEBUG   //Draw FPS if debugging
            DrawTimeSpan(ref graphics, GameLoop.Instance.TickDeltaTime, "tickspersec", 0.0f);
            DrawTimeSpan(ref graphics, GameLoop.Instance.TickDeltaTime*GameLoop.FrameRateSkip, "fps", 24.0f);
#endif
        }

        /// <summary>
        /// TODO: Actually draw based on entity type
        /// TODO: Draw Images instead of rectangles
        /// TODO: Z-indeces?
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawShapes(ref Graphics graphics, ref IList<IShape2D> shapes)
        {
            foreach(var shape in shapes)
            {
                shape.DrawEntity(ref graphics);
            }
        }

        /// <summary>
        /// TODO: Update this to new tick vs rendered frame method
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="deltaTimeSpan"></param>
        private void DrawTimeSpan(ref Graphics graphics, TimeSpan deltaTimeSpan, string tag = "fps", float y = 0.0f)
        {
            var rightBound = graphics.ClipBounds.Right;
            var dataString = ((int)(1.0f / deltaTimeSpan.Milliseconds * 1000.0f)).ToString() + tag;
            graphics.DrawString(dataString,
                TestFont,
                new SolidBrush(Color.Blue),
                new PointF(rightBound-(dataString.Length*16f), y));
        }
    }

    public interface IRenderer
    {
        public void Render(object? sender, PaintEventArgs e, IList<IShape2D> shapes);
    }
}
