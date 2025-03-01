using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class Image2D : Shape2D
    {
        public Image? Image { get; set; } = null;

        private Image2D(): base() { }

        public Image2D(VectorTwo position, VectorTwo scale, string key, string filePath): 
            base(position, scale, key)
        {
            Image = Image.FromFile(filePath);
        }

        public override void DrawEntity(ref Graphics g)
        {
            g.DrawImage(Image?? Image.FromFile("defaultItem.jpg"), Position?.X??0, Position?.Y??0, Scale?.X??0, Scale?.Y??0);
        }
    }

    public class Shape2D : Entity, IShape2D
    {
        private static readonly SolidBrush TestSolidBrush = new SolidBrush(Color.Cyan);

        public VectorTwo? Position { get; set; } = null!;
        public VectorTwo? Scale { get; set; } = null!;

        protected Shape2D(): base(string.Empty) { }

        public Shape2D(VectorTwo position, VectorTwo scale, string key): base(key)
        {
            Position = position;
            Scale = scale;
            // TODO: Don't make shapes register themselves...let the system
        }

        public virtual void DrawEntity(ref Graphics g)
        {
            g.FillRectangle(TestSolidBrush,
                Position!.X, Position!.Y,
                Scale!.X, Scale!.Y);
        }

        //TODO: Provide entity system here, don't do this here (have the system manage this instead)
        public void RegisterShape(ref IList<IShape2D> shapes)
        {
            shapes.Add(this);
            var entities = shapes.Cast<IEntity>().ToList();
            RegisterEntity(ref entities, Key);
        }

        public override void Update(IInputManager inputManager)
        {
            var playerSpeed = GameEngine.playerSpeed;
            var loopSpeed = (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
            //TODO: Normalize vector for movement before changing coordinates...
            //TODO: Calculated vector instead of piecemeal change
            if ((InputAction.Up & inputManager.CurrentInputAction) != 0)
            {
                Position!.Y -= playerSpeed * loopSpeed;
            }
            if ((InputAction.Down & inputManager.CurrentInputAction) != 0)
            {
                Position!.Y += playerSpeed * loopSpeed;
            }
            if ((InputAction.Left & inputManager.CurrentInputAction) != 0)
            {
                Position!.X -= playerSpeed * loopSpeed;
            }
            if ((InputAction.Right & inputManager.CurrentInputAction) != 0)
            {
                Position!.X += playerSpeed * loopSpeed;
            }
            base.Update(inputManager);
        }

        //TODO: Provide entity system here
        ~Shape2D()
        {
            
        }
    }

    public interface IShape2D : IEntity
    {
        VectorTwo? Position { get; set; }
        VectorTwo? Scale { get; set; }

        void DrawEntity(ref Graphics g);
    }
}
