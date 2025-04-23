using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Particles
{
    public class StarPiecredExtra98 : BasicExtra98Particle
    {
        public StarPiecredExtra98(Vector2 vel,Vector2 pos)
        {
            position = pos;
            velocity = vel;
            color = Color.Purple;
            scale = new Vector2(1,5);
            TimeLeftMax = 4000;
        }
        public override void PostUpdate()
        {
            rotation = velocity.ToRotation() + MathHelper.PiOver2;
            scale.X *= 0.9f;
            if (scale.X < 0.2f)
                ShouldRemove = true;
        }
        public override bool ShouldUpdatePos() => false;
    }
}
