using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using log4net;
using ParasiticNanites.Effects;
using XxDefinitions;

namespace ParasiticNanites
{
	public class ParasiticNanites : Mod
	{
		public static bool ProjBoom = false;
		public static bool ProjChasing = false;
		public static bool ProjSpike = false;
		public static ParasiticNanites Instance;
		public static class ParasiticNanitesMode {
			public static bool ParasiticNanitesProjSpread = false;
		}
		public static int ParasiticNanitesProjNum = 0;
		public static int ParasiticNanitesProjMaxNum = 256;
		public static int ParasiticNanitesUpdateTime = 2;
		public static HashSet<int> ParasiticNanitesTiles = new HashSet<int>();
		public ParasiticNanites() {
			Instance = this;
			NetPacketTreeMain = new XxDefinitions.NetPacketTreeMain<int>(this, XxDefinitions.BinaryIOFunc.WriteBinaryInt, XxDefinitions.BinaryIOFunc.ReadBinaryInt);
			Projectiles.ParasiticNanitesProj.Load();
		}
		public override void Load()
		{

		}
		public override void PostSetupContent()
		{
			ParasiticNanitesDraw.SetDef(this);
			ParasiticNanitesDraw.Update();
			ParasiticNanitesDrawTiles.SetDef(this);
			ParasiticNanitesDrawTiles.Update();
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
			{
				bossChecklist.Call(
					"AddBossWithInfo",
					11.01f,
					ModContent.NPCType<NPCs.ParasiticNanitesKingSlime>(),
					this, // Mod
					"$Mods.ParasiticNanites.NPCNames.ParasiticNanitesKingSlime",
					(Func<bool>)(() => ParasiticNanitesWorld.downedPNKingSlime),
					null,
					null,
					new List<int> { ModContent.ItemType<Items.ParasiticNanitesSignalTransmitter>(), ModContent.ItemType<Items.ParasiticNanitesItem>() },
					"$Mods.ParasiticNanites.BossSpawnInfo.ParasiticNanitesKingSlime"
				);
			}
		}
		public override void PreSaveAndQuit()
		{
			//Terraria.ID.TileID.Dirt;
			//Terraria.ID.TileID.CorruptGrass;//¸¯»¯
			//Terraria.ID.TileID.S
			//Terraria.ID.TileID.CorruptHardenedSand;
			//Terraria.ID.TileID.CrimsonHardenedSand;
			//Terraria.ID.TileID.HallowHardenedSand;
			//Terraria.ID.TileID.Corrupt
			//Terraria.ID.TileID.Sets.
			/*
			ParasiticNanitesProjNum = 0;
			Myrandom = new Terraria.Utilities.UnifiedRandom( Main.rand.Next(v c));*/
			ParasiticNanitesProjNum = 0;
		}
		public override void PreUpdateEntities()
		{
			ParasiticNanitesMode.ParasiticNanitesProjSpread = false;
			ProjBoom = false;
			ProjChasing = false;
			ProjSpike = false;
			if (Main.netMode == Terraria.ID.NetmodeID.Server&&Main.time%30==1) {
				Projectiles.ParasiticNanitesProj.GetRandNet.AutoDo();
			}
			AdjustParasiticNanites();
			SeparateParasiticNanites();
			ParasiticNanitesDraw.Update();
			if(Main.time%60==17)
				ParasiticNanitesDrawTiles.Update();
		}
		public override void PostUpdateEverything()
		{
		}
		public override void Unload()
		{
			NetPacketTreeMain = null;
			Projectiles.ParasiticNanitesProj.GetRandNet=null;
			Instance = null;
			ParasiticNanitesDraw.Unload();
			ParasiticNanitesDrawTiles.Unload();
			log = null;
		}
		~ParasiticNanites() {
		}
		public static int AdjustParasiticNanitesNow = 0;
		public const int MaxAPNNum = 50;
		public static int AdjustParasiticNanitesNum => (int)Math.Ceiling(MaxAPNNum*Math.Pow((float)(ParasiticNanitesProjNum) / (ParasiticNanitesProjMaxNum),2));
		public const int MaxAPNL = 64;
		public static int AdjustParasiticNanitesLength=> (int)Math.Ceiling(MaxAPNL * Math.Pow((float)(ParasiticNanitesProjNum) / (ParasiticNanitesProjMaxNum), 2));
		public static int MergeParasiticNanites(int a, int b) {
			Projectile A = Main.projectile[a];
			Projectile B = Main.projectile[b];
			int c = Projectiles.ParasiticNanitesProj.NewParasiticNanitesProjPerfect(
				   (A.Center+ B.Center)/2, 
				   (A.velocity + B.velocity) / 2,
				   A.damage+B.damage, Projectiles.ParasiticNanitesProj.ParasiticNanitesProjFlying(A)|| Projectiles.ParasiticNanitesProj.ParasiticNanitesProjFlying(B),
				   (int)((A.ai[0]==B.ai[0])?(A.ai[0]):(0)));
			Projectile C = Main.projectile[c];
			if (A.localAI[0] != 0 && A.localAI[0] != Projectiles.ParasiticNanitesProj.SpecialDeath)
				C.localAI[0] = A.localAI[0];

			if (B.localAI[0] != 0 && B.localAI[0] != Projectiles.ParasiticNanitesProj.SpecialDeath)
				C.localAI[0] = B.localAI[0];
			A.localAI[0] = Projectiles.ParasiticNanitesProj.SpecialDeath;
			B.localAI[0] = Projectiles.ParasiticNanitesProj.SpecialDeath;
			C.timeLeft = (A.timeLeft + B.timeLeft) / 2;
			C.localAI[1] = C.timeLeft;
			A.Kill();
			B.Kill();
			return c;
		}
		public static bool AbleToDo(int id) {
			Projectile i = Main.projectile[id];
			return (i.active) && (i.type == ModContent.ProjectileType<Projectiles.ParasiticNanitesProj>()) &&( i.timeLeft <= (i.localAI[1] - 15));
		}
		public static bool AbleToDo(Projectile i)
		{
			return (i.active) && (i.type == ModContent.ProjectileType<Projectiles.ParasiticNanitesProj>()) && (i.timeLeft <= (Projectiles.ParasiticNanitesProj.TimeMax - 15));
		}
		private ILog log = LogManager.GetLogger("ParasiticNanites");
		public static ILog Logging => Instance.log;
		//public class NPTM : XxDefinitions.CtorByF<XxDefinitions.NetPacketTreeMain<int>> {
		//	public override NetPacketTreeMain<int> F()=> new XxDefinitions.NetPacketTreeMain<int>(null, XxDefinitions.BinaryIOFunc.WriteBinaryInt, XxDefinitions.BinaryIOFunc.ReadBinaryInt);
		//}
		public static XxDefinitions.NetPacketTreeMain<int> NetPacketTreeMain;
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			NetPacketTreeMain.Handle(reader,whoAmI);
		}
		public static int SPNMin = 32;
		public static int SeparateParasiticNanitesMinNum => (int)(SPNMin *XxDefinitions.Utils.CalculateUtils.FastlyIncreaseToInf( Math.Pow((float)(ParasiticNanitesProjNum) / (ParasiticNanitesProjMaxNum), 1f/2)))+ SPNMin;
		public static int SeparateParasiticNanitesNum => (int)((MaxAPNNum - AdjustParasiticNanitesNum) / 5);
		public static void AdjustParasiticNanites()
		{
			int APNNum = AdjustParasiticNanitesNum;
			if (APNNum <= 1) return;
			if (AdjustParasiticNanitesNow >= Main.projectile.Length) AdjustParasiticNanitesNow = 0;
			//int PID = ModContent.ProjectileType<Projectiles.ParasiticNanitesProj>();
			int APNLen = AdjustParasiticNanitesLength;
			int i = AdjustParasiticNanitesNow + 1;
			
			//int SPNM = SeparateParasiticNanitesMinNum;
			//if (ParasiticNanitesProjNum >= ParasiticNanitesProjMaxNum) SPNM = 0;
			//int SPNN = SeparateParasiticNanitesNum;
			//int SPNi = 0;
			while (!(AbleToDo(i)) && (i != AdjustParasiticNanitesNow))
			{
				i += 1;
				if (i >= Main.projectile.Length) i = 0;
			}
			if (i == AdjustParasiticNanitesNow) return;
			Projectile I = Main.projectile[i];
			List<Projectile> APNL = new List<Projectile>
			{
				Main.projectile[i]
			};
			i += 1;
			while ((APNL.Count < APNNum) && i != (AdjustParasiticNanitesNow))
			{
				I = Main.projectile[i];
				if (AbleToDo(i))
				{
					
						bool Deled = false;
						for (int k = 0; k < APNL.Count; ++k)
						{
							Projectile J = APNL[k];
							if ((J.Center - I.Center).Length() <= APNLen + J.scale * 9 + I.scale * 9)
							{
								XxDefinitions.XDebugger.Utils.AddDraw.AddDrawLineTo(I.Center, J.Center, DrawTime: 3);
								APNL.RemoveAt(k);//k -= 1;
								MergeParasiticNanites(i, J.whoAmI);
								Deled = true;
								break;
							}
						}
						if (!Deled) APNL.Add(I);
				}
				i += 1;
				if (i >= Main.projectile.Length) i = 0;
			}
			AdjustParasiticNanitesNow = i;
		}
		public static void SeparatePrarsiticNano(int i) {
			Projectile I = Main.projectile[i];
			if (Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanitesNum(I.damage) <= 1) return;
			XxDefinitions.XDebugger.Utils.AddDraw.AddDrawCircle(I.Center, 16, DrawTime: 3);
			XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{I.damage},{I.timeLeft}", I.Center, time: 15);
			List<int> IDs = Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(I.Center, I.damage, Projectiles.ParasiticNanitesProj.ParasiticNanitesProjFlying(I), (int)I.ai[0], I.velocity.Length() / 1.5f + 4);
			foreach (var k in IDs)
			{
				Main.projectile[k].timeLeft = Math.Min(Projectiles.ParasiticNanitesProj.TimeMax, I.timeLeft + 5);
				Main.projectile[k].localAI[1] = Main.projectile[k].timeLeft;
				Main.projectile[k].localAI[0] = I.localAI[0];
			}
			I.localAI[0] = Projectiles.ParasiticNanitesProj.SpecialDeath;
			I.Kill();
		}
		public static void LimAdd(ref int i, int max) {
			i += 1;if (i >= max) i = 0;
		}
		public static int LastSeparateParasiticNanitesN = 0;
		public static void SeparateParasiticNanites()
		{/*
			if (ParasiticNanitesProjNum >= ParasiticNanitesProjMaxNum) return;
			int SPNM = SeparateParasiticNanitesMinNum;
			if (AdjustParasiticNanitesNow >= Main.projectile.Length) AdjustParasiticNanitesNow = 0;
			//int PID = ModContent.ProjectileType<Projectiles.ParasiticNanitesProj>();
			int i = AdjustParasiticNanitesNow + 1;
			int SPNN = SeparateParasiticNanitesNum;
			int j = 0;
			for (j = 0; j < SPNN; ++j) {
				while (!(AbleToDo(i) && Main.projectile[i].damage > SPNM) && (i != AdjustParasiticNanitesNow))
				{
					i += 1;
					if (i >= Main.projectile.Length) i = 0;
				}
				if (i == AdjustParasiticNanitesNow) return;
				Projectile I = Main.projectile[i];
				if (Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanitesNum(I.damage) <= 1) continue;
				XxDefinitions.XDebugger.Utils.AddDraw.AddDrawCircle(I.Center,16,DrawTime:3);
				XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{I.damage},{I.timeLeft}", I.Center, time:15);
				List<int> IDs= Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(I.Center, I.damage, Projectiles.ParasiticNanitesProj.ParasiticNanitesProjFlying(I), (int)I.ai[0],I.velocity.Length()/1.5f+4);
				foreach (var k in IDs) {
					Main.projectile[k].timeLeft = Math.Min(Projectiles.ParasiticNanitesProj.TimeMax,I.timeLeft+5);
					Main.projectile[k].localAI[1] = Main.projectile[k].timeLeft;
				}
				I.localAI[0] = Projectiles.ParasiticNanitesProj.SpecialDeath;
				I.Kill();
			}
			AdjustParasiticNanitesNow = i;*/
			if (ParasiticNanitesProjNum >= ParasiticNanitesProjMaxNum * 0.9f) return;
			int MaxID=-1;
			int MaxN = SeparateParasiticNanitesMinNum;
			for (int i = LastSeparateParasiticNanitesN+1; i != LastSeparateParasiticNanitesN; LimAdd(ref i,Main.projectile.Length)) {
				if (AbleToDo(i) && Main.projectile[i].damage > MaxN) {
					MaxID = i;
					MaxN = Main.projectile[i].damage;
				}
			}
			if (MaxID == -1) return;
			SeparatePrarsiticNano(MaxID);
			LastSeparateParasiticNanitesN = MaxID;
		}
	}
}