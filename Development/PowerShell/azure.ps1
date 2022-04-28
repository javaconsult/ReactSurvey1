# F8 - select a block, then F8 to run
# https://docs.microsoft.com/en-us/dotnet/core/tools/
# Powershell VS code extension


# LOGIN
az login


# RESOURCE GROUP
# create resource group
$resourceGroup = "ResourceGroup1"
$location = "uksouth"
az group create --location $location --name $resourceGroup --output none
Write-Output "Created resource group"
az account list-locations --output table


# WEB APP
# create an app service plan
$resourceGroup = "ResourceGroup1"
$appServicePlan = "AppServicePlan1"
$sku = "FREE"
az appservice plan create --name $appServicePlan --resource-group $resourceGroup --sku $sku --output none

# create a web app
$webApp = "examplewebapp1"
$deploymentLocalGitUrl = az webapp create --resource-group $resourceGroup --plan $appServicePlan --name $webApp `
--deployment-local-git --query "deploymentLocalGitUrl" --output tsv
Write-Output "Deploy to $deploymentLocalGitUrl"

# assign managed service identity to the web app
# https://docs.microsoft.com/en-us/azure/azure-app-configuration/howto-integrate-azure-managed-service-identity
az webapp identity assign --name $webApp --resource-group $resourceGroup --output none

# delete a web app. also deletes app service
# az webapp delete --resource-group $resourceGroup --name  $webApp 

# DATABASE
# Create a server
# https://docs.microsoft.com/en-us/cli/azure/sql/server?view=azure-cli-latest#az-sql-server-create
# variables
$resourceGroup = "ResourceGroup1"
$location = "uksouth"
$sqlServerName = "exampleserver1"
$sqlServerUser = "exampleuser"
$sqlServerPassword = "xxx"

az sql server create --name $sqlServerName --resource-group $resourceGroup --location $location `
--admin-user $sqlServerUser --admin-password $sqlServerPassword --output none

# create a database
# https://docs.microsoft.com/en-us/cli/azure/sql/db?view=azure-cli-latest#az-sql-db-create
$databaseName = "db1"
$serviceObjective="Free"
az sql db create --name $databaseName --resource-group $resourceGroup --server $sqlServerName `
--service-objective $serviceObjective --output none

# list service objectives
az sql db list-editions --available --location $location --output table

# show database connection string
# https://docs.microsoft.com/en-us/cli/azure/sql/db?view=azure-cli-latest#az-sql-db-show-connection-string
az sql db show-connection-string --client ado.net --server $sqlServerName --name $databaseName  

# create a firewall rule enabling access from local PC 
# https://docs.microsoft.com/en-us/cli/azure/sql/server/firewall-rule?view=azure-cli-latest#az-sql-server-firewall-rule-create 
$remoteIpAddressFrom = "0.0.0.0" #https://www.whatsmyip.org/
$remoteIpAddressTo = "255.255.255.255" 
az sql server firewall-rule create --name RemoteConnection1 --resource-group $resourceGroup `
--server $sqlServerName --start-ip-address=$remoteIpAddressFrom --end-ip-address=$remoteIpAddressTo --output none

# create a firewall rule enabling access from Azure services
az sql server firewall-rule create --name AllowAzureIps --resource-group $resourceGroup `
--server $sqlServerName --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0 --output none

# Enable Azure Active Directory authentication
# https://docs.microsoft.com/en-gb/azure/azure-sql/database/authentication-aad-overview

# get currently logged-in user
$userPrincipalName = az ad signed-in-user show --query "userPrincipalName" --output tsv
Write-Output "userPrincipalName $userPrincipalName"

# get Azure AD user 
# https://docs.microsoft.com/en-us/cli/azure/ad/user?view=azure-cli-latest
$objectId = az ad user show --id $userPrincipalName --query "objectId" --output tsv
Write-Output "Azure AD user $objectId"

# Enable Azure Active Directory authentication 
az sql server ad-admin create --resource-group $resourceGroup --server-name $sqlServerName `
--display-name ADMIN --object-id $objectId --output none



#SSMS
$connectionString="Server=tcp:$sqlServer.database.windows.net,1433;Database=$database;Authentication=Active Directory Managed Identity;"
Write-Output $connectionString
# Providing the App's Managed Identity Access to the Database
CREATE USER examplewebapp1 FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER examplewebapp1
ALTER ROLE db_datawriter ADD MEMBER examplewebapp1


# KEY VAULT

# create a key vault
$resourceGroup = "ResourceGroup1"
$location = "uksouth"
$vaultName = "examplevault1"
az keyvault create --name $vaultName --resource-group $resourceGroup --location $location --output none

# add a secret to Key Vault – db connection string
$secretNameDbConnection = "ConnectionStrings--AzureConnection"
$connectionString="Server=tcp:$sqlServer.database.windows.net,1433;Database=$database;Authentication=Active Directory Managed Identity;"
az keyvault secret set --vault-name $vaultName --name  $secretNameDbConnection --value $connectionString `
--output none

# show the secret 
az keyvault secret show --vault-name $vaultName --name $secretNameDbConnection --query "value"  --output tsv

# add a secret to Key Vault – jwt token key
$secretNameTokenKey = "Settings--TokenKey"
$tokenKey = "xxx"
az keyvault secret set --vault-name $vaultName --name  $secretNameTokenKey --value $tokenKey --output none

# give web app permission to do get and list operations on the key vault (see key vault / access policies in portal)
# get web app's objectId 
$principalId = az webapp identity show --name $webApp --resource-group $resourceGroup --query "principalId" `
--output tsv
Write-Output $principalId
az keyvault set-policy --name $vaultName --object-id $principalId --secret-permissions get list --output none









# LOGGING
# AddAzureWebAppDiagnostics in Program.cs
# set log level in App Service to Information from the default level Error
az webapp log config --name $webApp --resource-group $resourceGroup --application-logging filesystem `
--level information --output none
# start log streaming
az webapp log tail --name $webApp --resource-group $resourceGroup
Write-Output "Set up logging"


# REACT build
# generate production files in web app's wwwroot folder
# include in package.json after build script:   "postbuild": "move build ../WebApi1/wwwroot",
$project="reactapp1"
Set-Location $project
npm run build
Write-Output "Generated React production build in wwwroot"


# GIT deployment
# Local Git deployment to Azure App Service https://docs.microsoft.com/en-us/azure/app-service/deploy-local-git
# To get automated builds, there must be a *.sln or *.csproj in the repository root 
# variables
$deploymentUser = "exampledeployuser"
$deploymentPassword = "xxx"
# create a deployment user - shared by all web apps in the subscription
az webapp deployment user set --user-name $deploymentUser --password $deploymentPassword --output none

# Get the details of a source control deployment configuration
$kuduUrl = az webapp deployment source show --name $webApp --resource-group $resourceGroup `
--query "repoUrl" --output tsv
Write-Output "Kudu server $kuduUrl"



# GIT remote
# display git remote url
$webApp = "examplewebapp1"
$deploymentUser = "exampledeployuser"
$remoteGitUrl="https://$deploymentUser@$webApp.scm.azurewebsites.net/$webApp.git"
Write-Output $remoteGitUrl

# change to directory containing .git
Set-Location $repositoryRoot
# https://<deployment-username>@<app-name>.scm.azurewebsites.net/<app-name>.git
$remoteGitUrl="https://$deploymentUser@$webApp.scm.azurewebsites.net/$webApp.git"
# add an Azure remote to the local Git repository
# https://<username>@<app-name>.scm.azurewebsites.net/<app-name>.git.
git remote add azure $remoteGitUrl
#set the default branch as main
git branch -m main
# make any changes, change to git directory, stage, commit 
git add .
git commit -m "Update $(Get-Date)"
# Push to the Azure remote. Enter deployment user password in dialog
git push azure main --quiet
Write-Output "Deployed to git remote"


# MIGRATIONS
#dotnet tool update --global dotnet-ef
# -p The target project (containing DbContext)
# -s Relative path to the project folder of the startup project
dotnet ef migrations add Initial -p ClassLibrary1 -s WebApi1
dotnet ef database update -p ClassLibrary1 -s WebApi1
Write-Output "Migrations completed (created tables)"


# DELETE
# delete a resource group
$resourceGroup = "ResourceGroup1"
az group delete --name $resourceGroup --yes
Write-Output "Deleted resource group"
# purge vault
$vaultName = "examplevault1"
az keyvault purge --name $vaultName
Write-Output "Purged vault"






#open browser
# start-process https://examplewebapp1.azurewebsites.net/



