import type { CodegenConfig } from '@graphql-codegen/cli'

const config: CodegenConfig = {
  overwrite: true,
  schema: 'https://localhost:7128/graphql/',
  //documents: './src/**/*.ts',
  generates: {
    './graphql/generated.ts': {
      plugins: ['typescript', 'typescript-operations', 'typescript-apollo-angular']
    }
  },
  //ignoreNoDocuments: true,
}
export default config