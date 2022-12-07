#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;
layout (location = 3) in vec3 aTangent;
layout (location = 4) in vec3 aBiTangent;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 transform;

uniform bool selectedObj;
uniform float outlineOffset;

out vec2 texCoord;
out vec3 FragPos;
out vec3 Normal;
out mat3 TBN;

void main(void)
{
    gl_Position = vec4(aPosition, 1) * transform * view * projection;

    float test = outlineOffset;

    FragPos = vec3(vec4(aPosition, 1.0) * transform);

    Normal = aNormal * mat3(transpose(inverse(transform)));
    texCoord = aTexCoord;

    vec3 T = normalize(vec3(aTangent * mat3(transpose(inverse(transform)))));
    vec3 B = normalize(vec3(aBiTangent * mat3(transpose(inverse(transform)))));
    vec3 N = normalize(Normal);
    TBN = mat3(T, B, N);
}