using ParasiticNanites.Buffs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
namespace ParasiticNanites.NPCs
{
	public class ParasiticNanitesGlobalNPC:GlobalNPC
	{
		public override bool PreNPCLoot(NPC npc)
		{
			if (npc.HasBuff(ModContent.BuffType<Buffs.ParasiticNanitesBuff>()))
			{
				int ParasiticNanitesIndex = npc.FindBuffIndex(ModContent.BuffType<ParasiticNanitesBuff>());
				Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(npc.Center, npc.buffTime[ParasiticNanitesIndex], npc.noGravity, npc.whoAmI + 1);
			}
			return true;
		}
	}
}
