using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
namespace ParasiticNanites.Items.Accessories
{
	public class ParasiticNanitesTNTExtension:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites TNT Extension");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人炸药扩展");
			Tooltip.SetDefault("Causes parasitic droids to explode when disappearing.\nGlobal effect.Effective when you alive----you have to alive");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "使寄生机器人在消失时爆炸。\n全局有效。你活着时有效——你得先活着");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Cyan;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ParasiticNanites.ProjBoom = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 5);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSignalTransmitter>(), 1);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddIngredient(ItemID.ExplosivePowder, 100);
			recipe.AddIngredient(ItemID.Wire, 25);
			recipe.AddIngredient(ItemID.BeetleHusk, 5);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
