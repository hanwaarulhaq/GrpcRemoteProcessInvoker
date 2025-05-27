# gRPC Remote Process Invoker

A modern replacement for .NET Remoting using gRPC in .NET 6+, featuring client-server command execution with proper logging and error handling.

## Features

- Execute commands remotely via gRPC
- Cross-platform support (Windows, Linux, macOS)
- Secure communication (TLS support)
- Detailed logging
- Example utilities included

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or later
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

## Solution Structure
GrpcRemoteProcessInvoker/

├── GrpcService/ # gRPC Server implementation

│ ├── Protos/ # Protocol Buffer definitions

│ ├── Services/ # gRPC service implementations

│ └── appsettings.json # Configuration

├── GrpcClient/ # Command-line client

├── scripts/ # Utility scripts

│ └── echoargs.bat # Argument testing script

└── GrpcRemoteProcessInvoker.sln


## Getting Started

### 1. Clone the repository

git clone https://github.com/yourusername/GrpcRemoteProcessInvoker.git
cd GrpcRemoteProcessInvoker

## Run the gRPC Server
dotnet run --project GrpcService

The server will start listening on:

HTTP: http://localhost:5000

gRPC: http://localhost:5001

## Run the Client
In a new terminal:
dotnet run --project GrpcClient

## Usage Examples
### Using the echoargs.bat Utility
The included batch file helps test argument passing:
scripts/echoargs.bat hello world "this is quoted" 123

Output:
Batch file name: echoargs.bat
Number of arguments: 4
All arguments: hello world "this is quoted" 123

Listing arguments one by one:
Argument 1: hello
Argument 2: world
Argument 3: "this is quoted"
Argument 4: 123

## Configuration
Modify GrpcService/appsettings.json for server settings:
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      },
      "Https": {
        "Url": "https://0.0.0.0:5001",
        "Certificate": {
          "Path": "certificate.pfx",
          "Password": "yourpassword"
        }
      }
    }
  }
}

## Deployment
Windows Service
New-Service -Name "GrpcService" -BinaryPathName "C:\path\to\GrpcService.exe" -StartupType Automatic
Start-Service -Name "GrpcService"

Security Considerations
Always use HTTPS in production

Restrict allowed commands via configuration

Run service with minimal privileges

Implement proper authentication

Troubleshooting

Q: Command fails with "file not found" error

A: Use full paths to executables or prefix shell commands with cmd /c or powershell -command

Q: Connection issues between client and server

A: Verify firewall rules allow traffic on the gRPC port (default: 5001)
