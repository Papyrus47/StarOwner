using Microsoft.Xna.Framework.Graphics.PackedVector;
using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using StarOwner.Core.SwingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.StarPierced
{
    public class StarPiercedSpurt(NPC npc, Func<Vector2> toTarget, Func<bool> canActive) : StarOwnerSkills(npc)
    {
        public Func<Vector2> toTarget = toTarget;
        public Vector2 vel;
        public float rot;
        public int spDir;
        public bool ByDef;
        public int ChangeTime;
        public float ActionDmg = 1f;
        public Action<StarPiercedSpurt> OnUse;
        public Func<bool> CanActive = canActive;
        public override void AI()
        {
            NPC.ai[0]++;
            OnUse?.Invoke(this);
            Vector2 vector = toTarget.Invoke();
            if (NPC.velocity.X < 0.2f && NPC.velocity.X > -0.2f)
                NPC.velocity.X += Math.Sign(Target.Center.X - NPC.Center.X) * 0.2f;
            else
                NPC.velocity.X *= 0.5f;
            NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;

            if (NPC.ai[0] < ChangeTime)
                vel = vector.SafeNormalize(default) * 2;
            else if ((int)NPC.ai[0] == ChangeTime)
            {
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing.WithPitchOffset(0.8f), NPC.Center);
                //for (int i = 0; i < 110; i++)
                //{
                //    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.PinkFairy, vel.SafeNormalize(default) * i * 0.25f);
                //    dust.noGravity = true;
                //}
            }
            else if (!ByDef && NPC.ai[0] < ChangeTime + 8)
            {
                vel = Vector2.Lerp(vel, vector, 0.5f);
                StarPiecredExtra98 starPiecredExtra98 = new(vel, NPC.Center + vel);
                Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, starPiecredExtra98);
            }
            else
                vel = Vector2.Lerp(default, vel, 0.8f);
            spDir = vel.X >= 0 ? 1 : -1;
            rot = MathF.Atan2(vel.Y * spDir, vel.X * spDir) + MathHelper.PiOver4 * spDir;
            float r = 0;
            #region 碰撞检测
            if (NPC.ai[0] > ChangeTime && Collision.CheckAABBvLineCollision(Target.getRect().TopLeft(), Target.getRect().Size(), NPC.Center, NPC.Center + vel + vel.SafeNormalize(default) * 110, 6, ref r) && !Target.immune && !ByDef)
            {
                DefAttackAndDodgePlayer defAttackPlayer = Target.GetModPlayer<DefAttackAndDodgePlayer>();
                if (defAttackPlayer.defenseAttack > 0)
                {
                    ByDef = true;
                    StarOwner.byDefAtk++;
                    defAttackPlayer.defenseCD = 0;
                    SoundEngine.PlaySound(SoundID.Item178.WithVolume(3).WithPitchOffset(0.2f), Target.Center);
                }
                else
                {
                    Player.HurtModifiers hurtModifiers = new()
                    {
                        Dodgeable = false,
                    };
                    hurtModifiers.SourceDamage += ActionDmg - 1f;
                    hurtModifiers.SourceDamage += StarOwner.DamageAdd;
                    Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(3, Target.statDefense, Target.DefenseEffectiveness.Value, 0, true);
                    hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                    Target.Hurt(hurtInfo);
                    for (int j = Main.rand.Next(5, 8); j > 0; j--)
                    {
                        HitPiecredExtra98 hitPiecredExtra98 = new(vel * 0.2f, Target.Center);
                        Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                    }
                    Target.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 10;
                    StarOwner.DamageAdd += 0.5f;
                    //Target.immuneTime /= 3;
                    Core.SPBuffs.StarPower starPower = Target.GetModPlayer<StarPowerPlayer>().starPower;
                    StarOwner.starPower.Value += starPower.Value;
                    starPower.Value = 0;
                    Target.velocity += vel.SafeNormalize(default);
                   Main.instance.CameraModifiers.Add(new PunchCameraModifier(Target.position, -Vector2.UnitX * Target.direction, MathF.Log2(hurtInfo.Damage), 60, 20));
                }
            }
            #endregion
            if(NPC.ai[0] > ChangeTime + 25)
            {
                SkillTimeOut = true;
                return;
            }
            base.AI();
            StarOwner.drawPlayer.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.Atan2(vel.Y * StarOwner.drawPlayer.direction, vel.X * StarOwner.drawPlayer.direction) - MathHelper.PiOver2 * StarOwner.drawPlayer.direction);
        }
        public override bool ActivationCondition(NPCSkills activeSkill) => CanActive.Invoke();
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > ChangeTime + 20;
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            vel = default;
            ByDef = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            #region 绘制剑本体
            Texture2D tex = ModContent.Request<Texture2D>(this.GetInstancePart() + "StarPierced").Value;
            Main.EntitySpriteDraw(tex, NPC.Center + vel - screenPos, null, drawColor, rot, tex.Size() * 0.5f, NPC.scale, spDir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            #endregion
            return false;
        }
    }
}
