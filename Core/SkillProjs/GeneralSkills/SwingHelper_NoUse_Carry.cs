using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.SkillProjs.GeneralSkills
{
    public class SwingHelper_NoUse_Carry : ProjSkill_Instantiation
    {
        public Player Player;
        public SwingHelper SwingHelper;
        public float Length = 30;
        public Func<bool> CanChange;
        public SwingHelper_NoUse_Carry(Player player, SwingHelper swingHelper, ModProjectile proj) : base(proj)
        {
            Player = player;
            SwingHelper = swingHelper;
        }
        public override void AI()
        {
            //Projectile.localAI[0] += player.velocity.X / 10;
            //Projectile.localAI[0] %= 6.28f;
            //float rotation = Projectile.localAI[0] % (MathHelper.PiOver4 * 2) - MathHelper.PiOver4;
            SkillTimeOut = false;
            SwingHelper.SetRotVel(0);
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Player.velocity.X / 10f); // 设置玩家的前臂为合成状态，并设置其伸展程度为Full
            Player.itemRotation = Player.compositeFrontArm.rotation; // 设置玩家的手臂角度为玩家的前臂的旋转角度

            if (Player.velocity.X != Player.oldVelocity.X && Main.myPlayer == Projectile.owner)
                Projectile.netUpdate = true;

            Player.heldProj = Projectile.whoAmI;
            SwingHelper.Change(Vector2.UnitX, Vector2.One, 0); // 起始位置,缩放
            SwingHelper.ProjFixedPos(Player.MountedCenter, -Length * 0.5f);
            Projectile.position.X -= Player.direction * Player.width * 0.75f;
            SwingHelper.SetSwingActive(); // 激活挥舞
            Projectile.spriteDirection = Player.direction; // 弹幕贴图朝向与玩家一致
            //Player.fullRotationOrigin = Player.Size * 0.5f;
            //Player.fullRotation = Player.velocity.X * 0.05f;
            //player.legRotation = -player.fullRotation;
            Projectile.rotation = 0;
            SwingHelper.SwingAI(Length, Player.direction, MathHelper.PiOver2);
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 120)
            {
                Projectile.ai[0] = 0;
                SkillTimeOut = true;
            }
        }
        public override void OnSkillActive()
        {
            base.OnSkillActive();
            SkillTimeOut = false;
        }
        public override bool ActivationCondition() => true; // 无条件激活
        public override bool SwitchCondition()
        {
            if (CanChange?.Invoke() != false)
                return true;
            else
                return false;
        }
        public override bool? CanDamage() => false; // 无法造成伤害
        public override bool PreDraw(SpriteBatch sb, ref Color lightColor)
        {
            SwingHelper.Swing_Draw_ItemAndTrailling(lightColor, null, null); // 用这个只绘制弹幕本体
            // 禁用绘制
            return false;
        }
    }
}
