// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "UI/MovingUIStoreGun"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        _Speed ("Speed", Float) = 0
        _TrailFactor ( "Trail Time", Float) = 0

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 global   : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _OffsetLimit;
            float _Speed;
            float _CurrentX;
            float _TrailFactor;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.global = OUT.vertex;
                
                //OUT.global = mul (unity_ObjectToWorld, v.vertex).xy;
                //OUT.global = UnityObjectToViewPos(v.vertex);
                
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                //OUT.texcoord = TRANSFORM_TEX(v.texcoord + float2(-sin((_Time.y * _Speed / 4) % 0.01),0), _MainTex);
                
                
                //OUT.texcoord = TRANSFORM_TEX(v.texcoord , _MainTex);
                //OUT.texcoord.xy += float2(1.1,0);
                OUT.color = v.color;// * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord)) * IN.color;
                //half abc = IN.vertex.x / ( (sin(_Time[1] % 1.2) * 3600));
                //color.a *= (1 - abc); //color * half4(1, 1 * abc, 1 * abc,1);

                //_CurrentX = 6500 * _SinTime[3];

                float curX =  sin((_Time.y * _Speed) % 1.3) * 7800;
                float abc = saturate(curX - IN.vertex.x);
                abc = abc * (1 - ((curX) - IN.vertex.x) / curX * _TrailFactor);
                color.a *= abc;
                //#ifdef UNITY_UI_CLIP_RECT
                //color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                //#endif

                //#ifdef UNITY_UI_ALPHACLIP
                //clip (color.a - 0.001);
                //endif

                return color;
            }
        ENDCG
        }
    }
}