Shader "Entranced/Particle Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo Texture", 2D) = "white" {}
    }

    SubShader 
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        Cull Off

        CGPROGRAM
        #pragma surface surf NoLighting alpha

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        float4 _Masking;


        void surf (Input IN, inout SurfaceOutput o)
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
            o.Alpha = c.a;
        }

        fixed4 LightingNoLighting (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}
