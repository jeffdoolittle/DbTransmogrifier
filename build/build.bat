@%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe /target:Build,Test

..\src\.nuget\nuget pack ..\src\DbTransmogrifier\DbTransmogrifier.csproj -Prop Configuration=Release
