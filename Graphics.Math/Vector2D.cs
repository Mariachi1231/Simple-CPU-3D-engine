using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Math
{
    public struct Vector2D
    {
        #region Fields

        public double X;
        public double Y;

        #endregion

        #region Constructors

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Properties


        public Matrix<double> Matrix
        {
            get { return new Matrix<double>(new double[] { X, Y }, 2, 1); }
        }

        public double Length
        {
            get { return System.Math.Sqrt((X * X) + (Y * Y)); }
        }

        #endregion

        #region Virtual functions

        public override string ToString()
        {
            return String.Format("({0:F2}, {1:F2})", X, Y);
        }

        #endregion


        #region Operators

        public static Vector2D operator +(Vector2D c1, Vector2D c2)
        {
            return new Vector2D(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static Vector2D operator -(Vector2D c1, Vector2D c2)
        {
            return new Vector2D(c1.X - c2.X, c1.Y - c2.Y);
        }

        public static Vector2D operator /(Vector2D c1, double cofficient)
        {
            return new Vector2D(c1.X / cofficient, c1.Y / cofficient);
        }

        public static Vector2D operator *(Vector2D c1, double cofficient)
        {
            return new Vector2D(c1.X * cofficient, c1.Y * cofficient);
        }

        #endregion
    }
}
