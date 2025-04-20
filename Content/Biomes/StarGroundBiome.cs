using StarOwner.Content.Subworld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Capture;

namespace StarOwner.Content.Biomes
{
    public class StarGroundBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/StarGroundMusic");
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
        public override Color? BackgroundColor => Color.Purple;
        public override bool IsBiomeActive(Player player) => SubworldLibrary.SubworldSystem.IsActive<StarGround>();
        // 音乐优先级
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }
}
