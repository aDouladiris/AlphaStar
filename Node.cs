using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public int GetG_cost(int starting_X, int starting_Y)
        {
            return Math.Abs(X - starting_X) + Math.Abs(Y - starting_Y);
        }

        public int GetH_cost(int ending_X, int ending_Y)
        {
            return Math.Abs(X - ending_X) + Math.Abs(Y - ending_Y);
        }

        public int GetF_cost(int starting_X, int starting_Y, int ending_X, int ending_Y)
        {
            int G_cost = Math.Abs(X - starting_X) + Math.Abs(Y - starting_Y);
            int H_cost = Math.Abs(X - ending_X) + Math.Abs(Y - ending_Y);
            int F_cost = G_cost + H_cost;

            //$"G: {G_cost}\nH: {H_cost}\nF: {F_cost}";

            return F_cost;
        }
    }
}
