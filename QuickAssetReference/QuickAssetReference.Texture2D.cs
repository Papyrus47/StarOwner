using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StarOwner.QuickAssetReference;
public static class ModAssets_Texture2D
{
    public static Asset<Texture2D> iconAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(iconPath);
    public static Asset<Texture2D> iconImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(iconPath, AssetRequestMode.ImmediateLoad);
    public const string iconPath = "icon";
    public static class Assets
    {
        public static class Images
        {
            public static Asset<Texture2D> PerlinAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(PerlinPath);
            public static Asset<Texture2D> PerlinImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(PerlinPath, AssetRequestMode.ImmediateLoad);
            public const string PerlinPath = "Assets/Images/Perlin";
            public static Asset<Texture2D> StarsAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarsPath);
            public static Asset<Texture2D> StarsImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarsPath, AssetRequestMode.ImmediateLoad);
            public const string StarsPath = "Assets/Images/Stars";
            public static class ColorMap
            {
                public static Asset<Texture2D> ColorMap_0Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ColorMap_0Path);
                public static Asset<Texture2D> ColorMap_0ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ColorMap_0Path, AssetRequestMode.ImmediateLoad);
                public const string ColorMap_0Path = "Assets/Images/ColorMap/ColorMap_0";
                public static Asset<Texture2D> ColorMap_1Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ColorMap_1Path);
                public static Asset<Texture2D> ColorMap_1ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ColorMap_1Path, AssetRequestMode.ImmediateLoad);
                public const string ColorMap_1Path = "Assets/Images/ColorMap/ColorMap_1";
                public static Asset<Texture2D> ColorMap_2Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ColorMap_2Path);
                public static Asset<Texture2D> ColorMap_2ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ColorMap_2Path, AssetRequestMode.ImmediateLoad);
                public const string ColorMap_2Path = "Assets/Images/ColorMap/ColorMap_2";
            }

            public static class Extra
            {
                public static Asset<Texture2D> Extra_0Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_0Path);
                public static Asset<Texture2D> Extra_0ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_0Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_0Path = "Assets/Images/Extra/Extra_0";
                public static Asset<Texture2D> Extra_1Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_1Path);
                public static Asset<Texture2D> Extra_1ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_1Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_1Path = "Assets/Images/Extra/Extra_1";
                public static Asset<Texture2D> Extra_2Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_2Path);
                public static Asset<Texture2D> Extra_2ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_2Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_2Path = "Assets/Images/Extra/Extra_2";
                public static Asset<Texture2D> Extra_3Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_3Path);
                public static Asset<Texture2D> Extra_3ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_3Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_3Path = "Assets/Images/Extra/Extra_3";
                public static Asset<Texture2D> Extra_4Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_4Path);
                public static Asset<Texture2D> Extra_4ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_4Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_4Path = "Assets/Images/Extra/Extra_4";
                public static Asset<Texture2D> Extra_5Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_5Path);
                public static Asset<Texture2D> Extra_5ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_5Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_5Path = "Assets/Images/Extra/Extra_5";
                public static Asset<Texture2D> Extra_6Asset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_6Path);
                public static Asset<Texture2D> Extra_6ImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(Extra_6Path, AssetRequestMode.ImmediateLoad);
                public const string Extra_6Path = "Assets/Images/Extra/Extra_6";
            }

        }

    }

    public static class Content
    {
        public static class Items
        {
            public static Asset<Texture2D> BrokenStarsSlashAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarsSlashPath);
            public static Asset<Texture2D> BrokenStarsSlashImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarsSlashPath, AssetRequestMode.ImmediateLoad);
            public const string BrokenStarsSlashPath = "Content/Items/BrokenStarsSlash";
            public static Asset<Texture2D> BrokenStarsSlashProjAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarsSlashProjPath);
            public static Asset<Texture2D> BrokenStarsSlashProjImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarsSlashProjPath, AssetRequestMode.ImmediateLoad);
            public const string BrokenStarsSlashProjPath = "Content/Items/BrokenStarsSlashProj";
            public static class VisibleArmors
            {
                public static Asset<Texture2D> StarOwnerBodyAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBodyPath);
                public static Asset<Texture2D> StarOwnerBodyImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBodyPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerBodyPath = "Content/Items/VisibleArmors/StarOwnerBody";
                public static Asset<Texture2D> StarOwnerBodyShowAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBodyShowPath);
                public static Asset<Texture2D> StarOwnerBodyShowImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBodyShowPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerBodyShowPath = "Content/Items/VisibleArmors/StarOwnerBodyShow";
                public static Asset<Texture2D> StarOwnerBodyShow_BodyAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBodyShow_BodyPath);
                public static Asset<Texture2D> StarOwnerBodyShow_BodyImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBodyShow_BodyPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerBodyShow_BodyPath = "Content/Items/VisibleArmors/StarOwnerBodyShow_Body";
                public static Asset<Texture2D> StarOwnerBody_BodyAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBody_BodyPath);
                public static Asset<Texture2D> StarOwnerBody_BodyImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerBody_BodyPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerBody_BodyPath = "Content/Items/VisibleArmors/StarOwnerBody_Body";
                public static Asset<Texture2D> StarOwnerHeadAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerHeadPath);
                public static Asset<Texture2D> StarOwnerHeadImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerHeadPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerHeadPath = "Content/Items/VisibleArmors/StarOwnerHead";
                public static Asset<Texture2D> StarOwnerHead_HeadAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerHead_HeadPath);
                public static Asset<Texture2D> StarOwnerHead_HeadImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerHead_HeadPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerHead_HeadPath = "Content/Items/VisibleArmors/StarOwnerHead_Head";
                public static Asset<Texture2D> StarOwnerLegsAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerLegsPath);
                public static Asset<Texture2D> StarOwnerLegsImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerLegsPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerLegsPath = "Content/Items/VisibleArmors/StarOwnerLegs";
                public static Asset<Texture2D> StarOwnerLegs_LegsAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerLegs_LegsPath);
                public static Asset<Texture2D> StarOwnerLegs_LegsImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerLegs_LegsPath, AssetRequestMode.ImmediateLoad);
                public const string StarOwnerLegs_LegsPath = "Content/Items/VisibleArmors/StarOwnerLegs_Legs";
            }

        }

        public static class NPCs
        {
            public static Asset<Texture2D> StarOwnerNPCAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerNPCPath);
            public static Asset<Texture2D> StarOwnerNPCImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarOwnerNPCPath, AssetRequestMode.ImmediateLoad);
            public const string StarOwnerNPCPath = "Content/NPCs/StarOwnerNPC";
            public static class Skills
            {
                public static class General
                {
                    public static class BrokenStarsSlash
                    {
                        public static Asset<Texture2D> BrokenStarsSlashAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarsSlashPath);
                        public static Asset<Texture2D> BrokenStarsSlashImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarsSlashPath, AssetRequestMode.ImmediateLoad);
                        public const string BrokenStarsSlashPath = "Content/NPCs/Skills/General/BrokenStarsSlash/BrokenStarsSlash";
                    }

                    public static class BrokenStarStick
                    {
                        public static Asset<Texture2D> BrokenStarStickAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarStickPath);
                        public static Asset<Texture2D> BrokenStarStickImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenStarStickPath, AssetRequestMode.ImmediateLoad);
                        public const string BrokenStarStickPath = "Content/NPCs/Skills/General/BrokenStarStick/BrokenStarStick";
                    }

                    public static class Guns
                    {
                        public static Asset<Texture2D> FearAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(FearPath);
                        public static Asset<Texture2D> FearImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(FearPath, AssetRequestMode.ImmediateLoad);
                        public const string FearPath = "Content/NPCs/Skills/General/Guns/Fear";
                        public static Asset<Texture2D> MadnessAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(MadnessPath);
                        public static Asset<Texture2D> MadnessImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(MadnessPath, AssetRequestMode.ImmediateLoad);
                        public const string MadnessPath = "Content/NPCs/Skills/General/Guns/Madness";
                    }

                    public static class SkinningAndBrokenBones
                    {
                        public static class Change
                        {
                            public static Asset<Texture2D> BrokenBonesChangeAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenBonesChangePath);
                            public static Asset<Texture2D> BrokenBonesChangeImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenBonesChangePath, AssetRequestMode.ImmediateLoad);
                            public const string BrokenBonesChangePath = "Content/NPCs/Skills/General/SkinningAndBrokenBones/Change/BrokenBonesChange";
                        }

                        public static class NoChange
                        {
                            public static Asset<Texture2D> BrokenBonesAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenBonesPath);
                            public static Asset<Texture2D> BrokenBonesImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BrokenBonesPath, AssetRequestMode.ImmediateLoad);
                            public const string BrokenBonesPath = "Content/NPCs/Skills/General/SkinningAndBrokenBones/NoChange/BrokenBones";
                            public static Asset<Texture2D> SkinningAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(SkinningPath);
                            public static Asset<Texture2D> SkinningImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(SkinningPath, AssetRequestMode.ImmediateLoad);
                            public const string SkinningPath = "Content/NPCs/Skills/General/SkinningAndBrokenBones/NoChange/Skinning";
                        }

                    }

                    public static class StarPierced
                    {
                        public static Asset<Texture2D> StarPiercedAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarPiercedPath);
                        public static Asset<Texture2D> StarPiercedImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarPiercedPath, AssetRequestMode.ImmediateLoad);
                        public const string StarPiercedPath = "Content/NPCs/Skills/General/StarPierced/StarPierced";
                    }

                }

            }

        }

        public static class Subworld
        {
            public static Asset<Texture2D> StarGroundAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarGroundPath);
            public static Asset<Texture2D> StarGroundImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StarGroundPath, AssetRequestMode.ImmediateLoad);
            public const string StarGroundPath = "Content/Subworld/StarGround";
        }

        public static class Tiles
        {
            public static Asset<Texture2D> TestTileAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TestTilePath);
            public static Asset<Texture2D> TestTileImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TestTilePath, AssetRequestMode.ImmediateLoad);
            public const string TestTilePath = "Content/Tiles/TestTile";
        }

    }

}

