using Microsoft.Xna.Framework.Graphics;
using StarOwner.Content.NPCs.Mode;
using StarOwner.Content.NPCs.Particles;
using StarOwner.Content.NPCs.Skills.General;
using StarOwner.Content.NPCs.Skills.General.BrokenStarsSlash;
using StarOwner.Content.NPCs.Skills.General.BrokenStarStick;
using StarOwner.Content.NPCs.Skills.General.Guns;
using StarOwner.Content.NPCs.Skills.General.SkinningAndBrokenBones.Change;
using StarOwner.Content.NPCs.Skills.General.SkinningAndBrokenBones.NoChange;
using StarOwner.Content.NPCs.Skills.General.StarPierced;
using StarOwner.Content.NPCs.Skills.Phase1;
using StarOwner.Core.ModPlayers;
using StarOwner.Core.SkillsNPC;
using StarOwner.Core.SPBuffs;
using StarOwner.Core.SwingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StarOwner.Content.NPCs.Skills.BasicSwingSkill;

namespace StarOwner.Content.NPCs
{
    public class StarOwnerNPC : BasicSkillNPC
    {
        public const string Phase1Text = "Mods.StarOwner.Bosses.StarOwner.Phase1Text";
        public Player drawPlayer;
        public Player TargetPlayer
        {
            get
            {
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                    NPC.TargetClosest();
                return Main.player[NPC.target];
            }
        }
        /// <summary>
        /// 伤害计量器
        /// </summary>
        public float DamageAdd;
        /// <summary>
        /// 技能随机数最大值
        /// </summary>
        public int RandomSkillMax;
        /// <summary>
        /// 斧强化
        /// </summary>
        public bool InStrongAxeMode;
        public static float ClosePlayer => 15 * 16;
        public static float FarPlayer => 30 * 16;
        public Walk1 walk1;
        /// <summary>
        /// 被防御住的攻击
        /// </summary>
        public int byDefAtk;
        public StarPower starPower = new();
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void Load()
        {
            //TheUtility.RegisterText(Phase1Text);
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
            NPC.width = 30;
            NPC.height = 44;
            NPC.boss = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000;
            NPC.value = Item.buyPrice(80);
            NPC.defense = 99999;
            NPC.knockBackResist = 0;
        }
        public override bool CheckDead()
        {
            if(starPower.Value > 0)
            {
                starPower.Value--;
                NPC.life += 350;
                NPC.HealEffect(350);
                NPC.active = true;
                return false;
            }
            return base.CheckDead();
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (starPower.Value == 0)
                return true;
            position.X -= 16;
            int hb1Width = TextureAssets.Hb1.Width();
            int width = (int)((float)starPower.Value / (starPower.ValueMax / 10) * hb1Width);
            Main.spriteBatch.Draw(TextureAssets.Hb2.Value, position + new Vector2(0, 10) - Main.screenPosition, new Rectangle(0, 0, TextureAssets.Hb2.Width(),
                TextureAssets.Hb2.Height()), Color.Lerp(Color.Purple, Color.Purple * 2, width / hb1Width / 10f), 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);

            if (starPower.Value == starPower.ValueMax)
                width = hb1Width * 10 - 1;
            Main.spriteBatch.Draw(TextureAssets.Hb1.Value, position + new Vector2(0, 10) - Main.screenPosition, new Rectangle(2, 0, width % hb1Width,
                TextureAssets.Hb1.Height()), Color.Lerp(Color.Red, Color.DeepPink, width / hb1Width / 10f), 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
            position.X += 16;
            return true;
        }
        public override void AI()
        {
            if(NPC.lifeMax > 1000)
            {
                NPC.life = NPC.lifeMax = 1000;
            }
            NPC.defense = NPC.defDefense; // 防星击
            starPower.ValueMax = (int)(NPC.lifeMax * 2.2f);
            if(starPower.starPowerResetTime > 0)
                starPower.starPowerResetTime--;
            
            if(starPower.Value > starPower.ValueMax)
                starPower.Value = starPower.ValueMax;

            if(NPC.life < NPC.lifeMax - 350 && starPower.Value > 0)
            {
                if (starPower.Value >= starPower.ValueMax)
                    starPower.starPowerResetTime = 180;
                starPower.Value--;
                NPC.life += 350;
                NPC.HealEffect(350);
            }
            base.AI();
        }
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            base.ModifyIncomingHit(ref modifiers);
            if (DamageAdd > 0)
                DamageAdd -= 0.1f;
            //modifiers.SetMaxDamage(20);
        }
        public override bool CheckActive() => false;
        public override void Init()
        {
            OldSkills = new();
            drawPlayer = new Player();
            #region 注册
            SO_Phase1 phase1 = new(NPC);

            #region 一阶段
            Phase1Start phase1Start = new(NPC);
            walk1 = new(NPC);

            NoAtk noAtk_1 = new(NPC,5);

            RandomSkillMax = 6;
            float BS_Length = new Vector2(76, 74).Length();
            float SP_Length = new Vector2(110).Length();
            float BSS_Length = new Vector2(230).Length();
            float Skinning_Length = new Vector2(54, 42).Length();
            float BB_Length = new Vector2(74, 68).Length();
            float BBC_Length = new Vector2(86,84).Length();
            Skills.BasicSwingSkill.PreSwing LongPreTime = new()
            {
                PreSwingTime = 15
            };
            Skills.BasicSwingSkill.PreSwing ShortPreTime = new()
            {
                PreSwingTime = 7
            };
            #region 破星之斩

            BrokenStarsSlashSwing BS_combo1_1 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.8f),
                    VisualRotation = -0.2f,
                    OnHitStopTime = 5,
                    ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BS_Length
                });
            BrokenStarsSlashSwing BS_combo1_2 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 1.2f,
                    StartVel = Vector2.UnitY.RotatedBy(0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.2f) * 1.2f,
                    VisualRotation = -0.8f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = false,
                    ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BS_Length
                });
            BrokenStarsSlashSwing BS_combo1_3 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f) * 1.1f,
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BS_Length
                });
            BrokenStarsSlashSwing BS_combo2_1 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    OnUse = (_) =>
                    {
                        NPC.velocity.X = NPC.spriteDirection * 10;
                    },
                    OnChange = (_) =>
                    {
                        NPC.velocity.X *= 0.1f;
                    },
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 3f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < ClosePlayer * 2 && Main.rand.NextBool(2) && NPC.collideY,
                    SwingLenght = BS_Length
                });
            BrokenStarsSlashSwing BS_combo2_2 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 7f,
                    StartVel = Vector2.UnitY.RotatedBy(0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.6f) * 2f,
                    VisualRotation = -0.4f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = false,
                    ChangeCondition = () => DisTarget() < ClosePlayer * 2 && NPC.collideY,
                    SwingLenght = BS_Length
                });
            BrokenStarsSlashSwing BS_combo3 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 5f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.6f) * 2f,
                    VisualRotation = -0.4f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < ClosePlayer * 2 && Main.rand.NextBool(2) && NPC.collideY,
                    SwingLenght = BS_Length
                });
            #region 十字斩
            BrokenStarsSlashSwing BS_TenCharSlash_1 = new(NPC,
               new()
               {
                   PreSwingTime = 20,
                   OnUse = (_) =>
                   {
                       if (NPC.ai[1] < 10)
                       {
                           NPC.velocity.X = -Math.Sign((TargetPlayer.Center - NPC.Center).X) * 10;
                           NPC.spriteDirection = NPC.direction = Math.Sign((TargetPlayer.Center - NPC.Center).X);
                       }
                       else
                       {
                           NPC.velocity *= 0.5f;
                       }
                   },
                   OnChange = (_)=>
                   {
                       NPC.velocity.X = Math.Sign((TargetPlayer.Center - NPC.Center).X) * 20;
                       NPC.spriteDirection = NPC.direction = Math.Sign((TargetPlayer.Center - NPC.Center).X);
                   }
               },
               new()
               {
                   SwingTime = 15,
                   SwingTimeChange = GeneralSwing,
                   OnChange =(_)=>
                   {
                       NPC.velocity.X *= 0.02f;
                   }
               }, new()
               {
                   PostSwingTime = 10,
                   PostSwingTimeMax = 60
               }, new()
               {
                   ActionDmg = 11f,
                   StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                   SwingRot = MathHelper.Pi + 0.8f,
                   VelScale = new Vector2(1, 1f) * 1.5f,
                   VisualRotation = 0f,
                   OnHitStopTime = 5,
                   SwingDirectionChange = true,
                   ChangeCondition = () => DisTarget() > ClosePlayer && DisTarget() < FarPlayer && Main.rand.NextBool(5) && NPC.collideY,
                   SwingLenght = BS_Length
               });
            BrokenStarsSlashSwing BS_TenCharSlash_2 = new(NPC,
               LongPreTime,
               new()
               {
                   SwingTime = 15,
                   SwingTimeChange = GeneralSwing
               }, new()
               {
                   PostSwingTime = 10,
                   PostSwingTimeMax = 60
               }, new()
               {
                   ActionDmg = 1.2f,
                   StartVel = Vector2.UnitY.RotatedBy(0.4),
                   SwingRot = MathHelper.Pi + 0.8f,
                   VelScale = new Vector2(1, 0.2f) * 1.5f,
                   VisualRotation = -0.8f,
                   OnHitStopTime = 5,
                   SwingDirectionChange = false,
                   ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                   SwingLenght = BS_Length
               });
            #endregion
            #region 上捞
            BrokenStarsSlashSwing BS_SlashUp = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    OnUse = (_) =>
                    {
                        NPC.velocity.Y = -8;
                    },
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 3f,
                    StartVel = Vector2.UnitY.RotatedBy(0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = false,
                    ChangeCondition = () => DisTarget() < ClosePlayer && Main.rand.NextBool(18) && NPC.collideY,
                    SwingLenght = BS_Length
                });
            #endregion
            #region 坠星
            BrokenStarsSlashSwing BS_StarFall = new(NPC,
                 new()
                 {
                     PreSwingTime = 50,
                     OnUse = (_) =>
                     {
                         NPC.velocity.Y *= 0.1f;
                     }
                 },
                new()
                {
                    SwingTime = 15,
                    OnUse = (_) =>
                    {
                        NPC.velocity.Y = 30;
                        if(!NPC.collideY)
                            NPC.noGravity = true;
                    },
                    OnChange= (_) =>
                    {
                        NPC.noGravity = false;
                    },
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 20f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi - 0.4f,
                    VelScale = new Vector2(1, 1f) * 5,
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => !NPC.collideY,
                    SwingLenght = BS_Length
                })
            {
                CanDef = false
            };
            #endregion
            #region 格挡反击
            BrokenStarsSlashDef BS_Def = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 1f,
                    StartVel = Vector2.UnitY,
                    SwingRot = 0,
                    VelScale = Vector2.One,
                    VisualRotation = -0.8f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => Main.rand.NextBool(4) && NPC.collideY,
                    SwingLenght = BS_Length
                });
            BrokenStarsSlashSwing BS_DefSlash = new(NPC,
                   LongPreTime,
                   new()
                   {
                       SwingTime = 15,
                       SwingTimeChange = GeneralSwing,
                       OnUse = (_) =>
                       {
                           NPC.velocity.X = (TargetPlayer.Center.X - NPC.Center.X) > 0 ? 10 : -10;
                           NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
                       },
                       OnChange = (_) =>
                       {
                           NPC.velocity.X *= 0.1f;
                       },
                   }, new()
                   {
                       PostSwingTime = 10,
                       PostSwingTimeMax = 60
                   }, new()
                   {
                       ActionDmg = 10f,
                       StartVel = Vector2.UnitY.RotatedBy(0.4),
                       SwingRot = MathHelper.Pi + 0.8f,
                       VelScale = new Vector2(1, 0.2f) * 4f,
                       VisualRotation = -0.8f,
                       OnHitStopTime = 5,
                       SwingDirectionChange = false,
                       ChangeCondition = () => true,
                       SwingLenght = BS_Length
                   });
            #endregion
            #endregion
            #region 繁星刺破
            StarPiercedSpurt SP_combo1_1 = new(NPC, () => Vector2.UnitX * NPC.spriteDirection * SP_Length, () => DisTarget() < ClosePlayer && Main.rand.NextBool(RandomSkillMax))
            {
                ChangeTime = 30,
                ActionDmg = 0.5f,
            };
            StarPiercedSpurt SP_combo1_2 = new(NPC, () => (TargetPlayer.Center - NPC.Center).SafeNormalize(default) * SP_Length, () => DisTarget() < ClosePlayer)
            {
                ChangeTime = 90
            };

            StarPiercedSwing SP_combo2_1 = new(NPC,
                ShortPreTime,
                new()
                {
                    SwingTime = 20,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 5,
                    PostSwingTimeMax = 60
                }, new()
                {
                    StartVel = Vector2.UnitY.RotatedBy(0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    SwingDirectionChange = false,
                    ChangeCondition = () => DisTarget() < ClosePlayer && Main.rand.NextBool(2) && NPC.collideY,
                    SwingLenght = SP_Length
                });
            StarPiercedSwing SP_combo2_2 = new(NPC,
                ShortPreTime,
                new()
                {
                    SwingTime = 20,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 5,
                    PostSwingTimeMax = 60
                }, new()
                {
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = SP_Length
                });
            StarPiercedSpurt SP_combo2_3 = new(NPC, () => (TargetPlayer.Center - NPC.Center).SafeNormalize(default) * SP_Length, () => DisTarget() < ClosePlayer)
            {
                ChangeTime = 5
            };

            StarPiercedSwing SP_combo3_1 = new(NPC,
                ShortPreTime,
                new()
                {
                    SwingTime = 20,
                    SwingTimeChange = GeneralSwing,
                    OnUse = (_) =>
                    {
                        NPC.velocity.X = NPC.spriteDirection * 15;
                    },
                    OnChange = (_) =>
                    {
                        NPC.velocity.X *= 0.01f;
                    }
                }, new()
                {
                    PostSwingTime = 25,
                    PostSwingTimeMax = 60
                }, new()
                {
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.2f),
                    VisualRotation = -0.8f,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < FarPlayer && Main.rand.NextBool(2) && NPC.collideY,
                    SwingLenght = SP_Length
                });
            StarPiercedCoiledSpurt SP_combo3_2 = new(NPC, () => Vector2.UnitX * NPC.spriteDirection * SP_Length * 0.25f, () => DisTarget() < ClosePlayer,20)
            {
                ChangeTime = 15,
                ActionDmg = 0.3f,
            };
            StarPiercedSpurt SP_combo4 = new(NPC, () => Vector2.UnitX * NPC.spriteDirection * SP_Length * 0.25f, () => DisTarget() < FarPlayer && Main.rand.NextBool(4))
            {
                ChangeTime = 15,
                ActionDmg = 4f,
                OnUse = (skill) =>
                {
                    if(skill.ByDef && (int)NPC.ai[2] == 0)
                    {
                        NPC.ai[2]++;
                        NPC.velocity.X = -NPC.spriteDirection;
                    }
                    if((int)NPC.ai[0] == skill.ChangeTime + 2 && NPC.ai[1] < 30)
                    {
                        NPC.velocity.X = NPC.spriteDirection * 25;
                        NPC.ai[1]++;
                        NPC.ai[0]--;
                        for (int i = 0; i < 110; i++)
                        {
                            Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.PinkFairy, skill.vel.SafeNormalize(default) * i * 0.25f);
                            dust.noGravity = true;
                        }
                    }
                    else
                    {
                        NPC.velocity.X = NPC.spriteDirection * 4;
                    }
                }
            };
            #region 咿呀剑法
            StarPiercedSpurt SP_YiyaDash = new(NPC, () => Vector2.UnitX * NPC.spriteDirection * SP_Length * 0.25f, () => DisTarget() < FarPlayer && Main.rand.NextBool(8))
            {
                ChangeTime = 15,
                ActionDmg = 4f,
                OnUse = (skill) =>
                {
                    if (skill.ByDef && (int)NPC.ai[2] == 0)
                    {
                        NPC.ai[2]++;
                        NPC.velocity.X = -NPC.spriteDirection;
                    }
                    if ((int)NPC.ai[0] == skill.ChangeTime + 2 && NPC.ai[1] < 30)
                    {
                        NPC.velocity.X = NPC.spriteDirection * 45;
                        NPC.velocity.Y = -0.001f;
                        NPC.ai[1]++;
                        NPC.ai[0]--;
                        //StarPiecredExtra98 starPiecredExtra98 = new(NPC.velocity, NPC.Center + NPC.velocity);
                        //Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, starPiecredExtra98);
                    }
                    else
                    {
                        NPC.velocity.X = NPC.spriteDirection * 4;
                    }
                }
            };
            #endregion
            #endregion
            #region 癫疯与恐惧
            MadnessAndFearNormalShoot madnessAndFearNormalShoot = new(NPC,() => DisTarget() > ClosePlayer && Main.rand.NextBool(RandomSkillMax * 5));
            MadnessAndFearFullShot madnessAndFearFullShot = new(NPC, () => DisTarget() > FarPlayer);
            #endregion
            #region 断星棍
            BrokenStarStickSwing BSS_combo1_1 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 5,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 1f,
                    StartVel = Vector2.UnitX.RotatedBy(0.2),
                    SwingRot = 0.2f,
                    VelScale = new Vector2(1, 0.8f),
                    VisualRotation = 0,
                    SwingDirectionChange = false,
                    OnHitStopTime = 5,
                    ChangeCondition = () => InMiddleDis() && Main.rand.NextBool(RandomSkillMax) && NPC.collideY,
                    SwingLenght = BSS_Length
                });
            BrokenStarStickSwing BSS_combo1_2 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 10,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = Vector2.UnitY,
                    SwingRot = MathHelper.PiOver2 + 0.5f,
                    VelScale = new Vector2(1, 0.4f),
                    VisualRotation = 0,
                    SwingDirectionChange = false,
                    OnHitStopTime = 5,
                    ChangeCondition = () => InMiddleDis() && NPC.collideY,
                    SwingLenght = BSS_Length
                });
            BrokenStarStickSwing BSS_combo1_3 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 10,
                    SwingTimeChange = GeneralSwing
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 4f,
                    StartVel = -Vector2.UnitY,
                    SwingRot = MathHelper.PiOver2 + 0.5f,
                    VelScale = new Vector2(1, 0.9f),
                    VisualRotation = 0,
                    SwingDirectionChange = true,
                    OnHitStopTime = 5,
                    ChangeCondition = () => InMiddleDis() && NPC.collideY,
                    SwingLenght = BSS_Length
                });
            BrokenStarStickSwing BSS_combo1_4 = new(NPC,
                    LongPreTime,
                    new()
                    {
                        SwingTime = 15,
                        SwingTimeChange = GeneralSwing
                    }, new()
                    {
                        PostSwingTime = 10,
                        PostSwingTimeMax = 60
                    }, new()
                    {
                        ActionDmg = 5f,
                        StartVel = Vector2.UnitX.RotatedBy(0.4),
                        SwingRot = MathHelper.Pi + 0.8f,
                        VelScale = new Vector2(1, 0.9f),
                        VisualRotation = 0,
                        SwingDirectionChange = false,
                        OnHitStopTime = 5,
                        ChangeCondition = () => InMiddleDis() && NPC.collideY,
                        SwingLenght = BSS_Length
                    });
            BrokenStarStickSwing BSS_combo1_5 = new(NPC,
                    LongPreTime,
                    new()
                    {
                        SwingTime = 20,
                        SwingTimeChange = GeneralSwing,
                        OnUse = (_) =>
                        {
                            NPC.velocity.X = NPC.spriteDirection * 10;
                        },
                        OnChange = (_) =>
                        {
                            NPC.velocity.X *= 0.1f;
                        },
                    }, new()
                    {
                        PostSwingTime = 10,
                        PostSwingTimeMax = 60
                    }, new()
                    {
                        ActionDmg = 5f,
                        StartVel = -Vector2.UnitX,
                        SwingRot = MathHelper.Pi + 0.4f,
                        VelScale = new Vector2(1, 1f),
                        VisualRotation = 0,
                        SwingDirectionChange = true,
                        OnHitStopTime = 5,
                        ChangeCondition = () => InMiddleDis() && NPC.collideY,
                        SwingLenght = BSS_Length
                    });
            #region 跳跃
            BrokenStarStickSwing_Jump BSS_Jump = new(NPC,
                    LongPreTime,
                    new()
                    {
                        SwingTime = 25,
                        SwingTimeChange = GeneralSwing,
                        OnChange = (_) =>
                        {
                            NPC.velocity.Y -= 6;
                            NPC.velocity.X = NPC.spriteDirection * 5;
                        }
                    }, new()
                    {
                        PostSwingTime = 3,
                        PostSwingTimeMax = 10
                    }, new()
                    {
                        ActionDmg = 1f,
                        StartVel = Vector2.UnitX.RotatedBy(-0.9),
                        SwingRot = MathHelper.PiOver2 + 0.9f,
                        VelScale = new Vector2(1, 1f) * 0.95f,
                        VisualRotation = 0,
                        SwingDirectionChange = true,
                        OnHitStopTime = 5,
                        ChangeCondition = () => DisTarget() < FarPlayer && NPC.collideY && Main.rand.NextBool(15),
                        SwingLenght = BSS_Length
                    })
            {
                CanDef = false
            };
            #endregion
            #region 蜻蜓点水
            BrokenStarStickSwing_Jump BSS_Jump_DragonflySkimmingTheWater = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    onHit = (target, _) =>
                    {
                        target.GetModPlayer<ControlPlayer>().StopControl *= 2;
                        target.velocity.Y -= 5;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 10f,
                    StartVel = -Vector2.UnitY,
                    SwingRot = MathHelper.Pi,
                    VelScale = new Vector2(0.1f, 1f) * 0.95f,
                    VisualRotation = 0,
                    SwingDirectionChange = true,
                    OnHitStopTime = 5,
                    ChangeCondition = () => !NPC.collideY && Main.rand.NextBool(2),
                    SwingLenght = BSS_Length
                })
            {
                CanDef = false
            };
            BrokenStarStickSwing BSS_Jump_DragonflySkimmingTheWater_After = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 45,
                    SwingTimeChange = GeneralSwing,
                    onHit = (target,_)=>
                    {
                        target.GetModPlayer<ControlPlayer>().StopControl *= 2;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 20f,
                    StartVel = -Vector2.UnitX,
                    SwingRot = MathHelper.Pi + MathHelper.PiOver2 * 0.65f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0,
                    SwingDirectionChange = true,
                    OnHitStopTime = 5,
                    ChangeCondition = () => !NPC.collideY && Main.rand.NextBool(2),
                    SwingLenght = BSS_Length
                })
            {
                CanDef = false
            };
            #endregion
            #endregion
            #region 剥皮与断骨
            #region 剥皮
            SkinningSwing skinning_NThorw = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _,ref Player.HurtModifiers hurt)=> hurt.SourceDamage *= 3
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f) * 1.1f,
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() >= ClosePlayer && Main.rand.NextBool(RandomSkillMax) && NPC.collideY,
                    SwingLenght = Skinning_Length
                })
            {
                CanThrow = true,
                ThrowDir = () => (TargetPlayer.Center - NPC.Center).SafeNormalize(default)
            };
            #endregion
            #region 断骨
            BrokenBonesSwing BB_PursuitSlash = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 5
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => true,
                    SwingLenght = BB_Length
                });

            BrokenBonesSwing BB_NCombo_1 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 2
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < ClosePlayer && Main.rand.NextBool(RandomSkillMax) && NPC.collideY,
                    SwingLenght = BB_Length
                });
            BrokenBonesSwing BB_NCombo_2 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 3
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = Vector2.UnitY.RotatedBy(0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.3f),
                    VisualRotation = -0.7f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = false,
                    ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BB_Length
                });
            BrokenBonesSwing BB_NCombo_3 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 5
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 2f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BB_Length
                });
            #endregion
            #region 断骨变形
            #region 空中咬合
            BrokenBonesChangeSwing_Biting BBC_Biting = new(NPC, // 咬
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 3
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 15f,
                    StartVel = -Vector2.UnitY,
                    SwingRot = MathHelper.Pi,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && !NPC.collideY && Main.rand.NextBool(3),
                    SwingLenght = BBC_Length
                })
            {
                OnBiting = (skill) =>
                {
                    var rect = TargetPlayer.getRect();
                    if(skill.Bite1.GetColliding(rect) || skill.Bite2.GetColliding(rect))
                    {
                        if(TargetPlayer.immune)
                        {
                            return;
                        }
                        Player.HurtModifiers hurtModifiers = new()
                        {
                            Dodgeable = false,
                        };
                        hurtModifiers.SourceDamage += skill.setting.ActionDmg - 1f;
                        hurtModifiers.SourceDamage += DamageAdd;
                        skill.onSwing.modifyHit.Invoke(TargetPlayer, ref hurtModifiers);
                        Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(skill.WeaponDamage, TargetPlayer.statDefense, TargetPlayer.DefenseEffectiveness.Value, 0, true);
                        hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                        TargetPlayer.Hurt(hurtInfo);
                        for (int j = Main.rand.Next(5, 8); j > 0; j--)
                        {
                            HitPiecredExtra98 hitPiecredExtra98 = new(skill.swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * skill.setting.SwingDirectionChange.ToDirectionInt() * NPC.spriteDirection) * 0.2f, TargetPlayer.Center);
                            Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                        }
                        TargetPlayer.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 10;
                        DamageAdd += 0.5f;
                        TargetPlayer.immuneTime /= 3;
                        skill.hitNum++;
                    }
                }
            };
            #endregion
            #region 傲慢咬合
            BrokenBonesChangeSwing_Biting BBC_ArroganceBiting = new(NPC, // 傲慢咬合
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 5
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 15f,
                    StartVel = -Vector2.UnitX,
                    SwingRot = MathHelper.Pi,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && NPC.collideY && Main.rand.NextBool(3),
                    SwingLenght = BBC_Length
                })
            {
                OnBiting = (skill) =>
                {
                    var rect = TargetPlayer.getRect();
                    if (skill.Bite1.GetColliding(rect) || skill.Bite2.GetColliding(rect))
                    {
                        if(skill.hitNum >= 15)
                        {
                            skill.CanChange = true;
                            return;
                        }
                        Player.HurtModifiers hurtModifiers = new()
                        {
                            Dodgeable = false,
                        };
                        NPC.velocity.X *= 0.99f;
                        hurtModifiers.SourceDamage += DamageAdd;
                        skill.onSwing.modifyHit.Invoke(TargetPlayer, ref hurtModifiers);
                        TargetPlayer.velocity *= 0;
                        TargetPlayer.Center = skill.swingHelper.Center + skill.swingHelper.velocity;
                        Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(skill.WeaponDamage, TargetPlayer.statDefense, TargetPlayer.DefenseEffectiveness.Value, 0, true);
                        hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                        TargetPlayer.Hurt(hurtInfo);
                        for (int j = Main.rand.Next(5, 8); j > 0; j--)
                        {
                            HitPiecredExtra98 hitPiecredExtra98 = new(skill.swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * skill.setting.SwingDirectionChange.ToDirectionInt() * NPC.spriteDirection) * 0.2f, TargetPlayer.Center);
                            Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                        }
                        TargetPlayer.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 10;
                        DamageAdd += 0.5f;
                        TargetPlayer.immuneTime = 0;
                        skill.hitNum++;
                        NPC.ai[1] -= 1;
                        Main.instance.CameraModifiers.Add(new PunchCameraModifier(TargetPlayer.position, -Vector2.UnitX * TargetPlayer.direction * (skill.hitNum % 2 == 0 ? 1 : -1), MathF.Log2(hurtInfo.Damage), 60, 20));
                    }
                }
            };
            BrokenBonesChangeSwing BBC_ArroganceBiting_End = new(NPC, // 傲慢咬合结束
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) => hurt.SourceDamage *= 15,
                    OnUse = (skill) =>
                    {
                        TargetPlayer.SetImmuneTimeForAllTypes(2);
                        TargetPlayer.Center = skill.swingHelper.Center + skill.swingHelper.velocity;
                    },
                    OnChange = (skill) =>
                    {
                        Player.HurtModifiers hurtModifiers = new()
                        {
                            Dodgeable = false,
                        };
                        hurtModifiers.SourceDamage += DamageAdd;
                        skill.onSwing.modifyHit.Invoke(TargetPlayer, ref hurtModifiers);
                        Player.HurtInfo hurtInfo = hurtModifiers.ToHurtInfo(skill.WeaponDamage, TargetPlayer.statDefense, TargetPlayer.DefenseEffectiveness.Value, 0, true);
                        hurtInfo.DamageSource = PlayerDeathReason.ByNPC(NPC.whoAmI);
                        TargetPlayer.Hurt(hurtInfo);
                        for (int j = Main.rand.Next(5, 8); j > 0; j--)
                        {
                            HitPiecredExtra98 hitPiecredExtra98 = new(skill.swingHelper.velocity.RotatedBy(MathHelper.PiOver2 * skill.setting.SwingDirectionChange.ToDirectionInt() * NPC.spriteDirection) * 0.2f, TargetPlayer.Center);
                            Core.Particles.ParticlesSystem.AddParticle(Core.Particles.BasicParticle.DrawLayer.AfterDust, hitPiecredExtra98);
                        }
                        TargetPlayer.GetModPlayer<ControlPlayer>().StopControl = (int)Math.Log2(hurtInfo.Damage) * 20;
                        DamageAdd += 0.5f;
                        TargetPlayer.immuneTime = 0;
                        Main.instance.CameraModifiers.Add(new PunchCameraModifier(TargetPlayer.position, -Vector2.UnitX * TargetPlayer.direction, MathF.Log2(hurtInfo.Damage), 60, 20));
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 5f,
                    StartVel = Vector2.UnitX,
                    SwingRot = MathHelper.Pi,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = false,
                    ChangeCondition = () => true,
                    SwingLenght = BBC_Length
                });
            #endregion
            #region 断骨碎魂
            BrokenBonesChangeSwing BBC_BrokenBonesSlashSoul_1 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 5,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 5;
                        hurt.FinalDamage += 2;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 10f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.3f),
                    VisualRotation = -0.7f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && NPC.collideY && Main.rand.NextBool(3),
                    SwingLenght = BBC_Length
                });
            BrokenBonesChangeSwing BBC_BrokenBonesSlashSoul_2 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 5,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 7;
                        hurt.FinalDamage += 2;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 10f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.3f),
                    VisualRotation = -0.7f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => true,
                    SwingLenght = BBC_Length
                });
            BrokenBonesChangeSwing BBC_BrokenBonesSlashSoul_3 = new(NPC, 
                LongPreTime,
                new()
                {
                    SwingTime = 5,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 10;
                        hurt.FinalDamage += 2;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 30f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => true,
                    SwingLenght = BBC_Length
                });
            #endregion
            #region 基础Combo
            BrokenBonesChangeSwing BBC_Combo_1 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 3;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 4f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.6f),
                    VisualRotation = -0.4f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BBC_Length
                });

            BrokenBonesChangeSwing BBC_Combo_2 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 4;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 5f,
                    StartVel = Vector2.UnitY.RotatedBy(0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.3f),
                    VisualRotation = -0.7f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = false,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BBC_Length
                });
            BrokenBonesChangeSwing BBC_Combo_3 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 15,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 4;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 7f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi + 0.8f,
                    VelScale = new Vector2(1, 0.3f),
                    VisualRotation = -0.7f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BBC_Length
                });
            BrokenBonesChangeSwing BBC_Combo_4 = new(NPC,
                LongPreTime,
                new()
                {
                    SwingTime = 10,
                    SwingTimeChange = GeneralSwing,
                    modifyHit = (Player _, ref Player.HurtModifiers hurt) =>
                    {
                        hurt.SourceDamage *= 5;
                    }
                }, new()
                {
                    PostSwingTime = 10,
                    PostSwingTimeMax = 60
                }, new()
                {
                    ActionDmg = 10f,
                    StartVel = -Vector2.UnitY.RotatedBy(-0.4),
                    SwingRot = MathHelper.Pi,
                    VelScale = new Vector2(1, 1f),
                    VisualRotation = 0f,
                    OnHitStopTime = 5,
                    SwingDirectionChange = true,
                    ChangeCondition = () => InStrongAxeMode && DisTarget() < ClosePlayer && NPC.collideY,
                    SwingLenght = BBC_Length
                });
            #endregion
            #endregion
            #endregion
            #endregion
            #endregion

            #region 登记
            SkillNPC.Register(phase1);
            SkillNPC.Register(phase1Start,walk1, noAtk_1);
            SkillNPC.Register(BS_combo1_1, BS_combo1_2, BS_combo1_3, BS_combo2_1, BS_combo2_2, BS_combo3,
                              BS_TenCharSlash_1, BS_TenCharSlash_2, BS_SlashUp, BS_StarFall, SP_combo1_1, SP_combo1_2,
                              SP_combo2_1, SP_combo2_2, SP_combo2_3, SP_combo3_1, SP_combo3_2, SP_combo4, SP_YiyaDash,
                              madnessAndFearNormalShoot, madnessAndFearFullShot, BSS_combo1_1, BSS_combo1_2,
                              BSS_combo1_3, BSS_combo1_4, BSS_combo1_5, BSS_Jump, BSS_Jump_DragonflySkimmingTheWater,
                              BSS_Jump_DragonflySkimmingTheWater_After, BS_Def, BS_DefSlash, skinning_NThorw,
                              BB_PursuitSlash, BB_NCombo_1, BB_NCombo_2, BB_NCombo_3, BBC_Biting, BBC_ArroganceBiting,
                              BBC_ArroganceBiting_End, BBC_BrokenBonesSlashSoul_1, BBC_BrokenBonesSlashSoul_2,
                              BBC_BrokenBonesSlashSoul_3, BBC_Combo_1, BBC_Combo_2, BBC_Combo_3, BBC_Combo_4);
            #endregion

            #region 链接
            phase1.OnEnterMode();
            CurrentMode = phase1;

            #region 一阶段技能
            noAtk_1.AddBySkilles(BS_combo1_1, BS_combo1_2, BS_combo1_3, BS_combo2_1, BS_combo2_2, BS_combo3, SP_combo1_1,
                                 SP_combo1_2, BS_SlashUp, BS_StarFall, BS_TenCharSlash_1, BS_TenCharSlash_2, SP_combo2_1,
                                 SP_combo2_2, SP_combo2_3, SP_combo4, SP_YiyaDash, BSS_combo1_1, BSS_combo1_2,
                                 BSS_combo1_3, BSS_combo1_4, BSS_combo1_5, BB_NCombo_1, BB_NCombo_2, BB_NCombo_3,
                                 BB_PursuitSlash, BBC_BrokenBonesSlashSoul_1, BBC_BrokenBonesSlashSoul_2,
                                 BBC_BrokenBonesSlashSoul_3, BBC_Combo_1, BBC_Combo_2, BBC_Combo_3, BBC_Combo_4,
                                 BBC_ArroganceBiting, BBC_Biting, BBC_BrokenBonesSlashSoul_1, BBC_BrokenBonesSlashSoul_2,
                                 BBC_BrokenBonesSlashSoul_3, BS_DefSlash);

            walk1.AddSkill(BBC_BrokenBonesSlashSoul_1).AddSkill(BBC_BrokenBonesSlashSoul_2).AddSkill(BBC_BrokenBonesSlashSoul_3);
            walk1.AddSkill(BBC_ArroganceBiting).AddSkill(BBC_ArroganceBiting_End);
            walk1.AddSkill(BBC_Biting);
            walk1.AddSkill(BBC_Combo_1).AddSkill(BBC_Combo_2).AddSkill(BBC_Combo_3).AddSkill(BBC_Combo_4);
            skinning_NThorw.AddBySkilles(walk1).AddSkill(BB_PursuitSlash);
            walk1.AddSkill(BB_NCombo_1).AddSkill(BB_NCombo_2).AddSkill(BB_NCombo_3);

            BSS_Jump_DragonflySkimmingTheWater.AddBySkilles(walk1).AddSkilles(BSS_Jump_DragonflySkimmingTheWater_After, BSS_Jump_DragonflySkimmingTheWater);
            BSS_Jump.AddBySkilles(walk1, BS_combo1_1, BS_combo1_2, BS_combo1_3, BS_combo2_1, BS_combo2_2, BS_combo3,
                                  SP_combo1_1, SP_combo1_2, BS_SlashUp, BS_StarFall, BS_TenCharSlash_1,
                                  BS_TenCharSlash_2, SP_combo2_1, SP_combo2_2, SP_combo2_3, SP_combo4, SP_YiyaDash,
                                  BSS_combo1_1, BSS_combo1_2, BSS_combo1_3, BSS_combo1_4, BSS_combo1_5);
            BS_combo1_1.AddBySkilles(BSS_combo1_2, BSS_combo1_3, BSS_combo1_4, BSS_combo1_5);
            BSS_combo1_1.AddSkill(BSS_combo1_2).AddSkill(BSS_combo1_3).AddSkill(BSS_combo1_4).AddSkill(BSS_combo1_5);
            BSS_combo1_1.AddBySkilles(walk1, BS_combo1_3, BS_combo2_2, BS_combo3, SP_combo1_2, SP_combo2_3, SP_combo3_2, SP_combo4, SP_YiyaDash, BS_TenCharSlash_2);

            madnessAndFearFullShot.AddBySkilles(walk1);

            madnessAndFearNormalShoot.AddBySkilles(walk1, SP_YiyaDash, BS_TenCharSlash_2);

            SP_YiyaDash.AddBySkilles(SP_combo1_1, SP_combo1_2, SP_combo2_1, SP_combo2_2, SP_combo2_3, SP_combo3_1, SP_combo3_2, SP_combo4, walk1);
            SP_combo2_2.AddSkill(SP_combo4);
            SP_combo2_1.AddSkill(SP_combo3_1).AddSkill(SP_combo3_2).AddSkill(BS_combo1_1);
            SP_combo1_1.AddSkill(SP_combo2_1).AddSkill(SP_combo2_2).AddSkill(SP_combo2_3).AddSkill(BS_combo1_1);
            walk1.AddSkill(SP_combo1_1).AddSkill(SP_combo1_2).AddSkill(BS_combo1_1);

            BS_Def.AddBySkilles(walk1, BS_combo1_1, BS_combo1_2, BS_combo1_3, BS_combo2_1, BS_combo2_2, BS_combo3,
                                SP_combo1_1, SP_combo1_2, BS_SlashUp, BS_StarFall, BS_TenCharSlash_1, BS_TenCharSlash_2,
                                SP_combo2_1, SP_combo2_2, SP_combo2_3, SP_combo4, SP_YiyaDash, BSS_combo1_1,
                                BSS_combo1_2, BSS_combo1_3, BSS_combo1_4, BSS_combo1_5).AddSkill(BS_DefSlash);
            BS_SlashUp.AddBySkilles(BS_combo1_1, BS_combo1_2, BS_combo1_3, BS_combo2_1, BS_combo2_2, BS_combo3, walk1, BS_TenCharSlash_2);
            BS_SlashUp.AddSkill(BS_StarFall);
            BS_TenCharSlash_1.AddBySkilles(BS_combo1_1, BS_combo1_2, BS_combo1_3, BS_combo2_1, BS_combo2_2, BS_combo3, walk1);
            BS_TenCharSlash_1.AddSkill(BS_TenCharSlash_2);
            BS_combo1_2.AddSkill(BS_combo3).AddSkill(SP_combo1_1);
            BS_combo1_1.AddSkill(BS_combo2_1).AddSkill(BS_combo2_2).AddSkill(SP_combo1_1);
            walk1.AddSkill(BS_combo1_1).AddSkill(BS_combo1_2).AddSkill(BS_combo1_3).AddSkill(SP_combo1_1);
            phase1Start.AddSkill(walk1);
            #endregion

            phase1Start.OnSkillActive(null);
            CurrentSkill = phase1Start;
            #endregion

        }
        public float DisTarget() => TargetPlayer.Center.Distance(NPC.Center);
        public float GeneralSwing(float arg) => MathHelper.SmoothStep(0, 1f, MathF.Pow(arg, 2.5f));
        public bool InMiddleDis() => DisTarget() > ClosePlayer && DisTarget() < FarPlayer;
        public override void OnSkillTimeOut()
        {
            if(CurrentMode is SO_Phase1)
            {
                CurrentSkill.OnSkillDeactivate(walk1);
                walk1.OnSkillActive(CurrentSkill);
                OldSkills.Enqueue(CurrentSkill);
                CurrentSkill = walk1;
            }
        }
    }
}
