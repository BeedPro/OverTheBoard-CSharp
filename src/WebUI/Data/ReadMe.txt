go to Package manager console
1. Select "OverTheBoard.Data"
2. run Add-Migration [Name-Migration] -Context SecurityDbContext
3. run Update-Database -Context SecurityDbContext



Examples
Add-Migration Initial-Security -context "SecurityDbContext"
Update-Database -context "SecurityDbContext"


[Application schema migration]
go to Package manager console
1. Select "OverTheBoard.Data"
2. run Add-Migration [Name-Migration] -Context ApplicationDbContext
3. run Update-Database -Context ApplicationDbContext

Examples
Add-Migration -context ApplicationDbContext Check-any
Update-Database -context ApplicationDbContext

