using Graphics.Math;

namespace Graphics.Core
{
    public class Camera : WorldObject
    {
        #region Fields

        private static Camera instance;

        private double lastRotX;
        private double curruntRotX;

        #endregion

        #region Constructor

        private Camera()
            : base()
        {
            lastRotX = curruntRotX = 0;
        }

        #endregion

        #region Properties

        public Vector3D Target { get; set; }

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        #endregion

        #region Functions

        public void Move(double speed, Direction dir)
        {
            Vector3D direction = Target - Position;
            switch (dir)
            {
                case Direction.Forward:
                    break;
                case Direction.Right:
                    direction = direction.Rotate(90, RotationType.XZ);
                    break;
                case Direction.Left:
                    direction = direction.Rotate(-90, RotationType.XZ);
                    break;
                case Direction.Back:
                    direction = -direction;
                    break;
                default:
                    direction = Vector3D.ZeroVector;
                    break;
            }

            Position += direction * speed;
            Target += direction * speed;
            Target.Normalize();
        }

        public void Rotate(double angle, Vector3D axis)
        {
            Vector3D direction = Target - Position;

            Matrix<double> rotMatrix = Matrix.GetRotationMatrixFromAxisAngle(angle, axis);

            Vector3D rotVector = Matrix.ConvertToVector3D(rotMatrix * direction);


            Target = Position + rotVector;
        }

        public void RotateByMouse(int prevX, int prevY, Vector2D mousePosition)
        {
            if (prevX == (int)mousePosition.X && prevY == (int)mousePosition.Y)
                return;


            double angleY = (double)(prevX - mousePosition.X) / 1000.0;
            double angleZ = (double)(prevY - mousePosition.Y) / 1000.0;
            Vector3D axis;

                axis = Vector3D.Cross(Target - Position, Vector3D.UnitVectorY);
                axis.Normalize();
            Rotate(angleZ, axis);


            Rotate(angleY, new Vector3D { X = 0, Y = 1, Z = 0 });
        }

        #endregion
    }
}
