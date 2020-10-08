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
        public static IHowlGlobal HowlGlobal { get; set; }
        public static List<Sound> Sounds { get; set; }
        public static bool MuteAllSounds { get; set; }
        public delegate void SoundStoppedEventHandler(Howler.Blazor.Components.Events.HowlEventArgs e);
        public static SoundStoppedEventHandler OnEnd;
        public static bool SoundIsOff { get; set; }

        public enum SoundManagerSounds
        {
            fire,
            bluebughit,
            dive,
            redbughit,
            galagahit,
            galagadestroyed,
            breathing,
            tractorbeam,
            tractorbeamcapture,
            fightercapturedsong,
            introsong,
            coin,
            empty,
            levelup,
            fighterrescuedsong,
            challengingstage,
            challengingstageover,
            challengingstageperfect,
            capturedfighterdestroyedsong
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

            Howl.Load();

            // Register callbacks
            Howl.OnPlay += e =>
            {
                //Sounds.ForEach(a => {
                //    if (e.SoundId == a.SoundId) a.IsPlaying = true;
                //});
            };

            Howl.OnEnd += e =>
            {
                Sounds.Where(a=>a.SoundId==e.SoundId).ToList().ForEach(a => {
                     a.IsPlaying = false;
                });
                SoundStoppedEventHandler handler = OnEnd;
                if (handler != null) handler(e);
            };

            PlaySound(SoundManagerSounds.empty);
        }

        public static async void TurnSoundOff()
        {
            await HowlGlobal.Mute(true);

            SoundIsOff = true;
        }

        public static void StopAllSounds()
        {
            Howl.Stop();
            Sounds.ForEach(a => Howl.Pause(a.SoundId));
        }

        public static async void PlaySound(SoundManagerSounds sound, bool oneatatime = false,bool excludefrommute=false)
        {
          
            if (MuteAllSounds && !excludefrommute) return;

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
