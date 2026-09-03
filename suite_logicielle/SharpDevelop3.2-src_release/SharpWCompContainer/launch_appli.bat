@ECHO OFF

SET base_assembly=

:Loop
IF [%~1]==[] GOTO Continue
    IF NOT [%~1]==[-l] GOTO Error
        IF [%~2]==[] GOTO Error
			SET base_assembly=%2
			SHIFT
SHIFT
GOTO Loop

:Error
echo "Error in parameters: launch_appli.bat -l base_assembly_file.wcc"
GOTO End

:Continue

echo %base_assembly%

IF "%base_assembly%"=="" (Container.exe -n Appli -p 3000 -r "../Beans") ELSE (Container.exe -n Appli -p 3000 -r "../Beans" -l %base_assembly%)

:End