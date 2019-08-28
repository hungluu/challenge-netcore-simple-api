FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Stores/Stores.API/Stores.API.csproj", "Stores/Stores.API/"]
RUN dotnet restore "Stores/Stores.API/Stores.API.csproj"
COPY . .
WORKDIR "/src/Stores/Stores.API"
RUN dotnet build "Stores.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Stores.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Stores.API.dll"]