using System;
using Assimp;
using Assimp.Configs;

namespace OpenTK_Learning
{
    class R_Loading
    {
        static Scene m_model;
        public static VertexData[] importedData;
        public static int[] importindices;
        public static string importname;

        public static void LoadModel(string path)
        {
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(2f));
            m_model = importer.ImportFile(path,
                PostProcessPreset.TargetRealTimeMaximumQuality |
                PostProcessSteps.FlipWindingOrder | PostProcessSteps.GenerateSmoothNormals);

            importedData = new VertexData[m_model.Meshes[0].Vertices.Count];
            importindices = m_model.Meshes[0].GetIndices();
            importname = m_model.Meshes[0].Name;

            for (int i = 0; i < importedData.Length; i++)
            {
                importedData[i] = new VertexData(
                    Math_Functions.FromVector(m_model.Meshes[0].Vertices[i]),
                    Math_Functions.FromVector(m_model.Meshes[0].TextureCoordinateChannels[0][i]).Xy,
                    Math_Functions.FromVector(m_model.Meshes[0].Normals[i]));
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
