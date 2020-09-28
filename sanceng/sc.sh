mkdir $1
cd $1
dotnet new mvc -o $1.web
dotnet new classlib -o $1.model
dotnet new classlib -o $1.dal
dotnet new classlib -o $1.bll
dotnet new sln
dotnet sln $1.sln add $1.web/$1.web.csproj
dotnet sln $1.sln add $1.model/$1.model.csproj
dotnet sln $1.sln add $1.dal/$1.dal.csproj
dotnet sln $1.sln add $1.bll/$1.bll.csproj 
dotnet add $1.web/$1.web.csproj reference $1.model/$1.model.csproj
dotnet add $1.web/$1.web.csproj reference $1.bll/$1.bll.csproj
dotnet add $1.bll/$1.bll.csproj reference $1.dal/$1.dal.csproj
dotnet add $1.bll/$1.bll.csproj reference $1.model/$1.model.csproj
dotnet add $1.dal/$1.dal.csproj reference $1.model/$1.model.csproj
dotnet restore
