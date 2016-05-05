using System;

namespace Graphics.Math
{
    public static class ArithmExtensions
    {
        #region Functions

        public static double ConvertToRadians(this double angle)
        {
            return (System.Math.PI / 180) * angle;
        }

        public static Vector3D Rotate(this Vector3D vector, double angle, RotationType rotationType)
        {
            Matrix<double> result;
            switch (rotationType)
            {
                case RotationType.XY:
                    result = Matrix.GetXYRotationMatrix(angle) * vector;
                    break;
                case RotationType.YZ:
                    result = Matrix.GetYZRotationMatrix(angle) * vector;
                    break;
                case RotationType.XZ:
                    result = Matrix.GetXZRotationMatrix(angle) * vector;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid rotationType");
            }

            return new Vector3D(result[0, 0], result[1, 0], result[2, 0]);
        }

        public static Vector3D Scale(this Vector3D vector, double iteration, ScaleType scaleType)
        {
            Matrix<double> result;
            switch (scaleType)
            {
                case ScaleType.X:
                    result = Matrix.GetXScalingMatrix(iteration) * vector;
                    break;
                case ScaleType.Y:
                    result = Matrix.GetYScalingMatrix(iteration) * vector;
                    break;
                case ScaleType.Z:
                    result = Matrix.GetZScalingMatrix(iteration) * vector;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid scaleType");
            }

            return new Vector3D(result[0, 0], result[1, 0], result[2, 0]);
        }

        public static Vector3D Scale(this Vector3D vector, double[] scaleVector)
        {
            int length = scaleVector.Length;
            if (length != 3)
                throw new ArgumentOutOfRangeException("Invalid scaleVector length");

            Matrix<double> result = Matrix.GetScalingMatrix(scaleVector[0], scaleVector[1], scaleVector[2])
                                        * vector;

            return new Vector3D(result[0, 0], result[1, 0], result[2, 0]);
        }

        #endregion
    }
}
