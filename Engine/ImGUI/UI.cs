using System;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Learning
{
    class UI
    {
        public static void LoadTheme()
        {
            // Styling
            ImGui.GetStyle().FrameRounding = 2.5f;
            ImGui.GetStyle().FrameBorderSize = 1f;
            ImGui.GetStyle().TabRounding = 0f;
            ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.None;
            ImGui.GetIO().FontGlobalScale = Main.fontSize;

            // Something
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(25f, 25f, 25f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);

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
            ImGui.PushStyleColor(ImGuiCol.Header, new System.Numerics.Vector4(51f, 77f, 128f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(51f, 77f, 128f, 180f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(100f, 100f, 100f, 255f) / 255);

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
            // Game Window
            ImGui.Begin("Game");

            CameraWidth = ImGui.GetWindowWidth() - 18;
            CameraHeight = ImGui.GetWindowHeight() - 50;

            ImGui.Image((IntPtr)R_3D.framebufferTexture,
                new System.Numerics.Vector2(CameraWidth, CameraHeight),
                new System.Numerics.Vector2(0.0f, 1.0f),
                new System.Numerics.Vector2(1.0f, 0.0f),
                new System.Numerics.Vector4(1.0f),
                new System.Numerics.Vector4(1, 1, 1, 0.2f));
            ImGui.End();
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
                    ImGui.Checkbox("Statistics", ref Main.showStatistics); ImGui.SameLine(); ImGui.TextDisabled("Shortcut: 1");
                    ImGui.Separator();
                    ImGui.Checkbox("ImGUI Demo ", ref Main.showDemoWindow);
                    ImGui.EndMenu();
                }

                ImGui.Separator();

                if (ImGui.BeginMenu("Editor"))
                {
                    ImGui.Checkbox("Show Object Properties", ref Main.showObjectProperties); ImGui.SameLine(); ImGui.TextDisabled("Shortcut: 2");
                    ImGui.Separator();
                    ImGui.Checkbox("Show Light Properties", ref Main.showLightProperties); ImGui.SameLine(); ImGui.TextDisabled("Shortcut: 3");
                    ImGui.Separator();
                    ImGui.Checkbox("Show Outliner", ref Main.showOutliner); ImGui.SameLine(); ImGui.TextDisabled("Shortcut: 4");
                    ImGui.Separator();
                    ImGui.Checkbox("Show Settings", ref Main.showSettings); ImGui.SameLine(); ImGui.TextDisabled("Shortcut: 5");
                }
                ImGui.EndMenu();
            }

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
                ImGui.Text("Objects: " + R_3D.Objects.Count.ToString());
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
                ImGui.Text("Triangles: " + (R_3D.Objects[selectedObject].Indices.Length).ToString());
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
                    Name = inpstr,
                    LightColor = R_3D.Lights[selectedLight].LightColor,
                    Shader = R_3D.Lights[selectedLight].Shader,
                    VertData = R_3D.Lights[selectedLight].VertData,
                    Indices = R_3D.Lights[selectedLight].Indices,
                    Location = R_3D.Lights[selectedLight].Location,
                    Rotation = R_3D.Lights[selectedLight].Rotation,
                    Direction = R_3D.Lights[selectedLight].Direction
                };
            }

            ImGui.SameLine(ImGui.GetWindowWidth() - 60);

            if (ImGui.Button("Delete"))
            {
                if (selectedLight > 0)
                {
                    R_3D.Lights.RemoveAt(selectedLight);
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
                        Name = R_3D.Lights[selectedLight].Name,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = new Vector3(_loc.X, _loc.Y, _loc.Z),
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                        Direction = R_3D.Lights[selectedLight].Direction
                    };
                }

                ImGui.SameLine();
                if (ImGui.Button("##1", new System.Numerics.Vector2(20f)))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Name = R_3D.Objects[selectedLight].Name,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = new Vector3(0.0f),
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                        Direction = R_3D.Lights[selectedLight].Direction
                    };
                }

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
                        Name = R_3D.Lights[selectedLight].Name,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = R_3D.Lights[selectedLight].Location,
                        Rotation = new Vector3(_rot.X, _rot.Y, _rot.Z),
                        Direction = R_3D.Lights[selectedLight].Direction
                    };
                }

                ImGui.SameLine();
                if (ImGui.Button("##2", new System.Numerics.Vector2(20f)))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Name = R_3D.Lights[selectedLight].Name,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = R_3D.Lights[selectedLight].Location,
                        Rotation = new Vector3(0.0f),
                        Direction = R_3D.Lights[selectedLight].Direction
                    };
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
                        Name = R_3D.Lights[selectedLight].Name,
                        LightColor = new Vector3(_color.X, _color.Y, _color.Z),
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = R_3D.Lights[selectedLight].Location,
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                        Direction = R_3D.Lights[selectedLight].Direction
                    };
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                // Light direction
                System.Numerics.Vector3 direction = new System.Numerics.Vector3(
                    R_3D.Lights[selectedLight].Direction.X,
                    R_3D.Lights[selectedLight].Direction.Y,
                    R_3D.Lights[selectedLight].Direction.Z);
                ImGui.Text("Direction");
                if (ImGui.SliderFloat3("##Direction", ref direction, -1.0f, 1.0f))
                {
                    R_3D.Lights[selectedLight] = new R_3D.Light
                    {
                        Name = R_3D.Lights[selectedLight].Name,
                        LightColor = R_3D.Lights[selectedLight].LightColor,
                        Shader = R_3D.Lights[selectedLight].Shader,
                        VertData = R_3D.Lights[selectedLight].VertData,
                        Indices = R_3D.Lights[selectedLight].Indices,
                        Location = R_3D.Lights[selectedLight].Location,
                        Rotation = R_3D.Lights[selectedLight].Rotation,
                        Direction = new Vector3(direction.X, direction.Y, direction.Z)
                    };
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                ImGui.TreePop();
            }

            ImGui.End();
        }

        static void HelpMarker(string desc)
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
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(25, 25, 25, 255) / 255);

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
                            string PlaneName = "Plane";

                            for (int i = 0; i < R_3D.Objects.Count; i++)
                            {
                                if (R_3D.Objects[i].Name == PlaneName)
                                {
                                    PlaneName += i.ToString();
                                }
                            }

                            R_3D.AddObjectToArray(false, PlaneName, Main.M_Default, new Vector3(1f), new Vector3(0), new Vector3(0f), Plane.vertices, Plane.indices);
                            R_3D.ConstructObjects();
                        }

                        ImGui.SameLine();

                        if (ImGui.Button("Cube"))
                        {
                            string CubeName = "Cube";

                            for (int i = 0; i < R_3D.Objects.Count; i++)
                            {
                                if (R_3D.Objects[i].Name == CubeName)
                                {
                                    CubeName += i.ToString();
                                }
                            }

                            R_3D.AddObjectToArray(false, CubeName, Main.M_Default, new Vector3(1f), new Vector3(0), new Vector3(0f), Cube.vertices, Cube.indices);
                            R_3D.ConstructObjects();
                        }

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Lights"))
                    {
                        ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                        if (ImGui.Button("Point Lights"))
                        {
                            string LightName = "PointLight";

                            for (int i = 0; i < R_3D.Lights.Count; i++)
                            {
                                if (R_3D.Lights[i].Name == LightName)
                                {
                                    LightName += i.ToString();
                                }
                            }

                            R_3D.AddLightToArray(LightName, new Vector3(1f), Main.LightShader, new Vector3(1f), new Vector3(0f), new Vector3(0f), R_Loading.importedData, R_Loading.importindices);
                            R_3D.ConstructLights();
                        }
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Miscellaneous"))
                    {
                        ImGui.Text("Random stuff here");
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

            if (ImGui.BeginListBox("##ObjectsList", new System.Numerics.Vector2(ImGui.GetWindowWidth() - 20, ImGui.GetWindowHeight() - 100)))
            {
                for (int i = 0; i < R_3D.Lights.Count; i++)
                {
                    _lights[i] = R_3D.Lights[i].Name;
                    if (ImGui.Selectable("  " + _lights[i], selectedLight == i))
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

                    if (ImGui.Selectable("  " + _objects[i], selectedObject == i))
                    {
                        selectedObject = i;
                    }

                    ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));
                }

                ImGui.EndListBox();
            }

            ImGui.EndTabBar();
            ImGui.End();

            ImGui.PopStyleColor();
        }
    }
}
