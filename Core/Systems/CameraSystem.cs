using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics;

namespace StarOwner.Core.Systems
{
    public class CameraSystem : ModSystem
    {
        public static Vector2 ScreenCenter;
        /// <summary>
        /// 缩放比例
        /// </summary>
        public static float ScreenScale = 1f;
        public override void ModifyScreenPosition()
        {
            if(ScreenCenter != default)
            {
                Main.screenPosition = Vector2.Lerp(ScreenCenter - Main.ScreenSize.ToVector2() * 0.5f, Main.screenPosition,0.1f);
                ScreenCenter = Vector2.Lerp(ScreenCenter,default,0.9f);
                if (ScreenCenter.LengthSquared() < 1000000)
                    ScreenCenter = default;
            }
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            Transform.Zoom *= ScreenScale;
            ScreenScale += (1 - ScreenScale) * 0.1f;
        }
    }
}
