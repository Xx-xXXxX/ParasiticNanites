using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ParasiticNanites.Dusts
{
	public class Ninja_Head:ModDust
	{
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 0;
            dust.frame = new Rectangle(0, 0, 22, 20);
            dust.noGravity = false;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if(!dust.noGravity)
            dust.velocity.Y += 0.3f;
            dust.alpha += 2;
            dust.rotation += (dust.dustIndex % 4 - 1.5f) * dust.scale / 30;
            if (dust.alpha>=255) dust.active = false;
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            Color color = Color.White;
            color.A = (byte)(255 - dust.alpha);
            return color;
        }
    }
}
