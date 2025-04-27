using StarOwner.Core.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase4.Proj
{
    public class IceSoul : ModProjectile
    {
        public static readonly SoundStyle GhostSound = new(ModAsset.GhostSummon_Mod);
        public NPC Owner => Main.npc[(int)Projectile.ai[0]];
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.alpha = 100;
            Projectile.timeLeft = 3600;
            Projectile.aiStyle = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            SoundEngine.PlaySound(GhostSound, Projectile.Center);
        }
        public override void AI()
        {
            if (!Owner.active)
                Projectile.Kill();
            if (Projectile.timeLeft < 150)
                Projectile.alpha++;
            Projectile.spriteDirection = Owner.spriteDirection;
            Projectile.Center = Owner.Center + new Vector2(Projectile.spriteDirection == 1 ? -30 : 20, -40);
            Player player = Main.player[Projectile.owner];
            if(player.Center.Distance(Projectile.Center) < 200 && Projectile.ai[1] <= 0)
            {
                Projectile.ai[1] = 180;
                player.GetModPlayer<ControlPlayer>().IceControl = 60;
            }
            if (Projectile.ai[1] > 0)
                Projectile.ai[1]--;

            for(int i = 0; i < 300; i++)
            {
                float factor = i / 300f;
                Dust dust = Dust.NewDustDirect(Projectile.Center + Vector2.UnitX.RotatedBy(factor * MathHelper.TwoPi) * 200, 1, 1, DustID.t_Frozen);
                dust.noGravity = true;
                dust.velocity *= 0f;
                dust.scale *= 0.3f;
            }
        }
    }
}
