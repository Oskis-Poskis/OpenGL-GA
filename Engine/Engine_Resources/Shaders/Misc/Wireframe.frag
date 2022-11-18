#version 330 core

in vec2 texCoord;
in vec3 Normal;
in mat3 TBN;

out vec4 fragColor;

uniform vec3 col;
uniform sampler2D normalTex;

void main()
{
	vec3 test = col;
	vec3 texColor = texture(normalTex, texCoord).rgb;

	vec3 normal = texture(normalTex, texCoord).rgb * 2 - 1;
    normal = normalize(TBN * normal);

	float diff = 0;
	diff =  max(dot(normal, normalize(vec3(-1,  1,  1))), 0);
	diff += max(dot(normal, normalize(vec3( 1,  1, -1))), 0);
	diff += max(dot(normal, normalize(vec3( 0, -1,  0))), 0);

	diff *= 0.25+ 0.1;

	if (col == vec3(1)) fragColor = vec4(0.75, 0, 0, 1);
	else fragColor = vec4(vec3(diff), 1);
}