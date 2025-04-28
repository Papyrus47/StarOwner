using StarOwner.Core.SkillProjs;
using StarOwner.Core.SpurtHelpers;

namespace StarOwner.Core.SkillProjs.GeneralSkills
{
    /// <summary>
    /// 单次突刺
    /// </summary>
    public class SpurtHelper_GeneralOneSpurt : ProjSkill_Instantiation
    {
        public SpurtHelper SpurtHelper;
        public Player Player;
        public Setting setting;
        public SpurtHelper_GeneralOneSpurt(ModProjectile modProj, Setting setting, SpurtHelper spurtHelper, Player player) : base(modProj)
        {
            this.setting = setting;
            SpurtHelper = spurtHelper;
            Player = player;
        }
        public class Setting
        {
            /// <summary>
            /// 突刺持续时间
            /// 以委托方式获取，以便在游戏中动态修改
            /// </summary>
            public Func<int> UseTimeMax;
            /// <summary>
            /// 突刺长度
            /// 以委托方式获取，以便在游戏中动态修改
            /// </summary>
            public Func<float> SpurtLength;
            public Func<bool> ActivationCondition;
            public delegate void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers);
            public delegate void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers);
            public delegate void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone);
            public delegate void OnHitPlayer(Player target, Player.HurtInfo hurt);
            public OnHitNPC OnHit;
            public OnHitPlayer OnHit_Player;
            public ModifyHitPlayer ModifyHit_Player;
            public ModifyHitNPC ModifyHit;
        }
        public Action<SpurtHelper_GeneralOneSpurt> OnUse;
        public override void AI()
        {
            OnUse?.Invoke(this);
            Projectile.ai[0]++;
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter, true); // 中心位置更改
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Player.direction;
            Player.itemAnimation = Player.itemTime = 2;
            Player.ChangeDir(Projectile.direction);
            Player.heldProj = Projectile.whoAmI;
            //Player.fullRotation = Projectile.velocity.X * SpurtHelper.Time * 0.02f;
            //Player.fullRotationOrigin = new Vector2(Player.width * 0.5f, Player.height);
            //Player.itemRotation = MathF.Atan2(Projectile.velocity.Y * Player.direction, Projectile.velocity.X * Player.direction);
            SyncData();
            if ((int)Projectile.ai[0] == setting.UseTimeMax.Invoke())
                SoundEngine.PlaySound(SoundID.Item1 with { Pitch = 0.5f }, Projectile.Center);
            SpurtHelper.Update(Projectile.Center, Player.direction, Projectile.velocity);
            if (setting.UseTimeMax.Invoke() * 1.7f <= Projectile.ai[0])
            {
                SkillTimeOut = true;
            }
        }
        public virtual void SyncData()
        {
            if (Main.myPlayer == Player.whoAmI)
                Projectile.netUpdate = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            setting?.OnHit?.Invoke(target, hit, damageDone);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurt)
        {
            setting?.OnHit_Player?.Invoke(target, hurt);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            setting?.ModifyHit?.Invoke(target, ref modifiers);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers hurtModifiers)
        {
            setting?.ModifyHit_Player?.Invoke(target, ref hurtModifiers);
        }
        public override void OnSkillActive()
        {
            Projectile.ai[0] = Projectile.ai[1] = Projectile.ai[2] = 0;
            SkillTimeOut = false;
            Projectile.originalDamage = Player.GetWeaponDamage(Player.HeldItem);
            Projectile.damage = Projectile.originalDamage;
            Projectile.knockBack = Player.GetWeaponKnockback(Player.HeldItem);
            Projectile.CritChance = Player.GetWeaponCrit(Player.HeldItem);
            TheUtility.ResetProjHit(Projectile);
            Player.fullRotation = 0;
            Projectile.numHits = 0;
            SpurtHelper.Time = 0;
            if (Main.myPlayer == Player.whoAmI)
                Projectile.velocity = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);
            SpurtHelper.Change(setting.SpurtLength.Invoke(), Projectile.velocity, (int)(setting.UseTimeMax.Invoke() * 1.7f), Projectile.Size * 0.2f, 1, ASpurtDraw_Proj);
            SyncData();
        }
        public override void OnSkillDeactivate()
        {
            Projectile.ai[0] = Projectile.ai[1] = Projectile.ai[2] = 0;
            SkillTimeOut = false;
            Projectile.originalDamage = Player.GetWeaponDamage(Player.HeldItem);
            Projectile.damage = Projectile.originalDamage;
            Projectile.knockBack = Player.GetWeaponKnockback(Player.HeldItem);
            Projectile.CritChance = Player.GetWeaponCrit(Player.HeldItem);
            TheUtility.ResetProjHit(Projectile);
            Projectile.rotation = 0;
            SyncData();
            Projectile.numHits = 0;
            Player.fullRotation = 0;
            Player.fullRotationOrigin = new Vector2(Player.width * 0.5f, Player.height);
        }
        public void ASpurtDraw_Proj(SpurtHelper spurt, SpriteBatch sb, params object[] args)
        {
            float scaleY = Math.Min(Projectile.ai[0] / setting.UseTimeMax.Invoke(), 1f);
            Projectile projectile = spurt.Owner as Projectile;
            Vector2 drawPos = spurt.SpurtPos - Main.screenPosition;
            Vector2 drawSize = new Vector2(spurt.Size.X, spurt.Size.Y);
            Main.instance.LoadProjectile(927);
            #region 绘制突刺
            Texture2D tex2 = TextureAssets.Projectile[927].Value;
            Vector2 scale = new Vector2(spurt.SpurtLenght / tex2.Height, scaleY) * projectile.scale;
            #endregion
            sb.Draw(tex2, drawPos, null, Projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(tex2.Width / 4f, tex2.Height / 2f), scale, SpriteEffects.None, 0f);
            #region 绘制弹幕

            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle drawRect = new Rectangle(0, (int)((float)projectile.frame / Main.projFrames[projectile.type] * tex.Height), tex.Width, tex.Height);
            sb.Draw(tex, drawPos, drawRect, Lighting.GetColor((spurt.SpurtPos / 16).ToPoint()), projectile.rotation + MathHelper.PiOver4, drawRect.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            #endregion
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => SpurtHelper.Colliding(targetHitbox);
        public override bool PreDraw(SpriteBatch sb, ref Color lightColor)
        {
            SpurtHelper.Draw(sb);
            return false;
        }
        public override bool ActivationCondition() => setting?.ActivationCondition?.Invoke() == true;
        public override bool SwitchCondition() => Projectile.ai[0] / setting.UseTimeMax.Invoke() > 0.5f;
    }
}
