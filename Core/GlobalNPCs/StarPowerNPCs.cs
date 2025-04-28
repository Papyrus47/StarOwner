using StarOwner.Content.NPCs;
using StarOwner.Core.SPBuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.GlobalNPCs
{
    public class StarPowerNPCs : GlobalNPC
    {
        public StarPower starPower = new();
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            if (entity.ModNPC is StarOwnerNPC)
                return false;
            return base.AppliesToEntity(entity, lateInstantiation);
        }
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc)
        {
            starPower ??= new();
            starPower.ValueMax = npc.lifeMax / 10;
            starPower.Value = Math.Clamp(starPower.Value, 0, starPower.ValueMax);
            if(starPower.starPowerResetTime > 0)
                starPower.starPowerResetTime--;
            if (npc.life <= npc.lifeMax - 350 && starPower.starPowerResetTime <= 0)
            {
                starPower.Value--;
                npc.life += 350;
                npc.HealEffect(350);
                if (npc.life > npc.lifeMax)
                    npc.life = npc.lifeMax;
                if (starPower.Value > starPower.ValueMax * 0.5f)
                    starPower.starPowerResetTime = 180;
            }
            if(starPower.Value == starPower.ValueMax)
                npc.StrikeInstantKill();
        }
    }
}
