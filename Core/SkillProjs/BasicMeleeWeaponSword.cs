﻿using StarOwner.Core.ModPlayers;

namespace StarOwner.Core.SkillProjs
{
    public abstract class BasicMeleeWeaponSword : ModProjectile, IBasicSkillProj
    {
        #region 机器复制粘贴环节
        public Item SpawnItem;
        public Player Player;
        public SwingHelper SwingHelper;
        public List<ProjSkill_Instantiation> OldSkills { get; set; }
        public ProjSkill_Instantiation CurrentSkill
        {
            get
            {
                return SkillsParis[ID];
            }
            set
            {
                (this as IBasicSkillProj).Register(value);
                ID = IDParis[value];
            }
        }

        public int ID { get; set; }
        public Dictionary<ProjSkill_Instantiation, int> IDParis { get; set; }
        public Dictionary<int, ProjSkill_Instantiation> SkillsParis { get; set; }
        /// <summary>
        /// 属于物品攻击命中
        /// </summary>
        public virtual bool IsItemHit => true;
        public IBasicSkillProj BasicSkillProj => this;
        #region 使用相关
        public int MouseLeftTime;
        public int MouseRightTime;
        public int StopTime;
        public bool IsMouseLeftChick => MouseLeftTime < 10 && MouseLeftTime > 0;
        public bool IsMouseLeftChanning => MouseLeftTime >= 10;
        public bool IsMouseRightChick => MouseRightTime < 10 && MouseRightTime > 0;
        public bool IsMouseRightChanning => MouseRightTime >= 10;
        #endregion
        public override void SetDefaults()
        {
            Projectile.ownerHitCheck = true; // 弹幕检查是否隔墙
            Projectile.penetrate = -1; // 弹幕穿透
            Projectile.aiStyle = -1; // 弹幕AI样式
            Projectile.friendly = true; // 弹幕友好为true允许造成伤害
            Projectile.tileCollide = false; // 弹幕不碰撞墙壁
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地NPC伤害（本地无敌帧）
            Projectile.localNPCHitCooldown = -1; // -1表示只能命中一次
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemUse && itemUse.Item != null)
            {
                SpawnItem = itemUse.Item;
                Player = itemUse.Player;
                Projectile.Name = SpawnItem.Name;
                NewSwingHelper();
                Projectile.scale = Player.GetAdjustedItemScale(SpawnItem);
                Projectile.Size = SpawnItem.Size * Projectile.scale;
                IDParis = new();
                SkillsParis = new();
                OldSkills = new();
                //SwingLength = Projectile.Size.Length();
                //Main.projFrames[Type] = TheUtility.GetItemFrameCount(SpawnItem);
                Init();
            }
        }
        public virtual void NewSwingHelper()
        {
            SwingHelper ??= new(Projectile, 40, TextureAssets.Item[SpawnItem.type]);
            SwingHelper.DrawTrailCount = 3; // 绘制拖尾的次数
            SwingHelper.DrawItem_ScaleMoreThanOne = false; // 绘制武器的比例是否大于1
        }

        public override void OnKill(int timeLeft)
        {
            if (Player != null)
                Player.fullRotation = 0;
        }
        public override void AI()
        {
            if (Player == null || Player.HeldItem != SpawnItem || Player.dead) // 玩家手上物品不是生成物品,则清除
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2; // 弹幕不消失
            CurrentSkill.AI();
            Player.ResetMeleeHitCooldowns();
            IBasicSkillProj basicSkillProj = this;
            basicSkillProj.SwitchSkill();
            MouseUpdate();
            if (CurrentSkill.SwitchCondition())
                StopTime++;
            else
                StopTime = 0;
        }
        #region 同步环节
        public override void SendExtraAI(BinaryWriter writer)
        {
            (this as IBasicSkillProj).SendData(writer);
            SwingHelper?.SendData(writer);
            //writer.Write(Player.whoAmI);
            //writer.Write(Player.controlUseTile);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            (this as IBasicSkillProj).ReceiveData(reader);
            SwingHelper?.RendData(reader);
            //Player = Main.player[reader.ReadInt32()];
            //Player.controlUseTile = reader.ReadBoolean();
        }
        #endregion
        /// <summary>
        /// 鼠标更新
        /// </summary>
        public virtual void MouseUpdate()
        {
            if (Main.mouseLeft)
                MouseLeftTime++;
            else
                MouseLeftTime = 0;
            if (Main.mouseRight)
                MouseRightTime++;
            else
                MouseRightTime = 0;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => CurrentSkill.CanDamage();
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CurrentSkill.Colliding(projHitbox, targetHitbox);
        public override bool PreDraw(ref Color lightColor)
        {
            //Main.spriteBatch.Draw(DrawColorTex, new Vector2(500), null, Color.White, 0f, default, 4, SpriteEffects.None, 0f);
            return CurrentSkill.PreDraw(Main.spriteBatch, ref lightColor);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (IsItemHit)
            {
                ItemLoader.ModifyHitNPC(SpawnItem, Player, target, ref modifiers); // 调用Mod物品的ModifyHitNPC
                CurrentSkill.ModifyHitNPC(target, ref modifiers);

                modifiers.HideCombatText();
                modifiers.FinalDamage *= 0;
            }
            else
                CurrentSkill.ModifyHitNPC(target, ref modifiers);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return CurrentSkill.OnTileCollide(oldVelocity);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            ItemLoader.ModifyHitPvp(SpawnItem, Player, target, ref modifiers); // 调用Mod物品的ModifyHitPvp
            CurrentSkill.ModifyHitPlayer(target, ref modifiers);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsItemHit)
            {
                Type type = Player.GetType();
                type.GetField("_spawnVolcanoExplosion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
                type.GetField("_spawnBloodButcherer", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
                type.GetField("_batbatCanHeal", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
                type.GetField("_spawnTentacleSpikes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
                CurrentSkill.OnHitNPC(target, hit, damageDone); // 技能命中效果
                ItemLoader.OnHitNPC(SpawnItem, Player, target, hit, damageDone); // Mod物品命中
                ItemLoader.MeleeEffects(SpawnItem, Player, target.getRect()); // Mod物品命中近战特效
                Player.attackCD = 0;
                TheUtility.ItemCheck_MeleeHitNPCs(SpawnItem, Player, target.getRect(), hit.SourceDamage, hit.Knockback);
                TheUtility.VillagesItemOnHit(SpawnItem, Player, Projectile.Hitbox, Projectile.originalDamage, Projectile.knockBack, target.whoAmI, Projectile.damage, damageDone); // 原版物品命中
            }
            else
                CurrentSkill.OnHitNPC(target, hit, damageDone); // 技能命中效果
            
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Type type = Player.GetType();
            type.GetField("_spawnVolcanoExplosion", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
            type.GetField("_spawnBloodButcherer", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
            type.GetField("_batbatCanHeal", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
            type.GetField("_spawnTentacleSpikes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Player, true);
            CurrentSkill.OnHitPlayer(target, info); // 技能命中效果
            ItemLoader.OnHitPvp(SpawnItem, Player, target, info); // Mod物品命中
            // TheUtility.VillagesItemOnHit(SpawnItem, Player, Projectile.Hitbox, Projectile.originalDamage, Projectile.knockBack, target.whoAmI, Projectile.damage, Projectile.originalDamage); // 原版物品命中
        }
        #endregion
        public abstract void Init();
        public virtual bool PreSkillTimeOut() => true;
        #region 可以移除的辅助函数
        public bool ControlDir_MoveFaceDir() => Player.GetModPlayer<InputControlAboutDashOrDoubleChickButtonPlayer>().IsPlayerFaceDoubleTapDir(true);
        public bool ControlDir_MoveUnFaceDir() => Player.GetModPlayer<InputControlAboutDashOrDoubleChickButtonPlayer>().IsPlayerFaceDoubleTapDir(false);
        #endregion
    }
}
