[CmdletBinding()]
param (
	$azPw,
	$discordToken
)

az login
az acr login --name traybotreg.azurecr.io
docker build -t traybot -f .\traybot\Dockerfile .
docker tag traybot traybotreg.azurecr.io/traybot:latest
docker push traybotreg.azurecr.io/traybot:latest
az container delete --resource-group tray-bot-reg --name traybot 
az container create --resource-group tray-bot-reg --name traybot --image traybotreg.azurecr.io/traybot:latest --cpu 1 --memory 1 --registry-login-server traybotreg.azurecr.io --registry-username traybotreg --registry-password "$azPw" --ip-address Public --dns-name-label traybot1 --ports 80 --environment-variables Discord_Bot_Token=$discordToken