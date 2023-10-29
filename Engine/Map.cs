using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class Map
    {
        public string[,] Tiles { get; set; }

        /// <summary>
        /// Stupid map-build representation
        /// Used to make 2D vectors
        /// </summary>
        public Map()
        {
            Tiles = new string[,] 
            {
                { "", "", "" },
                { "", "", "" },
                { ",", ",", ","},
            };
        }
    }
}
