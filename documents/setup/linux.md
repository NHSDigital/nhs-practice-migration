
# Linux Setup

Load terminal and run the following commands

```
Sudo apt-get install git
```

## Install homebrew
[Brew instructions](https://brew.sh/)

```
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"
```

❗️Restart terminal
## Uninstall Node (If you don’t already have NVM installed)

```
brew uninstall node
```

## Install NVM

```
curl https://raw.githubusercontent.com/creationix/nvm/master/install.sh |  shellenv
```
❗️Restart terminal

## Install specific version of Node - 14.18.3

```
nvm install v14.18.3
nvm use v14.18.3
nvm alias default v14.18.3
```

## Install Docker

[Docker setup instructions](https://docs.docker.com/desktop/install/linux-install/)

## Install gulp

[Gulp setup instructions](https://gulpjs.com/docs/en/)

```
npm install --global gulp-cli
```

## Install MSSQL

```
sudo docker pull mcr.microsoft.com/azure-sql-edge

sudo docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=<STRONGPASSWORDHERE>' -p 1433:1433 --name sqledge -d mcr.microsoft.com/azure-sql-edge
```

## Install Azure data studio
[Azure data studio setup instructions](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16)

## Install SQL Package
[SQL Package instructions](https://docs.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver16)

❗️Sql Package uses an old version of libssl so need to install the old one.
```
wget http://security.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.1f-1ubuntu2.16_amd64.deb 
wget http://security.ubuntu.com/ubuntu/pool/main/o/openssl/openssl_1.1.1f-1ubuntu2.16_amd64.deb 

sudo dpkg -i libssl1.1_1.1.1f-1ubuntu2.16_amd64.deb 
sudo dpkg -i openssl_1.1.1f-1ubuntu2.16_amd64.deb
```

slight tweak required to path as does not work in gulp, only terminal, when following the instructions from microsoft
```
mkdir sqlpackage
unzip ~/Downloads/sqlpackage-linux-x64-en-<Version>.zip -d ~/sqlpackage 
chmod a+x ~/sqlpackage/sqlpackage
echo 'export PATH="$HOME/sqlpackage:$PATH"' >> ~/.bash_profile
source ~/.bash_profile
sqlpackage
```

## Install .net 7
```
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

### Install the SDK
```
sudo apt-get update && \ sudo apt-get install -y dotnet-sdk-7.0
```

## Git Clone Repo
cd to location you want to pull code down to, eg cd Documents/Source
```
git clone https://github.com/NHSDigital/nhs-practice-migration.git
cd nhs-practice-migration
git reset --hard origin/main
```

## Install npm packages
```
eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"

brew install vips
```

### Install yarn
```
curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | sudo apt-key add -
echo "deb https://dl.yarnpkg.com/debian/ stable main" | sudo tee /etc/apt/sources.list.d/yarn.list
sudo apt update
sudo apt install yarn
```

### Pull down packages for root and web app
```
yarn

```

## Config 
Save the config from below (Api Secrets) into an apisecrets.json file to your user’s drive and fill in all of the details with the relevant connections strings and credentials, make a note of the location for this file and run the following command:
cd ..

```
cat ~/apisecrets.json | dotnet user-secrets set --project "GPMigratorApp/GPMigratorApp.csproj"
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

