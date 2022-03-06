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

namespace ParasiticNanites.Items
{
	public class ParasiticNanitesSignalTransmitter:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites' Signal Transmitter");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人的信号发射器");
			Tooltip.SetDefault(
				"Really it emits signals?\n" +
				"And really what it emits is signals?" +
				"How do Parasitic Nanites respond to that?\n" +
				"And how did Parasitic Nanites do it?");
			Tooltip.AddTranslation(Terraria.Localization.GameCulture.Chinese, "这玩意真的可以发射信号？\n"+
				"它发出的真的是信号吗？"+
				"寄生机器人怎么会对此有反应？\n" +
				"寄生机器人又是怎么做到的？");
		}
		public override void SetDefaults()
		{
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 10;
			item.width = 16;
			item.height = 32;
            item.maxStack = 99;
		}
	}
}
