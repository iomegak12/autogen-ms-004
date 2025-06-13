# agpy

## Overview

agpy is a Python project template designed for rapid development and deployment. It includes a robust project structure, Docker support, environment configuration, and CI/CD integration with GitHub Actions.

## Features
- MIT License
- Docker and docker-compose support
- GitHub Actions workflow for Docker image builds
- Environment variable management via `.env`
- Organized folders for logs, data, uploads, and documentation
- All dependencies managed in `requirements.txt`
- Source code in `src/`

## Project Structure
```
agpy/
├── LICENSE
├── README.md
├── .gitignore
├── .env
├── requirements.txt
├── Dockerfile
├── docker-compose.yml
├── .github/
│   └── workflows/
│       └── docker-image.yml
├── logs/
├── data/
├── uploads/
├── docs/
└── src/
    └── main.py
```

## Getting Started

1. **Clone the repository**
2. **Install dependencies**
   ```sh
   pip install -r requirements.txt
   ```
3. **Configure environment variables**
   - Edit the `.env` file as needed.
4. **Run the application**
   ```sh
   python src/main.py
   ```
5. **Run with Docker**
   ```sh
   docker-compose up --build
   ```

## Environment Variables
All configuration and secrets should be placed in the `.env` file.

## Logging, Data, and Uploads
- `logs/`: Store log files here.
- `data/`: Store data files here.
- `uploads/`: Store user-uploaded files here.

## Documentation
- Place all documentation in the `docs/` folder.

## License
This project is licensed under the MIT License.
