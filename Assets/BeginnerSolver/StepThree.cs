using BeginnerSolve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPhaseSolver;
using Unity.VisualScripting;
using UnityEngine.Animations;
using UnityEngine.UIElements;

namespace Assets.BeginnerSolver
{
    internal class StepThree
    {
        private static readonly Dictionary<int, string> alignEdgeMoves = new()
        {
                {8, "D' R' D R D F D' F'"},
                {9, "D L D' L' D' F' D F"},
                {10, "D B D' B' D' L' D L"},
                {11, "D R D' R' D' B' D B"},
        };

        private static readonly Dictionary<(int, int), string> centerEdgeMoves = new()
        {
            { (7,11), "D' D' B' D B D R D' R'" },
            { (7,39), "D R D' R' D' B' D B" },
            { (7,31), "D2 B D' B' D' L' D L"},
            { (7,35), "D' L' D L D B D' B'"},
            { (7,23), "D' L D' L' D' F' D F"},
            { (7,27), "F' D F D L D' L'"},
            { (7,19), "D R' D R D F D' F'"},
            { (7,15), "F D' F' D' R' D R"},       

            { (6,23), "D2 L D' L' D' F' D F" },
            { (6,27), "D' F' D F D L D' L'" },
            { (6,31), "D B D' B' D' L' D L"},
            { (6,35), "D' D' L' D L D' B D' B'"},
            { (6,11), "D' B' D B D R D' R'" },
            { (6,39), "R D' R' D' B' D B" },
            { (6,19), "R' D R D F D' F'" },
            { (6,15), "D' F D' F' D' R' D R" },

            { (5,19), "D' R' D R D F D' F'" },
            { (5,15), "D2 F D' F' D' R' D R" },
            { (5,23), "D L D' L' D' F' D F" },
            { (5,27), "D' D' F' D F D L D' L'" },
            { (5,11), "B' D B D R D' R'" },
            { (5,39), "D' R D' R' D' B' D B" },
            { (5,31), "B D' B' D' L' D L"},
            { (5,35), "D L' D L D B D' B'"},

            { (4,19), "D' D' R' D R D F D'" },
            { (4,15), "D F D' F' D' R' D R" },
            { (4,11), "D' B' D B D R D' R'"},
            { (4,39), "D2 R D' R' D' B' D B"},
            { (4,23), "L D' L' D' F' D F" },
            { (4,27), "D F' D F D L D' L'" },
            { (4,31), "D' B D' B' D' L' D L"},
            { (4,35), "L' D L D B D' B'"},
        };

        private static readonly Dictionary<int, int> CenterStickerColor = new()
        {
            {7, 37},
            {6, 29},
            {5, 21},
            {4, 13},
        };

        public static CubeStats Solve(CubeStats cStats)
        {
            solveEdgesInBottomLayer(cStats);
            solveEdgesInMiddleLayer(cStats);
            cStats.AddToSolution();
            cStats.AddStep(1);
            return cStats;

        }
        
        private static CubeStats solveEdgesInMiddleLayer(CubeStats cStats)
        {
            int pos;
            string rotation = "";
            for (int i = 8; i < 12; i++)
            {
                if (i != cStats.cube.edges[i].pos || cStats.cube.edges[i].orient != 0)
                {
                    pos = SearchBeginner.GetCubieByID(cStats.cube.edges, i);
                    rotation = alignEdgeMoves[pos];
                    Move target = new Move(rotation);
                    cStats.cube = target.apply(cStats.cube);
                    cStats.Add(rotation);
                    pos = SearchBeginner.GetCubieByID(cStats.cube.edges, i);
                    solveEdgeX(cStats, pos);
                }
            }
            return cStats;
        }

        private static CubeStats solveEdgesInBottomLayer(CubeStats cStats)
        {
            int pos;
            for (int i = 8; i < 12; i++)
            {
                pos = SearchBeginner.GetCubieByID(cStats.cube.edges, i);
                solveEdgeX(cStats, pos);
            }
            return cStats;
        }

        private static CubeStats solveEdgeX(CubeStats cStats, int position)
        {
            int cubieFace = int.Parse(SearchBeginner.getCubeString(cStats)[CenterStickerColor[position]]);
            string rotation = centerEdgeMoves[(position, cubieFace)];
            Move target = new Move(rotation);
            cStats.cube = target.apply(cStats.cube);
            cStats.Add(rotation);
            return cStats;
        }

    }
}
