using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;

using Terraria.ModLoader;
using ParasiticNanites.Tiles;
namespace ParasiticNanites.Tiles
{
	
	public class ParasiticNanitesTile:GlobalTile
	{
		public override void RandomUpdate(int i, int j, int type)
		{
			ParasiticNanitesTileSpread(i,j,type);
		}
		public static int CountParasiticNanitesTile(int i, int j) {
			int Count = 0;
			for (int ni = i - 5; ni <= i + 5; ++ni)
			{
				for (int nj = j - 5; nj <= j + 5; ++nj)
				{
					if (Main.tile[ni, nj].active() && ParasiticNanites.ParasiticNanitesTiles.Contains(Main.tile[ni, nj].type))
						if (Main.tile[ni, nj].type == ModContent.TileType<ParasiticNanitesGrass>())
						{
							Count += 6;
						}
						else
							Count += 1;
				}
			}
			return Count;
		}
		public static void ParasiticNanitesTileSpread(int i, int j, int type)
		{
			if (ParasiticNanites.ParasiticNanitesTiles.Contains(type))
			{
				int TurnToType = -1;
				if (TileID.Sets.Conversion.Stone[type]) TurnToType = TileID.Stone;
				else if (TileID.Sets.Conversion.Sand[type]) TurnToType = TileID.Sand;
				else if (TileID.Sets.Conversion.Grass[type]) TurnToType = TileID.Grass;
				if (TurnToType == -1) return;
				bool Willspread = true;
				//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{Count},{Willspread},{TurnToType}",new Microsoft.Xna.Framework.Vector2(i*16,j*16), ((Count == 0) ? (1) : (300)));
				//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawRect(new Microsoft.Xna.Framework.Rectangle((i - 5) * 16, (j - 5) * 16, 16 * 11, 16 * 11), DrawTime: ((Count == 0) ? (1) : (60)));
				if(Willspread)
				{
					//WorldGen.PlaceTile(i, j, TurnToType,forced:true);
					Main.tile[i, j].type = (ushort)TurnToType;
					WorldGen.SquareTileFrame(i, j, true);
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
			else
			{
				int TurnToType = -1;
				bool Special = false;
				if (TileID.Sets.Conversion.Stone[type]) { TurnToType = ModContent.TileType<ParasiticNanitesStone>();if (type != TileID.Stone) Special = true; }
				else if (TileID.Sets.Conversion.Sand[type]) { TurnToType = ModContent.TileType<ParasiticNanitesSand>(); if (type != TileID.Sand) Special = true; }
				else if (TileID.Sets.Conversion.Grass[type]) { TurnToType = ModContent.TileType<ParasiticNanitesGrass>(); if (type != TileID.Grass) Special = true; }
				if (TurnToType == -1) return;
				if (TileID.Sets.Hallow[type]) Special = true;
				if (TileID.Sets.Corrupt[type]) Special = true;
				if (TileID.Sets.Crimson[type]) Special = true;
				bool Willspread = false;
				int Count = CountParasiticNanitesTile(i,j);

				if (Special)
				{
					if (Count >= 8 && Count <= 100) Willspread = true;
				}
				else
				{
					if (Count>=30&&Count<=60) Willspread = true;
				}
				//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{Count},{Special},{Willspread},{TurnToType}", new Microsoft.Xna.Framework.Vector2(i * 16, j * 16),((Count==0)?(1):(300)));
				//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawRect(new Microsoft.Xna.Framework.Rectangle((i-5)*16,(j-5)*16,16*11,16*11), DrawTime:((Count == 0) ? (1) : (60)));
				if (Willspread) //Main.tile[i,j].
				{
					Main.tile[i, j].type = (ushort)TurnToType;
					WorldGen.SquareTileFrame(i, j, true);
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			
			}
		}
	}
}
