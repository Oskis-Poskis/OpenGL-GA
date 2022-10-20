#version 330 core

layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

out vec3 FragPos;
out vec3 Normal;
out vec2 texCoord;

in DATA
{
    vec3 FragPos;
    vec3 Normal;
	vec2 texCoord;
    mat4 projection;
} data_in[];

// Default main function
void main()
{
    gl_Position =  gl_in[0].gl_Position * data_in[0].projection;
    FragPos = data_in[0].FragPos;
    Normal = data_in[0].Normal;
    texCoord = data_in[0].texCoord;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position * data_in[1].projection;
    FragPos = data_in[1].FragPos;
    Normal = data_in[1].Normal;
    texCoord = data_in[1].texCoord;
    EmitVertex();

    gl_Position = gl_in[2].gl_Position * data_in[2].projection;
    FragPos = data_in[2].FragPos;
    Normal = data_in[2].Normal;
    texCoord = data_in[2].texCoord;
    EmitVertex();

    EndPrimitive();
}