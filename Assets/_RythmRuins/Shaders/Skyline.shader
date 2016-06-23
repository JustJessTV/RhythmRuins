Shader "Unlit/Skyline"
{
	Properties
	{
		_Clouds ("Clouds", 2D) = "white" {}
		_MainTex ("Texture", 2D) = "white" {}
		_Gradient ("Gradient", 2D) = "white" {}
		_GradientDepth ("GradientDepth", 2D) = "white" {}
		_Umph ("Umph",Range(-1,1))=0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};
			
			sampler2D _MainTex;
			sampler2D _Clouds;
			sampler2D _Gradient;
			sampler2D _GradientDepth;
			sampler2D _SPECTRUM;
			float4 _MainTex_ST;
			float _Umph;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(_Object2World,v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float zPoint = saturate((i.worldPos.z-1)*0.05);
				if(col.a<0.5) discard;
				//return fixed4(1,1,1,1)*(pow(tex2D(_SPECTRUM,i.uv).r,0.5));
				float3 cloud = float3(
				tex2D(_Clouds, float2(_Time.x+i.uv.x,_Time.x+i.uv.y)).x,
				tex2D(_Clouds, float2(-_Time.x+i.uv.x,_Time.x+i.uv.y)).x,
				tex2D(_Clouds, float2(i.uv.x,-_Time.x+i.uv.y)).x);
				float sample = 0.002;
				float sampleD = sample*0.7;
				float blendAmount = 
					tex2D(_MainTex, float2(i.uv.x+sample,i.uv.y+0.00)).g+
					tex2D(_MainTex, float2(i.uv.x-sample,i.uv.y+0.00)).g+
					tex2D(_MainTex, float2(i.uv.x+0.00,i.uv.y+sample)).g+
					tex2D(_MainTex, float2(i.uv.x+0.00,i.uv.y-sample)).g+
					tex2D(_MainTex, float2(i.uv.x+sampleD,i.uv.y+sampleD)).g+
					tex2D(_MainTex, float2(i.uv.x-sampleD,i.uv.y+sampleD)).g+
					tex2D(_MainTex, float2(i.uv.x+sampleD,i.uv.y-sampleD)).g+
					tex2D(_MainTex, float2(i.uv.x-sampleD,i.uv.y-sampleD)).g;
				blendAmount*=0.125f;
				cloud = 1-(abs(cloud*2-1));
				cloud = pow(cloud,3);
				float finalCloud = (cloud.x+cloud.y+cloud.z)/3;
				float3 lightColor = tex2D(_Gradient,saturate(finalCloud+_Umph));
				return (1-blendAmount)*tex2D(_GradientDepth,zPoint)+lightColor.xyzz*blendAmount*1.5;
			}
			ENDCG
		}
	}
}
