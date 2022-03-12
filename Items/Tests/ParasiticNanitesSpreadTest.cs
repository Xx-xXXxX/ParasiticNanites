/*
 * 这是一个基本枪械类武器的例子
 */

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ParasiticNanites.Projectiles;
using ParasiticNanites.Tiles;



// 注意这里命名空间变了，多了个.Items
// 因为这个文件在Items文件夹，而读取图片的时候是根据命名空间读取的，如果写错了可能图片就读不到了
// 跟那把剑一样，后面我就不说了
namespace ParasiticNanites.Items.Tests
{
    // 保证类名跟文件名一致，这样也方便查找
    public class ParasiticNanitesSpreadTest : ModItem
    {
        // 设置物品名字，描述的地方
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("ParasiticNanitesSpreadTest");
            Tooltip.SetDefault("ParasiticNanitesSpreadTest Test Item");
        }

        // 最最最重要的物品基本属性部分
        public override void SetDefaults()
        {
            item.damage = 10;
            item.knockBack = 0f;
            item.rare = 2;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 5;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.UseSound = SoundID.Item36;
            item.width = 24;
            item.height = 24;
            item.scale = 0.85f;
            item.maxStack = 1;
            item.noMelee = true;
        }
        public Point P => (Main.MouseWorld / 16).ToPoint();

        public override void HoldItem(Player player)
		{
            //XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"\n\n{ParasiticNanites.ParasiticNanitesProjNum}\n{ParasiticNanites.ParasiticNanitesProjMaxNum}",Main.MouseWorld);
            Rectangle rect = new Rectangle(P.X*16, P.Y * 16, 16, 16);
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{((Main.tile[P.X,P.Y]==null)?("nothing"):(Main.tile[P.X, P.Y].type.ToString()))}",Main.MouseWorld+Vector2.One*16);
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawRect(rect);
		}
		public override bool UseItem(Player player)
		{
            Tile t = Main.tile[P.X, P.Y];
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawRect(new Rectangle((P.X - 5) * 16, (P.Y - 5) * 16, 16 * 11, 16 * 11), DrawTime: 60);
            int Count = Tiles.ParasiticNanitesTile.CountParasiticNanitesTile(P.X, P.Y);
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{ParasiticNanites.ParasiticNanitesTiles.Contains(t.type)},{Count.ToString()}", new Vector2(P.X * 16, P.Y * 16), 60);
            Tiles.ParasiticNanitesTile.ParasiticNanitesTileSpread(P.X,P.Y,t.type);
            return true;
		}
		// 物品合成表的设置部分
		// 我没有写，这部分由你写
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            // 合成材料，需要10个泥土块
            recipe.AddIngredient(ItemID.DirtBlock, 0);
            // 以及在工作台旁边
            recipe.AddTile(TileID.WorkBenches);
            // 生成1个这种物品
            recipe.SetResult(this);
            // 这样可以生成50个
            // recipe.SetResult(this, 50);

            // 把这个合成表装进tr的系统里
            recipe.AddRecipe();
        }
    }
}
