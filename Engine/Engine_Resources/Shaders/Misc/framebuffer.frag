#version 330 core

in vec2 texCoord;
out vec4 fragColor;

uniform sampler2D screenTexture;

const float offset_x = 1.0f / 800.0f;  
const float offset_y = 1.0f / 800.0f;  

vec2 offsets[9] = vec2[]
(
    vec2(-offset_x,  offset_y), vec2( 0.0f,    offset_y), vec2( offset_x,  offset_y),
    vec2(-offset_x,  0.0f),     vec2( 0.0f,    0.0f),     vec2( offset_x,  0.0f),
    vec2(-offset_x, -offset_y), vec2( 0.0f,   -offset_y), vec2( offset_x, -offset_y) 
);

float kernel[9] = float[]
(
    1,  1, 1,
    1, -9, 1,
    1,  1, 1
);

void main()
{
    vec4 result = texture(screenTexture, texCoord);
    //result = vec4(pow(result, vec3(2.2)));
    fragColor = vec4(result);
    //vec3 color = vec3(0.0f);
    //for(int i = 0; i < 9; i++)
    //    color += vec3(texture(screenTexture, texCoord + offsets[i])) * kernel[i];
    //fragColor = vec4(color, 1.0f);
}