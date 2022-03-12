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



// 注意这里命名空间变了，多了个.Items
// 因为这个文件在Items文件夹，而读取图片的时候是根据命名空间读取的，如果写错了可能图片就读不到了
// 跟那把剑一样，后面我就不说了
namespace ParasiticNanites.Items.Tests
{
    // 保证类名跟文件名一致，这样也方便查找
    public class ParasiticNanitesProjTest : ModItem
    {
        // 设置物品名字，描述的地方
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("ParasiticNanitesProjTest");
            Tooltip.SetDefault("TestParasiticNanitesProj,Test Item");
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
            item.shoot = 1;
            item.shootSpeed = 1f;
        }
		public override void HoldItem(Player player)
		{
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"\n\n{ParasiticNanites.ParasiticNanitesProjNum}\n{ParasiticNanites.ParasiticNanitesProjMaxNum}\nN:{ParasiticNanites.AdjustParasiticNanitesNum},L:{ParasiticNanites.AdjustParasiticNanitesLength}\n{ParasiticNanites.AdjustParasiticNanitesNow}\n{ParasiticNanites.SeparateParasiticNanitesMinNum} N:{ParasiticNanites.SeparateParasiticNanitesNum}",Main.MouseWorld);
		}

		// 控制这把枪使用时候的重写函数
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //ParasiticNanitesProj.SummonSomeParasiticNanites(Main.MouseWorld,100,true,action:(i)=> { NetMessage.SendData(MessageID.SyncProjectile, number: i); });
            XxDefinitions.Utils.SummonUtils.SummonDustExplosion(Main.MouseWorld, 64, 0, 0, ModContent.DustType<Dusts.ParasiticNanitesDust>(), 4, 4, 4);

            return false;
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
