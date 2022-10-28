﻿#version 330 core
in vec2 texCoord;
in vec3 FragPos;
in vec3 Normal;

struct Material
{
    vec3 ambient;
    vec3 albedo;
    float metallic;
    float roughness;
};

struct PointLight {
    vec3 lightPos;
    vec3 lightColor;

    float radius;
    float compression;
    float strength;
}; 

struct DirectionalLight {
    vec3 direction;
    vec3 color;
    float strength;
};

#define MAX_PointsLights 16
uniform int NR_PointLights;
uniform PointLight pointLights[MAX_PointsLights];
uniform DirectionalLight dirLight;
uniform Material material;

uniform samplerCube skybox;

uniform vec3 viewPos;
uniform sampler2D diffuseMap;
uniform sampler2D normalMap;

const float constant = 1;
const float linear = 0.09;
const float quadratic = 0.032;

uniform highp float NoiseAmount;
highp float NoiseCalc = NoiseAmount / 255;
highp float random(highp vec2 coords) {
   return fract(sin(dot(coords.xy, vec2(12.9898,78.233))) * 43758.5453);
}

const float ao = 1;

const float PI = 3.14159265359;
// ----------------------------------------------------------------------------
float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a = roughness*roughness;
    float a2 = a*a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH*NdotH;

    float nom   = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;

    return nom / denom;
}
// ----------------------------------------------------------------------------
float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float nom   = NdotV;
    float denom = NdotV * (1.0 - k) + k;

    return nom / denom;
}
// ----------------------------------------------------------------------------
float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx2 = GeometrySchlickGGX(NdotV, roughness);
    float ggx1 = GeometrySchlickGGX(NdotL, roughness);

    return ggx1 * ggx2;
}
// ----------------------------------------------------------------------------
vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(clamp(1.0 - cosTheta, 0.0, 1.0), 5.0);
}




out vec4 fragColor;

void main()
{
    vec3 N = normalize(Normal);
    vec3 V = normalize(viewPos - FragPos);
    vec3 R = reflect(-V, N);

    vec3 F0 = vec3(0.04);
    F0 = mix(F0, material.albedo, material.metallic);

    vec3 Lo = vec3(0.0);
    for (int i = 0; i < NR_PointLights; i++)
    {
        // Calc per light radiance
        vec3 L = normalize(pointLights[i].lightPos - FragPos);
        vec3 H = normalize(V + L);
        float distance = length(pointLights[i].lightPos - FragPos);
        float attenuation = 1.0 / (distance * distance);
        vec3 radiance = pointLights[i].lightColor * attenuation;

        // Cook-Torrance BRDF
        float NDF = DistributionGGX(N, H, material.roughness);   
        float G   = GeometrySmith(N, V, L, material.roughness);      
        vec3 F    = fresnelSchlick(clamp(dot(H, V), 0.0, 1.0), F0);

        vec3 numerator    = NDF * G * F; 
        float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0) + 0.0001; // + 0.0001 to prevent divide by zero
        vec3 specular = numerator / denominator;

        vec3 kS = F;
        vec3 kD = vec3(1) - kS;
        kD *= 1 - material.metallic;

        float NDotL = max(dot(N, L), 0.0);

        Lo += (kD * material.albedo / PI + specular) * radiance * NDotL;
    } 

    vec3 ambient = material.ambient * material.albedo * ao;
    vec3 color = ambient + Lo;

    color = color / (color + vec3(1));
    color = pow(color, vec3(1 / 2.2));

    // Reduce color banding
    color += mix(-NoiseCalc, NoiseCalc, random(texCoord));

    // Final Color
    fragColor = vec4(color, 1.0);
}