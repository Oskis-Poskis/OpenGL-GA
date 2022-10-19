using System;
using OpenTK.Mathematics;

namespace OpenTK_Learning
{
    public readonly struct VertexAttribute
    {
        public readonly string Name;
        public readonly int Index;
        public readonly int ComponentCount;
        public readonly int Offset;

        public VertexAttribute(string name, int index, int componentcount, int offset)
        {
            this.Name = name;
            this.Index = index;
            this.ComponentCount = componentcount;
            this.Offset = offset;
        }
    }

    public sealed class VertexInfo
    {
        public readonly Type Type;
        public readonly int SizeInBytes;
        public readonly VertexAttribute[] VertexAttributes;

        public VertexInfo(Type type, params VertexAttribute[] attributes)
        {
            this.Type = type;
            this.SizeInBytes = 0;

            this.VertexAttributes = attributes;

            for (int i = 0; i < this.VertexAttributes.Length; i++)
            {
                VertexAttribute attribute = this.VertexAttributes[i];
                this.SizeInBytes += attribute.ComponentCount * sizeof(float);
            }
        }

    }


    public readonly struct VertexData
    {
        public readonly Vector3 Position;
        public readonly Vector2 texCoord;
        public readonly Vector3 Normals;

        public static readonly VertexInfo VertexInfo = new VertexInfo(
            typeof(VertexData),
            new VertexAttribute("Position", 0, 3, 0),
            new VertexAttribute("texCoord", 1, 2, 3 * sizeof(float)),
            new VertexAttribute("Normals", 2, 3, 5 * sizeof(float)));

        public VertexData(Vector3 position, Vector2 texCoord, Vector3 normals)
        {
            this.Position = position;
            this.texCoord = texCoord;
            this.Normals = normals;
        }
    }
}