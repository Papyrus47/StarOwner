using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.SkinningAndBrokenBones.Change
{
    public class BrokenBonesChangeSwing_Biting : BrokenBonesChangeSwing
    {
        public BrokenBonesChangeSwing_Biting(NPC npc, PreSwing preSwing, OnSwing onSwing, PostSwing postSwing, Setting setting) : base(npc, preSwing, onSwing, postSwing, setting)
        {
        }
        public GeneralSwingHelper Bite1;
        public GeneralSwingHelper Bite2;
        public int hitNum;
        public bool CanChange;
        public Action<BrokenBonesChangeSwing_Biting> OnBiting;
        public override void AI()
        {
            if ((SwingType)NPC.ai[0] == SwingType.PostSwing)
            {
                int updateNum = setting.ExtraUpdate + 1;
                for (int i = 0; i < updateNum; i++)
                {
                    Bite1.Change(setting.StartVel.RotatedBy(MathHelper.PiOver2), new Vector2(1, 1f));
                    Bite2.Change(setting.StartVel.RotatedBy(-MathHelper.PiOver2), new Vector2(1, 1f));
                    Bite1.spriteDirection = NPC.spriteDirection * setting.SwingDirectionChange.ToDirectionInt();
                    Bite2.spriteDirection = NPC.spriteDirection * -setting.SwingDirectionChange.ToDirectionInt();

                    NPC.ai[1]++;
                    float swingTime = NPC.ai[1] / (onSwing.SwingTime * (setting.ExtraUpdate + 1));
                    if(swingTime > 1)
                    {
                        SkillTimeOut = true;
                    }
                    swingTime = onSwing.SwingTimeChange.Invoke(swingTime);
                    UpdatePlayerFrame();
                    Bite1.SetSwingActive();
                    Bite2.SetSwingActive();
                    Bite1.ProjFixedPos(NPC.Center);
                    Bite2.ProjFixedPos(NPC.Center);

                    Bite1.SwingAI(setting.SwingLenght, NPC.spriteDirection, swingTime * setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt());
                    Bite2.SwingAI(setting.SwingLenght, NPC.spriteDirection, swingTime * setting.SwingRot * -setting.SwingDirectionChange.ToDirectionInt());

                    swingHelper.SetSwingActive();
                    swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt());
                    OnBiting?.Invoke(this);
                }
                return;
            }
            base.AI();
        }
        public override bool SwitchCondition(NPCSkills changeToSkill)
        {
            return CanChange;
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            Bite1 ??= new(NPC, 30, DrawTex)
            {
                Size = Size,
                frameMax = 1
            };
            Bite1.DrawTrailCount = 3;
            Bite2 ??= new(NPC, 30, DrawTex)
            {
                Size = Size,
                frameMax = 1
            };
            Bite2.DrawTrailCount = 3;
            Bite1.SetNotSaveOldVel(true);
            Bite2.SetNotSaveOldVel(true);
            CanChange = false;
            hitNum = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            if ((SwingType)NPC.ai[0] == SwingType.PostSwing && NPC.ai[1] > 2)
            {
                Texture2D texture = ModAsset.Extra_6.Value;
                Bite1.Swing_TrailingDraw(texture, (_) => Color.LightBlue with { A = 0 } * 0.25f, null);
                Bite2.Swing_TrailingDraw(texture, (_) => Color.LightBlue with { A = 0 } * 0.25f, null);
            }
            return false;
        }
    }
}
