using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Mode
{
    public class SO_Phase4 : NPCModes
    {
        public StarOwnerNPC starOwner => NPC.ModNPC as StarOwnerNPC;
        public SO_Phase4(NPC npc) : base(npc)
        {
        }
        public override void OnEnterMode()
        {
            NPC.lifeMax = 11000000;
            NPC.life = 11000000;
            NPC.defense = 0;
            starOwner.ChangeMode = false;
            starOwner.Music = MusicLoader.GetMusicSlot("StarOwner/Assets/Music/Boss4");
            starOwner.SaveSkills.Clear();
            starOwner.SaveSkillsID.Clear(); // 清空技能
            starOwner.walk3.SkillsTree.Clear();

            starOwner.RegisterPhase4();
            starOwner.phase4Start.OnSkillActive(null);
            starOwner.CurrentSkill = starOwner.phase4Start;
        }
        public override bool ActivationCondition(NPCModes activeMode) => starOwner.ChangeMode && activeMode is SO_Phase3;
        public override bool SwitchCondition(NPCModes changeToMode) => true;
    }
}
