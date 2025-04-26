using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.General
{
    public interface IDefenceAttack
    {
        /// <summary>
        /// 防御成功
        /// </summary>
        public bool DefenceSucceed { get; set; }
        /// <summary>
        /// 可以防御
        /// </summary>
        /// <returns></returns>
        public bool CanDefence() => true;
        /// <summary>
        /// 防御成功函数
        /// </summary>
        public void OnDefenceSucceed() { }
    }
}
