\
: array  create  dup , dup cells here swap 0 fill cells allot ;
: [cell] 1+ cells  + ;  \ select cell
: >[cell] [cell] f! ; 	\ put in
: [cell]> [cell] f@ ; 	\ fetch out

5 array example			\ create array
2.0e example 0 >[cell]	\ whitin some random data
3.5e example 1 >[cell]
4.7e example 2 >[cell]
\ example 
: multiple 
	3 0 do 
		example I [cell]>	\ fetch example values
	loop
	f* f* f. \ multiply them & display result
	cr
; 

cr multiple cr 0 (bye) \ run example & exit with 0 exit status

