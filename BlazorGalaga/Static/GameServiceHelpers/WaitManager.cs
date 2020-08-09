using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class WaitManager
    {
        private static List<WaitStep> steps = new List<WaitStep>();

        public class WaitStep
        {
            public bool Complete { get; set; }
            public float TimeStamp { get; set; }
            public enStep Step { get; set; }
            public enum enStep
            {
                CleanUp,
                Pause1,
                ShowLevelText,
                Pause2,
                ClearLevelText
            }
        }

        public static void ClearSteps()
        {
            steps.Clear();
        }

        public static void DoOnce(Action action, WaitStep.enStep step)
        {
            var waitstep = steps.FirstOrDefault(a => a.Step == step);

            if (waitstep == null)
            {
                waitstep = new WaitStep()
                {
                    Step = step,
                    Complete = true
                };
                steps.Add(waitstep);
                action.Invoke();
            }
        }

        public static bool WaitFor(int milliseconds, float timestamp, WaitStep.enStep step)
        {
            var waitstep = steps.FirstOrDefault(a => a.Step == step);

            if (waitstep == null)
            {
                waitstep = new WaitStep()
                {
                    Step = step,
                    TimeStamp = timestamp
                };
                steps.Add(waitstep);
            }

            if (waitstep.Complete) return true;

            if (timestamp - waitstep.TimeStamp > milliseconds)
            {
                waitstep.Complete = true;
                return true;
            }
            else
                return false;
        }
    }
}
