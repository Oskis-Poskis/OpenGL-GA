using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Text.Json;
using System.IO;
using System;

using ImGuiNET;

using static Engine.UserInterface.GUI;
using static Engine.Importer.Import;
using static Engine.RenderEngine.Rendering;
using static Engine.SettingUP.Setup;
using Engine.MathLib;
using System.Text.Json.Serialization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Engine.SettingUP;
using Object = Engine.SettingUP.Setup.Object;
using System.Linq;

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
        public static Texture PointLightTexture;
        public static Material M_Default = new Material(new Vector3(1), 0.5f, 0, 1, new int[] { 0, 0, 0, 0, 0 });
        public static Material M_Misc = new Material(new Vector3(1), 1, 1, 1, new int[] { 1, 1, 0, 1, 1 });

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
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.PolygonOffsetFill);
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
            PBRmaps[0] = Texture.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "Resources/3D_Models/statue/DefaultMaterial_albedo.jpg", TextureUnit.Texture0);
            PBRmaps[1] = Texture.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "Resources/3D_Models/statue/DefaultMaterial_roughness.jpg", TextureUnit.Texture0);
            PBRmaps[2] = Texture.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "Resources/3D_Models/statue/DefaultMaterial_roughness.jpg", TextureUnit.Texture0);
            PBRmaps[3] = Texture.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "Resources/3D_Models/statue/DefaultMaterial_normal.jpg", TextureUnit.Texture0);
            PBRmaps[4] = Texture.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "Resources/3D_Models/statue/DefaultMaterial_AO.jpg", TextureUnit.Texture0);
            PointLightTexture = Texture.LoadFromFile(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/Images/PointLightTexture.png", TextureUnit.Texture0);

            // First model, placeholder at 0 in array so array isnt empty
            LoadModel(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/Primitives/PointLightMesh.fbx");
            AddObjectToArray("Scene Root", M_Default, new Vector3(1), new Vector3(0), new Vector3(0), importedVertexData, importindices);
            //////////////////////////////////////////////////////////

            LoadModel(AppDomain.CurrentDomain.BaseDirectory + "Resources/3D_Models/statue/model.dae");
            AddObjectToArray(importname, M_Misc, new Vector3(3), new Vector3(0, 4, 0), new Vector3(0), importedVertexData, importindices);
            LoadModel(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/Primitives/Plane.fbx");
            AddObjectToArray("Floor", M_Default, new Vector3(10), importedLocation, new Vector3(-90, 0, 0), importedVertexData, importindices);

            LoadModel(AppDomain.CurrentDomain.BaseDirectory + "Engine/Engine_Resources/Primitives/Plane.fbx", true);
            AddLightToArray(5, 0, "Point Light", new Vector3(1), new Vector3(4, 5, 3), new Vector3(0f), importedVertPosData, importindices);

            ConstructObjects();
            ConstructLights();
            PBRShader.SetFloat("NoiseAmount", NoiseAmount);

            GenFBO(CameraWidth, CameraHeight);
            GenScreenRect();
            SetUpCubeMap();
            GLFW.MaximizeWindow(WindowPtr);

            base.OnLoad();
        }

        Matrix4 projection;
        static bool UIon = true;

        // Render loop
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (UIon == true) GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            else GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.ClearColor(new Color4(BG_Color.X, BG_Color.Y, BG_Color.Z, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            if (isMainHovered == true | IsMouseButtonDown(MouseButton.Right) | IsKeyDown(Keys.LeftAlt)) GeneralInput(args);
            if (IsMouseButtonDown(MouseButton.Right) | IsKeyDown(Keys.LeftAlt)) MouseInput();
            if (IsMouseButtonReleased(MouseButton.Right) | IsKeyReleased(Keys.LeftAlt)) CursorState = CursorState.Normal;

            if (UIon == true) projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), Math.Abs(CameraWidth / CameraHeight), 0.1f, 100.0f);
            else projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), Math.Abs(WindowWidth / WindowHeight), 0.1f, 100.0f);

            Matrix4 view = Matrix4.LookAt(position, position + front, up);

            DrawObjects(projection, view, wireframeonoff);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubeMapTexture);
            if (showCubeMap == true) DrawCubeMapCube(projection, view, position);

            // Draw all lights
            if (UIon == true)
            {
                DrawLights(projection, view);

                GL.Disable(EnableCap.DepthTest);
                fboShader.Use();
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
                GL.BindVertexArray(rectVAO);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

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

                LoadGameWindow(ref CameraWidth, ref CameraHeight);
                UIController.Render();
                ImGuiController.CheckGLError("End of frame");
            }

            // Close editor
            if (IsKeyDown(Keys.Escape) | CloseWindow == true) Close();

            Context.SwapBuffers();            
            base.OnRenderFrame(args);
        }

        public static System.Numerics.Vector3 Vec3toSN(Vector3 v)
        {
            return new System.Numerics.Vector3(v.X, v.Y, v.Z);
        }

        public static System.Numerics.Vector2 Vec2toSN(Vector2 v)
        {
            return new System.Numerics.Vector2(v.X, v.Y);
        }

        public static Vector3 SNtoVec3(System.Numerics.Vector3 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector2 SNtoVec2(System.Numerics.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static ConvertedVertexData[] ConvertVertexData(VertexData[] v)
        {
            ConvertedVertexData[] convertArray = new ConvertedVertexData[v.Length];

            for (int i = 0; i < convertArray.Length; i++)
            {
                convertArray[i] = new ConvertedVertexData(Vec3toSN(v[i].Position), Vec2toSN(v[i].texCoord), Vec3toSN(v[i].Normals), Vec3toSN(v[i].Tangents), Vec3toSN(v[i].BiTangents));
            }

            return convertArray;
        }

        public static VertexData[] RevertVertexData(ConvertedVertexData[] v)
        {
            VertexData[] convertArray = new VertexData[v.Length];

            for (int i = 0; i < convertArray.Length; i++)
            {
                convertArray[i] = new VertexData(SNtoVec3(v[i].Position), SNtoVec2(v[i].texCoord), SNtoVec3(v[i].Normals), SNtoVec3(v[i].Tangents), SNtoVec3(v[i].BiTangents));
            }

            return convertArray;
        }

        public static void SaveFile(string filename)
        {
            ConvertedObject[] convertedObject = new ConvertedObject[Objects.Count];
            for (int i = 0; i < convertedObject.Length; i++)
            {
                convertedObject[i] = new ConvertedObject(
                Objects[i].Name,
                Objects[i].ID,
                new ConvertedMaterial(Vec3toSN(
                    Objects[i].Material.albedo),
                    Objects[i].Material.roughness,
                    Objects[i].Material.metallic,
                    Objects[i].Material.ao,
                    Objects[i].Material.Maps),
                ConvertVertexData(Objects[i].VertData), Objects[i].Indices,
                Vec3toSN(Objects[i].Location),
                Vec3toSN(Objects[i].Rotation),
                Vec3toSN(Objects[i].Scale));
            }

            var _options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            string jsonString = JsonSerializer.Serialize(convertedObject, _options);
            File.WriteAllText(filename, jsonString);
            Console.WriteLine("Saved file to " + filename);
        }

        public static void LoadSave(string filename)
        {
            string loadedjson = File.ReadAllText(filename);
            ConvertedObject[] loadedSave = JsonSerializer.Deserialize<ConvertedObject[]>(loadedjson, new JsonSerializerOptions() { IncludeFields = true });
            Object[] revertedObject = new Object[loadedSave.Length];

            for (int i = 0; i < revertedObject.Length; i++)
            {
                revertedObject[i] = new Object(
                    loadedSave[i].Name,
                    loadedSave[i].ID,
                    new Material(SNtoVec3(
                        loadedSave[i].Material.albedo),
                        loadedSave[i].Material.roughness,
                        loadedSave[i].Material.metallic,
                        loadedSave[i].Material.ao,
                        loadedSave[i].Material.Maps),
                    RevertVertexData(loadedSave[i].VertData), loadedSave[i].Indices,
                    SNtoVec3(loadedSave[i].Location),
                    SNtoVec3(loadedSave[i].Rotation),
                    SNtoVec3(loadedSave[i].Scale));
            }

            List<Object> newObjectsArray = revertedObject.ToList();
            while (VAO.Count < newObjectsArray.Count)
            {
                VAO.Add(0);
            }

            Objects = newObjectsArray;

            Console.WriteLine(VAO.Count);
            Console.WriteLine(Objects.Count);

            ConstructObjects();

            Console.WriteLine("Loaded save from " + filename);
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

            if (IsKeyPressed(Keys.K)) UIon = Functions.ToggleBool(UIon);

            if (IsKeyDown(Keys.F)) position = Objects[selectedObject].Location;

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