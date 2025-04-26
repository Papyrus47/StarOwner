using Microsoft.Xna.Framework.Graphics;
using StarOwner.Content.NPCs.Skills.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase2.WalkerOfRain
{
    public class WalkerOfRainSwing : BasicSwingSkill,IDefenceAttack
    {
        public bool IsOpen;
        public WalkerOfRainSwing(NPC npc, PreSwing preSwing, OnSwing onSwing, PostSwing postSwing, Setting setting) : base(npc, preSwing, onSwing, postSwing, setting)
        {
        }
        public bool CanDefence() => IsOpen;
        public void OnDefenceSucceed() 
        {
            DefenceSucceed = false;
            StarOwner.WalkerOfRain_Boom(this);
            NPC.ai[1] = 0;
        }
        public override Asset<Texture2D> DrawTex => ModContent.Request<Texture2D>(this.GetInstancePart() + "WalkerOfRainClose");

        public override int WeaponDamage => 7;

        public override Vector2 Size => new(86,26);

        public bool DefenceSucceed { get; set; }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            if ((SwingType)NPC.ai[0] == SwingType.PreSwing && NPC.ai[1] < 2)
                return false;
            if (!IsOpen)
            {
                Effect effect = ModAsset.StarOwnerSwingShader.Value;
                GraphicsDevice gd = Main.instance.GraphicsDevice;
                gd.Textures[1] = ModAsset.ColorMap_3.Value;
                gd.Textures[2] = ModAsset.Perlin.Value;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                Matrix scale = Matrix.CreateScale(Main.GameViewMatrix.Zoom.Length() / MathF.Sqrt(2));
                Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
                effect.Parameters["uTransform"].SetValue(model * projection * scale);
                swingHelper.Swing_Draw_ItemAndTrailling(Color.White, ModAsset.Extra_0.Value, (_) => Color.White, effect);
            }
            else // 只绘制伞本体
            {
                swingHelper.Swing_Draw_ItemAndTrailling(Color.White, null,null);
            }
            return false;
        }
        public override void NewSwingHelper()
        {
            if (IsOpen)
            {
                swingHelper ??= new GunSwingHelper(NPC, 1, ModContent.Request<Texture2D>(this.GetInstancePart() + "WalkerOfRainOpen"))
                {
                    Size = new(78,86),
                    frameMax = 1
                };
            }
            else
            {
                swingHelper ??= new GunSwingHelper(NPC, 30, DrawTex)
                {
                    Size = Size,
                    frameMax = 1
                };
            }
        }
    }
}
