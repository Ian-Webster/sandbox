# DataAccess Sample

## Introduction

A sample application for the [DataAccess.Repository](https://github.com/Ian-Webster/DataAccess/pkgs/nuget/DataAccess.Repository) NuGet package

## Usage instructions

To run this project;
1. Set up the [Sandbox database](https://github.com/Ian-Webster/sandbox/tree/main/sandbox-database) 
2. Set up your local NuGet configuration to have GitHub as a source (see https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry)
3. Make sure DataAccess.Sample.Web is the startup project
4. Run the solution
5. The web app is set up to use swagger so you should see the swagger UX open in a browser, from here you can use that UX to test the API endpoints.
6. The default credentials for the database server are;
	1. Server = localhost:5432
	2. Username = postgres
	3. Password = postgres
	4. Database = Sandbox
	5. Schema = public
7. If you are unfamiliar with Postgres and do not currently have an IDE installed for connecting to Postgres servers here are some suggestions for Windows;
	1.  https://www.pgadmin.org/
	2.  https://dbeaver.io/
	3.  https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16
	4.  https://www.jetbrains.com/datagrip/
