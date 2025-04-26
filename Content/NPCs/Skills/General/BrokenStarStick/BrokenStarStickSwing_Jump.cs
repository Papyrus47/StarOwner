using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.BrokenStarStick
{
    public class BrokenStarStickSwing_Jump : BrokenStarStickSwing
    {
        public BrokenStarStickSwing_Jump(NPC npc, PreSwing preSwing, OnSwing onSwing, PostSwing postSwing, Setting setting) : base(npc, preSwing, onSwing, postSwing, setting)
        {

        }
        public Vector2 fixedPos;
        public override void AI()
        {
            swingHelper ??= new(NPC, 30, DrawTex)
            {
                Size = Size,
                frameMax = 1
            };
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
                    NPC.velocity.X = Math.Sign(Target.Center.X - NPC.Center.X) * 0.1f; // 改变速度
                    preSwing.OnUse?.Invoke(this);
                    NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;

                    if (NPC.collideX)
                    {
                        NPC.velocity.Y = -7;
                    }
                    if (NPC.ai[1] > preSwing.PreSwingTime)
                    {
                        NPC.ai[1] = 0;
                        NPC.velocity.Y = 40;
                        NPC.noGravity = true;
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
                                NPC.noGravity = false;
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
                        try
                        {
                            if (!Collision.CanHit(NPC.Center, 1, 1, NPC.Center + swingHelper.velocity, 1, 1) && NPC.ai[1] > 2)
                            {
                                NPC.velocity = (fixedPos - swingHelper.velocity - NPC.Center) * 0.2f;
                                NPC.Center = fixedPos - swingHelper.velocity;
                                swingHelper.ProjFixedPos(NPC.Center);
                            }
                            else
                            {
                                fixedPos = NPC.Center + swingHelper.velocity;
                            }
                        }
                        catch { }
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
                                    Dust dust = Dust.NewDustDirect(swingHelper.velocity * (j / 40f) + NPC.Center, 1, 1, DustID.FireworkFountain_Red);
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
                                HitPiecredExtra98 hitPiecredExtra98 = new(swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * setting.SwingDirectionChange.ToDirectionInt() * NPC.spriteDirection) * 0.2f, Target.Center);
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
                    NPC.noGravity = false;
                    NPC.ai[1]++;
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
            UpdatePlayerFrame();
            StarOwner.drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.Atan2(swingHelper.velocity.Y * StarOwner.drawPlayer.direction, swingHelper.velocity.X * StarOwner.drawPlayer.direction) - MathHelper.PiOver2 * StarOwner.drawPlayer.direction);
        }
    }
}
