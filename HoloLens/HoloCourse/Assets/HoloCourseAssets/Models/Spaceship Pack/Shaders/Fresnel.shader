Shader "Spaceships/Fresnel" {

Properties {

    _Color ("Main Color", Color) = (1,1,1,1)

    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)

    _Shininess ("Shininess", Range (0.01, 1)) = 0.078125

    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}

    _Illum ("Illumin (A)", 2D) = "white" {}

    _BumpMap ("Normalmap", 2D) = "bump" {}

    _EmissionLM ("Emission (Lightmapper)", Float) = 0

    _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)

    _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0

}

SubShader {

    Tags { "RenderType"="Opaque" }

    LOD 400

CGPROGRAM

#pragma surface surf BlinnPhong

 

sampler2D _MainTex;

sampler2D _BumpMap;

sampler2D _Illum;

fixed4 _Color;

half _Shininess;

float4 _RimColor;

float _RimPower;

 

struct Input {

    float2 uv_MainTex;

    float2 uv_Illum;

    float2 uv_BumpMap;

    float3 viewDir;

};

 

void surf (Input IN, inout SurfaceOutput o) {

    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

    fixed4 c = tex * _Color;

    o.Albedo = c.rgb;

    

    o.Gloss = tex.a;

    o.Alpha = c.a;

    o.Specular = _Shininess;

    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

    half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

    o.Emission =  c.rgb * tex2D(_Illum, IN.uv_Illum).a  + pow (rim, _RimPower) * _RimColor.rgb;

}

ENDCG

}

FallBack "Self-Illumin/Specular"

}