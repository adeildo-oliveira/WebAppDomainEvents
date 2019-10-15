[![Build status](https://ci.appveyor.com/api/projects/status/ucxwgqb0ypj73tt9?svg=true)](https://ci.appveyor.com/project/adeildo-oliveira/webappdomainevents)

>## WebApiDomainEvents
Este projeto tem como objetivo, exemplificar o uso do IMediator. Baseando-se nos conceitos de eventos, o domínio deste projeto possui comandos que executam suas devidas responsabilidades para comunicar com a base de dados bem como realizar as validações.

### [NuGet Mediator](https://www.nuget.org/packages/MediatR)
``Install-Package MediatR``
### [NuGet Mediator DependencyInjection](https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/)
``Install-Package MediatR.Extensions.Microsoft.DependencyInjection``

>## Banco de Dados
Para esse projeto, será preciso criar um banco de dados dentro do docker.

[SQL Server Docker](https://docs.microsoft.com/pt-br/sql/linux/sql-server-linux-configure-docker?view=sql-server-2017)

* Logo abaixo segue alguns comandos. As configuraçõe de connnection string devem ser alteradas para a senha que for definida nos camando abaixo, bem como a porta que desejar configurar.

```
docker pull microsoft/mssql-server-linux:2017-latest
```
```
docker run --name SQLServer2017 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD={SENHA}" -e "MSSQL_PID=Developer" --cap-add SYS_PTRACE -p {PORTA EXTERNA}:{PORTA INTERNA} -d microsoft/mssql-server-linux:2017-latest
```

* Como exemplo, deixei um usuário já configurado na connection string, caso queira, pode ser usado o mesmo usuário quando for realizada as devidas configurações no SQL Server.

### No projeto, entrando na pasta aonde está o arquivo **Dockerfile**, executar os comandos abaixo:

1. Buid da imagem:
    ```
    docker build -t api-domainevents .
    ```
2. Criação do container a partir da imagem:
    ```
    docker run -d -p 8080:80 --name api-events api-domainevents
    ```