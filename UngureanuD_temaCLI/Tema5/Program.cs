
// Laborator EGC - Ungureanu Diana, grupa 3132A

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;

namespace ControlObiectRandareOpenTK
{
    class Game : GameWindow
    {
        private Vector3 cubePosition = Vector3.Zero;
        private Color4[] faceColors = new Color4[6]; // Culoarea fiecărei fețe a cubului
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;
        private Random random = new Random();

        public Game(int width, int height) : base(width, height, GraphicsMode.Default, "Control Cub 3D cu Tastatură")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color4.Pink);
            GL.Enable(EnableCap.DepthTest); // Activăm testul de adâncime

            // Încarcă coordonatele cubului dintr-un fișier text
            if (File.Exists("cube_coordinates.txt"))
            {
                string[] lines = File.ReadAllLines("cube_coordinates.txt");
                if (lines.Length == 3)
                {
                    float x = float.Parse(lines[0]);
                    float y = float.Parse(lines[1]);
                    float z = float.Parse(lines[2]);
                    cubePosition = new Vector3(x, y, z);
                }
            }

            // Inițializează culorile pentru fiecare față a cubului
            for (int i = 0; i < faceColors.Length; i++)
            {
                faceColors[i] = new Color4(1f, 1f, 1f, 1f);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState keyboard = Keyboard.GetState();

            // Modifică culoarea fețelor cu tastele
            if (keyboard[Key.R]) faceColors[0].R = Math.Min(faceColors[0].R + 0.01f, 1.0f); // Crește roșu pe fața 1
            if (keyboard[Key.F]) faceColors[0].R = Math.Max(faceColors[0].R - 0.01f, 0.0f); // Scade roșu pe fața 1

            if (keyboard[Key.G]) faceColors[0].G = Math.Min(faceColors[0].G + 0.01f, 1.0f); // Crește verde
            if (keyboard[Key.H]) faceColors[0].G = Math.Max(faceColors[0].G - 0.01f, 0.0f); // Scade verde

            if (keyboard[Key.B]) faceColors[0].B = Math.Min(faceColors[0].B + 0.01f, 1.0f); // Crește albastru
            if (keyboard[Key.N]) faceColors[0].B = Math.Max(faceColors[0].B - 0.01f, 0.0f); // Scade albastru

            if (keyboard[Key.T]) faceColors[0].A = Math.Min(faceColors[0].A + 0.01f, 1.0f); // Crește transparența
            if (keyboard[Key.Y]) faceColors[0].A = Math.Max(faceColors[0].A - 0.01f, 0.0f); // Scade transparența
            
            // Randomizează culorile fețelor la apăsarea tastei Space
            if (keyboard[Key.Space])
            {
                for (int i = 0; i < faceColors.Length; i++)
                {
                    faceColors[i] = new Color4(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        1.0f); // Opacitate maximă
                }
            }

            // Rotește cubul cu tastele săgeți
            if (keyboard[Key.Left]) rotationY -= 1.0f;
            if (keyboard[Key.Right]) rotationY += 1.0f;
            if (keyboard[Key.Up]) rotationX -= 1.0f;
            if (keyboard[Key.Down]) rotationX += 1.0f;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(cubePosition);
            GL.Rotate(rotationX, 1.0f, 0.0f, 0.0f);
            GL.Rotate(rotationY, 0.0f, 1.0f, 0.0f);

            DrawCube();

            SwapBuffers();
        }

        private void DrawCube()
        {
            // Desenăm fiecare față a cubului cu coordonatele specifice pentru fiecare față și culoare
            GL.Begin(PrimitiveType.Quads);

            // Fața frontală
            GL.Color4(faceColors[0]);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);

            // Fața din spate
            GL.Color4(faceColors[1]);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);

            // Fața de sus
            GL.Color4(faceColors[2]);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);

            // Fața de jos
            GL.Color4(faceColors[3]);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);

            // Fața din dreapta
            GL.Color4(faceColors[4]);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);

            // Fața din stânga
            GL.Color4(faceColors[5]);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);

            GL.End();
        }

        [STAThread]
        static void Main()
        {
            using (var game = new Game(800, 600))
            {
                game.Run(60.0);
            }
        }
    }
}