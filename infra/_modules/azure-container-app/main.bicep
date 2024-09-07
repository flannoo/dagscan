@minLength(5)
@maxLength(32)
param name string
param location string = resourceGroup().location
param appserviceEnvironmentName string
param identityName string
param registryName string
param secrets array = []
param environmentVariables array = []
param containerImageName string
param cpu string = '0.25'
param memory string = '0.5Gi'
param minReplicas int = 1
param maxReplicas int = 1
param ingress object = {}
param ingressEnabled bool = true

param tags object = {}

resource managedEnvironment 'Microsoft.App/managedEnvironments@2024-03-01' existing = {
  name: appserviceEnvironmentName
}

resource userIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: identityName
}

resource registry 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' existing = {
  name: registryName
}

resource app 'Microsoft.App/containerApps@2024-03-01' = {
  name: name
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userIdentity.id}': {}
    }
  }
  properties: {
    environmentId: managedEnvironment.id
    configuration: {
      secrets: secrets
      registries: [
        {
          identity: userIdentity.id
          server: registry.properties.loginServer
        }
      ]
      ingress: ingressEnabled ? ingress : null
    }
    template: {
      containers: [
        {
          name: name
          image: '${registry.properties.loginServer}/${containerImageName}'
          env: environmentVariables
          resources: {
            cpu: json(cpu)
            memory: memory
          }
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
      }
    }
  }
  tags: tags
}
