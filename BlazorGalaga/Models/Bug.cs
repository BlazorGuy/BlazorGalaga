using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Bug : AnimatableBase
    {
        public enum enCaptureState
        {
            NotStarted,
            Started,
            FlyingBackHome,
            RecaptureStarted,
            Complete,
        }

        public Point HomePoint { get; set; }
        public bool IsDiving { get; set; }
        public bool IsExploding { get; set; }
        public List<Bug> ChildBugs { get; set; }
        public bool IsDiveBomber { get; set; }
        public PointF DiveBombLocation { get; set; }
        public int Wave { get; set; }
        public Point ChildBugOffset { get; set; }
        public string Tag { get; set; }
        public bool OutputDebugInfo { get; set; }
        public int HomePointYOffset { get; set; }
        public Bug CapturedBug { get; set; }
        public enCaptureState CaptureState { get; set; }
        public bool FighterCapturedMessageShowing { get; set; }
        public bool ClearFighterCapturedMessage { get; set; }

        public bool AligningHorizontally { get; set; }
        public bool AligningVertically { get; set; }

        public List<int> MissileCountDowns { get; set; }

        public bool IsInIntro { get; set; }

        public Bug(Sprite.SpriteTypes spritetype)
        {
            Sprite = new Sprite(spritetype);
            SpriteBank = new List<Sprite>();
            ChildBugs = new List<Bug>();
            CaptureState = enCaptureState.NotStarted;
            MissileCountDowns = new List<int>();
        }
    }
}
