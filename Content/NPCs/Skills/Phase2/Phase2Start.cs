using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase2
{
    public class Phase2Start : StarOwnerSkills
    {
        public Phase2Start(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.velocity.X *= 0.4f;
            NPC.ai[0]++;
            NPC.dontTakeDamage = true;
            StarOwner.drawPlayer.SetCompositeArmFront(false, Player.CompositeArmStretchAmount.Full, 0);
            switch ((int)NPC.ai[0])
            {
                case 150:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase2Text + ".Text1"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 250:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase2Text + ".Text2"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
            }
            base.AI();
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            NPC.dontTakeDamage = false;
            base.OnSkillDeactivate(changeToSkill);
        }
        public override bool SwitchCondition(NPCSkills changeToSkill)
        {
            return NPC.ai[0] > 350;
        }
    }
}
