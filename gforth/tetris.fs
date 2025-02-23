\ tt.pfe	Tetris for terminals, redone in ANSI-Forth.
\		Written 05Apr94 by Dirk Uwe Zoller,
\			e-mail duz@roxi.rz.fht-mannheim.de.
\		Look&feel stolen from Mike Taylor's "TETRIS FOR TERMINALS"
\		Please copy and share this program, modify it for your system
\		and improve it as you like. But don't remove this notice.
\		Thank you.
\ As original author asks it to be kept free it is OK for my own freedom mind :) 
\ current version questions ask goblinrieur@gmail.com
 
only forth also definitions
vocabulary tetris  tetris also definitions
 
decimal
27 CONSTANT ESC 
-28 CONSTANT ctrlC	\ trap ctrl+C
: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ; \ ASCII TERMINAL ONLY 

\ Variables, constants
bl bl 2constant empty		\ an empty position
variable wiping			\ if true: wipe brick, else draw brick
2 constant col0			\ position of the pit
0 constant row0
10 constant wide		\ size of pit in brick positions
25 constant deep		\ 25 is a good choice for a hard scoring game (long and challenging enough) 
char J value left-key		\ customize if you don't like them
char K value rot-key
char L value right-key
bl value drop-key
char P value pause-key
12 value refresh-key
char Q value quit-key
variable score			\ need to add high-score savings 
variable pieces
variable levels
variable delay
variable brow			\ where the brick is
variable bcol
 
: miplace  over >r rot over 1+ r> move c! ; \ just replace deprecated place word
\ stupid random number generator
variable seed
: randomize	time&date + + + + + seed ! ;

\ cursor management needed in two vocabulary's
: curseur ( true|anything-else -- display cursor | or not )
	0 = if .\" \e[?25l" else .\" \e[?25h" then 
;
 
1 cells 4 = [IF]
$10450405 Constant generator
: rnd  ( -- n )  seed @ generator um* drop 1+ dup seed ! ;
: random ( n -- 0..n-1 )  rnd um* nip ;
[ELSE]
: random	\ max --- n ; return random number < max
		seed @ 13 * [ hex ] 07FFF [ decimal ] and
		dup seed !  swap mod ;
[THEN]

\ Prepare saving score file
4 constant max-line
0 value fid1
create line-buffer max-line 2 + allot
create fname 8 allot s" .score" fname miplace					\ nice way to assign a file name 
variable #src-fd-in
variable 'src-fd-in
variable fd-in
variable tobeat
-1 tobeat !

: readfile
	here 'src-fd-in ! 							\ ram position
	s" .score" r/o open-file throw fd-in !
	here 16 fd-in @ read-file throw 
	dup allot								\ one alloc = 1 line
	fd-in @ close-file throw						\ now close file
	here 'src-fd-in @ - #src-fd-in ! 					\ get allocated
	'src-fd-in @ #src-fd-in @ s>number drop tobeat ! 
;

: displayscoretobeat
	tobeat @ 0< if 
		readfile							\ read it from file only once
	else
		32 colorize
		." Score to beat "  
		tobeat @ . 		 					\ display it  
		0 colorize
	then
;

: highscore? ( finalscore > fd-in -- file )
	."       The score to beat  " tobeat @ . cr
	tobeat @  score @  < if
		33 colorize
		0 30 at-xy ."     ***** NEW HIGH SCORE *****" 			\ save score if it is a new high score
		fname count file-status nip if i				\ fileexists ?
			fname count r/w create-file throw
		else
			fname count r/w open-file throw
		then to fid1 							\ do not forget the file ID
		score @ s>d <# #s #> 						\ format score as a string
		fid1 write-line throw						\ write it on file 
		fid1 close-file throw						\ make real save of file 
		0 colorize
	then
;
 
\ Access pairs of characters in memory:
: 2c@		dup 1+ c@ swap c@ ;
: 2c!		dup >r c! r> 1+ c! ;
: d<>		d= 0= ;
 
\ Drawing primitives:
 
: 2emit		emit emit ;
: position	\ row col --- ; cursor to the position in the pit
		2* col0 + swap row0 + at-xy ;
 
: stone		\ c1 c2 --- ; draw or undraw these two characters
		wiping @ if  2drop 2 spaces  else  2emit  then ;
 
\ Define the pit where bricks fall into:
 
: def-pit	create	wide deep * 2* allot
		does>	rot wide * rot + 2* + ;
 
def-pit pit
 
: empty-pit	deep 0 do wide 0 do  empty j i pit 2c!
		loop loop ;
 
\ Displaying:
: draw-bottom	\ --- ; redraw the bottom of the pit
		deep -1 position
		[char] + dup stone
		wide 0 do  [char] = dup stone  loop
		[char] + dup stone ;
 
: draw-frame	\ --- ; draw the border of the pit
		deep 0 do
		    31 colorize
		    i -1   position [char] | dup stone
		    i wide position [char] | dup stone
		    0 colorize
		loop  draw-bottom ;
 
: bottom-msg	\ addr cnt --- ; output a message in the bottom of the pit
		deep over 2/ wide swap - 2/ position type ;
 
: draw-line	\ line ---
		dup 0 position  wide 0 do 
			dup i pit 2c@ 2emit 
		loop  drop 
;
 
: draw-pit	\ --- ; draw the contents of the pit
		deep 0 do
			i draw-line 
		loop
;
 
: show-key	\ char --- ; visualization of that character
		dup bl <
		if  [char] @ or  [char] ^ emit  emit  space
		else  [char] ` emit  emit  [char] ' emit
		then ;
 
: show-help	\ --- ; display some explanations
		32 colorize
		30  1 at-xy ." ***** T E T R I S *****"
		30  2 at-xy ." ======================="
		30  4 at-xy ." Use keys: (or arrows)"
		32  5 at-xy left-key	show-key ."  Move left"
		32  6 at-xy rot-key	show-key ."  Rotate"
		32  7 at-xy right-key	show-key ."  Move right"
		32  8 at-xy drop-key	show-key ."  Drop"
		32  9 at-xy pause-key	show-key ."  Pause"
		32 10 at-xy refresh-key	show-key ."  Refresh"
		32 11 at-xy quit-key	show-key ."  Quit"
		32 13 at-xy ." -> "
		30 17 at-xy ." Score:"
		30 18 at-xy ." Pieces:"
		30 19 at-xy ." Levels:"
		30 20 at-xy displayscoretobeat
		0 27 at-xy ."  ==== This program was written 1994 in pure   ANS Forth by Dirk Uwe Zoller ===="
		0 28 at-xy ."  =================== Copy it, port it, play it, enjoy it! =====================" 
		0 29 at-xy ."  =========== Forked on 2020 09 19 for fun purpose = goblinrieur@gmail.com =====" 
		0 30 at-xy ."  =========== 2023 08 21 last changes ============== goblinrieur@gmail.com =====" 
		0 colorize 
;
 
: update-score	\ --- ; display current score
		38 17 at-xy score @ 3 .r
		38 18 at-xy pieces @ 3 .r
		38 19 at-xy levels @ 3 .r ;
 
: refresh	\ --- ; redraw everything on screen
		page draw-frame draw-pit show-help update-score ;
 
\ Define shapes of bricks:
 
: def-brick	create	4 0 do
			    ' execute  0 do  dup i chars + c@ c,  loop drop
			    refill drop
			loop
		does>	rot 4 * rot + 2* + ;
 
def-brick brick1	s"         "
			s" ######  "
			s"   ##    "
			s"         "
 
def-brick brick2	s"         "
			s" <><><><>"
			s"         "
			s"         "
 
def-brick brick3	s"         "
			s"   {}{}{}"
			s"   {}    "
			s"         "
 
def-brick brick4	s"         "
			s" ()()()  "
			s"     ()  "
			s"         "
 
def-brick brick5	s"         "
			s"   [][]  "
			s"   [][]  "
			s"         "
 
def-brick brick6	s"         "
			s" @@@@    "
			s"   @@@@  "
			s"         "
 
def-brick brick7	s"         "
			s"   %%%%  "
			s" %%%%    "
			s"         "
 
\ this brick is actually in use:
 
def-brick brick		s"         "
			s"         "
			s"         "
			s"         "
 
def-brick scratch	s"         "
			s"         "
			s"         "
			s"         "
 
create bricks	' brick1 ,  ' brick2 ,  ' brick3 ,  ' brick4 ,
		' brick5 ,  ' brick6 ,  ' brick7 ,
 
create brick-val 1 c, 2 c, 3 c, 3 c, 4 c, 5 c, 5 c,
 
: is-brick	\ brick --- ; activate a shape of brick
		>body ['] brick >body 32 cmove ;
 
: new-brick	\ --- ; select a new brick by random, count it
		1 pieces +!  7 random
		bricks over cells + @ is-brick
		brick-val swap chars + c@ score +! 
;
 
: rotleft	4 0 do 4 0 do
		    j i brick 2c@  3 i - j scratch 2c!
		loop loop
		['] scratch is-brick ;
 
: rotright	4 0 do 4 0 do
		    j i brick 2c@  i 3 j - scratch 2c!
		loop loop
		['] scratch is-brick ;
 
: draw-brick	\ row col ---
		4 0 do 4 0 do
		    j i brick 2c@  empty d<>
		    if  over j + over i +  position
			j i brick 2c@  stone
		    then
		loop loop  2drop ;
 
: show-brick	\ ensure brick is yellowed while dropping ( will be painted white on row elimination & stacked states )
		33 COLORIZE wiping off draw-brick 0 COLORIZE 
;	
: hide-brick	wiping on  draw-brick ;
 
: put-brick	\ row col --- ; put the brick into the pit
		4 0 do 4 0 do
		    j i brick 2c@  empty
			d<> if  over j +  over i +  pit
					j i brick 2c@  rot 2c!
		    then
		loop loop  2drop ;
 
: remove-brick	\ row col --- ; remove the brick from that position
		4 0 do 4 0 do
		    j i brick 2c@  empty
			d<> if  over j + over i + pit empty rot 2c!  then
		loop loop  2drop ;
 
: test-brick	\ row col --- flag ; could the brick be there?
		4 0 do 4 0 do
		    j i brick 2c@ empty d<>
		    if  over j +  over i +
			over dup 0< swap deep >= or		\ 1st condition
			over dup 0< swap wide >= or		\ 2cd condition
			2swap pit 2c@  empty d<> 		\ or 3rd condition 
			or or if  unloop unloop 2drop false  exit  then
		    then
		loop loop  2drop true ;
 
: move-brick	\ rows cols --- flag ; try to move the brick
		brow @ bcol @ remove-brick swap brow @ + swap bcol @ + 2dup test-brick
		if  brow @ bcol @ hide-brick
		    2dup bcol ! brow !  2dup show-brick put-brick  true
		else  2drop brow @ bcol @ put-brick  false
		then ;
 
: rotate-brick	\ flag --- flag ; left/right, success
		brow @ bcol @ remove-brick
		dup if  rotright  else  rotleft  then
		brow @ bcol @ test-brick
		over if  rotleft  else  rotright  then
		if  brow @ bcol @ hide-brick
		    if  rotright  else  rotleft  then
		    brow @ bcol @ put-brick brow @ bcol @ show-brick  true
		else  drop false  then ;
 
: insert-brick	\ row col --- flag ; introduce a new brick
		2dup test-brick
		if  2dup bcol ! brow !
		    2dup put-brick  draw-brick  true
		else  false  then ;
 
: drop-brick	\ --- ; move brick down fast
		begin  1 0 move-brick 0=  until ;
 
: move-line	\ from to ---
		over 0 pit  over 0 pit  wide 2*  cmove  draw-line
		dup 0 pit  wide 2*  blank  draw-line ;
 
: line-full	\ line-no --- flag
		true  wide 0
		do  over i pit 2c@ empty 
			d= if  drop false  leave  then
		loop nip ;
 
: remove-lines	\ ---
		deep deep
		begin
		    swap
		    begin  1- dup 0< if  2drop exit  then  dup line-full
		    while  1 levels +!  10 score +!  repeat
		    swap 1- 2dup <> if  2dup move-line  then
		again ;
 
: to-upper	\ char --- char ; convert to upper case
    dup [char] a [char] z 1+ within if
			bl -
    then ;

: interaction	\ --- flag
	\ ctrl+c is a boss-key ( emergency exit ) if run from shell script
	['] ekey catch dup -28 = if drop exit then throw ekey>char if to-upper ( printable char ) 
		case
		   left-key		of  0 -1 move-brick drop  endof
		   right-key	of  0  1 move-brick drop  endof
		   rot-key		of  0 rotate-brick drop  endof
           drop-key		of  drop-brick	endof
           refresh-key	of  refresh		endof
           quit-key		of  false exit  endof
       endcase  
	else 
		ekey>fkey if ( arrow key-id ) \ to allows also that mode
			case
				k-left	of	0 -1 move-brick drop	endof
				k-right	of	0  1 move-brick drop	endof
				k-up	of	0 rotate-brick drop		endof
				k-down	of	drop-brick				endof
			endcase 
		else
			drop \ ignore other key
		then
	then true
;
 
: initialize	\ --- ; prepare for playing
		false curseur 	\ hide cursor
		randomize empty-pit refresh
		0 score !  0 pieces !  0 levels !  100 delay ! ;
 
: adjust-delay	\ --- ; make it faster with increasing score
		levels @
		dup  50 < if  100 over -  else
		dup 100 < if   62 over 4 / -  else
		dup 500 < if   31 over 16 / -  else  0  then then then
		delay !  drop ;
 

: play-game	\ --- ; play one tetris game
		begin
		    new-brick
		    -1 3 insert-brick
		while
		    begin  4 0
			do  35 13 at-xy
			    delay @ ms key?
			    if interaction 0=
				if  unloop exit  then
			    then
			loop
			1 0 move-brick  0=
		    until
		    remove-lines update-score adjust-delay highscore? 
		repeat 
;
 
forth definitions

: tt		\ --- ; play the tetris game
		page false curseur 	\ hide cursor
		31 COLORIZE page s" tetris.txt" slurp-file type key
		0 colorize initialize s"  Press any key " bottom-msg key drop draw-bottom	\ wait user is ready 
		begin
		    play-game true curseur s"  Again? " bottom-msg key to-upper [CHAR] Y = while \ any other key ends 
		   	initialize  
		repeat
		0 23 at-xy ;
 
only forth also definitions
\ cursor management redefined for that dictionary 
: curseur ( true|anything-else -- display cursor | or not )
	0 = if .\" \e[?25l" else .\" \e[?25h" then
;

tt page true dup curseur (bye) \ run & exit zero
