FROM mcr.microsoft.com/mssql/server:2019-latest

# Elevate to root to install required packages
USER root

RUN apt-get update \
    && apt-get install unzip libunwind8 libicu66 libssl-dev -y


# Install SQLPackage for Linux and make it executable
RUN wget -progress=bar:force -q -O sqlpackage.zip https://aka.ms/sqlpackage-linux \
    && unzip -qq sqlpackage.zip -d /opt/sqlpackage \
    && chmod +x /opt/sqlpackage/sqlpackage \
    && chown -R mssql /opt/sqlpackage \
    && mkdir /tmp/db \
    && chown -R mssql /tmp/db

# Add the DACPAC to the image
COPY ./bin/Debug/netstandard2.0/Weather.Database.dacpac /tmp/db/Weather.Database.dacpac

# Configure external build arguments to allow configurability.
ARG DBNAME=WeatherDatabase
ARG PASSWORD

# Configure the required environmental variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=$PASSWORD

# Launch SQL Server, confirm startup is complete, deploy the DACPAC, then terminate SQL Server.
# See https://stackoverflow.com/a/51589787/488695
RUN ( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" \
    && /opt/sqlpackage/sqlpackage /a:Publish /tsn:localhost /tdn:${DBNAME} /tu:sa /tp:$SA_PASSWORD /TargetEncryptConnection:False /sf:/tmp/db/Weather.Database.dacpac \
    && rm -r /tmp/db \
    && pkill sqlservr \
    && rm -r /opt/sqlpackage 


# running container
# docker build . --build-arg PASSWORD="Passw0rd!!" -t mydatabase:1.0 --no-cache 
# docker run -p 1433:1433 --name sqldb -d mydatabase:1.0