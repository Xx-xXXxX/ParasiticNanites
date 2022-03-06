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
	public class ParasiticNanitesSpikeExtension:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Spike Extension");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人尖刺扩展");
			Tooltip.SetDefault("Spines Parasitic Nanites, causing higher impact damage,can damage immune entities.\nGlobal effect.Effective when you alive");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "使寄生机器人带刺，造成更高碰撞伤害，可以伤害有免疫的生物。\n全局有效。你活着时有效");
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
			ParasiticNanites.ProjSpike = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 2);
			recipe.AddIngredient(ItemID.Spike, 100);
			recipe.AddIngredient(ItemID.WoodenSpike, 25);
			recipe.AddIngredient(ItemID.Wire, 15);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
