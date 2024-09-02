@minLength(5)
@maxLength(63)
param name string
param location string = resourceGroup().location
param tenantId string = tenant().tenantId
param tags object

@description('Include firewall rule that allows Azure services and resources to access this server.')
param allowAzureIps bool = true

@allowed([
  'User'
  'Group'
  'Application'
])
@description('Principal Type of the sever administrator.')
param administratorPrincipalType string
@description('Login name of the server administrator.')
param administratorLogin string
@description('SID (object ID) of the server administrator.')
param administratorObjectId string
@description('Optional. Restrict access to only azure AD authentication.')
param azureADOnlyAuthentication bool = false
@description('Conditional. The administrator username for the server. Required if azureADOnlyAuthentication is set to false.')
param sqladministratorLogin string = ''
@description('Conditional. The administrator login password. Required if azureADOnlyAuthentication is set to false.')
@secure()
param sqladministratorLoginPassword string = ''

resource sqlserver 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: name
  location: location
  tags: tags
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      login: administratorLogin
      azureADOnlyAuthentication: azureADOnlyAuthentication
      principalType: administratorPrincipalType
      sid: administratorObjectId
      tenantId: tenantId
    }
    administratorLogin: azureADOnlyAuthentication ? null : sqladministratorLogin
    administratorLoginPassword: azureADOnlyAuthentication ? null : sqladministratorLoginPassword
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
    version: '12.0'
  }

  resource firewallAzureIpsRule 'firewallRules@2022-05-01-preview' = if(allowAzureIps) {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      endIpAddress: '0.0.0.0'
      startIpAddress: '0.0.0.0'
    }
  }
}

output id string = sqlserver.id
output name string = sqlserver.name
