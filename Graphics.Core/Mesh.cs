using Graphics.Math;

namespace Graphics.Core
{
    public class Mesh : WorldObject
    {
        #region Properties
  
        public Vector3D[] Vertices { get; private set; }
        public Face[] Faces { get; private set; }

        public string Name { get; private set; }

        #endregion

        #region Constructor

        public Mesh(int verticesCount, int facesCount, string name = "")
        {
            this.Vertices = new Vector3D[verticesCount];
            this.Faces = new Face[facesCount];
            this.Name = name;
        }

        #endregion
    }
}
