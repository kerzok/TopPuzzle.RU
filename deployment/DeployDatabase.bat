sqlcmd  -v DatabaseName=TopPuzzle -i CreateDB.sql -f 65001
sqlcmd  -v DatabaseName=TopPuzzle -i StoredProcedures.sql -f 65001

pause