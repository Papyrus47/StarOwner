using StarOwner.Core.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.PlayerDrawLayers
{
    public class StarPowerDrawLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.GetModPlayer<StarPowerPlayer>().starPower.Value > 0;
        }
        public override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            StarPowerPlayer starPowerPlayer = player.GetModPlayer<StarPowerPlayer>();
            Texture2D tex = ModAsset.StarPowerUI.Value;
            Rectangle rect= new Rectangle(0,0,tex.Width,tex.Height / 2);
            Vector2 origin = rect.Size() * 0.5f;
            drawInfo.DrawDataCache.Add(new(tex,player.Bottom + new Vector2(0,10) - Main.screenPosition,rect,Color.White,0f,origin,1f,SpriteEffects.None,0f));

            rect.X = 16;
            rect.Y = 38;
            rect.Width = (int)(44 * ((float)starPowerPlayer.starPower.Value / starPowerPlayer.starPower.ValueMax));
            rect.Height = 8;
            origin = new Vector2(rect.X, -2);
            drawInfo.DrawDataCache.Add(new(tex, player.Bottom + new Vector2(0, 10) - Main.screenPosition, rect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f));
        }

        public override Position GetDefaultPosition() => new AfterParent(Terraria.DataStructures.PlayerDrawLayers.Head);
    }
}
