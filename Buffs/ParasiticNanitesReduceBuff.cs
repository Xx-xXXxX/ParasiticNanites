using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using ParasiticNanites.Projectiles;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace ParasiticNanites.Buffs
{
	public class ParasiticNanitesReduceBuff:ModBuff
	{
        public override void SetDefaults()
        {
            DisplayName.SetDefault("ParasiticNanitesReduce");
            DisplayName.AddTranslation(GameCulture.Chinese, "寄生机器人减少");
            Description.SetDefault("Parasitic Nanites Can't Last Long");
            Description.AddTranslation(GameCulture.Chinese, "寄生机器人待不了多久");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            this.canBeCleared = false;
            Main.lightPet[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            this.longerExpertDebuff = false;
            Main.pvpBuff[Type] = false;
            Main.vanityPet[Type] = false;
        }
		public override void Update(NPC npc, ref int buffIndex)
		{
            npc.buffTime[buffIndex] += 1;
            if (Main.time % (8 * ParasiticNanites.ParasiticNanitesUpdateTime) == 3)
            {
                //npc.AddBuff(ModContent.BuffType<ParasiticNanitesBuff>(), -npc.buffTime[buffIndex]);
                int ParasiticNanitesNum = 0;
                if (npc.HasBuff(ModContent.BuffType<ParasiticNanitesBuff>()))
                {
                    int ParasiticNanitesIndex = npc.FindBuffIndex(ModContent.BuffType<ParasiticNanitesBuff>());
                    Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(npc.Center, Math.Min(npc.buffTime[buffIndex], npc.buffTime[ParasiticNanitesIndex]), npc.noGravity,npc.whoAmI+1);
                    npc.buffTime[ParasiticNanitesIndex] -= npc.buffTime[buffIndex];
                    if (npc.buffTime[ParasiticNanitesIndex] < 0) npc.buffTime[ParasiticNanitesIndex] = 0;
                    ParasiticNanitesNum = npc.buffTime[ParasiticNanitesIndex];
                }
                npc.buffTime[buffIndex] -= (int)(Math.Floor(XxDefinitions.Utils.CalculateUtils.SlowlyIncreaseRaw(npc.buffTime[buffIndex], 2f)) / 2f *XxDefinitions.Utils.CalculateUtils.SlowlyDecreaseLim1To0(ParasiticNanitesNum));
                if (npc.buffTime[buffIndex] < 0) npc.buffTime[buffIndex] = 0;
            }
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{npc.buffTime[buffIndex]}", npc.Center + new Vector2(0, +48 + 16));
            //XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"ParasiticNanites:{player.buffTime[buffIndex]}\n+={(int)Math.Floor(Math.Log(Math.Sqrt(Num) + 1) * Math.Exp(Math.Sqrt(Math.Log(player.statLife + 1))))}\n-={(int)Math.Ceiling(Math.Log(Num + 1))}", player.Center + new Vector2(0, 128));
        }
		public override void Update(Player player, ref int buffIndex)
		{
            player.buffTime[buffIndex] += 1;
            if (Main.time % (8 * ParasiticNanites.ParasiticNanitesUpdateTime) == 3)
            {
                //player.AddBuff(ModContent.BuffType<ParasiticNanitesBuff>(), -player.buffTime[buffIndex]);
                int ParasiticNanitesNum = 0;
                if ( player.HasBuff(ModContent.BuffType<ParasiticNanitesBuff>()) )
                {
                    int ParasiticNanitesIndex = player.FindBuffIndex(ModContent.BuffType<ParasiticNanitesBuff>());
                    Projectiles.ParasiticNanitesProj.SummonSomeParasiticNanites(player.Center, Math.Min(player.buffTime[buffIndex], player.buffTime[ParasiticNanitesIndex]), player.gravity == 0,-(player.whoAmI+1));
                    player.buffTime[ParasiticNanitesIndex] -= player.buffTime[buffIndex];
                    if (player.buffTime[ParasiticNanitesIndex] < 0) player.buffTime[ParasiticNanitesIndex] = 0;
                    ParasiticNanitesNum = player.buffTime[ParasiticNanitesIndex];
                }
                player.buffTime[buffIndex] -= (int)(Math.Floor(XxDefinitions.Utils.CalculateUtils.SlowlyIncreaseRaw(player.buffTime[buffIndex], 2f)) / 2f * XxDefinitions.Utils.CalculateUtils.SlowlyDecreaseLim1To0(ParasiticNanitesNum));
                if (player.buffTime[buffIndex] < 0) player.buffTime[buffIndex] = 0;
            }
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"ParasiticNanitesReduceBuff:{player.buffTime[buffIndex]}\n", player.Center + new Vector2(0, 128));
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
		{
            npc.buffTime[buffIndex] += time;
            return true;
        }
		public override bool ReApply(Player player, int time, int buffIndex)
		{
            player.buffTime[buffIndex] += time;
            return true;
        }
	}
}
