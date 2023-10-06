\  example words around random

require random.fs
0 value length
rnd seed !

: fill-in ( -- 1st n .. 10000th n ) 10000 0 do 32 random C, loop ;
create data fill-in  \ creates data with it 
: display ( addr -- ) 
	cr begin
		dup c@ dup 5 .r over 1+ 
		c@ < while 
	1+ repeat drop
; 
: locator ( end start -- addr ) 0 -rot do drop i i c@ i 1+ c@ > if
	leave
then loop ;
: driver ( -- ) 1 to length data 10000 over + swap do
	i' i locator i - 1+ dup length > if
		to length i display
	else 
		drop
	then loop
;

\ driver 
\   18   31
\    8    9   28
\    8    9   11   16   17   21
\    2   12   13   14 ok

cr .\" \e[33m" driver cr .\" \e[0m" cr 0 (bye) 
