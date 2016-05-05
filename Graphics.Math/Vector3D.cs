using System;

namespace Graphics.Math
{
    public struct Vector3D
    {
        #region Static properties

        public static readonly Vector3D UnitVectorX = new Vector3D(1.0D, 0.0D, 0.0D);
        public static readonly Vector3D UnitVectorY = new Vector3D(0.0D, 1.0D, 0.0D);
        public static readonly Vector3D UnitVectorZ = new Vector3D(0.0D, 0.0D, 1.0D);
        public static readonly Vector3D ZeroVector = new Vector3D(0.0D, 0.0D, 0.0D);

        #endregion

        #region Fields

        public double X;
        public double Y;
        public double Z;

        #endregion

        #region Constructors


        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Properties

        public Matrix<double> Matrix
        {
            get { return new Matrix<double>(new double[] { X, Y, Z }, 3, 1); }
        }

        public double Length { get { return System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z)); } }

        #endregion

        #region Functions

        public void Normalize()
        {
            double length = Length;

            if (length != 0)
            {
                double invariant = 1.0D / length;
                X = X * invariant;
                Y = Y * invariant;
                Z = Z * invariant;
            }
        }

        #endregion

        #region Static functions

        public static Vector3D Cross(Vector3D left, Vector3D right)
        {
            return new Vector3D(
                (left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X)
                );
        }

        public static double Dot(Vector3D left, Vector3D right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);        
        }

        public static Vector3D Matrix4DTransform(Vector3D coordinate, Matrix<double> matrix)
        {
            var x = (coordinate.X * matrix[0, 0]) + (coordinate.Y * matrix[1, 0]) + (coordinate.Z * matrix[2, 0]) + matrix[3, 0];
            var y = (coordinate.X * matrix[0, 1]) + (coordinate.Y * matrix[1, 1]) + (coordinate.Z * matrix[2, 1]) + matrix[3, 1];
            var z = (coordinate.X * matrix[0, 2]) + (coordinate.Y * matrix[1, 2]) + (coordinate.Z * matrix[2, 2]) + matrix[3, 2];
            var w = 1.0D / ((coordinate.X * matrix[0, 3]) + (coordinate.Y * matrix[1, 3]) + (coordinate.Z * matrix[2, 3]) + matrix[3, 3]);

            return new Vector3D(x * w, y * w, z * w);
        }

        #endregion

        #region Virtual functions

        public override string ToString()
        {
            return String.Format("({0:F2}, {1:F2}, {2:F2})", X, Y, Z);
        }

        #endregion


        #region Operators 

        public static Vector3D operator +(Vector3D c1, Vector3D c2)
        {
            return new Vector3D(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static Vector3D operator -(Vector3D c1, Vector3D c2)
        {
            return new Vector3D(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public static Vector3D operator /(Vector3D c1, double cofficient)
        {
            return new Vector3D(c1.X / cofficient, c1.Y / cofficient, c1.Z / cofficient);
        }

        public static Vector3D operator *(Vector3D c1, double cofficient)
        {
            return new Vector3D(c1.X * cofficient, c1.Y * cofficient, c1.Z * cofficient);
        }

        public static Vector3D operator -(Vector3D c1)
        {
            return new Vector3D { X = -c1.X, Y = -c1.Y, Z = -c1.Z };
        }

        #endregion
    }
}
