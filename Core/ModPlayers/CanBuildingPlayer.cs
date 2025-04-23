using StarOwner.Content.Subworld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.ModPlayers
{
    public class CanBuildingPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            if(SubworldLibrary.SubworldSystem.IsActive<StarGround>())
                Player.noBuilding = true;
        }
    }
}
