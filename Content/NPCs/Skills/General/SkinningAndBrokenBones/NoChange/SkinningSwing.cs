using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.SkinningAndBrokenBones.NoChange
{
    public class SkinningSwing(NPC npc, BasicSwingSkill.PreSwing preSwing, BasicSwingSkill.OnSwing onSwing, BasicSwingSkill.PostSwing postSwing, BasicSwingSkill.Setting setting) : BasicSwingSkill(npc, preSwing, onSwing, postSwing, setting)
    {
        public override Asset<Texture2D> DrawTex => ModContent.Request<Texture2D>(this.GetInstancePart() + "BrokenBones");
        public override int WeaponDamage => 12;
        public override Vector2 Size => new(54,42);
        public bool IsThrow;
        public bool IsHit;
        public bool IsBack;
        /// <summary>
        /// 丢出去的方向
        /// </summary>
        public Func<Vector2> ThrowDir;
        public Vector2 ThrowVel;
        public bool CanThrow;
        public bool CanChange;
        public override void AI()
        {
            if (IsThrow)
            {
                if (!IsBack) // 向前抛
                {
                    NPC.ai[1] += 0.4f;
                    NPC.ai[1] %= 6.28f;
                }
                else // 回归
                {
                    NPC.ai[3]--;
                    (swingHelper as GeneralSwingHelper_DrawPosFixed).FixedDrawCenter = NPC.Center + ThrowVel * NPC.ai[3]; // 固定绘制位置
                    swingHelper.ProjFixedPos(NPC.Center + ThrowVel * NPC.ai[3], setting.SwingLenght * 0.5f);
                    if (IsHit)
                    {
                        Target.Center = swingHelper.Center;
                        Target.GetModPlayer<ControlPlayer>().StopControl = 30;
                        Target.velocity.X = 0;
                    }
                    if (NPC.ai[3] <= 4)
                    {
                        if (IsHit)
                            CanChange = true;
                        else
                            SkillTimeOut = true;
                    }
                    swingHelper.SetSwingActive();
                    swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, NPC.ai[1] * setting.SwingDirectionChange.ToDirectionInt());
                    return;
                }
                if(swingHelper.Center.Distance(NPC.Center) > 1000) // 距离太远跳出
                {
                    SkillTimeOut = true;
                    return;
                }
                if(ThrowVel == default)
                    ThrowVel = (ThrowDir?.Invoke() ?? default).SafeNormalize(default) * 16;
                NPC.ai[3]++;
                (swingHelper as GeneralSwingHelper_DrawPosFixed).FixedDrawCenter = NPC.Center + ThrowVel * NPC.ai[3]; // 固定绘制位置
                swingHelper.ProjFixedPos(NPC.Center + ThrowVel * NPC.ai[3], setting.SwingLenght * 0.5f); // 固定抛出位置
                swingHelper.SetSwingActive();
                swingHelper.SwingAI(setting.SwingLenght, NPC.spriteDirection, NPC.ai[1] * setting.SwingDirectionChange.ToDirectionInt());

                #region 碰撞检测
                if (swingHelper.GetColliding(Target.getRect()) && !Target.immune && !ByDef)
                {
                    IsBack = true;
                    DefAttackAndDodgePlayer defAttackPlayer = Target.GetModPlayer<DefAttackAndDodgePlayer>();
                    if (defAttackPlayer.defenseAttack > 0 && CanDef)
                    {
                        ByDef = true;
                        defAttackPlayer.defenseCD = 0;
                        SoundEngine.PlaySound(SoundID.Item178.WithVolume(3).WithPitchOffset(0.2f), Target.Center);
                        for (int j = 0; j < 40; j++)
                        {
                            Dust dust = Dust.NewDustDirect(swingHelper.velocity * (j / 40f) + swingHelper.Center, 1, 1, DustID.FireworkFountain_Red);
                            dust.velocity = swingHelper.velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2 * NPC.spriteDirection * setting.SwingDirectionChange.ToDirectionInt()) * (5 + j * 0.2f);
                            dust.noGravity = true;
                        }
                    }
                    else
                    {
                        IsHit = true;
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
                }
                #endregion
                return;
            }
            base.AI();
            NPC.spriteDirection = Math.Sign(Target.Center.X - NPC.Center.X);
            if ((SwingType)NPC.ai[0] == SwingType.OnSwing && NPC.ai[1] > onSwing.SwingTime / 5 && CanThrow)
            {
                IsThrow = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            if ((SwingType)NPC.ai[0] == SwingType.PreSwing && NPC.ai[1] < 2)
                return false;
            Effect effect = ModAsset.StarOwnerSwingShader.Value;
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            gd.Textures[1] = ModAsset.ColorMap_1.Value;
            gd.Textures[2] = ModAsset.Stars.Value;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix scale = Matrix.CreateScale(Main.GameViewMatrix.Zoom.Length() / MathF.Sqrt(2));
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * projection * scale);
            swingHelper.Swing_Draw_ItemAndTrailling(Color.White, ModAsset.Extra_5.Value, (_) => Color.White, effect);
            return false;
        }
        public override void NewSwingHelper()
        {
            swingHelper ??= new GeneralSwingHelper_DrawPosFixed(NPC, 30, DrawTex)
            {
                Size = Size,
                frameMax = 1
            };
        }
        public override bool SwitchCondition(NPCSkills changeToSkill)
        {
            if (CanThrow)
                return CanChange;
            return base.SwitchCondition(changeToSkill);
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            IsHit = false;
            ThrowVel = default;
            CanChange = false;
            IsBack = false;
            IsThrow = false;
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            (swingHelper as GeneralSwingHelper_DrawPosFixed).FixedDrawCenter = default;
            IsThrow = false;
        }
    }
}
