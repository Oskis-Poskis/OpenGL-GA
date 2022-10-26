#version 330 core
in vec2 texCoord;
in vec3 FragPos;
in vec3 Normal;

struct Material
{
    vec3 ambient;
    vec3 diffuse;
    int diffMap;

    vec3 specular;
    float shininess;
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
uniform int NR_DirLights;
uniform PointLight pointLights[MAX_PointsLights];
uniform DirectionalLight dirLight;
uniform Material material;

uniform vec3 viewPos;
uniform sampler2D diffuseMap;
uniform sampler2D normalMap;

const float constant = 1;
const float linear = 0.09;
const float quadratic = 0.032;

// Calculate a directional light with color
vec3 CalcPointLight(PointLight light, vec3 fragPos, vec3 viewDir, vec3 color, vec3 norm)
{
    vec3 ambient = material.ambient;

    vec3 lightDir = normalize(light.lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.lightColor * (diff * material.diffuse * color);
    
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = spec * light.lightColor * material.specular;
    
    float _distance = length(light.lightPos - FragPos);
    //float attenuation = 1.0 / (constant + linear * _distance + quadratic * (_distance * _distance)); Correct attenuation
    float attenuation = pow(smoothstep(light.radius, 0, _distance), light.compression); // Attenuation with radius and fallof settings

    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular) * light.strength;
}

// Calculate a point light with color
vec3 CalcDirectionalLight(DirectionalLight directLight, vec3 color, vec3 norm)
{
    vec3 ambient = material.ambient;
  	
    vec3 lightDir = normalize(directLight.direction);  
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = directLight.color * diff * color;

    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = spec * directLight.color * material.specular;
        
    return (ambient + diffuse + specular) * directLight.strength;
}

uniform highp float NoiseAmount;
highp float NoiseCalc = NoiseAmount / 255;
highp float random(highp vec2 coords) {
   return fract(sin(dot(coords.xy, vec2(12.9898,78.233))) * 43758.5453);
}


out vec4 fragColor;

void main()
{
    vec3 result = vec3(0);
    vec3 col;
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    // If has a diffuse map
    if (material.diffMap == 1)
    {
        col = vec3(texture(diffuseMap, texCoord));
        norm = norm;
        vec3 empty = vec3(texture(normalMap, texCoord)); // Placeholder for using normal texture
    }
    // Else regular color
    else col = vec3(material.diffuse);

    // Directional Lights
    if (NR_DirLights == 1) result += CalcDirectionalLight(dirLight, col, norm);

    // Multiple Point Lights
    for (int i = 0; i < NR_PointLights; i++) result += CalcPointLight(pointLights[i], FragPos, viewDir, col, norm);

    // Reduce color banding
    result += mix(-NoiseCalc, NoiseCalc, random(texCoord));

    float ndc = gl_FragCoord.z * 2 - 1;
    float near = 0.01;
    float far = 100;

    float linearDepth = (2.0 * near * far) / (far + near - ndc * (far - near));	

    // Final Color
    fragColor = vec4(vec3(result), 1.0);
}