\
: array  create  dup , dup cells here swap 0 fill cells allot ;
: [cell] 1+ cells  + ;    
: >[cell] [cell] f! ; 
: [cell]> [cell] f@ ; 

5 array example		\ create array
2.0e example 0 >[cell]	\ whitin some random data
3.5e example 1 >[cell]
4.7e example 2 >[cell]

: multiple 
	3 0 do 
		example I [cell]>
	loop
	f* f* f. 
	cr
; 

cr
multiple
cr
bye

