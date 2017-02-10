// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33813,y:32710,varname:node_3138,prsc:2|emission-7241-RGB,custl-7241-RGB,voffset-3721-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31284,y:32584,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Color,id:6132,x:31284,y:32823,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:_Color2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.2,c3:0.4,c4:1;n:type:ShaderForge.SFN_Lerp,id:9296,x:31709,y:32778,varname:node_9296,prsc:2|A-7241-RGB,B-6132-RGB,T-8546-OUT;n:type:ShaderForge.SFN_ComponentMask,id:151,x:32132,y:31839,varname:node_151,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4361-Z;n:type:ShaderForge.SFN_ValueProperty,id:855,x:32436,y:31785,ptovrint:False,ptlb:Value,ptin:_Value,varname:_Value,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:25;n:type:ShaderForge.SFN_Multiply,id:6692,x:32690,y:31840,varname:node_6692,prsc:2|A-855-OUT,B-9836-OUT,C-5460-OUT;n:type:ShaderForge.SFN_Sin,id:2703,x:32867,y:31840,varname:node_2703,prsc:2|IN-6692-OUT;n:type:ShaderForge.SFN_RemapRange,id:8546,x:33224,y:31839,varname:node_8546,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-2703-OUT;n:type:ShaderForge.SFN_Add,id:9836,x:32308,y:31955,varname:node_9836,prsc:2|A-151-OUT,B-4014-OUT;n:type:ShaderForge.SFN_Time,id:1394,x:31868,y:32050,varname:node_1394,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:4361,x:31883,y:31849,varname:node_4361,prsc:2;n:type:ShaderForge.SFN_Slider,id:9119,x:31789,y:32256,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:_Speed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01,max:1;n:type:ShaderForge.SFN_Multiply,id:4014,x:32132,y:32070,varname:node_4014,prsc:2|A-1394-T,B-9119-OUT;n:type:ShaderForge.SFN_Tau,id:5460,x:32490,y:32012,varname:node_5460,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3721,x:32281,y:32493,varname:node_3721,prsc:2|A-2703-OUT,B-5167-OUT;n:type:ShaderForge.SFN_Slider,id:5167,x:32124,y:32658,ptovrint:False,ptlb:Wave,ptin:_Wave,varname:_Wave,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.05,max:1;proporder:7241-6132-855-9119-5167;pass:END;sub:END;*/

Shader "Lagash/GameLogo" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Color2 ("Color2", Color) = (0.07843138,0.2,0.4,1)
        _Value ("Value", Float ) = 25
        _Speed ("Speed", Range(0, 1)) = 0.01
        _Wave ("Wave", Range(-1, 1)) = 0.05
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Value;
            uniform float _Speed;
            uniform float _Wave;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
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
                float3 emissive = _Color.rgb;
                float3 finalColor = emissive + _Color.rgb;
                return fixed4(finalColor,1);
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
