using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlphaStar
{
    class Node
    {
        public Color color { get; set; }
        public int X { get; }
        public int Y { get; }
        public Node Parent { get; set; }

        public Node(int X, int Y, Color color)
        {            
            this.X = X;
            this.Y = Y;
            this.color = color;
        }

        public int G_cost { get; set; }
        public int H_cost { get; set; }
        public int F_cost { get { return G_cost + H_cost; }  }

    }
}
