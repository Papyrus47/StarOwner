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
	public class BrokenStarsSlash : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.StarOwner.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.width = 76;
			Item.height = 74;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(1);
			Item.rare = ItemRarityID.Master;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<BrokenStarsSlashProj>();
			Item.shootSpeed = 10;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!SubworldSystem.IsActive<StarGround>())
                SubworldSystem.Enter<StarGround>();
            if (player.ownedProjectileCounts[type] >= 1)
				return false;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
