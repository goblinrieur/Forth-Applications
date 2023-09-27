\
\	formula Ramanujan method
\          .-----------                                                                                                                                                                                                                   
\          |   yy + xx                                                                                                                                                                                                                    
\   2pi  \ |  --------                                                                                                                                                                                                                    
\         \|      2 
\
\	 ellipse perimeter
\

fvariable X		( half lentgh ellipse )
fvariable Y		( half heitgh ellipse )

: fsquare		( n -- n*n ) fdup f* ;

: 2pi			( -- 2pi ) pi 2.0e f* ;
 
: ellipse-perim	( -- n )
		Y f@ fsquare X f@ fsquare f+	\ adds X*X+Y*Y
		2.0e f/							\ divide by 2
		fsqrt 							\ rootsquare it
		2pi f* 							\ multiply by 2pi
		cr f. cr 						\ display result
;

: calc		( -- n )
		10.0e Y f! 5.0e X f! 			\ set 10 by 5 half diameters 
		cr ellipse-perim cr 
;

\	colorize red calculate displays restore color & exit
.\" \e[31m"
calc
.\" \e[0m"
0 (bye)  

