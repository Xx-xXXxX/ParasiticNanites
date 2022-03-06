using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ParasiticNanites.Buffs;

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
	public class ParasiticNanitesArrow:ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Arror");     //The English name of the projectile
			DisplayName.AddTranslation(GameCulture.Chinese, "寄生机器人箭");
		}
		public override void SetDefaults()
		{
			projectile.width = 8;               //The width of projectile hitbox
			projectile.height = 8;              //The height of projectile hitbox
			projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 120;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;            //Set to above 0 if you want the projectile to update multiple time in a frame
			//aiType = ProjectileID.WoodenArrowFriendly;           //Act exactly like default Bullet
			projectile.arrow = true;
			
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			Dust.NewDust(projectile.Center-new Vector2(-4,-4),8,8,ModContent.DustType<Dusts.ParasiticNanitesDust>(), projectile.velocity.X,projectile.velocity.Y);
		}
		public int Num
		{
			get => projectile.damage;
			set => projectile.damage = value;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			projectile.Center -= projectile.velocity;
			ParasiticNanitesProj.SummonSomeParasiticNanites(projectile.Center,(int)Math.Sqrt(Num),false,speed:projectile.velocity.Length()/4f,action:(i)=> { Main.projectile[i].localAI[1] += 30; Main.projectile[i].velocity += projectile.velocity/3f; });
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(
				ModContent.GetTexture(Texture),
				projectile.Center - Main.screenPosition,
				null, lightColor,
				projectile.rotation, new Vector2(26.5f, 6.5f), 1, SpriteEffects.None, 0f
				);
			return false;
		}
	}
}
