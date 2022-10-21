using System;
using ImGuiNET;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
using static OpenTK_Learning.R_3D;

namespace OpenTK_Learning
{
    class Main : GameWindow
    {
        // Create a window and assign values
        public Main(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Resizable,
                StartVisible = false,
                StartFocused = true,
                WindowState = WindowState.Normal,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            })
        {
            // Center the window
            CenterWindow();
            WindowHeight = Size.Y;
            WindowWidth = Size.X;
            CameraHeight = Size.Y;
            CameraWidth = Size.X;
        }

        private Texture diffuseMap;
        private Texture normalMap;
        public static Shader LightShader = new Shader("./../../../Engine/Engine_Resources/shaders/light.vert", "./../../../Engine/Engine_Resources/shaders/light.frag");
        public static Shader PhongShader = new Shader("./../../../Engine/Engine_Resources/shaders/default.vert", "./../../../Engine/Engine_Resources/shaders/default.frag", true);

        public static Material M_Default;
        public static Material M_Car;
        public static Material M_Floor;

        public static System.Numerics.Vector3 BG_Color = new System.Numerics.Vector3(0.12f);
        public static float fontSize = 1.0f;
        public static bool wireframeonoff = false;
        bool vsynconoff = true;
        float spacing = 5f;
        int selectedObject = 2;
        int selectedLight = 0;

        // Window bools
        public static bool showDemoWindow = false;
        public static bool showStatistics = false;
        public static bool showObjectProperties = true;
        public static bool showLightProperties = true;
        public static bool showOutliner = true;
        public static bool showSettings = false;
        public static bool CloseWindow = false;

        // Camera settings
        public static float WindowWidth;
        public static float WindowHeight;
        float CameraWidth;
        float CameraHeight;
        float Yaw;
        float Pitch = -90f;
        int FOV = 90;
        int speed = 12;
        float sensitivity = 0.25f;

        public static float lineWidth = 0.1f;

        ImGuiController _controller;

        // Camera transformations
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        public static Vector3 position = new Vector3(0.0f, 3.0f, 3.0f);

        // Runs when the window is resizeds
        protected override void OnResize(ResizeEventArgs e)
        {
            GL.BindTexture(TextureTarget.Texture2D, R_3D.framebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, R_3D.framebufferTexture2);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)CameraWidth, (int)CameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);

            _controller.WindowResized((int)WindowWidth, (int)WindowHeight);

            // Matches window scale to new resize
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        // Runs after Run();
        protected override void OnLoad()
        {
            IsVisible = true;
            VSync = VSyncMode.On;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1f));

            GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace); 

            // Load textures
            diffuseMap = Texture.LoadFromFile("./../../../Resources/3D_Models/Car_diffuse.jpg", TextureUnit.Texture0);
            //normalMap = Texture.LoadFromFile("./../../../Resources/3D_Models/Car_normal.png", TextureUnit.Texture0);

            M_Default = new R_3D.Material
            {
                ambient = new Vector3(0.1f),
                diffuse = new Vector4(1, 1, 1, 0),
                specular = new Vector3(0.5f),
                shininess = 96.0f
            };
            M_Car = new R_3D.Material
            {
                ambient = new Vector3(0.1f),
                diffuse = new Vector4(1, 1, 1, 1),
                specular = new Vector3(0.5f),
                shininess = 96.0f
            };
            M_Floor = new R_3D.Material
            {
                ambient = new Vector3(0.1f),
                diffuse = new Vector4(0.5f, 0.5f, 0.5f, 0),
                specular = new Vector3(0.5f),
                shininess = 64.0f
            };

            // Add default objects
            R_3D.AddObjectToArray(false, "Cube", M_Default,
                new Vector3(2f),           // Scale
                new Vector3(0f, 2f, 0f),  // Location
                new Vector3(0f),  // Rotation
                Cube.vertices, Cube.indices);
            R_3D.AddObjectToArray(false, "Plane", M_Floor,
                new Vector3(15f), // Scale
                new Vector3(0f),  // Location
                new Vector3(0f),  // Rotation
                Plane.vertices, Plane.indices);

            // Load model into temporary variables
            R_Loading.LoadModel("./../../../Resources/3D_Models/Monkey.fbx");
            // Spawn i * j objects using the temporary assigned variables
            int numit = 5;
            for (int i = 0; i < numit; i++)
            {
                for (int j = 0; j < numit; j++)
                {
                    R_3D.AddObjectToArray(false, R_Loading.importname + Math_Functions.RandInt(1, 1000), M_Default,
                        new Vector3(1.0f),            // Scale
                        new Vector3(i * 3 - (numit/2 * 3), j * 3 + 6, 0f),    // Location
                        new Vector3(-90f, 0f, 0f),  // Rotation
                        R_Loading.importedData, R_Loading.importindices);
                }
            }

            R_Loading.LoadModel("./../../../Resources/3D_Models/Car.fbx");
            R_3D.AddObjectToArray(false, R_Loading.importname, M_Car,
                        new Vector3(2f),            // Scale
                        new Vector3(0, 5, 6),    // Location
                        new Vector3(180f, -90f, 0f),  // Rotation
                        R_Loading.importedData, R_Loading.importindices);

            // Generate VAO, VBO and EBO
            R_3D.ConstructObjects();

            // Add lights
            R_Loading.LoadModel("./../../../Engine/Engine_Resources/Primitives/PointLightMesh.fbx");
            R_3D.AddLightToArray(0.75f, 1, "DirLight.2", new Vector3(1f, 1f, 1f), LightShader, new Vector3(0.75f, -0.6f, -0.75f), new Vector3(10f, 8f, 10f), new Vector3(0f), R_Loading.importedData, R_Loading.importindices);
            //R_3D.AddLightToArray(1, 0, "Blue PL", new Vector3(0f, 0f, 1f), LightShader, new Vector3(1f), new Vector3(3f, 14, 4f), new Vector3(0f), R_Loading.importedData, R_Loading.importindices);
            //R_3D.AddLightToArray(1, 0, "Red PL", new Vector3(1f, 0f, 0f), LightShader, new Vector3(1f), new Vector3(-3f, 8f, 4f), new Vector3(0f), R_Loading.importedData, R_Loading.importindices);
            //R_3D.AddLightToArray(1, 0, "Green PL", new Vector3(0f, 1f, 0f), LightShader, new Vector3(1f), new Vector3(0f, 11, 4f), new Vector3(0f), R_Loading.importedData, R_Loading.importindices);
            R_3D.ConstructLights();

            // Generate two screen triangles
            R_3D.GenFBO(CameraWidth, CameraHeight);
            R_3D.GenScreenRect();

            _controller = new ImGuiController((int)WindowWidth, (int)WindowHeight);
            UI.LoadTheme();

            base.OnLoad();
        }

        // Render loop
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Bind FBO and clear color and depth buffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, R_3D.FBO);
            GL.ClearColor(new Color4(BG_Color.X, BG_Color.Y, BG_Color.Z, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Enable(EnableCap.DepthTest);

            // Draw 3D objects
            {
                GeneralInput(args);

                // Camera pos and FOV
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), CameraWidth / CameraHeight, 0.1f, 100.0f);
                Matrix4 view = Matrix4.LookAt(position, position + front, up);

                // Use texture
                diffuseMap.Use(TextureUnit.Texture0);
                //normalMap.Use(TextureUnit.Texture1);

                // Main function for drawing the array of objects
                if (wireframeonoff == true) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                else GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
                GL.StencilMask(0xFF);
                R_3D.DrawObjects(projection, view);

                GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
                GL.StencilMask(0x00);
                GL.Disable(EnableCap.DepthTest);
                PhongShader.SetInt("outline", 1);
                R_3D.DrawOneObject(selectedObject, projection, view);
                PhongShader.SetInt("outline", 0);
                GL.StencilMask(0xFF);
                GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
                GL.Enable(EnableCap.DepthTest);

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                R_3D.DrawLights(projection, view);

                // Editor navigation on right click
                if (IsMouseButtonDown(MouseButton.Right) | IsKeyDown(Keys.LeftAlt))
                {
                    MouseInput();
                }
            }

            R_3D.FBOlogic();

            // UI
            _controller.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();

            if (showDemoWindow)
            {
                ImGui.ShowDemoWindow();
            }

            UI.LoadMenuBar();
            if (showStatistics) UI.LoadStatistics(CameraWidth, CameraHeight, Yaw, Pitch, position, spacing);
            if (showObjectProperties) UI.LoadObjectProperties(ref selectedObject, spacing);
            if (showLightProperties) UI.LoadLightProperties(ref selectedLight, spacing);
            if (showOutliner) UI.LoadOutliner(ref selectedObject, ref selectedLight, spacing);

            if (showSettings)
            {
                // Settings
                ImGui.Begin("Settings");
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                if (ImGui.TreeNode("Rendering"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.Checkbox("V-Sync", ref vsynconoff))
                    {
                        if (vsynconoff == false) VSync = VSyncMode.Off;
                        if (vsynconoff == true) VSync = VSyncMode.On;
                    }
                    ImGui.Checkbox("Wireframe", ref wireframeonoff);
                    ImGui.TreePop();
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));


                if (ImGui.TreeNode("Camera"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.SliderInt("Field of View", ref FOV, 10, 179);
                    ImGui.SliderFloat("Sensitivity", ref sensitivity, 0.1f, 2.0f, "%.1f");
                    ImGui.SliderInt("Speed", ref speed, 1, 30);
                    ImGui.TreePop();
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                if (ImGui.TreeNode("Lightning"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Text("Background Color");
                    if (ImGui.ColorEdit3("Background Color", ref BG_Color, ImGuiColorEditFlags.NoLabel))
                    {
                        GL.ClearColor(new Color4(BG_Color.X, BG_Color.Y, BG_Color.Z, 1f));
                    }
                    ImGui.TreePop();
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                if (ImGui.TreeNode("Editor"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.SliderFloat("Font Size", ref fontSize, 0.8f, 2.0f, "%.1f"))
                    {
                        ImGui.GetIO().FontGlobalScale = fontSize;

                        ImGui.TreePop();
                    }

                    ImGui.SliderFloat("Spacing", ref spacing, 1f, 10f, "%.1f");
                }
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                ImGui.End();
            }

            UI.LoadGameWindow(ref CameraWidth, ref CameraHeight);

            _controller.Render();
            ImGuiController.CheckGLError("End of frame");

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        private void MouseInput()
        {
            if (Yaw > 89.0f)
            {
                Yaw = 89.0f;
            }

            else if (Yaw < -89.0f)
            {
                Yaw = -89.0f;
            }

            else
            {
                Yaw -= MouseState.Delta.Y * sensitivity;
            }

            Pitch += MouseState.Delta.X * sensitivity;

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            front = Vector3.Normalize(front);
        }

        // Keyboard input
        private void GeneralInput(FrameEventArgs args)
        {
            if (IsMouseButtonPressed(MouseButton.Right) | IsKeyDown(Keys.LeftAlt))
            {
                CursorState = CursorState.Grabbed;
            }

            if (IsMouseButtonReleased(MouseButton.Right) | IsKeyReleased(Keys.LeftAlt))
            {
                CursorState = CursorState.Normal;
            }

            // Close editor
            if (IsKeyDown(Keys.Escape) | CloseWindow == true)
            {
                Close();
            }

            // Delete
            if (IsKeyPressed(Keys.Delete))
            {
                if (selectedObject > 0)
                {
                    R_3D.Objects.RemoveAt(selectedObject);
                    selectedObject -= 1;
                }
            }

            // X and Z movement
            if (IsKeyDown(Keys.W))
            {
                position += front * speed * (float)args.Time;
            }
            if (IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)args.Time;
            }
            if (IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)args.Time;
            }
            if (IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)args.Time;
            }

            // Y movement
            if (IsKeyDown(Keys.Q))
            {
                position -= up * speed * (float)args.Time;
            }
            if (IsKeyDown(Keys.E))
            {
                position += up * speed * (float)args.Time;
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            /*
            if (IsKeyPressed(Keys.D1)) showStatistics = Math_Functions.ToggleBool(showStatistics);
            if (IsKeyPressed(Keys.D2)) showObjectProperties = Math_Functions.ToggleBool(showObjectProperties);
            if (IsKeyPressed(Keys.D3)) showLightProperties = Math_Functions.ToggleBool(showLightProperties);
            if (IsKeyPressed(Keys.D4)) showOutliner = Math_Functions.ToggleBool(showOutliner);
            if (IsKeyPressed(Keys.D5)) showSettings = Math_Functions.ToggleBool(showSettings);
            */

            base.OnKeyDown(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _controller.MouseScroll(e.Offset);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);

            _controller.PressChar((char)e.Unicode);
        }
    }
}