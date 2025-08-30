using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NqueenSolver
{
    public partial class Main : Form
    {
        Stopwatch stopWatch = new Stopwatch();
        public Main()
        {
            InitializeComponent();                        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ChessSize =(int) numericUpDown1.Value;
            int Iteration = (int)numericUpDown2.Value;
            int Npop = (int)numericUpDown3.Value;
            float Pcrossover= (float) numericUpDown4.Value/100;
            float Pmutation = (float) numericUpDown5.Value/100;
            stopWatch.Reset();
            stopWatch.Start();
            Genetic GA = new Genetic(ChessSize, Iteration, Npop, Pcrossover, Pmutation);
            GA.Start();
            stopWatch.Stop();
            Form1 form = new Form1(GA, GetTime());
            form.ShowDialog();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            int ChessSize = (int)numericUpDown1.Value;
            int Iteration = (int)numericUpDown2.Value;
            double T0 = (double)numericUpDown8.Value;
            double Alpha = (double)numericUpDown7.Value / 100;
            stopWatch.Reset();
            stopWatch.Start();
            SimulatedAnnealing SA = new SimulatedAnnealing(ChessSize, Iteration,T0,Alpha);
            SA.Start();            
            stopWatch.Stop();
            Form1 form = new Form1(SA, GetTime());
            form.ShowDialog();
        }        
        public string GetTime()
        {
            TimeSpan ts = stopWatch.Elapsed;
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);   
        }

    }
}
