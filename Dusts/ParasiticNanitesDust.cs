using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace ParasiticNanites.Dusts
{
	public class ParasiticNanitesDust:ModDust
	{
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale *= 3;
            //dust.frame = new Rectangle(1, 1, 6, 6);
            //dust.scale *= 5f;
            //dust.velocity *= 0.25f;
            
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= 0.9f;
            dust.scale -= 0.1f;
            dust.rotation += (dust.dustIndex%4-1.5f)* dust.scale/30;
            if (dust.scale <= 0.5f) dust.active = false;
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return lightColor;
        }
    }
}
