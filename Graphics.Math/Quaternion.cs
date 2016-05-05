using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Math
{
    public class Quaternion
    {
        #region Fields

        private double[] values = new double[4];

        #endregion


        #region Constructors

        public Quaternion()
        {
        }

        public Quaternion(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #endregion

        #region Properties
        public double X
        {
            get { return values[0]; }
            set { values[0] = value; }
        }

        public double Y
        {
            get { return values[1]; }
            set { values[1] = value; }
        }

        public double Z
        {
            get { return values[2]; }
            set { values[2] = value; }
        }

        public double W
        {
            get { return values[3]; }
            set { values[3] = value; }
        }

        #endregion

        #region Static functions

        public static Quaternion GetRotationYawPitchRollQuaternion(double yaw, double pitch, double roll)
        {
            double halfYaw = yaw * 0.5D;
            double halfPitch = pitch * 0.5D;
            double halfRoll = roll * 0.5D;

            Quaternion qYaw = new Quaternion(0, 0, System.Math.Sin(halfYaw), System.Math.Cos(halfYaw));
            Quaternion qPitch = new Quaternion(0, System.Math.Sin(halfPitch), 0, System.Math.Cos(halfPitch));
            Quaternion qRoll = new Quaternion(System.Math.Sin(halfRoll), 0, 0, System.Math.Cos(halfRoll));

            Quaternion qTmp = qYaw * qPitch;
            return qTmp * qRoll;
        }

        #endregion


        #region Operators

        public static Quaternion operator*(Quaternion left, Quaternion right)
        {
            Quaternion result = new Quaternion();

            result.W = (left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z);
            result.X = (left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y);
            result.Y = (left.W * right.Y - left.X * right.Z + left.Y * right.W + left.Z * right.X);
            result.Z = (left.W * right.Z + left.X * right.Y - left.Y * right.X + left.Z * right.W);

            return result;
        }

        #endregion
    }
}
