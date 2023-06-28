Shader "Custom/SeaCellularSurface" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Range(1, 100)) = 16
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
        float _NoiseScale;

        float2 random2(float2 st) {
            st = float2(dot(st, float2(127.1, 311.7)),
                        dot(st, float2(269.5, 183.3)));
            return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
        }

        float cellularnoise(float2 st, float n) {
            st *= n;

            float2 ist = floor(st);
            float2 fst = frac(st);

            float distance = 5.0;

            for (int y = -1; y <= 1; y++)
            for (int x = -1; x <= 1; x++) {
                float2 neighbor = float2(x, y);
                float2 p = 0.5 + 0.5 * sin(_Time.y + 6.2831 * random2(ist + neighbor));
                float2 diff = neighbor + p - fst;
                distance = min(distance, length(diff));
            }

            return distance * 0.2;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 originalColor = tex2D(_MainTex, IN.uv_MainTex);
            float noise = cellularnoise(IN.uv_MainTex, _NoiseScale);
            fixed4 effectColor = noise * float4(1, 2, -2, 1) + float4(0.1,0.5,1.5,1);

            // Blend the original color with the effect color.
            // Adjust the 0.5 value to control the blending (0.0 = all original, 1.0 = all effect).
            fixed4 finalColor = lerp(originalColor, effectColor, 0.08);

            o.Albedo = finalColor.rgb;
            o.Alpha = originalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
