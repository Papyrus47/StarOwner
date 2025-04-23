using StarOwner.Content.NPCs;
using StarOwner.Content.NPCs.Skills.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.Systems
{
    public class NPCDamageSystem : ModSystem
    {
        public override void Load()
        {
            On_NPC.StrikeNPC_HitInfo_bool_bool += OnStrikeNPC;
        }

        public static int OnStrikeNPC(On_NPC.orig_StrikeNPC_HitInfo_bool_bool orig, NPC self, NPC.HitInfo hit, bool fromNet, bool noPlayerInteraction)
        {
            if(self.ModNPC is StarOwnerNPC starOwnerNPC && starOwnerNPC.CurrentSkill is IDefenceAttack defenceAttack)
            {
                defenceAttack.DefenceSucceed = true;
                defenceAttack.OnDefenceSucceed();
                return 0;
            }
            return orig.Invoke(self, hit, fromNet, noPlayerInteraction);
        }
    }
}
