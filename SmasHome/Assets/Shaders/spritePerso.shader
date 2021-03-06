﻿Shader "spritePerso"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		_TeamColor ("Team color", Color) = (0.5, 0.98, 0.5, 1)
		_TeamColorBase ("Team color base", Color) = (1, 1, 0.412, 1)
		_GhostColor ("Ghost color", Color) = (0.5, 0.5, 0.5, 1)
		[Toggle] _IsBackground("Is background", Float) = 0
		[Toggle] _IsGhost("Is ghost", Float) = 0
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

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 _TeamColor;
			fixed4 _TeamColorBase;
			fixed4 _GhostColor;
			float _IsBackground;
			float _IsGhost;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				/* Color from texture */
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				/* Color from team's flag */
				float diff = (abs (c.r - _TeamColorBase.r) + abs (c.g - _TeamColorBase.g) + abs (c.b - _TeamColorBase.b));
				float colorMask = pow(diff, 2);
				colorMask = 1.0 - clamp(colorMask, 0.0, 1.0);
				//float colorMask = clamp((c.r + c.g + c.b) / 3.0, 0.0, 1.0); // Get greyscale value
				//colorMask = pow(colorMask, 10); // Decroissance rapide
				fixed3 colorTeam = colorMask * _TeamColor.rgb + (1.0 - colorMask) * c.rgb;

				/* Color from ghost */
				float greyScale = clamp((c.r + c.g + c.b) / 3.0, 0.0, 1.0);
				fixed3 colorGhost;
				colorGhost.rgb = greyScale;
				colorTeam = (1.0 - _IsGhost) * colorTeam + _IsGhost * (colorMask * _TeamColor.rgb + colorGhost * _GhostColor.rgb); // Color
				c.a = (1.0 - _IsGhost) * c.a + _IsGhost * c.a * 0.5 * (0.75 + _SinTime.w / 4.0); // Alpha

				/* Color from background */
				fixed3 backgroundColor = _IsBackground * 0.7 * colorTeam.rgb + (1.0 - _IsBackground) * colorTeam.rgb;

				//c.rgb = colorMask;
				c.rgb = backgroundColor;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}