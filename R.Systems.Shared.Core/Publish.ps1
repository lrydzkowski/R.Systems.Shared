param(
    [Parameter(Mandatory)]
    [string]$version,
    [Parameter(Mandatory)]
    [string]$apiKey
)

dotnet pack -c Release;
dotnet nuget push ".\bin\Release\R.Systems.Shared.Core.${version}.nupkg" `
    --api-key $apiKey `
    --source https://api.nuget.org/v3/index.json;