#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 transform;
uniform vec3 lightcolor;

out vec3 LightColor;
out vec3 Normal;
out vec2 TexCoord;
out mat4 Projection;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    TexCoord = aTexCoord;
    Normal = aNormal * mat3(transpose(inverse(transform)));
    LightColor = lightcolor;
}