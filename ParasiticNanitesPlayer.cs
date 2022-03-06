using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ParasiticNanites.Buffs;

namespace ParasiticNanites
{
	public class ParasiticNanitesPlayer:ModPlayer
	{
		public override void PostUpdate()
		{
			/*
			bool adj=false;
			foreach (var i in ParasiticNanites.ParasiticNanitesTiles) {
				//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{player.adjTile[i]}", player.position - player.Size);
				if (player.adjTile[i]) { adj = true;break; }
			}
			//string TTData = $"{Main.time}\n{ParasiticNanites.ParasiticNanitesTiles.Count} {ModContent.TileType<Tiles.ParasiticNanitesSand>()}\n{adj}\n";
			//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString(TTData, player.position + player.Size);
			if (Main.time % 30 == 1)
			{
				if(adj)
					player.AddBuff(ModContent.BuffType< Buffs.ParasiticNanitesBuff>(), 10);
			}*/
			/*
			string adjs1 = "";
			for (int i =0; i < player.adjTile.Length; ++i)
			{
				if (player.adjTile[i])
					adjs1 += i + " ";
			}
			XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString(adjs1, Main.screenPosition + new Vector2(16, 256));

			string adjs2 = "";
			for (int i = ModContent.TileType<Tiles.ParasiticNanitesSand>()-10; i < player.adjTile.Length; ++i) {
				if (player.adjTile[i])
					adjs2 += i + " ";
			}
			XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString(adjs2, Main.screenPosition+new Vector2(16,300));*/
		}
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if (player.HasBuff(ModContent.BuffType<ParasiticNanitesBuff>()))
			{
				int ParasiticNanitesIndex = player.FindBuffIndex(ModContent.BuffType<ParasiticNanitesBuff>());
				Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(player.Center, player.buffTime[ParasiticNanitesIndex], player.gravity == 0, -(player.whoAmI + 1));
			}
		}
	}
}
