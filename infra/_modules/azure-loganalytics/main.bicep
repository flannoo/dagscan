@minLength(4)
@maxLength(63)
param name string
param location string = resourceGroup().location
param tags object = {}
param retentionInDays int = 30

resource loganalyticsworkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: name
  location: location
  properties: any({
    retentionInDays: retentionInDays
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
  tags: tags
}

output id string = loganalyticsworkspace.id
output name string = loganalyticsworkspace.name
output customerId string = loganalyticsworkspace.properties.customerId
