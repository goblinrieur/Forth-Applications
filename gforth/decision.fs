DECIMAL
20 constant strsize
27 constant ESC
: buffer: create allot ;
10 strsize * buffer: strings \ might be enough for all use-cases
$10450405 constant generator
variable looping variable rndseed
\  string array operators
: [$]   ( ndx buffer -- addr[i] )  	swap strsize * + ; 
: [$]!  ( addr len ndx buffer -- ) 	[$] place ;
: [$]@ 	( ndx buffer -- addr len)  	[$] count ; 
: .[$]  ( ndx buffer -- )  			[$]@ type ;
\ usage
\ s" This is string #1"  1 strings [$]!   
\ 2 strings .[$]
: 2cr cr cr ; \ double vertical jump
: cursor 0 = if .\" \e[?25l" else .\" \e[?25h" then ;	\ 0 disable 1 enable cursor display
: colorize esc EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ; \ ASCII TERMINAL ONLY
: input# ( -- true | false )
	0. strsize pad swap accept pad swap dup >r >number nip nip r> <> dup 0 = if 
	    nip
	then
;
: fetch-input ( - n f ) \ check if input is a number or a string
	pad strsize accept pad swap s>number? >r d>s r> 
;
: INPUT$ ( n -- addr n ) \ input string
	PAD SWAP ACCEPT PAD SWAP
;
\ here we have words for reading numbers and texts 
: rnd ( -- n ) rndseed @ generator um* drop 1+ dup rndseed ! ;
: genrnd ( limit -- 1..limit ) begin rnd um* nip 1 + dup 10 <= until ;
: wlist? ( n of s +  s  -- s ) \ get user inputs
	32 colorize 2cr 
	."      Enter no more than ten (10) words of few characters maybe less than 20 each." cr
	."      (others will just be ignored)" 
	2cr 33 colorize
	."       How many words to randomize ? " fetch-input if 1 - looping ! then
	2cr \ might append behind here a check about that number & re- ask user input if it is over 10 :)
	looping @ 1 < if 1 looping ! then	\ prevent user error
	begin
		31 colorize
		2cr ."       Word ? " 0 colorize strsize input$ looping @ strings [$]!	
		looping @ 1 - looping ! 
	looping @ 0 < until
;
: main
	page wlist? cr
	."                      " 10 genrnd strings .[$]  \ really enough for a good random seed 
	2cr 
;
0 cursor main 1 cursor cr 0 (bye)	\ exit properly with giving 0 exit state to system
