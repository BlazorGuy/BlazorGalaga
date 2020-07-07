using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGalaga.Static
{
    public class dOutInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public static class Utils
    {
        public static string DiagnosticInfo
        {
            get
            {
                string ret="";
                dOuts.ToList().ForEach(a =>
                {
                    ret += a.Key + ": " + a.Value + "</br>";
                });
                return ret;
            }
        }

        public static float FPS;
        private static long framesRendered = 0;
        private static Stopwatch timer = new Stopwatch();
        private static List<dOutInfo> dOuts = new List<dOutInfo>();

        public static void dOut(string key, object value)
        {
            if (dOuts.Any(a => a.Key == key))
                dOuts.FirstOrDefault(a => a.Key == key).Value = value.ToString();
            else
                dOuts.Add(new dOutInfo() { Key = key, Value = value.ToString() });
        }

        public static void LogFPS()
        {
            framesRendered += 1;
            if (!timer.IsRunning) timer.Start();

            if (timer.ElapsedMilliseconds >= 1000)
            {
                FPS = framesRendered;
                dOut("FPS", FPS.ToString());
                framesRendered = 0;
                timer.Restart();
            }
        }
    }
}
