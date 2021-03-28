Shader "Entranced/Standard Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0


        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _MainTex;
        half _Glossiness;
        fixed4 _Color;
        float4 _Masking;


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Mask clipping functions
            float2 offset = IN.worldPos.xz - _Masking.zw;
            float outOfBounds = max(offset.x, offset.y);

            offset = _Masking.xy - IN.worldPos.xz;
            outOfBounds = max(outOfBounds, max(offset.x, offset.y));

            clip(-outOfBounds);


            // Default surface shading functions
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}
