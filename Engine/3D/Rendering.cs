using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using static Engine.SettingUP.Setup;
using static Engine.Main;
using System;

// Mostly used in OnRenderFrame();
namespace Engine.RenderEngine
{
    class Rendering
    {
        public static int numPL;

        public static Shader PBRShader = new Shader(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/PBR/pbr.vert", AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/PBR/pbr.frag");
        public static Shader LightShader = new Shader(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/PBR/light.vert", AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/PBR/light.frag");
        public static Shader WireframeShader = new Shader(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/Misc/Wireframe.vert", AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/Misc/Wireframe.frag");

        public static System.Numerics.Vector3 BG_Color = new System.Numerics.Vector3(0f);
        public static int selectedObject = 1;
        public static int selectedLight = 0;

        public static float NoiseAmount = 0.5f;
        public static System.Numerics.Vector3 LightDirection = new System.Numerics.Vector3(-1, 1, 1);
        public static System.Numerics.Vector3 LightColor = new System.Numerics.Vector3(1);

        // Post processing
        public static bool ChromaticAbberationOnOff = false;
        public static float ChromaticAbberationOffset = 0.005f;
        public static float exposure = 1;
        public static bool showCubeMap = false;

        // Draw the array of objects
        public static void DrawObjects(Matrix4 projection, Matrix4 view, bool overrideShader)
        {
            if (overrideShader == false)
            {
                for (int i = 1; i < Objects.Count; i++)
                {
                    PBRShader.Use();
                    GL.BindVertexArray(VAO[i]);
                    SetTransform(PBRShader, MakeTransform(
                        Objects[i].Scale * Objects[0].Scale,
                        Objects[i].Location + Objects[0].Location,
                        Objects[i].Rotation + Objects[0].Rotation));
                    SetProjView(PBRShader, projection, view);

                    Vector3 ambient = new Vector3(BG_Color.X, BG_Color.Y, BG_Color.Z);
                    PBRShader.SetVector3("material.ambient", ambient);
                    PBRShader.SetVector3("viewPos", position);

                    PBRShader.SetVector3("material.albedo", Objects[i].Material.albedo);
                    PBRShader.SetFloat("material.roughness", Objects[i].Material.roughness);
                    PBRShader.SetFloat("material.metallic", Objects[i].Material.metallic);
                    PBRShader.SetFloat("material.ao", Objects[i].Material.ao);

                    PBRShader.SetInt("material.albedoTex", 0);
                    PBRShader.SetInt("material.roughnessTex", 1);
                    PBRShader.SetInt("material.metallicTex", 2);
                    PBRShader.SetInt("material.normalTex", 3);
                    PBRShader.SetInt("material.ao", 4);

                    if (Objects[i].Material.Maps[0] != 0) PBRmaps[0].Use(TextureUnit.Texture0);
                    else DefaultMaps[0].Use(TextureUnit.Texture0);

                    if (Objects[i].Material.Maps[1] != 0) PBRmaps[1].Use(TextureUnit.Texture1);
                    else DefaultMaps[1].Use(TextureUnit.Texture1);

                    if (Objects[i].Material.Maps[2] != 0) PBRmaps[2].Use(TextureUnit.Texture2);
                    else DefaultMaps[2].Use(TextureUnit.Texture2);

                    if (Objects[i].Material.Maps[3] != 0) PBRmaps[3].Use(TextureUnit.Texture3);
                    else DefaultMaps[3].Use(TextureUnit.Texture3);

                    if (Objects[i].Material.Maps[4] != 0) PBRmaps[4].Use(TextureUnit.Texture4);
                    else DefaultMaps[4].Use(TextureUnit.Texture4);

                    PBRShader.SetVector3("dirLight.direction", new Vector3(LightDirection.X, LightDirection.Y, LightDirection.Z));
                    PBRShader.SetVector3("dirLight.color", new Vector3(LightColor.X, LightColor.Y, LightColor.Z));

                    numPL = 0;

                    // Lights
                    for (int j = 0; j < Lights.Count; j++)
                    {
                        switch (Lights[j].Type)
                        {
                            // Set each Point Light in FS
                            case 0:
                                numPL += 1;
                                PBRShader.SetVector3("pointLights[" + j + "].lightColor", Lights[j].LightColor);
                                PBRShader.SetVector3("pointLights[" + j + "].lightPos", Lights[j].Location);
                                PBRShader.SetFloat("pointLights[" + j + "].strength", Lights[j].Strength);
                                //PBRShader.SetFloat("pointLights[" + j + "].radius", Lights[j].Radius);
                                //PBRShader.SetFloat("pointLights[" + j + "].falloff", Lights[j].FallOff);
                                break;

                            case 1:
                                break;
                        }
                    }

                    // Set the for loop length in FS shader
                    PBRShader.SetInt("NR_PointLights", numPL);

                    // Draw objects with indices
                    GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }

            else
            {
                for (int i = 1; i < Objects.Count; i++)
                {
                    WireframeShader.Use();
                    GL.BindVertexArray(VAO[i]);
                    SetTransform(WireframeShader, MakeTransform(
                        Objects[i].Scale * Objects[0].Scale,
                        Objects[i].Location + Objects[0].Location,
                        Objects[i].Rotation + Objects[0].Rotation));
                    SetProjView(WireframeShader, projection, view);

                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    WireframeShader.SetVector3("col", new Vector3(1));
                    GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);

                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    WireframeShader.SetVector3("col", new Vector3(0.3f));
                    GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        // Draw the array of lights
        public static void DrawLights(Matrix4 projection, Matrix4 view)
        {
            for (int i = 0; i < Lights.Count; i++)
            {
                PointLightTexture.Use(TextureUnit.Texture0);

                LightShader.Use();
                GL.BindVertexArray(VAOlights[i]);
                SetTransform(LightShader, MakeLightTransform(Lights[i].Location, Lights[i].Rotation));
                SetProjView(LightShader, projection, view);
                GL.Uniform3(LightShader.GetUniformLocation("lightcolor"), Lights[i].LightColor);
                LightShader.SetInt("lightTexture", 0);

                GL.DrawElements(PrimitiveType.Triangles, Lights[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        public static void SetProjView(Shader shader, Matrix4 projection, Matrix4 view)
        {
            GL.UniformMatrix4(shader.GetUniformLocation("projection"), true, ref projection);
            GL.UniformMatrix4(shader.GetUniformLocation("view"), true, ref view);
        }

        public static void SetTransform(Shader shader, Matrix4 transform)
        {
            GL.UniformMatrix4(shader.GetUniformLocation("transform"), true, ref transform);
        }

        static Matrix4 MakeTransform(Vector3 scale, Vector3 location, Vector3 rotation)
        {
            var transform = Matrix4.CreateScale(scale);
            Quaternion QuatRot = new Quaternion(
                MathHelper.DegreesToRadians(rotation.X),
                MathHelper.DegreesToRadians(rotation.Y),
                MathHelper.DegreesToRadians(rotation.Z));
            Matrix4 Mat4Rot = Matrix4.CreateFromQuaternion(QuatRot);
            transform *= Mat4Rot;
            transform *= Matrix4.CreateTranslation(location);

            return transform;
        }

        static Matrix4 MakeLightTransform(Vector3 location, Vector3 rotation)
        {
            var transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale((location - position).Length * 0.04f);
            transform *=
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Math.Clamp(Yaw, -89, 89))) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Pitch - 90)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0));
            transform *= Matrix4.CreateTranslation(location);

            return transform;
        }

        public static Shader fboShader = new Shader(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/Misc/framebuffer.vert", AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/shaders/Misc/framebuffer.frag");
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
            fboShader.SetBool("ChromaticAbberationOnOff", ChromaticAbberationOnOff);
            fboShader.SetFloat("ChromaticAbberationOffset", ChromaticAbberationOffset);
            fboShader.SetFloat("exposure", exposure);
        }
    }
}