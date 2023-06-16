\ use case test / ideas / and fix
\ help from reddit :; community
create buffer 1024 allot        \ for general purpose usage redefined buffer   
2variable out 			\ general purpose output  changed to 2variable ; idea from Armok628 on redit  
: commandout ( "system" -- string ) 
	r/o open-pipe throw dup buffer swap 256 swap read-file throw  swap close-pipe throw drop buffer swap out 2! 
; 				\ system capture   
\ random example to get a value  
s" psql -c 'select avg(prct)*100 from jx ;' | sed '3!d' " commandout  
: DELIMITED ( addr len char --  addr len) 
	>R 2DUP R> SCAN NIP - \ good idea from bfox9900 reddit
;
: test 				\ draw bargraph as proof of concept
	dup 			\ for tailing spaces
	." ["
	0 DO ." #" loop		\ N # chars
	100 swap - spaces	\ draw tailing spaces needed
	." ]"			\ close bar after remaining N spaces
	cr
;
page						\ start from clean screen
out 2@ char . deliMITED evaluate cr test cr	\ for number found , convert it to number & draw bargraph
bye

