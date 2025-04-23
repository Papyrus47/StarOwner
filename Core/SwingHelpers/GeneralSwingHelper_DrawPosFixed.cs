using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.SwingHelpers
{
    public class GeneralSwingHelper_DrawPosFixed(object spawnEntity, int oldVelLength, Asset<Texture2D> swingItemTex = null) : GeneralSwingHelper(spawnEntity, oldVelLength, swingItemTex)
    {
        public Vector2 FixedDrawCenter;
        public override bool GetColliding(Rectangle targetHitBox)
        {
            float r = 0;
            if (FixedDrawCenter != default)
                return Collision.CheckAABBvLineCollision(targetHitBox.TopLeft(), targetHitBox.Size(), FixedDrawCenter, FixedDrawCenter + velocity.RotatedBy(rotation * spriteDirection),
                width / 2, ref r);
            return base.GetColliding(targetHitBox);
        }
        public override Vector2 GetDrawCenter(int index = 0)
        {
            if(FixedDrawCenter != default)
                return FixedDrawCenter;
            return base.GetDrawCenter(index);
        }
        public override Vector2 GetOldVel(int i, bool notFilp = true)
        {
            Vector2 vector2 = base.GetOldVel(i, notFilp);
            if(FixedDrawCenter != default)
                vector2 /= vector2.Length() / (_changeHeldLength + 0.5f);
            return vector2;
        }
    }
}
