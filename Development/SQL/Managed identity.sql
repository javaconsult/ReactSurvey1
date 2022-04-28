-- https://codemag.com/Article/2107041/Eliminate-Secrets-from-Your-Applications-with-Azure-Managed-Identity
-- Providing the App's Managed Identity Access to the Database
-- mssql extension https://docs.microsoft.com/en-us/azure/azure-sql/database/connect-query-vscode#install-visual-studio-code
-- Server=tcp:$sqlServer.database.windows.net,1433;Database=$database;Authentication=Active Directory Managed Identity;


CREATE USER examplewebapp1 FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER examplewebapp1
ALTER ROLE db_datawriter ADD MEMBER examplewebapp1
