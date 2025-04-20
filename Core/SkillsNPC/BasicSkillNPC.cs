using System.IO;

namespace StarOwner.Core.SkillsNPC
{
    public abstract class BasicSkillNPC : ModNPC, IBasicSkillNPC
    {
        public bool IsInit;

        public IBasicSkillNPC SkillNPC => this;
        public NPCSkills CurrentSkill
        {
            get
            {
                if (SaveSkills == null)
                    return null;
                if (SaveSkills.TryGetValue(CurrentSkillID, out NPCSkills skill))
                    return skill;
                return null;
            }
            set
            {
                if (SaveSkills == null)
                    return;
                if (SaveSkillsID.TryGetValue(value, out int id))
                {
                    CurrentSkillID = id;
                    return; // 找到了技能ID，直接取消后面操作
                }
                SkillNPC.Register(value); // 没有则注册
            }
        }
        public NPCModes CurrentMode
        {
            get
            {
                if (SaveSkills == null)
                    return null;
                if (SaveModes.TryGetValue(CurrentModesID, out NPCModes mode))
                    return mode;
                return null;
            }
            set
            {
                if (SaveSkills == null)
                    return;
                if (SaveModesID.TryGetValue(value, out int id))
                {
                    CurrentModesID = id;
                    return; // 找到了状态ID，直接取消后面操作
                }
                SkillNPC.Register(value); // 没有则注册
            }
        }
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
        public Queue<NPCSkills> OldSkills { get; set; }
        public override void AI()
        {
            if (!IsInit)
            {
                IsInit = true;
                Init();
            }
            CurrentSkill?.AI();
            SkillNPC.TryChangeMode(); // 先切换模式
            SkillNPC.TryChangeSkill(); // 再切换技能
        }
        public abstract void Init();
        public abstract void OnSkillTimeOut();
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            SkillNPC.SendData(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            SkillNPC.ReadData(reader);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            CurrentSkill?.ModifyHitPlayer(target, ref modifiers);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            CurrentSkill?.OnHitPlayer(target, hurtInfo);
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            CurrentSkill?.ModifyHitByItem(player, item, ref modifiers);
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            CurrentSkill?.ModifyHitByProjectile(projectile, ref modifiers);
        }
        public override void FindFrame(int frameHeight)
        {
            CurrentSkill?.FindFrame(frameHeight);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => CurrentSkill?.PreDraw(spriteBatch, screenPos, drawColor) != false;
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            CurrentSkill?.OnHitByItem(player, item, hit, damageDone);
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            CurrentSkill?.OnHitByProjectile(projectile, hit, damageDone);
        }
    }
}
