using System.IO;

namespace StarOwner.Core.SkillsNPC
{
    public interface IBasicSkillNPC
    {
        public Queue<NPCSkills> OldSkills { get; set; }
        public NPCSkills CurrentSkill
        {
            get
            {
                if (SaveSkills.TryGetValue(CurrentSkillID, out NPCSkills skill))
                    return skill;
                return null;
            }
            set
            {
                if (SaveSkillsID.TryGetValue(value, out int id))
                {
                    CurrentSkillID = id;
                    return; // 找到了技能ID，直接取消后面操作
                }
                Register(value); // 没有则注册
            }
        }
        public NPCModes CurrentMode
        {
            get
            {
                if (SaveModes.TryGetValue(CurrentModesID, out NPCModes mode))
                    return mode;
                return null;
            }
            set
            {
                if (SaveModesID.TryGetValue(value, out int id))
                {
                    CurrentModesID = id;
                    return; // 找到了状态ID，直接取消后面操作
                }
                Register(value); // 没有则注册
            }
        }
        public void OnSkillTimeOut();
        #region 切换
        /// <summary>
        /// 尝试技能切换
        /// </summary>
        public void TryChangeSkill()
        {
            if (CurrentSkill == null)
                return;
            if (CurrentSkill.SkillTimeOut) // 技能强制回滚
            {
                OnSkillTimeOut();
                return;
            }
            foreach (var targetSkill in CurrentSkill.SkillsTree)
            {
                if (targetSkill.ActivationCondition(CurrentSkill) && CurrentSkill.SwitchCondition(targetSkill) || targetSkill.CompulsionSwitchSkill(CurrentSkill))
                {
                    OldSkills.Enqueue(CurrentSkill);
                    if (OldSkills.Count > 10) // 大于10
                        OldSkills.Dequeue(); // 减少一个
                    targetSkill.OnSkillActive(CurrentSkill);
                    CurrentSkill.OnSkillDeactivate(targetSkill);
                    CurrentSkill = targetSkill;
                    break;
                }
            }
        }
        /// <summary>
        /// 尝试状态切换
        /// </summary>
        public void TryChangeMode()
        {
            SaveModes ??= [];
            SaveModesID ??= [];
            foreach (var targetMode in SaveModesID.Keys)
            {
                if (targetMode.ActivationCondition(CurrentMode) && CurrentMode.SwitchCondition(targetMode))
                {
                    CurrentMode.OnExitMode();
                    targetMode.OnEnterMode();
                    CurrentMode = targetMode;
                }
            }
        }
        #endregion
        #region 注册与维护
        public void SendData(BinaryWriter writer)
        {
            writer.Write(CurrentModesID);
            writer.Write(CurrentSkillID);
        }
        public void ReadData(BinaryReader reader)
        {
            CurrentModesID = reader.ReadInt32();
            CurrentSkillID = reader.ReadInt32();
        }
        /// <summary>
        /// 当前技能的ID
        /// </summary>
        public int CurrentSkillID { get; set; }
        public int CurrentModesID { get; set; }
        /// <summary>
        /// 保存的技能
        /// </summary>
        public Dictionary<int, NPCSkills> SaveSkills { get; set; }
        /// <summary>
        /// 维护技能表
        /// </summary>
        public Dictionary<NPCSkills, int> SaveSkillsID { get; set; }
        /// <summary>
        /// 维护状态表
        /// </summary>
        public Dictionary<NPCModes, int> SaveModesID { get; set; }
        /// <summary>
        /// 保存的状态
        /// </summary>
        public Dictionary<int, NPCModes> SaveModes { get; set; }
        public void Init();
        /// <summary>
        /// 注册状态
        /// </summary>
        /// <param name="mode"></param>
        /// <exception cref="Exception">是否注册过这个状态ID</exception>
        public void Register(NPCModes mode)
        {
            SaveModes ??= [];
            SaveModesID ??= [];
            if (!SaveModes.ContainsValue(mode)) // 不存在这个状态
            {
                int i = 0;
                while (true)
                {
                    if (!SaveModes.ContainsKey(i)) // 如果不存在这个状态ID
                    {
                        SaveModes.Add(i, mode); // 注册状态
                        SaveModesID.Add(mode, i); // 注册维护表
                        break; // 跳出循环
                    }
                    i++; // 尝试下一个状态ID
                }
            }
        }
        /// <summary>
        /// 注册多个状态
        /// </summary>
        /// <param name="mode"></param>
        public void Register(params NPCModes[] mode)
        {
            foreach (var m in mode)
            {
                Register(m);
            }
        }
        /// <summary>
        /// 注册技能
        /// </summary>
        /// <param name="skill">技能</param>
        /// <exception cref="Exception">是否注册过这个技能ID</exception>
        public void Register(NPCSkills skill)
        {
            SaveSkills ??= [];
            SaveSkillsID ??= [];
            if (!SaveSkills.ContainsValue(skill)) // 不存在这个技能
            {
                int i = 0;
                while (true)
                {
                    if (!SaveSkills.ContainsKey(i)) // 如果不存在这个技能ID
                    {
                        SaveSkills.Add(i, skill); // 注册技能
                        SaveSkillsID.Add(skill, i); // 注册维护表
                        break; // 跳出循环
                    }
                    i++; // 尝试下一个技能ID
                }
            }
        }
        /// <summary>
        /// 注册多个技能
        /// </summary>
        /// <param name="skill">技能数组</param>
        public void Register(params NPCSkills[] skill)
        {
            foreach (var s in skill)
            {
                Register(s);
            }
        }
        #endregion
    }
}
