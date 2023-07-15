\ Hanoi.fs
variable counter
\ display move
: left   ." left" ;
: right  ." right" ;
: middle ." middle" ;
\ move a disk
: move-disk ( v t f n -- v t f )
	dup 0= if 
		drop exit 
	then
	1-       >R
	rot swap R@ ( t v f n-1 )  recurse
	rot swap 2dup cr ." Move " counter @ 1+ dup counter ! . ."  where disk goes from " execute ."  to " execute
	swap rot R> ( f t v n-1 )  recurse
	swap rot 
;
\ solve
: hanoi ( n -- )
	0 counter ! 1 max >R ['] right ['] middle ['] left R> move-disk drop drop drop 
;

cr ." How to use ? " cr cr ." 10 hanoi" cr cr ." do not forget to use small number" cr
cr ." because 2^N-1 moves are needed as a minimum" cr cr 
\ how to use it 
\ <INT> hanoi 
\ 10 hanoi

\ Move 4294967267  where disk goes from middle to left
\ Move 4294967268  where disk goes from right to middle
\ Move 4294967269  where disk goes from left to right
\ Move 4294967270  where disk goes from left to middle
\ Move 4294967271  where disk goes from right to middle
\ Move 4294967272  where disk goes from right to left
\ Move 4294967273  where disk goes from middle to left
\ Move 4294967274  where disk goes from middle to right
\ Move 4294967275  where disk goes from left to right
\ Move 4294967276  where disk goes from middle to left
\ Move 4294967277  where disk goes from right to middle
\ Move 4294967278  where disk goes from right to left
\ Move 4294967279  where disk goes from middle to left
\ Move 4294967280  where disk goes from right to middle
\ Move 4294967281  where disk goes from left to right
\ Move 4294967282  where disk goes from left to middle
\ Move 4294967283  where disk goes from right to middle
\ Move 4294967284  where disk goes from left to right
\ Move 4294967285  where disk goes from middle to left
\ Move 4294967286  where disk goes from middle to right
\ Move 4294967287  where disk goes from left to right
\ Move 4294967288  where disk goes from left to middle
\ Move 4294967289  where disk goes from right to middle
\ Move 4294967290  where disk goes from right to left
\ Move 4294967291  where disk goes from middle to left
\ Move 4294967292  where disk goes from right to middle
\ Move 4294967293  where disk goes from left to right
\ Move 4294967294  where disk goes from left to middle
\ Move 4294967295  where disk goes from right to middle


\ real	984m31.446s
\ user	200m13.571s
\ sys	591m9.879s
\ N^H-1 determines the minimum move number to solve a H (height) pile hanoi tower
