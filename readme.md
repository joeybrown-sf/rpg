# RPG Simulator

```
dotnet run --project src/Game --launch-profile Game
```

```
dotnet test
```

```
pack build --default-process Game --builder heroku/builder:24 rpg
docker run --rm --env DOTNET_ENVIRONMENT=Development rpg
```
