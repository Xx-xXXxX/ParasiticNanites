using Microsoft.Xna.Framework;

using ParasiticNanites.Buffs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using ParasiticNanites.Items;
using XxDefinitions;
namespace ParasiticNanites.Projectiles
{
	public class ParasiticNanitesProj:ModProjectile
	{
		public override void SetStaticDefaults()
		{

			DisplayName.SetDefault("Parasitic Nanites");     //The English name of the projectile
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "寄生机器人");
		}
		public static ulong Rand1=0,Rand2=0;
		public static int RandF() {
			Rand1 = Terraria.Utils.RandomNextSeed(Rand1 ^ Rand2);
			Rand2 = Terraria.Utils.RandomNextSeed(Rand1 - Rand2);
			return (int)(Rand1 ^ Rand2);
		}
		public static XxDefinitions.IRandomByDelegate rand = new XxDefinitions.IRandomByDelegate(RandF);
		public static XxDefinitions.NetPacketTreeLeaf<int> GetRandNet;
		public static void Load() {
			GetRandNet = new XxDefinitions.NetPacketTreeLeaf<int>(
				  (reader, who) => {
					  Rand1 = reader.ReadUInt64();
					  Rand2 = reader.ReadUInt64();
					  ParasiticNanites.AdjustParasiticNanitesNow = reader.ReadInt32();
				  }, ParasiticNanites.NetPacketTreeMain, 10,
				   (N) => {
					   N.Write(Rand1);
					   N.Write(Rand2);
					   N.Write(ParasiticNanites.AdjustParasiticNanitesNow);
				   });
		}
		public static int NewParasiticNanitesProj(Vector2 Pos,int Num,bool Flying,int Ignore=0, float speed = 7) {
			for (int i = 0; i < Num % 3; ++i) {
				Rand1=Terraria.Utils.RandomNextSeed(Rand1^ Rand2);
				Rand2= Terraria.Utils.RandomNextSeed(Rand1 - Rand2);
			}
			//Projectile.NewProjectile(Pos,new Vector2((Terraria.Utils.RandomFloat(ref Rand1)-0.5f)*2*speed, (Terraria.Utils.RandomFloat(ref Rand2) - 0.5f) * 2 * speed), ModContent.ProjectileType<ParasiticNanitesProj>(), Num, 0,0, ai1:XxDefinitions.BitOperate.IToFBit(ai1),ai0: Ignore);
			return NewParasiticNanitesProjPerfect(Pos, new Vector2((Terraria.Utils.RandomFloat(ref Rand1) - 0.5f) * 2 * speed, (Terraria.Utils.RandomFloat(ref Rand2) - 0.5f) * 2 * speed), Num, Flying, Ignore);
		}
		public static int NewParasiticNanitesProjPerfect(Vector2 Pos, Vector2 vel, int Num, bool Flying, int Ignore = 0)
		{
			int ai1 = 0;
			ai1 = XxDefinitions.BitOperate.SetBits(ai1, Flying ? 1 : 0, 0, 1);
			int i= Projectile.NewProjectile(Pos, vel, ModContent.ProjectileType<ParasiticNanitesProj>(), Num, 0, Main.myPlayer, ai1: XxDefinitions.BitOperate.IToFBit(ai1), ai0: Ignore);
			Main.projectile[i].scale= (float)Math.Sqrt(Num) * 4/9;
			Main.projectile[i].localAI[1] = Main.projectile[i].timeLeft;
			Main.projectile[i].tileCollide = !Flying;
			if (Flying) Main.projectile[i].rotation = Main.rand.NextFloat() * (float)Math.PI * 2;
			return i;
		}
		public static int SummonSomeParasiticNanitesNum(int Num) { 
			return (int)Math.Ceiling(Math.Min(Math.Log((ParasiticNanites.ParasiticNanitesProjMaxNum - ParasiticNanites.ParasiticNanitesProjNum)), Math.Sqrt(Num)));
		}
		/// <param name="Ignore">0 for none + for npc - for player</param>
		public static List<int> SummonSomeParasiticNanites(Vector2 Pos, int Num,bool Flying, int Ignore =0,float speed=7,Action<int> action=null) {
			// Max ln(ParasiticNanitesBuff.ParasiticNanitesProjNum)
			List<int> D = new List<int>();
			if (ParasiticNanites.ParasiticNanitesProjNum+1>= ParasiticNanites.ParasiticNanitesProjMaxNum) {
				D.Add(NewParasiticNanitesProj(Pos, Num, Flying,Ignore, speed));
				return D;
			} 
			else{
				int SummonNum = SummonSomeParasiticNanitesNum(Num);
				if (SummonNum == 0) return D;
				int HasPProjD = (int)Math.Floor((float)Num / SummonNum);
				int HasPProjA = Num % SummonNum;
				int realnum = 0;
				for (int i = 1; i <= SummonNum; ++i) {
					int it = NewParasiticNanitesProj(Pos, HasPProjD + ((i > HasPProjA) ? (0) : (1)), Flying, Ignore, speed);
					action?.Invoke(it);
					D.Add(it);
					realnum += HasPProjD + ((i > HasPProjA) ? (0) : (1));
				}
				return D;
			}
		}
		public static List<int> SummonSomeParasiticNanites(Rectangle rect, int Num, bool Flying, int Ignore = 0, float speed = 7, Action<int> action = null)
		{
			// Max ln(ParasiticNanitesBuff.ParasiticNanitesProjNum)
			List<int> D = new List<int>();
			if (ParasiticNanites.ParasiticNanitesProjNum + 1 >= ParasiticNanites.ParasiticNanitesProjMaxNum)
			{
				D.Add(NewParasiticNanitesProj(rand.NextVectorInRect(rect), Num, Flying, Ignore, speed));
				return D;
			}
			else
			{
				int SummonNum = SummonSomeParasiticNanitesNum(Num);
				if (SummonNum == 0) return D;
				int HasPProjD = (int)Math.Floor((float)Num / SummonNum);
				int HasPProjA = Num % SummonNum;
				int realnum = 0;
				for (int i = 1; i <= SummonNum; ++i)
				{
					int it = NewParasiticNanitesProj(rand.NextVectorInRect(rect), HasPProjD + ((i > HasPProjA) ? (0) : (1)), Flying, Ignore, speed);
					action?.Invoke(it);
					D.Add(it);
					realnum += HasPProjD + ((i > HasPProjA) ? (0) : (1));
				}
				return D;
			}
			return D;
		}

		public static int TimeMax {
			get => 360;
		}
		public float R {
			get => projectile.scale * 9;
			set => projectile.scale = value / 9;
		}
		public override void SetDefaults()
		{

			//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{Num}", projectile.Center, 15);
			ParasiticNanites.ParasiticNanitesProjNum += 1;
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.trap = true;
			projectile.ignoreWater = false;
			projectile.timeLeft = TimeMax;
			projectile.tileCollide = true;
			projectile.penetrate =5;
			projectile.usesLocalNPCImmunity = true;
			//projectile.colld
			projectile.scale = 0.5f;
			projectile.hide = false;
			projectile.npcProj = false;
			projectile.localAI[0] = 0;
			projectile.localNPCHitCooldown = 6;
		}
		public static int SpecialDeath => -1000;
		public override void Kill(int timeLeft)
		{
			ParasiticNanites.ParasiticNanitesProjNum -= 1;
			if (projectile.localAI[0] != SpecialDeath) {
				if (ParasiticNanites.ProjBoom) {
					XxDefinitions.Utils.SummonUtils.SummonProjExplosionTrap(projectile.Center, (float)Math.Sqrt(Num * 4) * 4 + 4, Num*4 ,Num/8, Color.Red);
				}
			}
		}
		public int Num {
			get => projectile.damage;
			set => projectile.damage = value;
		}
		public int IntAI0
		{
			get => XxDefinitions.BitOperate.FToIBit(projectile.ai[0]);
			set => projectile.ai[0] = XxDefinitions.BitOperate.IToFBit(value);
		}
		public int IntAI1 {
			get => XxDefinitions.BitOperate.FToIBit(projectile.ai[1]);
			set => projectile.ai[1]=XxDefinitions.BitOperate.IToFBit(value);
		}
		public bool Flying {
			get => XxDefinitions.BitOperate.GetBits(IntAI1,0,1)==1;
			set => IntAI1=XxDefinitions.BitOperate.SetBits(IntAI1,value?1:0,0,1);
		}
		public static bool ParasiticNanitesProjFlying(Projectile projectile) { 
			return XxDefinitions.BitOperate.GetBits(XxDefinitions.BitOperate.FToIBit(projectile.ai[1]), 0, 1) == 1;
		}
		public bool AbleToChasing(NPC i) {
			return i.active && i.CanBeChasedBy() && CanHitNPC(i).Value&&!i.buffImmune[ModContent.BuffType<Buffs.ParasiticNanitesBuff>()];
		}
		public bool AbleToChasing(Player i)
		{
			return XxDefinitions.Utils.PlayerCanFind(i) && CanHitPlayer(i)&&!i.buffImmune[ModContent.BuffType<Buffs.ParasiticNanitesBuff>()];
		}
		public XxDefinitions.UnifiedTarget FindTarger() {
			float Range = 240;
			XxDefinitions.UnifiedTarget tarjet = new XxDefinitions.UnifiedTarget();
			float ClosedR = Range;
			foreach (var i in Main.npc) {
				if (AbleToChasing(i)) {
					float Nr = (i.Center - projectile.Center).Length()-i.Size.Length()*2-R;
					if (Nr < ClosedR) { tarjet.NPCID = i.whoAmI; ClosedR = Nr; }
				}
			}
			foreach (var i in Main.player)
			{
				if (AbleToChasing(i))
				{
					float Nr = (i.Center - projectile.Center).Length() - i.Size.Length() * 2-R;
					if (Nr < ClosedR) { tarjet.PlayerID = i.whoAmI; ClosedR = Nr; }
				}
			}
			return tarjet;
		}
		public int OldTL=TimeMax;
		public List<int> OldData = new List<int>(new int[20]);
		public override void AI()
		{
			//R = (float)Math.Sqrt(projectile.damage)*4;
			if (!Flying)
			{
				projectile.velocity.Y += 0.1f;
				projectile.rotation += projectile.velocity.X / R;
				Point P = (projectile.Center / 16).ToPoint();
			}
			else {
				projectile.velocity *= 0.98f;
				projectile.rotation += projectile.velocity.Length() / R*(projectile.whoAmI%2-0.5f);
			}
			/*
			//ushort N = (ushort)Terraria.Utils.RandomNextSeed(Terraria.Utils.RandomNextSeed((ulong)(projectile.timeLeft - projectile.whoAmI))-(uint)projectile.whoAmI);
			ushort N = (ushort)Terraria.Utils.RandomNextSeed(
						Terraria.Utils.RandomNextSeed((ulong)projectile.timeLeft)+
						Terraria.Utils.RandomNextSeed((ulong)projectile.whoAmI));
			bool Able = (N) % 7== 1;
			OldData.Add(N % 7);
			OldData.RemoveAt(0);
			if (Able) {
				projectile.timeLeft -= 1;
				
			}

			string shownString = $"{Able}\n{N}\n{projectile.timeLeft}\n{OldTL - projectile.timeLeft}\n{projectile.whoAmI}\n";
			foreach (var i in OldData) {
				shownString += $"{i}\n";
			}
			XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString(shownString, projectile.Center);
			OldTL = projectile.timeLeft;

			*/
			ref float I = ref projectile.localAI[0];
			if (projectile.timeLeft % 30 == 1) {
				if (ParasiticNanites.ProjChasing)
				{
					if (I != SpecialDeath) {
						I = (short)FindTarger();
					}
				}
			}
			if (I != SpecialDeath)
			{
				XxDefinitions.UnifiedTarget target = new XxDefinitions.UnifiedTarget((short)I);
				if (!target.IsNull)
				{
					if (target.IsNPC)
					{
						NPC nPC = target.npc;
						if (!AbleToChasing(nPC))
						{
							I = (short)FindTarger();
						}
						else
						if ((nPC.Center - projectile.Center).Length() != 0)
							projectile.velocity += 0.25f * Vector2.Normalize(nPC.Center - projectile.Center);
					}
					else
					{
						Player player = target.player;
						if (!AbleToChasing(player))
						{
							I = (short)FindTarger();
						}
						else
						if ((player.Center - projectile.Center).Length() != 0)
							projectile.velocity += 0.25f * Vector2.Normalize(player.Center - projectile.Center);
					}
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			int x = (int)projectile.Center.X / 16;
			int y = (int)projectile.Center.Y / 16;
			if (Main.tile[x,y].collisionType==1)
			{
				projectile.localAI[0] = SpecialDeath;
				projectile.Kill();
			}

			if (projectile.velocity.Length() > 1)
			{
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, (int)R * 2, (int)R * 2);
				//Main.PlaySound(Terraria.ID.SoundID.Item10, projectile.position);
			}
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}

			projectile.velocity.X = (float)Math.Floor(projectile.velocity.X * 0.95f * 10) / 10;
			projectile.velocity.Y = (float)Math.Floor(projectile.velocity.Y * 0.95f * 10) / 10;
			return false;
		}
		public override bool CanDamage()
		{
			return projectile.timeLeft < TimeMax-15;
		}
		public static bool SpecialIgnore(NPC target) {
			return target.type == NPCID.TargetDummy || target.type == ModContent.NPCType<NPCs.ParasiticNanitesSlime>() || target.type == ModContent.NPCType<NPCs.ParasiticNanitesKingSlime>();
		}
		public override bool? CanHitNPC(NPC target)
		{
			if ((!ParasiticNanites.ProjSpike&&target.buffImmune[ModContent.BuffType<Buffs.ParasiticNanitesBuff>()]) || SpecialIgnore (target)||  (target.whoAmI+1==projectile.ai[0]&&projectile.timeLeft> TimeMax-60))
				return false;
			return true;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (ParasiticNanites.ProjSpike)
			{
				if (target.friendly)
					damage *= 2;
				else
					damage *= 3;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.buffImmune[ModContent.BuffType<ParasiticNanitesBuff>()]) {
				//target.immune[projectile.owner] = 0;
				//Projectile.perIDStaticNPCImmunity[ModContent.ProjectileType<ParasiticNanitesProj>()][target.whoAmI] = (uint)((int)Main.GameUpdateCount + 10);
			}
			else{
				target.AddBuff(ModContent.BuffType<ParasiticNanitesBuff>(), Num);
				projectile.localAI[0] = SpecialDeath;
				projectile.Kill(); 
			}
		}
		public override bool CanHitPlayer(Player target)
		{
			if ((!ParasiticNanites.ProjSpike && target.buffImmune[ModContent.BuffType<Buffs.ParasiticNanitesBuff>()])||(target.whoAmI+1==-projectile.ai[0]&&projectile.timeLeft>480)) 
				return false;
			return true;
		}
		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (ParasiticNanites.ProjSpike)
				damage *= 2;
			
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (target.buffImmune[ModContent.BuffType<ParasiticNanitesBuff>()])
			{
				
			}
			else {
				target.AddBuff(ModContent.BuffType<ParasiticNanitesBuff>(), Num);
				projectile.localAI[0] = Projectiles.ParasiticNanitesProj.SpecialDeath;
				projectile.Kill();
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(
				ModContent.GetTexture(Texture),
				projectile.Center - Main.screenPosition,
				null, lightColor,
				projectile.rotation, new Vector2(9, 9), projectile.scale/2, SpriteEffects.None, 0f
				);
			if (ParasiticNanites.ProjBoom) {
			spriteBatch.Draw(
					ModContent.GetTexture("ParasiticNanites/Items/Accessories/ParasiticNanitesTNTExtension"),
					projectile.Center - Main.screenPosition,
					null, lightColor,
					projectile.rotation, new Vector2(16, 16), projectile.scale*9/16/2, SpriteEffects.None, 0f
					);
			}
			if (ParasiticNanites.ProjSpike) {
				spriteBatch.Draw(
					ModContent.GetTexture("ParasiticNanites/Items/Accessories/ParasiticNanitesSpikeExtension"),
					projectile.Center - Main.screenPosition,
					null, lightColor,
					projectile.rotation, new Vector2(16, 16), projectile.scale * 9 / 16 / 1.25f, SpriteEffects.None, 0f
					);
			}
			if (ParasiticNanites.ProjChasing)
			{
				spriteBatch.Draw(
					ModContent.GetTexture("ParasiticNanites/Items/Accessories/ParasiticNanitesMotorExtension"),
					projectile.Center - Main.screenPosition,
					null, lightColor,
					projectile.velocity.ToRotation(), new Vector2(16, 16), projectile.scale * 9 / 16 / 1.25f, SpriteEffects.None, 0f
					);
			}
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return XxDefinitions.Utils.CalculateUtils.CheckAABBvCircleColliding(targetHitbox, projectile.Center, R/2);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[1]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[1] = reader.ReadSingle();
		}
	}
}
