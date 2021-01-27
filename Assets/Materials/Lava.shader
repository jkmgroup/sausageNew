
Shader "Custom/Lava"{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FlowMap("Flow Map", 2D) = "grey" {}
		_Speed("Speed", Range(-1, 1)) = 0.2
		_Amplitude("Amplitude", Range(0,4)) = 1.0
		_Movement("Movement", Range(-100,100)) = 0
	}

	SubShader{
		Pass {
			Tags { "RenderType" = "Opaque" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _FlowMap;
			fixed _Speed;
			float _Amplitude;
			float _Movement;

			fixed4 _MainTex_ST;

			v2f vert(appdata_base IN) {
				v2f o;
				o.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);

				float4x4 Matrice = unity_ObjectToWorld;

				float4 posWorld = mul(Matrice, IN.vertex);

				float displacement = (cos(posWorld.y) + cos(posWorld.x + posWorld.z + _Movement * _Time));
				posWorld.y = posWorld.y + _Amplitude * displacement;

				o.pos = mul(UNITY_MATRIX_VP, posWorld);
				return o;
			}

			fixed4 frag(v2f v) : COLOR {
				fixed4 c;
				half3 flowVal = (tex2D(_FlowMap, v.uv) * 2 - 1) * _Speed;

				float dif1 = frac(_Time.y * 0.25 + 0.5);
				float dif2 = frac(_Time.y * 0.25);

				half lerpVal = abs((0.5 - dif1) / 0.5);

				half4 col1 = tex2D(_MainTex, v.uv - flowVal.xy * dif1);
				half4 col2 = tex2D(_MainTex, v.uv - flowVal.xy * dif2);

				c = lerp(col1, col2, lerpVal);
				return c;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}

