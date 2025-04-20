namespace StarOwner.Core.SkillsNPC
{
    public class NPCSkills
    {
        /// <summary>
        /// 技能树
        /// </summary>
        public List<NPCSkills> SkillsTree = new();
        /// <summary>
        /// 这个技能指定的NPC
        /// </summary>
        public NPC NPC;
        /// <summary>
        /// NPC的ModNPC们
        /// </summary>
        public ModNPC ModNPC => NPC.ModNPC;
        /// <summary>
        /// 技能强制跳出,回滚
        /// </summary>
        public bool SkillTimeOut;
        public NPCSkills(NPC npc)
        {
            NPC = npc;
        }
        #region 添加技能列表
        /// <summary>
        /// 添加技能到技能树
        /// </summary>
        /// <param name="skills">需要添加的技能</param>
        /// <returns>添加的技能</returns>
        public NPCSkills AddSkill(NPCSkills skills)
        {
            SkillsTree.Add(skills);
            return skills;
        }
        /// <summary>
        /// 添加多个技能到技能树
        /// </summary>
        /// <param name="skills">技能列表</param>
        public void AddSkilles(params NPCSkills[] skills)
        {
            foreach (var skill in skills)
            {
                AddSkill(skill);
            }
        }
        /// <summary>
        /// 添加当前技能到目标技能
        /// </summary>
        /// <param name="skills">目标技能</param>
        /// <returns>被添加者</returns>
        public NPCSkills AddBySkilles(params NPCSkills[] targetSkills)
        {
            foreach (var targetSkill in targetSkills)
            {
                targetSkill.AddSkill(this);
            }
            return this;
        }
        #endregion
        #region 构成技能的东西
        public virtual void AI() { }
        public virtual bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => true;
        public virtual void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) { }
        public virtual void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) { }
        public virtual bool CanHitPlayer(Player target, ref int cooldownSlot) => true;

        public virtual void FindFrame(int frameHeight) { }
        public virtual void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers) { }
        public virtual void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers) { }
        public virtual void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) { }
        /// <summary>
        /// 激活这个技能的条件
        /// </summary>
        /// <param name="activeSkill">存在的技能</param>
        /// <returns></returns>
        public virtual bool ActivationCondition(NPCSkills activeSkill) => true;
        /// <summary>
        /// 技能切换的条件
        /// </summary>
        /// <param name="changeToSkill">要切换到的技能</param>
        /// <returns>返回true 则可以切换技能</returns>
        public virtual bool SwitchCondition(NPCSkills changeToSkill) => true;
        /// <summary>
        /// 强制切换到这个技能,无视前者的条件
        /// </summary>
        /// <param name="activeSkill">存在的技能</param>
        /// <returns></returns>
        public virtual bool CompulsionSwitchSkill(NPCSkills activeSkill) => false;
        /// <summary>
        /// 技能切换后调用
        /// </summary>
        /// <param name="changeToSkill">切换到的技能</param>
        public virtual void OnSkillDeactivate(NPCSkills changeToSkill) { }
        /// <summary>
        /// 技能激活时调用
        /// </summary>
        /// <param name="activeSkill">激活的技能</param>
        public virtual void OnSkillActive(NPCSkills activeSkill) { }
        #endregion
    }
}
