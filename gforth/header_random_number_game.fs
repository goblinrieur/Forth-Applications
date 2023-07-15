\
\ load random lib
require random.fs
\ generate random seed
	\ maybe 
27 constant ESC
\ colorize terminal
: colorize ( n -- )
	esc EMIT ." [" base @ >R \ only if needed to save the current workbase
	0 <# #S #> type R> base ! ." m" \  ESC[31m  like form  
;
\ init random seed
: randomize rnd time&date 4 0 do + loop * seed ! ; 
\ create 24 tiles 
create tiles_array 24 cells allot  
		\ push values in 1-10,1-10,25,50,75,100 (24)
variable A variable B variable C variable D variable E variable F variable GOAL variable tmp
\ fill array
: getout { n }  tiles_array n cells + @ ;		\ put nth element on stack
: putin { n p } n tiles_array p cells + ! ; 	\ put in array value n at position p 
: fillit ( -- ) 														\ create statistic tiles array (many 10's, two each 1,2,...9
	20 0 do I dup putin loop 										\ fill  1-10
	20 11 do I getout 10 - I putin loop								\ fill  1-10 second wave
	10 20 putin 25 21 putin 50 22 putin 75 23 putin 100 0 putin	\ fill one only 25, 50, 75 & 100 tiles
;
			\ first shuffle the cells 
\ pick 6 of them 
: getrandom 24 random getout ; 			\ pick one 
\ display all of that
	\ use cart display : like
		\  +---+ +---+ +---+ +---+ 
		\  | 25| |100| |  3| |  9|
		\  +---+ +---+ +---+ +---+   etc.... for each (note 100 can fit in) 
: drawtile ( x y n -- ) { x y n }
	0 colorize
	x y at-xy ." +---+" x y 1+ at-xy 124 emit 31 colorize n 3 .r 0 colorize 124 emit \ ensure N is displayed on 3 chars whatever is value is 
    x y 2 + at-xy ." +---+"					\ ex .   5 7 2 drawtile 20 15 25 drawtile 30 15 100 drawtile  
;
: getthem ( -- n n n n n n ) 
		getrandom A !  getrandom B !  getrandom C !  getrandom D !  getrandom E !  getrandom f ! 
		 5 3 A @ drawtile  	\ and assign letters to faked-tiles for user usage/selction 
		10 3 B @ drawtile 15 3 C @ drawtile 20 3 D @ drawtile 25 3 E @ drawtile 30 3 F @ drawtile
		31 colorize 6 6 at-xy ."  A    B    C    D    E    F "
;
: shuffle ( -- ) 
	9 random 1+ 0 do getthem loop 	\ do it many times to get real very very random 
; 
\ use tile
: a_use A @ 0 A ! ; 
: b_use B @ 0 B ! ; 
: c_use C @ 0 C ! ; 
: d_use D @ 0 D ! ; 
: e_use E @ 0 E ! ; 
: f_use F @ 0 F ! ; 
\ generate goal number to find 
: getgoal ( -- n [100-999] ) 
	begin
		1000 random 
	dup 0 > until GOAL ! 
;
	\ use a bigger one for the goal 
		\  +-----+
		\  | 576 |
		\  +-----+
: biggertile ( -- )
	16 0 at-xy 33 colorize ." +-----+" 16 1 at-xy ." | " 31 colorize GOAL @ 3 .r 33 colorize ."  |" 16 2 at-xy ." +-----+" 0 colorize
; 
\ exit 
: exitprog ( -- ) 
	0 colorize .\" \e[?25h"	\ restore cursor
	page bye
; 
\ victory process
: victory ( -- ) 
	cr ." replay ? " key
	case
		[char] n of exitprog endof
		[char] N of exitprog endof
		false swap
	endcase
;
\ get user proposal A & B tiles
: whichtile? ( c -- ) 
	32 colorize 10 10 at-xy ." which tile to use as first one ?                          " 
	key 
	case
		[CHAR] a of A @ endof
		[CHAR] b of B @ endof
		[CHAR] c of C @ endof
		[CHAR] d of D @ endof
		[CHAR] e of E @ endof
		[CHAR] f of F @ endof
		[CHAR] A of A @ endof
		[CHAR] B of B @ endof
		[CHAR] C of C @ endof
		[CHAR] D of D @ endof
		[CHAR] E of E @ endof
		[CHAR] F of F @ endof
		false swap
	endcase
;
: loopgame
	\ to be fixed 
	0 tmp ! 
	5 0 do
			10 10 at-xy ." which tile to use now ?                         "
			key 
			case
				[CHAR] a of A @ 0 = if ." already used" exitprog else a_use then endof
				[CHAR] b of B @ 0 = if ." already used" exitprog else b_use then endof
				[CHAR] c of C @ 0 = if ." already used" exitprog else c_use then endof
				[CHAR] d of D @ 0 = if ." already used" exitprog else d_use then endof
				[CHAR] e of E @ 0 = if ." already used" exitprog else e_use then endof
				[CHAR] f of F @ 0 = if ." already used" exitprog else f_use then endof
				[CHAR] A of A @ 0 = if ." already used" exitprog else a_use then endof
				[CHAR] B of B @ 0 = if ." already used" exitprog else b_use then endof
				[CHAR] C of C @ 0 = if ." already used" exitprog else c_use then endof
				[CHAR] D of D @ 0 = if ." already used" exitprog else d_use then endof
				[CHAR] E of E @ 0 = if ." already used" exitprog else e_use then endof
				[CHAR] F of F @ 0 = if ." already used" exitprog else f_use then endof
						false swap
			endcase
			10 10 at-xy ." which operation do do with it?                  " tmp @ 
			key 
			case
					[CHAR] + of + tmp ! endof
					[CHAR] - of - tmp ! endof
					[CHAR] / of / tmp ! endof
					[CHAR] * of * tmp ! endof
						false swap
			endcase
			tmp @ goal @ = if 
				victory
			then
	loop
	cr ." you loose sorry : " tmp ?
	key
	0 colorize		\ restore default "0" coloration
	.\" \e[?25h"	\ restore cursor
	cr cr bye
;
\ compare to goal
\ if ok => win 1000 points
	\ player can replay 
\ if not 
	\ compute a solution
	\ if found player looses then call displayscores & exit
	\ if not player can replay in loop 
: init 
	page .\" \e[?25l" \ hide cursor
	randomize fillit shuffle getthem getgoal biggertile
	0 7 at-xy 
; 
init whichtile?  loopgame
\ display score & exit process
