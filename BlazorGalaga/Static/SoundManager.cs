using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorGalaga.Static
{
    public static class SoundManager
    {
        public static IHowl Howl { get; set; }
        public static List<Sound> Sounds { get; set; }

        public enum SoundManagerSounds
        {
            fire,
            bluebughit,
            dive,
            redbughit,
            galagahit,
            galagadesgtroyed,
            breathing,
            tractorbeam,
            tractorbeamcapture,
            fightercapturedsong
        }

        public class Sound
        {
            public SoundManagerSounds SoundName { get; set; }
            public int SoundId { get; set; }
            public bool IsPlaying { get; set; }
        }


        public static void Init()
        {
            Sounds = new List<Sound>();

            // Register callbacks
            Howl.OnPlay += e =>
            {
                //Sounds.ForEach(a => {
                //    if (e.SoundId == a.SoundId) a.IsPlaying = true;
                //});
            };

            Howl.OnEnd += e =>
            {
                Sounds.ForEach(a => {
                    if (e.SoundId == a.SoundId) a.IsPlaying = false;
                });
            };

        }

        public static async void PlaySound(SoundManagerSounds sound, bool oneatatime = false,bool stopallsounds =false)
        {
            if (stopallsounds) await Howl.Stop();

            if (oneatatime)
            {
                if (Sounds.Any(a => a.SoundName == sound && a.IsPlaying)) return;
            }

            var options = new HowlOptions
            {
                Sources = new[] { "/Assets/sounds/" + Enum.GetName(typeof(SoundManagerSounds), sound) + ".mp3" },
                Formats = new[] { "mp3" }
            };

            var soundid =  await Howl.Play(options);

            if (!Sounds.Any(a => a.SoundName == sound))
            {
                Sounds.Add(new Sound()
                {
                    SoundId = soundid,
                    SoundName = sound, 
                    IsPlaying = true
                });
            }
            else
            {
                Sounds.FirstOrDefault(a => a.SoundName == sound).SoundId = soundid;
                Sounds.FirstOrDefault(a => a.SoundName == sound).IsPlaying = true;
            }
        }
    }
}
