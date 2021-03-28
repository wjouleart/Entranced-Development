Shader "Entranced/The Island Shader"
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
            float4 vcolor : COLOR; // vertex color
        };

        sampler2D _MainTex;
        fixed4 _Color;
        half _Glossiness;


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * IN.vcolor;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            o.Smoothness = _Glossiness;
        }
        
        ENDCG
    }

    FallBack "Diffuse"
}
