#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 transform;
uniform bool outline;

out vec2 texCoord;
out vec3 FragPos;
out vec3 Normal;


void main(void)
{
    //gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    //TexCoord = texCoord;
    //Normal = aNormal * mat3(transpose(inverse(transform)));
    //FragPos = vec3(vec4(aPosition, 1.0) * transform);

    if (outline == true) gl_Position = vec4(aPosition + aNormal * 0.025, 1.0) * transform * view * projection;
    else gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    
    FragPos = vec3(vec4(aPosition, 1.0) * transform);
    Normal = aNormal * mat3(transpose(inverse(transform)));;
    texCoord = aTexCoord;
}