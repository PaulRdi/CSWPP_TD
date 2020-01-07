Shader "Custom/Fade"
{
	//in .shader Dateien sind zwei Programmiersprachen zu finden.
	//Eine ist "Shaderlab" eine Unity-Interne Sprache, die zum definieren von Eigenschaften benutzt wird.
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_FadeColor("Fade Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_FadeThresh("Fade Y Threshold", Float) = 0.0
		_FadeDist("Fade Distance", Range(.1, 2)) = 1.0
    }
    SubShader
    {
			//Transparentes rendering!
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
		//Ab hier wird cg (kurz für "C for Graphics") benutzt, um den tatsächlichen Shader zu schreiben.

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			//world pos hinzugefügt, um Position abfragen zu können
			float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
		float _FadeThresh;
		float _FadeDist;
        fixed4 _Color;
		fixed4 _FadeColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


			//Da wir ein Standard-Setting gewählt haben, existiert bereits eine Surface-Funktion, in der wir Berechnungen anstellen können.
			//
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			//distanz von der Welt-Position zur entsprechenden Y-Koordinate
			float distToY = IN.worldPos.y - _FadeThresh;
			//die cg-Funktion "clamp" setzt den Wert zwischen 0 und FadeDist fest. Das heißt die Maximale Distanz für unseren Fade wird FadeDist sein!
			//Dies funktioniert, da wir später beim Interpolieren durch FadeDist teilen!
			distToY = abs(clamp(distToY, -_FadeDist, 0.0));
			float t = distToY / _FadeDist;
			c.r = lerp(c.r, _FadeColor.r, t);
			c.g = lerp(c.g, _FadeColor.g, t);
			c.b = lerp(c.b, _FadeColor.b, t);
			o.Alpha = c.a;
			o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;


        }
        ENDCG
    }
    FallBack "Diffuse"
}
