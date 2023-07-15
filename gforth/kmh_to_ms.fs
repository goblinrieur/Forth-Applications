\ convert km/h to m/s

: kmh	( f -- f ) 
	cr ." kmh ? " pad dup 20 accept >float \ user input
	5e0 18e0 f/ f* 		 \ convert in meter/seconds
	CR f. cr		 \ display result
;

\ 12.3e0 kmh

\ 3.41666666666667 
\  ok
\ bye

