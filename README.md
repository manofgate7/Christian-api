# Christian-api

dotnet-coverage collect -f xml -o coverage.xml dotnet test ChristianApi.sln 
reportgenerator -reports:coverage.xml -targetdir:.\report -assemblyfilters:+ChristianApi.dll