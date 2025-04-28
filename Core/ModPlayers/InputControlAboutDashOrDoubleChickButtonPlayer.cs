using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.ModPlayers
{
    public class InputControlAboutDashOrDoubleChickButtonPlayer : ModPlayer
    {
        public const int DashRight = 2;
        public const int DashLeft = 3;
        public int DashTimer;
        public int DashDir = -1;
        public override void ResetEffects()
        {
            if (DashTimer > 0) DashTimer--;

            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                DashDir = DashRight;
                DashTimer = 30;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                DashDir = DashLeft;
                DashTimer = 30;
            }
            else if (DashTimer <= 0)
            {
                DashDir = -1;
            }
        }
        public static int GetPlayerDoubleTapDir(int Dir)
        {
            if (Dir == 1)
                return 2; // 朝向为正-右边
            else
                return 3;
        }
        public bool GetPlayerDoubleTap(int Dir) => DashDir == Dir;
        /// <summary>
        /// 使用IsFaceMove判断朝向，返回是否为双击朝向
        /// </summary>
        /// <param name="IsFaceMove">是否是玩家面朝方向</param>
        /// <returns></returns>
        public bool IsPlayerFaceDoubleTapDir(bool IsFaceMove) => GetPlayerDoubleTap(GetPlayerDoubleTapDir(Player.direction * IsFaceMove.ToDirectionInt()));
    }
}
