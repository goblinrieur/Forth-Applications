\ needs to have tcl/tk wish installed
0 value tk-in
0 value tk-out
variable #clicks
0 #clicks !

: wish{	\ send command to wish
	tk-in to outfile-id 
;
: }wish	\ finish command to wish
	tk-in flush-file throw
	stdout to outfile-id 
; 

: add-one  1 #clicks +! ;	\ get an action as example here counting clicks 
: update-wish   wish{ .\" .label configure -text \"clicks: " #clicks @ . .\" \"" cr }wish ;	\ redraw window 
: cleaning
	tk-in close-file
	tk-out close-file
	s" rm tk-in tk-out" system \ clear temprary files for a clean code
	bye
;

: counting
begin
	tk-out key-file
	dup '+' = if add-one update-wish then	\ add one if '+' received
	#clicks @ 5 > if
		wish{ ." exit" }wish	\ exit on 5 clicks 
		cleaning
	then
4 = until 
;			


: initiating
	s" mkfifo tk-in tk-out" system		\ create temporary files here new version mignt use a memory block as file
	s" wish <tk-in >tk-out &" system	\ manage how they are used input/output to the external interpreter
	s" tk-in" w/o open-file throw to tk-in	\ only input has to be writable
	s" tk-out" r/o open-file throw to tk-out	\ so output is read only to get performances (how cares on modern PCs)
	wish{ 	
		.\" wm title . \"forth count\"" cr
		.\" wm geometry . \"200x100+100+100\"" cr
		.\" label  .label -text \"There have been no clicks yet\" " cr
		.\" button .click -text \"click me\" -command \" puts '+' \" " cr 
		.\" button .exit -text \"EXIT\" -command exit " cr 
		." pack .label .click .exit" cr 
	}wish					\ TK window management
;

: checkrunispossible 
	s" which wish 1> /dev/null" system  \ system bash exexute
	$? 0 > if				\ if TK wish tool is not installed quit
		cr 
		27 emit .\" [31;1m TCL-TK wish command must be installed." 
		27 emit .\" [0m"
		cr
		bye
	then
;
		
checkrunispossible initiating counting cleaning
