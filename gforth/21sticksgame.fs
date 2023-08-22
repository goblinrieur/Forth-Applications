\ loads random lib to get random seed & random function
require random.fs
\ defines ref 
-28 constant key-ctrl-c-ior	\ trap control+c key code
21 constant ref
variable joueur 	\ alternate computer/player
variable batons		\ sticks ( squares in there )
: key ( -- x ) \ redefine key to manage trap on CTRL+C
  ['] key catch dup key-ctrl-c-ior = if exit then throw
;
: curseur ( true|anything-else -- display cursor | or not )
	0 = if .\" \e[?25l" else .\" \e[?25h" then 
;
: quitterlejeu	\ restore cursor , text mode and exit 
	.\" \e[0m" true curseur cr cr 0 (bye)	
;
: alternance ( t|f -- f|t )  \ revert true|false state
	joueur @ true = if 
		false joueur ! 
	else
		true joueur ! 
	then
; 
: quijouelepremier ( -- t|f on joueur variable ) \ randomly defines if computer or player plays first
	rnd 50 random 
	25 < if 
		false joueur ! 
	else
		true joueur ! 
	then 
; 
: preparejeu ( -- ) \ prepare game with bold text & clear page & set variable
	.\" \e[1m" false curseur ref batons !  page quijouelepremier
;
: dessinecarres	\ draws sticks as red squares but stars for the last one to be kept
	0 0 at-xy ref 2 * 0 do ."  " loop	\ remove stick line
	0 0 at-xy batons @ 0 do I 1 >= if 128997 xemit else 10024 xemit then loop \ draws them
; 
: joueurgagne
	3 3 at-xy 10062 xemit ."  Player  WINS                          " 10071 xemit \ quit on player winning move 
	quitterlejeu
;
: joueurperds 
	3 3 at-xy 10060 xemit ."  Computer WINS                         " 10071 xemit \ quit on computer wining move
	quitterlejeu
;
: executejeu
	preparejeu dessinecarres
	begin	\ removes asked number of tiles if [1;4]
	joueur @ true = if \ if it is player turn interactively ask for a key 
		3 3 at-xy ." How much to remove ?       " key .\" \b " case	\ replace input by Unicode char [1;4]
			113 of quitterlejeu endof
			81 of quitterlejeu endof \ pressing Q/q also quits
			49 of 10112 xemit batons @ 1 - batons ! endof
			50 of 10113 xemit batons @ 2 - batons ! endof
			51 of 10114 xemit batons @ 3 - batons ! endof
			52 of 10115 xemit batons @ 4 - batons ! endof
			3 3 at-xy ." Warning [1;4] please   "	\ emit a warning if non attended key is used
		endcase
		batons @ 1 <= if 
		    dessinecarres joueurgagne	\ check winning condition
		then
		\ wait for user to press space bar
		dessinecarres 3 4 at-xy ." Press space" begin key 32 = until 3 4 at-xy ."                    " 
		\ gives time to think on strategy
	else
		3 3 at-xy ." Computer plays it turn                      "
		batons @ 5 > if 
				batons @ 4 random 1 + - batons ! \ plays randomly [1;4]
		else
			\ make some very minimal fake dummy intelligence to computer player 
			\ forcing wining move on specific cases 
			batons @ 4 = if batons @ 3 -  batons ! then 
			batons @ 3 = if batons @ 2 -  batons ! then 
			batons @ 2 = if batons @ 1 -  batons ! then 
		then 
		\ wait for user to press space bar
		dessinecarres 3 4 at-xy ." Press space" begin key 32 = until 3 4 at-xy ."                    " 
		batons @ 0 < if
			1 batons ! \ avoid errors on forcing batons to never goes below 1
		then
		batons @ 1 <= if 
			dessinecarres joueurperds	\ check lost condition
		then
		dessinecarres \ draws remaining squares line
	then
	alternance	\ if player turn next is computer turn & reverse
	batons @ 1 <= until		\ ends executejeu actions
;	
executejeu quitterlejeu \ manage an exit on exception
