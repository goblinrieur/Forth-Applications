\ github.com/Veltas/demo/blob/main/script.4th
\ Copyright 2023 Christopher Leonard - MIT Licence

0 VALUE SCRIPT-FILE

: CREATE-FILE!  CREATE-FILE  ABORT" failed to open file" ;
: FILE-SIZE!    FILE-SIZE ABORT" failed to get file size" ;
: SET-POSITION!  REPOSITION-FILE  ABORT" failed to reposition file" ;
: WRITE-LINE!  WRITE-LINE  ABORT" failed to write line to file" ;
: CLOSE-FILE!  CLOSE-FILE ABORT" failed to write file" ;
: FAST-FORWARD! ( file)  DUP FILE-SIZE! ROT SET-POSITION! ;
: OPEN-OR-CREATE! ( a u - file)
	2DUP  W/O OPEN-FILE IF
		DROP  W/O CREATE-FILE!
	ELSE
		NIP NIP
	THEN ;
: APPEND-FILE! ( a u - file)  OPEN-OR-CREATE!  DUP FAST-FORWARD! ;
: REST-OF-LINE  SOURCE >IN @ /STRING ;
: SCRIPT
	PARSE-NAME  APPEND-FILE!
	DUP  REST-OF-LINE  ROT  WRITE-LINE!
	CLOSE-FILE! ;  IMMEDIATE
: SCRIPT((
	PARSE-NAME  APPEND-FILE!  TO SCRIPT-FILE
	BEGIN CR REFILL WHILE SOURCE S" ))" COMPARE WHILE
		source script-file write-line! source evaluate ." OK"
	REPEAT THEN CR REFILL DROP
	SCRIPT-FILE CLOSE-FILE! ; immediate

\ usecase 
\ script a 1 2 3 + + . 
\ will create file named "a" containing "1 2 3 + + ." code 

: empty s" ---marker--- marker ---marker---" evaluate ;
: edit s" vim file.fs" system ;			\ to be changed by the filename to edit
: run s" file.fs" included ;			\ also here
: ecr edit run ;

marker ---marker--- 		\ make a marker

