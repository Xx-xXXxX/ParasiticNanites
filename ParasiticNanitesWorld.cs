using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParasiticNanites
{
	public class ParasiticNanitesWorld:ModWorld
	{
		private static int counter = 0;
		public static bool downedPNKingSlime;
		public static int KingSlimeCount;
		public static ulong RandSF = 0;
		public static ulong NewRSF() {
			return RandSF=Terraria.Utils.RandomNextSeed(RandSF);
		}
		public override void Initialize()
		{
			downedPNKingSlime = false;
			RandSF =0;
			KingSlimeCount = -1;
			counter = 0;
		}
		public override TagCompound Save()
		{
			var downed = new List<string>();
			if (downedPNKingSlime)
			{
				downed.Add("PNKingSlime");
			}
			return new TagCompound
			{
				["downed"] = downed,
				["RS"] = RandSF
			};
		}
		public override void Load(TagCompound tag)
		{

			if (tag.ContainsKey("downed"))
			{
				var downed = tag.GetList<string>("downed");
				downedPNKingSlime = downed.Contains("PNKingSlime");
			}
			if (tag.ContainsKey("RS"))
			{
				RandSF = (ulong)tag.GetAsLong("RS");
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = downedPNKingSlime;
			writer.Write(flags);
			writer.Write(RandSF);
			writer.Write(counter);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedPNKingSlime = flags[0];
			RandSF=reader.ReadUInt64();
			counter = reader.ReadInt32();
		}
		public override void PostUpdate()
		{
			counter += 1;
			if (counter % 30 == 1) {
				KingSlimeCount = NPC.CountNPCS(ModContent.NPCType<NPCs.ParasiticNanitesKingSlime>());
			}
			if (counter > 10000) counter = 0;
		}
	}
}
