$artifactsDir = 'artifacts'
git clean -fxd $artifactsDir

dotnet publish --configuration Release --output $artifactsDir