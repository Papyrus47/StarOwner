using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Content.Items.VisibleArmors
{
    [AutoloadEquip(EquipType.Head)]
    public class StarOwnerHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            int slot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[slot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 18; // Width of the item
            Item.height = 18; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = ItemRarityID.Master; // The rarity of the item
            Item.defense = 6; // The amount of defense the item will give when equipped
        }
    }
}
