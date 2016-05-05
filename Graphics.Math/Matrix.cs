using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Math
{
    public class Matrix
    {
        public static Matrix<double> GetXYRotationMatrix(double angle)
        {
            angle = angle.ConvertToRadians();
            return new Matrix<double>(new double[3, 3]
            {
        {
          System.Math.Cos(angle),
          -System.Math.Sin(angle),
          0.0
        },
        {
          System.Math.Sin(angle),
          System.Math.Cos(angle),
          0.0
        },
        {
          0.0,
          0.0,
          1.0
        }
            });
        }

        public static Matrix<double> GetXZRotationMatrix(double angle)
        {
            angle = angle.ConvertToRadians();
            return new Matrix<double>(new double[3, 3]
            {
        {
          System.Math.Cos(angle),
          0.0,
          System.Math.Sin(angle)
        },
        {
          0.0,
          1.0,
          0.0
        },
        {
          -System.Math.Sin(angle),
          0.0,
          System.Math.Cos(angle)
        }
            });
        }

        public static Matrix<double> GetYZRotationMatrix(double angle)
        {
            angle = angle.ConvertToRadians();
            return new Matrix<double>(new double[3, 3]
            {
        {
          1.0,
          0.0,
          0.0
        },
        {
          0.0,
          System.Math.Cos(angle),
          -System.Math.Sin(angle)
        },
        {
          0.0,
          System.Math.Sin(angle),
          System.Math.Cos(angle)
        }
            });
        }

        public static Matrix<double> GetRotationMatrixFromAxisAngle(double angle, Vector3D axis)
        {
            double num1 = System.Math.Cos(angle);
            double num2 = System.Math.Sin(angle);
            return new Matrix<double>(new double[3, 3]
            {
        {
          num1 + (1.0 - num1) * axis.X * axis.X,
          (1.0 - num1) * axis.X * axis.Y - axis.Z * num2,
          (1.0 - num1) * axis.X * axis.Z + axis.Y * num2
        },
        {
          (1.0 - num1) * axis.X * axis.Y + axis.Z * num2,
          num1 + (1.0 - num1) * axis.Y * axis.Y,
          (1.0 - num1) * axis.Y * axis.Z - axis.X * num2
        },
        {
          (1.0 - num1) * axis.X * axis.Z - axis.Y * num2,
          (1.0 - num1) * axis.Y * axis.Z + axis.X * num2,
          num1 + (1.0 - num1) * axis.Z * axis.Z
        }
            });
        }

        public static Matrix<double> GetScalingMatrix(double x, double y, double z)
        {
            double[,] values = new double[3, 3];
            values[0, 0] = x;
            values[1, 1] = y;
            values[2, 2] = z;
            return new Matrix<double>(values);
        }

        public static Matrix<double> GetXScalingMatrix(double iterations)
        {
            return Matrix.GetScalingMatrix(iterations, 1.0, 1.0);
        }

        public static Matrix<double> GetYScalingMatrix(double iterations)
        {
            return Matrix.GetScalingMatrix(1.0, iterations, 1.0);
        }

        public static Matrix<double> GetZScalingMatrix(double iterations)
        {
            return Matrix.GetScalingMatrix(1.0, 1.0, iterations);
        }

        public static Matrix<double> GetIdentityMatrix(int power)
        {
            Matrix<double> matrix = new Matrix<double>(power, power);
            for (int index = 0; index < power; ++index)
                matrix[index, index] = 1.0;
            return matrix;
        }

        public static Matrix<double> GetLookAtLHMatrix(Vector3D cameraPosition, Vector3D cameraTarget, Vector3D upVector)
        {
            Vector3D zaxis = cameraTarget - cameraPosition;
            zaxis.Normalize();
            Vector3D xaxis = Vector3D.Cross(upVector, zaxis);
            xaxis.Normalize();
            Vector3D yaxis = Vector3D.Cross(zaxis, xaxis);
            Matrix<double> identityMatrix = Matrix.GetIdentityMatrix(4);
            identityMatrix[0, 0] = xaxis.X;
            identityMatrix[1, 0] = xaxis.Y;
            identityMatrix[2, 0] = xaxis.Z;
            identityMatrix[0, 1] = yaxis.X;
            identityMatrix[1, 1] = yaxis.Y;
            identityMatrix[2, 1] = yaxis.Z;
            identityMatrix[0, 2] = zaxis.X;
            identityMatrix[1, 2] = zaxis.Y;
            identityMatrix[2, 2] = zaxis.Z;
            identityMatrix[3, 0] = -Vector3D.Dot(xaxis, cameraPosition);
            identityMatrix[3, 1] = -Vector3D.Dot(yaxis, cameraPosition);
            identityMatrix[3, 2] = -Vector3D.Dot(zaxis, cameraPosition);
            return identityMatrix;
        }

        public static Matrix<double> GetFoVPerspectiveLH(double fov, double aspect, double zMin, double zMax)
        {
            double num1 = 1.0 / System.Math.Tan(fov * 0.5);
            double num2 = zMax / (zMax - zMin);
            Matrix<double> matrix1 = new Matrix<double>(4, 4);
            Matrix<double> matrix2 = new Matrix<double>(4, 4);
            matrix2[0, 0] = num1 / aspect;
            matrix2[1, 1] = num1;
            matrix2[2, 2] = num2;
            matrix2[2, 3] = 1.0;
            matrix2[3, 2] = -num2 * zMin;
            return matrix2;
        }

        public static Matrix<double> GetQuaternionRotationMatrixYawPitchRoll(double yaw, double pitch, double roll)
        {
            return Matrix.GetRotationMatrixFromQuaternion(Quaternion.GetRotationYawPitchRollQuaternion(yaw, pitch, roll));
        }

        public static Matrix<double> GetEulerRotationMatrixYawPitchRoll(double yaw, double pitch, double roll)
        {
            return (Matrix<double>)((Matrix<double>)(Matrix.GetYZRotationMatrix(yaw) * Matrix.GetXZRotationMatrix(pitch)) * Matrix.GetXYRotationMatrix(roll));
        }

        public static Matrix<double> GetRotationMatrixFromQuaternion(Quaternion quaternion)
        {
            Matrix<double> identityMatrix = Matrix.GetIdentityMatrix(4);
            double num1 = quaternion.X + quaternion.X;
            double num2 = quaternion.Y + quaternion.Y;
            double num3 = quaternion.Z + quaternion.Z;
            double num4 = quaternion.X * num1;
            double num5 = quaternion.X * num2;
            double num6 = quaternion.X * num3;
            double num7 = quaternion.Y * num2;
            double num8 = quaternion.Y * num3;
            double num9 = quaternion.Z * num3;
            double num10 = quaternion.W * num1;
            double num11 = quaternion.W * num2;
            double num12 = quaternion.W * num3;
            identityMatrix[0, 0] = 1.0 - (num7 + num9);
            identityMatrix[1, 0] = num5 - num12;
            identityMatrix[2, 0] = num6 + num11;
            identityMatrix[0, 1] = num5 + num12;
            identityMatrix[1, 1] = 1.0 - (num4 + num9);
            identityMatrix[2, 1] = num8 - num10;
            identityMatrix[0, 2] = num6 - num11;
            identityMatrix[1, 2] = num8 + num10;
            identityMatrix[2, 2] = 1.0 - (num4 + num7);
            return identityMatrix;
        }

        public static Matrix<double> GetTranslationMatrix(double x, double y, double z)
        {
            Matrix<double> identityMatrix = Matrix.GetIdentityMatrix(4);
            identityMatrix[3, 0] = x;
            identityMatrix[3, 1] = y;
            identityMatrix[3, 2] = z;
            return identityMatrix;
        }

        public static Vector3D ConvertToVector3D(Matrix<double> matrix)
        {
            if (matrix.FirstDimensionLen != 3 || matrix.SecondDimensionLen != 1)
                throw new ArgumentException("Invalid dimensions of matrix");
            return new Vector3D()
            {
                X = matrix[0, 0],
                Y = matrix[1, 0],
                Z = matrix[2, 0]
            };
        }
    }

    public class Matrix<T> where T : struct
    {
        private T[,] matrix;

        public int FirstDimensionLen
        {
            get
            {
                return this.matrix.GetLength(0);
            }
        }

        public int SecondDimensionLen
        {
            get
            {
                return this.matrix.GetLength(1);
            }
        }

        public T this[int row, int column]
        {
            get
            {
                return this.matrix[row, column];
            }
            set
            {
                this.matrix[row, column] = value;
            }
        }

        public Matrix(int row, int column)
        {
            this.matrix = new T[row, column];
            this.Fill(new T[25]);
        }

        public Matrix(T[] values, int row, int column)
        {
            this.matrix = new T[row, column];
            this.Fill(values);
        }

        public Matrix(T[,] values)
        {
            this.matrix = values;
        }

        public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        {
            int firstDimensionLen1 = m1.FirstDimensionLen;
            int firstDimensionLen2 = m2.FirstDimensionLen;
            int secondDimensionLen1 = m1.SecondDimensionLen;
            int secondDimensionLen2 = m2.SecondDimensionLen;

            if (secondDimensionLen1 != firstDimensionLen2)
                throw new ArgumentException(string.Format("Invalid dimension {0} != {1}", (object)secondDimensionLen1, (object)firstDimensionLen2));

            T[,] values = new T[firstDimensionLen1, secondDimensionLen2];
            for (int index1 = 0; index1 < firstDimensionLen1; ++index1)
            {
                for (int index2 = 0; index2 < secondDimensionLen2; ++index2)
                {
                    for (int index3 = 0; index3 < secondDimensionLen1; ++index3)
                    {
                        dynamic first = m1[index1, index3];
                        dynamic second = m2[index3, index2];
                        values[index1, index2] += first * second;
                    }
                }
            }

            return new Matrix<T>(values);
        }

        public static Matrix<double> operator *(Matrix<T> m1, Vector3D v1)
        {
            dynamic result = v1.Matrix;
            return m1 * result;
        }

        public static Matrix<double> operator *(Vector3D v1, Matrix<T> m2)
        {
            dynamic m = v1.Matrix;
            return m * m2;
        }

        private void Fill(T[] values)
        {
             int length1 = this.matrix.GetLength(0);
             int length2 = this.matrix.GetLength(1);
             for (int index1 = 0; index1 < length1; ++index1)
             {
                 for (int index2 = 0; index2 < length2; ++index2)
                    this.matrix[index1, index2] = values[index1 * length2 + index2];
             }
        }
  }
}

