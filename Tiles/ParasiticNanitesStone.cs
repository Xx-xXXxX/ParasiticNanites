using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ParasiticNanites.Dusts;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticNanites.Tiles
{
	public class ParasiticNanitesStone:ModTile
	{
		public override void SetDefaults()
		{
			ParasiticNanites.ParasiticNanitesTiles.Add(Type);
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileStone[Type] = true;
			TileID.Sets.Stone[Type] =true;
			TileID.Sets.Conversion.Stone[Type] = true; // Allows Clentaminator solutions to convert this tile to their respective Sand tiles.
			//TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true; // Allows Sandshark enemies to "swim" in this sand.
			TileID.Sets.Falling[Type] = false;
			AddMapEntry(new Color(63, 31, 31));
			//Set the dust type to Sparkle
			dustType = ModContent.DustType<ParasiticNanitesDust>();
			//Drop the ExampleSandBlock
			drop = Terraria.ID.ItemID.StoneBlock;
		}
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override bool HasWalkDust()
		{
			return false;
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (Main.rand.Next(0, 90) == 10)
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<ParasiticNanitesDust>());

		}
		public override void RandomUpdate(int i, int j)
		{
			if (!Main.tile[i, j - 1].active())
			{
				Projectiles.ParasiticNanitesProj.NewParasiticNanitesProj(new Vector2(i * 16, (j - 1) * 16), 5, false, speed: 3);
				return;
			}
			if (!Main.tile[i - 1, j].active())
			{
				Projectiles.ParasiticNanitesProj.NewParasiticNanitesProj(new Vector2((i - 1) * 16, (j) * 16), 5, false, speed: 3);
				return;
			}
			if (!Main.tile[i + 1, j].active())
			{
				Projectiles.ParasiticNanitesProj.NewParasiticNanitesProj(new Vector2((i + 1) * 16, (j) * 16), 5, false, speed: 3);
				return;
			}
			if (!Main.tile[i, j + 1].active())
			{
				Projectiles.ParasiticNanitesProj.NewParasiticNanitesProj(new Vector2(i * 16, (j + 1) * 16), 5, false, speed: 3);
				return;
			}
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if(!fail)
				Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(new Vector2(i * 16 + 8, j * 16 + 8),10,false);
		}


		public Point GetPNDXY(int x, int y)
		{
			//return new Point(XxDefinitions.Utils.LimitLoop( x * 16,0,Effects.ParasiticNanitesDraw.Width-1), y * 16);
			return Effects.ParasiticNanitesDrawTiles.PointLimit(new Point(x * 16, y * 16));
		}
		public Point GetPNDOrigin(int x, int y)
		{
			Tile t = Main.tile[x, y];
			return new Point(t.frameX, t.frameY);
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			XxDefinitions.Utils.SpriteBatchUsingEffect(spriteBatch);
			Effects.ParasiticNanitesDrawTiles.UseEffect(GetPNDXY(i, j), new Point(288, 270), GetPNDOrigin(i, j), Terraria.Lighting.GetColor(i, j));
			return true;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			XxDefinitions.Utils.SpriteBatchEndUsingEffect(spriteBatch);
		}
	}
}
