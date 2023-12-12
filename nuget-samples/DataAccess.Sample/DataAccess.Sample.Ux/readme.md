# DataAccess.Sample.Ux
## Introduction
This is an Angular project and it's purpose is to test interactions with the [DataAccess.Sample.Web](https://github.com/Ian-Webster/sandbox/tree/main/nuget-samples/DataAccess.Sample/DataAccess.Sample.Web) project from a frontend perspective.

## Routes
* /movie loads movies via GraphQL

## GraphQL setup
This project leverages the [GraphQL Code Generator](https://the-guild.dev/graphql/codegen) library (GitHub link [here](https://github.com/dotansimha/graphql-code-generator)) to create typings for the various GraphQL plumbing required to interact with the API.

The following are the setup steps that were taken to set up the library (instructions were mostly taken from [here](https://the-guild.dev/graphql/codegen/docs/guides/angular) but mixed in with some troubleshooting steps also);
1. Installed the various NPM packages required for the library into the project;
    ```
    npm i graphql
    npm i -D typescript
    npm i -D @graphql-codegen/cli
    npm i -D @graphql-codegen/typescript
    npm i -D @graphql-codegen/typescript-operations
    npm i -D @graphql-codegen/typescript-apollo-angular
    ```
2. Created a typescript file named "codegen.ts" in the root of the project;
    ```typescript
    import type { CodegenConfig } from '@graphql-codegen/cli'
 
    const config: CodegenConfig = {
      schema: 'https://localhost:7128/graphql',
      generates: {
        './graphql/generated.ts': {
          plugins: ['typescript', 'typescript-operations', 'typescript-apollo-angular']
        }
      }
    }
    export default config
    ```
3. Modified the package.json scripts block to include a new command named generate;
    ```json
      "scripts": {
        "ng": "ng",
        "start": "ng serve",
        "build": "ng build",
        "watch": "ng build --watch --configuration development",
        "test": "ng test",
        "generate": "set NODE_TLS_REJECT_UNAUTHORIZED=0&& graphql-codegen" <-- new value
      }
    ```
    Note: we have to set the `NODE_TLS_REJECT_UNAUTHORIZED` environment setting to false to fix an issue introduced in [this PR](https://github.com/dotansimha/graphql-code-generator/issues/1806) that changed the default behavior of the library to reject self-signed SSL certs (see this [comment](https://github.com/dotansimha/graphql-code-generator/issues/1785#issuecomment-493976501) for details) and because localhost .net certs are self certifcated the library will refused to communicate without the setting being set to false
4. Run the [DataAccess.Sample.Web](https://github.com/Ian-Webster/sandbox/tree/main/nuget-samples/DataAccess.Sample/DataAccess.Sample.Web)
5. Run the new npm command inserted in step 3 with the command `npm run generate`