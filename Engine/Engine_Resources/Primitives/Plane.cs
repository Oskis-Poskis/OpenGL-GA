using OpenTK.Mathematics;
using static Engine.SettingUP.Setup;

namespace Engine
{
    class Plane
    {
        // Cube verts + UVs
        public static VertexData[] vertices = new VertexData[]
        {
            // Pos, UV, Normals
            new VertexData(new Vector3(-0.5f, 0.0f, -0.5f), new Vector2(0, 0), new Vector3(0.0f, 1.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, 0.0f, -0.5f), new Vector2(1, 0), new Vector3(0.0f, 1.0f, 0.0f)),
            new VertexData(new Vector3(-0.5f, 0.0f, 0.5f), new Vector2(0, 1), new Vector3(0.0f, 1.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, 0.0f, 0.5f), new Vector2(1, 1), new Vector3(0.0f, 1.0f, 0.0f)),
        };

        // Cube indices
        public static int[] indices = new int[]
        {
            0, 1, 2, 2, 1, 3,   // Face
        };
    }
}