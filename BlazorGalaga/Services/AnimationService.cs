using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;
using BlazorGalaga.Static;

namespace BlazorGalaga.Services
{
    public class AnimationService
    {
        public List<Animation> Animations = new List<Animation>();
        public Canvas2DContext CanvasCtx { get; set; }

        private BezierCurveService bezierCurveService;
        private SpriteService spriteService;

        public AnimationService(BezierCurveService bcs, SpriteService ss)
        {
            bezierCurveService = bcs;
            spriteService = ss;
        }

        public void InitAnimations()
        {

            var bugAnimation = new Animation() { Speed = 1, LoopBack = true };
            var bug = new Bug()
            {
                Path = new BezierCurve()
                {
                    StartPoint = new PointF(Constants.CanvasSize.Width, Constants.CanvasSize.Height),
                    EndPoint = new PointF(Constants.CanvasSize.Width / 2, Constants.CanvasSize.Height / 2),
                    ControlPoint1 = new PointF(0, Constants.CanvasSize.Height / 2),
                    ControlPoint2 = new PointF(Constants.CanvasSize.Width / 2, 0)
                },
                DrawPath = false,
                RotateAlongPath = true
            };
            bugAnimation.Animatables.Add(bug);
            Animations.Add(bugAnimation);

            var shipAnimation = new Animation() { Speed = 0 };
            var ship = new Ship()
            {
                Path = new BezierCurve()
                {
                    StartPoint = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                    EndPoint = new PointF(Constants.CanvasSize.Width - Constants.SpriteDestSize.Width, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                    ControlPoint1 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                    ControlPoint2 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height)
                },
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false
            };
            shipAnimation.Animatables.Add(ship);
            Animations.Add(shipAnimation);

        }

        public async Task ResetCanvas()
        {
            await CanvasCtx.ClearRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
            await CanvasCtx.SetFillStyleAsync("#000000");
            await CanvasCtx.FillRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
        }

        public void Animate(Animation animation)
        {
            animation.Percent += animation.Speed;

            if (animation.Percent < 0)
            {
                animation.Percent = 0;
                if (animation.LoopBack) animation.Speed *= -1;
            }
            else if (animation.Percent > 100)
            {
                animation.Percent = 100;
                if (animation.LoopBack) animation.Speed *= -1;
            };

        }

        public void Draw(Animation animation)
        {
            foreach(var animatable in animation.Animatables)
            {
                if (animatable.Path != null)
                {
                    if (animatable.PathIsLine)
                    {
                        animatable.PevLocation = bezierCurveService.getLineXYatPercent(animatable.Path, animation.Percent - animation.Speed);
                        animatable.Location = bezierCurveService.getLineXYatPercent(animatable.Path, animation.Percent);
                        animatable.NextLocation = bezierCurveService.getLineXYatPercent(animatable.Path, animation.Percent + animation.Speed);
                    }
                    else
                    {
                        animatable.PevLocation = bezierCurveService.getCubicBezierXYatPercent(animatable.Path, animation.Percent - animation.Speed);
                        animatable.Location = bezierCurveService.getCubicBezierXYatPercent(animatable.Path, animation.Percent);
                        animatable.NextLocation = bezierCurveService.getCubicBezierXYatPercent(animatable.Path, animation.Percent + animation.Speed);
                    }
                    if (animatable.DrawPath) bezierCurveService.DrawCurve(CanvasCtx, animatable.Path);
                }

                if (animatable.Sprite != null)
                {
                    var rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);
                    spriteService.DrawSprite(animatable.Sprite, animatable.Location, animatable.RotateAlongPath ? rotation : 0);
                }
            }
        }
    }
}
