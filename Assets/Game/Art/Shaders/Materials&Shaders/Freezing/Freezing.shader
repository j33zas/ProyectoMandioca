// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/Freezing"
{
	Properties
	{
		_StepRocks("StepRocks", Float) = 0
		_StepValueStyle("StepValueStyle", Float) = 0
		_ScaleRock("ScaleRock", Float) = 0
		_MaskFreezing("MaskFreezing", Float) = 0
		_MaskMoveStyle("MaskMoveStyle", Float) = 0
		_GradiantMaskFreezing("GradiantMaskFreezing", Float) = 0
		_MinClampStyle("MinClampStyle", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _StepValueStyle;
		uniform float _MaskMoveStyle;
		uniform float _MinClampStyle;
		uniform float _ScaleRock;
		uniform float _StepRocks;
		uniform float _MaskFreezing;
		uniform float _GradiantMaskFreezing;


		float2 voronoihash10( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi10( float2 v, float time, inout float2 id )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash10( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2 - F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_cast_0 = (_MaskMoveStyle).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float dotResult59 = dot( ( temp_cast_0 - ase_worldlightDir ) , cross( ddx( ase_vertex3Pos ) , ddy( ase_vertex3Pos ) ) );
			float clampResult51 = clamp( step( _StepValueStyle , dotResult59 ) , _MinClampStyle , 1.0 );
			float time10 = 0.0;
			float2 coords10 = i.uv_texcoord * _ScaleRock;
			float2 id10 = 0;
			float voroi10 = voronoi10( coords10, time10,id10 );
			float temp_output_17_0 = step( voroi10 , _StepRocks );
			float4 color4 = IsGammaSpace() ? float4(0,0.3662698,1,0) : float4(0,0.1104432,1,0);
			float4 color23 = IsGammaSpace() ? float4(0,0.1390704,0.2627451,0) : float4(0,0.01719082,0.05612849,0);
			float4 color27 = IsGammaSpace() ? float4(0.4415584,1,0,0) : float4(0.1638788,1,0,0);
			float2 uv_TexCoord75 = i.uv_texcoord + float2( -0.5,-0.5 );
			float temp_output_42_0 = ( distance( ase_vertex3Pos , float3( uv_TexCoord75 ,  0.0 ) ) / _MaskFreezing );
			float4 lerpResult25 = lerp( ( ( ( 1.0 - temp_output_17_0 ) * color4 ) + ( temp_output_17_0 * color23 ) ) , color27 , saturate( pow( temp_output_42_0 , _GradiantMaskFreezing ) ));
			o.Albedo = ( clampResult51 * lerpResult25 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;394;1086;295;-763.2125;488.0909;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;29;-2099.815,4.874763;Inherit;False;Property;_ScaleRock;ScaleRock;3;0;Create;True;0;0;False;0;0;18.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-2191.122,-138.2745;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-1678.453,151.1678;Inherit;False;Property;_StepRocks;StepRocks;1;0;Create;True;0;0;False;0;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;10;-1807.377,-107.8826;Inherit;True;0;0;1;2;1;False;1;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;7.28;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.PosVertexDataNode;63;-2155.561,-550.5519;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-1289.733,695.6915;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;77;-1245.488,528.2191;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-828.7288,695.0704;Inherit;False;Property;_MaskFreezing;MaskFreezing;4;0;Create;True;0;0;False;0;0;2.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DdyOpNode;54;-1579.675,-389.5076;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;17;-1456.171,-148.7938;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.64;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;52;-1947.829,-720.2429;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;40;-835.7078,587.0331;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DdxOpNode;55;-1619.498,-518.0294;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1951.508,-830.4444;Inherit;False;Property;_MaskMoveStyle;MaskMoveStyle;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;-528.3108,497.4312;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-598.5796,719.4913;Inherit;False;Property;_GradiantMaskFreezing;GradiantMaskFreezing;6;0;Create;True;0;0;False;0;0;25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;21;-1099.126,264.6395;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;56;-1419.392,-500.6564;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;18;-1101.762,-20.28967;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-1362.958,305.0974;Inherit;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;0,0.1390704,0.2627451,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-1110.689,69.38769;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0,0.3662698,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;57;-1458.36,-851.2432;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;46;-224.8948,680.8203;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-802.214,278.0928;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;59;-1176.148,-634.5477;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1220.01,-802.9001;Inherit;False;Property;_StepValueStyle;StepValueStyle;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-815.8073,85.09013;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;27;-555.4778,194.7357;Inherit;False;Constant;_Color2;Color 2;2;0;Create;True;0;0;False;0;0.4415584,1,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;60;-898.1619,-694.5587;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-673.7749,-518.9506;Inherit;False;Property;_MinClampStyle;MinClampStyle;7;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-601.828,88.59798;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;49;-259.9628,313.995;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;-51.77728,106.5122;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;51;-354.5983,-601.2658;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;687.5966,-412.7196;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;62;-1854.377,-394.174;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DitheringNode;92;1205.654,-318.716;Inherit;False;1;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;105;753.7827,-89.102;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;61;-2101.863,-358.5761;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1600.304,-578.7932;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Effects/Freezing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;-0.02;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;7;0
WireConnection;10;2;29;0
WireConnection;54;0;63;0
WireConnection;17;0;10;0
WireConnection;17;1;3;0
WireConnection;40;0;77;0
WireConnection;40;1;75;0
WireConnection;55;0;63;0
WireConnection;42;0;40;0
WireConnection;42;1;43;0
WireConnection;21;0;17;0
WireConnection;56;0;55;0
WireConnection;56;1;54;0
WireConnection;18;0;17;0
WireConnection;57;0;53;0
WireConnection;57;1;52;0
WireConnection;46;0;42;0
WireConnection;46;1;45;0
WireConnection;20;0;21;0
WireConnection;20;1;23;0
WireConnection;59;0;57;0
WireConnection;59;1;56;0
WireConnection;19;0;18;0
WireConnection;19;1;4;0
WireConnection;60;0;58;0
WireConnection;60;1;59;0
WireConnection;24;0;19;0
WireConnection;24;1;20;0
WireConnection;49;0;46;0
WireConnection;25;0;24;0
WireConnection;25;1;27;0
WireConnection;25;2;49;0
WireConnection;51;0;60;0
WireConnection;51;1;50;0
WireConnection;69;0;51;0
WireConnection;69;1;25;0
WireConnection;62;0;61;0
WireConnection;92;0;105;0
WireConnection;105;0;42;0
WireConnection;0;0;69;0
ASEEND*/
//CHKSM=DC7BE2F4C859BD26A739E6F2A8057D3C05E47048