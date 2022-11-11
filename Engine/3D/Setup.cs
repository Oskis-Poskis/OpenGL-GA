using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;
using System.IO;
using System;

using static Engine.Importer.Import;
using static Engine.RenderEngine.Rendering;
using System.Windows.Forms;

// Mostly used in OnLoad();
namespace Engine.SettingUP
{
    class Setup
    {
        public struct VertexData
        {
            public Vector3 Position;
            public Vector2 texCoord;
            public Vector3 Normals;
            public Vector3 Tangents;
            public Vector3 BiTangents;

            public VertexData(Vector3 position, Vector2 texCoord, Vector3 normals, Vector3 tangents, Vector3 bitangents)
            {
                this.Position = position;
                this.texCoord = texCoord;
                this.Normals = normals;
                this.Tangents = tangents;
                this.BiTangents = bitangents;
            }
        }

        public struct VertPosData
        {
            public Vector3 Position;
            public VertPosData(Vector3 position)
            {
                this.Position = position;
            }
        }

        // Material struct
        public class Material
        {
            public Vector3 albedo;
            public float roughness;
            public float metallic;
            public float ao;
            public int[] Maps;
        }

        // Struct with object data
        public class Object
        {
            public string Name;
            public string ID;
            public Material Material;
            public VertexData[] VertData;
            public int[] Indices;
            public Vector3 Location;
            public Vector3 Rotation;
            public Vector3 Scale;
        }

        // Struct with light data
        public class Light
        {
            public float Strength;
            public float Radius;
            public float FallOff;
            public int Type;
            public string Name;
            public string ID;
            public Vector3 LightColor;
            public Shader Shader;
            public VertPosData[] LightVertData;
            public int[] Indices;
            public Vector3 Location;
            public Vector3 Rotation;
        }

        public static List<Object> Objects = new List<Object>();
        public static List<int> VAO = new List<int>();
        public static List<Light> Lights = new List<Light>();
        public static List<int> VAOlights = new List<int>();

        // Add 3D object to rendering list
        public static void AddObjectToArray(string name, Material material, Vector3 scale, Vector3 location, Vector3 rotation, VertexData[] vertices, int[] indices)
        {
            Object _object = new Object
            {
                ID = MathLib.Functions.RandInt(0, 100000).ToString(),
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
        public static void AddLightToArray(float strength, float radius, float falloff, int type, string name, Vector3 lightColor, Shader shader, Vector3 location, Vector3 rotation, VertPosData[] vertices, int[] indices)
        {
            Light _light = new Light
            {
                Strength = strength,
                Radius = radius,
                FallOff = falloff,
                Type = type,
                Name = name,
                ID = MathLib.Functions.RandInt(0, 100000).ToString(),
                LightColor = lightColor,
                Shader = shader,
                LightVertData = vertices,
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
                // Generate and bind Vertex Array
                VAO[i] = GL.GenVertexArray();
                GL.BindVertexArray(VAO[i]);
                // Generate and bind Vertex Buffere
                int VBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, Objects[i].VertData.Length * 14 * sizeof(float), Objects[i].VertData, BufferUsageHint.StaticDraw);
                // Generate and bind Element Buffer
                int EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Objects[i].Indices.Length * sizeof(uint), Objects[i].Indices, BufferUsageHint.StaticDraw);

                // Set attributes in shaders - vertex positions, UV's and normals
                GL.EnableVertexAttribArray(PBRShader.GetAttribLocation("aPosition"));
                GL.VertexAttribPointer(PBRShader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 0);
                GL.EnableVertexAttribArray(PBRShader.GetAttribLocation("aTexCoord"));
                GL.VertexAttribPointer(PBRShader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 14 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(PBRShader.GetAttribLocation("aNormal"));
                GL.VertexAttribPointer(PBRShader.GetAttribLocation("aNormal"), 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 5 * sizeof(float));
                GL.EnableVertexAttribArray(PBRShader.GetAttribLocation("aTangent"));
                GL.VertexAttribPointer(PBRShader.GetAttribLocation("aTangent"), 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 8 * sizeof(float));
                GL.EnableVertexAttribArray(PBRShader.GetAttribLocation("aBiTangent"));
                GL.VertexAttribPointer(PBRShader.GetAttribLocation("aBiTangent"), 3, VertexAttribPointerType.Float, false, 14 * sizeof(float), 11 * sizeof(float));
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
                GL.BufferData(BufferTarget.ArrayBuffer, Lights[i].LightVertData.Length * 3 * sizeof(float), Lights[i].LightVertData, BufferUsageHint.StaticDraw);
                // Generate and bind Element Buffer
                int EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Lights[i].Indices.Length * sizeof(uint), Lights[i].Indices, BufferUsageHint.StaticDraw);

                // Set attributes in shaders - vertex positions, UV's and normals
                GL.EnableVertexAttribArray(Lights[i].Shader.GetAttribLocation("aPosition"));
                GL.VertexAttribPointer(Lights[i].Shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            }
        }

        public static Texture[] DefaultMaps = new Texture[5];

        public static void LoadDefaultMaps()
        {
            DefaultMaps[0] = Texture.LoadFromFile("./../../../Engine/Engine_Resources/Images/White1x1.png", TextureUnit.Texture0);
            DefaultMaps[1] = Texture.LoadFromFile("./../../../Engine/Engine_Resources/Images/White1x1.png", TextureUnit.Texture0);
            DefaultMaps[2] = Texture.LoadFromFile("./../../../Engine/Engine_Resources/Images/White1x1.png", TextureUnit.Texture0);
            DefaultMaps[3] = Texture.LoadFromFile("./../../../Engine/Engine_Resources/Images/Normal1x1.png", TextureUnit.Texture0);
            DefaultMaps[4] = Texture.LoadFromFile("./../../../Engine/Engine_Resources/Images/White1x1.png", TextureUnit.Texture0);
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
        static int[] CubeMapIndices;
        static VertexData[] CubeMapData;
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
                using Stream stream = File.OpenRead(cubeMapTextureString[i]);
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
            }

            LoadModel("./../../../Engine/Engine_Resources/Primitives/CubeMapMesh.fbx");
            CubeMapData = importedData;
            CubeMapIndices = importindices;

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

        static int gridVAO;
        static int[] gridIndices;
        static VertPosData[] gridData;
        public static void SetupGrid()
        {
            LoadModel("./../../../Engine/Engine_Resources/Primitives/FloorGrid.fbx", true);
            gridData = importedLightData;
            gridIndices = importindices;

            WireframeShader.Use();

            gridVAO = GL.GenVertexArray();
            GL.BindVertexArray(gridVAO);
            // Generate and bind Vertex Buffere
            int VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, gridData.Length * 3 * sizeof(float), gridData, BufferUsageHint.StaticDraw);
            // Generate and bind Element Buffer
            int EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, gridIndices.Length * sizeof(uint), gridIndices, BufferUsageHint.StaticDraw);

            // Set attributes in shaders - vertex positions, UV's and normals
            GL.EnableVertexAttribArray(WireframeShader.GetAttribLocation("aPosition"));
            GL.VertexAttribPointer(WireframeShader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public static void DrawGrid(Matrix4 projection, Matrix4 view)
        {
            WireframeShader.Use();
            GL.BindVertexArray(gridVAO);

            SetProjView(CubeMapShader, projection, view);
            Matrix4 GridTrans = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90));
            GridTrans *= Matrix4.CreateTranslation(0, -0.5f, 0);
            GridTrans *= Matrix4.CreateTranslation(Vector3.Zero);

            WireframeShader.SetVector3("col", new Vector3(0.7f));
            GL.UniformMatrix4(WireframeShader.GetUniformLocation("transform"), true, ref GridTrans);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);
            GL.DrawElements(PrimitiveType.Triangles, gridIndices.Length, DrawElementsType.UnsignedInt, 0);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public static int FBO, RBO;
        public static int framebufferTexture, depthTexture;

        public static void GenFBO(float CameraWidth, float CameraHeight)
        {
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Color Texture
            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            // Attach color to FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, framebufferTexture, 0);

            // Depth Texture
            depthTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            // Attach Depth to FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTexture, 0);

            /*
            RBO = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, (int)CameraWidth, (int)CameraHeight);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);
            */

            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus);
            }
        }
    }
}
