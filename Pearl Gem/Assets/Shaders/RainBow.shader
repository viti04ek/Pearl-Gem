Shader "Custom/Rainbow"
{
    Properties
    {
        _Speed ("Speed", Range(0.1, 5)) = 1.0   // Скорость движения радуги
        _StripeWidth ("Stripe Width", Range(0.1, 2.0)) = 0.5  // Толщина полос
        _Blend ("Blend Strength", Range(0, 1)) = 0.3  // Плавность границ
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _Speed;
            float _StripeWidth;
            float _Blend;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Переводим координаты в мир
                return o;
            }

            // Чистые цвета радуги (без белого и черного)
            fixed4 RainbowPalette(float t)
            {
                t = fmod(t, 6.0); // Цикличность для 6 цветов радуги
                if (t < 1.0) return fixed4(1, 0, 0, 1);      // Красный
                if (t < 2.0) return fixed4(1, 0.5, 0, 1);    // Оранжевый
                if (t < 3.0) return fixed4(1, 1, 0, 1);      // Желтый
                if (t < 4.0) return fixed4(0, 1, 0, 1);      // Зеленый
                if (t < 5.0) return fixed4(0, 0, 1, 1);      // Синий
                return fixed4(0.5, 0, 1, 1);                 // Фиолетовый
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float movement = _Time.y * _Speed; // Двигаем полосы со временем
                float stripePattern = (i.worldPos.y + movement) / _StripeWidth; // Делаем равномерные полосы
                float hue = fmod(stripePattern, 6.0); // Число 6 — это количество цветов радуги
                float blendFactor = abs(frac(stripePattern) - 0.5) * 2.0; // Градиентная плавность

                // Генерируем цветные полосы с плавными переходами
                return lerp(RainbowPalette(hue), RainbowPalette(hue + 0.5), smoothstep(0.5 - _Blend, 0.5 + _Blend, blendFactor));
            }
            ENDCG
        }
    }
}
