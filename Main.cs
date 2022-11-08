using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

using ImGuiNET;

using static Engine.UserInterface.GUI;
using static Engine.Importer.Import;
using static Engine.RenderEngine.Rendering;
using static Engine.SettingUP.Setup;
using Engine.MathLib;

namespace Engine
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

        public static Texture[] PBRmaps = new Texture[5];
        public static Material M_Default = new Material
        {
            albedo = new Vector3(1),
            roughness = 0.25f,
            metallic = 0,
            ao = 1,
            Maps = new int[] { 0, 0, 0, 0, 0 }
        };
        public static Material M_Misc = new Material
        {
            albedo = new Vector3(1),
            roughness = 1,
            metallic = 1,
            ao = 1,
            Maps = new int[] { 1, 1, 0, 1, 1 }
        };

        public static bool wireframeonoff = false;

        // Window bools
        public static bool showSettings = true;
        public static bool CloseWindow = false;
        public static bool isMainHovered;
        bool fullScreen = false;
        bool vsynconoff = true;

        // Camera settings
        public static float WindowWidth;
        public static float WindowHeight;
        public static float CameraWidth;
        public static float CameraHeight;

        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        public static Vector3 up = Vector3.UnitY;
        public static Vector3 position = new Vector3(0.0f, 4.0f, 4.0f);

        float sensitivity = 0.25f;
        public static float Pitch = -90f;
        public static float Yaw;
        int FOV = 75;
        int speed = 12;

        ImGuiController UIController;

        // Runs when the window is resizeds
        protected override void OnResize(ResizeEventArgs e)
        {
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            GL.DeleteFramebuffer(FBO);
            GenFBO(CameraWidth, WindowHeight);

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        // Runs after Run();
        unsafe protected override void OnLoad()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            //GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1f));
            GL.LineWidth(2f);
            GL.PointSize(5f);

            LoadIcons();
            Icon = LoadedIcon;
            UIController = new ImGuiController((int)WindowWidth, (int)WindowHeight);
            LoadTheme();

            VSync = VSyncMode.On;
            IsVisible = true;

            LoadDefaultMaps();
            /*
            PBRmaps[0] = Texture.LoadFromFile("./../../../Resources/Images/rusted-steel_albedo.png", TextureUnit.Texture0);
            PBRmaps[1] = Texture.LoadFromFile("./../../../Resources/Images/rusted-steel_roughness.png", TextureUnit.Texture0);
            PBRmaps[2] = Texture.LoadFromFile("./../../../Resources/Images/rusted-steel_metallic.png", TextureUnit.Texture0);
            PBRmaps[3] = Texture.LoadFromFile("./../../../Resources/Images/rusted-steel_normal-ogl.png", TextureUnit.Texture0);
            PBRmaps[4] = Texture.LoadFromFile("./../../../Resources/Images/rusted-steel_albedo.png", TextureUnit.Texture0);
            */
            
            PBRmaps[0] = Texture.LoadFromFile("./../../../Resources/3D_Models/statue/DefaultMaterial_albedo.jpg", TextureUnit.Texture0);
            PBRmaps[1] = Texture.LoadFromFile("./../../../Resources/3D_Models/statue/DefaultMaterial_roughness.jpg", TextureUnit.Texture0);
            PBRmaps[2] = Texture.LoadFromFile("./../../../Resources/3D_Models/statue/DefaultMaterial_roughness.jpg", TextureUnit.Texture0);
            PBRmaps[3] = Texture.LoadFromFile("./../../../Resources/3D_Models/statue/DefaultMaterial_normal.jpg", TextureUnit.Texture0);
            PBRmaps[4] = Texture.LoadFromFile("./../../../Resources/3D_Models/statue/DefaultMaterial_AO.jpg", TextureUnit.Texture0);
            

            LoadModel("./../../../Resources/3D_Models/statue/model.dae");
            AddObjectToArray(importname, M_Misc,
                new Vector3(3),          // Scale
                new Vector3(0, 4, 0),    // Location
                new Vector3(0),          // Rotation
                importedData, importindices);

            LoadModel("./../../../Engine/Engine_Resources/Primitives/CurveWall.fbx");
            AddObjectToArray("Curve", M_Default,
                new Vector3(1),     // Scale
                new Vector3(0),    // Location
                new Vector3(-90, 0, 0),    // Rotation
                importedData, importindices);

            LoadModel("./../../../Engine/Engine_Resources/Primitives/PointLightMesh.fbx", true);
            AddLightToArray(5, 10, 2, 0,
                "Point Light", new Vector3(1),
                LightShader,
                new Vector3(4, 5, 3),
                new Vector3(0f),
                importedLightData, importindices);

            ConstructObjects();
            ConstructLights();
            PBRShader.SetFloat("NoiseAmount", NoiseAmount);

            SetupGrid();

            GenFBO(CameraWidth, CameraHeight);
            GenScreenRect();
            SetUpCubeMap();
            GLFW.MaximizeWindow(WindowPtr);

            base.OnLoad();
        }

        // Render loop
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.ClearColor(new Color4(BG_Color.X, BG_Color.Y, BG_Color.Z, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            if (isMainHovered == true | IsMouseButtonDown(MouseButton.Right) | IsKeyDown(Keys.LeftAlt)) GeneralInput(args);
            if (IsMouseButtonDown(MouseButton.Right) | IsKeyDown(Keys.LeftAlt)) MouseInput();
            if (IsMouseButtonReleased(MouseButton.Right) | IsKeyReleased(Keys.LeftAlt)) CursorState = CursorState.Normal;

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), CameraWidth / CameraHeight, 0.1f, 100.0f);
            Matrix4 view = Matrix4.LookAt(position, position + front, up);

            DrawObjects(projection, view, wireframeonoff);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTexture);
            if (showCubeMap == true) DrawCubeMapCube(projection, view, position);

            // Draw all lights
            DrawLights(projection, view);
            DrawGrid(projection, view);

            /*
            GL.Disable(EnableCap.DepthTest);
            fboShader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            GL.BindVertexArray(rectVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            */

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // UI
            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            WindowOnOffs();

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

                if (ImGui.TreeNode("Post Processing"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.Checkbox("Chromatic Abberation", ref ChromaticAbberationOnOff))
                    {
                        fboShader.SetBool("ChromaticAbberationOnOff", ChromaticAbberationOnOff);
                    }

                    if (ChromaticAbberationOnOff == true)
                    {
                        if (ImGui.SliderFloat("##Chromatic Abberation Offset", ref ChromaticAbberationOffset, 0, 0.05f, "%.3f"))
                        {
                            fboShader.SetFloat("ChromaticAbberationOffset", ChromaticAbberationOffset);
                        }
                    }

                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Separator();
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                    ImGui.Text("Noise Amount"); ImGui.SameLine(); UserInterface.GUI.HelpMarker("Values around 0.5 reduce banding \nHigh values causes visible noise");
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.SliderFloat("##NA", ref NoiseAmount, 0.01f, 10.0f, "%.2f")) PBRShader.SetFloat("NoiseAmount", NoiseAmount);
                    ImGui.TreePop();
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                if (ImGui.TreeNode("Lightning"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Text("Sun Direction");
                    ImGui.SliderFloat3("##SunDirection", ref LightDirection, -1, 1);
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Text("Sun Color");
                    ImGui.ColorEdit3("##SunColor", ref LightColor);

                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Separator();
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                    ImGui.Text("Background Color");
                    if (ImGui.ColorEdit3("Background Color", ref BG_Color, ImGuiColorEditFlags.NoLabel))
                    {
                        GL.ClearColor(new Color4(BG_Color.X, BG_Color.Y, BG_Color.Z, 1f));
                    }
                    ImGui.Text("Show Cubemap");
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Checkbox("##Show Cubemap", ref showCubeMap);
                    ImGui.TreePop();
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                if (ImGui.TreeNode("Editor"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.SliderFloat("Font Size", ref fontSize, 0.1f, 2.0f, "%.2f"))
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

            // Close editor
            if (IsKeyDown(Keys.Escape) | CloseWindow == true) Close();

            LoadGameWindow(ref CameraWidth, ref CameraHeight);
            UIController.Render();
            ImGuiController.CheckGLError("End of frame");

            Context.SwapBuffers();            

            base.OnRenderFrame(args);
        }

        private void MouseInput()
        {
            CursorState = CursorState.Grabbed;

            if (Pitch > 360) Pitch = 0;
            if (Pitch < -360) Pitch = 0;

            Pitch += MouseState.Delta.X * sensitivity;
            Yaw -= MouseState.Delta.Y * sensitivity;
            
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Math.Clamp(Yaw, -89, 89))) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Math.Clamp(Yaw, -89, 89)));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(Math.Clamp(Yaw, -89, 89))) * (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            front = Vector3.Normalize(front);
        }

        // Keyboard input
        private void GeneralInput(FrameEventArgs args)
        {
            // Delete
            if (IsKeyPressed(Keys.Delete))
            {
                if (selectedObject > 0)
                {
                    Objects.RemoveAt(selectedObject);
                    selectedObject -= 1;
                }
            }

            if (IsKeyDown(Keys.F)) position = Objects[selectedObject].Location * 1.2f;

            // X and Z movement
            if (IsKeyDown(Keys.W)) position += front * speed * (float)args.Time;
            if (IsKeyDown(Keys.S)) position -= front * speed * (float)args.Time;
            if (IsKeyDown(Keys.A)) position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)args.Time;
            if (IsKeyDown(Keys.D)) position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)args.Time;

            // Y movement
            if (IsKeyDown(Keys.Q)) position -= up * speed * (float)args.Time;
            if (IsKeyDown(Keys.E)) position += up * speed * (float)args.Time;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (isMainHovered)
            {
                if (IsKeyPressed(Keys.D1)) showStatistics = Functions.ToggleBool(showStatistics);
                if (IsKeyPressed(Keys.D2)) showObjectProperties = Functions.ToggleBool(showObjectProperties);
                if (IsKeyPressed(Keys.D3)) showLightProperties = Functions.ToggleBool(showLightProperties);
                if (IsKeyPressed(Keys.D4)) showOutliner = Functions.ToggleBool(showOutliner);
                if (IsKeyPressed(Keys.D5)) showSettings = Functions.ToggleBool(showSettings);
            }

            if (IsKeyPressed(Keys.F11)) fullScreen = Functions.ToggleBool(fullScreen);

            base.OnKeyDown(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            UIController.MouseScroll(e.Offset);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);

            UIController.PressChar((char)e.Unicode);
        }
    }
}