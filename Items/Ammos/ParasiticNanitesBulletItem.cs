using ParasiticNanites.Projectiles;

using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ParasiticNanites.Items.Ammos
{
	public class ParasiticNanitesBulletItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet with Parasitic Nanites");
			DisplayName.AddTranslation(GameCulture.Chinese, "带寄生机器人的子弹");
			Tooltip.SetDefault("attach some Parasitic Nanites to the target when hitting\nthe number of Parasitic Nanites depends on the damage");
			Tooltip.AddTranslation(GameCulture.Chinese, "击中附着寄生机器人\n寄生机器人数量取决于伤害");
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
			item.shoot = ModContent.ProjectileType<ParasiticNanitesBullet>();   //The projectile shoot when your weapon using this ammo
			item.ammo = AmmoID.Bullet;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
