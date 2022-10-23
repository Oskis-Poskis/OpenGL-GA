#version 330 core
in vec3 LightColor;

out vec4 fragColor;

void main()
{
    fragColor = vec4(LightColor, 1.0);
}