# Lab Identity API

[![Travis](https://img.shields.io/travis/com/thiagonishio/LabAspNetCoreIdentityWebApi?label=TRAVIS&logo=travis&style=for-the-badge)](https://travis-ci.org/github/thiagonishio/LabAspNetCoreIdentityWebApi)
[![AppVeyor](https://img.shields.io/appveyor/build/thiagonishio/labaspnetcoreidentitywebapi?label=AppVeyor&logo=AppVeyor&style=for-the-badge)](https://ci.appveyor.com/project/thiagonishio/labaspnetcoreidentitywebapi)
![GitHub](https://img.shields.io/github/license/thiagonishio/LabAspNetCoreIdentityWebApi?style=for-the-badge)

Inspirado pelos cursos do Eduardo Pires (https://desenvolvedor.io)

Aprendi sobre badges, Cake, Travis CI e Appveyor no blog do Wellington Nascimento (https://www.wellingtonjhn.com)


## Como usar:

- Necessário a última versão do Visual Studio 2019 e a última versão do .NET Core SDK.

## Tecnologias utilizadas:

- ASP.NET WebApi Core (com .NET Core 3.1)
- ASP.NET Core Identity
- Entity Framework Core 3.1
  - Rodando em SQL Server
  - Rodando InMemory para teste de integração
- Swagger para documentação da WebApi
- xUnit para testes de integração
- CAKE (C# MAKE) para build e executar testes
- Ferramenta de CI AppVeyor para rodar em ambiente Windows
- Ferramenta de CI Travis para rodar em ambiente Linux e OSX

## Para rodar:

- Para gerar o banco de dados, executar no Package Manager Console PM> update-database