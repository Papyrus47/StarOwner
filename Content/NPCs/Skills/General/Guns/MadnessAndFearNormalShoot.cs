using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.Guns
{
    public class MadnessAndFearNormalShoot(NPC npc, Func<bool> canActive) : StarOwnerSkills(npc)
    {
        public Asset<Texture2D> Fear => ModContent.Request<Texture2D>(this.GetInstancePart() + "Fear");
        public Asset<Texture2D> Madness => ModContent.Request<Texture2D>(this.GetInstancePart() + "Madness");
        public Guns FearGun = new();
        public Guns MadnessGun = new();
        public Func<bool> CanActive = canActive;
        public override void AI()
        {
            Vector2 toTarget = (Target.Center - NPC.Center).SafeNormalize(default);
            NPC.spriteDirection = toTarget.X > 0 ? 1 : -1;
            NPC.velocity.X = 0;
            if (NPC.spriteDirection == -1)
            {
                FearGun.rot += MathHelper.WrapAngle(toTarget.ToRotation() - FearGun.rot + MathHelper.Pi) * 0.3f;
                MadnessGun.rot += MathHelper.WrapAngle(toTarget.ToRotation() - MadnessGun.rot + MathHelper.Pi) * 0.3f;
            }
            else
            {
                FearGun.rot += MathHelper.WrapAngle(toTarget.ToRotation() - FearGun.rot) * 0.3f;
                MadnessGun.rot += MathHelper.WrapAngle(toTarget.ToRotation() - MadnessGun.rot) * 0.3f;
            }
            MadnessGun.offset = toTarget.RotatedBy((0.2 + MathHelper.PiOver2) * -NPC.spriteDirection) * 15 + toTarget * 15;
            FearGun.offset = toTarget.RotatedBy(MathHelper.PiOver2 * -NPC.spriteDirection) * 5 + toTarget * 15;
            base.AI();
            Shoot();
            if (NPC.ai[1] > 8)
            {
                SkillTimeOut = true;
            }
        }
        public virtual void Shoot()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] > 60)
            {
                NPC.ai[0] = 50;
                NPC.ai[1]++;
                if ((int)NPC.ai[1] % 2 == 0)
                {
                    for(int i = 0;i < 15; i++)
                    {
                        Dust.NewDustPerfect(NPC.Center + FearGun.offset + FearGun.rot.ToRotationVector2() * 30, DustID.Smoke, FearGun.rot.ToRotationVector2().RotatedByRandom(0.3) * 6 * NPC.spriteDirection);
                    }
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + FearGun.offset + FearGun.rot.ToRotationVector2() * 30 * NPC.spriteDirection, (Target.Center - NPC.Center).SafeNormalize(default) * 15f, ProjectileID.Bullet, NPC.GetAttackDamage_ForProjectiles_MultiLerp(100,100,100), 0f, Target.whoAmI);
                    proj.friendly = false;
                    proj.hostile = true;
                    proj.extraUpdates = 5;
                    proj.timeLeft /= 15;
                    FearGun.rot -= MathHelper.PiOver2 * NPC.spriteDirection * 0.5f;
                }
                for (int i = 0; i < 3; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + MadnessGun.offset + MadnessGun.rot.ToRotationVector2() * 30 * NPC.spriteDirection, (Target.Center - NPC.Center).SafeNormalize(default) * (10 - i) * 0.5f, ProjectileID.BulletHighVelocity, 1, 0f, Target.whoAmI);
                    proj.friendly = false;
                    proj.hostile = true;
                    proj.extraUpdates = 15;
                    proj.timeLeft /= 5;
                }
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustPerfect(NPC.Center + MadnessGun.offset + MadnessGun.rot.ToRotationVector2() * 30, DustID.Smoke, MadnessGun.rot.ToRotationVector2().RotatedByRandom(0.3) * 6 * NPC.spriteDirection);
                }
                MadnessGun.rot -= MathHelper.PiOver2 * NPC.spriteDirection * 0.25f;
            }
        }
        public override bool ActivationCondition(NPCSkills activeSkill) => CanActive.Invoke();
        public override bool SwitchCondition(NPCSkills changeToSkill) => (Target.Center - NPC.Center).Length() < StarOwnerNPC.ClosePlayer || NPC.ai[1] > 6; 
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D madness = Madness.Value;
            Texture2D fear = Fear.Value;
            spriteBatch.Draw(madness, NPC.Center + MadnessGun.offset - screenPos, null, drawColor, MadnessGun.rot,new Vector2(madness.Width * 0.7f,madness.Height * 0.2f),NPC.scale,NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,0f);
            base.PreDraw(spriteBatch, screenPos, drawColor);
            spriteBatch.Draw(fear, NPC.Center + FearGun.offset - screenPos, null, drawColor, FearGun.rot, new Vector2(fear.Width * 0.7f, fear.Height * 0.2f), NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
    }
}
