# https://docs.microsoft.com/en-us/dotnet/core/tools/
# Powershell VS code extension

dotnet --info
#declare variables
$solution="ReactSurvey"
$classLibProject="ClassLibrary1"; 
$webAppProject="WebApi1";
#create solution
dotnet new sln -n $solution
#create class library project
dotnet new classlib -n $classLibProject 
#create web app project
dotnet new webapi -n $webAppProject
#add projects to solution
dotnet sln add $classLibProject
dotnet sln add $webAppProject
dotnet sln list
# add a reference from web project to class lib project
Set-Location $webAppProject
dotnet add reference ../$classLibProject

#add NuGet packages to class library
#https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-dotnet-cli
Set-Location ..\$classLibProject
#dotnet add package <PACKAGE_NAME>
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
#Provides types for persisting Identity data with Entity Framework Core
#https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore?view=aspnetcore-6.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

#add NuGet packages to web app
Set-Location ..\$webAppProject
#https://docs.automapper.org/en/latest/Dependency-injection.html#asp-net-core 
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Mvc.Abstractions
#https://docs.microsoft.com/en-us/ef/core/cli/services 
dotnet add package Microsoft.EntityFrameworkCore.Design 
#swagger
dotnet add package Swashbuckle.AspNetCore

#https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet
#https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential
#DefaultAzureCredential used in Program.cs to access KeyVault
dotnet add package Azure.Identity

#AddAzureKeyVault used in Program.cs to access KeyVault
#https://docs.microsoft.com/en-us/dotnet/api/overview/azure/extensions.aspnetcore.configuration.secrets-readme
dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets


# create an empty Git repository
Set-Location ..\
git init
mkdir .gitignore

dotnet watch run --project $webAppProject --launch-profile $webAppProject






#create solution - script with fewer comments
$solution="ReactSurvey"
$classLibProject="ClassLibrary1"; 
$webAppProject="WebApi1";
dotnet new sln -n $solution
dotnet new classlib -n $classLibProject 
dotnet new webapi -n $webAppProject
dotnet sln add $classLibProject
dotnet sln add $webAppProject
dotnet sln list
Set-Location $webAppProject
dotnet add reference ../$classLibProject
Set-Location ..\$classLibProject
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Set-Location ..\$webAppProject
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Mvc.Abstractions
dotnet add package Microsoft.EntityFrameworkCore.Design 
dotnet add package Swashbuckle.AspNetCore
dotnet add package Azure.Identity
dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets
Set-Location ..\
git init
mkdir .gitignore
dotnet watch run --project $webAppProject --launch-profile $webAppProject