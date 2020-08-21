using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class GalagaCaptureManager
    {
        private static Ship CapturedShip;

        public static void DoTractorBeam(Bug bug, AnimationService animationService, Ship ship)
        {
            var tb = animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.TractorBeam).FirstOrDefault();

            if (tb == null)
            {
                tb = new TractorBeam()
                {
                    DrawPath = false,
                    RotateAlongPath = false,
                    Started = true,
                    DestroyAfterComplete = false,
                    IsMoving = false,
                    PathIsLine = true,
                    Location = new PointF(bug.Location.X - 93, bug.Location.Y - 5)
                };

                tb.SpriteBankIndex = 0;

                tb.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.TractorBeam));
                tb.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.TractorBeam2));
                tb.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.TractorBeam3));
                tb.SpriteBank.ForEach(a => {
                    a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, 0);
                    a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, 0);
                });

                animationService.Animatables.Add(tb);
            }
            else
            {
                if (tb.SpriteBank.First().DestRect.Value.Height < Constants.BiggerSpriteDestSize.Height)
                {
                    tb.SpriteBank.ForEach(a =>
                    {
                        a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.SourceRect.Value.Height + 20);
                        a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.DestRect.Value.Height + 20);
                    });
                }
                else
                {
                    ship.Visible = false;
                    if (CapturedShip==null)
                        CreateCapturedShip(animationService, ship, bug);
                    else
                    {
                        CapturedShip.Rotation += 5;
                    }
                }
                
                tb.SpriteBankIndex += 1;
                
                if (tb.SpriteBankIndex > 2)
                    tb.SpriteBankIndex = 0;
            }
        }

        private static void CreateCapturedShip(AnimationService animationService,Ship ship, Bug bug)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = ship.Location,
                EndPoint = ship.Location,
                ControlPoint1 = ship.Location,
                ControlPoint2 = ship.Location,
            });

            CapturedShip = new Ship()
            {
                Paths = paths,
                RotateAlongPath = false,
                Started = true,
                Index = -999,
                Speed = 5,
                RotateManually = true
            };

            CapturedShip.Paths.ForEach(a => {
                CapturedShip.PathPoints.AddRange(animationService.ComputePathPoints(a,true));
            });

            animationService.Animatables.Add(CapturedShip);
        }
    }
}
