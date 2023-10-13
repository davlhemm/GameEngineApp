using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class ScreenEng
    {
        public float X { get; set; }
        public float Y { get; set; }

        public ScreenEng() 
        {
            X = 0;
            Y = 0;
        }

        public ScreenEng(float x, float y)
        {
            X = x;
            Y = y;
        }

        public ScreenEng(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static ScreenEng Zero()
        {
            return new ScreenEng();
        }
    }
}
