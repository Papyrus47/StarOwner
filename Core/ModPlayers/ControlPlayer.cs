using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;

namespace StarOwner.Core.ModPlayers
{
    public class ControlPlayer : ModPlayer
    {
        public int StopControl;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (StopControl > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 vel = Vector2.UnitX.RotatedBy(i / 3f * MathHelper.TwoPi + StopControl * 0.1f) * 30;
                    vel.Y *= 0.2f;
                    Dust dust = Dust.NewDustPerfect(Player.Top + vel, DustID.GoldFlame, default, 0, Color.Gold, 1.2f);
                    dust.noGravity = true;
                }
                StopControl--;
                Player.controlUseItem = Player.controlUseTile = false;
                Player.controlUp = Player.controlDown = Player.controlLeft = Player.controlRight = false;
                Player.controlHook = false;
                Player.controlMount = false;
                Player.controlJump = false;
                Player.controlQuickHeal = false;
                Player.controlQuickMana = false;
                Player.RemoveAllGrapplingHooks();
                Main.mouseLeft = false;
                Main.mouseRight = false;
            }

        }
    }
}
