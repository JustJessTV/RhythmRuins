Shader "Shaded" {
	Properties {
		
		_Size ("Size",Float) = 1
		_Thickness ("Thickness",Range(0,1))=0.5
	}
 
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma target 3.0
			float _Size;
			float _Thickness;
			struct appdata {
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
			};
 
			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD2;
			};
 
 
 
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(_Object2World,v.vertex);
   
				return o;
			}
			half4 frag (v2f i) : COLOR
			{

				if(((i.worldPos.x+i.worldPos.y)+100)%_Size>_Size*_Thickness){
					discard;
				}
				return half4(.8,.4,.2,1);
   
			}
 
			ENDCG
		}
 
	}
	Fallback Off
}