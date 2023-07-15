\ core game

9 constant #boxes	\ game board size
1 constant X		\ declare cross player as 1 
2 constant O		\ declare round player as 2

create action #boxes cells allot 	\ create board

\ style coding  try to describe stack of a word on word declaration 
\ try to respect 
\ write word ends with !
\ read word ends with @
\ print word start or ends with dot
\ etc...
\ try doing some short words doing small things well

\ very minimal thinking of my solution around tic tac toe solution 
\ might use only few words for non-intelligent version 

: box! ( box # --- )
	\ write a symbol into a box.
	action rot 1- cells + ! 
;

: box@ ( box --- # )
	\ reads a symbol into a box.
	action swap 1- cells + @ 
;

\ those words are about writing & reading from game board current values on boxes
\ cause it is about a 0-8 numbering but board boxes are numbered from 1 to 9 then 
\ boths action read/write words uses decrement (-1) on cells to get the right one

: align3cr ( boxes/3? then insert cr )
	\ for correct display of bord as 3x3 boxes instead of a 9 one-liner array
	\ will be called from another word 
	3 mod 0= if 
		cr
	then 
;

: .game	( display )
	#boxes 1+ 1 \ 1 to 9 
	do
		i 1- align3cr 	\ count iteration on board
		i box@ . 	\ display iteration on box
		\ display 3 cells then CR
		\ three next
		\ last three ones
	loop
;

: cleanboard ( clean board ) 
	\ fill board with zeros 
	#boxes 1+ 1 
	do
		i 0 box!
	loop
;

\ need some X and O to put on the board now

: X! X box! ; 
: O! O box! ; 

\ now make a word to convert display vs values

: .box
	dup box@ case 
		0 of dup . endof
		1 of ." X" endof
		2 of ." O" endof
	endcase 1+ 
;

: dashes ( separator )
	cr ." ----------" \ keep it aligned with board size
;

: 3chars ( vertical separator )
	cr 
	.box ."  | "	\ do not forget the both spaces on each side of the bar
	.box ."  | "	\ keep it aligned with board size
	.box
;

: test ( debug only to check other words ) 
	 7 X! 8 O! 9 X! 4 O! 5 O! 6 X! 1 X! 2 O! 3 X!
	 cr 1 3chars dashes 3chars dashes 3chars cr drop
;


