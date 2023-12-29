using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class VectorTwo : IVectorTwo<float>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public VectorTwo() 
        {
            X = 0;
            Y = 0;
        }

        public VectorTwo(float x, float y)
        {
            X = x;
            Y = y;
        }

        public VectorTwo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static VectorTwo Zero()
        {
            return new VectorTwo();
        }
    }

    public interface IVectorTwo<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
}
