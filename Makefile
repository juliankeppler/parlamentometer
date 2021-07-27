build:
	@dotnet build
clean:
	@dotnet clean
restore:
	@dotnet restore
start:
	@dotnet run -p src
test:
	@dotnet test tests/tests.csproj -p:CollectCoverage=true -p:CoverletOutput=TestResults/ -p:CoverletOutputFormat=lcov -p:ExcludeByFile=\"**/src/Program.cs\"