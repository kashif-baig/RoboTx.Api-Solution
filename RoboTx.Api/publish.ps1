# Change current directory to where script resides.
$script_dir = Split-Path -Path $MyInvocation.MyCommand.Path -Parent
Set-Location $script_dir

# Create deployables folder
New-Item -Path "../.build" -Name "deployables" -ItemType Directory -Force
New-Item -Path "../.build/deployables" -Name "RoboTx" -ItemType Directory -Force

# Publish Robo-Tx assemblies
dotnet publish -p:PublishProfile=linux-arm64
dotnet publish -p:PublishProfile=linux-x64
dotnet publish -p:PublishProfile=osx-x64
dotnet publish -p:PublishProfile=win-x64

# Copy VS build files to deployables folder, excluding debug files.
$exclude = @('Robo-Tx.Api.pdb')
$source = "../.build/vs-build/*"
$destination = "../.build/deployables/RoboTx"

Copy-Item -Path $source -Destination $destination -Recurse -Force -Exclude $exclude

# Copy deployable content to deployables folder.
$source = "ContentToDeploy/*"
$destination = "../.build/deployables/RoboTx"

Copy-Item -Path $source -Destination $destination -Force

# Create ZIP archive for Robo-Tx assemblies
Compress-Archive -Path $destination -Force -DestinationPath "../.build/deployables/Robo-Tx-API-Lite-Edn.zip"
