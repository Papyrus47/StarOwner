using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.SwingHelpers
{
    public class GeneralSwingHelper : SwingHelper
    {
        public GeneralSwingHelper(object spawnEntity, int oldVelLength, Asset<Texture2D> swingItemTex = null) : base(spawnEntity, oldVelLength, swingItemTex)
        {
        }
        public override Vector2 Center { get; set; }
        public override Vector2 velocity { get; set; }
        public override int frame { get; set; }
        public override int frameMax { get; set; }
        public override Vector2 Size { get; set; }
        public override float rotation { get; set; }
        public override int spriteDirection { get; set; }
        public override int width { get; set; }
    }
}
