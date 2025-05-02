using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace StarOwner
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class StarOwner : Mod
	{
        public override string Name => "StarOwner";
        public static StarOwner Instance;
        public static ModKeybind DefAtk;
        public static ModKeybind DodgeDamage;
        public override void Load()
        {
            Instance = this;
            DefAtk = KeybindLoader.RegisterKeybind(this, "DefaultAttack", Microsoft.Xna.Framework.Input.Keys.F);
            DodgeDamage = KeybindLoader.RegisterKeybind(this, "DodgeAttack", Microsoft.Xna.Framework.Input.Keys.X);
            //MusicLoader.AddMusic(this, "Assets/Music/Boss1");
            //MusicLoader.AddMusic(this, "Assets/Music/StarGroundMusic");
            //MusicLoader.AddMusic(this, "Assets/Music/Ropocalypse2");
        }
        public override void PostSetupContent()
        {
            //DefAtk = KeybindLoader.RegisterKeybind(this, "DefaultAttack", Microsoft.Xna.Framework.Input.Keys.F);
            //DodgeDamage = KeybindLoader.RegisterKeybind(this, "DodgeAttack", Microsoft.Xna.Framework.Input.Keys.X);
        }
        public override void Unload()
        {
            base.Unload();
            DefAtk = null;
            Instance = null;
            DodgeDamage = null;
        }
    }
}
