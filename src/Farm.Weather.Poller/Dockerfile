ARG PROJECT_NAME="Farm.Weather.Poller"
ARG ASSEMBLY_NAME="$PROJECT_NAME"
ARG PROJECT_FILE="./src/$PROJECT_NAME/$PROJECT_NAME.csproj"
ARG MSBUILD_BUILDCONFIGURATION="Release"
ARG DOTNET_SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG DOTNET_ASPNET_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

FROM $DOTNET_ASPNET_IMAGE AS base

# Full PGO
ENV DOTNET_TieredPGO=1
ENV DOTNET_TC_QuickJitForLoops=1
ENV DOTNET_ReadyToRun=0

WORKDIR /app

FROM $DOTNET_SDK_IMAGE AS build
WORKDIR /src

# Restore.
COPY ["./src/*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p ./src/${file%.*}/ && mv $file ./src/${file%.*}/; done;
ARG PROJECT_FILE
RUN dotnet restore --locked-mode $PROJECT_FILE

# Copy files for a build.
COPY ["./src", "./src"]

# Build the project.
ARG MSBUILD_BUILDCONFIGURATION
RUN dotnet build $PROJECT_FILE --configuration $MSBUILD_BUILDCONFIGURATION --no-restore

FROM build AS publish

# Publish artifacts.
ARG MSBUILD_BUILDCONFIGURATION
RUN dotnet publish $PROJECT_FILE --configuration $MSBUILD_BUILDCONFIGURATION -o /app --no-build

FROM base AS final

COPY --from=publish /app .

ARG ASSEMBLY_NAME
ENV DOTNET_ENTRYPOINT=$ASSEMBLY_NAME.dll
ENTRYPOINT ["sh", "-c", "dotnet $DOTNET_ENTRYPOINT"]