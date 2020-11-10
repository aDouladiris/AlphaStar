using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphaStar
{
    public partial class AlphaStarAlgorithm : Form
    {
        Dictionary<string, Node> nodeGrid = new Dictionary<string, Node>();

        public AlphaStarAlgorithm()
        {
            InitializeComponent();

            int buttonSize = int.Parse(Prompt.ShowDialog("Valte megethos", "Btn size", 80));

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            exit_button.Location = new Point(Screen.PrimaryScreen.Bounds.Width-exit_button.Width - 5, exit_button.Location.Y);
            start_button.Location = new Point(Screen.PrimaryScreen.Bounds.Width - start_button.Width - 5, start_button.Location.Y);
            algo_button.Location = new Point(Screen.PrimaryScreen.Bounds.Width - algo_button.Width - 5, algo_button.Location.Y);
            clear_button.Location = new Point(Screen.PrimaryScreen.Bounds.Width - clear_button.Width - 5, clear_button.Location.Y);

            grid_panel.Location = new Point(5, 5);
            grid_panel.Width = Screen.PrimaryScreen.Bounds.Width - 2*exit_button.Width - 15;
            grid_panel.Height = Screen.PrimaryScreen.Bounds.Height - 10;

            int horizontal_tiles_number = (int)grid_panel.Width / buttonSize;
            int vertical_tiles_number = (int)grid_panel.Height / buttonSize;

            for (int i = 0; i < horizontal_tiles_number; i++)
            {
                for (int j = 0; j < vertical_tiles_number; j++)
                {
                    Button tmp = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(buttonSize * i, buttonSize * j),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.White,
                        Name = i.ToString() + "_" + j.ToString()
                    };

                    tmp.Tag = new Node(i, j, tmp.BackColor);
                    tmp.Click += Tmp_Click;
                    grid_panel.Controls.Add(tmp);

                }
            }

            //Resize panel
            grid_panel.Width = horizontal_tiles_number * buttonSize + 2;
            grid_panel.Height = vertical_tiles_number * buttonSize + 2;


            //foreach (Node n in whiteNodes)
            //{
            //    Console.WriteLine($"whiteNodes: {n.X}, {n.Y} ");
            //}

        }



        private void Tmp_Click(object sender, EventArgs e)
        {
            Button _sender = (Button)sender;

            if(_sender.BackColor == Color.White)
            {
                _sender.BackColor = Color.Black;
            }
            else if (_sender.BackColor == Color.Black)
            {
                _sender.BackColor = Color.Blue;
            }
            else if (_sender.BackColor == Color.Blue)
            {
                _sender.BackColor = Color.Red;
            }
            else if (_sender.BackColor == Color.Red || _sender.BackColor == Color.Green || _sender.BackColor == Color.Gray)
            {
                _sender.BackColor = Color.White;
            }

        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void algo_button_Click(object sender, EventArgs e)
        {
            Node startingNode = null;
            Node endingNode = null;


            foreach (Button b in grid_panel.Controls)
            {
                if(b.Width != 20)
                    b.Text = b.Name;

                if (b.BackColor == Color.Blue)
                {
                    startingNode = (Node)b.Tag;
                    b.ForeColor = Color.White;
                }

                if(b.BackColor == Color.Red)
                {
                    endingNode = (Node)b.Tag;
                    b.ForeColor = Color.White;
                }

                if (b.BackColor == Color.Green || b.BackColor == Color.Gray || b.BackColor == Color.Lime)
                {
                    b.BackColor = Color.White;
                }

                b.Refresh();

                Node n = (Node)b.Tag;
                n.color = b.BackColor;
            }

            if(startingNode == null)
            {
                MessageBox.Show("No starting point. Cannot proceed");
                return;
            }
            else if (endingNode == null)
            {
                MessageBox.Show("No ending point. Cannot proceed");
                return;
            }

            //Reset Grid
            nodeGrid.Clear();
            foreach (Button b in grid_panel.Controls)
            {                

                Node n = (Node)b.Tag;

                Console.WriteLine($"all button tags: {n.X} {n.Y} , {n.color}");

                nodeGrid.Add(b.Name, n);
            }

            foreach(Node n in nodeGrid.Values)
            {
                Console.WriteLine($"all nodes: {n.X} {n.Y} , {n.color}");
            }

            //1st
            List<Node> openSet = new List<Node>();
            //2nd
            List<Node> closedSet = new List<Node>();

            //3rd
            openSet.Add(startingNode);

            //4th
            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for(int i=0; i<openSet.Count; i++)
                {
                    if( 
                        (openSet[i].GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y) < currentNode.GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y))
                        || (openSet[i].GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y) == currentNode.GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y)
                        && openSet[i].GetH_cost(endingNode.X, endingNode.Y) < currentNode.GetH_cost(endingNode.X, endingNode.Y))
                    )
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == endingNode)
                {
                    RetracePath(startingNode, endingNode);

                    return;
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (closedSet.Contains(neighbour))
                        continue; 

                    int currentNodeToNeighbourNodeCost = currentNode.GetG_cost(startingNode.X, startingNode.Y) + GetDistance(currentNode, neighbour);

                    if(currentNodeToNeighbourNodeCost < neighbour.GetG_cost(startingNode.X, startingNode.Y) || !openSet.Contains(neighbour) )
                    {
                        if (currentNode != null)
                        {
                            neighbour.Parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }

            MessageBox.Show("Path not found");


        }

        private void start_button_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            foreach (Button b in grid_panel.Controls)
            {
                int colorProbability = r.Next(0, 2);

                if(colorProbability == 1)
                {
                    b.BackColor = Color.Black;
                }

                //Console.WriteLine($"All buttons: {b.Name}, {b.BackColor}");
            }
        }

        private void RetracePath(Node startingNode, Node endingNode)
        {
            List<Node> path = new List<Node>();

            Node currentNode = endingNode;

            while (currentNode != startingNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            } 

            path.Reverse();            

            foreach(Node n in path)
            {
                foreach(Control c in grid_panel.Controls)
                {
                    if (c.Tag.Equals(n))
                    {

                        if (c.BackColor != Color.Red)
                        {
                            c.BackColor = Color.Lime;
                        }

                        c.Refresh();
                        //Thread.Sleep(100);                        
                    }
                }
            }



        }


        private Node GetNodeByCoords(int X, int Y)
        {
            try
            {
                return nodeGrid[X.ToString() + "_" + Y.ToString()];
            }
            catch(Exception ex)
            {
                return null;
            }

            
        }

        private List<Node> GetNeighbours(Node node)
        { 
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Console.WriteLine($"x: {x}, y: {y}");

                    //Skip middle coords which correspond to the current node
                    if (x == 0 && y == 0)
                        continue;

                    Node tmp = GetNodeByCoords(node.X + x, node.Y + y);

                    if (tmp != null && !neighbours.Contains(tmp) && tmp.color != Color.Black && tmp.color != Color.Gray)
                    {
                        Console.WriteLine($"GetNeighbours node: {tmp.X}, {tmp.Y} : {tmp.color} ");
                        neighbours.Add(tmp);
                    }                    
                }
            }

            foreach (Node n in neighbours)
            {
                foreach (Control c in grid_panel.Controls)
                {
                    if (c.Tag.Equals(n))
                    {
                        if (c.BackColor == Color.Black)
                        {
                            continue;
                        }
                        else if (c.BackColor == Color.Green)
                        {
                            c.BackColor = Color.Gray;
                            n.color = c.BackColor;
                        }
                        else if (c.BackColor != Color.Blue && c.BackColor != Color.Red)
                        {
                            c.BackColor = Color.Green;
                            n.color = c.BackColor;
                        }
                        
                        c.Refresh();
                        //Thread.Sleep(100);
                    }
                }
            }

            return neighbours;
        }

        private int GetDistance(Node node_A, Node node_B)
        {
            int traveling_cost_X = 1;
            int traveling_cost_Y = 1;

            int dist_X = traveling_cost_X * Math.Abs(node_A.X - node_B.X);
            int dist_Y = traveling_cost_Y * Math.Abs(node_A.Y - node_B.Y);

            return dist_X + dist_Y;
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            nodeGrid.Clear();
            foreach (Button b in grid_panel.Controls)
            {
                b.Text = "";
                b.BackColor = Color.White;
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.Black;
            }
        }
    }
}
