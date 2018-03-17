# UnmanagedContainer

This repository contains a C++ library project, with a simple 'add' function for adding 2 numbers. This C++ library is used by a .Net Core ASPNET web API through platform invoke (DllImport). 

In the web API project there is also a dockerfile. This dockerfile creates a container with the wabapi. 


This is temporary:

docker build -t aspnet-site .
docker run -d -p 8000:80 --name aspnet-site aspnet-site
docker inspect --format="{{.NetworkSettings.Networks.nat.IPAddress}}" aspnet-site


az group create --name cont_rg --location "West Europe"
az acr create --resource-group cont_rg --name gitteRegistry --sku Basic --admin-enabled true
az acr login --name gitteRegistry
az acr list --resource-group cont_rg --query "[].{acrLoginServer:loginServer}" --output table
docker tag aspnet-site gitteregistry.azurecr.io/aspnet-site:v1
docker push gitteregistry.azurecr.io/aspnet-site:v1

#!/bin/bash

# Modify for your environment. The ACR_NAME is the name of your Azure Container
# Registry, and the SERVICE_PRINCIPAL_NAME can be any unique name within your
# subscription (you can use the default below).
$ACR_NAME="gitteregistry"
$SERVICE_PRINCIPAL_NAME="acr-service-principal-gitte"

# Obtain the full registry ID for subsequent command args
$ACR_REGISTRY_ID=$(az acr show --name $ACR_NAME --query id --output tsv)

# Create the service principal with rights scoped to the registry.
# Default permissions are for docker pull access. Modify the '--role'
# argument value as desired:
# reader:      pull only
# contributor: push and pull
# owner:       push, pull, and assign roles
$SP_PASSWD=$(az ad sp create-for-rbac --name $SERVICE_PRINCIPAL_NAME --scopes $ACR_REGISTRY_ID --role reader --query password --output tsv)
$SP_APP_ID=$(az ad sp show --id http://$SERVICE_PRINCIPAL_NAME --query appId --output tsv)

# Output the service principal's credentials; use these in your services and
# applications to authenticate to the container registry.
echo "Service principal ID: $SP_APP_ID"
echo "Service principal password: $SP_PASSWD"

az appservice plan create --name containerasp --resource-group cont_rg --sku S1 --is-linux
az webapp create --resource-group cont_rg --plan containerasp --name gittecontainerapp4 --deployment-container-image-name gittetitter/aspnet-site:v1

