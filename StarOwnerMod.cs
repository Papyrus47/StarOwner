using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace StarOwner
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class StarOwnerMod : Mod
	{
        public static StarOwnerMod Instance;
        public override void Load()
        {
            Instance = this;
            MusicLoader.AddMusic(this, "Assets/Music/StarGroundMusic");
            MusicLoader.AddMusic(this, "Assets/Music/Ropocalypse2");
        }
        public override void Unload()
        {
            base.Unload();
            Instance = null;
        }
    }
}
