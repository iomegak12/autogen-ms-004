# agentic-aipractices

A .NET 8.0 C# console application project with Docker support, GitHub Actions CI/CD, and xUnit test project.

## Features
- Console application using .NET 8.0
- xUnit test project for unit testing
- Dockerfile and docker-compose for containerization
- GitHub Actions workflow for Docker image build and push
- MIT License
- All configuration and secrets in `appsettings.json`

## Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

### Build and Run

```sh
dotnet build
cd agentic-aipractices
 dotnet run
```

### Run Tests

```sh
dotnet test
```

### Docker

Build and run the Docker image:

```sh
docker build -t agentic-aipractices .
docker run --rm agentic-aipractices
```

Or use docker-compose:

```sh
docker-compose up --build
```

### Configuration

Secrets and configuration are stored in `appsettings.json`:

```
{
  "AZUREOPENAI_API_KEY": "your-api-key-here"
}
```

## CI/CD

GitHub Actions workflow is provided in `.github/workflows/docker-image.yml` to build and push Docker images.

## License

This project is licensed under the MIT License.
