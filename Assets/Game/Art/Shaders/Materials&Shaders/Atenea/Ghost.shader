// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atenea/Ghost"
{
	Properties
	{
		_Bias("Bias", Float) = 0
		_Scale("Scale", Float) = 0
		_MaskBot("MaskBot", Range( -20 , 70)) = 0
		_MaskTop("MaskTop", Range( -40 , -1)) = 0
		_OpacityIntensity("OpacityIntensity", Range( 0 , 1)) = 0
		_Freq("Freq", Float) = 0
		_SinColor("SinColor", Color) = (1,0,0,0)
		_Timer("Timer", Float) = 0
		_Amplitude("Amplitude", Float) = 0
		_FresnelColor("FresnelColor", Color) = (1,0,0.03761339,0)
		_MainColor("MainColor", Color) = (1,0,0.03761339,0)
		_EmissionIntensity("EmissionIntensity", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _MainColor;
		uniform float4 _FresnelColor;
		uniform float _Bias;
		uniform float _Scale;
		uniform float4 _SinColor;
		uniform float _Freq;
		uniform float _Timer;
		uniform float _Amplitude;
		uniform float _EmissionIntensity;
		uniform float _MaskBot;
		uniform float _MaskTop;
		uniform float _OpacityIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV53 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode53 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV53, 5.0 ) );
			float4 lerpResult64 = lerp( _MainColor , _FresnelColor , saturate( fresnelNode53 ));
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime98 = _Time.y * _Timer;
			float temp_output_104_0 = ( saturate( sin( ( ( ase_vertex3Pos.y * _Freq ) + mulTime98 ) ) ) * _Amplitude );
			float4 lerpResult100 = lerp( lerpResult64 , _SinColor , temp_output_104_0);
			o.Emission = ( lerpResult100 * _EmissionIntensity ).rgb;
			o.Alpha = ( saturate( ( ( ase_vertex3Pos.x * _MaskBot * fresnelNode53 ) + ( 1.0 - ( fresnelNode53 * ase_vertex3Pos.x * _MaskTop ) ) ) ) * _OpacityIntensity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;408;975;281;-1128.347;-173.1484;1.044464;True;False
Node;AmplifyShaderEditor.RangedFloatNode;96;1516.081,-113.9053;Inherit;False;Property;_Freq;Freq;5;0;Create;True;0;0;False;0;0;51.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;94;1465.699,-265.2757;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;103;1623.561,-23.9911;Inherit;False;Property;_Timer;Timer;7;0;Create;True;0;0;False;0;0;1.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;57;785.7543,53.07388;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;56;784.6158,-85.86459;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;61;810.9834,317.962;Inherit;False;Property;_Scale;Scale;1;0;Create;True;0;0;False;0;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;805.9834,240.962;Inherit;False;Property;_Bias;Bias;0;0;Create;True;0;0;False;0;0;0.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;53;1158.407,185.2784;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.56;False;2;FLOAT;1.56;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;1846.286,-180.3746;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;98;1839.211,-50.3598;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;1442.012,610.123;Inherit;False;Property;_MaskTop;MaskTop;3;0;Create;True;0;0;False;0;0;-40;-40;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;86;1365.054,498.7835;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;2083.211,-168.3598;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;77;1467.422,369.6598;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;1833.076,522.7745;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;1537.015,256.4696;Inherit;False;Property;_MaskBot;MaskBot;2;0;Create;True;0;0;False;0;0;7;-20;70;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;87;1670.579,190.2243;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;93;2292.388,-154.2162;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;1303.205,-408.3442;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;65;1818.614,-732.8769;Inherit;False;Property;_MainColor;MainColor;10;0;Create;True;0;0;False;0;1,0,0.03761339,0;0.07583422,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;1873.637,242.7341;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;1810.679,-562.5942;Inherit;False;Property;_FresnelColor;FresnelColor;9;0;Create;True;0;0;False;0;1,0,0.03761339,0;0.9642474,1,0.6745283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;105;2478.951,-71.95383;Inherit;False;Property;_Amplitude;Amplitude;8;0;Create;True;0;0;False;0;0;0.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;99;2495.016,-174.1086;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;85;2118.722,500.3195;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;2242.343,-466.2341;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;2678.017,-175.9166;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;2268.726,271.7925;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;101;2258.64,-324.1467;Inherit;False;Property;_SinColor;SinColor;6;0;Create;True;0;0;False;0;1,0,0,0;1,0.9833598,0.466981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;59;3526.196,-140.9396;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;11;0;Create;True;0;0;False;0;0;1.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;63;2521.338,455.3571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;100;3260.582,-256.668;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;2408.888,601.2086;Inherit;False;Property;_OpacityIntensity;OpacityIntensity;4;0;Create;True;0;0;False;0;0;0.497;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;22;-1191.845,-670.2067;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;27;-1054.159,-477.8286;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;17;-825.2875,-637.2677;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;2978.064,499.9164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;3914.574,-215.1893;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;111;3365.752,-95.66856;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;110;2973.072,-106.9071;Inherit;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;107;2944.531,74.68425;Inherit;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;-1;None;d8ee8e3f104a5594b801ddb232bfc730;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;112;3258.385,176.9453;Inherit;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;False;0;0.71;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4177.814,-265.9229;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Atenea/Ghost;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;32;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;53;0;56;0
WireConnection;53;4;57;0
WireConnection;53;1;60;0
WireConnection;53;2;61;0
WireConnection;95;0;94;2
WireConnection;95;1;96;0
WireConnection;98;0;103;0
WireConnection;86;0;53;0
WireConnection;97;0;95;0
WireConnection;97;1;98;0
WireConnection;83;0;86;0
WireConnection;83;1;77;1
WireConnection;83;2;84;0
WireConnection;87;0;53;0
WireConnection;93;0;97;0
WireConnection;67;0;53;0
WireConnection;78;0;77;1
WireConnection;78;1;75;0
WireConnection;78;2;87;0
WireConnection;99;0;93;0
WireConnection;85;0;83;0
WireConnection;64;0;65;0
WireConnection;64;1;54;0
WireConnection;64;2;67;0
WireConnection;104;0;99;0
WireConnection;104;1;105;0
WireConnection;82;0;78;0
WireConnection;82;1;85;0
WireConnection;63;0;82;0
WireConnection;100;0;64;0
WireConnection;100;1;101;0
WireConnection;100;2;104;0
WireConnection;17;0;22;0
WireConnection;17;1;27;0
WireConnection;91;0;63;0
WireConnection;91;1;92;0
WireConnection;102;0;100;0
WireConnection;102;1;59;0
WireConnection;111;0;110;0
WireConnection;111;1;107;0
WireConnection;111;2;112;0
WireConnection;110;1;104;0
WireConnection;0;2;102;0
WireConnection;0;9;91;0
ASEEND*/
//CHKSM=E7DACE3335403AD95346EBABE5CCE4FDFA0AE2DE