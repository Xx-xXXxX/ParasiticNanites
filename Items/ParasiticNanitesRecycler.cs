using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ParasiticNanites.Items
{
	public class ParasiticNanitesRecycler:ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Parasitic Nanites Recycler");
            DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人回收装置");
            Tooltip.SetDefault("Break and recycle some Parasitic Nanites");
            Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "破坏并回收一些寄生机器人");
        }
		public override void SetDefaults()
        {
            item.damage = 0;
            item.knockBack = 0f;
            item.rare = ItemRarityID.Cyan;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.UseSound = SoundID.Item36;
            item.width = 24;
            item.height = 24;
            item.scale = 0.85f;
            item.maxStack = 1;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<ParasiticNanitesRecyclerProj>();
            item.shootSpeed = 8f;
            item.magic = true;
            item.mana = 20;
        }
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesItem>(), 5);
            recipe.AddIngredient(ModContent.ItemType<ParasiticNanitesSignalTransmitter>(), 2);
            recipe.AddIngredient(ItemID.HallowedBar, 15);
            recipe.AddIngredient(ItemID.BeetleHusk, 10);
            recipe.AddIngredient(ItemID.Wire, 25);
            recipe.AddTile(TileID.LihzahrdFurnace);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
