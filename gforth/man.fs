\ draws a man in stick man style from "*" chars
: star ( -- ) 
	42 emit 	\ draws one "*"
;
: .row ( n -- ) 
	cr 		\ change line
	8 0 do 
		dup 128 and if  \ n and 128 result if put star 
			star 
		else 
			space \ if not it is a space
		then  
		2 * 
	loop drop 
; 
: shape ( s -- object ) \ permit to describe a shape from a data list
	create 8 0 do 
		c, 
	loop  
	does> 
		\ creates from input the drawing shape
		dup 7 + do 
			I c@ .row -1 
		+loop cr 
;
hex					\ get hexadecimal input (easiest) 
18 18 3C 5A 99 24 24 24 shape man	\ create a man as a shape
man 					\ call its display made of '*' characters as pixels
bye
