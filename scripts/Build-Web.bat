echo off

:: set variable path
set projectPath="D:\GitHub-Watsons\DotNetServices"
set publishPath="D:\Folder VN\Publish\Web"
set iisSiteName="Dev"
set iisPoolName="Dev"
set issPath="D:\IIS\Web"


:: move to C:
C:
cd "C:\Windows\System32\inetsrv\"

:: stop IIS Site
appcmd.exe stop site /site.name:%iisSiteName%
timeout 2

:: stop IIS Pool
appcmd.exe stop apppool /apppool.name:%iisPoolName%
timeout 5

:: move back project path
:: D:
:: cd %projectPath%

:: publish project to folder
dotnet restore %projectPath%"\src\Watsons\Web\Web.csproj"
dotnet publish %projectPath%"\src\Watsons\Web\Web.csproj" -c Release -o %publishPath% > nul
echo "Publish OK!"

:: delete appsettings*.json in publish folder
:: cd D:\
:: cd /d %publishPath%
:: del "%publishPath%\appsettings*.json"
:: echo "Delete appsettings*.json OK!"

:: copy to IIS
:: /xo will copy only new version files & skip files same version
robocopy %publishPath% %issPath% /s /xo /xf appsettings*.json /xd "wwwroot"
echo "Copy to IIS OK!"

:: move to C:
C:
cd "C:\Windows\System32\inetsrv\"

:: start IIS Pool
appcmd.exe start apppool /apppool.name:%iisPoolName%
timeout 5

:: start IIS Site
appcmd.exe start site /site.name:%iisSiteName%
timeout 2

:: hold for view messages
pause