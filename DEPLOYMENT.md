# Deployment Guide

This project is containerized using Docker, making it easy to deploy on any platform that supports containers.

## Prerequisites
- [Docker](https://www.docker.com/) (for local containerization)
- A GitHub account (for automated deployment)

## Option 1: Render (Free/Easy)
1. Push your code to a GitHub repository.
2. Log in to [Render](https://render.com/).
3. Click **New +** > **Web Service**.
4. Connect your GitHub repository.
5. Render will automatically detect the `Dockerfile`.
6. Set the following environment variable in the Render dashboard:
   - `ASPNETCORE_ENVIRONMENT`: `Production`
7. **Important**: SQLite is a file-based database. On Render, files are not persistent by default. To persist data, you must add a **Disk** in the Render settings and mount it to `/app/data`.

## Option 2: Railway
1. Push your code to GitHub.
2. Log in to [Railway](https://railway.app/).
3. Click **New Project** > **Deploy from GitHub repo**.
4. Railway will build the Docker image and deploy it.
5. Add a volume and mount it to `/app/data` to persist your SQLite database.

## Option 3: Azure App Service (Professional)
1. Create an **App Service** in the Azure Portal.
2. Select **Docker Container** as the publish method.
3. Configure a CI/CD pipeline using **GitHub Actions**.
4. Use **Azure Storage** to mount a file share to `/app/data` for persistent SQLite storage.

## Option 4: Local Docker Run
To run the container locally:
```bash
docker build -t finance-backend .
docker run -d -p 8080:8080 -v finance_data:/app/data finance-backend
```
The API will be available at `http://localhost:8080`.
