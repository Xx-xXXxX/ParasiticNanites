using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace ParasiticNanites.Items
{
	public class ParasiticNanitesItem:ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parasitic Nanites");
            DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese,"寄生机器人");
            Tooltip.SetDefault("Parasitic On Living Things----including yourself\nNo...");
            Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生在生物上——包括你\n别...");

        }
        public override void SetDefaults()
		{
            item.damage = 1;
            item.knockBack = 0f;
            item.rare = 10;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.width = 18;
            item.height = 18;
            item.scale = 1f;
            item.maxStack = 999; 
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType< Projectiles.ParasiticNanitesProj>();
            item.shootSpeed = 10f;
            item.createTile = ModContent.TileType<Tiles.ParasiticNanitesGrass>();
        }
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Nanites, 4);
            recipe.AddIngredient(ItemID.ExplosivePowder, 4);
            recipe.AddIngredient(ItemID.Wire,4);
            recipe.AddIngredient(ItemID.JungleSpores, 4);
            recipe.AddIngredient(ItemID.HallowedBar,4);
            recipe.AddTile(TileID.LihzahrdFurnace);
            recipe.SetResult(this);
            recipe.AddRecipe();
            
        }
	}
}
