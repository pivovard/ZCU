@echo off

SET rezim=2
SET thread_count=8
SET input="output.fat"
SET output="output.out.fat"

SET /a count=1

:loop
echo .\FAT.exe %rezim% %count% %input% %output%
.\FAT.exe %rezim% %count% %input% %output%

SET /a count+=1
if %count% leq %thread_count% goto loop