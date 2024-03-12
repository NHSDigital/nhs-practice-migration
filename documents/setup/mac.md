
# Mac OS Setup

Load terminal and run the following commands

## Install homebrew
[Brew instructions](https://brew.sh/)
```
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```
❗️Restart terminal

## Uninstall Node (If you don’t already have NVM installed)

```
brew uninstall node
```

## Install NVM

```
curl https://raw.githubusercontent.com/creationix/nvm/master/install.sh | bash
```
❗️Restart terminal

## Install specific version of Node - 14.18.3

```
nvm install v14.18.3
nvm use v14.18.3
nvm alias default v14.18.3
```

## Install Docker

[Docker setup instructions](https://docs.docker.com/desktop/install/mac-install/)

## Install Git
[Git setup instructions](https://git-scm.com/download/mac)

```
brew install git
```

## Install gulp
[Gulp setup instructions](https://gulpjs.com/docs/en/)

```
npm install --global gulp-cli
```

## Install MSSql
[MSSql setup instructions](https://database.guide/how-to-install-sql-server-on-an-m1-mac-arm64/)

```
sudo docker pull mcr.microsoft.com/azure-sql-edge

sudo docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=<ENTERSTRONGPASSWORDHERE>' -p 1433:1433 --name sqledge -d mcr.microsoft.com/azure-sql-edge
```

## Install Azure data studio

[Azure data studio setup instructions](https://database.guide/how-to-install-azure-data-studio-on-a-mac/)

## Install SQL Package
[SQL Package instructions](https://docs.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver16)


slight tweak required to path as does not work in gulp, only terminal, when following the instructions from microsoft
```
mkdir sqlpackage
unzip ~/Downloads/sqlpackage-osx-<version string>.zip -d /usr/local/share/sqlpackage
chmod +x /usr/local/share/sqlpackage/sqlpackage
echo 'export PATH="/usr/local/share/sqlpackage:$PATH"' >> ~/.bash_profile
source ~/.bash_profile
sqlpackage
```

## Install .net 7
[.net 7 instructions](https://docs.microsoft.com/en-us/dotnet/core/install/macos)

For M1 chips select Arm64 version otherwise pick x64

## Git Clone Repo
cd to location you want to pull code down to, eg cd Documents/Source
```
git clone https://github.com/NHSDigital/nhs-practice-migration.git
cd nhs-practice-migration
git reset --hard origin/main
```

## Ensure python is in the env path
```
brew install pyenv

pyenv install 3.10.3

pyenv global 3.10.3

echo "export PATH=\"\${HOME}/.pyenv/shims:\${PATH}\"" >> ~/.bash_profile
```
### open a new terminal window and confirm your pyenv version is mapped to python
```
which python

python --version
```

## M1 Chip issues
Some issues were found running on m1 chip, following the guidance [here](https://github.com/nuxt/image/issues/204) to fix them 

### Install gcc
The "libvps" depends on gcc, so do:
```
brew install --build-from-source gcc
```
Install XCode Build Tools CLI
Also required by "libvps"
```
xcode-select --install
```

### Install "vips"
```
brew install vips
```


## Pull down packages
```
npm i

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

### Tip:

 - Allow keychain popups
 - Make sure your AirPlay receiver [System Preferences -> Sharing] is ticked off to listen port 5000

# Config Secrets

## Api Secrets:
```
{
  "Platform:Sql:ReadWriteConnectionString": "data source=localhost;initial catalog=GPData;user id=sa;password='<PASSWORDHERE>';TrustServerCertificate=True",
  "Platform:Sql:ReadOnlyConnectionString": "data source=localhost;initial catalog=GPData;user id=sa;password='<PASSWORDHERE>';TrustServerCertificate=True"
}
```

