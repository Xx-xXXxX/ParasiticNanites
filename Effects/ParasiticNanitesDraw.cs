using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
namespace ParasiticNanites.Effects
{
	public static class ParasiticNanitesDraw
	{
		//128*128
		//1/8 2048
		public static int Width = 256;
		public static int Height = 256;
		public static int TWidth => Width* SizePerPixel;
		public static int THeight => Height * SizePerPixel;
		public static int SizePerPixel = 2;
		public static Point[] PNPs = new Point[2048];
		public static Texture2D texture;
		public static Effect DrawPND;
		public static void SetDef(Mod mod)
		{
			PNPs = new Point[2048];
			for (int i = 0; i < PNPs.Length; ++i)
			{
				PNPs[i].X = Main.rand.Next(0, Width - 1);
				PNPs[i].Y = Main.rand.Next(0, Height - 1);
			}
			//texture = ModContent.GetTexture("ParasiticNanites/RandBR");
			texture = new Texture2D(Main.graphics.GraphicsDevice,TWidth,THeight);
			texture.SetData(Enumerable.Range(0, TWidth * THeight).Select(i => Color.Black).ToArray());
			DrawPND = mod.GetEffect("Effects/PND");
			DrawPND.Parameters["Nx"].SetValue(1);
			DrawPND.Parameters["Ny"].SetValue(1);
			DrawPND.Parameters["Tx"].SetValue(TWidth);
			DrawPND.Parameters["Ty"].SetValue(THeight);
			DrawPND.Parameters["Texture"].SetValue(texture);
		}
		public static int XYToI(int x, int y) {
			return y * Height*SizePerPixel + x;
		}
		public static void Update()
		{
			//Color[] Cols = Enumerable.Range(0, TWidth * THeight).Select(i => Color.Black).ToArray();
			Color[] Cols=new Color[TWidth*THeight];
			texture.GetData(Cols);
			texture = new Texture2D(Main.graphics.GraphicsDevice, TWidth, THeight, false, SurfaceFormat.Color);
			for (int i = 0; i < PNPs.Length; ++i) {

				ref Point P = ref PNPs[i];

				for (int j = 0; j < SizePerPixel; ++j)
				{
					for (int k = 0; k < SizePerPixel; ++k)
					{
						Cols[XYToI(P.X * SizePerPixel + j, P.Y * SizePerPixel + k)] = Color.Black;
					}
				}
			}
			for (int i = 0; i < PNPs.Length; ++i)
			{
				ref Point P = ref PNPs[i];

				switch (Main.rand.Next(0, 5))
				{
					case 0:
						P.X += 1;
						if (P.X >= Width) P.X = 0;
						break;
					case 1:
						P.X -= 1;
						if (P.X < 0) P.X = Width - 1;
						break;
					case 2:
						P.Y += 1;
						if (P.Y >= Height) P.Y = 0;
						break;
					case 3:
						P.Y -= 1;
						if (P.Y < 0) P.Y = Height - 1;
						break;
				}

				for (int j = 0; j < SizePerPixel; ++j)
				{
					for (int k = 0; k < SizePerPixel; ++k)
					{
						Cols[XYToI(P.X*SizePerPixel+j,P.Y * SizePerPixel + k)] = Color.Red;
					}
				}
			}
			texture.SetData(Cols);
			DrawPND.Parameters["Texture"].SetValue(texture);
		}
		public static void UseEffect(Point xy, Point Size, Point Origin, Color color)
		{
			DrawPND.Parameters["Nx"].SetValue(xy.X);
			DrawPND.Parameters["Ny"].SetValue(xy.Y);
			DrawPND.Parameters["Sx"].SetValue(Size.X);
			DrawPND.Parameters["Sy"].SetValue(Size.Y);
			DrawPND.Parameters["Ox"].SetValue(Origin.X);
			DrawPND.Parameters["Oy"].SetValue(Origin.Y);
			DrawPND.Parameters["dcolor"].SetValue(color.ToVector4() );
			//ParasiticNanites.Logging.
			DrawPND.CurrentTechnique.Passes["PND"].Apply();
		}
		public static void DrawEffect(SpriteBatch spriteBatch, Point xy, Point Size, Point Origin, Color dcolor, Action<SpriteBatch> DrawFunc)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			UseEffect(xy, Size, Origin, dcolor);
			DrawFunc(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		public static Point PointLimit(Point point) {
			point.X = XxDefinitions.Utils.LimitLoop(point.X,0,TWidth);
			point.Y = XxDefinitions.Utils.LimitLoop(point.Y, 0, THeight);
			return point;
		}
		public static Point GetRandPNDPoint() {
			Point point = new Point(Width , Height  );
			return new Point(Main.rand.Next(0,point.X), Main.rand.Next(0, point.Y));
		}
		/*
		public static Point GetRandPNDPoint(Point size)
		{
			return GetRandPNDPoint(size.X, size.Y);
		}
		public static Point GetRandPNDPoint(Vector2 size)
		{
			return GetRandPNDPoint(size.ToPoint());
		}
		public static Point GetRandPNDPoint(Entity entity)
		{
			return GetRandPNDPoint(entity.Size);
		}*/
		public static void Unload() {
			texture = null;
			DrawPND = null;
			PNPs = null;
		}
	}
}
