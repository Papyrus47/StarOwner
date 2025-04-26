using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Graphics;
using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using StarOwner.Core.SwingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StarOwner.Content.NPCs.Skills.BasicSwingSkill;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace StarOwner.Content.NPCs.Skills.Phase2.BrokenStarStickWhip
{
    public class BrokenStarStickWhipSwing : StarOwnerSkills
    {
        public enum HeldType
        {
            Head,
            Middle,
            Tail
        }
        public class Whip(Asset<Texture2D> drawTex, Vector2 size)
        {
            public Asset<Texture2D> DrawTex = drawTex;
            public Whip FrontWhip;
            public Vector2 Position;
            public float Rotation;
            public Vector2 Velocity;
            public Vector2 Size = size;
            public float Length;
            public float ScaleY = 1f;
            public void Reset(Vector2 pos, float rot, float length,float scaleY = 1f)
            {
                Position = pos;
                Rotation = rot;
                Length = length;
                ScaleY = scaleY;
            }
            public void Update(float factor, Vector2 swingRot, Vector2 heldPos = default)
            {
                if (FrontWhip == null) // 鞭子的起始关节
                {
                    const float BaseSpeed = 2.2f;
                    Velocity = swingRot * BaseSpeed * factor;
                    Rotation = Velocity.ToRotation();
                    Position = heldPos + Velocity;
                    Velocity.Y *= ScaleY;
                    Velocity *= BaseSpeed;
                }
                else // 速度链式传递
                {
                    Velocity = Vector2.Lerp(Velocity,FrontWhip.Velocity,0.15f);
                    Velocity.Y *= ScaleY;

                    Position = FrontWhip.Position + Velocity.SafeNormalize(default) * Length;
                    Rotation = Velocity.ToRotation();
                    // 粒子效果
                    //if (factor > 0)
                    //{
                    //    var particleVel = Velocity.RotatedBy(MathHelper.PiOver2);
                    //    BrokenStarStickWhipExtra98 particle = new(particleVel, Position);
                    //    Core.Particles.ParticlesSystem.AddParticle(
                    //        Core.Particles.BasicParticle.DrawLayer.AfterDust,
                    //        particle
                    //    );
                    //}
                }
            }
            public bool Colliding(Rectangle targetRect)
            {
                float r = 0;
                Vector2 vel = Length * Rotation.ToRotationVector2();
                if (FrontWhip == null)
                    vel = Size.Length() * Rotation.ToRotationVector2();
                if (Collision.CheckAABBvLineCollision(targetRect.TopLeft(),targetRect.Size(),Position - vel,Position + Size.Length() * Rotation.ToRotationVector2() * 0.5f,5,ref r))
                    return true;
                return false;
            }
            public void Draw()
            {
                SpriteBatch sb = Main.spriteBatch;
                Texture2D tex = DrawTex.Value;
                // 在Whip.Draw()内添加
                //Main.spriteBatch.DrawString(FontAssets.MouseText.Value,
                //    $"V:{Velocity.Length():F1}",
                //    Position - Main.screenPosition + new Vector2(0, 20),
                //    Color.Red);

                if (FrontWhip != null)
                {
                    Utils.DrawLine(sb, Position, FrontWhip.Position, Color.Purple * 0.5f, Color.Purple * 0.8f, 1f);
                }
                sb.Draw(tex, Position - Main.screenPosition, null, Color.White, Rotation - MathHelper.PiOver4, tex.Size() * 0.5f, 1f, SpriteEffects.FlipHorizontally, 0f);
            }
        }
        public BrokenStarStickWhipSwing(NPC npc, HeldType heldType, Vector2 startSwing, float swingRot, float swingTime, bool swingDir, Func<bool> canActive) : base(npc)
        {
            whip = new Whip[5];
            whip[0] = new(ModContent.Request<Texture2D>(this.GetInstancePartWithName() + "1"), new Vector2(32));
            whip[1] = new(ModContent.Request<Texture2D>(this.GetInstancePartWithName() + "2"), new Vector2(32));
            whip[2] = new(ModContent.Request<Texture2D>(this.GetInstancePartWithName() + "3"), new Vector2(32));
            whip[3] = new(ModContent.Request<Texture2D>(this.GetInstancePartWithName() + "4"), new Vector2(32));
            whip[4] = new(ModContent.Request<Texture2D>(this.GetInstancePartWithName() + "5"), new Vector2(32));
            held = heldType;
            if (held == HeldType.Head)
            {
                whip[1].FrontWhip = whip[0];
                whip[2].FrontWhip = whip[1];
                whip[3].FrontWhip = whip[2];
                whip[4].FrontWhip = whip[3];
            }
            else if (held == HeldType.Middle)
            {
                whip[1].FrontWhip = whip[3].FrontWhip = whip[2];
                whip[0].FrontWhip = whip[1];
                whip[4].FrontWhip = whip[3];
            }
            else if (held == HeldType.Tail)
            {
                whip[0].FrontWhip = whip[1];
                whip[1].FrontWhip = whip[2];
                whip[2].FrontWhip = whip[3];
                whip[3].FrontWhip = whip[4];
            }

            StartSwing = startSwing;
            SwingRot = swingRot;
            SwingTime = swingTime;
            SwingDir = swingDir;
            CanActive = canActive;
        }
        public Func<bool> CanActive;
        public HeldType held;
        public Whip[] whip;
        public Vector2 StartSwing;
        public float SwingRot;
        public float SwingTime;
        public bool SwingDir;
        public int OnHitTime;
        public float ActionDmg;
        public float ScaleY = 1f;
        public override void AI()
        {
            base.AI();
            //NPC.velocity.X = 0;
            for (int k = 0; k < 5; k++)
            {
                if (OnHitTime > 0)
                {
                    OnHitTime--;
                    NPC.ai[0] += 0.5f;
                }
                else
                    NPC.ai[0]++;
                float factor = NPC.ai[0] / (SwingTime * 10);
                if (factor > 1.2f)
                {
                    SkillTimeOut = true;
                    return;
                }
                factor = Math.Clamp(factor, 0.01f, 1f);
                int length = whip.Length;
                Vector2 startVel = StartSwing;
                int dirSign = NPC.spriteDirection == 1 ? 1 : -1;
                startVel = startVel.RotatedBy(SwingRot * factor * SwingDir.ToDirectionInt());
                startVel.X *= SwingDir.ToDirectionInt() * dirSign;

                for (int i = 0; i < length; i++)
                {
                    switch (held)
                    {
                        case HeldType.Head:
                            whip[0].Update(factor, startVel, NPC.Center);
                            if(i != 0)
                                whip[i].Update(factor, startVel);
                            break;
                        case HeldType.Middle:
                            whip[2].Update(factor, startVel, NPC.Center);
                            if (i != 2)
                                whip[i].Update(factor, startVel);
                            break;
                        case HeldType.Tail:
                            whip[length - 1].Update(factor, startVel, NPC.Center);
                            if (i != length - 1)
                                whip[i].Update(factor, startVel);
                            break;
                    }
                }
                if (NPC.ai[0] > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (whip[i].Colliding(Target.getRect()) && !Target.immune)
                        {
                            OnHitTime = 5;
                            Player.HurtModifiers hurtModifiers = new()
                            {
                                Dodgeable = false,
                            };
                            hurtModifiers.SourceDamage += ActionDmg - 1f;
                            hurtModifiers.SourceDamage += StarOwner.DamageAdd;
                            Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(4, Target.statDefense, Target.DefenseEffectiveness.Value, 0, true);
                            hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                            Target.Hurt(hurtInfo);
                            for (int j = Main.rand.Next(5, 8); j > 0; j--)
                            {
                                HitPiecredExtra98 hitPiecredExtra98 = new(whip[i].Rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2 * SwingDir.ToDirectionInt() * NPC.spriteDirection) * 0.2f, Target.Center);
                                Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                            }
                            Target.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 15;
                            StarOwner.DamageAdd += 0.5f;
                            Target.immuneTime /= 3;
                            Target.GetModPlayer<StarPowerPlayer>().starPower.Value += 4;
                        }
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            int length = whip.Length;
            for (int i = 0; i < length; i++)
            {
                whip[i].Draw();
            }
            return false;
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > SwingTime * 10  *1.1f;
        public override bool ActivationCondition(NPCSkills activeSkill) => CanActive.Invoke();
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            NPC.ai[0] = -30;
            int length = whip.Length;
            Vector2 startVel = StartSwing;
            startVel.X *= NPC.spriteDirection;
            for (int i = 0; i < length; i++)
            {
                switch (held)
                {
                    case HeldType.Head:
                        if (i == 0)
                            whip[i].Reset(NPC.Center, startVel.ToRotation(), 70f, ScaleY);
                        else
                            whip[i].Reset(NPC.Center, startVel.ToRotation(), 70f, ScaleY);
                        break;
                    case HeldType.Middle:
                        if (i == 2)
                            whip[i].Reset(NPC.Center, startVel.ToRotation(), 70f, ScaleY);
                        else if (i < 2)
                            whip[i].Reset(NPC.Center, startVel.ToRotation(), 70f, ScaleY);
                        else
                            whip[i].Reset(NPC.Center, -startVel.ToRotation(), -70f, ScaleY);
                        break;
                    case HeldType.Tail:
                        if (i == length - 1)
                            whip[i].Reset(NPC.Center, startVel.ToRotation(), 70f, ScaleY);
                        else
                            whip[i].Reset(NPC.Center, startVel.ToRotation(), 70f, ScaleY);
                        break;
                }
            }
        }
    }
}
