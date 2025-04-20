using StarOwner.Core.SkillProjs;

namespace StarOwner.Core.SkillProjs.GeneralSkills
{
    public class General_ThrowProj : ProjSkill_Instantiation
    {
        public enum AIStateType : byte
        {
            Held,
            Flying,
            Back
        }
        public AIStateType StateType
        {
            get => (AIStateType)Projectile.ai[0];
            set => Projectile.ai[0] = (byte)value;
        }
        public class GlobalSetting
        {
            public delegate bool Draw(SpriteBatch sb);
            public delegate bool Colliding(Rectangle projHitbox, Rectangle targetHitbox);
            public delegate bool OnTileCollide(Vector2 oldVelocity);
            /// <summary>
            /// 用于绘制的委托
            /// </summary>
            public Draw draw;
            /// <summary>
            /// 碰撞检测
            /// </summary>
            public Colliding colliding;
            /// <summary>
            /// 是否更新位置,具体情况传入
            /// <see cref="ModProjectile.ShouldUpdatePosition"/>
            /// </summary>
            public Func<bool> IsUpdatePos;
            /// <summary>
            /// tileCollide
            /// </summary>
            public OnTileCollide tileCollide;
        }
        public class HeldSetting
        {
            /// <summary>
            /// 丢出去的速度
            /// </summary>
            public float ThrowSpeed;
            public Action<General_ThrowProj> heldAI;
            /// <summary>
            /// 切换到下一个状态
            /// </summary>
            public Func<bool> SwitchNextState;
            /// <summary>
            /// 丢出去的方向
            /// </summary>
            public Func<Vector2> ThrowDir;
        }
        public class FlyingSetting
        {
            /// <summary>
            /// 回来的速度
            /// </summary>
            public float BackSpeed;
            public Action<General_ThrowProj> throwAI;
            /// <summary>
            /// 切换到下一个状态
            /// </summary>
            public Func<bool> SwitchNextState;
            /// <summary>
            /// 飞回来的方向
            /// </summary>
            public Func<Vector2> BackDir;
        }
        public class BackSetting
        {
            /// <summary>
            /// 可以飞回来
            /// </summary>
            public bool CanBack;
            public Action<General_ThrowProj> backAI;
            /// <summary>
            /// 试图结束状态
            /// </summary>
            public Func<bool> TryEndState;
        }
        /// <summary>
        /// 技能激活的条件
        /// </summary>
        public Func<bool> ChangeCondition;
        public GlobalSetting setting;
        public HeldSetting heldSetting;
        public FlyingSetting flyingSetting;
        public BackSetting backSetting;

        public General_ThrowProj(GlobalSetting setting, HeldSetting heldSetting, FlyingSetting flyingSetting, BackSetting backSetting, Func<bool> changeCondition, ModProjectile modProjectile) : base(modProjectile)
        {
            this.setting = setting;
            this.heldSetting = heldSetting;
            this.flyingSetting = flyingSetting;
            this.backSetting = backSetting;
            ChangeCondition = changeCondition;
        }
        public override bool ActivationCondition() => ChangeCondition.Invoke();
        public override void AI()
        {
            Projectile.timeLeft = 2;
            switch (StateType)
            {
                case AIStateType.Held: // 手持
                    heldSetting.heldAI?.Invoke(this);
                    if (heldSetting.SwitchNextState?.Invoke() == true)
                    {
                        if (heldSetting.ThrowDir != null)
                            Projectile.velocity = heldSetting.ThrowDir.Invoke() * heldSetting.ThrowSpeed;
                        StateType = AIStateType.Flying;
                    }
                    break;
                case AIStateType.Flying: // 丢出
                    flyingSetting.throwAI?.Invoke(this);
                    if (setting.IsUpdatePos?.Invoke() != true)
                        Projectile.position += Projectile.velocity;
                    if (flyingSetting.SwitchNextState?.Invoke() == true)
                    {
                        if (flyingSetting.BackDir != null)
                            Projectile.velocity = flyingSetting.BackDir.Invoke() * flyingSetting.BackSpeed;
                        StateType = AIStateType.Back;
                    }
                    break;
                case AIStateType.Back: // 飞回
                    backSetting.backAI?.Invoke(this);
                    if (setting.IsUpdatePos?.Invoke() != true)
                        Projectile.position += Projectile.velocity;
                    if (backSetting.TryEndState.Invoke() == true)
                    {
                        SkillTimeOut = true;
                    }
                    break;
            }
        }
        public virtual void SyncData()
        {
            if (Main.myPlayer == Projectile.owner)
                Projectile.netUpdate = true;
        }
        public override void OnSkillActive()
        {
            Projectile.ai[0] = Projectile.ai[1] = Projectile.ai[2] = 0;
            SkillTimeOut = false;
            Projectile.damage = Projectile.originalDamage;
            TheUtility.ResetProjHit(Projectile);
            SyncData();
            Projectile.numHits = 0;
        }
        public override void OnSkillDeactivate()
        {
            Projectile.ai[0] = Projectile.ai[1] = Projectile.ai[2] = 0;
            SkillTimeOut = false;
            Projectile.damage = Projectile.originalDamage;
            Projectile.hide = false;
            Projectile.netUpdate = true;
            Projectile.numHits = 0;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => setting.colliding?.Invoke(projHitbox, targetHitbox);
        public override bool PreDraw(SpriteBatch sb, ref Color lightColor) => setting.draw?.Invoke(sb) != false;
        public override bool OnTileCollide(Vector2 oldVelocity) => setting.tileCollide?.Invoke(oldVelocity) != false;
    }
}
