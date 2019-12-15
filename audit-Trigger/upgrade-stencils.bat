sqlcompact -d "Data Source=%1"  -q "DROP TABLE [__MigrationHistory]"

for /f "tokens=*" %a in ('dir *.codestencil /s') do "alter-schema-details.bat" %a

pause