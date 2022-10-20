#version 330 core

in vec2 TexCoord;
in vec3 Normal;
in vec3 LightColor;

out vec4 fragColor;

void main()
{
    fragColor = vec4(LightColor, 1.0);
}