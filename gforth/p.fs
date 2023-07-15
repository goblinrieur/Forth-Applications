#!  /usr/bin/env gforth
: >pyt. ( -- x y ) 
	2 0 do ." value : " pad dup 20 accept >float fdup f* cr loop 
	f+ fsqrt ( sqrt(x²+y² ) )
	cr 5 set-precision ." result sqrt(x2+y2) : " f. cr cr ;
CR
>pyt.
CR CR 
bye
