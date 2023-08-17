\ call gforth random.fs
require random.fs
\ colorize interface
decimal
\
27 CONSTANT ESC		\ escape 
\
variable soixantequatre		\ 64bits testing
\
: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ;	\ ASCII TERMINAL ONLY 
: hidecursor   .\" \e[?25l" ; 	\ escape sequence
: showcursor   .\" \e[?25h" ; 	\ escape sequence
: cursor 0 = if .\" \e[?25l" else .\" \e[?25h" then ;	\ 0 disable cursor anyother values restores it
\ randomize (init random) 
: randomize  ( -- )		rnd time&date 4 0 do + loop * + seed ! ;	\ init random seed
\ system
: 64bits? ( -- )	cell 8 <> if  s" 64-bit system required" exception throw bye then ;	\ check 64bits if not exit 
: 64? ( -- ) 	true here soixantequatre ! soixantequatre @ cell + <> if ." 64bits" true else ." less" false then cr ;   \ same but to display it instead of exiting
: checkversion ( -- )	version-string 2 - s\" 0.7" str= 0 >= if	\ easy method to get tooling version
		cr ." you might update your gnu-forth version" cr ." prehistoric age is over" cr cr bye
	then
;
\ some maths
: intsqrt ( u -- sqrt )  s>f fsqrt f>d drop ;	\ short method to get integered  sqrt 
: stackit ( n -- n-1 ,n , n+1 ) { n } n dup 1- swap dup 1+ ;
: add3stack ( n n n -- n ) + + ;
: mul3stack ( n n n -- n ) * * ;
: pi ( 3.1415. )		355e 113e f/ ; 
: e ( 2.71828..)		25946e 9545e f/ ; 
: 2En ( n -- 2^n ) 	1 swap ?dup if 0 do 2* loop  then ;	
: sqrt(2) ( 1.4142 )	27720e 19601e f/ ; 
: squared ( n -- n * n )	dup * ; 
: cubed ( n -- n * n * n )	dup squared * ; 
: fourth ( n -- n*n*n*n )	squared squared ; 
: favg ( n...n -- f ) 	fdepth 1 - dup 0 do f+ loop 1+ s>f f/ ;
\ bases 
: DHB.  ( n -- n $n Bn )
	dup base @ >R decimal . r> base ! dup   
	base @ >R hex u. r> base !
	base @ >R 2 base ! u. r> base !  
; 
\ stack helpers
: 3dup ( ... n m l -- ... n m l n' m' l' )			2 pick 2 pick 2 pick ;
: 4dup ( ... n m l k -- ... n m l n' m' l' k' )		2over 2over ;
\ strings
: n-form ( n m - m chars formated string of N ) 	0 <# swap 0 DO # loop #> ; 
: isitlowercase ( c -- t|f )	[char] a - 26 u< ;
: forceupper ( c -- c'upper )   dup isitlowercase bl and xor ; 
\ other
: date_dd-mm-yyyy ( -- 'd - m - y' string immediate display )  time&date 3 0 do 3 roll drop loop swap 2 roll 2 0 do . ." - " loop .  ; 
\ celcius vs fahrenheit
: c ( n[°f] -- n[°c] ) 9 5 */ 32 + . ;
: f ( n[°c] -- n[°f] ) 32 - 5 9 */ . ;
\ number input
: input ( -- n )	pad 5 blank pad 5 accept >r 0. pad r> >number 2drop drop ; 
: fetch-input ( - n f ) \ check if input is a number or a string
    pad strsize accept pad swap s>number? >r d>s r>
;
\ bargraph
: bar  ( v y x --) 	2dup at-xy .\" [" 100 spaces .\" ]" swap 1 + swap at-xy 0 DO     .\" #" LOOP cr ; 	\ draws bar as [####.....   ] with V "#"'s 
: pourcentbar ( y -- ) 		0 swap at-xy .\" [" 100 spaces .\" ]" ;  	\ draws a bar limits 
: inpourcentbar ( Y P -- ) 	swap 1 swap at-xy 0 do .\" #" loop  ; 	\ draws the percentage in an already drawn bar
\ files 
: filenamecat ( s -- text )	slurp-file type cr ; 	\ cat the file given as a string (s" filename" or from variable etc..) 
\ odd or even
: say-odd 100 dup dup 11 + 3 0 do emit loop ;			\ displays odd
: say-even 110 101 118 over 4 0 do emit loop ;			\ displays even
\ exit whit return state to system
: exit_1 1 (bye) ;
: exit_0 0 (bye) ;
: y? ( char -- flag ) dup [CHAR] y = swap [CHAR] Y = or ;	\ [y|Y]es question test on Y
: 3Dmul ( x y z n -- nx ny nz ) 	dup >r * rot r@ * rot r> * rot ; 	\ multiply all of 3D coordonates by same number
: sigma ( n .. m -- n+ N+1 ...M ) 	dup 0= if exit then 0 swap 0 do I + LOOP ; 	\ sum the integers from 0 to n-1
: runner ( -- ) 0 cursor main 1 cursor cr 0 (bye) ; 	\ some sort of standard runner to be used 
