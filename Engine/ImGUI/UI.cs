using System;
using System.Windows.Forms;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Learning
{
    class UI
    {
        public static void LoadTheme()
        {
            // Styling
            ImGui.GetStyle().FrameRounding = 4f;
            ImGui.GetStyle().FrameBorderSize = 1f;
            ImGui.GetStyle().TabRounding = 2f;
            ImGui.GetStyle().FramePadding = new System.Numerics.Vector2(4);
            ImGui.GetStyle().ItemSpacing = new System.Numerics.Vector2(8, 2);
            ImGui.GetStyle().ItemInnerSpacing = new System.Numerics.Vector2(1, 4);
            ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.None;
            ImGui.GetIO().FontGlobalScale = Main.fontSize;

            // Something
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(25f, 25f, 25f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);

            // Popup BG
            ImGui.PushStyleColor(ImGuiCol.ModalWindowDimBg, new System.Numerics.Vector4(30f, 30f, 30f, 150f) / 255);

            ImGui.PushStyleColor(ImGuiCol.TextDisabled, new System.Numerics.Vector4(150f, 150f, 150f, 255f) / 255);

            // Titles
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBg, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);

            // Tabs
            ImGui.PushStyleColor(ImGuiCol.Tab, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabActive, new System.Numerics.Vector4(40f, 40f, 40f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocused, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, new System.Numerics.Vector4(40f, 40f, 40f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabHovered, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);
            
            // Header
            ImGui.PushStyleColor(ImGuiCol.Header, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(0f, 153f, 76f, 180f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);

            // Rezising bar
            ImGui.PushStyleColor(ImGuiCol.Separator, new System.Numerics.Vector4(40f, 40f, 40f, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorActive, new System.Numerics.Vector4(80f, 80f, 80f, 255) / 255);

            // Buttons
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(255, 41, 55, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new System.Numerics.Vector4(255, 41, 55, 150) / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new System.Numerics.Vector4(255, 41, 55, 100) / 255);

            // Docking and rezise
            ImGui.PushStyleColor(ImGuiCol.DockingPreview, new System.Numerics.Vector4(255, 41, 55, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, new System.Numerics.Vector4(217, 35, 35, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, new System.Numerics.Vector4(217, 35, 35, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, new System.Numerics.Vector4(217, 35, 35, 150) / 255);

            // Sliders, buttons, etc
            ImGui.PushStyleColor(ImGuiCol.SliderGrab, new System.Numerics.Vector4(120f, 120f, 120f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new System.Numerics.Vector4(180f, 180f, 180f, 255f) / 255);
        }

        public static void LoadGameWindow(ref float CameraWidth, ref float CameraHeight)
        {
            ImGui.Begin("Game");

            CameraWidth = ImGui.GetWindowWidth() - 18;
            CameraHeight = ImGui.GetWindowHeight() - 50;

            Main.isMainHovered = ImGui.IsWindowHovered();

            ImGui.Image((IntPtr)R_3D.framebufferTexture,
                new System.Numerics.Vector2(CameraWidth, CameraHeight),
                new System.Numerics.Vector2(0.0f, 1.0f),
                new System.Numerics.Vector2(1.0f, 0.0f),
                new System.Numerics.Vector4(1.0f),
                new System.Numerics.Vector4(1, 1, 1, 0.2f));
            ImGui.End();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public static void LoadMenuBar()
        {
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Quit", "Alt+F4"))
                {
                    Main.CloseWindow = true;
                }

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Edit"))
            {
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Window"))
            {
                if (ImGui.BeginMenu("Debug"))
                {
                    ImGui.Checkbox("Statistics", ref Main.showStatistics);
                    ImGui.Separator();
                    ImGui.Checkbox("ImGUI Demo ", ref Main.showDemoWindow);
                    ImGui.EndMenu();
                }

                ImGui.Separator();

                if (ImGui.BeginMenu("Editor"))
                {
                    ImGui.Checkbox("Show Object Properties", ref Main.showObjectProperties);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Light Properties", ref Main.showLightProperties); 
                    ImGui.Separator();
                    ImGui.Checkbox("Show Outliner", ref Main.showOutliner);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Settings", ref Main.showSettings);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Material Editor", ref Main.showMaterialEditor);
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            ImGui.Dummy(new System.Numerics.Vector2(ImGui.GetWindowWidth() - 500, 0));
            ImGui.TextDisabled("Objects: " + (R_3D.Objects.Count).ToString());
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled(GL.GetString(StringName.Renderer));
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled("FPS: " + ImGui.GetIO().Framerate.ToString("0"));
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled("MS: " + (1000 / ImGui.GetIO().Framerate).ToString("0.00"));

            ImGui.EndMainMenuBar();
        }

        public static void LoadStatistics(float CameraWidth, float CameraHeight, float Yaw, float Pitch, Vector3 position, float spacing)
        {
            // Stats
            ImGui.Begin("Statistics");
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            if (ImGui.TreeNode("Rendering"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("Renderer: " + GL.GetString(StringName.Renderer));
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("FPS: " + ImGui.GetIO().Framerate.ToString("0.00"));
                ImGui.Text("MS: " + (1000 / ImGui.GetIO().Framerate).ToString("0.00"));
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("Objects: " + (R_3D.Objects.Count).ToString());
                ImGui.Text("Point Lights: " + (R_3D.numPL).ToString());
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("Viewport Width: " + CameraWidth); ImGui.SameLine(); ImGui.Text(" | "); ImGui.SameLine(); ImGui.Text("Viewport Height: " + CameraHeight);
                ImGui.TreePop();
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            if (ImGui.TreeNode("Camera"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("Yaw: " + Yaw.ToString());
                ImGui.Text("Pitch: " + Pitch.ToString());
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Text("X: " + position.X);
                ImGui.Text("Y: " + position.Y);
                ImGui.Text("Z: " + position.Z);
                ImGui.TreePop();
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.End();
        }

        public static void LoadMaterialEditor(int selectedObject, float spacing)
        {
            System.Numerics.Vector3 _albedo = new System.Numerics.Vector3(
                R_3D.Objects[selectedObject].Material.albedo.X,
                R_3D.Objects[selectedObject].Material.albedo.Y,
                R_3D.Objects[selectedObject].Material.albedo.Z);
            float _roughness = R_3D.Objects[selectedObject].Material.roughness;
            float _metallic = R_3D.Objects[selectedObject].Material.metallic;

            ImGui.Begin("Material Editor");
            ImGui.ColorEdit3("Albedo", ref _albedo);
            ImGui.SliderFloat("Roughness", ref _roughness, 0, 1);
            ImGui.SliderFloat("Metallic", ref _metallic, 0, 1);

            R_3D.Objects[Main.selectedObject] = new R_3D.Object
            {
                RelTransform = R_3D.Objects[selectedObject].RelTransform,
                Name = R_3D.Objects[selectedObject].Name,
                ID = R_3D.Objects[selectedObject].ID,
                Material = new R_3D.Material
                {
                    albedo = new Vector3(_albedo.X, _albedo.Y, _albedo.Z),
                    roughness = _roughness,
                    metallic = _metallic,
                },
                VertData = R_3D.Objects[selectedObject].VertData,
                Indices = R_3D.Objects[selectedObject].Indices,
                Location = R_3D.Objects[selectedObject].Location,
                Rotation = R_3D.Objects[selectedObject].Rotation,
                Scale = R_3D.Objects[selectedObject].Scale
            };

            ImGui.End();
        }

        public static void LoadObjectProperties(ref int selectedObject, float spacing)
        {
            // Object Properties
            ImGui.Begin("Object Properties");
            ImGui.Text(R_3D.Objects[selectedObject].Name);
            string inpstr = R_3D.Objects[selectedObject].Name;
            if (ImGui.InputTextWithHint("##Name", inpstr, ref inpstr, 30))
            {
                R_3D.Objects[selectedObject] = new R_3D.Object
                {
                    RelTransform = R_3D.Objects[selectedObject].RelTransform,
                    Name = inpstr,
                    ID = R_3D.Objects[selectedObject].ID,
                    Material = R_3D.Objects[selectedObject].Material,
                    VertData = R_3D.Objects[selectedObject].VertData,
                    Indices = R_3D.Objects[selectedObject].Indices,
                    Location = R_3D.Objects[selectedObject].Location,
                    Rotation = R_3D.Objects[selectedObject].Rotation,
                    Scale = R_3D.Objects[selectedObject].Scale
                };
            }

            ImGui.SameLine(ImGui.GetWindowWidth() - 60);

            if (ImGui.Button("Delete"))
            {
                if (selectedObject > 0)
                {
                    R_3D.Objects.RemoveAt(selectedObject);
                    R_3D.VAO.RemoveAt(selectedObject);
                    selectedObject -= 1;
                }
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            bool _relatve = R_3D.Objects[selectedObject].RelTransform;
            if (ImGui.Checkbox("Relative Transform", ref _relatve))
            {
                R_3D.Objects[selectedObject] = new R_3D.Object
                {
                    RelTransform = _relatve,
                    Name = R_3D.Objects[selectedObject].Name,
                    ID = R_3D.Objects[selectedObject].ID,
                    Material = R_3D.Objects[selectedObject].Material,
                    VertData = R_3D.Objects[selectedObject].VertData,
                    Indices = R_3D.Objects[selectedObject].Indices,
                    Location = R_3D.Objects[selectedObject].Location,
                    Rotation = R_3D.Objects[selectedObject].Rotation,
                    Scale = R_3D.Objects[selectedObject].Scale
                };
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            //  ----- Transform -----
            if (ImGui.TreeNode("Transform"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                // Location
                System.Numerics.Vector3 _loc = new System.Numerics.Vector3(
                    R_3D.Objects[selectedObject].Location.X,
                    R_3D.Objects[selectedObject].Location.Y,
                    R_3D.Objects[selectedObject].Location.Z);
                ImGui.Text("Location");
                if (ImGui.DragFloat3("##Location", ref _loc, 0.1f))
                {
                    R_3D.Objects[selectedObject] = new R_3D.Object
                    {
                        RelTransform = R_3D.Objects[selectedObject].RelTransform,
                        Name = R_3D.Objects[selectedObject].Name,
                        ID = R_3D.Objects[selectedObject].ID,
                        Material = R_3D.Objects[selectedObject].Material,
                        VertData = R_3D.Objects[selectedObject].VertData,
                        Indices = R_3D.Objects[selectedObject].Indices,
                        Location = new Vector3(_loc.X, _loc.Y, _loc.Z),
                        Rotation = R_3D.Objects[selectedObject].Rotation,
                        Scale = R_3D.Objects[selectedObject].Scale
                    };
                }

                ImGui.SameLine();
                if (ImGui.Button("##1", new System.Numerics.Vector2(20f)))
                {
                    R_3D.Objects[selectedObject] = new R_3D.Object
                    {
                        RelTransform = R_3D.Objects[selectedObject].RelTransform,
                        Name = R_3D.Objects[selectedObject].Name,
                        ID = R_3D.Objects[selectedObject].ID,
                        Material = R_3D.Objects[selectedObject].Material,
                        VertData = R_3D.Objects[selectedObject].VertData,
                        Indices = R_3D.Objects[selectedObject].Indices,
                        Location = new Vector3(0.0f),
                        Rotation = R_3D.Objects[selectedObject].Rotation,
                        Scale = R_3D.Objects[selectedObject].Scale
                    };
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                // Rotation
                System.Numerics.Vector3 _rot = new System.Numerics.Vector3(
                    R_3D.Objects[selectedObject].Rotation.X,
                    R_3D.Objects[selectedObject].Rotation.Y,
                    R_3D.Objects[selectedObject].Rotation.Z);
                ImGui.Text("Rotation");
                if (ImGui.DragFloat3("##Rotation", ref _rot))
                {
                    R_3D.Objects[selectedObject] = new R_3D.Object
                    {
                        RelTransform = R_3D.Objects[selectedObject].RelTransform,
                        Name = R_3D.Objects[selectedObject].Name,
                        ID = R_3D.Objects[selectedObject].ID,
                        Material = R_3D.Objects[selectedObject].Material,
                        VertData = R_3D.Objects[selectedObject].VertData,
                        Indices = R_3D.Objects[selectedObject].Indices,
                        Location = R_3D.Objects[selectedObject].Location,
                        Rotation = new Vector3(_rot.X, _rot.Y, _rot.Z),
                        Scale = R_3D.Objects[selectedObject].Scale
                    };
                }

                ImGui.SameLine();
                if (ImGui.Button("##2", new System.Numerics.Vector2(20f)))
                {
                    R_3D.Objects[selectedObject] = new R_3D.Object
                    {
                        RelTransform = R_3D.Objects[selectedObject].RelTransform,
                        Name = R_3D.Objects[selectedObject].Name,
                        ID = R_3D.Objects[selectedObject].ID,
                        Material = R_3D.Objects[selectedObject].Material,
                        VertData = R_3D.Objects[selectedObject].VertData,
                        Indices = R_3D.Objects[selectedObject].Indices,
                        Location = R_3D.Objects[selectedObject].Location,
                        Rotation = new Vector3(0.0f),
                        Scale = R_3D.Objects[selectedObject].Scale
                    };
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                // Scale
                System.Numerics.Vector3 _scale = new System.Numerics.Vector3(
                    R_3D.Objects[selectedObject].Scale.X,
                    R_3D.Objects[selectedObject].Scale.Y,
                    R_3D.Objects[selectedObject].Scale.Z);
                ImGui.Text("Scale");
                if (ImGui.DragFloat3("##Scale", ref _scale, 0.1f))
                {
                    R_3D.Objects[selectedObject] = new R_3D.Object
                    {
                        RelTransform = R_3D.Objects[selectedObject].RelTransform,
                        Name = R_3D.Objects[selectedObject].Name,
                        ID = R_3D.Objects[selectedObject].ID,
                        Material = R_3D.Objects[selectedObject].Material,
                        VertData = R_3D.Objects[selectedObject].VertData,
                        Indices = R_3D.Objects[selectedObject].Indices,
                        Location = R_3D.Objects[selectedObject].Location,
                        Rotation = R_3D.Objects[selectedObject].Rotation,
                        Scale = new Vector3(_scale.X, _scale.Y, _scale.Z)
                    };
                }

                ImGui.SameLine();
                if (ImGui.Button("##3", new System.Numerics.Vector2(20f)))
                {
                    R_3D.Objects[selectedObject] = new R_3D.Object
                    {
                        RelTransform = R_3D.Objects[selectedObject].RelTransform,
                        Name = R_3D.Objects[selectedObject].Name,
                        ID = R_3D.Objects[selectedObject].ID,
                        Material = R_3D.Objects[selectedObject].Material,
                        VertData = R_3D.Objects[selectedObject].VertData,
                        Indices = R_3D.Objects[selectedObject].Indices,
                        Location = R_3D.Objects[selectedObject].Location,
                        Rotation = R_3D.Objects[selectedObject].Rotation,
                        Scale = new Vector3(1.0f)
                    };
                }

                ImGui.TreePop();
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            if (ImGui.TreeNode("Information"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("Vertices: " + (R_3D.Objects[selectedObject].VertData.Length).ToString());
                ImGui.Text("Triangles: " + (R_3D.Objects[selectedObject].Indices.Length / 3).ToString());
            }

            ImGui.End();
        }

        public static void LoadLightProperties(ref int selectedLight, float spacing)
        {
            // Object Properties
            ImGui.Begin("Light Properties");
            ImGui.Text(R_3D.Lights[selectedLight].Name);
            string inpstr = R_3D.Lights[selectedLight].Name;
            if (ImGui.InputTextWithHint("##Name", inpstr, ref inpstr, 30))
            {
                R_3D.Lights[selectedLight] = new R_3D.Light
                {
                    Strength = R_3D.Lights[selectedLight].Strength,
                    Radius = R_3D.Lights[selectedLight].Radius,
                    FallOff = R_3D.Lights[selectedLight].FallOff,
                    Type = R_3D.Lights[selectedLight].Type,
                    Name = inpstr,
                    ID = R_3D.Lights[selectedLight].ID,
                    LightColor = R_3D.Lights[selectedLight].LightColor,
                    Shader = R_3D.Lights[selectedLight].Shader,
                    VertData = R_3D.Lights[selectedLight].VertData,
                    Indices = R_3D.Lights[selectedLight].Indices,
                    Location = R_3D.Lights[selectedLight].Location,
                    Rotation = R_3D.Lights[selectedLight].Rotation,
                };
            }

            ImGui.SameLine(ImGui.GetWindowWidth() - 60);

            if (ImGui.Button("Delete"))
            {
                if (selectedLight > 0)
                {
                    R_3D.Lights.RemoveAt(selectedLight);
                    R_3D.VAOlights.RemoveAt(selectedLight);
                    selectedLight -= 1;
                }
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            //  ----- Transform -----
            if (ImGui.TreeNode("Transform"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                // Location
                System.Numerics.Vector3 _loc = new System.Numerics.Vector3(
                    R_3D.Lights[selectedLight].Location.X,
                    R_3D.Lights[selectedLight].Location.Y,
                    R_3D.Lights[selectedLight].Location.Z);
                ImGui.Text("Location");
                if (ImGui.DragFloat3("##Location", ref _loc, 0.1f))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Strength = R_3D.Lights[selectedLight].Strength,
                        Radius = R_3D.Lights[selectedLight].Radius,
                        FallOff = R_3D.Lights[selectedLight].FallOff,
                        Type = R_3D.Lights[selectedLight].Type,
                        Name = R_3D.Lights[selectedLight].Name,
                        ID = R_3D.Lights[selectedLight].ID,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = new Vector3(_loc.X, _loc.Y, _loc.Z),
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                    };
                }

                ImGui.SameLine();
                if (ImGui.Button("##1", new System.Numerics.Vector2(20f)))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Strength = R_3D.Lights[selectedLight].Strength,
                        Radius = R_3D.Lights[selectedLight].Radius,
                        FallOff = R_3D.Lights[selectedLight].FallOff,
                        Type = R_3D.Lights[selectedLight].Type,
                        Name = R_3D.Lights[selectedLight].Name,
                        ID = R_3D.Lights[selectedLight].ID,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = new Vector3(0.0f),
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                    };
                }

                if (R_3D.Lights[selectedLight].Type == 2)
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Separator();
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                    // Rotation
                    System.Numerics.Vector3 _rot = new System.Numerics.Vector3(
                        R_3D.Lights[selectedLight].Rotation.X,
                        R_3D.Lights[selectedLight].Rotation.Y,
                        R_3D.Lights[selectedLight].Rotation.Z);
                    ImGui.Text("Rotation");
                    if (ImGui.DragFloat3("##Rotation", ref _rot))
                    {
                        R_3D.Lights[selectedLight] = new R_3D.Light
                        {
                            Strength = R_3D.Lights[selectedLight].Strength,
                            Radius = R_3D.Lights[selectedLight].Radius,
                            FallOff = R_3D.Lights[selectedLight].FallOff,
                            Type = R_3D.Lights[selectedLight].Type,
                            Name = R_3D.Lights[selectedLight].Name,
                            ID = R_3D.Lights[selectedLight].ID,
                            LightColor = R_3D.Lights[selectedLight].LightColor,
                            Shader = R_3D.Lights[selectedLight].Shader,
                            VertData = R_3D.Lights[selectedLight].VertData,
                            Indices = R_3D.Lights[selectedLight].Indices,
                            Location = R_3D.Lights[selectedLight].Location,
                            Rotation = new Vector3(_rot.X, _rot.Y, _rot.Z),
                        };
                    }

                    ImGui.SameLine();
                    if (ImGui.Button("##2", new System.Numerics.Vector2(20f)))
                    {
                        R_3D.Lights[selectedLight] = new R_3D.Light
                        {
                            Strength = R_3D.Lights[selectedLight].Strength,
                            Type = R_3D.Lights[selectedLight].Type,
                            Name = R_3D.Lights[selectedLight].Name,
                            ID = R_3D.Lights[selectedLight].ID,
                            LightColor = R_3D.Lights[selectedLight].LightColor,
                            Shader = R_3D.Lights[selectedLight].Shader,
                            VertData = R_3D.Lights[selectedLight].VertData,
                            Indices = R_3D.Lights[selectedLight].Indices,
                            Location = R_3D.Lights[selectedLight].Location,
                            Rotation = new Vector3(0.0f),
                        };
                    }
                }

                ImGui.TreePop();
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            if (ImGui.TreeNode("Settings"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                System.Numerics.Vector3 _color = new System.Numerics.Vector3(
                    R_3D.Lights[selectedLight].LightColor.X,
                    R_3D.Lights[selectedLight].LightColor.Y,
                    R_3D.Lights[selectedLight].LightColor.Z);
                ImGui.Text("Light Color");
                if (ImGui.ColorEdit3("##Light Color", ref _color))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Strength = R_3D.Lights[selectedLight].Strength,
                        Radius = R_3D.Lights[selectedLight].Radius,
                        FallOff = R_3D.Lights[selectedLight].FallOff,
                        Type = R_3D.Lights[selectedLight].Type,
                        Name = R_3D.Lights[selectedLight].Name,
                        ID = R_3D.Lights[selectedLight].ID,
                        LightColor = new Vector3(_color.X, _color.Y, _color.Z),
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = R_3D.Lights[selectedLight].Location,
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                    };
                }


                if (R_3D.Lights[selectedLight].Type == 0)
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Separator();
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                    float falloff = R_3D.Lights[selectedLight].FallOff;
                    float radius = R_3D.Lights[selectedLight].Radius;
                    ImGui.Text("Radius");
                    if (ImGui.SliderFloat("##Radius", ref radius, 0, 10.0f))
                    {
                        R_3D.Lights[selectedLight] = new R_3D.Light
                        {
                            Strength = R_3D.Lights[selectedLight].Strength,
                            Radius = radius,
                            FallOff = R_3D.Lights[selectedLight].FallOff,
                            Type = R_3D.Lights[selectedLight].Type,
                            Name = R_3D.Lights[selectedLight].Name,
                            ID = R_3D.Lights[selectedLight].ID,
                            LightColor = R_3D.Lights[selectedLight].LightColor,
                            Shader = R_3D.Lights[selectedLight].Shader,
                            VertData = R_3D.Lights[selectedLight].VertData,
                            Indices = R_3D.Lights[selectedLight].Indices,
                            Location = R_3D.Lights[selectedLight].Location,
                            Rotation = R_3D.Lights[selectedLight].Rotation,
                        };
                    }

                    ImGui.Text("Falloff");
                    if (ImGui.SliderFloat("##Falloff", ref falloff, 1, 5))
                    {
                        R_3D.Lights[selectedLight] = new R_3D.Light
                        {
                            Strength = R_3D.Lights[selectedLight].Strength,
                            Radius = R_3D.Lights[selectedLight].Radius,
                            FallOff = falloff,
                            Type = R_3D.Lights[selectedLight].Type,
                            Name = R_3D.Lights[selectedLight].Name,
                            ID = R_3D.Lights[selectedLight].ID,
                            LightColor = R_3D.Lights[selectedLight].LightColor,
                            Shader = R_3D.Lights[selectedLight].Shader,
                            VertData = R_3D.Lights[selectedLight].VertData,
                            Indices = R_3D.Lights[selectedLight].Indices,
                            Location = R_3D.Lights[selectedLight].Location,
                            Rotation = R_3D.Lights[selectedLight].Rotation,
                        };
                    }
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                float strength = R_3D.Lights[selectedLight].Strength;
                ImGui.Text("Strength");
                if (ImGui.SliderFloat("##Strength", ref strength, 0.0f, 10))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Strength = strength,
                        Radius = R_3D.Lights[selectedLight].Radius,
                        FallOff = R_3D.Lights[selectedLight].FallOff,
                        Type = R_3D.Lights[selectedLight].Type,
                        Name = R_3D.Lights[selectedLight].Name,
                        ID = R_3D.Lights[selectedLight].ID,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = R_3D.Lights[selectedLight].Location,
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                    };
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                ImGui.TreePop();
            }

            ImGui.End();
        }

        public static void HelpMarker(string desc)
        {
            ImGui.TextDisabled("(?)");

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted(desc);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
        }

        public static void LoadOutliner(ref int selectedObject, ref int selectedLight, float spacing)
        {
            // Outliner
            //ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(25, 25, 25, 255) / 255);

            ImGui.Begin("Outliner");

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            if (ImGui.TreeNode("Spawn"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                if (ImGui.BeginTabBar("##SpawnMenu"))
                {
                    if (ImGui.BeginTabItem("Primitives"))
                    {
                        ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                        if (ImGui.Button("Plane"))
                        {
                            R_3D.AddObjectToArray(false, "Plane", Main.M_Default, new Vector3(1f), new Vector3(0), new Vector3(0f), Plane.vertices, Plane.indices);
                            R_3D.ConstructObjects();
                            Main.selectedObject = R_3D.Objects.Count - 1;
                        }

                        ImGui.SameLine();

                        if (ImGui.Button("Cube"))
                        {
                            R_3D.AddObjectToArray(false, "Cube", Main.M_Default, new Vector3(1f), new Vector3(0), new Vector3(0f), Cube.vertices, Cube.indices);
                            R_3D.ConstructObjects();
                            Main.selectedObject = R_3D.Objects.Count - 1;
                        }

                        ImGui.SameLine();

                        if (ImGui.Button("Import Mesh"))
                        {
                            OpenFileDialog selectFile = new OpenFileDialog
                            {
                                Title = "Select File",
                                Filter = "FBX Files (*.fbx)|*.fbx"
                            };
                            selectFile.ShowDialog();

                            string path = selectFile.FileName;

                            R_Loading.LoadModel(path);
                            R_3D.AddObjectToArray(false, R_Loading.importname, Main.M_Default,
                                new Vector3(2f),            // Scale
                                new Vector3(0, 4, 0),       // Location
                                new Vector3(180f, 90f, 0f), // Rotation
                                R_Loading.importedData, R_Loading.importindices);
                            R_3D.ConstructObjects();
                            Main.selectedObject = R_3D.Objects.Count - 1;
                        }

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Lights"))
                    {
                        ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                        if (ImGui.Button("Point Light"))
                        {
                            R_Loading.LoadModel("./../../../Engine/Engine_Resources/Primitives/PointLightMesh.fbx");
                            R_3D.AddLightToArray(1, 5, 1, 0, "Point Light", new Vector3(1f), Main.LightShader, new Vector3(1f), new Vector3(0f), new Vector3(0f), R_Loading.importedData, R_Loading.importindices);
                            R_3D.ConstructLights();
                            Main.selectedLight = 0;
                        }

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Miscellaneous"))
                    {
                        ImGui.Text("Some cool stuff here");
                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }
                ImGui.TreePop();
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            string[] _objects = new string[R_3D.Objects.Count];
            string[] _lights = new string[R_3D.Lights.Count];

            if (ImGui.BeginListBox("##ObjectsList", new System.Numerics.Vector2(ImGui.GetWindowWidth() - 20, ImGui.GetWindowHeight() - 75)))
            {
                for (int i = 0; i < R_3D.Lights.Count; i++)
                {
                    _lights[i] = R_3D.Lights[i].Name;
                    if (ImGui.Selectable(" " + _lights[i] + "##" + R_3D.Lights[i].ID, selectedLight == i))
                    {
                        selectedLight = i;
                    }

                    ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));
                }

                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));

                for (int i = 0; i < R_3D.Objects.Count; i++)
                {
                    _objects[i] = R_3D.Objects[i].Name;

                    if (ImGui.Selectable(" " + _objects[i] + "##" + R_3D.Objects[i].ID, selectedObject == i))
                    {
                        selectedObject = i;
                    }

                    ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));
                }

                ImGui.EndListBox();
            }

            ImGui.EndTabBar();
            ImGui.End();

            //ImGui.PopStyleColor();
        }
    }
}
