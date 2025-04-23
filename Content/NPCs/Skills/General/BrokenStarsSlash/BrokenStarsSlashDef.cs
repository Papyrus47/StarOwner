using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using StarOwner.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.BrokenStarsSlash
{
    public class BrokenStarsSlashDef(NPC npc, BasicSwingSkill.PreSwing preSwing, BasicSwingSkill.OnSwing onSwing, BasicSwingSkill.PostSwing postSwing, BasicSwingSkill.Setting setting) : BrokenStarsSlashSwing(npc, preSwing, onSwing, postSwing, setting),IDefenceAttack
    {
        public bool DefenceSucceed { get; set; }
        public void OnDefenceSucceed() 
        {
            NPC.velocity.X = -NPC.spriteDirection * 8;
            CameraSystem.ScreenCenter = NPC.Center;
            CameraSystem.TargetScale = 4;
            SoundEngine.PlaySound(SoundID.Item178.WithVolume(3).WithPitchOffset(-0.2f), NPC.Center);
            for (int i = 0; i < setting.SwingLenght; i++)
            {
                var center = swingHelper.Center + swingHelper.velocity * (i / setting.SwingLenght);
                Dust dust = Dust.NewDustPerfect(center, DustID.PurpleTorch, Vector2.UnitX * NPC.spriteDirection * 5,0,default,3);
                dust.noGravity = true;
            }
        }
        public override void AI()
        {
            swingHelper ??= new(NPC, 30, DrawTex)
            {
                Size = Size,
                frameMax = 1
            };
            NPC.velocity.X *= 0.8f;
            swingHelper.spriteDirection = NPC.spriteDirection * setting.SwingDirectionChange.ToDirectionInt();
            NPC.ai[0]++;
            if (NPC.ai[0] > 120)
            {
                SkillTimeOut = true;
                return;
            }
            swingHelper.Change_Lerp(setting.StartVel, 0.4f, setting.VelScale, 0.4f, setting.VisualRotation, 0.4f);
            swingHelper.ProjFixedPos(NPC.Center, -setting.SwingLenght * 0.5f);
            swingHelper.SetSwingActive();
            swingHelper.SetNotSaveOldVel(true);
            swingHelper.Center += Vector2.UnitX * NPC.spriteDirection * 10;
            swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, 0f);
            preSwing.OnUse?.Invoke(this);
            UpdatePlayerFrame();
            StarOwner.drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.Atan2(-swingHelper.velocity.Y * StarOwner.drawPlayer.direction, -swingHelper.velocity.X * StarOwner.drawPlayer.direction) - MathHelper.PiOver2 * StarOwner.drawPlayer.direction);
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            DefenceSucceed = false;
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => DefenceSucceed;
    }
}
