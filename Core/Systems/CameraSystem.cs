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
        public static float TargetScale = 1f;
        public override void ModifyScreenPosition()
        {
            if(BossAndDownedSystem.StarOwnerIndex != -1)
            {
                if(ScreenCenter == default)
                    ScreenCenter = Vector2.Lerp(ScreenCenter, Main.LocalPlayer.Center, 0.99f);
                Main.screenPosition = Vector2.Lerp(ScreenCenter - Main.ScreenSize.ToVector2() * 0.5f, Main.screenPosition,0.01f);
                ScreenCenter = Vector2.Lerp(ScreenCenter,Main.LocalPlayer.Center,0.02f);
                Main.instance.CameraModifiers.ApplyTo(ref ScreenCenter);
            }
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            float targetScale = 1f;
            if (BossAndDownedSystem.StarOwnerIndex != -1)
                targetScale = 1.4f;
            Transform.Zoom *= ScreenScale;
            ScreenScale += (TargetScale - ScreenScale) * 0.1f;
            TargetScale += (targetScale - TargetScale) * 0.1f;
        }
    }
}
