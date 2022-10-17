using System;
using System.IO;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Learning
{
    class OBJ_Loader
    {
        public static List<VertexData> vertData = new List<VertexData>();

        public static void LoadOBJ(string path)
        {
            string[] Data = File.ReadAllLines(path);
            string mtlname = "_defaultMTL";
            string ObjectName = "_defaultName";

            List<Vector3> Positions = new List<Vector3>();
            List<Vector2> TexCoords = new List<Vector2>();
            List<Vector3> Normals = new List<Vector3>();

            List<Vector3i> PositionIndices = new List<Vector3i>();
            List<Vector3i> TexCoordIndices = new List<Vector3i>();
            List<Vector3i> NormalIndices = new List<Vector3i>();

            List<Vector3> groupPositions = new List<Vector3>();
            List<Vector2> groupTexCoords = new List<Vector2>();
            List<Vector3> groupNormals = new List<Vector3>();

            int vertexCount = 0;

            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].StartsWith("mtllib"))
                {
                    mtlname = Data[i];
                    mtlname = mtlname.Remove(0, 7);
                    Console.WriteLine("MTL File: " + mtlname);
                }

                if (Data[i].StartsWith("o"))
                {
                    ObjectName = Data[i];
                    ObjectName = ObjectName.Remove(0, 2);
                    Console.WriteLine("Object Name: " + ObjectName);
                }

                if (Data[i].StartsWith("v "))
                {
                    vertexCount += 1;
                    string[] posstring;
                    float[] posfloat = new float[3];

                    Data[i] = Data[i].Remove(0, 2);
                    posstring = Data[i].Split(" ", 3);

                    posfloat[0] = float.Parse(posstring[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    posfloat[1] = float.Parse(posstring[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    posfloat[2] = float.Parse(posstring[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                    Vector3 position = new Vector3(posfloat[0], posfloat[1], posfloat[2]);
                    Positions.Add(position);
                }

                if (Data[i].StartsWith("vn"))
                {
                    string[] normstring;
                    float[] normfloat = new float[3];

                    Data[i] = Data[i].Remove(0, 3);
                    normstring = Data[i].Split(" ", 3);

                    normfloat[0] = float.Parse(normstring[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    normfloat[1] = float.Parse(normstring[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    normfloat[2] = float.Parse(normstring[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                    Vector3 normal = new Vector3(normfloat[0], normfloat[1], normfloat[2]);
                    Normals.Add(normal);
                }

                if (Data[i].StartsWith("vt"))
                {
                    string[] texstring;
                    float[] texfloat = new float[2];

                    Data[i] = Data[i].Remove(0, 3);
                    texstring = Data[i].Split(" ", 2);

                    texfloat[0] = float.Parse(texstring[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    texfloat[1] = float.Parse(texstring[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                    Vector2 texCoord = new Vector2(texfloat[0], texfloat[1]);
                    TexCoords.Add(texCoord);
                }

                if (Data[i].StartsWith("f"))
                {
                    string[] indices;

                    string[] row1;
                    string[] row2;
                    string[] row3;

                    Data[i] = Data[i].Remove(0, 2);
                    indices = Data[i].Split(" ", 3);
                    row1 = indices[0].Split("/", 3);
                    row2 = indices[1].Split("/", 3);
                    row3 = indices[2].Split("/", 3);

                    PositionIndices.Add(new Vector3i(
                        int.Parse(row1[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(row2[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(row3[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture)
                        ));

                    TexCoordIndices.Add(new Vector3i(
                        int.Parse(row1[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(row2[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(row3[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture)
                        ));

                    NormalIndices.Add(new Vector3i(
                        int.Parse(row1[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(row2[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(row3[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture)
                        ));

                    /*
                    positionindices.Y = int.Parse(row2[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    positionindices.Z = int.Parse(row3[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                    textureindices.X = int.Parse(row1[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    textureindices.Y = int.Parse(row2[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    textureindices.Z = int.Parse(row3[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                    normalindices.X = int.Parse(row1[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    normalindices.Y = int.Parse(row2[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    normalindices.Z = int.Parse(row3[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                    */

                    //PositionIndices.Add(positionindices);
                    //TexCoordIndices.Add(textureindices);
                    //NormalIndices.Add(normalindices);

                    //Console.WriteLine(Positions[0]);

                    /*Console.WriteLine(
                        groupPositions[0].X.ToString() + groupPositions[0].Y.ToString() + groupPositions[0].Z.ToString() + " " +
                        groupTexCoords[0].X.ToString() + groupTexCoords[0].Y.ToString() + " " +
                        groupNormals[0].X.ToString() + groupNormals[0].Y.ToString() + groupNormals[0].Z.ToString());
                    */
                }
            }

            Console.WriteLine(PositionIndices.Count);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(Positions[PositionIndices[i].X].X.ToString() + " " + Positions[PositionIndices[i].Y].Y.ToString() + " "+ Positions[PositionIndices[i].Z].Z.ToString());
            }

            for (int i = 0; i < groupPositions.Count; i++)
            {
                groupPositions.Add(new Vector3(
                    Positions[PositionIndices[i].X].X +
                    Positions[PositionIndices[i].Y].Y +
                    Positions[PositionIndices[i].Z].Z));

                groupTexCoords.Add(new Vector2(
                    TexCoords[TexCoordIndices[i].X].X +
                    TexCoords[TexCoordIndices[i].Y].Y));

                groupNormals.Add(new Vector3(
                    Normals[NormalIndices[i].X].X +
                    Normals[NormalIndices[i].Y].Y +
                    Normals[NormalIndices[i].Z].Z));
            }
        }
    }
}
