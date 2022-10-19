#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 transform;
uniform bool outline;

//out vec2 TexCoord;
//out vec3 FragPos;
//out vec3 Normal;

out DATA
{
    vec3 FragPos;
    vec3 Normal;
	vec2 texCoord;
    mat4 projection;
} data_out;

void main(void)
{
    //gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    //TexCoord = texCoord;
    //Normal = aNormal * mat3(transpose(inverse(transform)));
    //FragPos = vec3(vec4(aPosition, 1.0) * transform);

    if (outline == true) gl_Position = vec4(aPosition + aNormal * 0.1, 1.0) * transform * view;
    else gl_Position = vec4(aPosition, 1.0) * transform * view;
    
    data_out.FragPos = vec3(vec4(aPosition, 1.0) * transform);
    data_out.Normal = aNormal * mat3(transpose(inverse(transform)));;
    data_out.texCoord = aTexCoord;
    data_out.projection = projection;
}