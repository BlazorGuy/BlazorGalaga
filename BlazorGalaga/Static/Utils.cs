using System;
using System.Diagnostics;

namespace BlazorGalaga.Static
{
    public static class Utils
    {
        private static long framesRendered = 0;
        private static Stopwatch timer = new Stopwatch();

        public static void LogFPS()
        {
            framesRendered += 1;
            if (!timer.IsRunning) timer.Start();

            if (timer.ElapsedMilliseconds >= 1000)
            {
                Console.WriteLine("FPS: " + framesRendered);
                framesRendered = 0;
                timer.Restart();
            }
        }
    }
}
