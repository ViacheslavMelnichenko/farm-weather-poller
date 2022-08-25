FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

ARG VAULT_URL
ARG VAULT_TOKEN

ENV VAULT_API_URL=$VAULT_URL
ENV VAULT_API_TOKEN=$VAULT_TOKEN

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Farm.Weather.Poller/Farm.Weather.Poller.csproj", "src/Farm.Weather.Poller/"]
RUN dotnet restore "src/Farm.Weather.Poller/Farm.Weather.Poller.csproj"
COPY . .
WORKDIR "/src/src/Farm.Weather.Poller"
RUN dotnet build "Farm.Weather.Poller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Farm.Weather.Poller.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Farm.Weather.Poller.dll"]