// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Borrowed code from UnlitVertexColor.shader in ASL from the Tango code base.
Shader "Custom/RoomShader" {
	Properties{
	}

	SubShader{
		Pass{
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

		struct IN
	{
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 color : COLOR;
	};

	v2f vert(IN input) {
		v2f o;
		o.pos = UnityObjectToClipPos(input.pos);
		o.color = input.color;
		return o;
	}

	fixed4 frag(v2f i) : COLOR{
		return i.color;
	}

		ENDCG
	}
	}
}