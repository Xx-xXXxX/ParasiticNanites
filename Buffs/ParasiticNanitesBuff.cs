using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ParasiticNanites.Buffs
{
	public class ParasiticNanitesBuff:ModBuff
	{
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Parasitic Nanites");
            DisplayName.AddTranslation(GameCulture.Chinese, "寄生机器人");
            Description.SetDefault("Parasitic Nanites use your life to clone itself");
            Description.AddTranslation(GameCulture.Chinese, "寄生机器人用你克隆它自己");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            this.canBeCleared = false;
            Main.lightPet[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            this.longerExpertDebuff = false;
            Main.pvpBuff[Type] = false;
            Main.vanityPet[Type] = false;
            ModTranslation DRS = mod.CreateTranslation("KilledByParasiticNanites");
            DRS.SetDefault("Became Parasitic Nanites");
            DRS.AddTranslation(GameCulture.Chinese, "变成了寄生机器人");
            mod.AddTranslation(DRS);
            ModTranslation NPN = mod.CreateTranslation("NumberOfParasiticNanites");
            NPN.SetDefault("There are {0} Parasitic Nanites on you");
            NPN.AddTranslation(GameCulture.Chinese, "有{0}个寄生机器人在你身上");
            mod.AddTranslation(NPN);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.buffTime[buffIndex] += 1;
            int Num = npc.buffTime[buffIndex];
            if ((npc.boss) ? (Main.time % (20 * ParasiticNanites.ParasiticNanitesUpdateTime) == 0) : (Main.time % (12 * ParasiticNanites.ParasiticNanitesUpdateTime) == 0)) {
                npc.StrikeNPC(npc.buffTime[buffIndex],0,0);
                npc.buffTime[buffIndex] +=(int)Math.Floor( Math.Log(Math.Sqrt(Num) + 1) * Math.Exp(Math.Sqrt(Math.Log(npc.life + 1))));
            }
            if (Main.time % (12 * ParasiticNanites.ParasiticNanitesUpdateTime) == 0) {
                npc.AddBuff(ModContent.BuffType<ParasiticNanitesReduceBuff>(), (int)Math.Ceiling(Math.Log(Num + 1)));
            }
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"{npc.buffTime[buffIndex]}", npc.Center + new Vector2(0, +48+32));
            if (npc.life < (npc.lifeMax / 2))
            {
                if ((NPCs.ParasiticNanitesSlime.SlimeNPC.Contains(npc.type)))
                {
                    NPC newnpc = Main.npc[NPC.NewNPC((int)npc.position.X, (int)npc.position.Y-1, ModContent.NPCType<NPCs.ParasiticNanitesSlime>())];
                    newnpc.scale = npc.scale;
                    npc.ai.CopyTo(newnpc.ai, 0);
                    npc.localAI.CopyTo(newnpc.localAI, 0);
                    newnpc.position = npc.position+new Vector2(0,-4);
                    newnpc.velocity = npc.velocity;
                    //npc.StrikeNPC(npc.lifeMax * 2, 0, 0, false, true, false);
                    npc.active = false;
                }
                else if (npc.type == Terraria.ID.NPCID.KingSlime) {
                    NPC newnpc = Main.npc[NPC.NewNPC((int)npc.position.X, (int)npc.position.Y-1, ModContent.NPCType<NPCs.ParasiticNanitesKingSlime>())];
                    //newnpc.scale = npc.scale;
                    newnpc.position = npc.position;
                    newnpc.velocity = npc.velocity;
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.NewText(Language.GetTextValue("Announcement.HasAwoken", newnpc.TypeName), 175, 75);
                    }
                    else if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", newnpc.GetTypeNetName()), new Color(175, 75, 255));
                    }
                    //npc.StrikeNPC(npc.lifeMax * 2, 0, 0, false, true, false);
                    Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(4,8)+new Vector2(0,-16), Terraria.ID.GoreID.KingSlimeCrown);
                    Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(4, 8) + new Vector2(0, -16), mod.GetGoreSlot("Gores/Ninja_Head"));
                    //Dust.NewDustPerfect(npc.Center,  ModContent.DustType<Dusts.Ninja_Head>(),Main.rand.NextVector2Circular(4, 8) + new Vector2(0, -16));
                    npc.active = false;
                }
            }
        }
		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
            npc.buffTime[buffIndex] += time;
            return true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] += 1;
            int Num = player.buffTime[buffIndex];
            if (Main.time % (12*ParasiticNanites.ParasiticNanitesUpdateTime) == 0)
            {
                //player.(player.buffTime[buffIndex], 0, 0);
                //Main.player
                //Terraria.ModLoader.ModTranslation DR = new ModTranslation($"{player.name} Were Eaten By Parasitic Nanites");
                //string DRS = $"{player.name} Were Eaten By Parasitic Nanites";
                
                //if (Terraria.G) { }
                player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name+" "+Language.GetTextValue("Mods.ParasiticNanites.KilledByParasiticNanites")), player.buffTime[buffIndex],0);
                player.buffTime[buffIndex] += (int)Math.Floor(Math.Log(Math.Sqrt(Num) + 1) * Math.Exp(Math.Sqrt(Math.Log(player.statLife + 1))));
            }
            if (Main.time % (6 * ParasiticNanites.ParasiticNanitesUpdateTime) == 0) {
                player.AddBuff(ModContent.BuffType<ParasiticNanitesReduceBuff>(), (int)Math.Ceiling(Math.Log(Num + 1)));
            }
            XxDefinitions.XDebugger.Utils.AddDraw.AddDrawString($"ParasiticNanites:{player.buffTime[buffIndex]}\n+={(int)Math.Floor(Math.Log(Math.Sqrt(Num) + 1) * Math.Exp(Math.Sqrt(Math.Log(player.statLife + 1))))}\n-={(int)Math.Ceiling(Math.Log(Num + 1))}",player.Center+new Vector2(0,-128));
        }
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            //Main.NewText($"Add:{time}");
            player.buffTime[buffIndex] += time;
            return true;
        }
		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
            tip = Language.GetTextValue("Mods.ParasiticNanites.NumberOfParasiticNanites");
            int bt = ModContent.BuffType<ParasiticNanitesBuff>();
            if (Main.player[Main.myPlayer].HasBuff(bt)) {
                int i = Main.player[Main.myPlayer].FindBuffIndex(bt);
                //tip = tip.Replace("%1", Main.player[Main.myPlayer].buffTime[i].ToString());
                tip = string.Format(tip, Main.player[Main.myPlayer].buffTime[i].ToString());
            }
            //Terraria.ID.BuffID.ManaSickness
		}
	}
}
