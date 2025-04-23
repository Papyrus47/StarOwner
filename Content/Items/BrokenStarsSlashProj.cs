using StarOwner.Core.SkillProjs;
using StarOwner.Core.SkillProjs.GeneralSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.Items
{
    public class BrokenStarsSlashProj : BasicMeleeWeaponSword
    {
        public override void Init()
        {
            OldSkills = new();

            float length = Projectile.Size.Length();
            SwingHelper_GeneralSwing swing1 = new(this, new()
            {
                ActionDmg = 10f,
                ChangeCondition = () => Main.mouseLeft,
                OnHitStopTime = 1,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.8f),
                VisualRotation = -0.2f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Projectile.rotation = -0.1f;
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f))
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing swing2 = new(this, new()
            {
                ActionDmg = 10f,
                ChangeCondition = () => Main.mouseLeft,
                OnHitStopTime = 1,
                StartVel = Vector2.UnitY.RotatedBy(0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.2f) * 1.2f,
                VisualRotation = -0.2f,
                SwingLenght = length,
                SwingDirectionChange = false,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Projectile.rotation = -0.1f;
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f))
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing swing3 = new(this, new()
            {
                ActionDmg = 10f,
                ChangeCondition = () => Main.mouseLeft,
                OnHitStopTime = 1,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f) * 1.1f,
                VisualRotation = -0.2f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Projectile.rotation = -0.1f;
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f))
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            swing1.OnSkillActive();
            swing1.AddSkill(swing2).AddSkill(swing3);
            CurrentSkill = swing1;
        }

        private bool DrawSword(SpriteBatch spriteBatch, Color drawColor)
        {
            Effect effect = ModAsset.StarOwnerSwingShader.Value;
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            gd.Textures[1] = ModAsset.ColorMap_0.Value;
            gd.Textures[2] = ModAsset.Stars.Value;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix scale = Matrix.CreateScale(Main.GameViewMatrix.Zoom.Length() / MathF.Sqrt(2));
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * projection * scale);
            SwingHelper.Swing_TrailingDraw(ModAsset.Extra_4.Value, (_) => Color.White, effect);
            //projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            //model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            //effect.Parameters["uTransform"].SetValue(model * projection);
            SwingHelper.Swing_Draw_ItemAndTrailling(drawColor, ModAsset.Extra_0.Value, (_) => Color.White, effect);
            return false;
        }

        public override bool PreSkillTimeOut()
        {
            Projectile.Kill();
            return false;
        }
    }
}
