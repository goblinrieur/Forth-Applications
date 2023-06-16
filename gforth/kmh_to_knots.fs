\ convert km/h to knots & reverse

: kmh	( f -- f ) 
	cr ." kmh ? "
	pad dup 20 accept >float 
	0.539957e f*
	CR f. cr
;

: knots ( f -- f ) 
	cr ." knots ? "
	pad dup 20 accept >float 
	0.539957e f/ 
	cr f. cr
;

