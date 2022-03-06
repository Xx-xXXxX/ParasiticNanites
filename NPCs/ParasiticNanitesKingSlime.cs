using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ParasiticNanites.Buffs;
using ParasiticNanites.Projectiles;
using Mono.Cecil;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using XxDefinitions;

namespace ParasiticNanites.NPCs
{
	[AutoloadBossHead]
	public class ParasiticNanitesKingSlime:ModNPC
	{
		public int IntAI0
		{
			get => XxDefinitions.BitOperate.FToIBit(npc.ai[0]);
			set => npc.ai[0] = XxDefinitions.BitOperate.IToFBit(value);
		}
		public int IntAI1
		{
			get => XxDefinitions.BitOperate.FToIBit(npc.ai[1]);
			set => npc.ai[1] = XxDefinitions.BitOperate.IToFBit(value);
		}
		public int IntAI2
		{
			get => XxDefinitions.BitOperate.FToIBit(npc.ai[2]);
			set => npc.ai[2] = XxDefinitions.BitOperate.IToFBit(value);
		}
		public int IntAI3
		{
			get => XxDefinitions.BitOperate.FToIBit(npc.ai[3]);
			set => npc.ai[3] = XxDefinitions.BitOperate.IToFBit(value);
		}
		public uint MoveTimeDelay {//0~2^11 2048 0 10
			get => (uint)XxDefinitions.BitOperate.GetBits(IntAI0,0,11);
			set => IntAI0=XxDefinitions.BitOperate.SetBits(IntAI0,(int)value,0,11);
		}
		public EMoveMode MoveMode//0~7
		{
			get => (EMoveMode)XxDefinitions.BitOperate.GetBits(IntAI0, 29, 3);
			set {
				int NM = (int)MoveMode;
				if (NM != (int)value) {
					IntAI0 = XxDefinitions.BitOperate.SetBits(IntAI0, (int)value, 29, 3);
					SetFrame();
				}
			}
		}
		public enum EMoveMode:int{ 
			Waiting,
			Jumping,
			Teleport,
		}
		public int JumpTime {//0~7 11 13
			get => XxDefinitions.BitOperate.GetBits(IntAI0, 11, 3);
			set => IntAI0=XxDefinitions.BitOperate.SetBits(IntAI0, value, 11, 3);
		}
		public ulong RandS;
		public int RandNextInt() => Terraria.Utils.RandomNext(ref RandS,32);
		public float RandNextFloat() => Terraria.Utils.RandomFloat(ref RandS);
		public float RandNextFloatDirection() => (RandNextFloat()-0.5f)*2;
		public bool RandNextBool()=> Terraria.Utils.RandomNext(ref RandS, 1)%2==0;
		public int LifeLevelLife() =>(int)((float) LifeLevel / 100*npc.lifeMax);
		public Vector2 TeleportPos {
			get => new Vector2(npc.localAI[2], npc.localAI[3]);
			set { npc.localAI[2] = value.X; npc.localAI[3] = value.Y; }
		}
		public Player Target { get => Main.player[npc.target]; }

		public static uint[] JumpTimeDelays = new uint[] { 120, 15, 30, 10, 10 };
		public static float[] JunpVels = {8,16,15,14,13};
		public Vector2 JunpVel => new Vector2(0, -JunpVels[JumpTime]);
		public static Vector2 HorizonalMoveA => new Vector2(0.075f, 0);
		public static int FrameCount = 6;
		public static int FrameX = 174;
		public static int FrameY=120;
		public Rectangle GetFrame(int N) {
			return new Rectangle(0,N* FrameY, FrameX, FrameY);
		}
		public int GetFrameC() {
			return npc.frame.Y / FrameY;
		}
		public override void BossHeadSlot(ref int index)
		{
			index = ModContent.GetModBossHeadSlot(BossHeadTexture);
		}
		public override void SetStaticDefaults()
		{
			
			ModTranslation DRS = mod.CreateTranslation("NPCNames.ParasiticNanitesKingSlime"); 
			DRS.SetDefault("Parasitic Nanites King Slime");
			DRS.AddTranslation(GameCulture.Chinese, "机器人寄生史莱姆王");
			mod.AddTranslation(DRS);
			DRS = mod.CreateTranslation("BossSpawnInfo.ParasiticNanitesKingSlime");
			DRS.SetDefault("Infects King Slime with Parasitic Nanites and below half blood");
			DRS.AddTranslation(GameCulture.Chinese, "使史莱姆王感染寄生机器人并在一半血以下");
			mod.AddTranslation(DRS);

			DisplayName.SetDefault("Parasitic Nanites King Slime");
			DisplayName.AddTranslation(Terraria.Localization.GameCulture.Chinese, "机器人寄生史莱姆王");
			Main.npcFrameCount[npc.type] = FrameCount;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}
		public static Vector2 Size = new Vector2(98, 92);
		public override void SetDefaults()
		{
			npc.boss = true;
			npc.Size = Size;
			npc.damage = 100;
			npc.defense = 25;
			npc.lifeMax = 30000;
			if (Main.expertMode) npc.lifeMax += 15000;
			npc.damage += 25;
			npc.knockBackResist = 0f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.alpha = 30;
			npc.value = 10000f;
			Scale = 1f;
			npc.buffImmune[ModContent.BuffType<ParasiticNanitesBuff>()] = true;
			npc.timeLeft = NPC.activeTime * 30;
			npc.aiStyle = -1;
			npc.stepSpeed = 0;
			npc.noGravity = true;
			aiType = -1;
			npc.frame = GetFrame(0);
			MoveTimeDelay = JumpTimeDelays[0];
			//npc.aiStyle = 15;
			//animationType = 15;
			//CheckTarget();
			MoveMode = EMoveMode.Teleport;
			RandS = Terraria.Utils.RandomNextSeed((ulong)(ParasiticNanitesWorld.NewRSF()+(ulong)npc.type+ (ulong)Main.projectile[1].type));
			PNDXY = Effects.ParasiticNanitesDraw.GetRandPNDPoint();
			ParasiticNanitesWorld.KingSlimeCount += 1;
		}
		public Point TXY= new Point(174,720);
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax= (int)((double)npc.lifeMax * 0.7 * (double)bossLifeScale);
		}
		public static int PNDustType => ModContent.DustType<Dusts.ParasiticNanitesDust>();
		//public static int PNProjType=ModContent.ProjectileType<PN>
		public bool TargetAble() {
			if (!npc.HasPlayerTarget) return false;
			int id = npc.target;
			if (!XxDefinitions.Utils.PlayerCanFind(Main.player[id])) return false;
			Player t = Main.player[id];
			if (t.Distance(npc.Center) > 6400) return false;
			return true;
		}
		//public override BossH
		public bool TargetCanTeleport()
		{
			if (!TargetAble()) return false;
			int id = npc.target;
			Player t = Main.player[id];
			if (!Collision.CanHitLine( npc.Center, 0, 0, t.Center, 0, 0)) return true;
			if (t.Distance(npc.Center) <= 1600) return false;
			return true;
		}
		public bool CheckTarget() {
			if (!TargetAble())
			{
				npc.TargetClosest(false);
				if (!TargetAble())
				{
					return false;
				}
				else return true;
			}
			else return true;
		}
		public static int MaxFallSpeed => 16;
		public void MoveAI() {
			Dust.NewDust(npc.position, npc.width, npc.height, PNDustType);
			//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{MoveMode}\nTimeDelay:{MoveTimeDelay}\nVY:{npc.velocity.Y}\nJumpTime:{JumpTime}", npc.Center);
			//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{Target.Distance(npc.Center)}", Target.Center);
			//XxDefinitions.XDebugger.Utils.AddDraw.AddDrawLineTo(npc.Center, Target.Center);
			npc.TargetClosest(false);
			if (npc.timeLeft % 10 == 0)
			{
				if (CheckTarget()) npc.active = false;
			}
			if (TargetAble()) {
				if (Target.position.Y> npc.position.Y + npc.height) npc.stairFall = true;
				else npc.stairFall = false;
			}
			switch (MoveMode)
			{
				case EMoveMode.Waiting:
					{
						if (npc.velocity.Y == 0) npc.velocity.X *= 0.95f;
						if (MoveTimeDelay > 1000) MoveTimeDelay = JumpTimeDelays[JumpTime];
						if (MoveTimeDelay == 0)
						{
							if (TargetCanTeleport())
							{
								if (Collision.CanHitLine(Target.Center, 0, 0, Target.Center + new Vector2(0, -512), 0, 0))
								{
									TeleportPos = Target.Center + new Vector2(0, -512);
									if (npc.Center.X < Target.Center.X - 1000) TeleportPos += new Vector2(800, 0);
									if(npc.Center.X > Target.Center.X + 1000) TeleportPos += new Vector2(-800, 0);
								}
								else TeleportPos = Target.Center;
								MoveTimeDelay = 0;
								MoveMode = EMoveMode.Teleport;
								npc.teleporting = true;
							}
							else
							{
								npc.velocity += JunpVel;
								npc.velocity += HorizonalMoveA * 10 * ((npc.Center.X > Main.player[npc.target].Center.X) ? (-1) : (1));
								JumpTime += 1;
								if (JumpTime >= JumpTimeDelays.Length) JumpTime = 0;
								MoveTimeDelay = JumpTimeDelays[JumpTime];
								MoveMode = EMoveMode.Jumping;
								foreach (var I in Main.npc)
								{
									if (I.active && I.type == ModContent.NPCType<ParasiticNanitesSlime>())
									{
										I.velocity += JunpVel / 2;
										if (I.Center.X < Target.Center.X - 64) I.velocity += HorizonalMoveA * 10;
										if (I.Center.X > Target.Center.X + 64) I.velocity -= HorizonalMoveA * 10;
									}
								}
							}
						}
						MoveTimeDelay -= 1;
						npc.velocity += new Vector2(0, 0.3f);
						if (npc.velocity.Y > MaxFallSpeed) npc.velocity.Y = MaxFallSpeed;
					}
					break;
				case EMoveMode.Jumping:
					{
						if (TargetAble())
						{
							npc.velocity += HorizonalMoveA * ((npc.Center.X > Main.player[npc.target].Center.X) ? (-1) : (1));
							
						}
						else if (!CheckTarget()) npc.active = false;
						if (npc.velocity.Y == 0)
						{
							MoveMode = EMoveMode.Waiting;
						}
						npc.velocity += new Vector2(0, 0.3f);
						if (npc.velocity.Y > MaxFallSpeed) npc.velocity.Y = MaxFallSpeed;
					}
					break;
				case EMoveMode.Teleport:
					{
						npc.velocity = Vector2.Zero;
						MoveTimeDelay += 1;
						XxDefinitions.Utils.SummonUtils.SummonDustExplosion(npc.Center, 1, 0, 0, PNDustType,2, 2, (120 - MoveTimeDelay) / 10);
						XxDefinitions.Utils.SummonUtils.SummonDustExplosion(TeleportPos, 1, 0, 0, PNDustType,2, 2, MoveTimeDelay);
						if (MoveTimeDelay < 60) { npc.alpha = (int)(MoveTimeDelay / 60f * 255);Scale = (60-MoveTimeDelay) / 60f * GetLifeValueScale(); }
						if (MoveTimeDelay == 60) npc.Center = TeleportPos;
						if (MoveTimeDelay > 60 && MoveTimeDelay < 120) { npc.alpha = (int)((120 - MoveTimeDelay) / 60f * 255); Scale = (( MoveTimeDelay-60) / 60f * GetLifeValueScale()); }
						if (MoveTimeDelay >= 120) { MoveMode = EMoveMode.Waiting; npc.velocity -= JunpVel; npc.teleporting = false; }
					}
					break;
			}
			npc.velocity.X *= 0.995f;
		}
		public int LifeLevel
		{//14 20
			get => 100-XxDefinitions.BitOperate.GetBits(IntAI0, 14, 7);
			set => IntAI0 = XxDefinitions.BitOperate.SetBits(IntAI0, 100-value, 14, 7);
		}
		public int GetLifeValue() {
			return (int)((float)npc.life / npc.lifeMax*100);
		}
		public static float MinScale => 1f;
		public static float MaxScale => 3f;
		public float GetLifeValueScale() {
			return LifeLevel/100f*(MaxScale-MinScale) + MinScale;
		}
		public void LifeValueAI() {
			DebugS += $"{GetLifeValue()} {LifeLevel}\n";
			while (LifeLevel > GetLifeValue()) {
				LifeLevel -= 1;
				ParasiticNanitesProj.SummonSomeParasiticNanites(npc.Center,20,false);
				if (LifeLevel % 3 == 0) {
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 1, ModContent.NPCType<NPCs.ParasiticNanitesSlime>());
				}
			}

			if (Scale > GetLifeValueScale()+0.1f) Scale -= 0.05f;
			else
			if (Scale < GetLifeValueScale()- 0.1f) Scale += 0.05f;
			else Scale = GetLifeValueScale();
		}
		public float Scale {
			get => npc.scale;
			set => ResetScale(value);
		}
		public void ResetScale(float newscale) {
			Vector2 Dp = Size * (newscale-npc.scale);
			npc.position.Y-=Dp.Y;
			npc.position.X -= Dp.X / 2;
			npc.Size = Size * newscale;
			npc.scale = newscale;
		}
		public uint AttackTimeDelay
		{
			get => (uint)XxDefinitions.BitOperate.GetBits(IntAI1, 0, 16);
			set => IntAI1 = XxDefinitions.BitOperate.SetBits(IntAI1, (int)value, 0, 16);
		}
		public int AttackCount
		{
			get => XxDefinitions.BitOperate.GetBits(IntAI1, 19, 3);
			set => IntAI1 = XxDefinitions.BitOperate.SetBits(IntAI1, value, 19, 3);
		}
		public EAttackType AttackType
		{
			get => (EAttackType)XxDefinitions.BitOperate.GetBits(IntAI1, 16, 3);
			set {
				if (value == AttackType) return;
				IntAI1 = XxDefinitions.BitOperate.SetBits(IntAI1, (int)value, 16, 3);
				AttackTimeDelay = AttackTypeTime[(int)AttackType];
			}
		}
		public static uint[] AttackTypeTime = { 30, 6, 6, 60, 120, 120 };
		public static int EAttackNum = 5;
		public enum EAttackType : int
		{
			None,
			Bullet,
			Arrow,
			Rocket,
			PNRain,
			PNDelWave,
		}
		public static int[] AttackTypeWeight = new int[] {20,20,4,2,1};
		public static int AttackTypeWeightMax => 47;
		public void AttackAI() {
			DebugS += $"{AttackType} {AttackTimeDelay}\n";
			switch (AttackType) {
				case EAttackType.None: {
						if (AttackTimeDelay == 0) {
							int D = Math.Abs(RandNextInt())% AttackTypeWeightMax;
							AttackType = (EAttackType)(
								(XxDefinitions.Utils.CalculateUtils.WeightingChoose(D, AttackTypeWeight)) + 1
								);
						}
					}break;
				case EAttackType.Bullet: {
						if (AttackTimeDelay == 2) {
							//float R1 = (RandNextFloat() * 2 - 1);
							//float R2= (RandNextFloat() * 2 - 1);
							float speed = 12;
							Vector2 Pos = npc.position + new Vector2(npc.width* RandNextFloat(),npc.height* RandNextFloat());
							Vector2 Vel = Vector2.Zero;
							float? D = (Target.Center - Pos).ToRotation();
							if (RandNextBool()&&(D=XxDefinitions.Utils.CalculateUtils.PredictWithVel(Target.Center- Pos, Target.velocity, speed)).HasValue)
							{
								
							}
							else {
								D = (Target.Center - Pos).ToRotation();
							}
							Vel = (Vector2.UnitX * speed).RotatedBy(D.Value);
							Projectile p = Projectile.NewProjectileDirect(Pos, Vel,ModContent.ProjectileType<ParasiticNanitesBullet>(),50,0,Main.myPlayer);
							p.npcProj = true;
							p.friendly = false;
							p.hostile = true;
							p.ai[0] = 1;
						}
						if (AttackTimeDelay == 0) {
							AttackType = EAttackType.None;
						}
					}
					break;
				case EAttackType.Arrow:{
						if (AttackTimeDelay == 2)
						{
							//float R1 = (RandNextFloat() * 2 - 1);
							//float R2= (RandNextFloat() * 2 - 1);
							float speed = 12;
							Vector2 Pos = npc.position + new Vector2(npc.width * RandNextFloat(), npc.height * RandNextFloat());
							Vector2 Vel = Vector2.Zero;
							float? D = (Target.Center - Pos).ToRotation();
							
							Vel = (Vector2.UnitX * speed).RotatedBy(D.Value);
							Projectile p = Projectile.NewProjectileDirect(Pos, Vel+new Vector2(0,-3), ModContent.ProjectileType<ParasiticNanitesArrow>(), 50, 0, Main.myPlayer);
							p.npcProj = true;
							p.friendly = false;
							p.hostile = true;
						}
						if (AttackTimeDelay == 0)
						{
							AttackType = EAttackType.None;
						}
					}
					break;
				case EAttackType.Rocket: {
						if (AttackTimeDelay == 15)
						{
							//float R1 = (RandNextFloat() * 2 - 1);
							//float R2= (RandNextFloat() * 2 - 1);
							float speed = 5;
							Vector2 Pos = npc.position + new Vector2(npc.width * RandNextFloat(), npc.height * RandNextFloat());
							Vector2 Vel = Vector2.Zero;
							float? D = (Target.Center - Pos).ToRotation();
							Vel = (Vector2.UnitX * speed).RotatedBy(D.Value);
							Projectile p = Projectile.NewProjectileDirect(Pos, Vel, ModContent.ProjectileType<ParasiticNanitesRocket>(), 150, 0, Main.myPlayer);
							p.npcProj = true;
							p.friendly = false;
							p.hostile = true;
						}
						if (AttackTimeDelay == 0)
						{
							AttackType = EAttackType.None;
						}
					}
					break;
				case EAttackType.PNRain:
					{
						if (AttackTimeDelay<=30&& AttackTimeDelay%3==0)
						{
							//float R1 = (RandNextFloat() * 2 - 1);
							//float R2= (RandNextFloat() * 2 - 1);
							float speed = 12;
							Vector2 Pos = npc.position + new Vector2(npc.width * RandNextFloat(), npc.height * RandNextFloat());
							Vector2 Vel = Vector2.Zero;
							float? D = (Target.Center - Pos).ToRotation();

							Vel = (Vector2.UnitX * speed).RotatedBy(D.Value)+ new Vector2(3 * RandNextFloatDirection(), 3 * RandNextFloatDirection());
							Projectile p = Projectile.NewProjectileDirect(Pos, Vel + new Vector2(0, -3), ModContent.ProjectileType<ParasiticNanitesArrow>(), 50, 0, Main.myPlayer);
							p.npcProj = true;
							p.friendly = false;
							p.hostile = true;
							p.timeLeft = (int)(p.timeLeft*0.5f);
						}
						if (AttackTimeDelay == 0)
						{
							AttackType = EAttackType.None;
						}
					}

					break;
				case EAttackType.PNDelWave: {
						if (AttackTimeDelay == 10) {
							Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<ParasiticNanitesKiller>(), 0, 0, Main.myPlayer);
						}
						if (AttackTimeDelay == 0)
						{
							AttackType = EAttackType.None;
						}
					}
					break;
				default:
					if (AttackTimeDelay == 0)
					{
						AttackType = EAttackType.None;
					}
					break;
			}
			if (AttackTimeDelay > 0)
				AttackTimeDelay -= 1;
		}
		public string DebugS = "";
		public override void AI()
		{
			ParasiticNanites.ProjBoom = true;
			ParasiticNanites.ProjSpike = true;
			DebugS = $"V:{npc.velocity.Y}\n";
			MoveAI();
			DebugS+= $"V:{npc.velocity.Y}\n";
			DebugS += $"S:{npc.scale} {GetLifeValueScale()}\n";
			LifeValueAI();
			AttackAI();
			XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString(DebugS, npc.Center);
			//Projectile p = Projectile.NewProjectileDirect(npc.position, Vector2.UnitX*-10,1,50,1,Main.myPlayer);
		}
		public void SetFrame() {
			int FN = GetFrameC();
			switch (MoveMode)
			{
				case EMoveMode.Waiting:
					{
						FN += 1;
						if (FN > 3) FN = 0;
						npc.frame = GetFrame(FN);
					}
					break;
				case EMoveMode.Jumping:
					{
						if(npc.velocity.Y>0) npc.frame = GetFrame(4);
						else npc.frame = GetFrame(5);
					}
					break;
				case EMoveMode.Teleport:
					{
						FN += 1;
						if (FN > 3) FN = 0;
						npc.frame = GetFrame(FN);
					}
					break;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			if (Main.time%15==0) SetFrame();
		}
		public override bool CheckActive()
		{
			return npc.life<=0;
		}
		public override bool CheckDead()
		{
			ParasiticNanitesWorld.KingSlimeCount -= 1;
			return base.CheckDead();
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(npc.Center, (int)(damage/4f), false, npc.whoAmI + 1);
		}
		public override void NPCLoot()
		{
			Item.NewItem(npc.Hitbox,ModContent.ItemType<Items.ParasiticNanitesSignalTransmitter>(),5);
			Item.NewItem(npc.Hitbox, ModContent.ItemType<Items.ParasiticNanitesItem>(), 50);
			if (!ParasiticNanitesWorld.downedPNKingSlime)
			{
				ParasiticNanitesWorld.downedPNKingSlime = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
				}
			}

			XxDefinitions.Utils.SummonUtils.SummonDustExplosion(npc.Center, 48, 0, 0, PNDustType, 16, 16, 4);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			XxDefinitions.Utils.SpriteBatchUsingEffect(spriteBatch);
			Effects.ParasiticNanitesDraw.UseEffect(PNDXY, TXY, npc.frame.Location, drawColor);
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			XxDefinitions.Utils.SpriteBatchEndUsingEffect(spriteBatch);
		}
		public Point PNDXY;
	}
}
