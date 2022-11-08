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
        public static LightVertexData[] importedLightData;
        public static VertexData[] importedData;
        public static int[] importindices;
        public static string importname;

        public static void LoadModel(string path, bool isLight = false)
        {
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(2f));
            m_model = importer.ImportFile(path,
                PostProcessPreset.TargetRealTimeMaximumQuality |
                PostProcessSteps.FlipWindingOrder | PostProcessSteps.GenerateSmoothNormals);

            importedLightData = new LightVertexData[m_model.Meshes[0].Vertices.Count];
            importedData = new VertexData[m_model.Meshes[0].Vertices.Count];
            importindices = m_model.Meshes[0].GetIndices();
            importname = m_model.Meshes[0].Name;

            if (isLight == false)
            {
                for (int i = 0; i < m_model.Meshes[0].Vertices.Count; i++)
                {
                    if (m_model.Meshes[0].HasTextureCoords(0) == true && m_model.Meshes[0].HasTangentBasis == true)
                    {
                        importedData[i] = new VertexData(
                        FromVector(m_model.Meshes[0].Vertices[i]),
                        FromVector(m_model.Meshes[0].TextureCoordinateChannels[0][i]).Xy,
                        FromVector(m_model.Meshes[0].Normals[i]),
                        FromVector(m_model.Meshes[0].Tangents[i]),
                        FromVector(m_model.Meshes[0].BiTangents[i]));
                    }

                    else
                    {
                        importedData[i] = new VertexData(
                        FromVector(m_model.Meshes[0].Vertices[i]),
                        Vector2.Zero,
                        FromVector(m_model.Meshes[0].Normals[i]),
                        Vector3.Zero,
                        Vector3.Zero);
                    }
                }
            }

            if (isLight == true)
            {
                for (int i = 0; i < m_model.Meshes[0].Vertices.Count; i++)
                {
                    importedLightData[i] = new LightVertexData(FromVector(m_model.Meshes[0].Vertices[i]));
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
