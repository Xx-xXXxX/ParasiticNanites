sampler uImage0 : register(s0);
texture Texture;
sampler TextureSampler = sampler_state{Texture = <Texture>;};
float4 dcolor;
int Nx;//Move
int Ny;//Move
int Tx;//Texture
int Ty;//Texture
int Sx;//Size
int Sy;//Size
int Ox;//Origin
int Oy;//Origin
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0 {
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    float2 N={ (coords.x*Sx-Ox+Nx) /Tx , (coords.y*Sy-Oy+Ny) /Ty };
    float4 Ncolor=tex2D(TextureSampler, N);
    if(Ncolor.a!=0){
        float4 gcolor=(Ncolor*0.75f+color*0.25f)*dcolor;
        return gcolor;
    }
    else{
        return color*dcolor;
    }
}
technique Technique1 {
    pass PNDT {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}