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
var containerAppWorkerName = replace(
  replace(
    replace(parameters.general.dagscan.containerAppWorkerName, '{env}', env),
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

// =================================================================================
// Azure Resources
// =================================================================================
module containerApp '_modules/azure-container-app/main.bicep' = {
  name: containerAppWorkerName
  params: {
    name: containerAppWorkerName
    location: location
    tags: tags
    appserviceEnvironmentName: containerAppEnvironmentName
    identityName: managedIdentityName
    registryName: registryName
    containerImageName: containerImageName
  }
}
