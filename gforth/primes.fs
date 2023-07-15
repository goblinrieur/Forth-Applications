\
: not ( ? -- t|f ) 
	0= 
; 
: between ( n1 n2 n3 -- t|f )
	1+ within
; 
: factor ( n f -- t|f)
	mod 0=
; 
: even ( n -- t|f)
	2 factor
; 
: square ( n -- n*n )
	dup *
;
: factorable ( n -- t|f)
	3 locals| f n |
	begin
		n f factor not while
			f 2 + to f	\ makes f next odd number
			n f square < not while
				\ continue if f squared <= n 
			repeat
		then
	n f factor 
;
: prime? ( n -- s )
	locals| n |
	n 1 3 between if ." prime"
			else 
				n even if ." composite"
					else
						n factorable if ." composite"
					else
						." prime"
					then
			then
	then
;
