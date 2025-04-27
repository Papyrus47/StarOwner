using StarOwner.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace StarOwner.Content.NPCs.Skills
{
    public class StarOwnerSkills : NPCSkills
    {
        public StarOwnerNPC StarOwner => NPC.ModNPC as StarOwnerNPC;
        public Player Target => StarOwner.TargetPlayer;
        public StarOwnerSkills(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            UpdatePlayerFrame();
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            SkillTimeOut = false;
            NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            SkillTimeOut = false;
            //NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.PlayerRenderer.DrawPlayer(Main.Camera, StarOwner.drawPlayer, NPC.position, NPC.rotation, StarOwner.drawPlayer.fullRotationOrigin, 0, NPC.scale);
            return false;
        }
        public void UpdatePlayerFrame()
        {
            StarOwner.drawPlayer.fullRotationOrigin = new Vector2(NPC.width / 2, NPC.height);
            StarOwner.drawPlayer.velocity = NPC.velocity;
            StarOwner.drawPlayer.position = NPC.position;
            StarOwner.drawPlayer.direction = NPC.spriteDirection;
            StarOwner.drawPlayer.UpdateDyes();
            StarOwner.drawPlayer.PlayerFrame();
            StarOwner.drawPlayer.head = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerHead", EquipType.Head);
            StarOwner.drawPlayer.body = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerBody", EquipType.Body);
            if (StarOwner.IsPhase(2) || StarOwner.IsPhase(4))
                StarOwner.drawPlayer.body = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerBodyShow", EquipType.Body);
            StarOwner.drawPlayer.legs = EquipLoader.GetEquipSlot(global::StarOwner.StarOwner.Instance, "StarOwnerLegs", EquipType.Legs);
            if(StarOwner.IsPhase(3))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust obj = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.TheDestroyer);
                    obj.position = NPC.Center + new Vector2(NPC.spriteDirection * 6, -8);
                    obj.noGravity = true;
                    obj.velocity = NPC.velocity * -0.25f * i;
                    obj.scale = 1.1f;

                    obj = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.TheDestroyer);
                    obj.position = NPC.Center + new Vector2(NPC.spriteDirection == 1 ? -6 : -16, -8);
                    obj.noGravity = true;
                    obj.velocity = NPC.velocity * -0.25f * i;
                    obj.scale = 1.1f;
                }
            }    
        }
    }
}
