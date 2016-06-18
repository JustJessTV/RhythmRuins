Shader "Unlit/MusicPulse"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "black" {}
		_Grad ("Grad", 2D) = "black" {}
		_Beat ("Beat", Range(0,1)) = 1
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
			};
			
			sampler2D _MainTex;
			sampler2D _Mask;
			sampler2D _Grad;
			float _Beat;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if(col.a<0.8) discard;
				fixed4 mask = tex2D(_Mask,i.uv);
				fixed4 grad = tex2D(_Grad,float2(i.uv.y*10+_Time.x*10,0));
				grad*=sin(_Time.x*50+i.uv.y*50+sin(i.uv.x*100+_Time.x*10))*0.5+0.5;
				return col+mask*pow(grad,_Beat);
			}
			ENDCG
		}
	}
}
