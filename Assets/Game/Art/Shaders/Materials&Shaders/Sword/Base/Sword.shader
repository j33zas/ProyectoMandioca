// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Character/Sword/Base"
{
	Properties
	{
		[HDR]_Color3("Color 3", Color) = (1,0,0.0379858,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Always
			Pass Keep
			Fail Keep
			ZFail Keep
		}
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _Color3;
		uniform float _Intensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color9 = IsGammaSpace() ? float4(0.3869259,0.3953965,0.4433962,0) : float4(0.123889,0.1296648,0.1653383,0);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode1 = tex2D( _TextureSample0, uv_TextureSample0 );
			float Red3 = tex2DNode1.r;
			float Green4 = tex2DNode1.g;
			float4 color8 = IsGammaSpace() ? float4(0.5660378,0.5660378,0.5660378,0) : float4(0.280335,0.280335,0.280335,0);
			float Blue5 = tex2DNode1.b;
			float4 color11 = IsGammaSpace() ? float4(0.4056604,0.1709471,0,0) : float4(0.13687,0.02476341,0,0);
			o.Albedo = ( ( color9 * Red3 ) + ( Green4 * color8 ) + ( Blue5 * color11 ) ).rgb;
			float4 Emission15 = ( Red3 * _Color3 * _Intensity );
			o.Emission = Emission15.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;359;724;330;3592.694;-1055.739;1.792195;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-4340.782,236.683;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;None;187b4a14ffca0f54899a85b99ffdc089;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;3;-3538.471,-18.20835;Inherit;False;Red;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;2;-3398.87,1132.862;Inherit;False;1300.292;532.4644;Emission;5;15;13;7;6;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;6;-3305.417,1348.864;Inherit;False;Property;_Color3;Color 3;0;1;[HDR];Create;True;0;0;False;0;1,0,0.0379858,0;0,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;7;-3348.87,1182.862;Inherit;False;3;Red;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-3141.06,1525.294;Inherit;False;Property;_Intensity;Intensity;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-3438.402,424.2081;Inherit;False;Blue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-3443.457,294.2747;Inherit;False;Green;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;-2690.289,786.2104;Inherit;False;Constant;_Color6;Color 6;3;0;Create;True;0;0;False;0;0.4056604,0.1709471,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;10;-2727.333,640.3299;Inherit;False;5;Blue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;-2760.717,92.19684;Inherit;False;3;Red;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-2663.832,459.4991;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.5660378,0.5660378,0.5660378,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;12;-2669.481,320.1277;Inherit;False;4;Green;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2893.157,1227.768;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;9;-2776.405,-200.8703;Inherit;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.3869259,0.3953965,0.4433962,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2362.593,405.5937;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-2403.634,684.4476;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2417.989,-10.6471;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-2341.58,1198.186;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1757.385,-121.8261;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1541.069,-15.80196;Inherit;False;15;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-892.3849,-206.5908;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Character/Sword/Base;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;True;1;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.27;1,0,0,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;1
WireConnection;5;0;1;3
WireConnection;4;0;1;2
WireConnection;13;0;7;0
WireConnection;13;1;6;0
WireConnection;13;2;26;0
WireConnection;18;0;12;0
WireConnection;18;1;8;0
WireConnection;17;0;10;0
WireConnection;17;1;11;0
WireConnection;16;0;9;0
WireConnection;16;1;14;0
WireConnection;15;0;13;0
WireConnection;19;0;16;0
WireConnection;19;1;18;0
WireConnection;19;2;17;0
WireConnection;0;0;19;0
WireConnection;0;2;20;0
ASEEND*/
//CHKSM=A157B554D094A16A80D6E46CADC0A0432EB89135