#version 330 core
in vec3 LightColor;
in vec2 TexCoord;
out vec4 fragColor;

uniform sampler2D lightTexture;

void main()
{
    vec4 color = texture(lightTexture, TexCoord);
    if (color.a < 0.9) discard;

    fragColor = vec4(color);
}