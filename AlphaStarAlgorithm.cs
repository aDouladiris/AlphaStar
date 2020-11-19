using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        List<Button> buttonsWithColor = new List<Button>();
        List<Button> buttonsWithObstacles = new List<Button>();
        List<Control> gridControlList = new List<Control>();
        Button startButton = null;
        Button finishButton = null;
        int vertical_tiles_number = 0;
        int horizontal_tiles_number = 0;
        string[] axis_dimensions = Prompt.ShowDialog("Διαλέξτε το μέγεθος κάθε τετραπλεύρου του χάρτη", "Μέγεθος Τετράπλευρου", 80, 80);
        bool isSlowMotionActive = false;
        int time_in_ms = 0;
        Size buttonSize;

        public AlphaStarAlgorithm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;          
                  


            buttonSize = new Size(int.Parse(axis_dimensions[0]), int.Parse(axis_dimensions[1]));
            TransformGrid();

            duration_label.Width = duration_label.Width * 2;
            duration_label.Location = new Point(grid_panel.Location.X + grid_panel.Width + 5, duration_label.Location.Y);
            //duration_label.BorderStyle = BorderStyle.FixedSingle;

            //timer_label.Width = 2 * timer_label.Width;
            timer_label.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 2 * timer_label.Width, timer_label.Location.Y);
            //timer_label.BorderStyle = BorderStyle.FixedSingle;



            List<Button> btns = new List<Button> { exit_button, obstacles_button, algo_button, clear_button, clearAll_button, debug_button, resize_button, slow_motion_button };

            foreach (Button b in btns)
            {
                if (!b.Equals(exit_button))
                    b.Width = 2 * b.Width;
                b.Location = new Point(grid_panel.Location.X + grid_panel.Width + 5, b.Location.Y);
            }
        }

        private void TransformGrid()
        {
            int label_width = 30;
            grid_panel.Location = new Point(5, 5);
            grid_panel.Width = Screen.PrimaryScreen.Bounds.Width - 2 * exit_button.Width - 15;
            grid_panel.Height = Screen.PrimaryScreen.Bounds.Height - 10;

            horizontal_tiles_number = (int)grid_panel.Width / buttonSize.Width;
            vertical_tiles_number = (int)grid_panel.Height / buttonSize.Height;

            for (int i = 0; i < horizontal_tiles_number; i++)
            {
                for (int j = 0; j < vertical_tiles_number; j++)
                {
                    Button tmp = new Button
                    {
                        Size = buttonSize,
                        Location = new Point(buttonSize.Width * i, buttonSize.Height * j),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.White,
                        Name = i.ToString() + "_" + j.ToString()
                    };

                    tmp.Tag = new Node(i, j, tmp.BackColor);
                    tmp.Click += Tmp_Click;
                    grid_panel.Controls.Add(tmp);
                    gridControlList.Add((Control)tmp);

                    if (buttonSize.Width >= label_width)
                        DrawLabelsAroundGrid(i, j, buttonSize);

                }
            }

            //Relocate grid if size is correct
            if (buttonSize.Width >= label_width)
                grid_panel.Location = new Point(30, 20);

            //Resize panel
            grid_panel.Width = horizontal_tiles_number * buttonSize.Width + 2;
            grid_panel.Height = vertical_tiles_number * buttonSize.Height + 2;



        }

        private void DrawLabelsAroundGrid(int i, int j, Size buttonSize)
        {
            //Vertical labels
            if (i == 0)
            {
                Label tmpLbl_Y = new Label
                {
                    Size = new Size(30, buttonSize.Height),
                    Location = new Point(i, 21 + buttonSize.Height * j),
                    BackColor = Color.Transparent,
                    Text = j.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    BorderStyle = BorderStyle.FixedSingle
                };

                this.Controls.Add(tmpLbl_Y);
                gridControlList.Add((Control)tmpLbl_Y);
            }
            //Horizontal labels
            if (j == 0)
            {
                Label tmpLbl_X = new Label
                {
                    Size = new Size(buttonSize.Width, 20),
                    Location = new Point(31 + buttonSize.Width * i, j),
                    BackColor = Color.Transparent,
                    Text = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    BorderStyle = BorderStyle.FixedSingle
                };

                this.Controls.Add(tmpLbl_X);
                gridControlList.Add((Control)tmpLbl_X);
            }
        }

        private void Tmp_Click(object sender, EventArgs e)
        {
            Button _sender = (Button)sender;
            Node _senderNode = (Node)_sender.Tag;

            //Black buttons
            if (_sender.BackColor == Color.White)
            {
                _sender.BackColor = Color.Black;
                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Add to obstacles list
                if (!buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Add(_sender);

                //Remove from color list
                if (buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Remove(_sender);
            }
            //Blue button
            else if (_sender.BackColor == Color.Black && startButton == null)
            {

                Console.WriteLine("Blue in");
                _sender.BackColor = Color.Blue;
                startButton = _sender;
                //n_start = (Node)_sender.Tag;
                //n_start.color = _sender.BackColor;


                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Add to color list
                if (!buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Add(_sender);

                //Remove from other lists
                if (buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Remove(_sender);
            }
            //Red button
            else if (_sender.BackColor == Color.Black && finishButton == null)
            {

                Console.WriteLine("Red in");
                _sender.BackColor = Color.Red;
                finishButton = _sender;
                //n_start = (Node)_sender.Tag;
                //n_start.color = _sender.BackColor;

                //Add to color list
                if (!buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Add(_sender);

                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;
            }
            else if (_sender.BackColor != Color.Blue && _sender.BackColor != Color.Red)
            {
                _sender.BackColor = Color.White;
                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Remove from other lists
                if (buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Remove(_sender);

                if (buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Remove(_sender);
            }


            _sender.Refresh();

            if (startButton != null)
                Console.WriteLine($"s button: {startButton.Name} {startButton.BackColor}");

            if (finishButton != null)
                Console.WriteLine($"e button: {finishButton.Name} {finishButton.BackColor}");
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void algo_button_Click(object sender, EventArgs e)
        {

            if (startButton == null)
            {
                MessageBox.Show("No starting point. Cannot proceed");
                return;
            }
            else if (finishButton == null)
            {
                MessageBox.Show("No ending point. Cannot proceed");
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string timerStr = "";

            //1st
            List<Node> openSet = new List<Node>();
            //2nd
            List<Node> closedSet = new List<Node>();

            Node startingNode = (Node)startButton.Tag;
            Node endingNode = (Node)finishButton.Tag;

            //3rd
            openSet.Add(startingNode);

            //4th
            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for(int i=0; i<openSet.Count; i++)
                {
                    openSet[i].GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y);
                    currentNode.GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y);

                    Button btn = GetButtonByCoords(openSet[i].X, openSet[i].Y);

                    if (buttonSize.Width >= 50 && !btn.Text.StartsWith("F: ") )
                    {
                        btn.Text = "F: " + openSet[i].F_cost.ToString() + "\nG: " + openSet[i].G_cost.ToString() + "\nH: " + openSet[i].H_cost;
                        btn.Refresh();
                        if (isSlowMotionActive)
                            Thread.Sleep(time_in_ms);
                    }
                        

                    if (openSet[i].F_cost < currentNode.F_cost)                    
                        currentNode = openSet[i];
                    else if (openSet[i].F_cost == currentNode.F_cost && openSet[i].H_cost < currentNode.H_cost)
                        currentNode = openSet[i];

                }

                Button currentNode_btn = GetButtonByCoords(currentNode.X, currentNode.Y);

                if (currentNode_btn.BackColor == Color.Gray)
                    continue;

                currentNode_btn.BackColor = Color.Gray;
                currentNode.color = currentNode_btn.BackColor;

                //currentNode_btn.Refresh();
                //if (isSlowMotionActive)
                //    Thread.Sleep(time_in_ms);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == endingNode)
                {
                    RetracePath(startingNode, endingNode);
                    stopwatch.Stop();
                    timerStr = stopwatch.ElapsedMilliseconds.ToString();
                    timer_label.Text = ParseTimeString(timerStr);
                    return;
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (closedSet.Contains(neighbour))
                        continue;


                    int currentNodeToNeighbourNode = currentNode.G_cost + GetDistance(currentNode, neighbour);

                    if(currentNodeToNeighbourNode < neighbour.G_cost || !openSet.Contains(neighbour) )
                    {
                        neighbour.Parent = currentNode;
                        openSet.Add(neighbour);                     
                    }
                }
            }


            stopwatch.Stop();
            timerStr = stopwatch.ElapsedMilliseconds.ToString();
            timer_label.Size = Prompt.GetStringSize(timerStr);
            timer_label.Text = ParseTimeString(timerStr);
            MessageBox.Show("Path not found");
        }

        private string ParseTimeString(string timerStr)
        {
            if(timerStr.Length <= 3)
            {
                return "seconds: " + timerStr;
            }
            else if(timerStr.Length > 3)
            {
                return "seconds: " + timerStr;
            }

            return "seconds: " + timerStr;
        }

        private void obstacles_button_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            foreach (Button b in grid_panel.Controls)
            {
                int colorProbability = r.Next(0, 2);

                if (colorProbability == 1)
                {
                    b.BackColor = Color.Black;
                    Node n = (Node)b.Tag;
                    n.color = b.BackColor;

                    if (!buttonsWithObstacles.Contains(b))
                        buttonsWithObstacles.Add(b);
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
            path.Add(currentNode);
            path.Reverse();

            foreach (Node n in path)
            {
                Button btn = GetButtonByCoords(n.X, n.Y);

                btn.BackColor = Color.Lime;

                btn.Refresh();
                if (isSlowMotionActive)
                    Thread.Sleep(time_in_ms);

            }

        }


        private Node GetNodeByCoords(int X, int Y)
        {
            try
            {
                int index = (X * vertical_tiles_number) + Y;
                return (Node)grid_panel.Controls[index].Tag;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }            
        }

        private Button GetButtonByCoords(int X, int Y)
        {
            try
            {
                int index = (X * vertical_tiles_number) + Y;
                return (Button)grid_panel.Controls[index];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        private List<Node> GetNeighbours(Node node)
        { 
            List<Node> neighbours = new List<Node>();

            Console.WriteLine($"Current node: {node.X}, {node.Y} : {node.color} ");

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Console.WriteLine($"x: {x}, y: {y}");

                    //Skip middle coords which corresponds to the current node
                    if (x == 0 && y == 0)
                        continue;                    

                    int new_X = node.X + x;
                    int new_Y = node.Y + y;

                    //If nex coords are crossing the grid bounds, skip them
                    if (new_X >= horizontal_tiles_number || new_X < 0 || new_Y >= vertical_tiles_number || new_Y < 0 )
                        continue;

                    Node tmp = GetNodeByCoords(new_X, new_Y);
                    Button btn = GetButtonByCoords(new_X, new_Y);
                    //Console.WriteLine($"Pre GetNeighbours node: {tmp.X}_{tmp.Y} : {tmp.color} ");

                    if (
                        tmp != null 
                        && !neighbours.Contains(tmp) 
                        && btn.BackColor != Color.Black
                        )
                    {
                        Console.WriteLine($"GetNeighbours node: {tmp.X}_{tmp.Y} : {tmp.color} ");
                        neighbours.Add(tmp);
                                                  

                        if(tmp.color == Color.White)
                        {
                            tmp.color = Color.Green;
                            btn.BackColor = tmp.color;
                            if (buttonSize.Width >= 50)
                                btn.Text = btn.Name;
                            buttonsWithColor.Add(btn);

                            btn.Refresh();
                            if (isSlowMotionActive)
                                Thread.Sleep(time_in_ms);
                        }

                        
                        
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
            startButton = null;
            finishButton = null;
            Node n;
            timer_label.Text = "";

            foreach (Button b in buttonsWithColor)
            {
                n = (Node)b.Tag;
                n.color = Color.White;
                b.BackColor = n.color;

                b.Text = "";
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.Black;
                b.Refresh();                
            }

            
            buttonsWithColor.Clear();
        }

        private void clearAll_button_Click(object sender, EventArgs e)
        {
            startButton = null;
            finishButton = null;
            Node n;
            timer_label.Text = "";

            foreach (Button b in buttonsWithObstacles)
            {
                n = (Node)b.Tag;
                n.color = Color.White;
                b.BackColor = n.color;

                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.Black;
                b.Refresh();
            }
            buttonsWithObstacles.Clear();

            foreach (Button b in buttonsWithColor)
            {
                n = (Node)b.Tag;
                n.color = Color.White;
                b.BackColor = n.color;

                b.Text = "";
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.Black;
                b.Refresh();
            }
            buttonsWithColor.Clear();
        }

        private void debug_button_Click(object sender, EventArgs e)
        {
            if(buttonSize.Width > 50)
            {
                //Debug
                foreach (Button b in grid_panel.Controls)
                {
                    if (b.Text.Equals(""))
                    {

                        string[] buffer = b.Name.Split('_');

                        b.Text = $"I : {grid_panel.Controls.IndexOf(b)}\nX: {buffer[0]}\nY: {buffer[1]}";
                        b.TextAlign = ContentAlignment.MiddleCenter;
                        b.FlatAppearance.BorderSize = 1;
                        b.FlatAppearance.BorderColor = Color.Black;

                        if (b.BackColor.GetBrightness() >= 0.5)
                        {
                            b.ForeColor = Color.Black;
                        }
                        else
                        {
                            b.ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        b.Text = "";
                    }
                }
            }

        }

        private void resize_button_Click(object sender, EventArgs e)
        {
            foreach(Control c in gridControlList)
            {
                c.Dispose();
            }
            this.Refresh();
            grid_panel.Refresh();

            string[] axis_dimensions = Prompt.ShowDialog("Διαλέξτε το μέγεθος κάθε τετραπλεύρου του χάρτη", "Μέγεθος Τετράπλευρου", 80, 80);
            buttonSize = new Size(int.Parse(axis_dimensions[0]), int.Parse(axis_dimensions[1]));
            TransformGrid();
        }

        private void slow_motion_button_Click(object sender, EventArgs e)
        {
            if(!isSlowMotionActive)
            {
                time_in_ms = int.Parse(Prompt.ShowDialogSlowMotion("Διαλέξτε τα milliseconds του slow motion", "Slow Μotion", 500));
                isSlowMotionActive = true;
                slow_motion_button.BackColor = Color.Lime;
                slow_motion_button.ForeColor = Color.Black;
            }
            else
            {
                time_in_ms = 0;
                isSlowMotionActive = false;
                slow_motion_button.BackColor = Color.SlateGray;
                slow_motion_button.ForeColor = Color.White;
            }
        }

        private void timer_label_Click(object sender, EventArgs e)
        {

        }
    }
}
