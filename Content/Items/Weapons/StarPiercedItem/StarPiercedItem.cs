using StarOwner.Content.Items.Weapons.BrokenStarsSlashItem;
using StarOwner.Content.NPCs;
using StarOwner.Content.NPCs.Particles;
using StarOwner.Core.GlobalNPCs;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.Particles;
using StarOwner.Core.SkillProjs;
using StarOwner.Core.SkillProjs.GeneralSkills;
using StarOwner.Core.SpurtHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;
using static StarOwner.QuickAssetReference.ModAssets_Texture2D.Content.NPCs;

namespace StarOwner.Content.Items.Weapons.StarPiercedItem
{
    public class StarPiercedItem : ModItem, IDamageAdd
    {
        public float DamageAdd { get; set; }
        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.width = 104;
            Item.height = 104;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(1);
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<StarPiercedPlayerProj>();
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, player.velocity, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
                proj.originalDamage = proj.damage;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
    public class StarPiercedPlayerProj : BasicMeleeWeaponSword
    {
        public StarPiercedItem StarPiercedItem => SpawnItem.ModItem as StarPiercedItem;
        public SpurtHelper spurtHelper;
        public SwingHelper_NoUse_Held noUse_Held;
        public SwingHelper_NoUse_Carry noUse_Carry;
        public override string Texture => ModAsset.StarPiercedItem_Mod;
        public override bool IsItemHit => false;
        public override void Init()
        {
            spurtHelper = new(Projectile);
            float length = Projectile.Size.Length();
            noUse_Carry = new(Player, SwingHelper, this)
            {
                Length = length,
                CanChange = () => IsMouseLeftChick
            };
            noUse_Held = new(Player, SwingHelper, this)
            {
                Length = length
            };
            SpurtHelper_GeneralOneSpurt combo1_1 = new(this, new()
            {
                OnHit = SeizeStarPower,
                UseTimeMax = () => (int)(SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem)),
                SpurtLength = () => 250,
                ActivationCondition = () => IsMouseLeftChick
            },spurtHelper,Player);
            SpurtHelper_GeneralOneSpurt combo1_2 = new(this, new()
            {
                OnHit = SeizeStarPower,
                UseTimeMax = () => (int)(SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem)),
                SpurtLength = () => 250,
                ModifyHit = (NPC _, ref NPC.HitModifiers modifiers) => modifiers.SourceDamage += 0.5f,
                ActivationCondition = () => IsMouseLeftChick
            }, spurtHelper, Player)
            {
                OnUse = (skill) =>
                {
                    if (Projectile.ai[1] < 90)
                    {
                        skill.SpurtHelper.Time = 0;
                        Projectile.ai[0] = -1;
                        Projectile.ai[1]++;
                        Projectile.velocity = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);
                    }
                    else
                    {
                        skill.SpurtHelper.Time++;
                        Projectile.ai[0]++;
                    }
                }
            };

            SwingHelper_GeneralSwing combo2_1 = new(this, new()
            {
                ActionDmg = 1f,
                ChangeCondition = () => IsMouseLeftChick && StopTime > 3,
                OnHitStopTime = 4,
                StartVel = Vector2.UnitY.RotatedBy(0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f),
                VisualRotation = 0f,
                SwingLenght = length,
                SwingDirectionChange = false,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(0.3f)
            };
            SwingHelper_GeneralSwing combo2_2 = new(this, new()
            {
                ActionDmg = 1f,
                ChangeCondition = () => IsMouseLeftChick,
                OnHitStopTime = 4,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f),
                VisualRotation = 0f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(0.3f)
            };
            SpurtHelper_GeneralOneSpurt combo2_3 = new(this, new()
            {
                OnHit = SeizeStarPower,
                UseTimeMax = () => (int)(SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem)),
                SpurtLength = () => 250,
                ActivationCondition = () => IsMouseLeftChick
            }, spurtHelper, Player);

            SwingHelper_GeneralSwing combo3_1 = new(this, new()
            {
                ActionDmg = 1f,
                ChangeCondition = () => IsMouseLeftChick && StopTime > 3,
                OnHitStopTime = 4,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.2f),
                VisualRotation = -0.8f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = Player.direction * 10;
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(0.3f)
            };

            SpurtHelper_GeneralMoreSpurt combo3_2 = new(this, new()
            {
                OnHit = SeizeStarPower,
                UseTimeMax = () => (int)(SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem)),
                SpurtLength = () => 250,
                DmgConut = 20,
                angleFix = 1f,
                rot = 0f,
                rotDiff = 0f,
                centerOffset = 60,
                ModifyHit = (NPC _, ref NPC.HitModifiers modifiers) => modifiers.SourceDamage *= 0.3f,
                ActivationCondition = () => IsMouseLeftChick,
            }, spurtHelper, Player);

            SpurtHelper_GeneralOneSpurt combo4 = new(this, new()
            {
                OnHit = SeizeStarPower,
                UseTimeMax = () => (int)(SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem)),
                SpurtLength = () => 250,
                ModifyHit = (NPC _, ref NPC.HitModifiers modifiers) => modifiers.SourceDamage += 1f,
                ActivationCondition = () => IsMouseLeftChick && StopTime > 3,
            }, spurtHelper, Player)
            {
                OnUse = (skill) =>
                {
                    if (Projectile.ai[1] < 30)
                    {
                        skill.SpurtHelper.Time = 0;
                        Projectile.ai[0] = -1;
                        Projectile.ai[1]++;
                        Player.velocity= Projectile.velocity.SafeNormalize(default) * 17;
                        StarPiecredExtra98 starPiecredExtra98 = new(Projectile.velocity, Player.Center);
                        // 添加
                        ParticlesSystem.AddParticle(BasicParticle.DrawLayer.AfterDust,starPiecredExtra98);
                    }
                }
            };

            SpurtHelper_GeneralOneSpurt Yiya = new(this, new()
            {
                OnHit = SeizeStarPower,
                UseTimeMax = () => (int)(SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem)),
                SpurtLength = () => 250,
                ModifyHit = (NPC _, ref NPC.HitModifiers modifiers) => modifiers.SourceDamage += 1f,
                ActivationCondition = () => IsMouseLeftChick && ControlDir_MoveFaceDir(),
            }, spurtHelper, Player)
            {
                OnUse = (skill) =>
                {
                    if (Projectile.ai[1] < 30)
                    {
                        skill.SpurtHelper.Time = 0;
                        Projectile.ai[0] = -1;
                        Projectile.ai[1]++;
                        Player.velocity = Projectile.velocity.SafeNormalize(default) * 17;
                        StarPiecredExtra98 starPiecredExtra98 = new(Projectile.velocity, Player.Center);
                        // 添加
                        ParticlesSystem.AddParticle(BasicParticle.DrawLayer.AfterDust, starPiecredExtra98);
                    }
                }
            };

            BasicSkillProj.Register(combo1_1, combo1_2, combo2_1, combo2_2, combo2_3, noUse_Held, noUse_Carry,combo3_1, combo3_2, combo4, Yiya);

            Yiya.AddBySkill(combo1_1, combo1_2, combo2_1, combo2_2, combo2_3, noUse_Held, noUse_Carry, combo3_1, combo3_2, combo4);

            combo2_2.AddSkill(combo4);
            combo2_1.AddSkill(combo3_1).AddSkill(combo3_2);
            combo1_1.AddSkill(combo2_1).AddSkill(combo2_2).AddSkill(combo2_3);
            combo1_1.AddSkill(combo1_2);
            noUse_Carry.AddSkill(noUse_Held).AddSkill(combo1_1);

            noUse_Carry.OnSkillActive();
            CurrentSkill = noUse_Carry;
        }
        public override Color? GetAlpha(Color lightColor) => Color.Purple;
        public static float TimeChange(float time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f));
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
            SwingHelper swingHelper = SwingHelper;
            swingHelper.Swing_TrailingDraw(ModAsset.Extra_4.Value, (_) => Color.White, effect);
            //projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            //model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            //effect.Parameters["uTransform"].SetValue(model * projection);
            swingHelper.Swing_Draw_ItemAndTrailling(drawColor, ModAsset.Extra_0.Value, (_) => Color.White, effect);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            StarPiercedItem.DamageAdd += 0.2f;
            if (target.TryGetGlobalNPC(out StarPowerNPCs npc))
                npc.starPower.Value += SpawnItem.damage;
            if (target.ModNPC is StarOwnerNPC starOwnerNPC)
                starOwnerNPC.starPower.Value += SpawnItem.damage;
        }
        public void SeizeStarPower(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.TryGetGlobalNPC(out StarPowerNPCs npc))
            {
                Player.GetModPlayer<StarPowerPlayer>().starPower.Value += npc.starPower.Value;
                npc.starPower.Value = 0;
            }
            if (target.ModNPC is StarOwnerNPC starOwnerNPC)
            {
                Player.GetModPlayer<StarPowerPlayer>().starPower.Value += starOwnerNPC.starPower.Value;
                starOwnerNPC.starPower.Value = 0;
            }
        }

        public override bool PreSkillTimeOut()
        {
            CurrentSkill.OnSkillDeactivate();
            if (CurrentSkill != noUse_Held && CurrentSkill != noUse_Carry)
            {
                noUse_Held.OnSkillActive();
                CurrentSkill = noUse_Held;
            }
            else if (CurrentSkill == noUse_Held)
            {
                noUse_Carry.OnSkillActive();
                CurrentSkill = noUse_Carry;
            }
            OldSkills.Clear();
            return false;
        }
    }
}
