using StarOwner.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarOwner.Core.ModPlayers
{
    public class StarOwnerWeaponDamageAddHurtToDecreaseDmgAdd : ModPlayer
    {
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Player.HeldItem.ModItem is IDamageAdd damageAdd && damageAdd.DamageAdd >= 0.1f)
                damageAdd.DamageAdd -= 0.1f;
        }
    }
}
