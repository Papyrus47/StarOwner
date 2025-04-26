using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;

namespace StarOwner.Core.ModPlayers
{
    public class DefAttackAndDodgePlayer : ModPlayer
    {
        public int dodgeTime;
        public int dodgeTimeCD;
        public int defenseAttack = 0;
        public int defenseCD = 0;
        public bool isDrawPlayer;
        public override void ResetEffects()
        {
            if (defenseAttack > 0)
                defenseAttack -= 3;
            if (defenseCD > 0)
            {
                for (int i = 0; i < defenseCD; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.MountedCenter + Vector2.UnitX.RotatedBy(i / 70f * MathHelper.TwoPi) * 50,1,1,DustID.Flare);
                    dust.velocity *= 0;
                    dust.noGravity = true;
                }
                defenseCD--;
            }
            if(dodgeTime > 0)
            {
                if(--dodgeTime <= 0)
                {
                    Player.velocity.X *= 0.1f;
                }
                Player.velocity.X += Player.direction;
                Player.SetImmuneTimeForAllTypes(5);
            }
            if(dodgeTimeCD > 0)
            {
                dodgeTimeCD--;
                for (int i = 0; i < dodgeTimeCD; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.MountedCenter + Vector2.UnitX.RotatedBy(i / 70f * MathHelper.TwoPi) * 50, 1, 1, DustID.Flare_Blue);
                    dust.velocity *= 0;
                    dust.noGravity = true;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (StarOwner.DefAtk.Current && defenseCD <= 0)
            {
                defenseCD = 70;
                defenseAttack = 60;
            }
            if(StarOwner.DodgeDamage.Current && dodgeTimeCD <= 0)
            {
                dodgeTime = 30;
                dodgeTimeCD = 70;
            }
        }
    }
}
