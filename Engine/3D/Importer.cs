using System;
using Assimp;
using Assimp.Configs;
using OpenTK.Mathematics;
using static Engine.SettingUP.Setup;
using static Engine.MathLib.Functions;

namespace Engine.Importer
{
    class Import
    {
        static Scene m_model;
        public static VertPosData[] importedVertPosData;
        public static VertexData[] importedVertexData;
        public static int[] importindices;
        public static string importname;

        public static Vector3 importedScale;
        public static Vector3 importedLocation;
        public static Vector3 importedRotation;

        public static void LoadModel(string path, bool vertPosOnly = false)
        {
            Vector3D tempScale;
            Vector3D tempLocation;
            Assimp.Quaternion tempRotation;

            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(2f));
            m_model = importer.ImportFile(path,
                PostProcessPreset.TargetRealTimeMaximumQuality |
                PostProcessSteps.FlipWindingOrder | PostProcessSteps.GenerateSmoothNormals);

            importedVertPosData = new VertPosData[m_model.Meshes[0].Vertices.Count];
            importedVertexData = new VertexData[m_model.Meshes[0].Vertices.Count];
            importindices = m_model.Meshes[0].GetIndices();
            importname = m_model.Meshes[0].Name;

            m_model.RootNode.Transform.Decompose(out tempScale, out tempRotation, out tempLocation);

            importedScale = new Vector3(tempScale.X, tempScale.Y, tempScale.Z);
            importedLocation = new Vector3(tempLocation.X, tempLocation.Y, tempLocation.Z);

            if (vertPosOnly == false)
            {
                for (int i = 0; i < m_model.Meshes[0].Vertices.Count; i++)
                {
                    if (m_model.Meshes[0].HasTextureCoords(0) == true && m_model.Meshes[0].HasTangentBasis == true)
                    {
                        importedVertexData[i] = new VertexData(
                        FromVector(m_model.Meshes[0].Vertices[i]),
                        FromVector(m_model.Meshes[0].TextureCoordinateChannels[0][i]).Xy,
                        FromVector(m_model.Meshes[0].Normals[i]),
                        FromVector(m_model.Meshes[0].Tangents[i]),
                        FromVector(m_model.Meshes[0].BiTangents[i]));
                    }

                    else
                    {
                        importedVertexData[i] = new VertexData(
                        FromVector(m_model.Meshes[0].Vertices[i]),
                        Vector2.Zero,
                        FromVector(m_model.Meshes[0].Normals[i]),
                        Vector3.Zero,
                        Vector3.Zero);
                    }
                }
            }

            if (vertPosOnly == true)
            {
                for (int i = 0; i < m_model.Meshes[0].Vertices.Count; i++)
                {
                    importedVertPosData[i] = new VertPosData(FromVector(m_model.Meshes[0].Vertices[i]));
                }
            }

            DebugImport();
        }

        private static void DebugImport()
        {
            Console.WriteLine("Imported mesh " + "'" + importname + "'" +
                "\nVertices: " + m_model.Meshes[0].Vertices.Count +
                "\nIndices: " + m_model.Meshes[0].GetIndices().Length.ToString() +
                "\n");
        }
    }
}
