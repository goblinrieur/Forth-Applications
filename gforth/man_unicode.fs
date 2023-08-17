\ draws a man in stick man style from "*" chars
hex	\ use hexadecimal as default all along that code
: star ( -- ) 
	26aa xemit 	\ draws shape from white filled circle unicode char
;
: .row ( n -- ) 
	cr 		\ change line
	8 0 do 
		dup 80 and if  \ n and 128 result if put star 
			star 
		else 
			2 spaces	\ else use 2 spaces (unicode used are 2 char whide so 2 spaces
		then  
		2 * 
	loop drop 
; 
: shape ( s -- object ) \ permit to describe a shape from a data list
	create 8 0 do 		\ shape are 8 wide & high here
		c, 
	loop  
	does> 
		\ creates from input the drawing shape
		dup 7 + do 
			I c@ .row -1	\ char or space
		+loop cr 
;
18 18 3C 5A 99 24 24 24 shape man	\ create a man as a shape
man 					\ call its display made of '*' characters as pixels
cr 0 (bye)				\ carriage return & 0 signal exit status 
