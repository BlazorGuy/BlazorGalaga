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

        private static bool diveplaying = false;
        private static int divesoundid;

        private static bool breathplaying = false;
        private static int breathsoundid;

        private static bool tractorbeamplaying = false;
        private static int tractorbeamsoundid;

        private static bool tractorbeamcaptureplaying = false;
        private static int tractorbeamcapturesoundid;

        public static void Init()
        {
            // Register callbacks
            Howl.OnPlay += e =>
            {
                //if (e.SoundId == divesoundid) diveplaying = true;
                //if (e.SoundId == breathsoundid) breathplaying = true;
            };

            Howl.OnEnd += e =>
            {
                if (e.SoundId == divesoundid) diveplaying = false;
                if (e.SoundId == breathsoundid) breathplaying = false;
                if (e.SoundId == tractorbeamsoundid) tractorbeamplaying = false;
                if (e.SoundId == tractorbeamcapturesoundid) tractorbeamcaptureplaying = false;
            };

        }

        private static async ValueTask<int> PlaySound(string source)
        {
            var options = new HowlOptions
            {
                Sources = new[] { source },
                Formats = new[] { "mp3" }
            };

            return await Howl.Play(options);
        }

        public static async void PlayFire()
        {
            await PlaySound("http://localhost:30873/Assets/sounds/fire.mp3");
        }

        public static async void PlayBlueBugHit()
        {
            await PlaySound("http://localhost:30873/Assets/sounds/bluebughit.mp3");
        }

        public static async void PlayDive()
        {
            if (diveplaying) return;

            diveplaying = true; 

            divesoundid = await PlaySound("http://localhost:30873/Assets/sounds/dive.mp3");
        }

        public static async void PlayRedBugHit()
        {
            await PlaySound("http://localhost:30873/Assets/sounds/redbughit.mp3");
        }

        public static async void PlayGalagaHit()
        {
            await PlaySound("http://localhost:30873/Assets/sounds/galagahit.mp3");
        }

        public static async void PlayGalagaDestroyed()
        {
            await PlaySound("http://localhost:30873/Assets/sounds/galagadestroyed.mp3");
        }
        public static async void PlayBreathing()
        {
            if (breathplaying) return;

            breathplaying = true;

            breathsoundid = await PlaySound("http://localhost:30873/Assets/sounds/breathing.mp3");
        }

        public static async void PlayTractorBeam()
        {
            if (tractorbeamplaying) return;

            tractorbeamplaying = true;

            await PlaySound("http://localhost:30873/Assets/sounds/tractorbeam.mp3");
        }

        public static async void PlayTractorBeamCapture()
        {

            if (tractorbeamcaptureplaying) return;

            tractorbeamcaptureplaying = true;

            await Howl.Stop();

            await PlaySound("http://localhost:30873/Assets/sounds/tractorbeamcapture.mp3");
        }
    }
}
