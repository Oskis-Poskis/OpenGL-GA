#version 330 core

in vec2 texCoord;
in vec3 FragPos;
in vec3 Normal;

struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shininess;
};

struct PointLight {
    vec3 lightPos;
    vec3 lightColor;

    float constant;
    float linear;
    float quadratic;
}; 

struct DirectionalLight {
    vec3 direction;
    vec3 color;
};

uniform Material material;

#define MAX_PointsLights 2
uniform int NR_PointLights;
uniform PointLight pointLights[MAX_PointsLights];
uniform DirectionalLight dirLight;

uniform vec3 viewPos;

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec3 color)
{
    vec3 ambient = material.ambient;

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.lightColor * (diff * material.diffuse * color);
    
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = spec * material.specular;
    
    float _distance = length(light.lightPos - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * _distance + light.quadratic * (_distance * _distance));

    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}

vec3 CalcDirectionalLight(DirectionalLight directLight)
{
    vec3 ambient = material.ambient;
  	
    // diffuse 
    vec3 norm = normalize(Normal);
    // vec3 lightDir = normalize(light.position - FragPos);
    vec3 lightDir = normalize(-directLight.direction);  
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = directLight.color * diff * material.diffuse;
    
    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = spec * material.specular;
        
    return (ambient + diffuse + specular);
}

out vec4 fragColor;
uniform sampler2D texture0;

void main()
{
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 tex = vec3(1);
    //vec3 tex = vec3(texture(texture0, texCoord));

    //CalcDirectionalLight(dirLight);

    vec3 result = vec3(0);
    for(int i = 0; i < NR_PointLights; i++)
        result += CalcPointLight(pointLights[i], Normal, FragPos, viewDir, tex);

    fragColor = vec4(result, 1.0);
}