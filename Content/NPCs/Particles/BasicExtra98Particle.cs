using StarOwner.Core.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Particles
{
    public abstract class BasicExtra98Particle : BasicParticle
    {
        public override Asset<Texture2D> Texture => TextureAssets.Extra[98];
    }
}
