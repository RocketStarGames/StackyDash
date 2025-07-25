﻿Shader "Unlit/Unlit CastShadow"
{
  	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {} 
        _Color ("Main Color", Color) = (1,0.5,0.5,1)

	}
	
	SubShader {
		//Tags {"Queue"="Opaque" }
		LOD 100
		
		Pass {
			Lighting Off
			SetTexture [_MainTex] { combine texture } 
                           SetTexture [_MainTex] { 
                // Sets our color as the 'constant' variable
                constantColor [_Color]
                
                // Multiplies color (in constant) with texture
                combine constant * texture
            }
		}
		
		// Pass to render object as a shadow caster
		Pass 
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			Fog {Mode Off}
			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"


	
			struct v2f { 
				V2F_SHADOW_CASTER;
			};
	
			v2f vert( appdata_base v )
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
	
			float4 frag( v2f i ) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG

		}

	}
}
