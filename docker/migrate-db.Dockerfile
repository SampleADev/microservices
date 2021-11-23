FROM microsoft/dotnet:2.1.1-runtime AS runtime
ADD src/services/RdErp.MigrateDatabase/bin/Debug/netcoreapp2.1/publish /app
WORKDIR /app
ENTRYPOINT ["dotnet", "RdErp.MigrateDatabase.dll"]