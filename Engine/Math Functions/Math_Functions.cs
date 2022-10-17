﻿using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTK_Learning
{
    class Math_Functions
    {
        public static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) * (to2 - from2) / (to1 - from1) + from2;
        }

        public static int RandInt(int min, int max)
        {
            Random rnd = new Random();
            int value = rnd.Next(min, max);

            return value;
        }

        public static float RandFloat(float min, float max)
        {
            Random rnd = new Random();
            float value = Map((float)rnd.NextDouble(), 0, 1, min, max);

            return value;
        }

        public static bool ToggleBool(bool toggleBool)
        {
            bool _bool = false;

            if (toggleBool == true) _bool = false;
            if (toggleBool == false) _bool = true;

            return _bool;
        }

        // FPS calc
        static double previousTime = GLFW.GetTime();
        static int frameCount = 0;

        public static float FPS;
        public static float ms;

        public static float CalcFPS()
        {
            // Calculate FPS
            double currentTime = GLFW.GetTime();
            frameCount++;
            if (currentTime - previousTime >= 1.0)
            {
                FPS = frameCount;
                ms = (float)((currentTime - previousTime) / frameCount) * 1000;
                frameCount = 0;
                previousTime = currentTime;
            }

            return FPS;
        }
    }
}
