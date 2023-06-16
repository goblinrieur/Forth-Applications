#! /usr/local/bin/gforth-fast-fast
create (arabic)
	1000 128 * char M + ,
	500 128 * char D + ,
	100 128 * char C + ,
	50 128 * char L + ,
	10 128 * char X + ,
	5 128 * char V + ,
	1 128 * char I + ,
does>
	7 cells bounds do
		i @ 2dup 
		127 and = if 
			nip 7 rshift leave 
		else 
			drop 
		then
		1 cells 
	+loop dup
;
\ here we have Arabic conversion table
 
: >arabic
	0 dup >r >r
	begin
		2dup
		while
		c@ dup (arabic) 
		rot <> while
			r> over r> over 
			over > if 
				2* negate + 
			else 
				drop 
			then 
			+ swap >r >r 1 /string
	repeat then drop 2drop r> r> drop
	. \ immediate display of result 
;
\ original >arabic function comes from Rosetta code site.
\ here starts my own work : 
 
create romans 73 c, 86 c, 88 c, 76 c, 67 c, 68 c, 77 c,
\ here we go with a table of Romans numbers chars

variable column# ( current-offset )
\ check char by char where to go 

( helpers )
: ones
	0 column# !
;

: tens
	2 column# !
;

: hundreds
	4 column# !
;

: thousands
	6 column# !
;

: column ( -- column-adr ) \ selection 
	romans column# @ +
;

: .symbol ( offset -- ) \ prints char
	column + c@ emit 
;

: oner
	0 .symbol
;

: fiver
	1 .symbol
;

: tener
	2 .symbol
;

: oners ( #of-ones -- ) \ select how many ones to print
	?dup if
		0 do
			oner
		loop
	then
;

: almost ( quotient-of-5/ -- ) \ select five or tens 
	oner if
		tener
	else
		fiver
	then
;

: digit ( digit -- ) 
	5 /mod over 
	4 = if 
		almost 
		drop
	else
		if
			fiver
		then
		oners
	then
;

: >roman ( number -- ) 
	1000 /mod	thousands 	digit
	100 /mod 	hundreds 	digit
	10 /mod 	tens		digit
			ones		digit
;

: bootmessage	\ help to user
	page
	cr
	s" convert Arabic > roman numbers : 	14 becomes XIV" type cr
	s" convert roman > Arabic numbers :	XIV becomes 14" type cr 
;

: isnum? 
	." next number ? "
	pad dup 6 accept 2dup 	\ ask for an user input
	s>number?  IF 		\ already an integer ? convert to roman
		swap >roman 
	ELSE  			\ we can suppose it is a string so try to convert it to Arabic  
		2drop >arabic
	THEN 
; 

: again? ( [char] q -- )
		cr ." again ? "	\ user might quit in many ways even ctrl-C
		key case
			[char] q of cr cr bye endof
			[char] Q of cr cr bye endof
			[char] n of cr cr bye endof
			[char] N of cr cr bye endof
		endcase
;

: main ( -- ) 
	begin
		bootmessage
		isnum? 
		again?		\ loop or not as a user choice.
	again
;

main
