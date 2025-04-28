using StarOwner.Content.Subworld;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarOwner.Content.Items
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class EnterStarGround : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.StarOwner.hjson' file.
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 62;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(1);
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
        public override bool CanUseItem(Player player) => !SubworldSystem.IsActive<StarGround>();
        public override bool? UseItem(Player player)
        {
            if (!SubworldSystem.IsActive<StarGround>())
                SubworldSystem.Enter<StarGround>();
            return false;
        }
        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
    }
}
