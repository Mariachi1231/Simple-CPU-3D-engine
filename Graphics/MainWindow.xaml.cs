using Graphics.Core;
using Graphics.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Graphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeviceCore deviceCore;
        private Mesh[] meshes;
        private Camera camera = Camera.Instance;
        private DispatcherTimer timer;
        private DateTime prev;

        private bool updateScene = false;

        private static readonly int BitmapWidth = 720;
        private static readonly int BitmapHeight = 480;

        private int prevMouseX;
        private int prevMouseY;

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public MainWindow()
        {
            this.InitializeComponent();

            Loaded += MainWindow_Loaded;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            updateScene = true;
        }

        private static void SetCursor(int x, int y)
        {
            int xLeft = (int)App.Current.MainWindow.Left;
            int yTop = (int)App.Current.MainWindow.Top;

            SetCursorPos(x + xLeft, y + yTop);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WriteableBitmap bitmap = new WriteableBitmap(BitmapWidth, BitmapHeight, 96, 96, PixelFormats.Bgr32, null);

            string meshesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "monkey.babylon");
            Mesh[] meshes1 = await MeshFactory.TakeMeshesFromJSONAsync(meshesPath);
            meshes1[0].Position = new Vector3D { X = 10, Y = 0, Z = 10 };
            //meshes = new Mesh[10];
            //meshes[0] = MeshFactory.CreateCube();
            //meshes[0].Position = Vector3D.ZeroVector;
            meshes = MeshFactory.CreateCubePolygon();
            meshes = meshes.Concat(meshes1).ToArray();


            deviceCore = new DeviceCore(bitmap);

            camera.Position = new Vector3D(0, 0, -20);
            camera.Target = Vector3D.ZeroVector;


            Main.Source = bitmap;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
            Keyboard.Focus(Main);
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var currentFps = 1000.0 / (now - prev).TotalMilliseconds;
            prev = now;

            Fps.Text = String.Format("{0:0.00} Fps", currentFps);
            if (updateScene)
            {
                meshes[0].Rotation = new Vector3D(meshes[0].Rotation.X + 0.025, meshes[0].Rotation.Y + 0.025, meshes[0].Rotation.Z);
                meshes[4].Rotation = new Vector3D(meshes[4].Rotation.X, meshes[4].Rotation.Y + 0.025, meshes[4].Rotation.Z);


                deviceCore.Fill(0, 0, 0, 255);
                deviceCore.Render(camera, meshes);
                deviceCore.Refresh();
                updateScene = false;
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            updateScene = true;

            switch (key)
            {
                case Key.W:
                    camera.Move(0.05, Direction.Forward);
                    break;
                case Key.S:
                    camera.Move(0.05, Direction.Back);
                    break;
                case Key.A:
                    camera.Move(0.05, Direction.Left);
                    break;
                case Key.D:
                    camera.Move(0.05, Direction.Right);
                    break;
                default:
                    break;
            }
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(this);

            if (prevMouseY == 0 && prevMouseX == 0)
            {
                prevMouseX = (int)(this.ActualWidth / 2);
                prevMouseY = (int)(this.ActualHeight / 2);
            }

            if (mousePos.X > this.ActualWidth - 200 || mousePos.X < 200
                || mousePos.Y > this.ActualHeight - 200 || mousePos.Y < 200)
            {
                SetCursor((int)this.ActualWidth / 2, (int)this.ActualHeight / 2);
                prevMouseX = (int)(this.ActualWidth / 2);
                prevMouseY = (int)(this.ActualHeight / 2);
                return;
            }

            updateScene = true;

            camera.RotateByMouse(prevMouseX, prevMouseY, new Vector2D { X = mousePos.X, Y = mousePos.Y });
            prevMouseX = (int)mousePos.X;
            prevMouseY = (int)mousePos.Y;
        }
    }
}