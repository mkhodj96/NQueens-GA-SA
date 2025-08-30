using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NqueenSolver
{
    public class Chromosome
    {
        public int Length { get; set; }
        public int Cost { get; set; }
        public int[] Position { get; set; }
        public Chromosome(int Length)
        {
            this.Length = Length;
            Position = new int[Length];            
        }

        public Chromosome(Chromosome chromosome)
        {
            this.Length = chromosome.Length;
            Position = new int[Length];
            for (int i = 0; i < Length; i++)            
                Position[i] = chromosome.Position[i];                        
            SetCost();         
        }      
        public void SetCost()
        {
            Cost = 0;
            for (int i = 0; i < Length - 1; i++)
                for (int j = i + 1; j < Length; j++)
                    if (Abstract(i - j) == Abstract(Position[i] - Position[j]))
                        Cost++;                             
        }
        private int Abstract(int Input)
        {
            if (Input < 0)
                Input *= -1;
            return Input;
        }
        
    }

}
