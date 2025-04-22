using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General
{
    public class BrokenStarStickSwing : BasicSwingSkill
    {
        public BrokenStarStickSwing(NPC npc, PreSwing preSwing, OnSwing onSwing, PostSwing postSwing, Setting setting) : base(npc, preSwing, onSwing, postSwing, setting)
        {
            if(onSwing != null)
            {
                onSwing.modifyHit = ModifyHitTarget;
            }
        }

        public void ModifyHitTarget(Player target, ref Player.HurtModifiers hurtInfo)
        {
            float dis = target.Center.Distance(NPC.Center);
            hurtInfo.FinalDamage *= dis / Size.Length();
        }

        public override Asset<Texture2D> DrawTex => ModContent.Request<Texture2D>(this.GetInstancePart() + "BrokenStarStick");
        public override int WeaponDamage => 4;

        public override Vector2 Size => new(230);

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PreDraw(spriteBatch, screenPos, drawColor);
            if ((SwingType)NPC.ai[0] == SwingType.PreSwing && NPC.ai[1] < 2)
                return false;
            Effect effect = ModAsset.StarOwnerSwingShader.Value;
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            gd.Textures[1] = ModAsset.ColorMap_1.Value;
            gd.Textures[2] = ModAsset.Stars.Value;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * projection);
            swingHelper.Swing_Draw_ItemAndTrailling(Color.White, ModAsset.Extra_5.Value, (_) => Color.White, effect);
            return false;
        }
    }
}
