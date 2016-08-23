// FogOfWar shader
// Copyright (C) 2013 Sergey Taraban <http://staraban.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

Shader "Sprite/MaskShader" {
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_FogRadius("FogRadius", Float) = 1.0
		_FogMaxRadius("FogMaxRadius", Float) = 0.5
		Planet1Pos("Planet1Pos", Vector) = (0,0,0,1)
		Planet2Pos("Planet2Pos", Vector) = (0,0,0,1)
		Planet3Pos("Planet3Pos", Vector) = (0,0,0,1)
			Planet4Pos("Planet4Pos", Vector) = (0,0,0,1)
			Planet5Pos("Planet5Pos", Vector) = (0,0,0,1)
			Planet6Pos("Planet6Pos", Vector) = (0,0,0,1)
			Planet7Pos("Planet7Pos", Vector) = (0,0,0,1)
			Planet8Pos("Planet8Pos", Vector) = (0,0,0,1)
			Planet9Pos("Planet9Pos", Vector) = (0,0,0,1)
			Planet10Pos("Planet10Pos", Vector) = (0,0,0,1)
			Planet11Pos("Planet11Pos", Vector) = (0,0,0,1)
			Planet12Pos("Planet12Pos", Vector) = (0,0,0,1)
			Planet13Pos("Planet13Pos", Vector) = (0,0,0,1)
			[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
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
#pragma shader_feature ETC1_EXTERNAL_ALPHA
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
			fixed4 color : COLOR;
			float2 texcoord  : TEXCOORD0;
			float2 location:TEXCOORD1;
		};

		fixed4 _Color;

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
			float4 posWorld = mul(_Object2World, IN.vertex);
			OUT.location.xy = posWorld.xy;
			return OUT;
		}


		sampler2D _MainTex;
		float     _FogRadius;
		float     _FogMaxRadius;
		float4     Planet1Pos;
		float4     Planet2Pos;
		float4     Planet3Pos;
		float4     Planet4Pos;
		float4     Planet5Pos;
		float4     Planet6Pos;
		float4     Planet7Pos;
		float4     Planet8Pos;
		float4     Planet9Pos;
		float4     Planet10Pos;
		float4     Planet11Pos;
		float4     Planet12Pos;
		float4     Planet13Pos;

		sampler2D _AlphaTex;

		fixed4 SampleSpriteTexture(float2 uv)
		{
			fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
			// get the color from an external texture (usecase: Alpha support for ETC1 on android)
			color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

			return color;
		}

		float powerForPos(float4 pos, float2 nearVertex) {
			float atten = clamp(_FogRadius - length(pos.xy - nearVertex.xy), 0.0, _FogRadius);

			return (1.0 / _FogMaxRadius)*atten / _FogRadius;
		}
		fixed4 frag(v2f IN) : SV_Target
		{
			fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

		    float alpha = 1.0 - powerForPos(Planet1Pos, IN.location) 
				- powerForPos(Planet2Pos, IN.location)
				- powerForPos(Planet3Pos, IN.location)
				- powerForPos(Planet4Pos, IN.location)
				- powerForPos(Planet5Pos, IN.location)
				- powerForPos(Planet6Pos, IN.location)
				- powerForPos(Planet7Pos, IN.location)
				- powerForPos(Planet8Pos, IN.location)
				- powerForPos(Planet9Pos, IN.location)
				- powerForPos(Planet10Pos, IN.location)
				- powerForPos(Planet11Pos, IN.location)
				- powerForPos(Planet12Pos, IN.location)
				- powerForPos(Planet13Pos, IN.location);

			c.a *= alpha;
		return c;
		}
			ENDCG
		}
		}
}