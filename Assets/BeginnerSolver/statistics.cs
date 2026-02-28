using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPhaseSolver;
using Unity.VisualScripting;

namespace Assets.BeginnerSolver
{
    public struct CubeStats
    {
        public Cube cube;
        public string solution,stepRotations;
        public int[] step;
    
        public CubeStats(Cube cube)
        {
            this.cube = cube;
            this.stepRotations = "";
            this.solution = "";
            this.step = new int[8];
        }
    
        public void Add(string c)
        {
            if(stepRotations == "")
            {
                stepRotations = c;
            }
            else
            {
                stepRotations += $" {c}";
            }
        }
    
        public void Add(int i,int length)
        {
            step[i] = length;
        }
    
    }
}
