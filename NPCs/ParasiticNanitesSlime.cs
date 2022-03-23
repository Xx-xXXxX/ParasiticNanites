using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using XxDefinitions;
namespace ParasiticNanites.NPCs
{
	public class ParasiticNanitesSlime:ModNPC
	{
		public Point TPNDXY=new Point(32,52);
		public Point PNDXY = new Point(32, 52);
		public static HashSet<int> SlimeNPC = new HashSet<int> { 
			NPCID.BabySlime,

			NPCID.IlluminantSlime,

			NPCID.CorruptSlime,
			NPCID.DungeonSlime,
			NPCID.IceSlime,
			NPCID.SandSlime,
			NPCID.JungleSlime,
			NPCID.LavaSlime,

			NPCID.MotherSlime,
			NPCID.UmbrellaSlime,

			NPCID.BlueSlime,
			NPCID.GreenSlime,
			NPCID.BlackSlime,
			NPCID.RedSlime,
			NPCID.RainbowSlime,
			NPCID.YellowSlime,
			NPCID.PurpleSlime
		};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parasitic Nanites Slime");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "机器人寄生史莱姆");
			Main.npcFrameCount[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			npc.lifeMax = 1000;
			npc.life = 505;
			npc.damage = 75;
			npc.defense = 15;
			npc.width = 32;
			npc.height = 24;
			PNDXY = Effects.ParasiticNanitesDraw.GetRandPNDPoint();
			npc.value = Item.buyPrice(0, 0, 50, 0);
			npc.npcSlots = 50f;
			//npc.HitSound = SoundID.NPCHit1;
			//npc.DeathSound = null;
			npc.buffImmune[ModContent.BuffType< Buffs.ParasiticNanitesBuff>()]= true;
			npc.aiStyle =1;
			npc.friendly = false;
			//aiType= NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			//aiType = 1;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(npc.Hitbox,(int)Math.Min(npc.life,damage)/2,false,npc.whoAmI+1);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType< Buffs.ParasiticNanitesBuff>(),(int)Math.Log(damage+1));
		}
		public override void AI()
		{
			if (ParasiticNanitesWorld.KingSlimeCount > 0) {
				XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{ParasiticNanitesWorld.KingSlimeCount}",npc.position);
				if (Math.Abs(npc.ai[0]) % 100 - 5 > 0) {
					npc.ai[0] += 1;
				}
			}
			if (Main.time % 15 == npc.whoAmI % 15) {
				Dust.NewDust(npc.position,npc.width,npc.height,ModContent.DustType< Dusts.ParasiticNanitesDust>());
				//npc.StrikeNPC((int)Math.Floor(-0.02f*npc.lifeMax),0,0);
				//npc.StrikeNPC(25+npc.defense, 0, 0);
				if (npc.life >= npc.lifeMax)
				{
					npc.life = npc.lifeMax - 1;
				}
				else
				if (npc.life > npc.lifeMax / 2)
				{
					npc.life = Math.Min(npc.lifeMax-1, npc.life + 10);
				}
				else
				{
					Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(npc.Center, 5, false, npc.whoAmI + 1);
					npc.life -= 10;
					if (npc.life <= 0) { npc.life = 1; npc.StrikeNPC(1, 0, 0); }
				}
			}
			if (npc.life <= npc.lifeMax / 2&&Main.time % 5 == npc.whoAmI % 5)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.ParasiticNanitesDust>());
			}
		}
		public override bool CheckDead()
		{
			if (ParasiticNanites.ProjBoom)
			{
				XxDefinitions.Utils.SummonUtils.SummonProjExplosionTrap(npc.Center, 196, 300,50, Microsoft.Xna.Framework.Color.Red);
			}
			XxDefinitions.Utils.SummonUtils.SummonDustExplosion(npc.Center, 16, 0, 0, ModContent.DustType<Dusts.ParasiticNanitesDust>(), 8, 8, 3);

			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			XxDefinitions.Utils.SpriteBatchUsingEffect(spriteBatch);
			Effects.ParasiticNanitesDraw.UseEffect(TPNDXY, TPNDXY,npc.frame.Location, drawColor);
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			XxDefinitions.Utils.SpriteBatchEndUsingEffect(spriteBatch);
		}
	}
}
