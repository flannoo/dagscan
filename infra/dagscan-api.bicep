// =================================================================================
// Input parameters
// =================================================================================
@allowed(['dev', 'tst', 'uat', 'prd'])
param env string = 'prd'

param containerImageName string

// =================================================================================
// Azure Resource Name variables
// =================================================================================
var parameters = loadJsonContent('infra.parameters.json')
var location = parameters.general.location
var locationAbbreviation = parameters.general.locationAbbreviation
var purpose = parameters.general.dagscan.purpose
var tags = parameters[env].tags

var containerAppEnvironmentName = replace(
  replace(
    replace(parameters.general.dagscan.containerAppEnvironmentName, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var registryName = replace(
  replace(
    replace(parameters.general.dagscan.containerRegistry, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var containerAppApiName = replace(
  replace(
    replace(parameters.general.dagscan.containerAppApiName, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var managedIdentityName = replace(
  replace(
    replace(parameters.general.dagscan.managedIdentityName, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var keyVaultName = replace(
  replace(replace(parameters.general.dagscan.keyvault, '{env}', env), '{locationAbbreviation}', locationAbbreviation),
  '{purpose}',
  purpose
)

var environmentVariables = [
  {
    name: 'Logging__LogLevel__Default'
    value: 'Information'
  }
  {
    name: 'Logging__LogLevel__Microsoft'
    value: 'Error'
  }
  {
    name: 'Logging__LogLevel__System'
    value: 'Warning'
  }
  {
    name: 'DB_CONNECTION_STRING'
    secretRef: 'https://${keyVaultName}.${environment().suffixes.keyvaultDns}/secrets/database-connectionstring'
  }
  {
    name: 'AZURE_CLIENT_ID'
    secretRef: 'https://${keyVaultName}.${environment().suffixes.keyvaultDns}/secrets/managed-identity-client-id'
  }
]

// =================================================================================
// Azure Resources
// =================================================================================
resource userIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: managedIdentityName
}

module containerApp '_modules/azure-container-app/main.bicep' = {
  name: containerAppApiName
  params: {
    name: containerAppApiName
    location: location
    tags: tags
    appserviceEnvironmentName: containerAppEnvironmentName
    identityName: managedIdentityName
    registryName: registryName
    containerImageName: containerImageName
    environmentVariables: environmentVariables
    secrets: [
      {
        name: 'database-connectionstring'
        identity: userIdentity.id
        keyVaultUrl: 'https://${keyVaultName}.${environment().suffixes.keyvaultDns}/secrets/database-connectionstring'
      }
      {
        name: 'managed-identity-client-id'
        identity: userIdentity.id
        keyVaultUrl: 'https://${keyVaultName}.${environment().suffixes.keyvaultDns}/secrets/managed-identity-client-id'
      }
    ]
  }
}
