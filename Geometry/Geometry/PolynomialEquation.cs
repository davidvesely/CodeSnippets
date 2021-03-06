﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CADBest.GeometryNamespace
{
    public class PolynomialEquation
    {
        private const double noSolution = -100000;
        /*!
         * If Equation is a * x^3 + b * x^2 + c * x + d = 0
         * Then coeffient is <a,b,c,d>
         */
        public static List<double> SolvePolynomialEquation(List<double> coeffient)
        {
            while (Math.Abs(coeffient[0]) < 0.01)
            {
                coeffient.RemoveAt(0);
            }

            List<double> solutions = new List<double>();
            if (coeffient.Count > 3)
            {
                List<double> dcoeff = new List<double>();
                for (int i = 0; i < coeffient.Count - 1; i++)
                    dcoeff.Add(coeffient[i] * (coeffient.Count - 1 - i));

                double sol = SingleSolution_Polynomial(coeffient, dcoeff, 1);
                if (sol != noSolution)
                {
                    solutions.Add(sol);

                    List<double> coEff = new List<double>();
                    coEff.Add(coeffient[0]);
                    for (int i = 1; i < coeffient.Count - 1; i++)
                    {
                        coEff.Add(coeffient[i] + sol * coEff[i - 1]);
                    }
                    List<double> partialSol = SolvePolynomialEquation(coEff);
                    solutions.AddRange(partialSol);
                }
            }
            else if (coeffient.Count == 3)
            {
                double delta = Math.Pow(coeffient[1], 2) - 4 * coeffient[0] * coeffient[2];
                if (delta >= 0)
                {
                    double rootVal = Math.Sqrt(delta);
                    solutions.Add((-1 * coeffient[1] + rootVal) / (2 * coeffient[0]));
                    solutions.Add((-1 * coeffient[1] - rootVal) / (2 * coeffient[0]));
                }
            }
            else if (coeffient.Count == 2)
            {
                solutions.Add(-1 * coeffient[1] / coeffient[0]);
            }
            return solutions;
        }

        private static double SingleSolution_Polynomial(List<double> coefficient, List<double> dCoeff, double initX)
        {
            double dFunc, func, x = initX, x0 = 0;
            const int maxIteration = 1000;
            int itr = 0;

            while (Math.Abs(x - x0) > 0.0001)
            {
                if (itr++ > maxIteration)
                {
                    return noSolution;
                }
                x0 = x;
                func = 0; dFunc = 0;
                for (int i = 0; i < coefficient.Count; i++)
                {
                    func += coefficient[i] * Math.Pow(x, coefficient.Count - 1 - i);
                }
                for (int i = 0; i < dCoeff.Count; i++)
                    dFunc += dCoeff[i] * Math.Pow(x, dCoeff.Count - 1 - i);

                if (dFunc != 0)
                    x = x - func / dFunc;
                else if (func < 0.0001)
                    return x;
                else
                    x += 1;
            }
            return x;
        }
    }
}
