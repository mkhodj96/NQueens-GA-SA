using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NqueenSolver
{
    public partial class Form1 : Form
    {
        PictureBox[,] Homes;
        int ChessSize;        
        Genetic GA;
        SimulatedAnnealing SA;        
        Color ColorBlack;
        Color ColorWhite;
        Color ColorRedBlack;
        Color ColorRedWhite;
        int Index = 0;
        public void ColorCreate()
        {
            ColorWhite = Color.FromArgb(220, 220, 200);
            ColorBlack = Color.FromArgb(50, 25, 10);
            ColorRedBlack = Color.FromArgb(125, 50, 50);
            ColorRedWhite = Color.FromArgb(250, 100, 100); 
        }
        public Form1(Genetic GA, string TimeStr)
        {
            ChessSize = GA.Population[0].Length;
            InitializeComponent();
            label4.Text=TimeStr;
            ColorCreate();
            this.GA = GA;            
            HomeCreate();
            GAstyle();
        }
        public Form1(SimulatedAnnealing SA, string TimeStr)
        {
            ChessSize = SA.BestChromosome.Length;
            InitializeComponent();
            label4.Text = TimeStr;
            ColorCreate();
            this.SA = SA;
            HomeCreate();
            SAstyle();
        }
        public void HomeCreate()
        {
            Homes = new PictureBox[ChessSize, ChessSize];
            for (int j = 0; j < ChessSize; j++)
                for (int i = 0; i < ChessSize; i++)            
                {
                    Homes[i, j] = new PictureBox();                    
                    this.panel3.Controls.Add(Homes[i,j]);
                    Homes[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    if ((i%2==0&&j%2==1)||(i%2==1&&j%2==0))
                        Homes[i,j].BackColor=ColorWhite;
                    else
                        Homes[i, j].BackColor = ColorBlack;
                }            
            locationSet();
            SizeSet();
        }
        public void SizeSet()
        {
            int Width = this.panel3.Size.Width / ChessSize;
            int Height = this.panel3.Size.Height / ChessSize;
            for (int j = 0; j < ChessSize; j++)
                for (int i = 0; i < ChessSize; i++)
                    Homes[i, j].Size = new Size(Width, Height);
        }
        public void locationSet()
        {
            int Width = this.panel3.Size.Width / ChessSize;
            int Height = this.panel3.Size.Height / ChessSize;
            for (int j = 0; j < ChessSize; j++)
                for (int i = 0; i < ChessSize; i++)
                    Homes[i, j].Location = new Point(Width * i, Height * j);
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {                     
                SizeSet();
                locationSet();            
        }
        public void SetQueen(int[] Ans)
        {
            ClearQueen();
            for (int i = 0; i < ChessSize; i++)            
                Homes[i, Ans[i]-1].Image= global::NqueenSolver.Properties.Resources.Qeen;                       
        }

        public void ClearQueen()
        {
            for (int i = 0; i < ChessSize; i++)
                for (int j = 0; j < ChessSize; j++)
                    Homes[i, j].Image = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Index == GA.Population.Count - 1)            
                Index = 0;            
            else
                Index++;
            SetData(Index);
        }
        public void SetData(int Index)
        {                            
            string temp = "Position : ";
            for (int i = 0; i < GA.Population[Index].Length; i++)
            {
                temp += GA.Population[Index].Position[i] + "";
                if (i != GA.Population[Index].Length - 1)                
                    temp += " , ";                
            }
            label1.Text = temp;
            label2.Text = "Index : " + Index;
            label3.Text = "Cost : " + GA.Population[Index].Cost;
            SetQueen(GA.Population[Index].Position);
            ErroColorSet(GA.Population[Index].Position);
       }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Index == 0)
                Index = GA.Population.Count - 1;
            else
                Index--;
            SetData(Index);
        }
     

        private void DefultColorSet()
        {
            for (int j = 0; j < ChessSize; j++)
                for (int i = 0; i < ChessSize; i++)
                {                                                            
                    if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                        Homes[i, j].BackColor = ColorWhite;
                    else
                        Homes[i, j].BackColor = ColorBlack;
                }
        }
        private void ErroColorSet(int[] Array)
        {
            DefultColorSet();
            for (int i = 0; i < Array.Length - 1; i++)
                for (int j = i + 1; j < Array.Length; j++)
                {
                    if ((Abstract(i - j) == Abstract(Array[i] - Array[j])) && Array[i] - Array[j] < 0)
                    {
                        for (int k = i; k <= j; k++)
                        {
                            if (Homes[k, Array[i] + k - i - 1].BackColor == ColorBlack)
                                Homes[k, Array[i] + k - i - 1].BackColor = ColorRedBlack;
                            else
                                Homes[k, Array[i] + k - i - 1].BackColor = ColorRedWhite;
                        }
                        if ((Abstract(i - j) == Abstract(Array[i] - Array[j])) && Array[i] - Array[j] > 0)
                        {
                            for (int k = i; k <= j; k++)
                            {
                                if (Homes[k, Array[i] - 1 - (k - i)].BackColor == ColorBlack)
                                    Homes[k, Array[i] - 1 - (k - i)].BackColor = ColorRedBlack;
                                else
                                    Homes[k, Array[i] - 1 - (k - i)].BackColor = ColorRedWhite;
                            }
                        }
                    }

                }
        }
        private int Abstract(int Input)
        {
            if (Input < 0)
                Input *= -1;
            return Input;
        }

        private void GAstyle()
        {
            this.panel2.Size = new Size(593, 117);
            button1.Enabled = true;
            button1.Show();
            button2.Enabled = true;
            button2.Show();
            SetData(Index);
        }
        private void SAstyle()
        {
            this.panel2.Size = new Size(593, 80);
            button1.Enabled = false;
            button1.Hide();
            button2.Enabled = false;
            button2.Hide();


            string temp = "Position : ";
            for (int i = 0; i < SA.BestChromosome.Length; i++)
            {
                temp += SA.BestChromosome.Position[i] + "";
                if (i != SA.BestChromosome.Length - 1)
                    temp += " , ";
            }
            label1.Text = temp;
            label2.Text = "Cost : " + SA.BestChromosome.Cost;
            SetQueen(SA.BestChromosome.Position);
            ErroColorSet(SA.BestChromosome.Position);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SizeSet();
            locationSet();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
