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

: add-one  1 #clicks +! ;
: update-wish   wish{ .\" .label configure -text \"clicks: " #clicks @ . .\" \"" cr }wish ;
: cleaning
	tk-in close-file
	tk-out close-file
	s" rm tk-in tk-out" system 
	bye
;

: counting
begin
	tk-out key-file
	dup '+' = if add-one update-wish then	\ add one if '+' received
	#clicks @ 5 > if
		wish{ ." exit" }wish
		cleaning
	then
4 = until 
;			


: initiating
	s" mkfifo tk-in tk-out" system
	s" wish <tk-in >tk-out &" system
	s" tk-in" w/o open-file throw to tk-in
	s" tk-out" r/o open-file throw to tk-out
	wish{ 	
		.\" wm title . \"forth count\"" cr
		.\" wm geometry . \"200x100+100+100\"" cr
		.\" label  .label -text \"There have been no clicks yet\" " cr
		.\" button .click -text \"click me\" -command \" puts '+' \" " cr 
		.\" button .exit -text \"EXIT\" -command exit " cr 
		." pack .label .click .exit" cr 
	}wish
;

: checkrunispossible 
	s" which wish 1> /dev/null" system  \ system bash exexute
	$? 0 > if
		cr 
		27 emit .\" [31;1m TCL-TK wish command must be installed." 
		27 emit .\" [0m"
		cr
		bye
	then
;
		
checkrunispossible initiating counting cleaning
