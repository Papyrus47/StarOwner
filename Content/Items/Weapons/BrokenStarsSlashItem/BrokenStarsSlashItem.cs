using StarOwner.Content.NPCs;
using StarOwner.Core.GlobalNPCs;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillProjs;
using StarOwner.Core.SkillProjs.GeneralSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.Items.Weapons.BrokenStarsSlashItem
{
    public class BrokenStarsSlashItem : ModItem,IDamageAdd
    {
        public int ChangeTime;
        public float DamageAdd { get; set; }
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.width = 76;
            Item.height = 74;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(1);
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<BrokenStatsSlashPlayerProj>();
        }
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            if(ChangeTime > 0)
                ChangeTime--;
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item),player.Center, player.velocity, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
                proj.originalDamage = proj.damage;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
    public class BrokenStatsSlashPlayerProj : BasicMeleeWeaponSword
    {
        public override string Texture => ModAsset.BrokenStarsSlashItem_Mod;
        public BrokenStarsSlashItem BrokenStarsSlashItem => SpawnItem.ModItem as BrokenStarsSlashItem;
        public override bool IsItemHit => false;
        public SwingHelper_NoUse_Held noUse_Held;
        public SwingHelper_NoUse_Carry noUse_Carry;
        public SwingHelper ShowSwingHelper;
        public int[] HitCount = new int[Main.npc.Length];
        public int MaxHitCount = 5;
        public override void NewSwingHelper()
        {
            base.NewSwingHelper();
            SwingHelper.DrawTrailCount = 1;
            ShowSwingHelper = new(Projectile, 25, ModAsset.BrokenStarsSlashChange_Async);
        }
        public override void AI()
        {
            base.AI();
            if(CurrentSkill.CanDamage() != true)
            {
                for(int i = 0; i < HitCount.Length; i++)
                {
                    HitCount[i] = 0;
                }
                MaxHitCount = 5;
            }
        }
        public override void Init()
        {
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

            SwingHelper_GeneralSwing combo1_1 = new(this, new()
            {
                ActionDmg = 1f,
                ChangeCondition = () => IsMouseLeftChick,
                OnHitStopTime = 4,
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
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing combo1_2 = new(this, new()
            {
                ActionDmg = 1.2f,
                ChangeCondition = () => IsMouseLeftChick,
                OnHitStopTime = 4,
                StartVel = Vector2.UnitY.RotatedBy(0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.2f) * 1.2f,
                VisualRotation = -0.8f,
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
                TimeChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f))
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing combo1_3 = new(this, new()
            {
                ActionDmg = 2f,
                ChangeCondition = () => IsMouseLeftChick,
                OnHitStopTime = 4,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi,
                VelScale = new Vector2(1, 1f) * 1.1f,
                VisualRotation = 0,
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
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };

            SwingHelper_GeneralSwing combo2_1 = new(this, new()
            {
                ActionDmg = 3f,
                ChangeCondition = () => IsMouseLeftChick && StopTime > 10,
                OnHitStopTime = 4,
                StartVel = Vector2.UnitY.RotatedBy(0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f),
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
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                    Player.velocity.X = Player.direction * 15;
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange,
                OnChange = (_) =>
                {
                    Player.velocity.X = 0;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing combo2_2 = new(this, new()
            {
                ActionDmg = 7f,
                ChangeCondition = () => IsMouseLeftChick,
                OnHitStopTime = 4,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f) * 2,
                VisualRotation = 0,
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
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };

            SwingHelper_GeneralSwing combo3 = new(this, new()
            {
                ActionDmg = 1.2f,
                ChangeCondition = () => IsMouseLeftChick && StopTime > 10,
                OnHitStopTime = 4,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.2f) * 2f,
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
            #region 十字斩
            SwingHelper_GeneralSwing TenCharSlash_1 = new(this, new()
            {
                ActionDmg = 10f,
                ChangeCondition = () => IsMouseLeftChick && ControlDir_MoveFaceDir(),
                OnHitStopTime = 6,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f) * 1.1f,
                VisualRotation = 0,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 15,
                OnUse =(_)=>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = -Player.direction * 15;
                },
                OnChange = (_) =>
                {
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange,
                OnUse = (_) =>
                {
                    Player.velocity.X = Player.direction * 17;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing TenCharSlash_2 = new(this, new()
            {
                ActionDmg = 5f,
                ChangeCondition = () => true,
                OnHitStopTime = 6,
                StartVel = Vector2.UnitX.RotatedBy(0.4),
                SwingRot = MathHelper.TwoPi,
                VelScale = new Vector2(1, 0.2f) * 1.1f,
                VisualRotation = -0.8f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 15,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = Player.direction * 5;
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
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            #endregion
            #region 格挡反击
            SwingHelper_Defence DefAttack = new(this, new()
            {
                ActionDmg = 10f,
                ChangeCondition = () => IsMouseRightChick,
                OnHitStopTime = 1,
                StartVel = -Vector2.UnitY,
                SwingRot = 0,
                VelScale = new Vector2(1, 1f) * 4f,
                VisualRotation = -0.8f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 1,
                OnUse = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = 0;
                },
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = 0;
                    //SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 4,
                PostAtkTime = 0
            }, new()
            {
                SwingTime =120,
                TimeChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f)),
                OnUse = (_) =>
                {
                    Player.velocity.X = 0;
                    DefAttackAndDodgePlayer defAttackAndDodgePlayer = Player.GetModPlayer<DefAttackAndDodgePlayer>();
                    defAttackAndDodgePlayer.InWeaponDef = 2;
                    if(defAttackAndDodgePlayer.WeaponDefSucceed > 0)
                    {
                        Projectile.ai[0] = 2;
                        Projectile.ai[1] = 0;
                    }
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };

            SwingHelper_GeneralSwing DefSlash = new(this, new()
            {
                ActionDmg = 10f,
                ChangeCondition = () => Player.GetModPlayer<DefAttackAndDodgePlayer>().WeaponDefSucceed > 0,
                OnHitStopTime = 7,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.2f) * 4f,
                VisualRotation = -0.8f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 5,
                OnUse = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = -Player.direction * 30;
                },
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = Player.direction * 15;
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = (time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f)),
                OnChange = (_) =>
                {
                    Player.velocity.X = 0;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            #endregion
            #region 上捞
            SwingHelper_GeneralSwing SlashUp = new(this, new()
            {
                ActionDmg = 3f,
                ChangeCondition = () => IsMouseLeftChick && ControlDir_MoveUnFaceDir(),
                OnHitStopTime = 1,
                StartVel = Vector2.UnitY.RotatedBy(0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f),
                VisualRotation = -0.2f,
                SwingLenght = length,
                SwingDirectionChange = false,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 9,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    //SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                    if(Main.mouseLeft)
                        Player.velocity.Y -= 15;
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange,
                OnChange = (_) =>
                {
                    Player.velocity.X = 0;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };

            SwingHelper_GeneralSwing FallStar = new(this, new()
            {
                ActionDmg = 20f,
                ChangeCondition = () => IsMouseLeftChick && Player.velocity.Y != 0,
                OnHitStopTime = 10,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi,
                VelScale = new Vector2(1, 1f) * 4f,
                VisualRotation = -0.2f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 9,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    //SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                    Player.velocity.Y = 15;
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange,
                OnChange = (_) =>
                {
                    Player.velocity.X = 0;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            #endregion
            #region 变形后
            #region 变形
            SwingHelper_GeneralSwing Change = new(this, new()
            {
                ActionDmg = 1.2f,
                ChangeCondition = () => IsMouseLeftChanning && BrokenStarsSlashItem.ChangeTime <= 0,
                OnHitStopTime = 4,
                StartVel = Vector2.UnitY,
                SwingRot = MathHelper.Pi,
                VelScale = new Vector2(1, 1f) * 2f,
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
                    //SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                    BrokenStarsSlashItem.ChangeTime = Player.statMana * 5;
                    Player.statMana = 0;
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
            #endregion
            #region 十字斩·改
            SwingHelper_GeneralSwing TenCharSlash_Change_1 = new(this, new()
            {
                ActionDmg = 7f,
                ChangeCondition = () => IsMouseLeftChick && ControlDir_MoveFaceDir() && BrokenStarsSlashItem.ChangeTime > 0,
                OnHitStopTime = 6,
                StartVel = Vector2.UnitY.RotatedBy(0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 1f) * 1.1f,
                VisualRotation = 0,
                SwingLenght = length,
                SwingDirectionChange = false,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 15,
                OnUse = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = -Player.direction * 5;
                },
                OnChange = (_) =>
                {
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange,
                OnUse = (_) =>
                {
                    Player.velocity.X = Player.direction * 30;
                    MaxHitCount = 10;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            SwingHelper_GeneralSwing TenCharSlash_Change_2 = new(this, new()
            {
                ActionDmg = 7f,
                ChangeCondition = () => true,
                OnHitStopTime = 6,
                StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                SwingRot = MathHelper.Pi + 0.8f,
                VelScale = new Vector2(1, 0.2f) * 1.1f,
                VisualRotation = -0.8f,
                SwingLenght = length,
                SwingDirectionChange = true,
                preDraw = DrawSword
            }, new()
            {
                PreTime = 15,
                OnChange = (_) =>
                {
                    Player.ChangeDir(((Main.MouseWorld - Player.Center).X > 0).ToDirectionInt());
                    Player.velocity.X = Player.direction * 30;
                    SwingHelper.SetRotVel(Player.direction == 1 ? (Main.MouseWorld - Player.Center).ToRotation() : -(Player.Center - Main.MouseWorld).ToRotation());
                }
            }, new()
            {
                PostMaxTime = 30,
                PostAtkTime = 10
            }, new()
            {
                SwingTime = SpawnItem.useAnimation * Player.GetWeaponAttackSpeed(SpawnItem),
                TimeChange = TimeChange,
                OnUse = (_) =>
                {
                    MaxHitCount = 10;
                }
            }, SwingHelper, Player)
            {
                playSound = SoundID.DD2_MonkStaffSwing.WithPitchOffset(-0.3f)
            };
            #endregion
            #endregion
            BasicSkillProj.Register(combo1_1, combo1_2, combo1_3, noUse_Held, noUse_Carry, combo2_1, combo2_2, combo3,TenCharSlash_1, TenCharSlash_2,DefAttack,DefSlash,SlashUp,FallStar,Change,TenCharSlash_Change_1,TenCharSlash_Change_2);

            TenCharSlash_Change_1.AddSkill(TenCharSlash_Change_2);
            TenCharSlash_Change_1.AddBySkill(combo1_1, combo1_2, combo1_3, noUse_Held, noUse_Carry, combo2_1, combo2_2, combo3);
            noUse_Held.AddSkill(Change);

            FallStar.AddBySkill(combo1_1, combo1_2, combo1_3, noUse_Held, noUse_Carry, combo2_1, combo2_2, combo3);
            SlashUp.AddBySkill(combo1_1, combo1_2, combo1_3, noUse_Held, noUse_Carry, combo2_1, combo2_2, combo3);

            DefAttack.AddSkill(DefSlash);
            DefAttack.AddBySkill(combo1_1, combo1_2, combo1_3, noUse_Held, noUse_Carry, combo2_1, combo2_2, combo3);

            TenCharSlash_1.AddSkill(TenCharSlash_2);
            TenCharSlash_1.AddBySkill(combo1_1, combo1_2, combo1_3, noUse_Held, noUse_Carry, combo2_1, combo2_2, combo3);

            combo1_2.AddSkill(combo3);
            combo1_1.AddSkill(combo2_1).AddSkill(combo2_2);
            combo1_1.AddSkill(combo1_2).AddSkill(combo1_3);
            noUse_Carry.AddSkill(noUse_Held).AddSkill(combo1_1);

            noUse_Carry.OnSkillActive();
            CurrentSkill = noUse_Carry;
        }
        public static float TimeChange(float time) => MathHelper.SmoothStep(0, 1f, MathF.Pow(time, 2.5f));
        private bool DrawSword(SpriteBatch spriteBatch, Color drawColor)
        {
            if (BrokenStarsSlashItem == null)
                return false;
            Effect effect = ModAsset.StarOwnerSwingShader.Value;
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            gd.Textures[1] = ModAsset.ColorMap_0.Value;
            gd.Textures[2] = ModAsset.Stars.Value;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix scale = Matrix.CreateScale(Main.GameViewMatrix.Zoom.Length() / MathF.Sqrt(2));
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            effect.Parameters["uTransform"].SetValue(model * projection * scale);
            SwingHelper swingHelper = SwingHelper;
            if (BrokenStarsSlashItem.ChangeTime > 0 && swingHelper != ShowSwingHelper)
            {
                gd.Textures[1] = ModAsset.ColorMap_2.Value;
                swingHelper = ShowSwingHelper;
                swingHelper.oldVels = SwingHelper.oldVels.Clone() as Vector2[];
                swingHelper.Center = SwingHelper.Center;
                swingHelper.velocity = SwingHelper.velocity;
                swingHelper.VisualRotation = SwingHelper.VisualRotation;
                swingHelper._velRotBy = SwingHelper._velRotBy;
            }
            swingHelper.Swing_TrailingDraw(ModAsset.Extra_4.Value, (_) => Color.White, effect);
            //projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            //model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            //effect.Parameters["uTransform"].SetValue(model * projection);
            swingHelper.Swing_Draw_ItemAndTrailling(drawColor, ModAsset.Extra_0.Value, (_) => Color.White, effect);
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.SourceDamage += BrokenStarsSlashItem.DamageAdd;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            BrokenStarsSlashItem.DamageAdd += 0.2f;
            if (target.TryGetGlobalNPC(out StarPowerNPCs npc))
                npc.starPower.Value += SpawnItem.damage;
            if(target.ModNPC is StarOwnerNPC starOwnerNPC)
                starOwnerNPC.starPower.Value += SpawnItem.damage;
            if(BrokenStarsSlashItem.ChangeTime > 0 && HitCount[target.whoAmI] < MaxHitCount)
            {
                HitCount[target.whoAmI]++;
                Projectile.localNPCImmunity[target.whoAmI] = 0;
            }
        }
        public override bool PreSkillTimeOut()
        {
            CurrentSkill.OnSkillDeactivate();
            if (CurrentSkill != noUse_Held && CurrentSkill != noUse_Carry)
            {
                noUse_Held.OnSkillActive();
                noUse_Held.SwingHelper = SwingHelper;
                if (BrokenStarsSlashItem.ChangeTime > 0)
                    noUse_Held.SwingHelper = ShowSwingHelper;
                CurrentSkill = noUse_Held;
            }
            else if(CurrentSkill == noUse_Held)
            {
                noUse_Carry.OnSkillActive();
                noUse_Carry.SwingHelper = SwingHelper;
                if (BrokenStarsSlashItem.ChangeTime > 0)
                    noUse_Carry.SwingHelper = ShowSwingHelper;
                CurrentSkill = noUse_Carry;
            }
            OldSkills.Clear();
            return false;
        }
    }
}
