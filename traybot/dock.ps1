az login
az acr login --name traybotreg.azurecr.io
docker build -t tray-bot -f traybot\Dockerfile .
docker tag traybot traybotreg.azurecr.io/traybot:v1
az container create --resource-group tray-bot-reg --name traybot --image traybotreg.azurecr.io/traybot:v1 --cpu 1 --memory 1 --registry-login-server traybotreg.azurecr.io --registry-username traybotreg --registry-password "" --ip-address Public --dns-name-label traybot1 --ports 80 --environment-variables
 Discord_Bot_Token=