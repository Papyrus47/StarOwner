using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Mode
{
    public class SO_Phase2 : NPCModes
    {
        public StarOwnerNPC starOwner => NPC.ModNPC as StarOwnerNPC;
        public SO_Phase2(NPC npc) : base(npc)
        {
        }
        public override void OnEnterMode()
        {
            starOwner.ChangeMode = false;
            starOwner.RandomSkillMax += 3;
        }
        public override bool ActivationCondition(NPCModes activeMode) => starOwner.ChangeMode && activeMode is SO_Phase1;
        public override bool SwitchCondition(NPCModes changeToMode) => true;
    }
}
