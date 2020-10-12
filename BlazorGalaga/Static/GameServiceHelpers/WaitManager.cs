using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class WaitManager
    {
        public static List<WaitStep> Steps = new List<WaitStep>();

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
                ClearLevelText,
                ShowBonusLabel,
                ShowBonus,
                ShowNumberOfHitsLabel,
                ShowNumberOfHits,
                Pause3,
                ShowReady,
                WaitReady
            }
        }

        public static void ClearSteps()
        {
            Steps.Clear();
        }

        public static void DoOnce(Action action, WaitStep.enStep step)
        {
            var waitstep = Steps.FirstOrDefault(a => a.Step == step);

            if (waitstep == null)
            {
                waitstep = new WaitStep()
                {
                    Step = step,
                    Complete = true
                };
                Steps.Add(waitstep);
                action.Invoke();
            }
        }

        public static bool WaitFor(int milliseconds, float timestamp, WaitStep.enStep step)
        {
            var waitstep = Steps.FirstOrDefault(a => a.Step == step);

            if (waitstep == null)
            {
                waitstep = new WaitStep()
                {
                    Step = step,
                    TimeStamp = timestamp
                };
                Steps.Add(waitstep);
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
