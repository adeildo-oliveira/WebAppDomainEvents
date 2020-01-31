[![Build status](https://ci.appveyor.com/api/projects/status/ucxwgqb0ypj73tt9?svg=true)](https://ci.appveyor.com/project/adeildo-oliveira/webappdomainevents)

>## WebApiDomainEvents
Este projeto tem como objetivo, exemplificar o uso do IMediator. Baseando-se nos conceitos de eventos, o domínio deste projeto possui comandos que executam suas devidas responsabilidades para comunicar com a base de dados bem como realizar as validações.

### [NuGet Mediator](https://www.nuget.org/packages/MediatR)
``Install-Package MediatR``
### [NuGet Mediator DependencyInjection](https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/)
``Install-Package MediatR.Extensions.Microsoft.DependencyInjection``

>## Network

Para facilitar a comunicação entre os container, vamos precisar criar uma rede e configura-los. Assim podemos comunicar os container através dos seus nomes.

```
docker network create --driver=bridge --subnet=172.28.0.0/16 --ip-range=172.28.5.0/24 --gateway=172.28.5.254 laboratorio
```

>## Logues MongoDB

1. Imagem mongoDB:
    ```
    docker pull tutum/mongodb
    ```
2. Container:
    ```
    docker run -d -p 17017:27017 --name mongodb mongo
    ```
3. Rede:
    ```
    docker network connect laboratorio IdContainer --ip=172.28.5.1
    ```

>## Banco de Dados
Para esse projeto, será preciso criar um banco de dados dentro do docker.

[SQL Server Docker](https://docs.microsoft.com/pt-br/sql/linux/sql-server-linux-configure-docker?view=sql-server-2017)

* Logo abaixo segue alguns comandos. As configuraçõe de connnection string devem ser alteradas para a senha que for definida nos camando abaixo, bem como a porta que desejar configurar.

1. Baixando a Imagem: 
    ``` 
    docker pull microsoft/mssql-server-linux:2017-latest
    ```
2. Definido o container e senha:
    ``` 
    docker run --name SQLServer2017 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD={SENHA}" -e "MSSQL_PID=Developer" --cap-add SYS_PTRACE -p {PORTA EXTERNA}:{PORTA INTERNA} -d microsoft/mssql-server-linux:2017-latest 
    ```
3. Rede:
    ```
    docker network connect laboratorio IdContainer --ip=172.28.5.2
    ```
### No projeto, entrando na pasta aonde está o arquivo **Dockerfile**, executar os comandos abaixo:

1. Buid da imagem:
    ```
    docker build -t api-domainevents .
    ```
2. Criação do container a partir da imagem:
    ```
    docker run -d -p 8080:80 --name api-events api-domainevents 
    ```
3. Rede:
    ```
    docker network connect laboratorio IdContainer --ip=172.28.5.3
    ```