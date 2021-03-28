Shader "Entranced/The Sea Shader" 
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        [Header(Wave Layer)]
        _NoiseTex ("Wave Texture", 2D) = "white" {}
        _NoiseColor ("Wave Color", Color) = (1,1,1,1)
        _NoiseDirection ("Wave Direction", Vector) = (0,1,0,0)
    }

    SubShader 
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200

        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha
        #pragma target 3.0

        struct Input 
        {
            float2 uv_NoiseTex;
            float3 worldPos;
        };

        fixed4 _Color;
        float4 _Masking;
        sampler2D _NoiseTex;
        fixed4 _NoiseColor;
        float2 _NoiseDirection;


        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
            // Mask clipping functions
            float2 offset = IN.worldPos.xz - _Masking.zw;
            float outOfBounds = max(offset.x, offset.y);
            offset = _Masking.xy - IN.worldPos.xz;
            outOfBounds = max(outOfBounds, max(offset.x, offset.y));
            clip(-outOfBounds);

            // Set the Noise_Texture scrolling behaviour
            fixed4 col = _Color;
            float2 noiseCoordinates = IN.uv_NoiseTex + _NoiseDirection * _Time.y;
            fixed4 noiseLayer = tex2D(_NoiseTex, noiseCoordinates) * _NoiseColor;
            col.rgb = lerp(col.rgb, noiseLayer.rgb, noiseLayer.a);
            col.a = lerp(col.a, 1, noiseLayer.a);

            // Apply values to output struct
            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }

        ENDCG
    }

    FallBack "Diffuse"
}