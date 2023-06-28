Shader "Custom/UnderwaterWobble" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)  // New color property
        _WobbleAmount ("Wobble Amount", Range(0, 1)) = 0.1
        _WobbleSpeed ("Wobble Speed", Range(0, 10)) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;  // New color property
        float _WobbleAmount;
        float _WobbleSpeed;

        void surf (Input IN, inout SurfaceOutput o) {
            // Apply a simple sine wave to the UV coordinates to create a wobbling effect
            float2 wobbledUV = IN.uv_MainTex;
            wobbledUV.x += sin(_Time.y * _WobbleSpeed + wobbledUV.y * 10) * _WobbleAmount;

            fixed4 c = tex2D(_MainTex, wobbledUV);
            o.Albedo = c.rgb * _Color.rgb;  // Apply the color
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
