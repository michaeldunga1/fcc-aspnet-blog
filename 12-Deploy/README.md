# 12-Deploy — Northline Press

```bash
docker compose up -d
cd 12-Deploy
dotnet restore
dotnet run --urls http://0.0.0.0:5080
```
