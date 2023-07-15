\ Bulls and Cows   is an old game played with pencil and paper that was later implemented using computers.
\ Task
\ Create a four digit random number from the digits   1   to   9,   without duplication.
\ The program should:
\                               ask for guesses to this number
\                               reject guesses that are malformed
\                               print the score for the guess
\ The score is computed as:
\     The player wins if the guess is the same as the randomly chosen number, and the program ends.
\     A score of one bull is accumulated for each digit in the guess that equals the corresponding digit in the randomly chosen initial number.
\     A score of one cow is accumulated for each digit in the guess that also appears in the randomly chosen number, but in the wrong position.

include random.fs
create hidden 4 allot

: ok? ( str -- ? )
  dup 4 <> if 2drop false exit then
  1 9 lshift 1- -rot
  bounds do
    i c@ '1 -
    dup 0 9 within 0= if 2drop false leave then
    1 swap lshift over and
    dup 0= if nip leave then
    xor
  loop 0<> ;

: init
  begin
    hidden 4 bounds do 9 random '1 + i c! loop
    hidden 4 ok?
  until ;

: check? ( addr -- solved? )
  0
  4 0 do
    over i + c@
    4 0 do
      dup hidden i + c@ = if     swap
        i j = if 8 else 1 then + swap
      then
    loop drop
  loop nip
  8 /mod tuck . ." bulls, " . ." cows"
  4 = ;

: guess: ( "1234" -- )
  bl parse 2dup 
  ok? 0= if 2drop ." Bad guess! (4 unique digits, 1-9)" exit then
  drop check? if cr ." You guessed it!" bye then ;


: main ( -- ) 
	init
	cr cr ."	use guess: 1234 "
	cr cr ."	use bye to exit "
	cr cr
; 


main

\  	guess: 1234 2 bulls, 1 cows ok
\  	guess: 1243 1 bulls, 2 cows ok
\  	guess: 1436 1 bulls, 3 cows ok
\  	guess: 6134 1 bulls, 3 cows ok
\  	guess: 6143 0 bulls, 4 cows ok
\  	guess: 4361 2 bulls, 2 cows ok
\  	guess: 4361 2 bulls, 2 cows ok
\  	guess: 1364 4 bulls, 0 cows
\  	You guessed it!    
