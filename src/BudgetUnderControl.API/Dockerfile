FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY BudgetUnderControl.API/BudgetUnderControl.API.csproj BudgetUnderControl.API/
COPY Database/BudgetUnderControl.Migrations.SqlServer/BudgetUnderControl.Migrations.SqlServer.csproj Database/BudgetUnderControl.Migrations.SqlServer/
COPY BudgetUnderControl.Common/BudgetUnderControl.Common.csproj BudgetUnderControl.Common/
COPY BudgetUnderControl.Commands/BudgetUnderControl.CommonInfrastructure.csproj BudgetUnderControl.Commands/
COPY BudgetUnderControl.Domain/BudgetUnderControl.Domain.csproj BudgetUnderControl.Domain/
COPY BudgetUnderControl.Infrastructure/BudgetUnderControl.ApiInfrastructure.csproj BudgetUnderControl.Infrastructure/
RUN dotnet restore BudgetUnderControl.API/BudgetUnderControl.API.csproj
COPY . .
WORKDIR /src/BudgetUnderControl.API
RUN dotnet build BudgetUnderControl.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BudgetUnderControl.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BudgetUnderControl.API.dll"]
