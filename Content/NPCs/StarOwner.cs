using StarOwner.Content.NPCs.Mode;
using StarOwner.Content.NPCs.Skills.Phase1;
using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs
{
    public class StarOwner : BasicSkillNPC
    {
        public const string Phase1Text = "Mods.StarOwner.Bosses.StarOwner.Phase1Text";
        public Player drawPlayer;
        public Player TargetPlayer
        {
            get
            {
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                    NPC.TargetClosest();
                return Main.player[NPC.target];
            }
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void Load()
        {
            TheUtility.RegisterText(Phase1Text);
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
            NPC.width = 30;
            NPC.height = 44;
            NPC.boss = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000;
            NPC.value = Item.buyPrice(80);
            NPC.defense = 99999;
            NPC.knockBackResist = 0;
        }
        public override void AI()
        {
            if(NPC.lifeMax > 1000)
            {
                NPC.life = NPC.lifeMax = 1000;
            }
            NPC.defense = NPC.defDefense; // 防星击
            base.AI();
        }
        public override void Init()
        {
            drawPlayer = new Player();
            #region 注册
            SO_Phase1 phase1 = new(NPC);

            Phase1Start phase1Start = new(NPC);
            #endregion

            #region 登记
            SkillNPC.Register(phase1);
            SkillNPC.Register(phase1Start);
            #endregion

            #region 链接
            phase1.OnEnterMode();
            CurrentMode = phase1;

            phase1Start.OnSkillActive(null);
            CurrentSkill = phase1Start;
            #endregion

        }

        public override void OnSkillTimeOut()
        {
        }
    }
}
