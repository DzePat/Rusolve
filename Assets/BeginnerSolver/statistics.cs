using System.Linq;
using TwoPhaseSolver;

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

        /// <summary>
        /// adds solution length at current step
        /// </summary>
        /// <param name="i"></param>
        public void AddStep(int i)
        {
            step[i] = solution.Split(" ").Count();
        }

        /// <summary>
        /// Adds stepRotations to solution
        /// </summary>
        public void AddToSolution()
        {
            if (solution == "")
            {
                solution = stepRotations;
                stepRotations = "";
            }
            else
            {
                solution += $" {stepRotations}";
                stepRotations = "";
            }
        }
    
    }
}
