\ convert km/h to m/s

: kmh	( f -- f ) 
	cr ." kmh ? " pad dup 20 accept >float \ user input
	5e0 18e0 f/ f* 		 \ convert in meter/seconds
	CR f. cr		 \ display result
;

: example 3 0 do kmh loop cr 0 (bye) ; 

example

\ 12.3e0 kmh

\ 3.41666666666667 
\  ok
\ bye

