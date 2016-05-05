using Graphics.Math;

namespace Graphics.Core
{
    public class WorldObject
    {
        #region Constructors

        public WorldObject()
        {
            Position = Vector3D.ZeroVector;
            Rotation = Vector3D.ZeroVector;
        }

        #endregion

        #region Properties

        public Vector3D Position { get; set; }

        public Vector3D Rotation { get; set; }

        #endregion
    }
}
