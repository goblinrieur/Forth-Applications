\ now a caesar cypher inspired by https://www.code4th.com/posts/ceasar-cipher-using-forth-programming/
\ default can be overwritten while calling main program with top stack integer value

variable caesar 3
decimal
: mymsg ( – len ) pad 20 erase pad 20 accept ;
: ciphershift ( char – shiftedchar ) dup 97 120 within if caesar @ + else 23 - then ;
: cipherout mymsg ." ->" 0 do pad i + c@ ciphershift emit loop ;
: reveal dup 100 123 within if caesar @ - else 23 - then ;
: showmsg mymsg ." -> " 0 do pad i + c@ reveal emit loop ;
: getswift caesar ! ; 
: main
	getswift	\ if not default will be kept
	." input : " cr
	cipherout
	cr
;
page cr mymsg main cr 0 (bye) 

