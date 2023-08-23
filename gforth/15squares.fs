\
cell 8 <> [if] s" 64-bit system required" exception throw [then]
\ who still ave a 32 bit .... over 2020... this simplify the code a lot :) 
\ This is forked from rosettacode http://www.rosettacode.org/wiki/15_Puzzle_Game#Forth at first 
\ fork is WIP since 2020-09-24
: checkversion version-string s" 0.7" search true = if \ searches for any 0.7.* string of version 
			drop drop 
		else
				cr 
				." you might update your gnu-forth version" cr
				." prehistoric age is over" cr
				cr .\" \e[?25h" 1 (bye) then
;
\ below stack comments are : 
\ "s" for a 64-bit integer representing a board state,
\ "t" a tile value (0..15, 0 is the hole),
\ "b" for a bit offset of a position within a state,
\ "m" for a masked value (4 bits selected out of a 64-bit state),
\ "w" for a weight of a current path,
\ "d" for a direction constant (0..3)
 
\ colorize interface
decimal
27 CONSTANT ESC 
: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ; \ ASCII TERMINAL ONLY 
\ compare movement 
0 constant up 1 constant left 2 constant right 3 constant down
\ defines the solution for winning conditions test
hex 123456789abcdef0 decimal constant solution
\ get constant solution to manage shuffle random play board & compare it to win avoid many useless code
\ move count
variable movecount
\ Utility
: exitprog ( -- restore cursor & exit ) 
	cr 0 colorize cr  .\" \e[?25h" 0 (bye) 
; 	
: movecount+ ( x -- x+1 )
	movecount @ 1 + movecount !
;
: .movecount ( -- s ) 
	33 colorize 5 2 at-xy ." moves count : " movecount @ . cr 
	0 colorize movecount+
;
: 3dup   2 pick 2 pick 2 pick ( s -- s s s )
;
: 4dup   2over 2over ( s -- s s s s )
;
: shift   
	dup 
	0 > if 
		lshift 
	else 
		negate rshift 
	then 
;
: row   
	2 rshift 
;
: col   
	3 and 
;
: up-valid?    ( h -- f ) 
	row 0 > 
;
: down-valid?  ( h -- f ) 
	row 3 < 
;
: left-valid?  ( h -- f ) 
	col 0 > 
;
: right-valid? ( h -- f ) 
	col 3 < 
;
\ To iterate over all possible directions, put direction-related functions into arrays:
: iterate ( u addr -- w ) 
	swap cells + @ 
;
create valid? ' up-valid? , ' left-valid? , ' right-valid? , ' down-valid? , 
	does> 
	iterate execute 
;
create step -4 , -1 , 1 , 4 , 
	does> iterate 
;
\ Advance from a single state to another:
: bits ( h -- b ) 
	15 swap - 4 * 
;
: tile ( s b -- t ) 
	rshift 15 and 
;
: new-state ( s h d -- s' ) 
	step dup >r + bits 2dup tile ( s b t ) swap lshift tuck - swap r> 4 * shift + 
;
: hole? ( s u -- f ) 
	bits tile 0= \ for external compare
;
: hole ( s -- h ) 
	16 0 do 
		dup 
		i hole? if 	\ we are at hole coordinates
			drop i unloop exit 
		then 
	loop drop 
;
\ Print the empty chars for hole
: .hole   
	 3 spaces
;
: .tile ( u -- ) 
	?dup-0=-if 
		.hole 
	else 
		dup 10 < if 
			space 
		then 
		36 colorize .  0 colorize
	then
;
\ Draw the board
: .board ( s -- ) 
	4 0 do 
		cr 
		4 0 do 
			dup j 4 * i + bits tile .tile 
		loop 
	loop drop 
;
: .help  ( just displays a help below game user interface )
	.\" \e[1m" cr cr 32 colorize 3 spaces ."  i" cr
	3 spaces ." jkl move, (arrows keys)" cr 3 spaces cr 
	3 spaces ." q quit" cr .\" \e[0m" 
;
\ Pseudo-random number generator:
create (rnd)   utime drop ,
: rnd  \ initialize random seed 
	(rnd) @ dup 13 lshift xor dup 17 rshift xor dup dup 5 lshift xor (rnd) ! 
;
: move ( s u -- s' ) 
	>r dup hole r> new-state 
;
: ?move ( s u -- s' ) 
	>r dup hole r@ valid? if 
		r> move 
	else 
		rdrop 
	then 
;
\ loop over random do get a better random value than just a random seed based on time
: shuffle ( s u -- s' ) 
	0 do 
		rnd 3 and ?move 
	loop 
;
: win ( -- won )
	cr 41 colorize ." you won!" cr 0 colorize exitprog \ bye with 0
;
: to-upper	\ char --- char ; convert any alphabetic to upper case
    dup [char] a [char] z 1+ within if
			bl -
    then 
;
: gameloop ( s -- )
	\ refresh user interface
	page cr cr dup .movecount .board .help	
	\ ctrl+c is a boss-key ( emergency exit ) if run from shell script
	['] ekey catch dup -28 = if drop exit then throw ekey>char if to-upper ( printable char )
		case
			73 OF down ?move endof
			74 OF right ?move endof
			75 OF up ?move endof
			76 OF left ?move endof
			81 OF exitprog ENDOF
			false swap
		endcase
	else	\ its not a character key
		ekey>fkey if ( arrow key-id ) \ to allow also that mode
			case
				k-left	OF	right ?move endof
				k-right	OF	left ?move endof
				k-up	OF	down ?move endof
				k-down	OF	up ?move endof
				false swap
			endcase
		else 
			drop \ ignore key
		then
	then
;
 
: main ( -- )
	begin 
	dup solution <> while  \ runs until either user quits either current board = solution constant
		gameloop 
	repeat 
	win \ won condition
;
\ main game : 
: initialyze ( -- )
		checkversion			\ guess it is useless but ... just in case...
		.\" \e[?25l"			\ hide cursor
		solution 1000 shuffle	\ random shuffle for board 
		main					\ plays until solution or user quit 
;
initialyze
