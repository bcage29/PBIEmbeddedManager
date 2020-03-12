# Power BI Embedded Automation Samples

## Overview

This is a sample application for automating the management of Power BI Embedded capacities and Power BI Workspaces (V2) using a dotnet core application. The application is divided into three different projects (Backend/Frontend/Database console app). The Visual Studio solution is set to have multiple start up projects which starts the Frontend and Backend projects. The  NOTE: There is no authentication between the Frontend and Backend projects.

This repo to contain demos for automating PowerBI Embedded Capacities and Workspaces. The samples will include how-to examples for the following scenarios:

- Power BI Embedded Capacity Management
  - Provisining/de-provisioning
  - Scale Up/Down
  - Pause/Resume
  - Update

- Power BI Workspace (V2) Management
  - Create Workspace
  - Assign to Power BI Embedded Capacity
  - Upload PBIX
  - Update Dataset Parameters
  - Update Data Source Credentials
  - Refresh (process cube) a Dataset
  - BYOK (Less than 10GB datasets) (requires User Admin Token / NOT IMPLEMENTED)


### Quick summary

#### PBIAzureManagerService (Backend)

The PBI Azure Manager Service contains the code samples for interacting with PBI Embedded Capacities (managed in Azure) and PBI Workspaces.
To run this sample, update the following App Settings:
``` JSON
"ApplicationSettings": {
    "TenantId": "Directory (tenant) ID",
    "SubscriptionId": "found in resource group overview",
    "ServicePrincipalId": "Azure AD Application (client) ID",
    "ServicePrincipalOid": "Click on Managed application in local directory - Object ID",
    "ServicePrincipalKey": "Azure AD Application Secret",
    "DatabaseServerName": "Data Warehouse Server Name",
    "DatabaseName": "Data Warehouse Database Name",
    "DatabaseUserName": "Data Warehouse User Name",
    "DatabasePassword": "Data Warehouse User Password",
    "UserEmail": "User account to add to the workspace as an Admin"
  }
```
#### PBIManagerWeb (Front End)

The PBI Manager Web is a dotnet core project hosting a simple Frontend for the interacting with the backend services.

## SetUp
1. Create a resource group
2. Create an Azure AD Application and Service Principal or run CreateServicePrincipal.ps1 script
3. Save ClientId, Client Secret and ObjectId for the service account
4. Create an Azure AD Group for the service principals (i.e., Power BI Service Principals)
5. Add the service account to the resource group with the contributor role
6. Add the service account to the group created in step 4
7. Go to https://app.powerbi.com/admin-portal/tenantSettings and configure Allow service principals to use Power BI APIs in the Developer Settings section
8. Update the application settings in the PBIAzureManagerService
9. If you need the service principal to create new service principals and add roles to those generated service principals, add the service account as a gloabl administrator (NOTE: only necessary for creating new service principals to manage a workspace. This process can happen before hand and is not necessary for interacting with PowerBI)

NOTE: The Workspace Service that handles creating a service principal and adding that service princpal to a workspace and capacity has been commented out. Once the service principal creating/fetching has been implemented, you can uncomment this code.

## Permissions
To run this sample, the service account that is talking to Azure and Power BI needs to be a contributor of the recource group where the Power BI capacities will be created. The service account also needs to be a member of the Power BI Service Principlas group (refer to step 4 in SetUp).

## PBIX
This sample was created from the NY Taxi Data Warehouse for simplicity and the sample PBIX can be found in the solution files. In Power BI Desktop, connect to your data source and create your model. Once your model is created, add parameters to the dataset for 'servername' and 'databasename'. This will allow for the dataset to be parametrized and updated in Power BI. 
NOTE: If you are using the PBIX file as a default PBIX for all datasets to create the cube in PowerBI, then follow the next steps. Create another data source (ie data warehouse) with the same schema and with no data in the tables. Update the dataset parameters to point to the empty data source. Save and close the file. 

 ### Resources
 - https://docs.microsoft.com/en-us/rest/api/power-bi-embedded/
 - https://docs.microsoft.com/en-us/power-bi/developer/embed-sample-for-customers
 - https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.management.resourcemanager.fluent.genericresource.definition?view=azure-dotnet
 - https://docs.microsoft.com/en-us/power-bi/service-premium-what-is
 - https://docs.microsoft.com/en-us/power-bi/service-encryption-byok