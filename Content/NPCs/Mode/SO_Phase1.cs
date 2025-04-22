using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Mode
{
    public class SO_Phase1 : NPCModes
    {
        public SO_Phase1(NPC npc) : base(npc)
        {
        }
        public StarOwnerNPC StarOwner => NPC.ModNPC as StarOwnerNPC;
        public override void OnEnterMode()
        {
            base.OnEnterMode();
            StarOwner.drawPlayer.head = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerHead", EquipType.Head);
            StarOwner.drawPlayer.body = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerBody", EquipType.Body);
            StarOwner.drawPlayer.legs = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerLegs", EquipType.Legs);
        }
        public override bool ActivationCondition(NPCModes activeMode) => false;
        public override bool SwitchCondition(NPCModes changeToMode) => true;
    }
}
