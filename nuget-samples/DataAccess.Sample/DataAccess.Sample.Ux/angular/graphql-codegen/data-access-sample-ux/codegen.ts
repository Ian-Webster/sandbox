
import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  overwrite: true,
  schema: "https://localhost:7128/graphql/",
  documents: "src/**/*.ts",
  generates: {
    "src/generated/graphql.ts": {
      plugins: ['typescript',"typescript-apollo-angular"]
    }
  }
};

export default config;
