
# Windows Setup

## Install Git
[Git setup instructions](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
[Git download page](https://git-scm.com/download/win)


## Install NVM
[NVM setup instructions](https://docs.microsoft.com/en-us/windows/dev-environment/javascript/nodejs-on-windows)

### Install version 14.18.3 of Node

```
nvm install v14.18.3
nvm use v14.18.3
nvm alias default v14.18.3
```

## Install Microsft Sql 2019
[MS Sql setup instructions](https://www.microsoft.com/en-gb/evalcenter/evaluate-sql-server-2019)

Use the default settings, make a note of the sa password, you will need this for later, you can alternatively create a separate user and use those credentials to connect, these details will need to be added to the shared secrets at the end.

### Install MS Sql Management studio
[MS Sql setup instructions](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

## Install SqlPackage
[SqlPackage setup instructions](https://docs.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver16)

In addition to this also add it to your path in the windows environment variables.

**To add a directory to the existing PATH in Windows**

1. Launch "Control Panel"
2. "System"
3. "Advanced system settings"
4. Switch to "Advanced" tab
5. "Environment variables"
6. Under "System Variables" (for all users), select "Path"
7. "Edit"
8. "New"
9. C:\Program Files\Microsoft SQL Server\160\DAC\bin
10. "Ok"

## Install .Net 7.0 sdk
[.Net 7 setup instructions](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Install gulp

[Gulp setup instructions](https://gulpjs.com/docs/en/)

```
npm install --global gulp-cli
```

## Git Clone Repo
cd to location you want to pull code down to, eg cd Documents/Source
```
git clone https://github.com/NHSDigital/nhs-practice-migration.git
cd nhs-practice-migration
git reset --hard origin/main
```

### Pull down packages for root and web app
```
npm i
```

## Config 
Save the config from below (Api Secrets) into an apisecrets.json file and fill in all of the details with the relevant connections strings and credentials, make a note of the location for this file and run the following command:
cd ..

```
type .\apisecrets.json | dotnet user-secrets set --project "GPMigratorApp/GPMigratorApp.csproj"
```

## Build the database

```
cd ..

gulp build
gulp deployGPDataDatabase
```

# Config Secrets

## Api Secrets:
```
{
  "Platform:Sql:ReadWriteConnectionString": "data source=localhost;initial catalog=GPData;user id=sa;password='<PASSWORDHERE>';TrustServerCertificate=True",
  "Platform:Sql:ReadOnlyConnectionString": "data source=localhost;initial catalog=GPData;user id=sa;password='<PASSWORDHERE>';TrustServerCertificate=True"
}
```
