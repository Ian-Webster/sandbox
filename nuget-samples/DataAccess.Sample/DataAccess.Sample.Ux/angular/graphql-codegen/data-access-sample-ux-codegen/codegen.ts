
import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  overwrite: true,
  schema: "https://localhost:7128/graphql/",
  documents: "code-gen/queries/*.graphql",
  generates: {
    "src/generated/graphql.ts": {
      plugins: ['typescript',"typescript-apollo-angular", "typescript-operations"]
    }
  }
};

export default config;
