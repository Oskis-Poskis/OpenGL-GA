using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static Maeve.Rendering;
using static Importer.Import;
using static Bernard.Setup;
using static Axyz.Main;
using StbImageSharp;
using ImGuiNET;
using System.Windows.Forms;
using System.IO;
using System;
using Axyz;

namespace AllImGUI
{
    class UserInterface
    {
        public static void LoadIcons()
        {
            resetButton = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, resetButton);
            StbImage.stbi_set_flip_vertically_on_load(0);
            using (Stream stream = File.OpenRead("./../../../Engine/Engine_Resources/Images/reset.png"))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }
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
            ImGui.GetIO().FontGlobalScale = fontSize;

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

            if (ImGui.GetWindowWidth() < 10 | ImGui.GetWindowHeight() < 10)
            {
                CameraWidth = 1920;
                CameraHeight = 1080;
            }

            else
            {
                CameraWidth = ImGui.GetWindowWidth();
                CameraHeight = ImGui.GetWindowHeight() - ImGui.GetIO().FontGlobalScale * 71;
            }
            

            isMainHovered = ImGui.IsWindowHovered();

            ImGui.Image((IntPtr)framebufferTexture,
                new System.Numerics.Vector2(CameraWidth, CameraHeight),
                new System.Numerics.Vector2(0.0f, 0.9f),
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
                    CloseWindow = true;
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
                    ImGui.Checkbox("Statistics", ref showStatistics);
                    ImGui.Separator();
                    ImGui.Checkbox("ImGUI Demo ", ref showDemoWindow);
                    ImGui.EndMenu();
                }

                ImGui.Separator();

                if (ImGui.BeginMenu("Editor"))
                {
                    ImGui.Checkbox("Show Object Properties", ref showObjectProperties);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Light Properties", ref showLightProperties); 
                    ImGui.Separator();
                    ImGui.Checkbox("Show Outliner", ref showOutliner);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Settings", ref showSettings);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Material Editor", ref showMaterialEditor);
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            ImGui.Dummy(new System.Numerics.Vector2(ImGui.GetWindowWidth() - 550, 0));
            ImGui.TextDisabled("Objects: " + (Objects.Count).ToString());
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
                ImGui.Text("Objects: " + (Objects.Count).ToString());
                ImGui.Text("Point Lights: " + (numPL).ToString());
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
                Objects[selectedObject].Material.albedo.X,
                Objects[selectedObject].Material.albedo.Y,
                Objects[selectedObject].Material.albedo.Z);
            float _roughness = Objects[selectedObject].Material.roughness;
            float _metallic = Objects[selectedObject].Material.metallic;
            float _ao = Objects[selectedObject].Material.ao;

            ImGui.Begin("Material Editor");
            ImGui.ColorEdit3("Albedo", ref _albedo);
            ImGui.SliderFloat("Roughness", ref _roughness, 0, 1);
            ImGui.SliderFloat("Metallic", ref _metallic, 0, 1);
            ImGui.SliderFloat("AO", ref _ao, 0, 1);

            Objects[Main.selectedObject].Material = new Material
            {
                albedo = new Vector3(_albedo.X, _albedo.Y, _albedo.Z),
                roughness = _roughness,
                metallic = _metallic,
                ao = _ao,
                Maps = Objects[selectedObject].Material.Maps
            };

            ImGui.End();
        }

        public static void LoadObjectProperties(ref int selectedObject, float spacing)
        {
            // Object Properties
            ImGui.Begin("Object Properties");
            ImGui.Text(Objects[selectedObject].Name);
            string inpstr = Objects[selectedObject].Name;
            if (ImGui.InputTextWithHint("##Name", inpstr, ref inpstr, 30))
            {
                Objects[selectedObject].Name = inpstr;
            }

            ImGui.SameLine(ImGui.GetWindowWidth() - 60);

            if (ImGui.Button("Delete"))
            {
                if (selectedObject > 0)
                {
                    Objects.RemoveAt(selectedObject);
                    VAO.RemoveAt(selectedObject);
                    selectedObject -= 1;
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
                    Objects[selectedObject].Location.X,
                    Objects[selectedObject].Location.Y,
                    Objects[selectedObject].Location.Z);
                ImGui.Text("Location");
                if (ImGui.DragFloat3("##Location", ref _loc, 0.1f))
                {
                    Objects[selectedObject].Location = new Vector3(_loc.X, _loc.Y, _loc.Z);
                }

                ImGui.SameLine();
                ImGui.PushID(0);
                if (ImGui.ImageButton((IntPtr)resetButton, new System.Numerics.Vector2(20), new System.Numerics.Vector2(0, 0), new System.Numerics.Vector2(1, 1), 0))
                {
                    Objects[selectedObject].Location = new Vector3(0.0f);
                }
                ImGui.PopID();

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                // Rotation
                System.Numerics.Vector3 _rot = new System.Numerics.Vector3(
                    Objects[selectedObject].Rotation.X,
                    Objects[selectedObject].Rotation.Y,
                    Objects[selectedObject].Rotation.Z);
                ImGui.Text("Rotation");
                if (ImGui.DragFloat3("##Rotation", ref _rot))
                {
                    Objects[selectedObject].Rotation = new Vector3(_rot.X, _rot.Y, _rot.Z);
                }

                ImGui.SameLine();
                ImGui.PushID(1);
                if (ImGui.ImageButton((IntPtr)resetButton, new System.Numerics.Vector2(20), new System.Numerics.Vector2(0, 0), new System.Numerics.Vector2(1, 1), 0))
                {
                    Objects[selectedObject].Rotation = new Vector3(0);
                }
                ImGui.PopID();

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                // Scale
                System.Numerics.Vector3 _scale = new System.Numerics.Vector3(
                    Objects[selectedObject].Scale.X,
                    Objects[selectedObject].Scale.Y,
                    Objects[selectedObject].Scale.Z);
                ImGui.Text("Scale");
                if (ImGui.DragFloat3("##Scale", ref _scale, 0.1f))
                {
                    Objects[selectedObject].Scale = new Vector3(_scale.X, _scale.Y, _scale.Z);
                }

                ImGui.SameLine();
                ImGui.PushID(2);
                if (ImGui.ImageButton((IntPtr)resetButton, new System.Numerics.Vector2(20), new System.Numerics.Vector2(0, 0), new System.Numerics.Vector2(1, 1), 0))
                {
                    Objects[selectedObject].Scale = new Vector3(1.0f);
                }
                ImGui.PopID();

                ImGui.TreePop();
            }

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            if (ImGui.TreeNode("Information"))
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Text("Vertices: " + (Objects[selectedObject].VertData.Length).ToString());
                ImGui.Text("Triangles: " + (Objects[selectedObject].Indices.Length / 3).ToString());
            }

            ImGui.End();
        }

        public static void LoadLightProperties(ref int selectedLight, float spacing)
        {
            // Object Properties
            ImGui.Begin("Light Properties");
            ImGui.Text(Lights[selectedLight].Name);
            string inpstr = Lights[selectedLight].Name;
            if (ImGui.InputTextWithHint("##Name", inpstr, ref inpstr, 30))
            {
                Lights[selectedLight].Name = inpstr;
            }

            ImGui.SameLine(ImGui.GetWindowWidth() - 60);

            if (ImGui.Button("Delete"))
            {
                if (selectedLight > 0)
                {
                    Lights.RemoveAt(selectedLight);
                    VAOlights.RemoveAt(selectedLight);
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
                    Lights[selectedLight].Location.X,
                    Lights[selectedLight].Location.Y,
                    Lights[selectedLight].Location.Z);
                ImGui.Text("Location");
                if (ImGui.DragFloat3("##Location", ref _loc, 0.1f))
                {
                    Lights[selectedLight].Location = new Vector3(_loc.X, _loc.Y, _loc.Z);
                }

                ImGui.SameLine();
                if (ImGui.Button("##1", new System.Numerics.Vector2(20f)))
                {
                    Lights[selectedLight].Location = new Vector3(0.0f);
                }

                if (Lights[selectedLight].Type == 2)
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Separator();
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                    // Rotation
                    System.Numerics.Vector3 _rot = new System.Numerics.Vector3(
                        Lights[selectedLight].Rotation.X,
                        Lights[selectedLight].Rotation.Y,
                        Lights[selectedLight].Rotation.Z);
                    ImGui.Text("Rotation");
                    if (ImGui.DragFloat3("##Rotation", ref _rot))
                    {
                        Lights[selectedLight].Rotation = new Vector3(_rot.X, _rot.Y, _rot.Z);
                    }

                    ImGui.SameLine();
                    if (ImGui.Button("##2", new System.Numerics.Vector2(20f)))
                    {
                        Lights[selectedLight].Rotation = new Vector3(0.0f);
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
                    Lights[selectedLight].LightColor.X,
                    Lights[selectedLight].LightColor.Y,
                    Lights[selectedLight].LightColor.Z);
                ImGui.Text("Light Color");
                if (ImGui.ColorEdit3("##Light Color", ref _color))
                {
                    Lights[selectedLight].LightColor = new Vector3(_color.X, _color.Y, _color.Z);
                }


                if (Lights[selectedLight].Type == 0)
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    ImGui.Separator();
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                    float falloff = Lights[selectedLight].FallOff;
                    float radius = Lights[selectedLight].Radius;
                    ImGui.Text("Radius");
                    if (ImGui.SliderFloat("##Radius", ref radius, 0, 10.0f))
                    {
                        Lights[selectedLight].Radius = radius;
                    }

                    ImGui.Text("Falloff");
                    if (ImGui.SliderFloat("##Falloff", ref falloff, 1, 5))
                    {
                        Lights[selectedLight].FallOff = falloff;
                    }
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

                float strength = Lights[selectedLight].Strength;
                ImGui.Text("Strength");
                if (ImGui.SliderFloat("##Strength", ref strength, 0.0f, 10))
                {
                    Lights[selectedLight].Strength = strength;
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

            if (ImGui.BeginTabBar("##SpawnMenu"))
            {
                if (ImGui.BeginTabItem("Primitives"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.Button("Plane"))
                    {
                        AddObjectToArray("Plane", M_Default, new Vector3(1f), new Vector3(0), new Vector3(0f), Plane.vertices, Plane.indices);
                        ConstructObjects();
                        Main.selectedObject = Objects.Count - 1;
                    }

                    ImGui.SameLine();

                    if (ImGui.Button("Cube"))
                    {
                        AddObjectToArray("Cube", M_Default, new Vector3(1f), new Vector3(0), new Vector3(0f), Cube.vertices, Cube.indices);
                        ConstructObjects();
                        selectedObject = Objects.Count - 1;
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

                        LoadModel(path);
                        AddObjectToArray(importname, M_Default,
                            new Vector3(2f),            // Scale
                            new Vector3(0, 4, 0),       // Location
                            new Vector3(180f, 90f, 0f), // Rotation
                            importedData, importindices);
                        ConstructObjects();
                        selectedObject = Objects.Count - 1;
                    }

                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Lights"))
                {
                    ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
                    if (ImGui.Button("Point Light"))
                    {
                        LoadModel("./../../../Engine/Engine_Resources/Primitives/PointLightMesh.fbx");
                        AddLightToArray(1, 5, 1, 0, "Point Light", new Vector3(1f), Main.LightShader, new Vector3(1f), new Vector3(0f), importedData, importindices);
                        ConstructLights();
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

            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

            string[] _objects = new string[Objects.Count];
            string[] _lights = new string[Lights.Count];

            for (int i = 0; i < Lights.Count; i++)
            {
                _lights[i] = Lights[i].Name;
                if (ImGui.Selectable(" " + _lights[i] + "##" + Lights[i].ID, selectedLight == i))
                {
                    selectedLight = i;
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));
            }

            ImGui.Separator();
            ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));

            for (int i = 0; i < Objects.Count; i++)
            {
                _objects[i] = Objects[i].Name;

                if (ImGui.Selectable(" " + _objects[i] + "##" + Objects[i].ID, selectedObject == i))
                {
                    selectedObject = i;
                }

                ImGui.Dummy(new System.Numerics.Vector2(0f, 0.2f));
            }

            ImGui.EndTabBar();
            ImGui.End();

            //ImGui.PopStyleColor();
        }
    }
}
