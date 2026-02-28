
# Application de gestionnaire de favoris
![Endpoint Badge](https://img.shields.io/endpoint?url=https%3A%2F%2Fbookmark-manager-api-latest-hkyo.onrender.com%2F)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/elsadse/bookmark-manager-app/deployment.yaml)
[![codecov](https://codecov.io/gh/elsadse/bookmark-manager-app/graph/badge.svg?token=H2Q4U70MHQ)](https://codecov.io/gh/elsadse/bookmark-manager-app)
![GitHub repo size](https://img.shields.io/github/repo-size/elsadse/bookmark-manager-app)
![GitHub License](https://img.shields.io/github/license/elsadse/bookmark-manager-app)
![GitHub top language](https://img.shields.io/github/languages/top/elsadse/bookmark-manager-app)
![GitHub commit activity](https://img.shields.io/github/commit-activity/t/elsadse/bookmark-manager-app)
![GitHub License](https://img.shields.io/github/license/elsadse/bookmark-manager-app)
![GitHub Repo stars](https://img.shields.io/github/stars/elsadse/bookmark-manager-app)

Fully functional bookmark manager with creation, edit, archive, search, and filter features.

## Features

Users are able to:

- Add new bookmarks with a title, description, website URL, and tags
- View all their bookmarks
- See bookmark details, including favicon, title, URL, description, tags, view count, last visited date, and date added
- Search for bookmarks by title in the search bar
- Filter bookmarks by selecting one or multiple tags from the sidebar
- Reset tag filters to view all bookmarks again
- View archived bookmarks
- Archive bookmarks to remove them from the main view without deleting them
- Pin/unpin bookmarks to keep important ones easily accessible
- Edit existing bookmarks to update their details
- Copy bookmark URLs to the clipboard
- Visit bookmarked websites directly from the app
- Sort bookmarks by "Recently added," "Recently visited," or "Most visited"
- Toggle between light and dark color themes
- View the optimal layout for the interface depending on their device's screen size

## Database diagram

![Database diagram](images/database-diagram.png)

## Run the application locally

```bash
# Prerequisite: Docker must be installed on your machine
# Download Docker Desktop from: https://www.docker.com/products/docker-desktop/

git clone https://github.com/elsadse/bookmark-manager-app.git
cd bookmark-manager-app
docker compose up
```

After executing these commands and once the Docker containers are started, you can access the application at the following address: **http://localhost/bookmark-manager-app** and the API at **http://localhost:8080/api**.

## Technical Stack

### Backend (API)

- **Language/Runtime:** C# 14, .NET 10
- **Framework:** ASP.NET Core (MVC / Controllers)
- **Data Access:** Entity Framework Core (code-first)
- **Database:** PostgreSQL
- **Authentication:** JWT Bearer auth (token stored in **HttpOnly cookie**)
- **Validation:** FluentValidation
- **API Documentation:** OpenAPI + Scalar API Reference

### Frontend (UI)

- **Framework:** React 19 + TypeScript
- **Build tool:** Vite
- **Runtime / Package manager:** Bun
- **Routing:** React Router
- **Styling:** Tailwind CSS (v4)
- **Data fetching / caching:** TanStack Query (React Query)
- **State management:** Zustand & React Context API
- **Validation / schemas:** Zod
- **Linting:** ESLint

### Testing

- **Unit & Integration tests:** xUnit3 + Moq
- **Integration infrastructure:** AspNetCore.Mvc.Testing + FluentAssertions + Testcontainers (PostgreSQL)

### DevOps / Tooling

- **Containerization:** Docker (multi-stage images) + Docker Compose
- **CI:** GitHub Actions
- **Coverage:** Cobertura XML + Codecov
- **Deployment:** Render (API) + GitHub Pages (UI)

## Auteurs

- [@elsadse](https://www.github.com/elsadse)