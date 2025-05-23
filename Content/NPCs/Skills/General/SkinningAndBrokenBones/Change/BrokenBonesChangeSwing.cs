﻿using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General.SkinningAndBrokenBones.Change
{
    public class BrokenBonesChangeSwing(NPC npc, BasicSwingSkill.PreSwing preSwing, BasicSwingSkill.OnSwing onSwing, BasicSwingSkill.PostSwing postSwing, BasicSwingSkill.Setting setting) : BasicSwingSkill(npc, preSwing, onSwing, postSwing, setting)
    {
        public override Asset<Texture2D> DrawTex => ModContent.Request<Texture2D>(this.GetInstancePart() + "BrokenBonesChange");
        public override int WeaponDamage => 12;

        public override Vector2 Size => new(86,84);
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
            Matrix scale = Matrix.CreateScale(Main.GameViewMatrix.Zoom.Length() / MathF.Sqrt(2));
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * projection * scale);
            swingHelper.Swing_Draw_ItemAndTrailling(Color.White, ModAsset.Extra_0.Value, (_) => Color.White, effect);
            return false;
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            StarOwner.InStrongAxeMode = !Main.rand.NextBool(50);
        }
    }
}
