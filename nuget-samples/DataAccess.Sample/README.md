# DataAccess Sample

## Introduction

A sample application for the [DataAccess.Repository](https://github.com/Ian-Webster/DataAccess/pkgs/nuget/DataAccess.Repository) NuGet package

## Usage instructions

To run this project;
1. Set up the [Sandbox database](https://github.com/Ian-Webster/sandbox/tree/main/sandbox-database) 
2. Set up your local NuGet configuration to have GitHub as a source
    1. Setup a personal access token  (see https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#authenticating-with-a-personal-access-token)
    2. Once you have your personal access token you'll need to modify your NuGet config to include GitHub as a source
    3. Open the "run" app and run the following command `%appdata%\NuGet\NuGet.Config`
    4. Add this to the package sources node;
        ```xml
        <add key="github" value="https://nuget.pkg.github.com/Ian-Webster/index.json" />
        ```
    5. Add this section below the package sources node, replacing TOKEN with your personal access token from above;
       ```xml
       <packageSourceCredentials>
        <github>
          <add key="Username" value="Ian-Webster" />
          <add key="ClearTextPassword" value="TOKEN" />
        </github>
        </packageSourceCredentials>  
       ```
    6. Restart Visual Studio, GitHub should now show up as a source when you are managing NuGet packages for your project
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

## Example GraphQL queries;

* Single select with filter:
```json
{
  movie(where: {movieId: {eq: "a2f2ee47-6084-484a-969a-aec304734623"}})
  {
    movieId,
    name
  }
}
```
* List with order by:
```json
{
  movies (order: [{name:ASC}]),
  {
    movieId,
    name
  }
}
```
* Paging with order by:
```json
{
  paginatedMovies (first: 10, order: [{name:DESC}])
  {
    totalCount,
    pageInfo {
      startCursor,
      endCursor,
      hasNextPage,
      hasPreviousPage
    }
    edges {
      node {
        movieId,
        name
      }
    }
  }
}
```