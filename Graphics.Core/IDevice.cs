using Graphics.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Core
{
    public interface IDevice 
    {
        Vector3D Project(Vector3D coordinate, Matrix<double> transMatrix);

        void DrawPoint(Vector3D point, Color4 color);

        void DrawTriangle(Vector3D point1, Vector3D point2, Vector3D point3, Color4 color);

        void PutPixel(int x, int y, double z, Color4 color);

        void DrawLine(Vector2D point1, Vector2D point2, Color4 color);

        void Render(Camera camera, params Mesh[] meshes);
    }
}
