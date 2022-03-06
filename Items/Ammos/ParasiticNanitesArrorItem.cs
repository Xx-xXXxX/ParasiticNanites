using Microsoft.Xna.Framework;

using ParasiticNanites.Projectiles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ParasiticNanites.Items.Ammos
{
	public class ParasiticNanitesArrorItem:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arror with Parasitic Nanites");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "带寄生机器人的箭");
			Tooltip.SetDefault("Sputter Parasitic Nanites when hitting or disappearing\nthe number of Parasitic Nanites depends on the damage");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "击中或消失时溅射出寄生机器人\n寄生机器人数量取决于伤害");
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 10;
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<ParasiticNanitesArrow>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 8f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 50);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
