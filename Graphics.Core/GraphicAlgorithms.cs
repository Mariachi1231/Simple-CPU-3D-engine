using Graphics.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Graphics.Core
{
    public static class GraphicAlgorithms
    {
        private static void Swap(ref Vector3D first, ref Vector3D second)
        {
            var temp = second;
            second = first;
            first = temp;
        }

        private static void Swap(ref Vector2D first, ref Vector2D second)
        {
            var temp = second;
            second = first;
            first = temp;
        }

        private static double Interpolate(double min, double max, double k)
        {
            if (k < 0 || k > 1)
                k = System.Math.Max(0, System.Math.Min(k, 1));
            return min + (max - min) * k;
        }

        // draw line from left line papb to right line pcpd.
        private static void FillLine(int y, Vector3D pa, Vector3D pb, Vector3D pc, Vector3D pd, Color4 color, IDevice device)
        {
            double dx1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            double dx2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int startX = (int)Interpolate(pa.X, pb.X, dx1);
            int endX = (int)Interpolate(pc.X, pd.X, dx2);

            double z1 = Interpolate(pa.Z, pb.Z, dx1);
            double z2 = Interpolate(pc.Z, pd.Z, dx2);


            for (int x = startX; x < endX; x++)
            {
                double dx = (x - startX) / (double)(endX - startX);

                double z = Interpolate(z1, z2, dx);

                device.DrawPoint(new Vector3D(x, y, z), color);
            }
        }


        // simple drawline algorithm without optimization.
        public static void RecursiveDrawLine(this IDevice device, Vector3D point1, Vector3D point2, Color4 color)
        {
            double distance = (point2 - point1).Length;

            if (distance < 2)
                return;

            Vector3D middlePoint = point1 + (point2 - point1) / 2;

            device.DrawPoint(middlePoint, color);

            RecursiveDrawLine(device, point1, middlePoint, color);
            RecursiveDrawLine(device, middlePoint, point2, color);
        }

        // base algorithm of rasterization
        public static void OtherTriangleRasterization(this IDevice device, Vector3D p1, Vector3D p2, Vector3D p3, Color4 color)
        {
            if (p1.Y > p2.Y)
                Swap(ref p1, ref p2);
            if (p1.Y > p3.Y)
                Swap(ref p1, ref p3);
            if (p2.Y > p3.Y)
                Swap(ref p2, ref p3);

            int totalHeight = (int)(p3.Y - p1.Y);
            for (int y = (int)p1.Y; y <= (int)p2.Y; y++)
            {
                int segmentHeight = (int)(p2.Y - p1.Y) + 1;
                double d1 = (double)(y - p1.Y) / totalHeight;
                double d2 = (double)(y - p1.Y) / segmentHeight;

                Vector3D A = p1 + (p3 - p1) * d1;
                Vector3D B = p1 + (p2 - p1) * d2;

                if (A.X > B.X)
                    Swap(ref A, ref B);
                for (int x = (int)A.X; x <= (int)B.X; x++)
                    device.DrawPoint(new Vector3D { X = x, Y = y, Z = 0 }, color);
            }

            for (int y = (int)p2.Y; y <= (int)p3.Y; y++)
            {
                int segmentHeight = (int)(p3.Y - p2.Y) + 1;
                double d1 = (double)(y - p1.Y) / totalHeight;
                double d2 = (double)(y - p2.Y) / segmentHeight;

                Vector3D A = p1 + (p3 - p1) * d1;
                Vector3D B = p2 + (p3 - p2) * d2;

                if (A.X > B.X)
                    Swap(ref A, ref B);

                for (int x = (int)A.X; x <= (int)B.X; x++)
                    device.DrawPoint(new Vector3D { X = x, Y = y, Z = 0 }, color);
            }
        }

        public static void TriangleRasterization(this IDevice device, Vector3D p1, Vector3D p2, Vector3D p3, Color4 color)
        {
            if (p1.Y > p2.Y)
                Swap(ref p1, ref p2);
            if (p2.Y > p3.Y)
                Swap(ref p2, ref p3);
            if (p1.Y > p2.Y)
                Swap(ref p1, ref p2);

            double inverseP1P2, inverseP1P3;

            if ((p2.Y - p1.Y) > 0)
                inverseP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
            else inverseP1P2 = 0;

            if ((p3.Y - p1.Y) > 0)
                inverseP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
            else inverseP1P3 = 0;


            if (inverseP1P2 > inverseP1P3)
            {
                for (int y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                        FillLine(y, p1, p3, p1, p2, color, device);
                    else FillLine(y, p1, p3, p2, p3, color, device);
                }
            } else
            {
                for (int y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                        FillLine(y, p1, p2, p1, p3, color, device);
                    else FillLine(y, p2, p3, p1, p3, color, device);
                }
            }

        }

        // bresenhem from wikipedia)
        // https://ru.wikibooks.org/wiki/%D0%A0%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D0%B8_%D0%B0%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC%D0%BE%D0%B2/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D0%91%D1%80%D0%B5%D0%B7%D0%B5%D0%BD%D1%85%D1%8D%D0%BC%D0%B0#.D0.A0.D0.B5.D0.B0.D0.BB.D0.B8.D0.B7.D0.B0.D1.86.D0.B8.D1.8F_.D0.BD.D0.B0_C.2B.2B
        public static void BresenhamLineRasterization(this IDevice device, Vector2D point1, Vector2D point2, Color4 color)
        {
            int x1 = (int)point1.X;
            int y1 = (int)point1.Y;

            int x2 = (int)point2.X;
            int y2 = (int)point2.Y;

            int dx = System.Math.Abs(x2 - x1);
            int dy = System.Math.Abs(y2 - y1);

            int stepX = (x1 < x2) ? 1 : -1;
            int stepY = (y1 < y2) ? 1 : -1;

            int err = dx - dy;
            while (true)
            {
                device.DrawPoint(new Vector3D(x1, y1, 0), color);

                if ((x1 == x2) && (y1 == y2))
                    break;

                int err2 = err * 2;

                if (err2 > -dy)
                {
                    err -= dy;
                    x1 += stepX;
                }

                if (err2 < dx)
                {
                    err += dx;
                    y1 += stepY;
                }

            }
        }
    }
}
