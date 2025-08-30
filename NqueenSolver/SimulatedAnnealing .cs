using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NqueenSolver
{
    public class SimulatedAnnealing
    {
        public Chromosome BestChromosome;
        private Chromosome chromosome;
        private double T0;
        private double Alpha;
        private Random random;
        private int ChessSize;
        private int Iteration;
        public int TimeSpent = 0;
        public SimulatedAnnealing(int ChessSize, int Iteration, double T0, double Alpha)
        {
            this.T0 = T0;
            this.Alpha = Alpha;
            this.ChessSize = ChessSize;
            this.Iteration = Iteration;
            random = new Random();           
        }
        public void Start()
        {
            TimeSpent = 0;
            Timer timer = new Timer();
            timer.Start();
            timer.Interval = 1;
            timer.Elapsed += T_Elapsed;            
            chromosome=CreateRandomSolution();
            BestChromosome = new Chromosome(chromosome);
            double T = T0;
            for (int i = 0; i < Iteration; i++)
            {
                Chromosome temp = Mutation(chromosome);
                if (temp.Cost <= chromosome.Cost)          //اگه کمتر بود نقل مکان کن      
                    chromosome = new Chromosome(temp);                
                else
                {
                    int Delta = temp.Cost-chromosome.Cost;
                    double P=Math.Exp(-(Delta/T));
                    double H = random.Next(1, 101) / 100;
                    if (P>H)
                    {
                        chromosome = new Chromosome(temp);     
                    }
                }
                if (chromosome.Cost < BestChromosome.Cost)
                    BestChromosome = new Chromosome(chromosome);
                T = T * Alpha;
            }
            timer.Stop();
        }

        void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpent++;
        }
        public Chromosome Mutation(Chromosome Input)
        {
            Chromosome chromosome = new Chromosome(Input);
            int Index1 = this.random.Next(0, ChessSize);
            int Index2;
            do
            {
                Index2 = random.Next(0, ChessSize);
            } while (Index1 == Index2);
            int temp = chromosome.Position[Index1];
            chromosome.Position[Index1] = chromosome.Position[Index2];
            chromosome.Position[Index2] = temp;
            chromosome.SetCost();
            return chromosome;
        }
        public Chromosome CreateRandomSolution()
        {
            Chromosome Ans = new Chromosome(ChessSize);
            List<int> list = new List<int>();
            for (int i = 1; i <= ChessSize; i++)
                list.Add(i);
            for (int i = 0; i < ChessSize; i++)
            {
                int randomNumber = random.Next(0, ChessSize - i);
                int temp = list[randomNumber];
                list.RemoveAt(randomNumber);
                Ans.Position[i] = temp;
            }
            Ans.SetCost();
            return Ans;
        }
    }
}
