using Graphics.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Graphics.Core
{
    public static class MeshFactory
    {
        public static Mesh CreateCube(string name = "Cube")
        {
            Mesh mesh = new Mesh(8, 12, name);

            mesh.Vertices[0] = new Vector3D(-1, 1, 1);
            mesh.Vertices[1] = new Vector3D(1, 1, 1);
            mesh.Vertices[2] = new Vector3D(-1, -1, 1);
            mesh.Vertices[3] = new Vector3D(1, -1, 1);
            mesh.Vertices[4] = new Vector3D(-1, 1, -1);
            mesh.Vertices[5] = new Vector3D(1, 1, -1);
            mesh.Vertices[6] = new Vector3D(1, -1, -1);
            mesh.Vertices[7] = new Vector3D(-1, -1, -1);

            mesh.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            mesh.Faces[1] = new Face { A = 1, B = 2, C = 3 };
            mesh.Faces[2] = new Face { A = 1, B = 3, C = 6 };
            mesh.Faces[3] = new Face { A = 1, B = 5, C = 6 };
            mesh.Faces[4] = new Face { A = 0, B = 1, C = 4 };
            mesh.Faces[5] = new Face { A = 1, B = 4, C = 5 };
            mesh.Faces[6] = new Face { A = 2, B = 3, C = 7 };
            mesh.Faces[7] = new Face { A = 3, B = 6, C = 7 };
            mesh.Faces[8] = new Face { A = 0, B = 2, C = 7 };
            mesh.Faces[9] = new Face { A = 0, B = 4, C = 7 };

            mesh.Faces[10] = new Face { A = 4, B = 5, C = 6 };
            mesh.Faces[11] = new Face { A = 4, B = 6, C = 7 };

            return mesh;
        }

        public static Mesh[] CreateCubePolygon()
        {
            Mesh[] meshes = new Mesh[4];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    meshes[i * 2 + j] = MeshFactory.CreateCube(String.Format("Cube{0}", i*2+j));
                    meshes[i * 2 + j].Position = new Vector3D(3*i, 3*j, 50);
                }
            return meshes;
        }

        public static async Task<Mesh[]> TakeMeshesFromJSONAsync(string path)
        {
            dynamic jsonObject;
            List<Mesh> meshes = new List<Mesh>();

            using (StreamReader sr = File.OpenText(path))
            {
                string data = await sr.ReadToEndAsync();
                jsonObject = JsonConvert.DeserializeObject(data);
            }

            for (int i = 0; i < jsonObject.meshes.Count; i++)
            {
                var vertices = jsonObject.meshes[i].vertices;
                var faces = jsonObject.meshes[i].indices;

                // textures number;
                var uvCount = jsonObject.meshes[i].uvCount.Value;

                int verticesStep;

                switch ((int)uvCount)
                {
                    case 0:
                        verticesStep = 6;
                        break;
                    case 1:
                        verticesStep = 8;
                        break;
                    case 2:
                        verticesStep = 10;
                        break;
                    default:
                        throw new ArgumentException("invalid uvCount");
                }

                var verticesCount = vertices.Count / verticesStep;
                var facesCount = faces.Count / 3;

                Mesh mesh = new Mesh(verticesCount, facesCount, jsonObject.meshes[i].name.Value);

                for (int j = 0; j < verticesCount; j++)
                {
                    var x = (double)vertices[j * verticesStep].Value;
                    var y = (double)vertices[j * verticesStep + 1].Value;
                    var z = (double)vertices[j * verticesStep + 2].Value;

                    mesh.Vertices[j] = new Vector3D(x, y, z);
                }

                for (int j = 0; j < facesCount; j++)
                {
                    var A = (int)faces[j * 3].Value;
                    var B = (int)faces[j * 3 + 1].Value;
                    var C = (int)faces[j * 3 + 2].Value;

                    mesh.Faces[j] = new Face { A = A, B = B, C = C };
                }

                var position = jsonObject.meshes[i].position;
                var X = (double)position[0].Value;
                var Y = (double)position[1].Value;
                var Z = (double)position[2].Value;

                mesh.Position = new Vector3D(X, Y, Z);
                meshes.Add(mesh);
            }

            return meshes.ToArray();
        }
    }
}
