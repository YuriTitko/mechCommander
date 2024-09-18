Shader "Custom/bareBoneShader"
{
    Properties
    {
		[KeywordEnum(None, Add, Multiply)] _Overlay("Overlay mode", Float) = 0

	    [Header(Textures)][Space(5)] 
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal map", 2D) = "bump" {}
		
		[Header(Values)][Space(5)] 
		_Int("Integer", int) = 1
		_Float("Float", float) = 1.0
		_Range("Range", Range(0, 1)) = 0.5
		_Vector("Vector4", Vector) = (0, 0, 0, 0)

		[Header(Color)][Space(5)]
		_Color ("Color", Color) = (1,1,1,1)
        
    }

    SubShader
    {
        Tags 
		{ 
			"Queue" = "Geometry"

			// Background
			//  (1000) : used for backgrounds and skyboxes,
			// Geometry
			//  (2000) : the default label used for most solid objects,
			// Transparent
			//  (3000) : used for materials with transparent properties, such glass, fire, particles and water;
			// Overlay
			//  (4000) : used for effects such as lens flares, GUI elements and texts.

			"RenderType" = "Opaque" 
		}
			
		Pass{
		CGPROGRAM

		#pragma vertex vert             
		#pragma fragment frag

		sampler2D _MainTex;
		sampler2D _NormalMap;

		int _Int;
		float _Float;
		float _Range;

		half4 _Color;
		float4 _Vector;

		struct vertInput {
			float4 pos : POSITION;
		};

		struct vertOutput {
			float4 pos : SV_POSITION;
		};

		vertOutput vert(vertInput input) {
			vertOutput o;
			o.pos = UnityObjectToClipPos(input.pos);
			return o;
		}

		half4 frag(vertOutput output) : COLOR {
			return _Color;
		}

		ENDCG
		}
    }
}
