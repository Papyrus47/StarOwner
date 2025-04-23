using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using StarOwner.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase1
{
    public class Phase1Start : StarOwnerSkills
    {
        public Phase1Start(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            //if (NPC.localAI[0] < 100)
            //    NPC.localAI[0]++;
            NPC.boss = false;
            CameraSystem.ScreenCenter = NPC.Center;
            CameraSystem.TargetScale = 2f;
            NPC.ai[0]++;
            if (NPC.ai[0] > 550)
            {
                NPC.spriteDirection = Math.Sign(Target.position.X - NPC.position.X);
            }
            else
            {
                Target.GetModPlayer<ControlPlayer>().StopControl = 1;
            }
            NPC.dontTakeDamage = true;
            switch ((int)NPC.ai[0])
            {
                case 1:
                    NPC.spriteDirection = -Math.Sign(Target.position.X - NPC.position.X);
                    break;
                case 150:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text1"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 250:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text2"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 350:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text3"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 450:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text4"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 550:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text5"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 650:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text6"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 750:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text7"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 850:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text8"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 950:
                    NPC.dontTakeDamage = false;
                    if(NPC.life == NPC.lifeMax)
                    {
                        NPC.ai[0]--;
                    }
                    else
                    {
                        NPC.life = NPC.lifeMax;
                        PopupText.NewText(new AdvancedPopupRequest()
                        {
                            Color = Color.Purple,
                            Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text9"),
                            DurationInFrames = 120,
                            Velocity = -Vector2.UnitY
                        }, NPC.Top);
                    }
                    break;
                case 1050:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text10"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 1150:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text11"),
                        DurationInFrames = 120,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 1250:
                    NPC.dontTakeDamage = false;
                    if (NPC.life == NPC.lifeMax)
                    {
                        NPC.ai[0]--;
                    }
                    else
                    {
                        NPC.life = NPC.lifeMax;
                        PopupText.NewText(new AdvancedPopupRequest()
                        {
                            Color = Color.Purple,
                            Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text12"),
                            DurationInFrames = 180,
                            Velocity = -Vector2.UnitY
                        }, NPC.Top);
                    }
                    break;
                case 1350:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text13"),
                        DurationInFrames = 180,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;
                case 1450:
                    PopupText.NewText(new AdvancedPopupRequest()
                    {
                        Color = Color.Purple,
                        Text = TheUtility.RegisterText(StarOwnerNPC.Phase1Text + ".Text14"),
                        DurationInFrames = 180,
                        Velocity = -Vector2.UnitY
                    }, NPC.Top);
                    break;

            }
            base.AI();
        }
        public override bool SwitchCondition(NPCSkills changeToSkill)
        {
            return true;
            return NPC.ai[0] > 1500;
        }
    }
}
