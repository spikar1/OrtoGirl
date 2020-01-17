Shader "Custom/HeightShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ColorMin ("Tint Color At Min", Color) = (0,0,0,1)
		_ColorMax ("Tint Color At Max", Color) = (1,1,1,1)
		_HeightMultiplier ("HeightMultiplier", Range(0.03,1)) = 0.6
		_ClampMin ("Clamp Min", Range(0.03,1)) = 0.3
		_ClampMax ("Clamp Max", Range(0.03,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		half _HeightMultiplier;
		half _ClampMin;
		half _ClampMax;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			
			float h = IN.worldPos.y;
			h = h * _HeightMultiplier;
			h = clamp(h, _ClampMin, _ClampMax);

			/*_Color.x *= (h + .3f);
			_Color.y *= (h + .4f);

			_Color.x = clamp(_Color.x, 0, 1);
			_Color.y = clamp(_Color.y, 0, 1);
			_Color.z = clamp(_Color.z, 0, 1);*/
			

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * h;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
