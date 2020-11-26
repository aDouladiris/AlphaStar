﻿using System;
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
        List<Control> controlPanel = new List<Control>();
        List<Control> controlPanelPhases = new List<Control>();
        Button startButton = null;
        Button finishButton = null;
        int vertical_tiles_number = 0;
        int horizontal_tiles_number = 0;
        string[] axis_dimensions = Helpers.ShowDialog("Διαλέξτε το μέγεθος κάθε τετραπλεύρου του χάρτη", "Μέγεθος Τετράπλευρου", 80, 80);
        bool isSlowMotionActive = false;
        int time_in_ms = 0;
        Size buttonSize;
        Label phase_label;
        Label phaseDescription_label;
        Button phaseNext_button;
        bool phaseOne = false;
        bool phaseTwo = false;
        bool phaseThree = false;

        public AlphaStarAlgorithm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            CheckConditions();

            buttonSize = new Size(int.Parse(axis_dimensions[0]), int.Parse(axis_dimensions[1]));
            TransformGrid();

            duration_label.Width = duration_label.Width * 2;
            duration_label.Location = new Point(grid_panel.Location.X + grid_panel.Width + 5, duration_label.Location.Y);
            controlPanel.Add(duration_label);

            timer_label.Location = new Point(duration_label.Location.X, timer_label.Location.Y);
            controlPanel.Add(timer_label);
            timer_values_label.Location = new Point(timer_label.Location.X + timer_label.Width, timer_label.Location.Y);
            controlPanel.Add(timer_values_label);


            List<Button> btns = new List<Button> { exit_button, obstacles_button, algo_button, clear_button, clearAll_button, debug_button, resize_button, slow_motion_button };

            foreach (Button b in btns)
            {
                if (b.Equals(exit_button))
                {
                    b.Width += 5;
                    b.Location = new Point(grid_panel.Location.X + grid_panel.Width + 30, grid_panel.Location.Y);
                }
                else
                {
                    b.Width = 2 * b.Width;
                    b.Location = new Point(grid_panel.Location.X + grid_panel.Width + 5, b.Location.Y);
                    controlPanel.Add(b);
                }

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
            if (_sender.BackColor == Color.White && !phaseTwo && !phaseThree)
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
            //Blue button null
            else if ((_sender.BackColor == Color.Black && startButton == null && !phaseOne && !phaseTwo && !phaseThree) || 
                (phaseOne && phaseTwo && !phaseThree && _sender.BackColor == Color.White && startButton == null)
                )
            {
                Console.WriteLine("blue null");
                _sender.BackColor = Color.Blue;
                startButton = _sender;

                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Add to color list
                if (!buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Add(_sender);

                //Remove from other lists
                if (buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Remove(_sender);
                
            }
            //Blue button !null
            else if (_sender.BackColor == Color.White && startButton != null && phaseOne && phaseTwo && !phaseThree)
            {
                Console.WriteLine("blue !null");
                startButton.BackColor = Color.White;
                //Remove from color list
                if (buttonsWithColor.Contains(startButton))
                    buttonsWithColor.Remove(startButton);
                startButton.Refresh();

                _sender.BackColor = Color.Blue;
                startButton = _sender;

                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Add to color list
                if (!buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Add(_sender);

                //Remove from other lists
                if (buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Remove(_sender);

            }
            //Red button null
            else if ((_sender.BackColor == Color.Black && finishButton == null && !phaseOne && !phaseTwo && !phaseThree) || 
                (phaseOne && phaseTwo && phaseThree && _sender.BackColor == Color.White && finishButton == null)
                )
            {
                Console.WriteLine("red null");
                _sender.BackColor = Color.Red;
                finishButton = _sender;

                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Add to color list
                if (!buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Add(_sender);

                //Remove from other lists
                if (buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Remove(_sender);

            }
            //Red button !null
            else if (_sender.BackColor == Color.White && finishButton != null && phaseOne && phaseTwo && phaseThree)
            {             

                Console.WriteLine("red !null");
                finishButton.BackColor = Color.White;
                //Remove from color list
                if (buttonsWithColor.Contains(finishButton))
                    buttonsWithColor.Remove(finishButton);
                finishButton.Refresh();

                _sender.BackColor = Color.Red;
                finishButton = _sender;

                //Assign the new color to the node            
                _senderNode.color = _sender.BackColor;

                //Add to color list
                if (!buttonsWithColor.Contains(_sender))
                    buttonsWithColor.Add(_sender);

                //Remove from other lists
                if (buttonsWithObstacles.Contains(_sender))
                    buttonsWithObstacles.Remove(_sender);

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

            CheckConditions();
            _sender.Refresh();
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool CheckConditions()
        {
            if ((startButton == null && finishButton != null) ||
                (startButton != null && finishButton == null)
                )
            {
                return false;
            }

            if (startButton != null && finishButton != null)
            {
                if (phaseNext_button != null)
                {
                    phaseNext_button.BackColor = Color.Lime;
                    phaseNext_button.ForeColor = Color.Black;
                }
                    
                algo_button.BackColor = Color.Lime;

                return true;
            }
            else
            {
                if(phaseNext_button != null)
                {
                    //phaseNext_button.BackColor = Color.Gray;
                }

                algo_button.BackColor = Color.Yellow;
                return false;
            }
            
                
            
        }

        private void algo_button_Click(object sender, EventArgs e)
        {


            if (startButton == null || finishButton == null)
            {

                foreach(Control c in controlPanel)
                {
                    c.Visible = false;
                }

                phase_label = new Label()
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = algo_button.Size,
                    Location = algo_button.Location,
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                    Text = "Στάδιο 1ο",
                    BorderStyle = BorderStyle.FixedSingle
                };

                controlPanelPhases.Add(phase_label);
                this.Controls.Add(phase_label);

                phaseDescription_label = new Label()
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = clear_button.Size,
                    Location = clear_button.Location,
                    Font = clear_button.Font,
                    Text = "Δημιουργία εμποδίων",
                    BorderStyle = BorderStyle.FixedSingle
                };

                controlPanelPhases.Add(phaseDescription_label);
                this.Controls.Add(phaseDescription_label);

                phaseNext_button = new Button()
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(100, 45),
                    Location = new Point(exit_button.Location.X-15, clearAll_button.Location.Y),
                    Font = clearAll_button.Font,
                    Text = "Επόμενο",
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White
                };
                phaseNext_button.Click += PhaseNext_button_Click;
                //phaseNext_button.Height -= 5;
                //phaseNext_button.Width += 5;

                controlPanelPhases.Add(phaseNext_button);
                this.Controls.Add(phaseNext_button);

                phaseOne = true;

            }
            else
            {                
                if (CheckConditions())
                    RunAlphastar();
            }


        }

        private void PhaseNext_button_Click(object sender, EventArgs e)
        {
            if (phaseOne && !phaseTwo && !phaseThree)
            {
                phase_label.Text = "Στάδιο 2ο";
                phaseDescription_label.Text = "Ορίστε το σημείο εκκίνησης";
                phaseTwo = true;
            }
            else if (phaseOne && phaseTwo && !phaseThree)
            {
                if (startButton == null)
                {
                    MessageBox.Show("Δεν έχετε ορίσει σημείο εκκίνησης");
                    return;
                }

                phase_label.Text = "Στάδιο 3ο";
                phaseDescription_label.Text = "Ορίστε το σημείο τερματισμού";
                phaseNext_button.Text = "Εκτέλεση αλγορίθμου";
                phaseThree = true;
            }
            else if(phaseOne && phaseTwo && phaseThree)
            {
                if (finishButton == null)
                {
                    MessageBox.Show("Δεν έχετε ορίσει σημείο τερματισμού");
                    return;
                }

                phaseOne = false;
                phaseTwo = false;
                phaseThree = false;

                foreach (Control c in controlPanelPhases)
                    c.Dispose();

                foreach (Control c in controlPanel)
                    c.Visible = true;                
            }

            if (CheckConditions())
                RunAlphastar();
                
        }

        private void RunAlphastar()
        {

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
                for (int i = 0; i < openSet.Count; i++)
                {
                    openSet[i].GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y);
                    currentNode.GetF_cost(startingNode.X, startingNode.Y, endingNode.X, endingNode.Y);

                    Button btn = GetButtonByCoords(openSet[i].X, openSet[i].Y);

                    if (buttonSize.Width >= 50 && !btn.Text.StartsWith("F: "))
                    {
                        btn.Text = "F: " + openSet[i].F_cost.ToString() + "\nG: " + openSet[i].G_cost.ToString() + "\nH: " + openSet[i].H_cost;
                        SetAutoForeColor(btn);
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
                SetAutoForeColor(currentNode_btn);
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
                    ParseTimeString(timerStr);
                    return;
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (closedSet.Contains(neighbour))
                        continue;


                    int currentNodeToNeighbourNode = currentNode.G_cost + GetDistance(currentNode, neighbour);

                    if (currentNodeToNeighbourNode < neighbour.G_cost || !openSet.Contains(neighbour))
                    {
                        neighbour.Parent = currentNode;
                        openSet.Add(neighbour);
                    }
                }
            }


            stopwatch.Stop();
            timerStr = stopwatch.ElapsedMilliseconds.ToString();
            ParseTimeString(timerStr);
            MessageBox.Show("Δε βρέθηκε μονοπάτι");
        }

        private void ParseTimeString(string timerStr)
        {            
            duration_label.Text = "Διάρκεια";
            Console.WriteLine("L: " + timerStr.Length);

            string desc = "";
            string result = "";

            if (timerStr.Length <= 3)
            {
                desc = "Χιλ. δευτ/ου:";
                result = timerStr;

            }
            else if(timerStr.Length > 3 && timerStr.Length <= 6)
            {

                string msecs = timerStr.Substring(timerStr.Length - 3, 3);
                string secs = timerStr.Substring(0, timerStr.Length-3);

                desc = "Δευτερόλεπτα:\nΧιλ. δευτ/ου:";
                result = $"{secs}\n{msecs}";

                Console.WriteLine("msecs: " + msecs);
                Console.WriteLine("secs: " + secs);


            }
            else if (timerStr.Length > 6 && timerStr.Length <= 9)
            {

                string msecs = timerStr.Substring(timerStr.Length - 3, 3);
                string secs = timerStr.Substring(timerStr.Length - 6, 3);
                string mins = timerStr.Substring(0, timerStr.Length - 6);

                desc = "Λεπτά:\nΔευτερόλεπτα:\nΧιλ. δευτ/ου:";
                result = $"{mins}\n{secs}\nm{msecs}";


            }
            else if (timerStr.Length > 9 && timerStr.Length <= 12)
            {

                string msecs = timerStr.Substring(timerStr.Length - 3, 3);
                string secs = timerStr.Substring(timerStr.Length - 6, 3);
                string mins = timerStr.Substring(timerStr.Length - 9, 3);
                string hours = timerStr.Substring(0, timerStr.Length - 9);

                Console.WriteLine("msecs: " + msecs);
                Console.WriteLine("secs: " + secs);
                Console.WriteLine("mins: " + mins);
                Console.WriteLine("hours: " + hours);

                desc = "Ώρες:\nΛεπτά:\nΔευτερόλεπτα:\nΧιλ. δευτ/ου:";
                result = $"{hours}\n{mins}\n{secs}\n{msecs}";


            }

            timer_label.Size = Helpers.GetStringSize(desc);
            timer_label.Text = desc;

            //timer_values_label.Size = Prompt.GetStringSize(result);
            timer_values_label.Location = new Point(timer_label.Location.X + timer_label.Width, timer_label.Location.Y);
            timer_values_label.Text = result;

            timer_label.Height = timer_values_label.Height;
            Console.WriteLine("f1: " + timer_label.Font);
            Console.WriteLine("f2: " + timer_values_label.Font);
            Console.WriteLine("h1: " + timer_label.Height);
            Console.WriteLine("h2: " + timer_values_label.Height);
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

                            SetAutoForeColor(btn);
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
            duration_label.Text = "";
            timer_label.Text = "";
            timer_values_label.Text = "";

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
            duration_label.Text = "";
            timer_label.Text = "";
            timer_values_label.Text = "";

            foreach (Button b in buttonsWithObstacles)
            {
                n = (Node)b.Tag;
                n.color = Color.White;
                b.BackColor = n.color;

                b.Text = "";
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

                        b.Text = $"I: {grid_panel.Controls.IndexOf(b)}\nX: {buffer[0]}\nY: {buffer[1]}";
                        b.TextAlign = ContentAlignment.MiddleLeft;
                        b.FlatAppearance.BorderSize = 1;
                        b.FlatAppearance.BorderColor = Color.Black;

                        SetAutoForeColor(b);
                    }
                    else
                    {
                        b.Text = "";
                    }
                }
            }

        }

        private void SetAutoForeColor(Button b)
        {
            if (b.BackColor.GetBrightness() >= 0.5 && b.BackColor != Color.Blue)
            {
                b.ForeColor = Color.Black;
            }
            else
            {
                b.ForeColor = Color.White;
            }

            b.FlatAppearance.BorderColor = Color.Black;
        }

        private void resize_button_Click(object sender, EventArgs e)
        {
            foreach(Control c in gridControlList)
            {
                c.Dispose();
            }
            this.Refresh();
            grid_panel.Refresh();

            string[] axis_dimensions = Helpers.ShowDialog("Διαλέξτε το μέγεθος κάθε τετραπλεύρου του χάρτη", "Μέγεθος Τετράπλευρου", 80, 80);
            buttonSize = new Size(int.Parse(axis_dimensions[0]), int.Parse(axis_dimensions[1]));
            TransformGrid();
        }

        private void slow_motion_button_Click(object sender, EventArgs e)
        {
            if(!isSlowMotionActive)
            {
                time_in_ms = int.Parse(Helpers.ShowDialogSlowMotion("Διαλέξτε τα χιλιοστά δευτερολέπτου (ms) της Αργής Κίνησης", "Αργή Κίνηση", 500));
                isSlowMotionActive = true;
                slow_motion_button.BackColor = Color.Yellow;
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
