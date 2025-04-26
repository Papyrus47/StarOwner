namespace StarOwner.Core.SwingHelpers
{
    public class GunSwingHelper : GeneralSwingHelper
    {
        /// <summary>
        /// 是否相反
        /// </summary>
        public bool IsOpposite;
        public GunSwingHelper(object spawnEntity, int oldVelLength, Asset<Texture2D> swingItemTex = null) : base(spawnEntity, oldVelLength, swingItemTex)
        {
        }
        public override void Swing_Draw_Afterimage(Func<float, Color> drawColorFunc, int drawCount = -1)
        {
            if (!_canDrawTrailing) return;

            GraphicsDevice gd = Main.graphics.GraphicsDevice;

            CustomVertexInfo[] customVertices = new CustomVertexInfo[6];
            int velLength = oldVels.Length;
            List<CustomVertexInfo> customVertexInfos = new List<CustomVertexInfo>();
            for (int i = 0; i < velLength; i++)
            {
                if (i > drawCount && drawCount != -1) break;

                float factor = (oldFrames[i] + 1f) / frameMax;
                Vector2 velocity = GetOldVel(i);
                Vector2 halfLength = new Vector2(-velocity.Y, velocity.X).RotatedBy(VisualRotation * spriteDirection).SafeNormalize(default)
                    * _halfSizeLength * spriteDirection;
                Vector2 center = GetDrawCenter(i);
                if (_drawCorrections)
                {
                    center = Center + (center - Center);
                }
                Vector2 halfVelPos = center + velocity * 0.5f;
                Vector2[] pos = new Vector2[4]
                {
                    center + velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * 0.5f * Size.Y - Main.screenPosition,
                    center - velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * 0.5f * Size.Y - Main.screenPosition,
                    center + velocity - velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * 0.5f * Size.Y - Main.screenPosition,
                    center + velocity + velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2) * 0.5f * Size.Y - Main.screenPosition,
                };
                Color drawColor = drawColorFunc.Invoke((float)i / velLength);
                customVertices[0] = customVertices[5] = new(pos[0], drawColor, new Vector3(factor, IsOpposite ? 0 : 1, 0)); // 左下角
                customVertices[1] = new(pos[1], drawColor, new Vector3(factor - 1f, IsOpposite ? 0 : 1, 0)); // 左上角
                customVertices[2] = customVertices[3] = new(pos[2], drawColor, new Vector3(factor - 1f, !IsOpposite ? 0 : 1, 0)); // 右上角
                customVertices[4] = new(pos[3], drawColor, new Vector3(factor, !IsOpposite ? 0 : 1, 0)); // 右下角

                customVertexInfos.AddRange(customVertices);
            }
            gd.Textures[0] = SwingItemTex.Value;
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, customVertexInfos.ToArray(), 0, customVertexInfos.Count / 3);
        }
        public override void DrawSwingItem(Color drawColor)
        {
            //if (projectile.localAI[0]++ > 2)
            //    Main.NewText(true);
            GraphicsDevice gd = Main.graphics.GraphicsDevice;
            if (projectile != null)
            {
                SwingItemTex ??= TextureAssets.Projectile[projectile.type];
            }
            //var origin = gd.RasterizerState;
            //RasterizerState rasterizerState = new()
            //{
            //    CullMode = CullMode.None,
            //    FillMode = FillMode.WireFrame
            //};
            //gd.RasterizerState = rasterizerState;

            Vector2 velocity = GetOldVel(-1, true).RotatedBy(rotation * spriteDirection);
            if (!DrawItem_ScaleMoreThanOne)
                velocity = velocity.Length() > Size.Length() ? velocity.SafeNormalize(Vector2.UnitX) * Size.Length() : velocity;
            //Vector2 halfLength = new Vector2(-velocity.Y, velocity.X).RotatedBy(VisualRotation * spriteDirection).SafeNormalize(default)
            //    * _halfSizeLength * spriteDirection;

            Vector2 center = GetDrawCenter();
            if (_drawCorrections)
            {
                center = Center + (center - Center);
            }
            //Vector2 halfVelPos = center + velocity * 0.5f;
            float rot = VisualRotation;
            float realRot = rot * spriteDirection;
            Vector2[] pos = new Vector2[4]
            {
                center + velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2 + realRot) * 0.5f * spriteDirection * Size.Y - Main.screenPosition,
                center - velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2 + realRot) * 0.5f * spriteDirection * Size.Y - Main.screenPosition,
                center + velocity - velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2 + realRot) * spriteDirection * 0.5f * Size.Y - Main.screenPosition,
                center + velocity + velocity.SafeNormalize(default).RotatedBy(MathHelper.PiOver2 + realRot) * spriteDirection * 0.5f * Size.Y - Main.screenPosition,
            };

            float factor = (frame + 1f) / frameMax;
            CustomVertexInfo[] customVertices = new CustomVertexInfo[6];
            customVertices[0] = customVertices[5] = new(pos[0], drawColor, new Vector3(factor, IsOpposite ? 0 : 1, 0)); // 左下角
            customVertices[1] = new(pos[1], drawColor, new Vector3(factor - 1f, IsOpposite ? 0 : 1, 0)); // 左上角
            customVertices[2] = customVertices[3] = new(pos[2], drawColor, new Vector3(factor - 1f, !IsOpposite ? 0 : 1, 0)); // 右上角
            customVertices[4] = new(pos[3], drawColor, new Vector3(factor, !IsOpposite ? 0 : 1, 0)); // 右下角

            gd.Textures[0] = SwingItemTex.Value;
            //gd.Textures[0] = TextureAssets.MagicPixel.Value;
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, customVertices, 0, 2);
            //gd.RasterizerState = origin;
        }
    }
}
