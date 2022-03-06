using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using XxDefinitions;

namespace ParasiticNanites.Projectiles
{
	public class ParasiticNanitesRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Rocket");     //The English name of the projectile
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "¼ÄÉú»úÆ÷ÈË»ð¼ý");
		}
		public XxDefinitions.UnifiedTarget target {
			get => new UnifiedTarget((short)projectile.ai[1]);
			set => projectile.ai[1] = (float)value;
		}
		public override void SetDefaults()
		{
			projectile.width = 20;               //The width of projectile hitbox
			projectile.height = 14;              //The height of projectile hitbox
			projectile.aiStyle = -1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 1200;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;          //Can the projectile collide with tiles?
			projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			int R = 8;
			return new Rectangle((int)projectile.Center.X - R, (int)projectile.Center.Y - R, R * 2, R * 2).Intersects(targetHitbox);
		}
		public override void AI()
		{
			if (projectile.timeLeft % 12 == 7) {
				int Num = (int)Math.Ceiling(projectile.damage*0.075f);
				ParasiticNanitesProj.SummonSomeParasiticNanites(projectile.Center, Num, true);
				projectile.damage -= Num;
				if (projectile.damage <= 0) projectile.Kill();
			}
			if (ParasiticNanites.ProjChasing) {
				if (projectile.timeLeft % 30 == 17) {
					target = XxDefinitions.Utils.CalculateUtils.FindTargetClosest(projectile.Center,240,projectile.hostile,projectile.friendly);
				}
				if (!target.IsNull)
				{
					if (target.IsNPC)
					{
						NPC nPC = target.npc;
						if (!nPC.active||(nPC.friendly&&!projectile.hostile)|| (!nPC.friendly && !projectile.friendly) || !nPC.WithinRange(projectile.Center, 240))
						{
							target = XxDefinitions.Utils.CalculateUtils.FindTargetClosest(projectile.Center, 240, false, true);
						}
						else
						if ((nPC.Center - projectile.Center).Length() != 0)
						{
							projectile.velocity += 0.75f * Vector2.Normalize(nPC.Center - projectile.Center);
							projectile.velocity *= 0.95f;
						}
					}
					else
					{
						Player player = target.player;
						if (!player.active||!projectile.hostile || !player.WithinRange(projectile.Center, 240))
						{
							target = XxDefinitions.Utils.CalculateUtils.FindTargetClosest(projectile.Center, 240, false, true);
						}
						else
						if ((player.Center - projectile.Center).Length() != 0)
						{
							projectile.velocity += 0.2f * Vector2.Normalize(player.Center - projectile.Center);
							projectile.velocity *= 0.97f;
						}
					}
				}
			}
			projectile.scale =(float)Math.Sqrt( projectile.damage / 10f);
			projectile.rotation = projectile.velocity.ToRotation();
			XxDefinitions.Utils.SummonUtils.SummonDustExplosion(projectile.Center, 0.1f, 0, 0, ModContent.DustType<Dusts.ParasiticNanitesDust>(),0, 2, (float)Math.Sqrt(projectile.damage * 5));
		}
		public override void Kill(int timeLeft)
		{
			if (projectile.damage > 0) { 
				ParasiticNanitesProj.SummonSomeParasiticNanites(projectile.Center, projectile.damage, true,speed:(float)Math.Sqrt(projectile.damage*2));
				XxDefinitions.Utils.SummonUtils.SummonDustExplosion(projectile.Center,(float)Math.Sqrt(projectile.damage*10), (projectile.friendly)?projectile.damage:0, (projectile.hostile) ? projectile.damage : 0, ModContent.DustType<Dusts.ParasiticNanitesDust>(),32,32, (float)Math.Sqrt(projectile.damage * 5),MakeDeathReason:(Player)=>Terraria.DataStructures.PlayerDeathReason.ByProjectile(projectile.owner,projectile.whoAmI));
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			projectile.Kill();
		}
	}
}
