@minLength(3)
@maxLength(24)
param name string
param location string = resourceGroup().location
@description('''Use 'recover' if the Key Vault is marked as soft-deleted and if it needs to be reactivated. If the Azure Key Vault isn't purged use 'default'''')
param createMode string
param tags object

@description('Specifies whether RBAC authorization should be enabled for the Key Vault.')
param enableRbacAuthorization bool = true

param enableForTemplateDeployment bool = true
param publicNetworkAccess string = 'Enabled'
param enableSoftDelete bool = true

resource keyvault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: name
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    enableSoftDelete: enableSoftDelete
    publicNetworkAccess: publicNetworkAccess
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
    accessPolicies: []
    createMode: createMode
    tenantId: subscription().tenantId
    enabledForTemplateDeployment: enableForTemplateDeployment
    enableRbacAuthorization: enableRbacAuthorization
  }
  tags: tags
}

output id string = keyvault.id
output name string = keyvault.name
