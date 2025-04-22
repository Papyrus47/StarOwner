using StarOwner.Content.NPCs;
using StarOwner.Core.Systems;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace StarOwner.Content.Subworld
{
    public class StarGround : SubworldLibrary.Subworld
    {
        public override int Width => 1065;

        public override int Height => 650 + 200;
        public override bool ShouldSave => false;
        public override bool NoPlayerSaving => true;
        public override List<GenPass> Tasks => new()
        {
            new SummonGround()
        };
        public override void OnLoad()
        {
            Main.dayTime = false;
            Main.time = 1000;
            Main.spawnTileX = Width - 20;
            Main.spawnTileY += 100;
            Main.worldSurface = Height;
        }
        public override void Update()
        {
            base.Update();
            Main.dayTime = false;
            Main.time = 1000;
            Player player = Main.player[Main.myPlayer];
            player.wingTimeMax = 0;
            player.wingTime = 0;
            player.ZoneUndergroundDesert = false;
            player.ZonePurity = false;
            player.ZoneUnderworldHeight = false;
            player.ZoneBeach = false;
            if (player.dead)
            {
                SubworldSystem.Exit();
            }
            // Main.NewText(player.position.ToString());
            if (!BossAndDownedSystem.downedStarOwner && BossAndDownedSystem.StarOwnerIndex == -1 && player.position.Y > 4300 && player.position.Y < 4600 && player.position.X > 12200 && player.position.X < 12310) // 召唤Boss
            {
                NPC.NewNPCDirect(player.GetSource_FromThis(), 11214, 4326, ModContent.NPCType<NPCs.StarOwnerNPC>());
            }
            else if (BossAndDownedSystem.StarOwnerIndex != -1)
            {
                if(player.position.X < 11102)
                {
                    player.velocity.X = 2;
                }
                else if(player.position.X > 15036)
                {
                    player.velocity.X = -2;
                }
            }
        }
        public class SummonGround : GenPass
        {
            public SummonGround() : base("Star Ground", 1) { }

            public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Summoning Star Ground...";
                Main.worldSurface = 0;
                Main.rockLayer = Main.maxTilesY;
                Main.worldSurface = Main.maxTilesY;
                Texture2D tileTex = ModAsset.StarGround.Value;
                Color[] tileData = new Color[tileTex.Width * tileTex.Height];

                Main.QueueMainThreadAction(() =>
                {
                    tileTex.GetData(0, tileTex.Bounds, tileData, 0, tileTex.Width * tileTex.Height);
                    for (int X = 0; X < Main.maxTilesX; X++)
                    {
                        for (int Y = 200; Y < Main.maxTilesY; Y++)
                        {
                            int y = Y - 200;
                            if (tileData[X + y * tileTex.Width].R == 190) // 粉红色
                            {
                                Main.tile[X, Y].TileType = TileID.Dirt;
                                WorldGen.PlaceTile(X, Y, TileID.Dirt, true);
                            }
                            else if (tileData[X + y * tileTex.Width].R == 107) // 暗紫色
                            {
                                Main.tile[X, Y].TileType = TileID.Stone;
                                WorldGen.PlaceTile(X, Y, TileID.Stone, true);
                            }
                            else if (tileData[X + y * tileTex.Width].R == 194) // 亮粉色,墙
                            {
                                Main.tile[X, Y].WallType = WallID.Stone;
                                WorldGen.PlaceWall(X, Y, WallID.Stone, true);
                            }
                        }
                    }
                });
            }
        }
    }
}
