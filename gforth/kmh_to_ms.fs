\ convert km/h to m/s

: kmh	( f -- f ) 
	cr ." kmh ? "
	pad dup 20 accept >float 
	5e0 18e0 f/ f* 
	CR f. cr
;

\ 12.3e0 kmh

\ 3.41666666666667 
\  ok
\ bye

