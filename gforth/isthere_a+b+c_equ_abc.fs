\ is there one or several solutions matching this condition 
\ [-100;100[	Solution?	(n-1)+n+(n+1)=(n-1)*n*(n+1) 

decimal
27 CONSTANT ESC 
variable solutions 0 

: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ; \ ASCII TERMINAL ONLY 
: stackit ( n -- n-1 ,n , n+1 ) { n } n dup 1- swap dup 1+ ;
: add3stack ( n n n -- n ) + + ;
: mul3stack ( n n n -- n ) * * ;
: run ( n -- s ) dup stackit add3stack swap dup stackit mul3stack rot 
	= if 
		cr . ." matches " cr 
		1 solutions +!
	then 
;
: main ( -- s ) 100 -100 do i run loop cr ; 
cr
31 colorize 
." is there one or several solutions matching (n-1)+n+(n+1)=(n-1)*n*(n+1) in interval [-100;100[ as integers ?" 
33 colorize 
main
32 colorize
solutions @ . ." solutions found." cr
0 colorize
cr 
bye
