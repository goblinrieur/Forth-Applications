\ variable name lenght dump 
 
hex
: ?ascii ( c -- printable c )
	dup 21 < if
		drop 2E
	else
		dup 7E > if ( if char outside [ 21 ; 7E ] ) 
			drop 2e 
		then
	then
;

: dump ( addr count -- )
	cr 0 do
		dup 0 4 d.r		( print addr )
		10 0 do dup i + c@ 3 .r loop 2 spaces 	( 16 bytes wide )
		10 0 do dup i + c@ ?ascii emit loop 	( ascii representation ) 
		10 + cr 10 +loop
	drop
;


