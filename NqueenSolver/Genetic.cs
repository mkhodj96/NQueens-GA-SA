using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NqueenSolver
{
    public class Genetic
    {
        public List<Chromosome> Population;
        private int ChessSize;
        private int Npop;
        private float PCrossover;
        private int NCrossover;
        private float PMutation;
        private int NMutation;
        private Random random;
        private bool[] Mask;
        private int Iteration;
        public Genetic(int ChessSize,int Iteration,int Npop,float PCrossover,float PMutation)
        {
            this.ChessSize=ChessSize ;
            this.Npop = Npop;
            this.PCrossover = PCrossover;
            this.PMutation = PMutation;
            this.Iteration = Iteration;
            NMutation = (int)Math.Floor(PMutation * Npop);
            NCrossover = (int)Math.Floor((PCrossover * Npop)/2);
            Population = new List<Chromosome>();
            random = new Random();           
        }
        public void Start()
        {
            PopInitialization();
            SortPopulation();
            CreateMask();
            for (int i = 0; i < Iteration; i++)
            {                
                for (int j = 0; j < NCrossover; j++)
                {
                    Chromosome[] temp = Crossover(Selection(), Selection());                 
                    Population.Add(temp[0]);
                    Population.Add(temp[1]);
                }                
                for (int j = 0; j < NMutation; j++)			    
                    Population.Add(Mutation(Selection()));
                SortPopulation();
                
                PopulationControl();
                Merge();
            }
        }      
        public void CreateMask()
        {
            Mask = new bool[ChessSize];
            Mask[0] = true;
            Mask[1] = false;
            Mask[2] = true;
            Mask[3] = true;
            Mask[4] = true;
            Mask[5] = false;
            Mask[6] = false;
            Mask[7] = true;
            
            //for (int i = 0; i < ChessSize; i++)
            //{
            //    int temp = random.Next(0, 2);
            //    if (temp == 0)
            //        Mask[i] = true;
            //}
        }       
        public void PopInitialization()
        {
            for (int i = 0; i < Npop; i++)
                Population.Add(CreateRandomSolution());
            Merge();
        }
        public Chromosome CreateRandomSolution()
        {
            Chromosome Ans = new Chromosome(ChessSize);
            List<int> list = new List<int>();
            for (int i = 1; i <= ChessSize; i++)
                list.Add(i);
            for (int i = 0; i < ChessSize; i++)
            {
                int randomNumber= random.Next(0, ChessSize - i);
                int temp=list[randomNumber];
                list.RemoveAt(randomNumber);
                Ans.Position[i] = temp;
            }
            Ans.SetCost();            
            return Ans;
        }

        public Chromosome Mutation(Chromosome Input)
        {
            Chromosome chromosome = new Chromosome(Input);
            int Index1 = this.random.Next(0, ChessSize);
            int Index2 ;
            do
            {
                Index2 = random.Next(0, ChessSize);
            } while (Index1==Index2);
            int temp=chromosome.Position[Index1];
            chromosome.Position[Index1] = chromosome.Position[Index2];
            chromosome.Position[Index2] = temp;
            chromosome.SetCost();
            return chromosome;
        }
        public Chromosome[] Crossover(Chromosome Input1, Chromosome Input2)
        {
            Chromosome[] chromosomes = new Chromosome[2];
            chromosomes[0] = new Chromosome(Input1);
            chromosomes[1] = new Chromosome(Input2);            
            for (int i = 0; i < Mask.Length; i++)
                if (Mask[i])
                {
                    int temp = chromosomes[0].Position[i];
                    chromosomes[0].Position[i] = chromosomes[1].Position[i];
                    chromosomes[1].Position[i] = temp;
                }
            FixChromosome(chromosomes[0]);
            FixChromosome(chromosomes[1]);            
            chromosomes[0].SetCost();
            chromosomes[1].SetCost();
            return chromosomes;
        }   
        private void FixChromosome(Chromosome chromosome )
        {
            List<int> list = new List<int>();
            for (int i = 1; i <= chromosome.Length; i++)
                if (!Exist(chromosome, i))
                    list.Add(i);
            for (int i = 0; i < chromosome.Length-1; i++)            
                for (int j = i + 1; j < chromosome.Length; j++)
                    if (chromosome.Position[i] == chromosome.Position[j])
                    {
                        int temp=random.Next(0,list.Count);
                        chromosome.Position[i] = list[temp];
                        list.RemoveAt(temp);
                    }                                        
        }
        public bool Exist(Chromosome chromosome,int Gene)
        {
            for (int i = 0; i < chromosome.Length; i++)
                if (chromosome.Position[i] == Gene)
                    return true;
            return false;
        }
        public Chromosome Selection()
        {                        
            return Population[ random.Next(0, Population.Count)]; 
        }        
        public void SortPopulation()
        {            
            for (int i = 0; i < Population.Count-1; i++)            
                for (int j = i+1; j < Population.Count; j++)
                {
                    if (Population[i].Cost > Population[j].Cost)
                    {                                            
                    var temp=Population[i];
                    Population[i]=Population[j];
                    Population[j] = temp;
                    }
                }            
        }
        public void PopulationControl()
        {
            while (Population.Count>Npop)            
                Population.RemoveAt(Population.Count-1);            
        }
        public void Merge()
        {            
            for (int i = 0; i < Population.Count-1; i++)            
                for (int j = i+1; j < Population.Count;)
                    if (Compare(Population[i].Position ,Population[j].Position))
                        Population.RemoveAt(j);                    
                    else                    
                        j++;
            while (Population.Count<Npop)            
                Population.Add(CreateRandomSolution());            
        }
        public bool Compare(int[] Input1,int[] Input2)
        {            
            if (Input1.Length != Input2.Length)
                return false;
            for (int i = 0; i < Input1.Length; i++)
                if (Input1[i] != Input2[i])
                    return false;
            return true;
        }
    }
}
