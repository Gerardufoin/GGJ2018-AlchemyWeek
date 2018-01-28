// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GGJ/TimeBar"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_Map("Map", 2D) = "white" {}
		_Fill("Fill", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			
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
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _Map;
			uniform float4 _Map_ST;
			uniform float _Fill;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_Map = IN.texcoord.xy * _Map_ST.xy + _Map_ST.zw;
				float4 temp_cast_0 = (0.0).xxxx;
				float4 temp_cast_1 = (1.0).xxxx;
				float4 temp_cast_2 = (0.8).xxxx;
				float4 temp_cast_3 = (1.0).xxxx;
				float2 uv2 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 appendResult9 = (float4((( (temp_cast_2 + (tex2D( _Map, uv_Map ) - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0)) * _Color )).rgb , ( 1.0 - ceil( ( uv2.y - _Fill ) ) )));
				
				fixed4 c = appendResult9;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14301
286;85;1627;892;1851.224;173.3201;1.466581;True;False
Node;AmplifyShaderEditor.RangedFloatNode;16;-1215.83,-233.1846;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1099.9,460.9999;Float;False;Property;_Fill;Fill;1;0;Create;True;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1363.143,-428.1078;Float;True;Property;_Map;Map;0;0;Create;True;None;0ee5687219492bf49b040089ddca8f1a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1215.83,-155.1847;Float;False;Constant;_Float1;Float 1;1;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1221.33,-74.88467;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1224.229,-2.384614;Float;False;Constant;_Float3;Float 3;1;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1061.9,338;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;-804.9001,393;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;20;-954.5283,-38.18457;Float;False;_Color;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;10;-892.7419,-292.4075;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0.3,0,0,0;False;4;COLOR;0.7,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-676.3291,-69.38456;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CeilOpNode;7;-623.9001,392;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-443.9001,393;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;15;-463.5,-75;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-206.5,33;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMasterNode;1;-52,33;Float;False;True;2;Float;ASEMaterialInspector;0;4;GGJ/TimeBar;0f8ba0101102bb14ebf021ddadce9b49;Sprites Default;3;One;OneMinusSrcAlpha;0;One;Zero;Off;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;6;0;2;2
WireConnection;6;1;5;0
WireConnection;10;0;4;0
WireConnection;10;1;16;0
WireConnection;10;2;17;0
WireConnection;10;3;18;0
WireConnection;10;4;19;0
WireConnection;21;0;10;0
WireConnection;21;1;20;0
WireConnection;7;0;6;0
WireConnection;8;0;7;0
WireConnection;15;0;21;0
WireConnection;9;0;15;0
WireConnection;9;3;8;0
WireConnection;1;0;9;0
ASEEND*/
//CHKSM=B52078ECF2A8BBC0A3EE715746AB07679D761E02