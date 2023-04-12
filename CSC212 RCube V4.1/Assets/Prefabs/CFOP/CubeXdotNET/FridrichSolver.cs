// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CubeXdotNET
{
    public class FridrichSolver
    {
        public char[] Cube = "".ToCharArray();
        public int Length = 0;
        public bool IsSolved = false;
        public int ErrorCode = 0;
        public TimeSpan SolverTime = TimeSpan.FromSeconds(0);

        public string Solution = "";

        public FridrichSolver(string ScramledCube)
        {
            this.Cube = ScramledCube.ToCharArray();
        }

        public int Solve()
        {


            DateTime StartTime = DateTime.Now;

            Cross.Solve(this);
            Debug.Log(ErrorCode);

            F2L.Solve(this);
            Debug.Log(ErrorCode);

            OLL.Solve(this);
            Debug.Log(ErrorCode);

            PLL.Solve(this);
            Debug.Log(ErrorCode);


            if (Enumerable.SequenceEqual(this.Cube, Constants.SolvedCube))
            {
                Tools.OptimizeSolution(this); //Removes redundant moves like "U U'" and reduces "R R2" to "R'"

                this.SolverTime = DateTime.Now - StartTime;

                this.IsSolved = true;

                return 1; //Success
            }
            else
                return -9; //Unknown Error
        }
    }
}