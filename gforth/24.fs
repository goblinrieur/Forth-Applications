\ 24 game   very basic version no intelligence 
\ no real check over the usages of tiles 
5 constant strsize
27 CONSTANT ESC 	\ escape 
\
variable a			\ manage 1st tile number
variable b			\ manage 2cd tile number
variable c			\ manage 3rd tile number
variable d			\ manage 4th tile number
\
\ Pseudo-random number generator:
require random.fs
\ inputs
: COLORIZE 
	ESC EMIT ." [" 
	base @ >R 0 <# #S #> type R> base ! 
	." m"  
; \ ASCII TERMINAL ONLY 
: fetch-input ( - n f ) \ check if input is a number or a string
	pad strsize accept pad swap s>number? >r d>s r> 
;
: [19]? ( f -- true|false ) \ is input within interval [1;9]
	fdup 
	1e f>= if true else false then	\ over one ? 
	9e f<= if true else false then 	\ bellow nine ?
	and				\ both 
;
: INPUT-NUMBERS ( -- )
	\ very first version 
	cr
	." Getting four numbers : " cr
	\ protect random to get stange values or zero as value
	begin 9 random s>f a f! a f@ [19]? until
	begin 9 random s>f b f! b f@ [19]? until
	begin 9 random s>f c f! c f@ [19]? until
	begin 9 random s>f d f! d f@ [19]? until 
;
: exitprog
	 .\" \e[?25h"	\ restore cursor
	0 colorize 0 (bye)
;
: DO-OPERATIONS ( -- )
	\ apply operations manually
	clearstack
	32 colorize
	3 0 do
		cr
		." Choose the tile to put on top ? ("
		a @ if ." a" else ."  " then	\ displays inside the parenthesis only availables tiles remaining
		b @ if ." b" else ."  " then	
		c @ if ." c" else ."  " then	
		d @ if ." d" else ."  " then	
		." )" 
		key
		case	
			\ be sure there is no REUSE of the same tile
			[CHAR] a of a @ false = if cr cr ." CHEATER ! BYE " cr cr bye THEN a f@ false a ! endof 
			[CHAR] b of b @ false = if cr cr ." CHEATER ! BYE " cr cr bye THEN b f@ false b ! endof 
			[CHAR] c of c @ false = if cr cr ." CHEATER ! BYE " cr cr bye THEN c f@ false c ! endof 
			[CHAR] d of d @ false = if cr cr ." CHEATER ! BYE " cr cr bye THEN d f@ false d ! endof 
			false swap
		endcase
		cr
		33 colorize
		." Enter an arithmetic operation (+, -, *, /) or 'q' to quit" 
		key
		case
			[CHAR] + of f+ endof
			[CHAR] - of f- endof
			[CHAR] / of f/ endof
			[CHAR] * of f* endof
			[CHAR] q of exitprog endof
			[CHAR] Q of exitprog endof
			false swap
		endcase
		32 colorize
                ." . Result is currently : " fdup f. cr
	loop
; 
: f@. ( f -- ) f@ f. ; \ just a syntax facilitator
: EVALUATE-EXPRESSION ( -- ) \ check 24 is accessed
	a f@ b f@ c f@ d f@ 
	32 colorize 
	cr ." Work around : [ a=" a f@. ."  |b=" b f@. ."  |c=" c f@. ."  |d=" d f@. ."  ]" cr cr
	." Choose witch tile you are playing with ? (a-d) "
	cr
	key
	case
		[CHAR] a of a f@ false a ! endof 
		[CHAR] b of b f@ false b ! endof 
		[CHAR] c of c f@ false c ! endof 
		[CHAR] d of d f@ false d ! endof 
		false swap
	endcase
	cr
	DO-OPERATIONS
	24e f= if 
		31 colorize
		CR CR ." YOU WON ! GETTING 24 ! :) " cr cr  exitprog 
	else
		31 colorize
		CR CR ." failed to get 24 ! looser :( " cr exitprog
	then 
; 
: play-24 ( -- ) \ main
	time&date + + + * * seed !
	rnd INPUT-NUMBERS EVALUATE-EXPRESSION
;
page 
3 set-precision				\ keep easy
 .\" \e[?25l" 				\ hide cursor
31 colorize
s" ./gamedata/24.txt" slurp-file type	\ display a nice 24 at start
33 colorize
play-24					\ main call 
