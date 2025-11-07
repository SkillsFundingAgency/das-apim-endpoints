# GQL
## Install the tools
Make sure you have the StrawberryCake graphql tools installed.

You can install it from a command line; navigate to the root of the SFA.DAS.RecruitJobs project and run the following commands:
```
dotnet new tool-manifest
dotnet tool install StrawberryShake.Tools
```

## Regenerating the schema
If you need to regenerate the client from an updated version of the GQL schema then:

* make sure the Recruit Inner API is running on your local machine
* from a command prompt go into the `SFA.DAS.RecruitJobs/GraphQL/RecruitInner` directory and run the following command:
  * ```dotnet graphql init https://localhost:7288/graphql/ -n RecruitGqlClient -p .```
* build the project

## Adding new queries
To add a new query just:
* add a new query file with the `.graphql` extension
* build the project

The StrawberryShake codegen should then make your new query available on the client via codegen.