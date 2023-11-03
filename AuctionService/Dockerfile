FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

#copy all .csproj files and restore as distinct layers. Use of the same COPy command
#for every dockerfile in the project to take advantage of docker caching

COPY Carsties.sln Carsties.sln
COPY AuctionService/AuctionService.csproj AuctionService/AuctionService.csproj
COPY SearchService/SearchService.csproj SearchService/SearchService.csproj
COPY GatewayService/GatewayService.csproj GatewayService/GatewayService.csproj
COPY Constracts/Constracts.csproj Constracts/Constracts.csproj
COPY IdentityService/IdentityService.csproj IdentityService/IdentityService.csproj

#Restore package deps
RUN dotnet restore Carsties.sln

#Copy the app folders over
COPY AuctionService AuctionService
COPY Constracts Constracts

WORKDIR /app/AuctionService
RUN dotnet publish -c Release -o app/out

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AuctionService.dll" ]