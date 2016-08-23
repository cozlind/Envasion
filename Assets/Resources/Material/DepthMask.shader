Shader "Sprite/Depth Mask" {
	Properties
	{
		[PerRendererData]_MainTex("Base (RGB)", 2D) = "white" {}
	_Color("Main Color", COLOR) = (1,1,1,1)
	_Mask("Culling Mask", 2D) = "white" {}
	_Cutoff("Alpha cutoff", Range(0,1)) = 0.1
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" }
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		AlphaTest GEqual[_Cutoff]
		Pass
	{
	SetTexture[_MainTex]
	{
		ConstantColor[_Color]
		combine texture*constant,texture*constant
	}
		SetTexture[_Mask]{ combine previous - texture,previous-texture }
	}

	}
}
