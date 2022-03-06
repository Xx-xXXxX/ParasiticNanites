using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
namespace ParasiticNanites.Items.Ammos
{
	public class ParasiticNanitesSolution : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black and Red Solution");
			DisplayName.AddTranslation(GameCulture.Chinese,"黑红溶液");
			Tooltip.SetDefault("Used by the Clentaminator"
				+ "\nSpreads the Parasitic Nanite\n"+
				"Parasitic Nanite terrain will spread in a special way, spread faster in special terrain(such as Corruption)");
			Tooltip.AddTranslation(GameCulture.Chinese, "由环境改造枪使用\n蔓延寄生机器人地形\n"+
				"寄生机器人地形会以特别的方式蔓延，在特殊地形中蔓延更快(比如腐化地形)");
		}

		public override void SetDefaults()
		{
			item.shoot = ModContent.ProjectileType<ParasiticNanitesSolutionProj>() - ProjectileID.PureSpray;
			item.ammo = AmmoID.Solution;
			item.width = 10;
			item.height = 12;
			item.value = Item.buyPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
			item.consumable = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GreenSolution, 10);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 1);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this, 10);
			recipe.AddRecipe();
		}
	}
}
