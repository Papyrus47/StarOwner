﻿using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.NPCs.Skills.Phase1
{
    public class Walk1 : StarOwnerSkills
    {
        public Walk1(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.dontTakeDamage = false;
            NPC.noTileCollide = false;
            if(!NPC.collideY)
                NPC.velocity.Y -= 0.2f;
            StarOwner.drawPlayer.SetCompositeArmFront(false, Player.CompositeArmStretchAmount.Full, 0f);
            NPC.ai[0]++;
            NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
            if (NPC.velocity.X < 1 && NPC.velocity.X > -1)
                NPC.velocity.X += Math.Sign(Target.Center.X - NPC.Center.X);
            else
                NPC.velocity.X *= 0.99f;
            float disY = NPC.Center.Y - Target.Center.Y;
            //Main.NewText(disY);
            if (NPC.collideX)
            {
                NPC.velocity.Y = -7;
            }
            if(disY > 64)
            {
                NPC.velocity.Y = -disY * 0.2f;
                NPC.noTileCollide = true;
            }
            else if (disY < -300) // 强制下降
            {
                NPC.position.Y += 16;
                NPC.velocity.Y = 0;
            }
            try
            {
                Point point = (NPC.Center / 16).ToPoint();
                if (Main.tile.Width > point.X && Main.tile.Height > point.Y)
                {
                    if (Main.tile[point].HasTile)
                    {
                        NPC.position.Y -= 8;
                        NPC.noTileCollide = true;
                    }
                }
            }
            catch { }
            NPC.rotation = 0;
            base.AI();
            if (NPC.ai[0] > 25)
                NPC.ai[0] = 10;
            if (StarOwner.IsPhase(2)) // 二阶段时
            {
                if (disY > 64 && !NPC.collideY)
                {
                    NPC.velocity.Y = -0.001f;
                    NPC.noTileCollide = false;
                    NPC.collideY = true;
                }
            }
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > 20;
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            NPC.dontTakeDamage = false;
            NPC.boss = true;
            StarOwner.SceneEffectPriority = SceneEffectPriority.BossHigh;
            StarOwner.Music = MusicLoader.GetMusicSlot("StarOwner/Assets/Music/Boss1");
        }
    }
}
