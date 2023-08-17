\
hex
: hidecursor   .\" \e[?25l" ; 	\ escape sequence
: showcursor   .\" \e[?25h" ; 	\ escape sequence
: phases ( -- ) 
	page 	\ cls
	3 0 do 
		8 1 do 
			0 0 at-xy 1f310 i + xemit \ draws current phase
			175 ms 	\ wait 250ms before next loop
		loop 
	loop 
; 
\ unicode moon phases display
hidecursor phases cr showcursor 0 (bye) 	\ exit with 0 return code
