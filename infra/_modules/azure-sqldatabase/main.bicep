@minLength(5)
@maxLength(128)
param name string
param location string = resourceGroup().location
param tags object

param sqlServerName string

param skuName string
param skuTier string

param collation string = 'SQL_Latin1_General_CP1_CI_AS'
param maxSizeBytes int = 53687091200

resource sqldatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  name: '${sqlServerName}/${name}'
  location: location
  tags: tags
  sku: {
    name: skuName
    tier: skuTier
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
  }
}
