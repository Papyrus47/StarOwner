using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Particles
{
    public class HitPiecredExtra98 : BasicExtra98Particle
    {
        public Vector2 gravity = new Vector2(0, 0.5f);
        public HitPiecredExtra98(Vector2 vel,Vector2 pos)
        {
            vel.Y = -15;
            velocity = vel.RotatedByRandom(0.5);
            position = pos;
            color = Color.Lerp(Color.DarkRed,Color.Red,Main.rand.NextFloat());
            color.A = 100;
            scale = new Vector2(0.2f, 1) * Main.rand.NextFloat(1, 3.5f);
            TimeLeftMax = 30;
        }
        public override void PostUpdate()
        {
            if (velocity.Length() > 10)
                velocity = velocity.SafeNormalize(default) * 10;
            //velocity = velocity.RotatedByRandom(0.2);
            velocity += gravity;
            rotation = velocity.ToRotation() + MathHelper.PiOver2;
            scale *= 0.9f;
        }
    }
}
