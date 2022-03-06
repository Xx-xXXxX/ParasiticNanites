using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ParasiticNanites.Buffs;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParasiticNanites.Projectiles
{
	public class ParasiticNanitesBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Bullet");     //The English name of the projectile
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人子弹");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;               //The width of projectile hitbox
			projectile.height = 8;              //The height of projectile hitbox
			//projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;            //Set to above 0 if you want the projectile to update multiple time in a frame
			projectile.extraUpdates = 1;
			
		}
		public int Num
		{
			get => projectile.damage;
			set => projectile.damage = value;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<ParasiticNanitesBuff>(), (int)Math.Sqrt(Num + 1));
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<ParasiticNanitesBuff>(), (int)Math.Sqrt(Num + 1));
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.ai[0] == 1)
				return true;
			else
				Terraria.Utils.DrawLine(spriteBatch, projectile.oldPosition + projectile.Size / 2 + (projectile.oldPosition + projectile.Size / 2 - projectile.Center), projectile.Center, Color.Transparent, Color.Red, 3);
			return false;
		}
	}
}
