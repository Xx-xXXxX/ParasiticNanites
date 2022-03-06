using ParasiticNanites.Dusts;
using ParasiticNanites.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace ParasiticNanites.Tiles
{
	public class ParasiticNanitesSand : ModTile
	{
		public override void SetDefaults() {
			ParasiticNanites.ParasiticNanitesTiles.Add(Type);
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSand[Type] = true;
			TileID.Sets.TouchDamageSands[Type] = 15;
			TileID.Sets.Conversion.Sand[Type] = true; // Allows Clentaminator solutions to convert this tile to their respective Sand tiles.
			TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true; // Allows Sandshark enemies to "swim" in this sand.
			TileID.Sets.Falling[Type] = true;
			AddMapEntry(new Color(127, 63, 63));
			//Set the dust type to Sparkle
			dustType = ModContent.DustType<ParasiticNanitesDust>();
			//Drop the ExampleSandBlock
			drop = Terraria.ID.ItemID.SandBlock;
			
			//Make ExampleCactus able to grow on this tile
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			if (Main.rand.Next(0, 30) == 1) Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, dustType);

			if (WorldGen.noTileActions)
				return true;
			Tile above = Main.tile[i, j - 1];
			Tile below = Main.tile[i, j + 1];
			bool canFall = true;
			if (below == null || below.active())
				canFall = false;

			if (above.active() && (TileID.Sets.BasicChest[above.type] || TileID.Sets.BasicChestFake[above.type] || above.type == TileID.PalmTree || TileLoader.IsDresser(above.type)))
				canFall = false;

			if (canFall) {
				//Set the projectile type to ExampleSandProjectile
				int projectileType = ModContent.ProjectileType<ParasiticNanitesSandProjectile>();
				float positionX = i * 16 + 8;
				float positionY = j * 16 + 8;

				if (Main.netMode == NetmodeID.SinglePlayer) {
					Main.tile[i, j].ClearTile();
					int proj = Projectile.NewProjectile(positionX, positionY, 0f, 0.41f, projectileType, 10, 0f, Main.myPlayer);
					Main.projectile[proj].ai[0] = 1f;
					WorldGen.SquareTileFrame(i, j);
				}
				else if (Main.netMode == NetmodeID.Server) {
					Main.tile[i, j].active(false);
					bool spawnProj = true;

					for (int k = 0; k < 1000; k++) {
						Projectile otherProj = Main.projectile[k];

						if (otherProj.active && otherProj.owner == Main.myPlayer && otherProj.type == projectileType && Math.Abs(otherProj.timeLeft - 3600) < 60 && otherProj.Distance(new Vector2(positionX, positionY)) < 4f) {
							spawnProj = false;
							break;
						}
					}

					if (spawnProj) {
						int proj = Projectile.NewProjectile(positionX, positionY, 0f, 2.5f, projectileType, 10, 0f, Main.myPlayer);
						Main.projectile[proj].velocity.Y = 0.5f;
						Main.projectile[proj].position.Y += 2f;
						Main.projectile[proj].netUpdate = true;
					}

					NetMessage.SendTileSquare(-1, i, j, 1);
					WorldGen.SquareTileFrame(i, j);
				}
				return false;
			}
			
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override bool HasWalkDust()
		{
			return false;
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

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (Main.rand.Next(0, 90) == 10)
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<ParasiticNanitesDust>());

		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail)
				Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(new Vector2(i * 16 + 8, j * 16 + 8), 10, false);
		}


		public Point GetPNDXY(int x, int y)
		{
			//return new Point(XxDefinitions.Utils.LimitCircular( x * 16,0,Effects.ParasiticNanitesDraw.Width-1), y * 16);
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
			Effects.ParasiticNanitesDrawTiles.UseEffect(GetPNDXY(i, j), new Point(288, 270), GetPNDOrigin(i, j), Color.White);
			return true;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			XxDefinitions.Utils.SpriteBatchEndUsingEffect(spriteBatch);
		}
	}
}