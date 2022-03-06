using Terraria.ModLoader;
using Terraria.ID;

namespace ParasiticNanites.Items.Placeable 
{
	public class ParasiticNanitesStone : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("This is a modded Stone block.");
		}

		public override void SetDefaults() {
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.ParasiticNanitesStone>();
			//item.ammo = AmmoID.Sand; Using this Sand in the Sandgun would require PickAmmo code and changes to ExampleSandProjectile or a new ModProjectile.
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(Terraria.ID.ItemID.DirtBlock);
			recipe.SetResult(this, 10);
			recipe.AddRecipe();
		}
	}
}