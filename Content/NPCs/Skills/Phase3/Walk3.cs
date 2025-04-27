using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase3
{
    public class Walk3 : StarOwnerSkills
    {
        public Walk3(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.dontTakeDamage = false;
            NPC.noTileCollide = true;
            //if (!NPC.collideY)
            //    NPC.velocity.Y -= 0.2f;
            StarOwner.drawPlayer.SetCompositeArmFront(false, Player.CompositeArmStretchAmount.Full, 0f);
            NPC.ai[0]++;
            NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
            NPC.velocity.X += (Target.Center.X - NPC.Center.X) * 0.01f;
            NPC.velocity.X *= 0.8f;
            if(StarOwner.IsPhase(4))
                NPC.velocity.X *= 0.5f;
            //NPC.position.Y = Target.position.Y;
            float disY = NPC.Center.Y - Target.Center.Y;
            //Main.NewText(disY);
            NPC.rotation = 0;
            NPC.velocity.Y = 0;
            base.AI();
            NPC.velocity.Y = (Target.Center.Y - NPC.Center.Y) * 0.02f;
            if (NPC.ai[0] > 25)
                NPC.ai[0] = 10;
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > 20;
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            NPC.dontTakeDamage = false;
        }
    }
}
