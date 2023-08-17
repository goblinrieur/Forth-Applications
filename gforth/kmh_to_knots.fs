\ convert km/h to knots & reverse

: kmh	( f -- f ) 
	cr ." kmh ? " pad dup 20 accept >float 	\ user input
	0.539957e f*			\ convert
	CR f. cr
;

: knots ( f -- f ) 
	cr ." knots ? " pad dup 20 accept >float 	\ user input
	0.539957e f/ 			\ convert
	cr f. cr
;

: example 
	." kmh" cr
	3 0 do  kmh loop cr
	." knots" cr
	3 0 do  knots loop cr
	0 (bye)
; 

example
