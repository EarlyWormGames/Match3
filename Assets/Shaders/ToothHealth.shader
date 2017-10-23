// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32273,y:32254,ptovrint:False,ptlb:HealthyTex,ptin:_HealthyTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:093fcfe4887a666489611af9445f7e2a,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32812,y:32818,cmnt:RGB,varname:node_1086,prsc:2|A-3299-OUT,B-5376-RGB,C-5983-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32267,y:33024,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32267,y:33147,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1749,x:33004,y:32835,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32823,y:33009,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5983-A,C-5376-A;n:type:ShaderForge.SFN_Tex2d,id:2867,x:32267,y:32449,ptovrint:False,ptlb:DecayedTex,ptin:_DecayedTex,varname:node_2867,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3f72844c2ba33eb40bd2f3bed27e3b38,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4549,x:32267,y:32643,ptovrint:False,ptlb:ZombieTex,ptin:_ZombieTex,varname:node_4549,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:79c21182a5defe74e8abc657c6fef64d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ChannelBlend,id:3299,x:32823,y:32528,varname:node_3299,prsc:2,chbt:0|M-697-OUT,R-4549-RGB,G-2867-RGB,B-4805-RGB;n:type:ShaderForge.SFN_Slider,id:2460,x:31428,y:32429,ptovrint:False,ptlb:HealthBar,ptin:_HealthBar,varname:node_2460,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Append,id:3560,x:32141,y:32861,varname:node_3560,prsc:2|A-3960-OUT,B-6297-OUT;n:type:ShaderForge.SFN_OneMinus,id:5292,x:31754,y:32590,varname:node_5292,prsc:2|IN-769-OUT;n:type:ShaderForge.SFN_Clamp01,id:3960,x:31754,y:32721,varname:node_3960,prsc:2|IN-5292-OUT;n:type:ShaderForge.SFN_Multiply,id:769,x:31754,y:32448,varname:node_769,prsc:2|A-2460-OUT,B-5280-OUT;n:type:ShaderForge.SFN_Vector1,id:5280,x:31585,y:32505,varname:node_5280,prsc:2,v1:2;n:type:ShaderForge.SFN_Subtract,id:8707,x:31973,y:32412,varname:node_8707,prsc:2|A-769-OUT,B-3331-OUT;n:type:ShaderForge.SFN_Vector1,id:3331,x:31754,y:32388,varname:node_3331,prsc:2,v1:1;n:type:ShaderForge.SFN_Clamp01,id:4023,x:31973,y:32553,varname:node_4023,prsc:2|IN-8707-OUT;n:type:ShaderForge.SFN_Append,id:697,x:32312,y:32861,varname:node_697,prsc:2|A-3560-OUT,B-4023-OUT;n:type:ShaderForge.SFN_Subtract,id:9552,x:31929,y:32900,varname:node_9552,prsc:2|A-3331-OUT,B-3960-OUT;n:type:ShaderForge.SFN_Subtract,id:6297,x:31973,y:32696,varname:node_6297,prsc:2|A-9552-OUT,B-4023-OUT;proporder:4805-5983-2867-4549-2460;pass:END;sub:END;*/

Shader "Shader Forge/ToothHealth" {
    Properties {
        _HealthyTex ("HealthyTex", 2D) = "black" {}
        _Color ("Color", Color) = (1,1,1,1)
        _DecayedTex ("DecayedTex", 2D) = "white" {}
        _ZombieTex ("ZombieTex", 2D) = "white" {}
        _HealthBar ("HealthBar", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Stencil ("Stencil ID", Float) = 0
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilOpFail ("Stencil Fail Operation", Float) = 0
        _StencilOpZFail ("Stencil Z-Fail Operation", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            Stencil {
                Ref [_Stencil]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilOp]
                Fail [_StencilOpFail]
                ZFail [_StencilOpZFail]
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _HealthyTex; uniform float4 _HealthyTex_ST;
            uniform float4 _Color;
            uniform sampler2D _DecayedTex; uniform float4 _DecayedTex_ST;
            uniform sampler2D _ZombieTex; uniform float4 _ZombieTex_ST;
            uniform float _HealthBar;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float node_769 = (_HealthBar*2.0);
                float node_3960 = saturate((1.0 - node_769));
                float node_3331 = 1.0;
                float node_4023 = saturate((node_769-node_3331));
                float3 node_697 = float3(float2(node_3960,((node_3331-node_3960)-node_4023)),node_4023);
                float4 _ZombieTex_var = tex2D(_ZombieTex,TRANSFORM_TEX(i.uv0, _ZombieTex));
                float4 _DecayedTex_var = tex2D(_DecayedTex,TRANSFORM_TEX(i.uv0, _DecayedTex));
                float4 _HealthyTex_var = tex2D(_HealthyTex,TRANSFORM_TEX(i.uv0, _HealthyTex));
                float node_603 = (_HealthyTex_var.a*_Color.a*i.vertexColor.a); // A
                float3 emissive = (((node_697.r*_ZombieTex_var.rgb + node_697.g*_DecayedTex_var.rgb + node_697.b*_HealthyTex_var.rgb)*i.vertexColor.rgb*_Color.rgb)*node_603);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_603);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
