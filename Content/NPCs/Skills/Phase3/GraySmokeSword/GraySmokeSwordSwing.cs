using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase3.GraySmokeSword
{
    public class GraySmokeSwordSwing : BasicSwingSkill
    {
        public GraySmokeSwordSwing(NPC npc, PreSwing preSwing, OnSwing onSwing, PostSwing postSwing, Setting setting) : base(npc, preSwing, onSwing, postSwing, setting)
        {
            AddStarPower = false;
        }

        public override Asset<Texture2D> DrawTex => ModContent.Request<Texture2D>(this.GetInstancePart() + "GraySmokeSword");

        public override int WeaponDamage => 100;

        public override Vector2 Size => new(94);
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
                    if (NPC.velocity.X < 3f && NPC.velocity.X > -3f)
                        NPC.velocity.X += Math.Sign(Target.Center.X - NPC.Center.X) * 0.2f;
                    else
                        NPC.velocity.X *= 0.9f;
                    NPC.spriteDirection = NPC.direction = Target.Center.X - NPC.Center.X > 0 ? 1 : -1;
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
                                Dodgeable = true,
                                HitDirection = NPC.spriteDirection,
                                HitDirectionOverride = NPC.spriteDirection
                            };
                            hurtModifiers.SourceDamage += setting.ActionDmg - 1f;
                            hurtModifiers.SourceDamage += StarOwner.DamageAdd;
                            onSwing.modifyHit?.Invoke(Target, ref hurtModifiers);
                            Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(WeaponDamage, Target.statDefense, Target.DefenseEffectiveness.Value, 0, true);
                            hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                            if (PlayerLoader.FreeDodge(Target, hurtInfo))
                                break;
                            if (PlayerLoader.ConsumableDodge(Target, hurtInfo))
                                break;
                            Target.Hurt(hurtInfo);
                            for (int j = Main.rand.Next(5, 8) + 15; j > 0; j--)
                            {
                                HitPiecredExtra98 hitPiecredExtra98 = new(swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * setting.SwingDirectionChange.ToDirectionInt() * NPC.spriteDirection) * 0.2f, Target.Center);
                                Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                            }
                            SoundEngine.PlaySound(SoundID.PlayerKilled.WithPitchOffset(-0.5f) with { Volume = 2}, Target.Center);
                            //Target.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 10;
                            StarOwner.DamageAdd += 0.5f;
                            Target.immuneTime /= 3;
                            Target.velocity += swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * -NPC.spriteDirection).SafeNormalize(default) * MathF.Log2(hurtInfo.Damage * 0.5f);
                            onSwing.onHit?.Invoke(Target, hurtInfo);
                            if (AddStarPower)
                                Target.GetModPlayer<StarPowerPlayer>().starPower.Value += WeaponDamage;
                            OnHitStopTime = setting.OnHitStopTime * (setting.ExtraUpdate + 1);
                            Main.instance.CameraModifiers.Add(new PunchCameraModifier(Target.position, -Vector2.UnitX * Target.direction, 30, 60, 20));
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

                    swingHelper.SetSwingActive();
                    swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, setting.SwingRot * setting.SwingDirectionChange.ToDirectionInt());
                    break;
            }
            NPC.velocity.Y = 0;
            UpdatePlayerFrame();
            NPC.velocity.Y = (Target.Center.Y - NPC.Center.Y) * 0.2f;
            StarOwner.drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.Atan2(swingHelper.velocity.Y * StarOwner.drawPlayer.direction, swingHelper.velocity.X * StarOwner.drawPlayer.direction) - MathHelper.PiOver2 * StarOwner.drawPlayer.direction);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            if ((SwingType)NPC.ai[0] == SwingType.PreSwing && NPC.ai[1] < 2)
                return false;
            Effect effect = ModAsset.GraySmokeSwingShader.Value;
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            gd.Textures[1] = ModAsset.ColorMap_4.Value;
            gd.Textures[2] = ModAsset.Perlin.Value;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix scale = Matrix.CreateScale(Main.GameViewMatrix.Zoom.Length() / MathF.Sqrt(2));
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * projection * scale);
            swingHelper.Swing_Draw_ItemAndTrailling(Color.White, ModAsset.Extra_0.Value, (_) => Color.White with { A = 0}, effect);
            return false;
        }
    }
}
