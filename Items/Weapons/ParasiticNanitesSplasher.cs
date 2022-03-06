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

namespace ParasiticNanites.Items.Weapons
{
	public class ParasiticNanitesSplasher:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Splasher");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人喷洒者");
			Tooltip.SetDefault("Splash a lot of Parasitic Nanites");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "喷洒寄生机器人");
        }
        public override void SetDefaults()
        {
            item.damage = 150;
            item.knockBack = 0f;
            item.rare = ItemRarityID.Cyan;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.UseSound = SoundID.Item36;
            item.width = 52;
            item.height = 22;
            item.scale = 0.85f;
            item.maxStack = 1;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<Projectiles.ParasiticNanitesRocket>();
            item.shootSpeed = 8f;
            item.ranged = true;
            item.useAmmo = AmmoID.Rocket;
        }
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 10);
            recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSignalTransmitter>(), 3);
            recipe.AddIngredient(ItemID.HallowedBar, 20);
            recipe.AddIngredient(ItemID.BeetleHusk, 15);
            recipe.AddIngredient(ItemID.LihzahrdBrick, 15);
            recipe.AddIngredient(ItemID.RocketLauncher, 1);
            recipe.AddIngredient(ItemID.Wire, 25);
            recipe.AddTile(TileID.LihzahrdFurnace);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            type= ModContent.ProjectileType<Projectiles.ParasiticNanitesRocket>();

            return true;
		}
	}
}
