// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33813,y:32710,varname:node_3138,prsc:2|emission-1855-OUT,custl-9296-OUT,alpha-54-OUT,voffset-3721-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31912,y:32424,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Color,id:6132,x:31912,y:32663,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:_Color2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.2,c3:0.4,c4:1;n:type:ShaderForge.SFN_Lerp,id:9296,x:32366,y:32560,varname:node_9296,prsc:2|A-7241-RGB,B-6132-RGB,T-2703-OUT;n:type:ShaderForge.SFN_ComponentMask,id:151,x:32132,y:31839,varname:node_151,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4361-Z;n:type:ShaderForge.SFN_ValueProperty,id:855,x:32436,y:31785,ptovrint:False,ptlb:Value,ptin:_Value,varname:_Value,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:25;n:type:ShaderForge.SFN_Multiply,id:6692,x:32690,y:31840,varname:node_6692,prsc:2|A-855-OUT,B-9836-OUT,C-5460-OUT;n:type:ShaderForge.SFN_Sin,id:2703,x:32867,y:31840,varname:node_2703,prsc:2|IN-6692-OUT;n:type:ShaderForge.SFN_Add,id:9836,x:32308,y:31955,varname:node_9836,prsc:2|A-151-OUT,B-4014-OUT;n:type:ShaderForge.SFN_Time,id:1394,x:31432,y:31954,varname:node_1394,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:4361,x:31883,y:31849,varname:node_4361,prsc:2;n:type:ShaderForge.SFN_Slider,id:9119,x:31353,y:32160,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:_Speed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01,max:1;n:type:ShaderForge.SFN_Multiply,id:4014,x:31694,y:31971,varname:node_4014,prsc:2|A-1394-T,B-9119-OUT;n:type:ShaderForge.SFN_Tau,id:5460,x:32490,y:32012,varname:node_5460,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3721,x:33274,y:32150,varname:node_3721,prsc:2|A-2703-OUT,B-5167-OUT;n:type:ShaderForge.SFN_Slider,id:5167,x:32889,y:32172,ptovrint:False,ptlb:Wave,ptin:_Wave,varname:_Wave,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.05,max:1;n:type:ShaderForge.SFN_Tex2d,id:3890,x:31538,y:33309,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0481ba26e3743de4bba3fa6de5a6247e,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1021,x:32365,y:33410,varname:node_1021,prsc:2|A-9296-OUT,B-5827-OUT;n:type:ShaderForge.SFN_Slider,id:4873,x:31502,y:33602,ptovrint:False,ptlb:LinesPower,ptin:_LinesPower,varname:_LinesPower,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:5,max:5;n:type:ShaderForge.SFN_Power,id:7235,x:31821,y:33430,varname:node_7235,prsc:2|VAL-3890-RGB,EXP-4873-OUT;n:type:ShaderForge.SFN_OneMinus,id:5827,x:32011,y:33430,varname:node_5827,prsc:2|IN-7235-OUT;n:type:ShaderForge.SFN_Color,id:9451,x:32793,y:33039,ptovrint:False,ptlb:LinesColor,ptin:_LinesColor,varname:_LinesColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:6643,x:32835,y:33393,varname:node_6643,prsc:2|A-2703-OUT,B-1021-OUT;n:type:ShaderForge.SFN_Multiply,id:1855,x:33019,y:33154,varname:node_1855,prsc:2|A-9451-RGB,B-6643-OUT;n:type:ShaderForge.SFN_Slider,id:711,x:33211,y:33505,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:_Opacity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:10,max:10;n:type:ShaderForge.SFN_Tex2d,id:6255,x:33290,y:33262,ptovrint:False,ptlb:LinesOpacity,ptin:_LinesOpacity,varname:_LinesOpacity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7a0737058a85d4348bd0a5f9f3cbe6fe,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:54,x:33539,y:33335,varname:node_54,prsc:2|A-6255-A,B-711-OUT;proporder:7241-6132-855-9119-5167-3890-4873-9451-711-6255;pass:END;sub:END;*/

Shader "Lagash/GameLogo2" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Color2 ("Color2", Color) = (0.07843138,0.2,0.4,1)
        _Value ("Value", Float ) = 25
        _Speed ("Speed", Range(0, 1)) = 0.01
        _Wave ("Wave", Range(-1, 1)) = 0.05
        _Texture ("Texture", 2D) = "bump" {}
        _LinesPower ("LinesPower", Range(-5, 5)) = 5
        _LinesColor ("LinesColor", Color) = (0,1,1,1)
        _Opacity ("Opacity", Range(0, 10)) = 10
        _LinesOpacity ("LinesOpacity", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float4 _Color2;
            uniform float _Value;
            uniform float _Speed;
            uniform float _Wave;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _LinesPower;
            uniform float4 _LinesColor;
            uniform float _Opacity;
            uniform sampler2D _LinesOpacity; uniform float4 _LinesOpacity_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_1394 = _Time + _TimeEditor;
                float node_2703 = sin((_Value*(mul(unity_ObjectToWorld, v.vertex).b.r+(node_1394.g*_Speed))*6.28318530718));
                float node_3721 = (node_2703*_Wave);
                v.vertex.xyz += float3(node_3721,node_3721,node_3721);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_1394 = _Time + _TimeEditor;
                float node_2703 = sin((_Value*(i.posWorld.b.r+(node_1394.g*_Speed))*6.28318530718));
                float3 node_9296 = lerp(_Color.rgb,_Color2.rgb,node_2703);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 emissive = (_LinesColor.rgb*(node_2703*(node_9296*(1.0 - pow(_Texture_var.rgb,_LinesPower)))));
                float3 finalColor = emissive + node_9296;
                float4 _LinesOpacity_var = tex2D(_LinesOpacity,TRANSFORM_TEX(i.uv0, _LinesOpacity));
                return fixed4(finalColor,(_LinesOpacity_var.a*_Opacity));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers gles3 metal xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Value;
            uniform float _Speed;
            uniform float _Wave;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 node_1394 = _Time + _TimeEditor;
                float node_2703 = sin((_Value*(mul(unity_ObjectToWorld, v.vertex).b.r+(node_1394.g*_Speed))*6.28318530718));
                float node_3721 = (node_2703*_Wave);
                v.vertex.xyz += float3(node_3721,node_3721,node_3721);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
