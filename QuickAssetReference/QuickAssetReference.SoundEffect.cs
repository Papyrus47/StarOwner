using ReLogic.Content;
using Microsoft.Xna.Framework.Audio;

namespace StarOwner.QuickAssetReference;
public static class ModAssets_SoundEffect
{
    public static class Music
    {
        public static Asset<SoundEffect> Boss1Asset => ModAssets_Utils.Mod.Assets.Request<SoundEffect>(Boss1Path);
        public static Asset<SoundEffect> Boss1ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<SoundEffect>(Boss1Path, AssetRequestMode.ImmediateLoad);
        public const string Boss1Path = "Assets/Music/Boss1";
        public static Asset<SoundEffect> Ropocalypse2Asset => ModAssets_Utils.Mod.Assets.Request<SoundEffect>(Ropocalypse2Path);
        public static Asset<SoundEffect> Ropocalypse2ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<SoundEffect>(Ropocalypse2Path, AssetRequestMode.ImmediateLoad);
        public const string Ropocalypse2Path = "Assets/Music/Ropocalypse2";
        public static Asset<SoundEffect> StarGroundMusicAsset => ModAssets_Utils.Mod.Assets.Request<SoundEffect>(StarGroundMusicPath);
        public static Asset<SoundEffect> StarGroundMusicImmediateAsset => ModAssets_Utils.Mod.Assets.Request<SoundEffect>(StarGroundMusicPath, AssetRequestMode.ImmediateLoad);
        public const string StarGroundMusicPath = "Assets/Music/StarGroundMusic";
    }

}

