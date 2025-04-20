using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.Particles
{
    public abstract class BasicPartcle
    {
        public enum DrawLayer : byte
        {
            /// <summary>
            /// 弹幕之后
            /// </summary>
            AfterProj,
            /// <summary>
            /// 粒子之后
            /// </summary>
            AfterDust,
            /// <summary>
            /// 物块之后
            /// </summary>
            AfterTile,
            /// <summary>
            /// 玩家之后
            /// </summary>
            AfterPlayer
        }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// 速度
        /// </summary>
        public Vector2 velocity;
        /// <summary>
        /// 旋转角度
        /// </summary>
        public float rotation;
        /// <summary>
        /// 绘制的缩放
        /// </summary>
        public Vector2 scale = Vector2.One;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color color = Color.White;
        /// <summary>
        /// 生命周期
        /// </summary>
        public float TimeLeft;
        /// <summary>
        /// 最大生命周期
        /// </summary>
        public float TimeLeftMax;
        /// <summary>
        /// 是否应该移除
        /// </summary>
        public bool ShouldRemove;
        /// <summary>
        /// 额外更新次数
        /// </summary>
        public int extraUpdate;
        public virtual Asset<Texture2D> Texture => ModContent.Request<Texture2D>(GetType().Namespace.Replace(".", "/") + "/" + GetType().Name);
        /// <summary>
        /// <see cref="Update"/>:内部有粒子位置更新,计时器减少的逻辑,不推荐去掉base的调用
        /// </summary>
        public virtual void Update()
        {
            if (ShouldUpdatePos())
                position += velocity;
            TimeLeft++;
            if (TimeLeft >= TimeLeftMax)
                ShouldRemove = true;
            PostUpdate();
        }
        /// <summary>
        /// 默认绘制在贴图中心位置,需要更改请重写<see cref="PreDraw(SpriteBatch)"/><para></para>
        /// <see cref="Draw"/>:绘制粒子,不推荐去掉base的调用
        /// </summary>
        public virtual void Draw() 
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (PreDraw(sb))
            {
                if (!Texture.IsLoaded)
                    _ = Texture;
                Texture2D texture = Texture.Value;
                sb.Draw(texture, position - Main.screenPosition, null, color, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            }
            PostDraw(sb);
            sb.End();
        }
        /// <summary>
        /// 调用在的末尾<see cref="Update"/>
        /// </summary>
        public virtual void PostUpdate() { }
        public virtual bool PreDraw(SpriteBatch spriteBatch) => true;
        public virtual void PostDraw(SpriteBatch spriteBatch) { }
        public virtual bool ShouldUpdatePos() => true;
    }
}
