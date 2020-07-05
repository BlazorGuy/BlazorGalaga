using System;
using BlazorGalaga.Models;
using Microsoft.JSInterop;

namespace BlazorGalaga.Static
{
    public static class KeyBoardHelper
    {

        public static  string KeyDown { get; set; }

        private static bool ignorenextkeyup;

        [JSInvokable("OnKeyDown")]
        public static void OnKeyDown(string keycode)
        {
            Console.WriteLine("KeyDown: " + keycode);

            if (keycode == Constants.ArrowLeft || keycode == Constants.ArrowRight)
            {
                if ((KeyDown == Constants.ArrowLeft && keycode == Constants.ArrowRight) ||
                (KeyDown == Constants.ArrowRight && keycode == Constants.ArrowLeft)) ignorenextkeyup = true;

                KeyDown = keycode;
            }
        }

        [JSInvokable("OnKeyUp")]
        public static void OnKeyUp(string keycode)
        {
            Console.WriteLine("KeyUp: " + keycode);

            if (keycode == Constants.ArrowLeft || keycode == Constants.ArrowRight)
            {
                if (ignorenextkeyup)
                    ignorenextkeyup = false;
                else
                    KeyDown = "";
            }
        }

        public static void ControlShip(Animatable shipanimatable)
        {

            if (KeyDown == Constants.ArrowLeft)
                shipanimatable.Speed = Constants.ShipMoveSpeed * -1;

            if (KeyDown == Constants.ArrowRight)
                shipanimatable.Speed = Constants.ShipMoveSpeed;

            if (KeyDown == string.Empty)
                shipanimatable.Speed = 0;
        }
    }
}
