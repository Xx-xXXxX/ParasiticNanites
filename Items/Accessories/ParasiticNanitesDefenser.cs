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
	public class ParasiticNanitesDefenser:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Defenser");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人防御装置");
			Tooltip.SetDefault("Immune Parasitic Nanites parasitizing you----ONLY parasitizing");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "免疫寄生机器人寄生——只免疫寄生");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Cyan;
			item.defense = 10;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[ModContent.BuffType<Buffs.ParasiticNanitesBuff>()] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 5);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSignalTransmitter>(), 1);
			recipe.AddIngredient(ItemID.HallowedBar, 15);
			recipe.AddIngredient(ItemID.BeetleHusk, 10);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddIngredient(ItemID.Wire, 25);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
