IDENTITY_SECRET_NAME="Identity:Secret"
IDENTITY_SECRET_VALUE=skdmf234i34nrkn98234444lsdklfsdf

IDENTITY_PUBLIC_ORIGIN="http://localhost:4999"

DEFAULT_LOG_LEVEL=debug

Identity_IssuerUri=http://auth.rd-erp.io
Identity_PublicOrigin=http://localhost:4999
Identity_Secret=skdmf234i34nrkn98234444lsdklfsdf
Logging_LogLevel_Default=debug

env \
    $IDENTITY_SECRET_NAME="$IDENTITY_SECRET_VALUE" \
    "Identity:IssuerUri"=http://rd-erp.io \
    "Identity:PublicOrigin"="$IDENTITY_PUBLIC_ORIGIN" \
    "Logging:LogLevel:Default"=$DEFAULT_LOG_LEVEL \
    ASPNETCORE_URLS=http://+:4999 \
dotnet ./src/services/RdErp.Identity.Service/bin/Debug/netcoreapp2.1/RdErp.Identity.Service.dll

