using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;
using static StarOwner.Content.NPCs.Skills.BasicSwingSkill.OnSwing;
using static StarOwner.Core.SkillProjs.GeneralSkills.SwingHelper_GeneralSwing;

namespace StarOwner.Content.NPCs.Skills
{
    public abstract class BasicSwingSkill(NPC npc, BasicSwingSkill.PreSwing preSwing, BasicSwingSkill.OnSwing onSwing, BasicSwingSkill.PostSwing postSwing, BasicSwingSkill.Setting setting) : StarOwnerSkills(npc)
    {
        public class PreSwing
        {
            public float PreSwingTime;
            public Action<BasicSwingSkill> OnChange;
            public Action<BasicSwingSkill> OnUse;
        }
        public class OnSwing
        {
            public float SwingTime;
            /// <summary>
            /// 变化函数
            /// </summary>
            public Func<float, float> SwingTimeChange;
            public Action<BasicSwingSkill> OnChange;
            public Action<BasicSwingSkill> OnUse;
            public delegate void OnHit(Player target, Player.HurtInfo hurtInfo);
            public OnHit onHit;
            public delegate void ModifyHit(Player target, ref Player.HurtModifiers hurtInfo);
            public ModifyHit modifyHit;
        }
        public class PostSwing
        {
            public float PostSwingTime;
            /// <summary>
            /// 技能超时时间
            /// </summary>
            public float PostSwingTimeMax;
            public Action<BasicSwingSkill> OnChange;
            public Action<BasicSwingSkill> OnUse;
        }
        public class Setting : ICloneable
        {
            /// <summary>
            /// 玩家朝向为1的时候,从哪个速度开始的向量(-1会自动调整)
            /// </summary>
            public Vector2 StartVel = Vector2.UnitX;
            /// <summary>
            /// 速度压缩变化
            /// </summary>
            public Vector2 VelScale = Vector2.One;
            /// <summary>
            /// 旋转的弧度
            /// </summary>
            public float SwingRot = MathHelper.Pi;
            /// <summary>
            /// 通过旋转实现透视
            /// </summary>
            public float VisualRotation;
            /// <summary>
            /// 挥舞的朝向
            /// </summary>
            public bool SwingDirectionChange = true;
            /// <summary>
            /// 如何使用这个技能
            /// </summary>
            public Func<bool> ChangeCondition = () => true;
            /// <summary>
            /// 动作值
            /// </summary>
            public float ActionDmg = 1;
            /// <summary>
            /// 挥舞长度
            /// </summary>
            public float SwingLenght = 10;
            /// <summary>
            /// 命中卡肉
            /// </summary>
            public int OnHitStopTime;
            /// <summary>
            /// 移动卡肉
            /// </summary>
            public bool IsMoveWhenOnHitStop;
            /// <summary>
            /// 在命中卡肉时,额外移动
            /// </summary>
            public float AddOnMoveWhenOnHitStop;
            /// <summary>
            /// 额外更新次数
            /// </summary>
            public int ExtraUpdate = 6;

            public object Clone()
            {
                Setting setting = new()
                {
                    StartVel = StartVel,
                    VelScale = VelScale,
                    SwingRot = SwingRot,
                    VisualRotation = VisualRotation,
                    SwingDirectionChange = SwingDirectionChange,
                    ChangeCondition = ChangeCondition,
                    ActionDmg = ActionDmg,
                    SwingLenght = SwingLenght,
                    OnHitStopTime = OnHitStopTime,
                    IsMoveWhenOnHitStop = IsMoveWhenOnHitStop,
                    AddOnMoveWhenOnHitStop = AddOnMoveWhenOnHitStop,
                    ExtraUpdate = ExtraUpdate
                };
                return setting;
            }
            public Setting DefClone() => (Setting)Clone();
        }
        public PreSwing preSwing = preSwing;
        public OnSwing onSwing = onSwing;
        public PostSwing postSwing = postSwing;
        public Setting setting = setting;
        public int OnHitStopTime;
        public abstract Asset<Texture2D> DrawTex { get; }
        /// <summary>
        /// 武器面板伤害
        /// </summary>
        public abstract int WeaponDamage { get; }
        public abstract Vector2 Size { get; }
        public GeneralSwingHelper swingHelper;
        public SoundStyle playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f);
        public bool ByDef;
        /// <summary>
        /// 可以弹刀
        /// </summary>
        public bool CanDef = true;
        public bool AddStarPower = true;
        public enum SwingType : int
        {
            PreSwing,
            OnSwing,
            PostSwing
        }
        public override void AI()
        {
            NewSwingHelper();
            swingHelper.spriteDirection = NPC.spriteDirection * setting.SwingDirectionChange.ToDirectionInt();
            switch ((SwingType)NPC.ai[0])
            {
                case SwingType.PreSwing:
                    NPC.ai[1]++;
                    float time = NPC.ai[1] / preSwing.PreSwingTime;
                    swingHelper.Change_Lerp(setting.StartVel, time, setting.VelScale, time, setting.VisualRotation, time);
                    swingHelper.ProjFixedPos(NPC.Center);
                    swingHelper.SetSwingActive();
                    swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, 0f);
                    preSwing.OnUse?.Invoke(this);
                    NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;

                    if (NPC.collideX)
                    {
                        NPC.velocity.Y = -7;
                    }
                    if (NPC.ai[1] > preSwing.PreSwingTime)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0] = (int)SwingType.OnSwing;
                        SoundEngine.PlaySound(playSound, NPC.Center);
                        preSwing.OnChange?.Invoke(this);
                    }
                    break;
                case SwingType.OnSwing:
                    int updateNum = setting.ExtraUpdate + 1;
                    if (ByDef)
                        updateNum = (setting.ExtraUpdate + 2) / 2;
                    for (int i = 0; i < updateNum; i++)
                    {
                        swingHelper.ProjFixedPos(NPC.Center);
                        swingHelper.SetSwingActive();
                        if (ByDef)
                        {
                            NPC.velocity.X = -NPC.spriteDirection * 2;
                            NPC.ai[1]--;
                            if (NPC.ai[1] < 0)
                            {
                                SkillTimeOut = true;
                                break;
                            }
                        }
                        else
                        {
                            if (OnHitStopTime <= 0)
                                NPC.ai[1]++;
                            else
                            {
                                OnHitStopTime--;
                                if (setting.IsMoveWhenOnHitStop)
                                    NPC.ai[1] += setting.AddOnMoveWhenOnHitStop;
                                swingHelper.SetNotSaveOldVel(false);
                            }
                        }
                        float swingTime = NPC.ai[1] / (onSwing.SwingTime * (setting.ExtraUpdate + 1));
                        if (swingTime > 1)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[0] = (int)SwingType.PostSwing;
                            onSwing.OnChange?.Invoke(this);
                            break;
                        }
                        onSwing.OnUse?.Invoke(this);
                        swingTime = onSwing.SwingTimeChange.Invoke(swingTime);

                        swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, swingTime * setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt());
                        #region 碰撞检测
                        if (swingHelper.GetColliding(Target.getRect()) && !Target.immune && !ByDef)
                        {
                            DefAttackAndDodgePlayer defAttackPlayer = Target.GetModPlayer<DefAttackAndDodgePlayer>();
                            if (defAttackPlayer.defenseAttack > 0 && CanDef)
                            {
                                ByDef = true;
                                StarOwner.byDefAtk++;
                                defAttackPlayer.defenseCD = 0;
                                SoundEngine.PlaySound(SoundID.Item178.WithVolume(3).WithPitchOffset(0.2f), Target.Center);
                                for (int j = 0; j < 40; j++)
                                {
                                    Dust dust = Dust.NewDustDirect(swingHelper.velocity * (j / 40f) + swingHelper.Center, 1, 1, DustID.FireworkFountain_Red);
                                    dust.velocity = swingHelper.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2 * NPC.spriteDirection * setting.SwingDirectionChange.ToDirectionInt()) * (5 + j * 0.2f);
                                    dust.noGravity = true;
                                }
                                break;
                            }
                            Player.HurtModifiers hurtModifiers = new()
                            {
                                Dodgeable = false,
                            };
                            hurtModifiers.SourceDamage += setting.ActionDmg - 1f;
                            hurtModifiers.SourceDamage += StarOwner.DamageAdd;
                            onSwing.modifyHit?.Invoke(Target, ref hurtModifiers);
                            Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(WeaponDamage, Target.statDefense, Target.DefenseEffectiveness.Value, 0, true);
                            hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                            Target.Hurt(hurtInfo);
                            for (int j = Main.rand.Next(5, 8); j > 0; j--)
                            {
                                HitPiecredExtra98 hitPiecredExtra98 = new(swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * setting.SwingDirectionChange.ToDirectionInt() * NPC.spriteDirection) * 0.2f,Target.Center);
                                Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                            }
                            Target.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 10;
                            StarOwner.DamageAdd += 0.5f;
                            Target.immuneTime /= 3;
                            Target.velocity += swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * -NPC.spriteDirection).SafeNormalize(default) * MathF.Log2(hurtInfo.Damage * 0.5f);
                            onSwing.onHit?.Invoke(Target, hurtInfo);
                            if (AddStarPower)
                                Target.GetModPlayer<StarPowerPlayer>().starPower.Value += WeaponDamage;
                            OnHitStopTime = setting.OnHitStopTime * (setting.ExtraUpdate + 1);
                            Main.instance.CameraModifiers.Add(new PunchCameraModifier(Target.position, -Vector2.UnitX * Target.direction, MathF.Log2(hurtInfo.Damage), 60, 20));
                        }
                        #endregion
                    }
                    break;
                case SwingType.PostSwing:
                    NPC.ai[1]++;
                    if (NPC.velocity.X < 0.2f && NPC.velocity.X > -0.2f)
                        NPC.velocity.X += Math.Sign(Target.Center.X - NPC.Center.X) * 0.2f;
                    else
                        NPC.velocity.X *= 0.5f;
                    if (NPC.collideX)
                    {
                        NPC.velocity.Y = -7;
                    }
                    if (NPC.ai[1] > postSwing.PostSwingTimeMax)
                    {
                        postSwing.OnChange?.Invoke(this);
                        SkillTimeOut = true;
                        break;
                    }
                    swingHelper.ProjFixedPos(NPC.Center);
                    swingHelper.SetSwingActive();
                    //swingHelper.SetNotSaveOldVel(true);
                    postSwing.OnUse?.Invoke(this);
                    swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt());
                    break;
            }
            base.AI();
            StarOwner.drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.Atan2(swingHelper.velocity.Y * StarOwner.drawPlayer.direction, swingHelper.velocity.X * StarOwner.drawPlayer.direction) - MathHelper.PiOver2 * StarOwner.drawPlayer.direction);
        }
        public virtual void NewSwingHelper()
        {
            swingHelper ??= new(NPC, 30, DrawTex)
            {
                Size = Size,
                frameMax = 1
            };
        }

        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] == (int)SwingType.PostSwing && NPC.ai[1] > postSwing.PostSwingTime;
        public override bool ActivationCondition(NPCSkills activeSkill) => setting.ChangeCondition.Invoke();
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            ByDef = false;
            NewSwingHelper();
        }
    }
}
