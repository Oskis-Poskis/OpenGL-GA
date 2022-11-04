using System;
using static Bernard.Setup;
using OpenTK.Mathematics;

namespace Axyz
{
    class Circle
    {
        // Function for generating vertices of a circle
        /// <summary>
        /// Generate vertices and UVs for a circle 
        /// </summary>
        /// <param name="resolution">Resolution of the circle</param>
        /// <param name="radius">Radius of the circle</param>
        public static VertexData[] GenCircleVerts(int resolution, float radius)
        {
            VertexData[] circleVerts = new VertexData[resolution + 1];
            for (int i = 0; i < resolution; i++)
            {
                circleVerts[i] = new VertexData(
                    // Verts
                    new Vector3(
                    (float)(Math.Cos(-i * 2 * Math.PI / resolution) * radius),
                    (float)(Math.Sin(-i * 2 * Math.PI / resolution) * radius), 0.0f),
                    // UV's
                    new Vector2(
                    (float)(Math.Cos(-i * 2 * Math.PI / resolution) + 1) / 2,
                    (float)(Math.Sin(-i * 2 * Math.PI / resolution) + 1) / 2),
                    // Normals
                    new Vector3(0.0f, 0.0f, -1.0f));
            }

            // Center the last vertice
            circleVerts[resolution] = new VertexData(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.5f, 0.5f), new Vector3(0.0f, 0.0f, -1.0f));

            return circleVerts;
        }

        // Function for generating indices of a circle
        /// <summary>
        /// Generate indices for a circle
        /// </summary>
        /// <param name="Resolution of the circle, should match GenCircleVerts resolution"></param>
        public static int[] GenCircleIndices(int resolution)
        {
            // Generate indices
            int[] circleIndi = new int[(resolution + 1) * 3 - 3];
            for (int i = 0; i < (resolution + 1) * 3 - 4; i += 3)
            {
                circleIndi[i] = resolution;
                circleIndi[i + 1] = i / 3;
                circleIndi[i + 2] = i / 3 + 1;
            }

            // Fix the last index to match the first index, 0
            circleIndi[(resolution + 1) * 3 - 4] = 0;

            return circleIndi;
        }
    }
}