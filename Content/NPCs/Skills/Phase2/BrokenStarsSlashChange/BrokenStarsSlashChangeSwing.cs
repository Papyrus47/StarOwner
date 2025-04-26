using StarOwner.Content.NPCs.Skills.General.BrokenStarsSlash;
using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase2.BrokenStarsSlashChange
{
    public class BrokenStarsSlashChangeSwing : BrokenStarsSlashSwing
    {
        public BrokenStarsSlashChangeSwing(NPC npc, PreSwing preSwing, OnSwing onSwing, PostSwing postSwing, Setting setting) : base(npc, preSwing, onSwing, postSwing, setting)
        {
        }
        public int numHit;
        public int shouldHit = 5;
        public override Asset<Texture2D> DrawTex => ModContent.Request<Texture2D>(this.GetInstancePart() + "BrokenStarsSlashChange");
        public override void AI()
        {
            base.AI();
            if(numHit < shouldHit && Target.immune)
            {
                numHit++;
                Target.immune = false;
                Target.velocity *= 0.02f;
            }
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            numHit = 0;
        }
    }
}
