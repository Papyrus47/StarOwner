using StarOwner.Content.Subworld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.GlobalNPC
{
    public class SpawnGlobalNPC : Terraria.ModLoader.GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if(SubworldLibrary.SubworldSystem.IsActive<StarGround>())
                maxSpawns = 0;
        }
    }
}
