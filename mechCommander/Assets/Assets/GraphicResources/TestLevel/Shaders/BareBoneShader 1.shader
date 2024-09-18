Shader "ShaderTasks/BareBone" {
    Properties {
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags 
        { 
            "Queue" = "Geometry"
            "RenderType" = "Opaque" 
        }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            uniform half4 _Color;
           
            struct appdata {
                float4 pos : POSITION;
            };
           
            struct v2f {
                float4 pos: SV_POSITION;
            };
          
            v2f vert(appdata v) {
                appdata o;
                o.pos = UnityObjectToClipPos(v.pos);
                return o;
            }
           
            half4 frag(appdata i) : COLOR {
				return _Color;
			}
            ENDCG
        }
    }
}