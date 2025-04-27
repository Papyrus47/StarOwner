using StarOwner.Content.NPCs.Skills.Phase4.Proj;
using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase4
{
    public class SummonIceSoul(NPC npc) : StarOwnerSkills(npc)
    {
        public int CD;
        public override void AI()
        {
            if (NPC.ai[0] != 1)
            {
                NPC.ai[0] = 1;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<IceSoul>(), 0, 0, Target.whoAmI, NPC.whoAmI, 0f);
            }
            base.AI();
            NPC.ai[1]++;
            if (NPC.ai[1] > 60)
            {
                SkillTimeOut = true;
            }
            StarOwner.drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.PiOver2 * NPC.spriteDirection);
        }
        public override bool ActivationCondition(NPCSkills activeSkill)
        {
            if(CD > 0)
            {
                CD--;
                return false;
            }
            return Main.rand.NextBool(10);
        }

        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            NPC.ai[0] = NPC.ai[1] = 0;
            CD = 3600;
            StarOwner.drawPlayer.SetCompositeArmFront(false, Player.CompositeArmStretchAmount.Full, MathHelper.PiOver2 * NPC.spriteDirection);
        }
    }
}
