// =================================================================================
// Input parameters
// =================================================================================
@allowed(['dev', 'tst', 'uat', 'prd'])
param env string = 'prd'

@secure()
param adminUserLogin string

@secure()
param adminUserSid string

// =================================================================================
// Azure Resource Name variables
// =================================================================================
var parameters = loadJsonContent('infra.parameters.json')
var location = parameters.general.location
var locationAbbreviation = parameters.general.locationAbbreviation
var purpose = parameters.general.dagscan.purpose
var tags = parameters[env].tags

var resourceGroupName = replace(
  replace(
    replace(parameters.general.dagscan.resourceGroupName, '{env}', env),
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
var applicationInsightsName = replace(
  replace(
    replace(parameters.general.dagscan.applicationinsightsName, '{env}', env),
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
var keyVaultName = replace(
  replace(replace(parameters.general.dagscan.keyvault, '{env}', env), '{locationAbbreviation}', locationAbbreviation),
  '{purpose}',
  purpose
)
var logAnalyticsName = replace(
  replace(
    replace(parameters.general.dagscan.loganalytics, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var containerAppEnvironmentName = replace(
  replace(
    replace(parameters.general.dagscan.containerAppEnvironmentName, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var sqlServerName = replace(
  replace(
    replace(parameters.general.dagscan.sqlServerName, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)
var sqlDatabaseName = replace(
  replace(
    replace(parameters.general.dagscan.sqlDatabaseName, '{env}', env),
    '{locationAbbreviation}',
    locationAbbreviation
  ),
  '{purpose}',
  purpose
)

// =================================================================================
// Azure Resources
// =================================================================================
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' existing = {
  name: resourceGroupName
  scope: subscription()
}

module managedIdentity '_modules/azure-managedidentity/main.bicep' = {
  name: managedIdentityName
  params: {
    identityName: managedIdentityName
    tags: tags
  }
  scope: resourceGroup
}

module containerRegistry '_modules/azure-container-registry/main.bicep' = {
  name: registryName
  params: {
    registryName: registryName
    location: location
    acrAdminUserEnabled: false
    publicNetworkAccess: 'Enabled'
    tags: tags
  }
  scope: resourceGroup
}

module containerRegistryManagedIdentityAccess '_modules/azure-container-registry/role-assignment/main.bicep' = {
  name: '${registryName}-mi-access'
  params: {
    roleName: 'acr pull'
    registryName: containerRegistry.outputs.name
    principalId: managedIdentity.outputs.principalId
    principalType: 'ServicePrincipal'
  }
  scope: resourceGroup
}

module containerRegistryAdminAccess '_modules/azure-container-registry/role-assignment/main.bicep' = {
  name: '${registryName}-admin-access'
  params: {
    roleName: 'acr pull'
    registryName: containerRegistry.outputs.name
    principalId: adminAdUser.sid
    principalType: 'User'
  }
  scope: resourceGroup
}

module logAnalytics '_modules/azure-loganalytics/main.bicep' = {
  name: logAnalyticsName
  params: {
    name: logAnalyticsName
    location: location
    tags: tags
  }
  scope: resourceGroup
}

module applicationInsights '_modules/azure-applicationinsights/main.bicep' = {
  name: applicationInsightsName
  params: {
    name: applicationInsightsName
    location: location
    workspaceResourceId: logAnalytics.outputs.id
    disableLocalAuth: true
    tags: tags
  }
  scope: resourceGroup
}

module keyVault '_modules/azure-keyvault/main.bicep' = {
  name: keyVaultName
  params: {
    name: keyVaultName
    location: location
    tags: tags
    createMode: 'default'
    enableForTemplateDeployment: false
    enableSoftDelete: false
    publicNetworkAccess: 'Disabled'
  }
  scope: resourceGroup
}

module keyvaultAccessManagedIdentity '_modules/azure-keyvault/role-assignment/main.bicep' = {
  name: '${keyVaultName}-mi-access'
  params: {
    roleName: 'Key Vault Secrets User'
    keyVaultName: keyVault.outputs.name
    principalId: managedIdentity.outputs.principalId
    principalType: 'ServicePrincipal'
  }
  scope: resourceGroup
}

module keyvaultAccessAdmin '_modules/azure-keyvault/role-assignment/main.bicep' = {
  name: '${keyVaultName}-admin-access'
  params: {
    roleName: 'Key Vault Administrator'
    keyVaultName: keyVault.outputs.name
    principalId: adminUserSid
    principalType: 'User'
  }
  scope: resourceGroup
}

module containerAppEnvironment '_modules/azure-container-appenvironment/main.bicep' = {
  name: containerAppEnvironmentName
  params: {
    name: containerAppEnvironmentName
    location: location
    tags: tags
    logAnalyticsWorkspaceId: logAnalytics.outputs.id
    logAnalyticsCustomerId: logAnalytics.outputs.customerId
  }
  scope: resourceGroup
}

module sqlServer '_modules/azure-sqlserver/main.bicep' = {
  name: sqlServerName
  params: {
    name: sqlServerName
    location: location
    tags: tags
    allowAzureIps: true
    administratorPrincipalType: 'User'
    administratorLogin: adminUserLogin
    administratorObjectId: adminUserSid
    azureADOnlyAuthentication: true
  }
  scope: resourceGroup
}

module sqlDatabase '_modules/azure-sqldatabase/main.bicep' = {
  name: sqlDatabaseName
  params: {
    sqlServerName: sqlServer.outputs.name
    name: sqlDatabaseName
    collation: 'SQL_Latin1_General_CP1_CS_AS'
    skuName: 'S0'
    skuTier: 'Standard'
    tags: tags
  }
  scope: resourceGroup
}
