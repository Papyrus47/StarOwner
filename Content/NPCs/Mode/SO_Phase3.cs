using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Mode
{
    public class SO_Phase3 : NPCModes
    {
        public StarOwnerNPC starOwner => NPC.ModNPC as StarOwnerNPC;
        public SO_Phase3(NPC npc) : base(npc)
        {
        }
        public override void OnEnterMode()
        {
            NPC.lifeMax = 10;
            NPC.life = 10;
            NPC.defense = 400;
            starOwner.ChangeMode = false;
            starOwner.Music = MusicLoader.GetMusicSlot("StarOwner/Assets/Music/Boss3");
            starOwner.SaveSkills.Clear();
            starOwner.SaveSkillsID.Clear(); // 清空技能

            starOwner.RegisterPhase3();
            starOwner.phase3Start.OnSkillActive(null);
            starOwner.CurrentSkill = starOwner.phase3Start;
        }
        public override bool ActivationCondition(NPCModes activeMode) => starOwner.ChangeMode && activeMode is SO_Phase2;
        public override bool SwitchCondition(NPCModes changeToMode) => true;
    }
}
