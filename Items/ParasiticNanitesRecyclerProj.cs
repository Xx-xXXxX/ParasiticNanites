using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
namespace ParasiticNanites.Items
{
	public class ParasiticNanitesRecyclerProj:ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;               //The width of projectile hitbox
			projectile.height = 32;              //The height of projectile hitbox
			projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = false;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 30;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;            //Set to above 0 if you want the projectile to update multiple time in a frame
			aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
			projectile.damage = 0;
		}
		public override void AI()
		{
			projectile.scale += 0.05f;
			foreach (var i in Main.projectile) {
				if (i.active && i.type == ModContent.ProjectileType<Projectiles.ParasiticNanitesProj>()) {
					float _d=0f;
					if (Collision.CheckAABBvLineCollision(i.Center+ new Vector2(i.scale * -9)	,
						  new Vector2(i.scale*18),
						  projectile.Center - new Vector2(-16 * projectile.scale, 0).RotatedBy(projectile.rotation),
						  projectile.Center - new Vector2(16 * projectile.scale, 0).RotatedBy(projectile.rotation),
						  16 * projectile.scale, ref _d
						  ))
					{
						if(i.damage / 5>0)
							Item.NewItem(i.position, i.Size, ModContent.ItemType<ParasiticNanitesItem>(), i.damage/5);
						i.localAI[0] = Projectiles.ParasiticNanitesProj.SpecialDeath;
						i.Kill();
					}
				}
			}
		}
	}
}
