using StarOwner.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using static StarOwner.Content.NPCs.StarOwnerNPC;

namespace StarOwner.Core.Systems
{
    public class BossAndDownedSystem : ModSystem
    {
        public static int StarOwnerIndex;
        public static bool downedStarOwner;
        public override void PostUpdateNPCs()
        {
            StarOwnerIndex = -1;
            foreach (NPC n in Main.npc)
            {
                if (n.type == ModContent.NPCType<Content.NPCs.StarOwnerNPC>() && n.active)
                {
                    StarOwnerIndex = n.whoAmI;
                    break;
                }
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add(nameof(downedStarOwner), downedStarOwner);
            tag.Add(nameof(Phase1Said), Phase1Said);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            downedStarOwner = tag.GetBool(nameof(downedStarOwner));
            Phase1Said = tag.GetBool(nameof(Phase1Said));
        }
    }
}
