using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_of_Life
{
    public partial class Form1 : Form
    {
        bool isToroidal = true;
        // The universe array
        bool[,] universe = new bool[30, 30];
        bool[,] scratchPad = new bool[30, 30];

        // Drawing colors
        Color gridColor = Properties.Settings.Default.GridColor;
        Color cellColor = Properties.Settings.Default.CellColor;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;


        public Form1()
        {
            InitializeComponent();

            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;



            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            int cells = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int num;



                    // apply the rules
                    if (isToroidal == true)
                    {
                        num = CountNeighborsToroidal(x, y);
                    }
                    else
                    {
                        num = CountNeighborsFinite(x, y);
                    }
                    scratchPad[x, y] = false;
                    //living cells with less than 2 living cells die in next
                    if (universe[x, y] == true)
                    {
                        if (num < 2)
                        {
                            scratchPad[x, y] = false;
                        }

                    }
                    //living cells with more than 3 living neighbors die in the next
                    if (universe[x, y] == true)
                    {
                        if (num > 3)
                        {
                            scratchPad[x, y] = false;
                        }
                    }
                    //living cells with 2 or 3 living neighbors live in the next
                    if (universe[x, y] == true)
                    {
                        if (num == 2 || num == 3)
                        {
                            scratchPad[x, y] = true;
                            cells++;
                        }
                    }
                    // dead cells with exactly 3 living neighbors live in the next 
                    if (universe[x, y] == false)
                    {
                        if (num == 3)
                        {
                            scratchPad[x, y] = true;
                            cells++;
                        }
                    }

                }

            }

            // copy from scratchpad into universe
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count
            generations++;


            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusAlive.Text = "Alive = " + cells.ToString();

            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {   //make the ints into floats
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);


            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
            Pen gridPen2 = new Pen(gridColor, 2);


            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);
            int neighbors;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (isToroidal == true)
                    {
                        neighbors = CountNeighborsToroidal(x, y);
                    }
                    else
                    {
                        neighbors = CountNeighborsFinite(x, y);
                    }
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);

                    }

                    for (int i = 0; i < universe.GetLength(1); i++)
                    {
                        // Iterate through the universe in the x, left to right
                        for (int o = 0; o < universe.GetLength(0); o++)
                        {
                            if (x % 10 == 0 && y % 10 == 0)
                            {

                                RectangleF Rect = RectangleF.Empty;
                                Rect.X = x * cellWidth;
                                Rect.Y = y * cellHeight;
                                Rect.Width = cellWidth;
                                Rect.Height = cellHeight;
                                e.Graphics.DrawLine(gridPen2, Rect.X, Rect.Y, graphicsPanel1.ClientSize.Width, Rect.Y);

                                e.Graphics.DrawLine(gridPen2, Rect.X, Rect.Y, Rect.X, graphicsPanel1.ClientSize.Height);

                            }


                        }
                    }
                    Font font = new Font("Arial", 10f);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    if (neighbors != 0)
                    {
                        if ((universe[x, y] == false && neighbors == 3) || (universe[x, y] == true && (neighbors == 2 || neighbors == 3)))
                        {
                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Green, cellRect, stringFormat);

                        }
                        else
                        {
                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Red, cellRect, stringFormat);
                        }
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                }
            }


            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            gridPen2.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
       
        #region Window

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                    scratchPad[x, y] = false;
                    generations = 0;
                    toolStripStatusAlive.Text = "Alive = 0 ";
                    toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
                }
            }
            graphicsPanel1.Invalidate();
        }
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = cellColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = gridColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = universe.GetLength(0);
            int y = universe.GetLength(1);
            Modal dlg = new Modal();

            dlg.SetNumber1(timer.Interval);
            dlg.SetNumber2(x);
            dlg.SetNumber3(y);

            if (DialogResult.OK == dlg.ShowDialog())
            {

                timer.Interval = dlg.GetNumber1();
                toolStripStatusTimer.Text = "Interval: " + timer.Interval.ToString();

                x = dlg.GetNumber2();
                toolStripStatusWidth.Text = "Universe Width: " + universe.GetLength(0).ToString();

                y = dlg.GetNumber3();
                toolStripStatusHeight.Text = "Universe Height: " + universe.GetLength(1).ToString();

                graphicsPanel1.Invalidate();

            }
        }
        private void torotialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isToroidal = true;
            toroidalToolStripMenuItem.Checked = true;
            finiteToolStripMenuItem.Checked = false;
            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isToroidal = false;
            toroidalToolStripMenuItem.Checked = false;
            finiteToolStripMenuItem.Checked = true;
            graphicsPanel1.Invalidate();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;

            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
        }
        #endregion

        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;


                }
            }
            return count;
        }

        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;

                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }


        public void Randomize(int s)
        {
            Random rando = new Random();
            int L1 = universe.GetLength(0);
            int L2 = universe.GetLength(1);

            for (int x = 0; x < L1; x++)
            {
                for (int y = 0; y < L2; y++)
                {
                    if (rando.Next(0, 3) == 0)
                    {
                        universe[x, y] = true;
                    }
                }
            }
        }


    }

}

