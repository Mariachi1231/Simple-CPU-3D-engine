using Graphics.Math;
using System.Windows;
using System.Windows.Media.Imaging;
using System;

namespace Graphics.Core
{
    public class DeviceCore : IDevice
    {

        #region Fields

        private byte[] buffer;
        private double[] depthBuffer;
        private WriteableBitmap bitmap;

        private int width;
        private int height;

        #endregion

        #region Constructor

        public DeviceCore(WriteableBitmap bitmap)
        {
            this.bitmap = bitmap;

            width = bitmap.PixelWidth;
            height = bitmap.PixelHeight;

            this.buffer = new byte[width * height * 4];
            this.depthBuffer = new double[width * height];
        }

        #endregion

        #region Functions

        public void Fill(byte red, byte green, byte blue, byte alpha)
        {
            int i = 0;
            while (i != buffer.Length)
            {
                // WriteableBitmap use format BGRA.

                buffer[i++] = blue;
                buffer[i++] = green;
                buffer[i++] = red;
                buffer[i++] = alpha;
            }

            for (i = 0; i < depthBuffer.Length; i++)
                depthBuffer[i] = double.MaxValue;
        }
        

        private bool CheckPoint(Vector3D point)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < width && point.Y < height)
                return true;
            return false;
        }

        public void Refresh()
        {
            bitmap.WritePixels(new Int32Rect(0, 0, width, height), 
                buffer, bitmap.BackBufferStride, 0);
        }

        public void PutPixel(int x, int y, double z, Color4 color)
        {
            var idx = (x + y * width);
            var idx4 = idx * 4;

            if (depthBuffer[idx] < z)
                return;

            depthBuffer[idx] = z;

            buffer[idx4++] = (byte)(color.Blue * 255);
            buffer[idx4++] = (byte)(color.Green * 255);
            buffer[idx4++] = (byte)(color.Red * 255);
            buffer[idx4] =   (byte)(color.Alpha * 255);
        }
        #endregion

        public Vector3D Project(Vector3D coordinate, Matrix<double> transMatrix)
        {
            Vector3D point = Vector3D.Matrix4DTransform(coordinate, transMatrix);

            var x = point.X * width + width / 2.0D;
            var y = -point.Y * height + height / 2.0D;

            return new Vector3D(x, y, point.Z);
        }

        public void DrawPoint(Vector3D point, Color4 color)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < width && point.Y < height)
                PutPixel((int)point.X, (int)point.Y, point.Z, color);
        }

        public void DrawLine(Vector2D point1, Vector2D point2, Color4 color)
        {
            this.BresenhamLineRasterization(point1, point2, color);
        }

        public void DrawTriangle(Vector3D point1, Vector3D point2, Vector3D point3, Color4 color)
        {
            this.TriangleRasterization(point1, point2, point3, color);
        }

        public void Render(Camera camera, params Mesh[] meshes)
        {
            Matrix<double> viewMatrix = Matrix.GetLookAtLHMatrix(camera.Position, camera.Target, Vector3D.UnitVectorY);
            Matrix<double> projectionMatrix = Matrix.GetFoVPerspectiveLH(0.8d, (double)width/ height, 0.01D, 1.0D);

            foreach (var mesh in meshes)
            {
                bool invalid = false;
                if (mesh == null)
                    continue;

                // by Euler angles Matrix<double> rotationMatrix = Matrix.GetEulerRotationMatrixYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z);
                Matrix<double> worldMatrix = Matrix.GetQuaternionRotationMatrixYawPitchRoll(mesh.Rotation.Z, mesh.Rotation.Y, mesh.Rotation.X)
                    * Matrix.GetTranslationMatrix(mesh.Position.X, mesh.Position.Y, mesh.Position.Z);


                Matrix<double> transformMatrix = worldMatrix * viewMatrix * projectionMatrix;         

                int i = 0;
                Vector3D[] projectedPoints = new Vector3D[mesh.Vertices.Length];
                foreach (var vertex in mesh.Vertices)
                {
                    Vector3D pointInCam = Vector3D.Matrix4DTransform(vertex, worldMatrix * viewMatrix);
                    if (pointInCam.Z < 0)
                    {
                        invalid = true;
                        break;
                    }
                    projectedPoints[i++] = Project(vertex, transformMatrix);
                }

                if (invalid)
                    continue;

                var color = 0.4f;
                foreach (var face in mesh.Faces)
                {
                    Vector3D pixelA = projectedPoints[face.A];
                    Vector3D pixelB = projectedPoints[face.B];
                    Vector3D pixelC = projectedPoints[face.C];

                    if (CheckPoint(pixelA) || CheckPoint(pixelB) || CheckPoint(pixelC))
                        DrawTriangle(pixelA, pixelB, pixelC, new Color4(color, color, color, 0.5f));

                    if (color == 1)
                        color = 0.4f;
                    else color += 0.1f;
                }
            }
        }
    }
}
