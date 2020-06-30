using System;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;

namespace BlazorGalaga.Services
{
    public class AnimationService
    {
        public Canvas2DContext CanvasCtx { get; set; }

        private BezierCurveService bezierCurveService;
        private SpriteService spriteService;

        public AnimationService(BezierCurveService bcs, SpriteService ss)
        {
            bezierCurveService = bcs;
            spriteService = ss;
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
                    animatable.Location = bezierCurveService.getCubicBezierXYatPercent(animatable.Path, animation.Percent);
                    if (animatable.DrawPath) bezierCurveService.DrawCurve(CanvasCtx, animatable.Path);
                }

                if (animatable.Sprite!=null)
                    spriteService.DrawSprite(animatable.Sprite);
            }
        }
    }
}
