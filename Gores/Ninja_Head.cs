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

namespace ParasiticNanites.Gores
{
	public class Ninja_Head:ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			//Terraria.GameContent.ChildSafety.SafeGore[gore.type] = true;
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
			updateType = GoreID.KingSlimeCrown;
		}
	}
}
