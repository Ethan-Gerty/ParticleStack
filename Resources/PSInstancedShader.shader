Shader "ParticleStack/Instanced"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"


            sampler2D _MainTex;

            UNITY_INSTANCING_BUFFER_START(ParticleProperties)
                UNITY_DEFINE_INSTANCED_PROP(float4, _PSColour)
                UNITY_DEFINE_INSTANCED_PROP(float4, _PSUVRect)

            UNITY_INSTANCING_BUFFER_END(ParticleProperties)


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv;

                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                float4 uvRect = UNITY_ACCESS_INSTANCED_PROP(ParticleProperties, _PSUVRect);
                float2 textureUV = uvRect.xy + i.uv * uvRect.zw;
                fixed4 textureColour = tex2D(_MainTex, textureUV);
                fixed4 particleColour = UNITY_ACCESS_INSTANCED_PROP(ParticleProperties, _PSColour);


                return textureColour * particleColour;
            }

            ENDCG
        }
    }
}