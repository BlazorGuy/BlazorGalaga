using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;

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

        public void InitSprites(CanvasDimension canvasdimension)
        {
            var shipAnimation = new Animation() { Speed = 2 };
            var ship = new Ship()
            {
                Path = new BezierCurve()
                {
                    StartPoint = new PointF(0, 0),
                    EndPoint = new PointF((int)canvasdimension.Width - 100, 0),
                    ControlPoint1 = new PointF(0, 0),
                    ControlPoint2 = new PointF(0, 0)
                },
                DrawPath = true,
                PathIsLine = true,
            };
            shipAnimation.Animatables.Add(ship);
            Animations.Add(shipAnimation);

        }

        public async Task ResetCanvas(CanvasDimension canvasdimension)
        {
            await CanvasCtx.ClearRectAsync(0, 0, (int)canvasdimension.Width, (int)canvasdimension.Height);
            await CanvasCtx.SetFillStyleAsync("#454545");
            await CanvasCtx.FillRectAsync(0, 0, (int)canvasdimension.Width, (int)canvasdimension.Height);
        }

        public void Animate(Animation animation, bool loopback = false)
        {
            animation.Percent += animation.Speed;

            if (animation.Percent < 0)
            {
                animation.Percent = 0;
                if (loopback) animation.Speed *= -1;
            }
            else if (animation.Percent > 100)
            {
                animation.Percent = 100;
                if (loopback) animation.Speed *= -1;
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
