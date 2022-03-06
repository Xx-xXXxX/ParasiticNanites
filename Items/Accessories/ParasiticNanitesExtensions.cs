using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ParasiticNanites.Items.Accessories
{
	public class ParasiticNanitesExtensions:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Extensions");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人扩展套");
			Tooltip.SetDefault("All the xtensions of Parasitic Nanites,and immunity");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "全部寄生机器人的扩展，以及免疫");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 40);
			item.rare = ItemRarityID.Cyan;
			item.defense = 10;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ParasiticNanites.ProjChasing = true;
			ParasiticNanites.ProjBoom = true;
			ParasiticNanites.ProjSpike = true;
			player.buffImmune[ModContent.BuffType<Buffs.ParasiticNanitesBuff>()] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 5);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSignalTransmitter>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesDefenser>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesMotorExtension>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSpikeExtension>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesTNTExtension>(), 1);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
