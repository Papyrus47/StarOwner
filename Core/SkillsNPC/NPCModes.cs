namespace StarOwner.Core.SkillsNPC
{
    /// <summary>
    /// 这是NPC的状态类 
    /// 如果没有什么意外，这个类将会被用来存储NPC的状态信息
    /// 例如距离玩家的距离，是否在战斗中，是否被攻击等等
    /// 这个类不用更改储存技能
    /// </summary>
    public class NPCModes
    {
        public NPC NPC;
        public NPCModes(NPC npc)
        {
            NPC = npc;
        }
        /// <summary>
        /// 激活模式的条件
        /// </summary>
        /// <param name="activeMode">存在的模式</param>
        /// <returns></returns>
        public virtual bool ActivationCondition(NPCModes activeMode) => true;
        /// <summary>
        /// 切换模式的条件
        /// </summary>
        /// <param name="changeToMode">要切换到的模式</param>
        /// <returns></returns>
        public virtual bool SwitchCondition(NPCModes changeToMode) => true;
        /// <summary>
        /// 进入Mode调用
        /// </summary>
        public virtual void OnEnterMode() { }
        /// <summary>
        /// 退出Mode调用
        /// </summary>
        public virtual void OnExitMode() { }
    }
}
