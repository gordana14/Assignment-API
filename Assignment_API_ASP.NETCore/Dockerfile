
FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
#COPY *.csproj ./

# Copy everything else and build
COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out
# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Assignment_API_ASP.NETCore.dll"]	