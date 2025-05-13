Shader "TextMeshPro/BGShader"
{
	Properties
	{
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		
		_CullMode ("Cull Mode", Float) = 0
		_ColorMask ("Color Mask", Float) = 15
		_ClipRect ("Clip Rect", vector) = (-32767, -32767, 32767, 32767)

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull [_CullMode]
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
            Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
			};
			
            sampler2D _MainTex;
			sampler2D _NoiseTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
			{
				v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				
                OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				
				float step = 0.1f;

				half4 color1 = (tex2D(_MainTex, IN.texcoord - float2(step, 0.0f)) + _TextureSampleAdd) * IN.color;
				half4 color2 = (tex2D(_MainTex, IN.texcoord - float2(0.0, step)) + _TextureSampleAdd) * IN.color;

				float2 d1Color = (color - color2);
				float2 d2Color = (color - color1);

				float2 dColor = max(d1Color, d2Color);

				float lightAreaNoise = (tex2D(_NoiseTex, IN.texcoord / 4.0f + dColor + _Time.xy / 8.0f) + _TextureSampleAdd).r;
				float blackAreaNoise = (tex2D(_NoiseTex, IN.texcoord / 4.0f - lightAreaNoise / 32.0f + _Time.yy / 32.0f) + _TextureSampleAdd).r;
				
				float avrNoise = (blackAreaNoise + lightAreaNoise) / 2.0f;

				////
				float lightAreaIntensity = 0.5f;
				float blackAreaIntensity = 0.1f;
				float borderAreaIntensity = 16.0f;

				float avrColor = (color.r + color.g + color.b) / 3.0f;
				float maskUpperBound = 0.2 * sin(_Time.y / 2.0f) + 0.5; 
				
				float actualLightAreaMask = smoothstep(0.0f, 0.9f, avrColor);
				float actualBlackAreaMask = 1.0f - actualLightAreaMask;

				float lightAreaMask = smoothstep(0.0f, maskUpperBound, avrColor);
				float blackAreaMask = 1.0f - lightAreaMask;

				float startAreaMask = min(color.r, min(color.g, color.b)) * (0.5 * sin(_Time.y) + 0.825) * 2.0f;
				
				/////////////
				lightAreaNoise = smoothstep(0.1, 0.5f, lightAreaNoise);

				float maxColor = max(color.r, max(color.g, color.b));

				lightAreaNoise *= lightAreaMask;
				lightAreaNoise *= lightAreaIntensity;
				lightAreaNoise = saturate(lightAreaNoise);
				///////////////

				/////////////////
				blackAreaNoise = smoothstep(0.1, 0.3, blackAreaNoise);
				blackAreaNoise *= blackAreaMask;
				blackAreaNoise *= blackAreaIntensity;
				blackAreaNoise = saturate(blackAreaNoise);

				float borderBlackAreaLightArea = 1.0f - smoothstep(actualLightAreaMask, 0.3f, actualBlackAreaMask);
				float borderNoise = blackAreaNoise * borderBlackAreaLightArea * borderAreaIntensity;
				borderNoise = smoothstep(0.5f, 0.9f , borderNoise);
				borderNoise = saturate(borderNoise);

				///////////////////
                #ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
					clip (color.a - 0.001);
				#endif

				float4 lightAreaColor = color * (1.0f - lightAreaNoise);
				float4 blackAreaColor = color * (1.0f - blackAreaNoise);
				
				return (blackAreaColor + lightAreaColor) / 2.0f;
			}
		ENDCG
		}
	}
}
