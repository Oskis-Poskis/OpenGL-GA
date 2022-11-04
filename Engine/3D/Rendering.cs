using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using Axyz;

using static Bernard.Setup;

// The Maeve namespace contains the drawing of entities, such as objects and lights
// Mostly used in OnRenderFrame();
namespace Maeve
{
    class Rendering
    {
        public static int numPL;

        // Draw the array of objects
        public static void DrawObjects(Matrix4 projection, Matrix4 view, bool overrideShader)
        {
            if (overrideShader == false)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    Main.PBRShader.Use();
                    GL.BindVertexArray(VAO[i]);
                    SetTransform(Main.PBRShader, MakeTransform(Objects[i].Scale, Objects[i].Location, Objects[i].Rotation));
                    SetProjView(Main.PBRShader, projection, view);

                    Vector3 ambient = new Vector3(Main.BG_Color.X, Main.BG_Color.Y, Main.BG_Color.Z);
                    Main.PBRShader.SetVector3("material.ambient", ambient);
                    Main.PBRShader.SetVector3("viewPos", Main.position);

                    Main.PBRShader.SetVector3("material.albedo", Objects[i].Material.albedo);
                    Main.PBRShader.SetFloat("material.roughness", Objects[i].Material.roughness);
                    Main.PBRShader.SetFloat("material.metallic", Objects[i].Material.metallic);
                    Main.PBRShader.SetFloat("material.ao", Objects[i].Material.ao);

                    Main.PBRShader.SetInt("material.albedoTex", 0);
                    Main.PBRShader.SetInt("material.roughnessTex", 1);
                    Main.PBRShader.SetInt("material.metallicTex", 2);
                    //Main.PBRShader.SetInt("material.normalTex", 3);
                    Main.PBRShader.SetInt("material.ao", 4);

                    if (Objects[i].Material.Maps[0] != 0) Main.PBRmaps[0].Use(TextureUnit.Texture0);
                    else Main.DefaultMaps[0].Use(TextureUnit.Texture0);

                    if (Objects[i].Material.Maps[1] != 0) Main.PBRmaps[1].Use(TextureUnit.Texture1);
                    else Main.DefaultMaps[1].Use(TextureUnit.Texture1);

                    if (Objects[i].Material.Maps[2] != 0) Main.PBRmaps[2].Use(TextureUnit.Texture2);
                    else Main.DefaultMaps[2].Use(TextureUnit.Texture2);

                    //if (Objects[i].Material.Maps[3] != 0) Main.PBRmaps[3].Use(TextureUnit.Texture3);
                    //else Main.DefaultMaps[3].Use(TextureUnit.Texture3);

                    if (Objects[i].Material.Maps[4] != 0) Main.PBRmaps[4].Use(TextureUnit.Texture4);
                    else Main.DefaultMaps[4].Use(TextureUnit.Texture4);

                    Main.PBRShader.SetVector3("dirLight.direction", new Vector3(Main.LightDirection.X, Main.LightDirection.Y, Main.LightDirection.Z));
                    Main.PBRShader.SetVector3("dirLight.color", new Vector3(Main.LightColor.X, Main.LightColor.Y, Main.LightColor.Z));

                    numPL = 0;

                    // Lights
                    for (int j = 0; j < Lights.Count; j++)
                    {
                        switch (Lights[j].Type)
                        {
                            // Set each Point Light in FS
                            case 0:
                                numPL += 1;
                                Main.PBRShader.SetVector3("pointLights[" + j + "].lightColor", Lights[j].LightColor);
                                Main.PBRShader.SetVector3("pointLights[" + j + "].lightPos", Lights[j].Location);
                                Main.PBRShader.SetFloat("pointLights[" + j + "].strength", Lights[j].Strength);
                                Main.PBRShader.SetFloat("pointLights[" + j + "].radius", Lights[j].Radius);
                                Main.PBRShader.SetFloat("pointLights[" + j + "].falloff", Lights[j].FallOff);
                                break;

                            case 1:
                                break;
                        }
                    }


                    // Set the for loop length in FS shader
                    Main.PBRShader.SetInt("NR_PointLights", numPL);

                    // Draw objects with indices
                    GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }

            else
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    Main.WireframeShader.Use();
                    GL.BindVertexArray(VAO[i]);
                    SetTransform(Main.WireframeShader, MakeTransform(Objects[i].Scale, Objects[i].Location, Objects[i].Rotation));
                    SetProjView(Main.WireframeShader, projection, view);

                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    Main.WireframeShader.SetVector3("col", new Vector3(1));
                    GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);

                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    Main.WireframeShader.SetVector3("col", new Vector3(0.3f));
                    GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        // Draw the array of lights
        public static void DrawLights(Matrix4 projection, Matrix4 view)
        {
            for (int i = 0; i < Lights.Count; i++)
            {
                Lights[i].Shader.Use();
                GL.BindVertexArray(VAOlights[i]);
                SetTransform(Lights[i].Shader, MakeLightTransform(new Vector3(1.0f), Lights[i].Location, Lights[i].Rotation));
                SetProjView(Lights[i].Shader, projection, view);
                GL.Uniform3(Lights[i].Shader.GetUniformLocation("lightcolor"), Lights[i].LightColor);

                GL.DrawElements(PrimitiveType.Triangles, Lights[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        // Set uniforms in shader
        public static void SetProjView(Shader shader, Matrix4 projection, Matrix4 view)
        {
            GL.UniformMatrix4(shader.GetUniformLocation("projection"), true, ref projection);
            GL.UniformMatrix4(shader.GetUniformLocation("view"), true, ref view);
        }

        // Set transform in shader
        public static void SetTransform(Shader shader, Matrix4 transform)
        {
            GL.UniformMatrix4(shader.GetUniformLocation("transform"), true, ref transform);
        }

        // Create a transform with scale, location and rotation
        public static Matrix4 MakeTransform(Vector3 scale, Vector3 location, Vector3 rotation)
        {
            var transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale(scale);
            transform *=
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            transform *= Matrix4.CreateTranslation(location);
            
            return transform;
        }

        public static Matrix4 MakeLightTransform(Vector3 scale, Vector3 location, Vector3 rotation)
        {
            var transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale(scale);
            transform *=
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            //transform *= Matrix4.Invert(Matrix4.LookAt(location, Main.position, Vector3.UnitY));
            transform *= Matrix4.CreateTranslation(location);

            return transform;
        }

        public static Shader fboShader = new Shader("./../../../Engine/Engine_Resources/shaders/Misc/framebuffer.vert", "./../../../Engine/Engine_Resources/shaders/Misc/framebuffer.frag");
        public static int rectVAO, rectVBO;
        static readonly float[] rectVerts = new float[]
        {
             1.0f, -1.0f,  1.0f, 0.0f,
            -1.0f, -1.0f,  0.0f, 0.0f,
            -1.0f,  1.0f,  0.0f, 1.0f,

             1.0f,  1.0f,  1.0f, 1.0f,
             1.0f, -1.0f,  1.0f, 0.0f,
            -1.0f,  1.0f,  0.0f, 1.0f
        };

        public static void GenScreenRect()
        {
            fboShader.Use();

            rectVAO = GL.GenVertexArray();
            GL.BindVertexArray(rectVAO);
            rectVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, rectVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, rectVerts.Length * sizeof(float), rectVerts, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(fboShader.GetAttribLocation("aPos"));
            GL.VertexAttribPointer(fboShader.GetAttribLocation("aPos"), 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(fboShader.GetAttribLocation("aTexCoord"));
            GL.VertexAttribPointer(fboShader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.Uniform1(fboShader.GetUniformLocation("framebufferTexture"), 0);

            // PP
            fboShader.SetBool("ChromaticAbberationOnOff", Main.ChromaticAbberationOnOff);
            fboShader.SetFloat("ChromaticAbberationOffset", Main.ChromaticAbberationOffset);
        }
    }
}