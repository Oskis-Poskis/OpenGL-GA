using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;
using System.IO;

namespace OpenTK_Learning
{
    class R_3D
    {
        public static List<Object> Objects = new List<Object>();
        public static List<int> VAO = new List<int>();
        public static List<Light> Lights = new List<Light>();
        public static List<int> VAOlights = new List<int>();

        // Material struct
        public struct Material
        {
            public Vector3 albedo;
            public float roughness;
            public float metallic;
        }

        // Struct with object data
        public struct Object
        {
            public string Name;
            public string ID;
            public Material Material;
            public VertexData[] VertData;
            public int[] Indices;
            public Vector3 Location;
            public Vector3 Rotation;
            public Vector3 Scale;
            public bool RelTransform;
        }

        // Struct with light data
        public struct Light
        {
            public float Strength;
            public float Radius;
            public float FallOff;
            public int Type;
            public string Name;
            public string ID;
            public Vector3 LightColor;
            public Shader Shader;
            public VertexData[] VertData;
            public int[] Indices;
            public Vector3 Location;
            public Vector3 Rotation;
        }

        // Add 3D object to rendering list
        public static void AddObjectToArray(bool _rel, string name, Material material, Vector3 scale, Vector3 location, Vector3 rotation, VertexData[] vertices, int[] indices)
        {
            Object _object = new Object
            {
                RelTransform = _rel,
                ID = Math_Functions.RandInt(0, 100000).ToString(),
                Name = name,
                Material = material,
                VertData = vertices,
                Indices = indices,
                Location = location,
                Rotation = rotation,
                Scale = scale
            };
            VAO.Add(0);
            Objects.Add(_object);
        }

        // Add light to rendering list
        public static void AddLightToArray(float strength, float radius, float falloff, int type, string name, Vector3 lightColor, Shader shader, Vector3 location, Vector3 rotation, VertexData[] vertices, int[] indices)
        {
            Light _light = new Light
            {
                Strength = strength,
                Radius = radius,
                FallOff = falloff,
                Type = type,
                Name = name,
                ID = Math_Functions.RandInt(0, 100000).ToString(),
                LightColor = lightColor,
                Shader = shader,
                VertData = vertices,
                Indices = indices,
                Location = location,
                Rotation = rotation,
            };
            VAOlights.Add(0);
            Lights.Insert(0, _light);
        }

        // Create buffers for all objects in array
        public static void ConstructObjects()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Console.WriteLine(VAO.Count);
                // Generate and bind Vertex Array
                VAO[i] = GL.GenVertexArray();
                GL.BindVertexArray(VAO[i]);
                // Generate and bind Vertex Buffere
                int VBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, Objects[i].VertData.Length * 8 * sizeof(float), Objects[i].VertData, BufferUsageHint.StaticDraw);
                // Generate and bind Element Buffer
                int EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Objects[i].Indices.Length * sizeof(uint), Objects[i].Indices, BufferUsageHint.StaticDraw);

                // Set attributes in shaders - vertex positions, UV's and normals
                GL.EnableVertexAttribArray(Main.PBRShader.GetAttribLocation("aPosition"));
                GL.VertexAttribPointer(Main.PBRShader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(Main.PBRShader.GetAttribLocation("aTexCoord"));
                GL.VertexAttribPointer(Main.PBRShader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(Main.PBRShader.GetAttribLocation("aNormal"));
                GL.VertexAttribPointer(Main.PBRShader.GetAttribLocation("aNormal"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            }
        }

        // Create buffers for all lights in array
        public static void ConstructLights()
        {
            for (int i = 0; i < Lights.Count; i++)
            {
                // Generate and bind Vertex Array
                VAOlights[i] = GL.GenVertexArray();
                GL.BindVertexArray(VAOlights[i]);
                // Generate and bind Vertex Buffere
                int VBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, Lights[i].VertData.Length * 8 * sizeof(float), Lights[i].VertData, BufferUsageHint.StaticDraw);
                // Generate and bind Element Buffer
                int EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Lights[i].Indices.Length * sizeof(uint), Lights[i].Indices, BufferUsageHint.StaticDraw);

                // Set attributes in shaders - vertex positions, UV's and normals
                GL.EnableVertexAttribArray(Lights[i].Shader.GetAttribLocation("aPosition"));
                GL.VertexAttribPointer(Lights[i].Shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            }
        }

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
                    SetTransform(Main.PBRShader, MakeTransform(i, Objects[i].Scale, Objects[i].Location, Objects[i].Rotation));
                    SetProjView(Main.PBRShader, projection, view);

                    Vector3 ambient = new Vector3(Main.BG_Color.X, Main.BG_Color.Y, Main.BG_Color.Z);
                    Main.PBRShader.SetVector3("material.ambient", ambient);
                    Main.PBRShader.SetVector3("viewPos", Main.position);

                    Main.PBRShader.SetVector3("material.albedo", Objects[i].Material.albedo);
                    Main.PBRShader.SetFloat("material.roughness", Objects[i].Material.roughness);
                    Main.PBRShader.SetFloat("material.metallic", Objects[i].Material.metallic);

                    Main.PBRShader.SetInt("material.albedoTex", 0);
                    Main.PBRShader.SetInt("material.roughnessTex", 1);
                    Main.PBRShader.SetInt("material.metallicTex", 2);
                    //Main.PBRShader.SetInt("material.normalTex", 3);

                    Main.PhongShader.SetVector3("dirLight.direction", new Vector3(-1, 1, 1));
                    Main.PhongShader.SetVector3("dirLight.color", new Vector3(1));
                    //Main.PhongShader.SetFloat("dirLight.strength", 0.75f);

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
                                //Main.PhongShader.SetFloat("pointLights[" + j + "].strength", Lights[j].Strength);
                                //Main.PhongShader.SetFloat("pointLights[" + j + "].radius", Lights[j].Radius);
                                //Main.PhongShader.SetFloat("pointLights[" + j + "].compression", Lights[j].FallOff);
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
                    SetTransform(Main.WireframeShader, MakeTransform(i, Objects[i].Scale, Objects[i].Location, Objects[i].Rotation));
                    SetProjView(Main.WireframeShader, projection, view);

                    // Draw objects with indices
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
        public static Matrix4 MakeTransform(int index, Vector3 scale, Vector3 location, Vector3 rotation)
        {
            var transform = Matrix4.Identity;
            if (Objects[index].RelTransform == true)
            {
                transform *= Matrix4.CreateScale(scale);
                transform *=
                    Matrix4.CreateFromAxisAngle(location, MathHelper.DegreesToRadians(rotation.X)) *
                    Matrix4.CreateFromAxisAngle(location, MathHelper.DegreesToRadians(rotation.Y)) *
                    Matrix4.CreateFromAxisAngle(location, MathHelper.DegreesToRadians(rotation.Z));
                transform *= Matrix4.CreateTranslation(location);
                /*
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                    Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                    Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
                */
            }

            else
            {
                transform *= Matrix4.CreateScale(scale);
                transform *=
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                    Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                    Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
                transform *= Matrix4.CreateTranslation(location);
            }

            return transform;
        }

        public static Matrix4 MakeLightTransform(Vector3 scale, Vector3 location, Vector3 rotation)
        {
            var transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale(scale);
            /*transform *=
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));*/
            transform *= Matrix4.Invert(Matrix4.LookAt(location, Main.position, Vector3.UnitY));
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

        public static int SRFBO, FBO, RBO;
        public static int SRtexture, framebufferTexture;

        public static void GenFBO(float CameraWidth, float CameraHeight)
        {
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Color texture
            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            // Attach color to FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, framebufferTexture, 0);

            RBO = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, (int)CameraWidth, (int)CameraHeight);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);

            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus);
            }

            /*
            SRFBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, SRFBO);

            // Color texture
            SRtexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, SRtexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            // Attach color to FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, SRtexture, 0);

            var fboStatus2 = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus2 != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus2);
            }
            */
        }

        static readonly string[] cubeMapTextureString = new string[6]
        {
            "./../../../Engine/Engine_Resources/Images/CubeMap2/px.png",
            "./../../../Engine/Engine_Resources/Images/CubeMap2/nx.png",
            "./../../../Engine/Engine_Resources/Images/CubeMap2/py.png",
            "./../../../Engine/Engine_Resources/Images/CubeMap2/ny.png",
            "./../../../Engine/Engine_Resources/Images/CubeMap2/pz.png",
            "./../../../Engine/Engine_Resources/Images/CubeMap2/nz.png",
        };

        public static Shader CubeMapShader = new Shader("./../../../Engine/Engine_Resources/shaders/Misc/CubeMap.vert", "./../../../Engine/Engine_Resources/shaders/Misc/CubeMap.frag");

        static int CubeMapVAO;
        static VertexData[] CubeMapData;
        static int[] CubeMapIndices;

        public static int cubeMapTexture;

        public static void SetUpCubeMap()
        {
            cubeMapTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTexture);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            for (int i = 0; i < 6; i++)
            {
                StbImage.stbi_set_flip_vertically_on_load(0);
                using (Stream stream = File.OpenRead(cubeMapTextureString[i]))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
                }
            }

            R_Loading.LoadModel("./../../../Engine/Engine_Resources/Primitives/CubeMapMesh.fbx");
            CubeMapData = R_Loading.importedData;
            CubeMapIndices = R_Loading.importindices;

            CubeMapShader.Use();
            CubeMapShader.SetInt("skybox", 0);

            CubeMapVAO = GL.GenVertexArray();
            GL.BindVertexArray(CubeMapVAO);
            // Generate and bind Vertex Buffere
            int VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, CubeMapData.Length * 8 * sizeof(float), CubeMapData, BufferUsageHint.StaticDraw);
            // Generate and bind Element Buffer
            int EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, CubeMapIndices.Length * sizeof(uint), CubeMapIndices, BufferUsageHint.StaticDraw);

            // Set attributes in shaders - vertex positions, UV's and normals
            GL.EnableVertexAttribArray(CubeMapShader.GetAttribLocation("aPosition"));
            GL.VertexAttribPointer(CubeMapShader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }

        public static void DrawCubeMapCube(Matrix4 projection, Matrix4 view, Vector3 position)
        {
            CubeMapShader.Use();
            GL.BindVertexArray(CubeMapVAO);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTexture);

            SetProjView(CubeMapShader, projection, view);
            Matrix4 CubeMapTransform = Matrix4.CreateScale(50);
            CubeMapTransform *= Matrix4.CreateTranslation(position);

            GL.UniformMatrix4(CubeMapShader.GetUniformLocation("transform"), true, ref CubeMapTransform);
            GL.DrawElements(PrimitiveType.Triangles, CubeMapIndices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}