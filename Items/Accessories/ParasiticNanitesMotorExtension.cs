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
	public class ParasiticNanitesMotorExtension:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Move Extension");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人机动扩展");
			Tooltip.SetDefault("Parasitic Nanites can trace the target what can be infected");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人会追踪可以被感染的生物");
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
			ParasiticNanites.ProjChasing = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 5);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSignalTransmitter>(), 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 10);
			recipe.AddIngredient(ItemID.RocketIII, 15);
			recipe.AddIngredient(ItemID.Wire, 15);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
