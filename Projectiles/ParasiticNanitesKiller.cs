using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ParasiticNanites.Projectiles
{
	public class ParasiticNanitesKiller:ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 0;
			projectile.height = 0;
			projectile.scale = 0.1f;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.damage = 0;
			projectile.timeLeft = 300;
			projectile.penetrate = -1;
			projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			projectile.scale += 0.2f;
			foreach (var i in Main.projectile) {
				if (i.active && i.type == ModContent.ProjectileType<ParasiticNanitesProj>())
				{
					float R1 = (i.Center - projectile.Center).Length();
					float R2 = 32 * projectile.scale;
					if (R2 * 0.95f <= R1 && R1 <= R2 * 1.05f) {
						i.localAI[0] = ParasiticNanitesProj.SpecialDeath;
						i.Kill();
						XxDefinitions.Utils.SummonUtils.SummonProjExplosionTrap(i.Center, (float)Math.Sqrt(i.damage * 8) * 8 + 16, i.damage * 2, i.damage / 2, Color.Red);
					}
				}
			}
			if (projectile.timeLeft < 100) projectile.alpha = (int)(255 / ((100 - projectile.timeLeft) / 100f));
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Color.White;
			color.A = (byte)(255-projectile.alpha);
			spriteBatch.Draw(
				ModContent.GetTexture(Texture),
				projectile.Center - Main.screenPosition,
				null, color,
				projectile.rotation, new Vector2(31, 31), projectile.scale, SpriteEffects.None, 0f
				);
			return false;
		}
	}
}
