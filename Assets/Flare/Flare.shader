Shader "Hidden/Custom/Flare"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

    float4 _FlareVector;
    float4 _FlareColor;
    float4 _ParaVector;
    float4 _ParaColor;

    half3 ApplyFlare(half3 color, float2 screenPos)
    {
        float2 flarePos = _FlareVector.xy;
        float2 paraPos = _ParaVector.xy;

        float flare = 1.0 - clamp(length(flarePos - screenPos) * _FlareVector.z, 0, 1);
        float para = 1.0 - clamp(length(paraPos - screenPos) * _ParaVector.z, 0, 1);

        color = color * lerp(float3(1, 1, 1), _ParaColor, para) + lerp(float3(0, 0, 0), _FlareColor, flare);
        return color;
    }

    half4 Frag(VaryingsDefault input) : SV_Target
    {
        half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.texcoord);
        color.rgb = ApplyFlare(color.rgb, (input.texcoord - 0.5) * 2.0);

        return color;
    }
    ENDHLSL
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        ZTest Always ZWrite Off Cull Off

        Pass
        {
            Name "Flare"

            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }
    }
}