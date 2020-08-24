using BlazorGalaga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class FlapWingsManager
    {
        public static int WingFlapCount;
        public static float LastWingFlapTimeStamp = 0;

        public static void FlapWings(List<Bug> bugs)
        {
            WingFlapCount++;

            bugs.Where(a => a.Started && a.SpriteBank != null).ToList().ForEach(bug =>
            {
                if (bug.IsMoving)
                {
                    if (bug.IsDiving)
                        bug.SpriteBankIndex = Utils.Rnd(1, 10) > 6 ? null : (int?)0;
                    else
                        bug.SpriteBankIndex = WingFlapCount % 4 == 0 ? null : (int?)0;
                }
                else
                    bug.SpriteBankIndex = WingFlapCount % 2 == 0 ? null : (int?)0;
            });
        }
    }
}
