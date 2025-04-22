using StarOwner.Core.SPBuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.ModPlayers
{
    public class StarPowerPlayer : ModPlayer
    {
        public StarPower starPower = new();
        public override void ResetEffects()
        {
            if(starPower.starPowerResetTime > 0)
                starPower.starPowerResetTime--;
        }
        public override void UpdateDead()
        {
            starPower.Value = 0;
        }
        public override void PostUpdate()
        {
            starPower.ValueMax = Player.statLifeMax2;
            if (starPower.Value == starPower.ValueMax)
            {
                Player.KillMe(PlayerDeathReason.LegacyDefault(), 10.0, Player.direction);
            }
            else if (starPower.Value > 0)
            {
                if (starPower.starPowerResetTime <= 0)
                {
                    if (Player.statLife < Player.statLifeMax2)
                    {
                        if (starPower.Value >= starPower.ValueMax)
                            starPower.starPowerResetTime = 180;
                        starPower.Value--;
                        Player.Heal(350);
                    }
                    if (Player.statMana < Player.statManaMax2)
                    {
                        if (starPower.Value >= starPower.ValueMax)
                            starPower.starPowerResetTime = 180;
                        starPower.Value--;
                        Player.ManaEffect(350);
                        Player.statMana += 350;
                        if (Player.statMana > Player.statManaMax2)
                            Player.statMana = Player.statManaMax2;

                    }
                }
            }
        }
    }
}
