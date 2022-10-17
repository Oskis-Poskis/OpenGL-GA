using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenTK_Learning
{
    class R_3D
    {
        public static List<Object> Objects = new List<Object>();
        public static int[] VAO = new int[10];
        public static List<Light> Lights = new List<Light>();
        public static int[] VAOlights = new int[10];

        // Material struct
        public struct Material
        {
            public Vector3 ambient;
            public Vector3 diffuse;
            public Vector3 specular;
            public float shininess;
        }

        // Struct with object data to assign in the array of objects
        public struct Object
        {
            public string Name;
            public Material Material;
            public VertexData[] VertData;
            public int[] Indices;
            public Vector3 Location;
            public Vector3 Rotation;
            public Vector3 Scale;
            public bool RelTransform;
        }

        public struct Light
        {
            public string Name;
            public Vector3 LightColor;
            public Shader Shader;
            public VertexData[] VertData;
            public int[] Indices;
            public Vector3 Location;
            public Vector3 Rotation;
            public Vector3 Scale;
        }

        public static void AddObjectToArray(bool _rel, string name, Material material, Vector3 scale, Vector3 location, Vector3 rotation, VertexData[] vertices, int[] indices)
        {
            Object _object = new Object
            {
                RelTransform = _rel,
                Name = name,
                Material = material,
                VertData = vertices,
                Indices = indices,
                Location = location,
                Rotation = rotation,
                Scale = scale
            };
            Array.Resize(ref VAO, VAO.Length + 1);
            Objects.Add(_object);
        }

        public static void AddLightToArray(string name, Vector3 lightColor, Shader shader, Vector3 scale, Vector3 location, Vector3 rotation, VertexData[] vertices, int[] indices)
        {
            Light _light = new Light
            {
                Name = name,
                LightColor = lightColor,
                Shader = shader,
                VertData = vertices,
                Indices = indices,
                Location = location,
                Rotation = rotation,
                Scale = scale
            };
            Array.Resize(ref VAOlights, VAOlights.Length + 1);
            Lights.Add(_light);
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
                GL.BufferData(BufferTarget.ArrayBuffer, Objects[i].VertData.Length * 8 * sizeof(float), Objects[i].VertData, BufferUsageHint.StaticDraw);
                // Generate and bind Element Buffer
                int EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Objects[i].Indices.Length * sizeof(uint), Objects[i].Indices, BufferUsageHint.StaticDraw);

                // Set attributes in shaders - vertex positions, UV's and normals
                GL.EnableVertexAttribArray(Game._PhongShader.GetAttribLocation("aPosition"));
                GL.VertexAttribPointer(Game._PhongShader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(Game._PhongShader.GetAttribLocation("aTexCoord"));
                GL.VertexAttribPointer(Game._PhongShader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(Game._PhongShader.GetAttribLocation("aNormal"));
                GL.VertexAttribPointer(Game._PhongShader.GetAttribLocation("aNormal"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
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
                GL.EnableVertexAttribArray(Lights[i].Shader.GetAttribLocation("aTexCoord"));
                GL.VertexAttribPointer(Lights[i].Shader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(Lights[i].Shader.GetAttribLocation("aNormal"));
                GL.VertexAttribPointer(Lights[i].Shader.GetAttribLocation("aNormal"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            }
        }

        // Draw the array of objects
        public static void DrawObjects(Matrix4 projection, Matrix4 view)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Game._PhongShader.Use();
                GL.BindVertexArray(VAO[i]);
                SetTransform(Game._PhongShader, MakeTransform(i, Objects[i].Scale, Objects[i].Location, Objects[i].Rotation));
                SetUniformMatrix(Game._PhongShader, projection, view);

                Vector3 ambient = new Vector3(Game.BG_Color.X, Game.BG_Color.Y, Game.BG_Color.Z);
                Game._PhongShader.SetVector3("material.ambient", ambient);
                Game._PhongShader.SetVector3("material.diffuse", Objects[i].Material.diffuse);
                Game._PhongShader.SetVector3("material.specular", Objects[i].Material.specular);
                Game._PhongShader.SetFloat("material.shininess", Objects[i].Material.shininess);

                Game._PhongShader.SetFloat("Point.constant", 1.0f);
                Game._PhongShader.SetFloat("Point.linear", 0.09f);
                Game._PhongShader.SetFloat("Point.quadratic", 0.032f);

                Game._PhongShader.SetVector3("Point.lightColor", Lights[0].LightColor);
                Game._PhongShader.SetVector3("Point.lightPos", Lights[0].Location);
                Game._PhongShader.SetVector3("viewPos", Game.position);

                GL.DrawElements(PrimitiveType.Triangles, Objects[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        // Draw the array of objects
        public static void DrawLights(Matrix4 projection, Matrix4 view)
        {
            for (int i = 0; i < Lights.Count; i++)
            {
                Lights[i].Shader.Use();
                GL.BindVertexArray(VAOlights[i]);
                SetTransform(Lights[i].Shader, MakeLightTransform(Lights[i].Scale, Lights[i].Location, Lights[i].Rotation));
                // Matrix4 lightLoc = Matrix4.CreateTranslation(Lights[i].Location);
                // GL.UniformMatrix4(Lights[i].Shader.GetUniformLocation("location"), true, ref lightLoc);
                SetUniformMatrix(Lights[i].Shader, projection, view);
                SetMaterialProperties(Lights[i].Shader, Lights[i].LightColor, "lightcolor");

                GL.DrawElements(PrimitiveType.Triangles, Lights[i].Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        // Set uniforms in shader
        public static void SetUniformMatrix(Shader shader, Matrix4 projection, Matrix4 view)
        {
            GL.UniformMatrix4(shader.GetUniformLocation("projection"), true, ref projection);
            GL.UniformMatrix4(shader.GetUniformLocation("view"), true, ref view);
        }

        // Set material properties
        public static void SetMaterialProperties(Shader shader, Vector3 color, string var)
        {
            GL.Uniform3(shader.GetUniformLocation(var), color);
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
            transform *=
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            transform *= Matrix4.CreateTranslation(location);

            return transform;
        }

        public static Shader fboShader = new Shader("./../../../Resources/shaders/framebuffer.vert", "./../../../Resources/shaders/framebuffer.frag");
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
            rectVAO = GL.GenVertexArray();
            GL.BindVertexArray(rectVAO);
            rectVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, rectVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, rectVerts.Length * sizeof(float), rectVerts, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(fboShader.GetAttribLocation("aPos"));
            GL.VertexAttribPointer(fboShader.GetAttribLocation("aPos"), 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(fboShader.GetAttribLocation("aTexCoord"));
            GL.VertexAttribPointer(fboShader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            GL.Uniform1(fboShader.GetUniformLocation("screenTexture"), 0);
        }

        public static int FBO;
        public static int PPfbo;
        public static int RBO;
        public static int PPtexture;
        public static int framebufferTexture;

        public static void GenFBO(float CameraWidth, float CameraHeight)
        {
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DMultisample, framebufferTexture);
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, true);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, framebufferTexture, 0);

            RBO = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.Depth24Stencil8, (int)CameraWidth, (int)CameraHeight);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);

            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus);
            }

            PPfbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, PPfbo);

            PPtexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, PPtexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PPtexture, 0);

            var fboStatus2 = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus2 != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus2);
            }
        }

        public static void FBOlogic(float CameraWidth, float CameraHeight)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, PPfbo);
            GL.BlitFramebuffer(0, 0, (int)CameraWidth, (int)CameraHeight, 0, 0, (int)CameraWidth, (int)CameraHeight, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            fboShader.Use();
            GL.BindVertexArray(rectVAO);
            GL.Disable(EnableCap.DepthTest);
            GL.BindTexture(TextureTarget.Texture2DMultisample, PPtexture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}