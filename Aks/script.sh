#!/bin/bash
RG_NAME='rgblog'
CLUSTER_NAME='aksblog'
ACR_NAME='spertus'

# Getting credentials
az aks get-credentials -g $RG_NAME -n $CLUSTER_NAME --overwrite-existing

# Login to azure acr
az acr login --name $ACR_NAME

# Tag image
docker tag dotmimblogsite:latest spertus.azurecr.io/dotmim/dotmimblogsite
docker tag dotmimpostsapi:latest spertus.azurecr.io/dotmim/dotmimpostsapi

# Upload Images
docker push spertus.azurecr.io/dotmim/dotmimblogsite:latest
docker push spertus.azurecr.io/dotmim/dotmimpostsapi:latest
