using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Particles
{
    public class BrokenStarStickWhipExtra98 : BasicExtra98Particle
    {
        public BrokenStarStickWhipExtra98(Vector2 vel, Vector2 pos)
        {
            velocity = vel;
            position = pos;
            color = Color.Purple;
            color.A = 200;
            scale = new Vector2(1f, 3) * Main.rand.NextFloat(0.2f,0.7f);
            TimeLeftMax = 300;
        }
        public override void PostUpdate()
        {
            rotation = velocity.ToRotation() + MathHelper.PiOver2;
            scale.X *= 0.96f;
            if (scale.X < 0.2f)
                ShouldRemove = true;
        }
    }
}
