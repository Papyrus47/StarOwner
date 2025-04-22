using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.Guns
{
    public class MadnessAndFearFullShot : MadnessAndFearNormalShoot
    {
        public MadnessAndFearFullShot(NPC npc, Func<bool> canActive) : base(npc, canActive)
        {
        }
        public override void Shoot()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] > 120)
            {
                if ((int)NPC.ai[2] != 0)
                {
                    SkillTimeOut = true;
                    return;
                }
                NPC.ai[0] = 110;
                NPC.ai[1]++;

                if ((int)NPC.ai[1] % 2 == 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDustPerfect(NPC.Center + FearGun.offset + FearGun.rot.ToRotationVector2() * 30, DustID.Smoke, FearGun.rot.ToRotationVector2().RotatedByRandom(0.3) * 3 * NPC.spriteDirection);
                    }
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + FearGun.offset + FearGun.rot.ToRotationVector2() * 30 * NPC.spriteDirection, (Target.Center - NPC.Center).SafeNormalize(default) * 15f, ProjectileID.Bullet, NPC.GetAttackDamage_ForProjectiles_MultiLerp(100, 100, 100), 0f, Target.whoAmI);
                    proj.friendly = false;
                    proj.hostile = true;
                    proj.tileCollide = false;
                    proj.extraUpdates = 150;
                    FearGun.rot -= MathHelper.PiOver2 * NPC.spriteDirection * 0.5f;
                }
                for (int i = 0; i < 3; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + MadnessGun.offset + MadnessGun.rot.ToRotationVector2() * 30 * NPC.spriteDirection, (Target.Center - NPC.Center).SafeNormalize(default) * (10 - i) * 0.5f, ProjectileID.BulletHighVelocity, 1, 0f, Target.whoAmI);
                    proj.friendly = false;
                    proj.hostile = true;
                    proj.tileCollide = false;
                    proj.extraUpdates = 150;
                }
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustPerfect(NPC.Center + MadnessGun.offset + MadnessGun.rot.ToRotationVector2() * 30, DustID.Smoke, MadnessGun.rot.ToRotationVector2().RotatedByRandom(0.3) * 3 * NPC.spriteDirection);
                }
                MadnessGun.rot -= MathHelper.PiOver2 * NPC.spriteDirection * 0.25f;
            }
            else if (NPC.ai[1] < 1)
            {
                FearGun.rot = MadnessGun.rot = NPC.ai[0] * 0.5f;
                if(NPC.Center.Distance(Target.Center) < StarOwnerNPC.ClosePlayer * 1.5f)
                    NPC.ai[2] = 1;
                else
                    NPC.ai[2] = 0;
            }
        }
    }
}
