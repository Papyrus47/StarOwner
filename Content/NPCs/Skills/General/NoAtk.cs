using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General
{
    public class NoAtk(NPC npc,int hitNum) : StarOwnerSkills(npc)
    {
        public readonly int HitNum = hitNum;
        public override void AI()
        {
            NPC.dontTakeDamage = false;
            NPC.noTileCollide = false;
            NPC.velocity.X *= 0.9f;
            StarOwner.drawPlayer.SetCompositeArmFront(false, Player.CompositeArmStretchAmount.Full, 0f);
            NPC.ai[0]++;
            float disY = NPC.Center.Y - Target.Center.Y;
            NPC.defense = 0;
            //Main.NewText(disY);
            NPC.rotation = MathF.Sin(NPC.ai[0] * 0.2f) * 0.3f;
            if (NPC.ai[0] > 300)
            {
                SkillTimeOut = true;
                return;
            }
            for(int i = 0; i < 3; i++)
            {
                Vector2 vel = Vector2.UnitX.RotatedBy(i / 3f * MathHelper.TwoPi + NPC.ai[0] * 0.1f) * 30;
                vel.Y *= 0.2f;
                Dust dust = Dust.NewDustPerfect(NPC.Top + vel, DustID.GoldFlame, default, 0, Color.Gold, 1.2f);
                dust.noGravity = true;
            }
            base.AI();
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            StarOwner.byDefAtk = 0;
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            NPC.rotation = 0;
        }
        public override bool ActivationCondition(NPCSkills activeSkill) => false;
        public override bool CompulsionSwitchSkill(NPCSkills activeSkill) => StarOwner.byDefAtk >= HitNum;
    }
}
