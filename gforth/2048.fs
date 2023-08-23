( Rewrite full of the game from a very easier algo to maintain
for future evolution of code & game . I took care of :
1. the international key-map optimisation using vi/vim mapping
2. forth thinking algo
3. tried to use as many boolean checks to simplify it all )
\ use random libs instead of your on random seed
REQUIRE random.fs
decimal HERE SEED !
\ general purpose constants
1 CONSTANT up___align 2 CONSTANT down_align 3 CONSTANT left_align 4 CONSTANT rigthalign 4 CONSTANT rows# 4 CONSTANT cols#
27 CONSTANT ESC 
: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ; \ ASCII TERMINAL ONLY 
\ in Forth, you do many things on your own. This word is used to define 2D arrays
: 2D-ARRAY ( height width )
	CREATE DUP ,
		* CELLS ALLOT
	DOES> ( y x base-address )
		ROT    ( x base-address y )
		OVER @ ( x base-address y width )
		*      ( x base-address y*width )
		ROT    ( base-address y*width x )
		+ 1+ CELLS +
;
: miplace  over >r rot over 1+ r> move c! ; 	\ just replace soon deprecated place word
\ other 2D-array purpose
rows# cols# 2D-ARRAY board
\ other allocations
CREATE havemoved? CELL ALLOT
CREATE gamescore# CELL ALLOT
CREATE move# CELL ALLOT
\ Prepare saving score file
4 constant max-line
0 value fid1
create line-buffer max-line 2 + allot
create fname 8 allot s" .score" fname miplace	\ nice way to assign a file name 
variable #src-fd-in variable 'src-fd-in variable fd-in
variable tobeat variable score -1 tobeat !
: readfile
	here 'src-fd-in ! 						\ ram position
	s" .score" r/o open-file throw fd-in !
	here 16 fd-in @ read-file throw 
	dup allot								\ one alloc = 1 line
	fd-in @ close-file throw				\ now close file
	here 'src-fd-in @ - #src-fd-in ! 		\ get allocated
	'src-fd-in @ #src-fd-in @ s>number drop tobeat ! 
;
: displayscoretobeat
        readfile							\ read it from file only once
        ." Score to beat "  tobeat @ . 		\ display it  
;
: highscore? ( final-score > fd-in -- file )
	CR ."       The score to beat  " tobeat @ . CR
	tobeat @  score @  < if
		31 colorize
		CR ."     ***** NEW HIGH SCORE *****" 		\ save score if it is a new high score
		fname count file-status nip if i			\ file exists ?
			fname count r/w create-file throw
		else
			fname count r/w open-file throw
		then to fid1 							\ do not forget the file ID
		score @ s>d <# #s #> 					\ format score as a string
		fid1 write-line throw					\ write it on file 
		fid1 close-file throw					\ make real save of file 
		0 colorize
	then
;
\ for better display only 
: fivespaces ( -- ) 5 SPACES
;
: curseur ( true|anything-else -- display cursor | or not )
	0 = if .\" \e[?25l" else .\" \e[?25h" then
;
: exitprog  CR CR true dup 2dup colorize curseur (bye) \ restore cursor on exit 
;
: missunderstooddirection ( -- ) \ exit on input error
	." Unknown direction constant: " . CR exitprog
;
: boardscore# 
	1 SWAP LSHIFT 
;
: ?drawsashes ( U -- ) \ draws a line on dash chars of U length
	0 ?DO 
		[CHAR] - EMIT 
	LOOP 
;
: redrawboard ( -- ) \ draws scoring, board & board content at once
	page CR CR 32 colorize fivespaces ." Score: " gamescore# @ 0 U.R
	move#  @ ?DUP IF
		33 colorize fivespaces ."  (+" 0 U.R ." )" 32 colorize
	THEN
	CR CR fivespaces 25 ?drawsashes CR
	rows# 0 ?DO
		fivespaces ." |" cols# 0 ?DO
			J I board @ ?DUP IF
				boardscore# 4 U.R
			ELSE
				4 SPACES
			THEN
			."  |"
		LOOP
		CR fivespaces 25 ?drawsashes CR
	LOOP
;
: freecells# ( -- free-spaces ) \ until there are free cells game continues
	0 ( count )
	rows# 0 ?DO
		cols# 0 ?DO
			J I board @ 0= IF 
				1+ \ 1 more freecell
			THEN
		LOOP
	LOOP
;
: findfreecell ( index -- addr )
	0 0 board SWAP ( curr-addr index )
	0 0 board @ 0<> IF 
		1+ 
	THEN
	0 ?DO ( find the next free space index times )
		BEGIN
			CELL+ DUP @ 0=
		UNTIL
	LOOP
;
: popblock ( -- )
	freecells#
	DUP 0<= IF 
		DROP EXIT 
	THEN
	random findfreecell 10 random 0= IF 
		2 
	ELSE 
		1 
	THEN SWAP !
;
: 2board ( a-y a-x b-y b-x -- a-addr b-addr ) 
	board -ROT board SWAP 
;
: currentmerge? ( dest-addr other-addr -- can-merge? )
	@ SWAP @ ( other-val dest-val )
	DUP 0<> -ROT = AND
;
: currentmagnetism? ( dest-addr other-addr -- can-it-paste? )
	@ SWAP @ ( other-val dest-val )
	0= SWAP 0<> AND
;
: willmerge? ( dest-y dest-x other-y other-x -- )
	2board ( dest-addr other-addr )
	2DUP currentmerge? IF
		TRUE havemoved? !
		0 SWAP ! ( dest-addr )
		DUP @ 1+ DUP ( dest-addr dest-val dest-val )
		ROT ! ( dest-val )
		boardscore# DUP ( score-diff score-diff )
		move# +!  gamescore# +!
		\ move is merging
	ELSE
		2DROP
		\ no merge
	THEN
;
: magnectic? ( did-something-before operator dest-y dest-x other-y other-x -- did-something-after operator )
	2board ( ... dest-addr other-addr )
	2DUP currentmagnetism? IF
		TRUE havemoved? !
		DUP @ ( ... dest-addr other-addr other-val )
		ROT ( ... other-addr other-val dest-addr ) ! ( ... other-addr )
		0 SWAP !  NIP TRUE SWAP
	ELSE
		2DROP
	THEN
;
: checklost? ( lost-before operator dy dx oy ox -- lost-after operator )
	2board currentmerge? INVERT ( lost-before operator lost-now )
	ROT AND SWAP ( lost-after operator )
;
: nextmovement ( direction operator -- )  \ vertical XOR horizontal vector can increase/decrease by 1 at once
	CASE
	SWAP
	up___align OF rows# 1- 0 ?DO	
		cols# 0 ?DO
			J I J 1+ I 4 PICK EXECUTE
		LOOP
	LOOP ENDOF
	down_align OF 1 rows# 1- ?DO
		cols# 0 ?DO
			J I J 1- I 4 PICK EXECUTE
		LOOP
	-1 +LOOP ENDOF
	left_align OF cols# 1- 0 ?DO
		rows# 0 ?DO
			I J I J 1+ 4 PICK EXECUTE
		LOOP
	LOOP ENDOF
	rigthalign OF 1 cols# 1- ?DO
		rows# 0 ?DO
			I J I J 1- 4 PICK EXECUTE
		LOOP
	-1 +LOOP ENDOF
	missunderstooddirection \ error detection trap 
	ENDCASE DROP 			\ alignment to up|down|left|right is done
;
\ very forth like method parsing an execution token ['] 
: merge ( move -- ) 
	['] willmerge?  nextmovement 
;
: magnaticone? ( move -- success? ) 
	FALSE SWAP ['] magnectic?  nextmovement 
;
: magnetism ( move -- ) \ look for vector X/Y cells that can be used to move or merge
	BEGIN
		DUP magnaticone? INVERT
	UNTIL DROP
;
: lostmove? ( move -- lost? ) 
	TRUE SWAP ['] checklost?  nextmovement 
;
\ I know you will loose more often than winning
: gamelost? ( -- lost? )
	freecells# 0= IF
		TRUE 5 1 DO 
			I lostmove? AND 
		LOOP
		IF 
			CR CR highscore? fivespaces 31 colorize ." You lose! Press Space bar." 
			begin key 32 = until exitprog	\ let time to player realize he failed
		THEN
	THEN
;
: game_won? ( -- ) \ not so easy to win 
	rows# 0 ?DO
		cols# 0 ?DO
			J I board @ boardscore# 2048 >= IF 
				CR CR highscore? fivespaces 44 colorize ." You win! Press Space bar."
				begin key 32 = until exitprog	\ let time to player realize he made it !
			THEN
		LOOP
	LOOP
;
\ next move ? or end of game ? 
: needanext? ( move -- )
	FALSE havemoved? !  0 move# !
	DUP magnetism DUP merge magnetism
	havemoved? @ IF 
		popblock redrawboard 
	THEN game_won? gamelost?
;
\ display in game help
: displayhlp ( -- )
	page false curseur 	\ hide cursor
	44 colorize CR CR 
	s" 2048title.txt" slurp-file type CR CR \ title header
	fivespaces ." Welcome to the Gnu-forth written 2048 game " fivespaces \ align it along title width
	0 colorize CR CR 32 colorize
	fivespaces ." use vi/vim like direction definition keys (or arrows)"
	33 colorize CR CR fivespaces ."	     I" CR CR fivespaces ."	 J       L" 
	CR CR fivespaces ."	     K" 32 colorize CR CR 
	fivespaces ." to push all blocs in that direction" CR
	fivespaces ." press q/Q to quit" 
	CR CR fivespaces displayscoretobeat CR CR
	31 colorize ." press any key to start." CR key drop 0 colorize	\ get ready ?
;
\ use vim key-logic so that feet all keyboards I know 
: to-upper	\ char --- char ; convert to upper case
    dup [char] a [char] z 1+ within if
			bl -
    then 
;
: moveget? ( -- move )
	BEGIN
	\ ctrl+c is a boss-key ( emergency exit ) if run from shell script
	['] ekey catch dup -28 = if drop exit then throw ekey>char if to-upper ( printable char )
		case
			73 OF up___align TRUE ENDOF 
			74 OF left_align TRUE ENDOF
			75 OF down_align TRUE ENDOF
			76 OF rigthalign TRUE ENDOF
			81 OF gamescore# @ score ! highscore? exitprog ENDOF
			false swap
		endcase
	else	\ its not a character key
		ekey>fkey if ( arrow key-id ) \ to allow also that mode
			case
				k-left	OF	left_align TRUE	ENDOF
				k-right	OF	rigthalign TRUE	ENDOF
				k-up	OF	up___align TRUE	ENDOF
				k-down	OF	down_align TRUE	ENDOF
				false swap
			endcase
		else 
			drop \ ignore key
		then
	then
	UNTIL
;
: preload ( -- ) \ sets all cells, score & move count to zero 
	rows# 0 ?DO
		cols# 0 ?DO
			0 J I board !
		LOOP
	LOOP
	0 gamescore# !  0 move# !  popblock redrawboard
;
: main ( -- ) 
	BEGIN
		moveget?  needanext?  gamescore# @ score !  highscore? \ main
	AGAIN 
;
displayhlp preload main exitprog \ start game by default immediately
