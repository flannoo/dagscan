param keyvaultName string
param name string
@secure()
param value string

resource secret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: '${keyvaultName}/${name}'
  properties: {
    value: value
  }
}

output secretName string = name
