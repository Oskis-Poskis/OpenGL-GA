#version 330 core

in vec2 texCoord;
out vec4 fragColor;

uniform sampler2D screenTexture;

const float MAX_ITER = 128;
const float width = 1278;
const float height = 828;

uniform float Xoffset;
uniform float Yoffset;
uniform float zoom;

float mandelbrot(vec2 uv)
{
   vec2 c = 5 * uv - vec2(0.7, 0);
   vec2 z= vec2(0);
   float iter = 0;
   for (float i; i < MAX_ITER; i++)
   {
       z = vec2(z.x * z.x - z.y * z.y,
                2.0 * z.x * z.y) + c;
       if (dot(z, z) > 4) return iter / MAX_ITER;
       iter++; 
   }
   return 0;
}

void main()
{
    vec2 uv = (gl_FragCoord.xy - 0.5 * vec2(width, height)) / height;
    uv -= vec2(0.5, 0);
    uv += vec2(Xoffset, Yoffset);
    uv *= vec2(zoom);
    vec3 _col = vec3(0);
    float m = mandelbrot(uv);
    _col += m;

    fragColor = vec4(_col, 1);

    //vec3 result = vec3(texture(screenTexture, texCoord));
    //fragColor = vec4(result, 1);
}