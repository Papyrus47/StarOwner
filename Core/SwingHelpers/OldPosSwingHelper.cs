namespace StarOwner.Core.SwingHelpers
{
    /// <summary>
    /// 旧位置的SwingHelper
    /// </summary>
    public class OldPosSwingHelper : SwingHelper
    {
        public Vector2[] oldPos;
        public OldPosSwingHelper(object spawnEntity, int oldVelLength, Asset<Texture2D> swingItemTex = null) : base(spawnEntity, oldVelLength, swingItemTex)
        {
            oldPos = new Vector2[oldVelLength];
        }
        public override void SwingAI(float velLength, int dir, float Rot)
        {
            #region 保存oldPos
            for (int i = oldPos.Length - 1; i >= 0; i--)
            {
                if (_acitveSwing && !_changeLerpInvoke)
                {
                    if (!_oldVelsSave) // 不保存旧速度
                        continue;
                    if (i == 0)
                    {
                        oldPos[i] = Center;
                    }
                    else
                    {
                        oldPos[i] = oldPos[i - 1];
                    }
                }
                else
                {
                    oldPos[i] = default;
                }
            }
            #endregion
            base.SwingAI(velLength, dir, Rot);
        }
        public override Vector2 GetDrawCenter(int index = 0)
        {
            Vector2 pos;
            if (index < 0)
                return Center;
            pos = oldPos[index];
            return pos;
        }
        public override object Clone()
        {
            OldPosSwingHelper sh = new(SpawnEntity, oldVels.Length, SwingItemTex)
            {
                oldVels = oldVels.Clone() as Vector2[],
                oldPos = oldPos.Clone() as Vector2[],
                velocity = velocity,
                rotation = rotation,
                spriteDirection = spriteDirection,
                Size = Size,
                StartVel = StartVel,
                oldFrames = oldFrames.Clone() as int[],
                UseShaderPass = UseShaderPass,
                _acitveSwing = _acitveSwing,
                _canDrawTrailing = _canDrawTrailing,
                _changeHeldLength = _changeHeldLength,
                _halfSizeLength = _halfSizeLength,
                _oldVelsSaveClean = _oldVelsSaveClean,
                _velLerp = _velLerp,
                _velRotBy = _velRotBy,
                _drawCorrections = _drawCorrections,
                VelScale = VelScale,
                VisualRotation = VisualRotation,
                DrawTrailCount = DrawTrailCount
            };
            return sh;
        }
    }
}
