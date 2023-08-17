\ pocket submarine fight
\
\ not a real project will be kept quick & dirty 
\ fork it in LICENSE limits
\
\ stack notes
\ n numeric
\ s string
\ f file
\ t text
\ 
DECIMAL
\ Gnu Forth
warnings off
require random.fs
\ constants
27 constant ESC 
468743574 constant seedref
\ variables
variable suboneA
variable subonex
variable suboney
variable subonez
variable subtwoA
variable subtwox
variable subtwoy
variable subtwoz
\ player variables
variable playerfood
variable playerfuel
variable playergrenades
variable playertorpedos
variable playerx
variable playery
variable playerz
\ game variables
variable movecount
variable rndseed
\ colorize interface
: COLORIZE ( n -- ) \ just like terminal ascii \e[31m, here use 31 colorize 
	ESC EMIT ." [" base @ >R 
	0 <# #S #> type 
	R> base ! ." m"  
; 
\ display intro & help
: cursor 0 = if .\" \e[?25l" else .\" \e[?25h" then ;
: intro ( f -- t ) 
	33 colorize
	cr s" ./gamedata/intro.txt" slurp-file type
	key
	page
	0 5 at-xy
	96 colorize
	s" ./gamedata/help.txt" slurp-file type
	0 colorize 
	key
;
\ display outro on exit
: outro ( f -- t ) 
	page
	33 colorize
	cr s" ./gamedata/outro.txt" slurp-file type cr
;
\ exit game properly
: exitgame ( -- )
	cr cr 
	outro
	cr cr 
	96 colorize 
	." Played for : " movecount @ dup 1 > if 
		. ." actions."
	else
		. ." action."
	then
	cr cr 0 colorize 1 cursor 0 (bye)
; 
\ define initial values for submarine 1
: initsubone ( -- ) 
	100 random 100 random 100 random 100 random 	\ get three random distinct numbers
	subonex ! suboney ! subonez ! drop
	1 suboneA ! \ still alive 
;
\ define initial values for submarine 2
: initsubtwo ( -- ) 
	100 random 100 random 100 random 100 random 	\ get three random distinct numbers
	subtwox ! subtwoy ! subtwoz ! drop
	1 subtwoA ! \ still alive 
;
\ define initial values for player variables 
: initplayer ( -- ) 
	199 dup playerfuel ! 100 - playerfood !	\ max is 99 for both
	6 dup playertorpedos ! 2 + playergrenades !
	0 movecount !
	100 random 100 random 100 random 100 random 	\ get three random distinct numbers
	playerx ! playery ! playerz ! drop
;
\ radar display
: radar ( -- )
	playerfood @ 3 - playerfood !
	playerfuel @ 3 - playerfuel !
	96 colorize
	0 3 at-xy 
	s" ./gamedata/radar.txt" slurp-file type
	0 colorize
	key
	0 3 at-xy 
	s" ./gamedata/radarmask.txt" slurp-file type
	0 colorize
	suboneA @ 0 > if 
		96 colorize 
		subonex @ playerx @ - abs 5 < if 
			3 25 at-xy ." A class submarine found at torpedo E/W range. " key 
		then
		suboney @ playery @ - abs 5 < if 
			3 25 at-xy ." A class submarine found at torpedo N/S range. " key
		then
		subonez @ playerz @ - abs 2 < if 
			3 25 at-xy ." We are at a good depth to fire.               " key
		then
		3 25 at-xy ."                                             "
		0 colorize
	then
	subtwoA @ 0 > if 
		96 colorize 
		subtwox @ playerx @ - abs 5 < if 
			3 25 at-xy ." B class submarine found at torpedo E/W range. " key
		then
		subtwoy @ playery @ - abs 5 < if 
			3 25 at-xy ." B class submarine found at torpedo N/S range. " key
		then
		subtwoz @ playerz @ - abs 2 < if 
			3 25 at-xy ." We are at a good depth to fire.               " key
		then
		3 25 at-xy ."                                             "
		0 colorize
	then
;
\ display the game interface
: display ( -- t ) 
	page
	32 colorize
	3 3 at-xy  ." North : " 
	18 3 at-xy ." East : " 
	30 3 at-xy ." Depth : "
	3 5 at-xy  ." Food  : "
	18 5 at-xy ." Fuel : "
	3 7 at-xy  ." Torpedos : "
	3 9 at-xy ." Grenades : "
	36 colorize
	11 3 at-xy playery @ .
	25 3 at-xy playerx @ .
	38 3 at-xy playerz @ .
	11 5 at-xy playerfood @ .
	25 5 at-xy playerfuel @ .
	15 7 at-xy playertorpedos @ .
	15 9 at-xy playergrenades @ .
;
\ echocheck long range radar
: echocheck ( -- t ) 
	\ insert function code here
	playerfood @ 1 - playerfood !
	playerfuel @ 1 - playerfuel !
	3 25 at-xy 33 colorize ." Yes captain.. checking for them ...             " key
	suboneA @ 0 > if 
		96 colorize 
		subonex @ playerx @ - abs 7 < if 
			3 25 at-xy ." E/W bank echo ! A class sub                 " key
		then
		suboney @ playery @ - abs 7 < if 
			3 25 at-xy ." N/S bank echo ! A class sub                 " key
		then
		subonez @ playerz @ - abs 3 < if 
			3 25 at-xy ." Might be low depth difference ; bank echo ! " key
		then
		3 25 at-xy ."                                             "
		0 colorize
	then
	subtwoA @ 0 > if 
		96 colorize 
		subtwox @ playerx @ - abs 7 < if 
			3 25 at-xy ." E/W bank echo ! B class sub                 " key
		then
		subtwoy @ playery @ - abs 7 < if 
			3 25 at-xy ." N/S bank echo ! B class sub                 " key
		then
		subtwoz @ playerz @ - abs 3 < if 
			3 25 at-xy ." Might be low depth difference ; bank echo ! " key
		then
		3 25 at-xy ."                                             "
		0 colorize
	then
;
\ firegrenade
: firegrenade ( -- t )
	playergrenades @ dup 1 - playergrenades !
	playergrenades @ 1 < if
		3 25 at-xy ." No more grenades available.                     " key
		0 playergrenades !
	then
	playerz @ subonez @ > if
		playerz @ subtwoz @ > if
			playerz @ dup
			suboneA @ 0 > if 
				subonex @ playerx @ = if 
					suboney @ playery @ = if 
						0 3 at-xy 
						33 colorize
						s" ./gamedata/infire.txt" slurp-file type
						key
						s" ./gamedata/infiremask.txt" slurp-file type
						31 colorize
						cr ." KILLED a sub"
						0 suboneA ! 
						key 
					then
				then
			then 				
			playerz @ dup
			subtwoA @ 0 > if 
				subtwox @ playerx @ = if 
					subtwoy @ playery @ = if 
						0 3 at-xy 
						32 colorize
						s" ./gamedata/infire.txt" slurp-file type
						key
						s" ./gamedata/infiremask.txt" slurp-file type
						31 colorize
						cr ." WE KILLED a sub"
						0 subtwoA ! 
						key 
					then
				then
			then 				
		then
	then
;
\ move position 
: movepos ( -- t )
	3 11 at-xy 33 colorize ." Depth ?      J   K              Down / UP                         " cr
	3 13 at-xy ."              U I O                north                           " cr
	3 14 at-xy ." Directions ? H   L       west               east                  " cr
	3 15 at-xy ."              V B N                south                           " cr
	playerfuel @ 1 - playerfuel !
	playerfood @ 1 - playerfood !
	32 25 at-xy
	KEY CASE
		[CHAR] j OF
			playerz @ 1 + playerz !
			playerz @ 0 < if
				0 playerz !
			then
		ENDOF
		[CHAR] J OF
			playerz @ 1 + playerz !
			playerz @ 0 < if
				0 playerz !
			then
		ENDOF
		[CHAR] K OF
			playerz @ 1 - playerz !
			playerz @ 99 > if
				99 playerz !
			then
		ENDOF
		[CHAR] k OF
			playerz @ 1 - playerz !
			playerz @ 99 > if
				99 playerz !
			then
		ENDOF
		[CHAR] u OF 
			playery @ 1 - playery !
			playery @ 0 < if
				0 playery !
			then
			playerx @ 1 - playerx !
			playerx @ 0 < if
				0 playerx !
			then
		ENDOF
		[CHAR] U OF 
			playery @ 1 - playery !
			playery @ 0 < if
				0 playery !
			then
			playerx @ 1 - playerx !
			playerx @ 0 < if
				0 playerx !
			then
		ENDOF
		[CHAR] b OF
			playery @ 1 + playery !
			playery @ 99 > if
				99 playery !
			then
		ENDOF
		[CHAR] B OF
			playery @ 1 + playery !
			playery @ 99 > if
				99 playery !
			then
		ENDOF
		[CHAR] o OF 
			playery @ 1 - playery !
			playery @ 0 < if
				0 playery !
			then
			playerx @ 1 + playerx !
			playerx @ 99 > if
				99 playerx !
			then
		ENDOF
		[CHAR] O OF 
			playery @ 1 - playery !
			playery @ 0 < if
				0 playery !
			then
			playerx @ 1 + playerx !
			playerx @ 99 > if
				99 playerx !
			then
		ENDOF
		[CHAR] h of
			playerx @ 1 - playerx !
			playerx @ 0 < if
				0 playerx !
			then
		ENDOF
		[CHAR] H of
			playerx @ 1 - playerx !
			playerx @ 0 < if
				0 playerx !
			then
		ENDOF
		[CHAR] l of
			playerx @ 1 + playerx !
			playerx @ 99 > if
				99 playerx !
			then
		ENDOF
		[CHAR] L of
			playerx @ 1 + playerx !
			playerx @ 99 > if
				99 playerx !
			then
		ENDOF
		[CHAR] v of
			playerx @ 1 - playerx !
			playerx @ 0 < if
				0 playerx !
			then
			playery @ 1 + playery !
			playery @ 99 > if
				99 playery !
			then
		ENDOF
		[CHAR] V of
			playerx @ 1 - playerx !
			playerx @ 0 < if
				0 playerx !
			then
			playery @ 1 + playery !
			playery @ 99 > if
				99 playery !
			then
		ENDOF
		[CHAR] i of
			playery @ 1 - playery !
			playery @ 0 < if
				0 playery !
			then
		ENDOF
		[CHAR] I of
			playery @ 1 - playery !
			playery @ 0 < if
				0 playery !
			then
		ENDOF
		[CHAR] n of
			playerx @ 1 + playerx !
			playerx @ 99 > if 
				99 playerx !
			then
			playery @ 1 + playery !
			playery @ 99 > if
				99 playery !
			then
		ENDOF
		[CHAR] N of
			playerx @ 1 + playerx !
			playerx @ 99 > if 
				99 playerx !
			then
			playery @ 1 + playery !
			playery @ 99 > if
				99 playery !
			then
		ENDOF
			
		FALSE SWAP
	ENDCASE
;
\ check victory conditions & which of computer or human player is alive
: checkvictory ( -- exit )
	playerfuel @ 0 < if 
		playerz @ 0 < if 
			31 colorize
			3 25 at-xy ." Captain, We're sinking !                    "   key
			0 3 at-xy 
			s" ./gamedata/sank.txt" slurp-file type	key
			0 3 at-xy 
			s" ./gamedata/sankmask.txt" slurp-file type	key
			exitgame
		else
			96 colorize
			3 25 at-xy ." Captain, We have to refuel.                 " key
			display
		then
	then
	playerfood @ 1 < if
		31 colorize
		3 25 at-xy ." Captain, It's mutiny !! You let guys staving!" key
		0 3 at-xy 
		s" ./gamedata/infire.txt" slurp-file type	key
		exitgame
	then
	playerfood @ 10 < if
		playerz @ 0 = if 
			playerfuel 50 < if
				96 colorize
				3 25 at-xy ." Captain, We have to refuel, guys are staving !" key
				display
			then
		else
			playerfuel 25 < if
				31 colorize
				3 25 at-xy ." Captain, It's mutiny !! You let guys starving!" key
				0 3 at-xy 
				s" ./gamedata/infire.txt" slurp-file type	key
				exitgame
			then
		then
	then
;
\ get surface & refuel
: surfacerefuel ( -- t )
	\ make some random killings to make game harder
	100 random 95 > if
		\ 10% risk
		31 colorize
		3 25 at-xy ." Captain, We are targeted                      " 
		100 random dup 10 > if
			." by a boat ! " key
		then
		dup 70 > if 
			." by a plane !                                      " key
		then
		90 > if 
			." by a missile ?                                   " key
		then
		0 3 at-xy 
		s" ./gamedata/infire.txt" slurp-file type	key
		exitgame
	then	
	\ now normal refuelling procedure if not killed
	playerz @ 0 do 
		playerfuel @ 2 - playerfuel !
		playerfood @ 1 - playerfood !
		playerz @ 1 - playerz ! 
		checkvictory	\ if  you cannot get surface in time ... you loose
	loop
	display
	96 colorize
	3 25 at-xy ." Captain, we're refueling from an allied frigate.                " key
	playerfuel @ 95 + playerfuel !
	playerfood @ 75 + playerfood !
	playerfood @ 199 > if 
		199 playerfood ! 	\ force limits to be logical
	then 
	playerfuel @ 199 > if 
		199 playerfuel ! 	\ force limits to be logical
	then 
	playergrenades @ 8 + playergrenades !
	playergrenades @ 8 > if 
		8 playergrenades ! 	\ force limits to be logical
	then 
	playertorpedos @ 6 + playertorpedos !
	playertorpedos @ 6 > if 
		6 playertorpedos ! 	\ force limits to be logical
	then 
	display
;
\ align & fire guided torpedo
: firetorpedo ( -- t )
	playertorpedos @ 1 < if
		31 colorize
		3 25 at-xy ." Captain, we have no more torpedos"
	else
		3 25 at-xy ." Captain, Fire                    "
		playertorpedos @ 1 - playertorpedos ! 
		playerfood @ 1 - playerfood ! 
		playerfuel @ 1 - playerfuel ! 
		suboneA @ 1 = if
			subonex @ playerx @ - abs 5 < if
				suboney @ playery @ - abs 5 < if
					subonez @ playerz @ - abs 2 < if
						0 3 at-xy
						96 colorize
						s" ./gamedata/sank.txt" slurp-file type key
						0 colorize
					then
				then
			then
		then
		subtwoA @ 1 = if
			subtwox @ playerx @ - abs 5 < if
				subtwoy @ playery @ - abs 5 < if
					subtwoz @ playerz @ - abs 2 < if
						0 3 at-xy
						96 colorize
						s" ./gamedata/sank.txt" slurp-file type key
						0 colorize
					then
				then
			then
		then
	then
;
\ player game turn
: playerturn ( -- )
	begin
		32 colorize 
		3 25 at-xy ." Captain, your orders ? : "
		0 colorize 
		KEY CASE
			[CHAR] E OF echocheck ENDOF
			[CHAR] G OF firegrenade ENDOF
			[CHAR] M OF movepos ENDOF
			[CHAR] Q OF exitgame ENDOF		\ exit
			[CHAR] R OF radar ENDOF
			[CHAR] S OF surfacerefuel ENDOF
			[CHAR] T OF firetorpedo ENDOF
			[CHAR] e OF echocheck ENDOF
			[CHAR] g OF firegrenade ENDOF
			[CHAR] m OF movepos ENDOF
			[CHAR] q OF exitgame ENDOF		\ exit
			[CHAR] r OF radar ENDOF
			[CHAR] s OF surfacerefuel ENDOF
			[CHAR] t OF firetorpedo ENDOF
			FALSE SWAP
		ENDCASE
	display
	movecount @ 1 + movecount !
	checkvictory
	AGAIN
;
\ main
: main ( -- ) 
	33 colorize
	intro 
	0 movecount !
	time&date + + + * * seed !		\ initialize random seed
	initplayer				\ create user starting stocks & position
	initsubone
	initsubtwo
	display
	display
	initplayer				\ create user starting stocks & position
	playerturn
	exitgame
;
0 cursor main
