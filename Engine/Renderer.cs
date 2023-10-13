using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void RedrawCallback(object? sender, EventArgs e)
        {
            try
            {
                var result = BeginInvoke((MethodInvoker)delegate { Refresh(); });
                //Debug.WriteLine("Refresh delegate result: "+result);
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
        public void Renderer(object? sender, PaintEventArgs e)
        {
            //Get graphics from paint event
            Graphics graphics = e.Graphics;
            Debug.WriteLine(String.Format("Graphics drawn: {0}", e.ToString()));
        }
    }

    public interface IRenderer
    {
        public void Renderer(object? sender, PaintEventArgs e);
    }
}
