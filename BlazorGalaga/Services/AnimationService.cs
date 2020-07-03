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
            var shipAnimation = new Animation() { Speed = 0 };
            var ship = new Ship()
            {
                Path = new BezierCurve()
                {
                    StartPoint = new PointF(0, Constants.CanvasSize.Height-Constants.SpriteDestSize.Height),
                    EndPoint = new PointF(Constants.CanvasSize.Width-Constants.SpriteDestSize.Width, 900),
                    ControlPoint1 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                    ControlPoint2 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height)
                },
                DrawPath = false,
                PathIsLine = true,
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
                        animatable.Location = bezierCurveService.getLineXYatPercent(animatable.Path, animation.Percent); 
                    else
                        animatable.Location = bezierCurveService.getCubicBezierXYatPercent(animatable.Path, animation.Percent);
                    if (animatable.DrawPath) bezierCurveService.DrawCurve(CanvasCtx, animatable.Path);
                }

                if (animatable.Sprite!=null)
                    spriteService.DrawSprite(animatable.Sprite,animatable.Location);
            }
        }
    }
}
