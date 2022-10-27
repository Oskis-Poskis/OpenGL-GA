#version 330 core
layout (location = 0) in vec3 aPosition;

out vec3 texCoords;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 transform;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    texCoords = vec3(aPosition.x, aPosition.y, -aPosition.z);
}   