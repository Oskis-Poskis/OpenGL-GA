using OpenTK.Mathematics;

namespace OpenTK_Learning
{
    class Cube
    {
        // Cube verts + UVs
        public static VertexData[] vertices = new VertexData[]
        {
            // Pos, UV, Normals
            new VertexData(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1), new Vector3(0.0f, 0.0f, 1.0f)),
            new VertexData(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1), new Vector3(0.0f, 0.0f, 1.0f)),
            new VertexData(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 0), new Vector3(0.0f, 0.0f, 1.0f)),
            new VertexData(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 0), new Vector3(0.0f, 0.0f, 1.0f)),

            new VertexData(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(0, 1), new Vector3(1.0f, 0.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 1), new Vector3(1.0f, 0.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 0), new Vector3(1.0f, 0.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 0), new Vector3(1.0f, 0.0f, 0.0f)),

            new VertexData(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(0, 1), new Vector3(0.0f, 0.0f, -1.0f)),
            new VertexData(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1, 1), new Vector3(0.0f, 0.0f, -1.0f)),
            new VertexData(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0, 0), new Vector3(0.0f, 0.0f, -1.0f)),
            new VertexData(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1, 0), new Vector3(0.0f, 0.0f, -1.0f)),

            new VertexData(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 1), new Vector3(-1.0f, 0.0f, 0.0f)),
            new VertexData(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 1), new Vector3(-1.0f, 0.0f, 0.0f)),
            new VertexData(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0), new Vector3(-1.0f, 0.0f, 0.0f)),
            new VertexData(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(1, 0), new Vector3(-1.0f, 0.0f, 0.0f)),

            new VertexData(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 1), new Vector3(0.0f, 1.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 1), new Vector3(0.0f, 1.0f, 0.0f)),
            new VertexData(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 0), new Vector3(0.0f, 1.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 0), new Vector3(0.0f, 1.0f, 0.0f)),

            new VertexData(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1), new Vector3(0.0f, -1.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 1), new Vector3(0.0f, -1.0f, 0.0f)),
            new VertexData(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0), new Vector3(0.0f, -1.0f, 0.0f)),
            new VertexData(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 0), new Vector3(0.0f, -1.0f, 0.0f)),
        };

        // Cube indices
        public static int[] indices = new int[]
        {
            0, 1, 2, 2, 1, 3,       // Front
            4, 5, 6, 6, 5, 7,       // Right
            8, 9, 10, 10, 9, 11,    // Back
            12, 13, 14, 14, 13, 15, // Left
            16, 17, 18, 18, 17, 19, // Top
            20, 21, 22, 22, 21, 23, // Bottom
        };
    }
}