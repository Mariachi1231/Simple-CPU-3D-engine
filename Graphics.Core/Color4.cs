using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Core
{
    public struct Color4
    {
        #region Static fields

        public static readonly Color4 Black = new Color4(0, 0, 0, 1);
        public static readonly Color4 White = new Color4(1, 1, 1, 1);

        #endregion

        #region Fields

        public float Alpha;
        public float Blue;
        public float Green;
        public float Red;

        #endregion

        #region Constructors

        public Color4(float red, float green, float blue, float alpha)
        {
            this.Alpha = alpha;
            this.Blue = blue;
            this.Green = green;
            this.Red = red;
        }

        #endregion
    }
}
