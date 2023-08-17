\ it is required to validate a date which is entered in the form  dd mm yyyy
\ year is valid in range 1752 to 2050 inclusive
\ a leap year is defined as a year which is exactly divisible by 4 if it is not divisible by 100, 
\ but exactly dividable by 400 if it is.
27 constant ESC
variable day variable month variable year
\ ( year mod 4 = 0 and year mod 100 <> 0 ) or year mod 400 = 0 
: within?           \ n1 n2 n3 -- t|f 
    1+ within
;
: year?             \ year -- t|f
    1752 2050 within? 
; 
: month?            \ month -- t|f
    1 12 within?
;
\ for the day it is more dependant on year and month already set
\ need more checking
: >otherdays        \ month -- max days
    dup 4 = over 6 = or over 9 = or swap 11 = or 
    if
        30
    else
        31
    then
;
: leap?             \ year -- t|f
    dup 4 mod 0= over 100 mod 0<> and swap 400 mod 0= or
;
: >leapdays         \ year -- max days
    leap? if 
        29
    else
        28
    then
;
: >days             \ year month -- max days
    dup 2 = if
        drop >leapdays
    else
        nip >otherdays
    then
;
: days?             \ day year month -- t|f
    >days 1 swap within? 
;
\ now we can put it all together
: valid?            \ day month year -- t|f
    dup year?
    if over month?
        if 
            swap days?
        else
            2drop drop 0    \ clean up and leave false
        then
    else
        2drop drop 0        \ clean up but leave false       
    then
;
\ display management 
: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m"  ;	\ ASCII TERMINAL ONLY 
: cursor? ( true|anything-else -- display cursor | or not )
	0 = if .\" \e[?25l" else .\" \e[?25h" then 
;
\ get user input of the date he want to test
\ check input type
: input# ( -- true | false )
	0. 16 pad swap accept pad swap dup >r >number nip nip r> <> dup 0 = if 
	    nip
	then
;
: ?weekday
    \                       13(m+1)         y%100     y/100       y 
    \ day-number = ( day + (-------) + k + (-----) + (-----) + 5(---) ) %7
    \                          5              4         4        100
    \ now start calculation 
	31 colorize
    month @ 1 + 13 * 5 / day @ +
    year @ 100 mod + year @ 100 mod 4 / + year @ 100 / 4 / + year @ 100 / 5 * +
    7 mod 
    case          \ stack is now one of (0-6) :
        0 of cr ."      Saturday       !" cr  endof
        1 of cr ."      Sunday         !" cr  endof
        2 of cr ."      Monday         !" cr  endof
        3 of cr ."      Tuesday        !" cr  endof
        4 of cr ."      Wednesday      !" cr  endof
        5 of cr ."      Thursday       !" cr  endof
        6 of cr ."      Friday         !" cr  endof
    endcase
	0 colorize
;
\ exit program restoring cursor 
: exitprog ( -- bye ) 
	CR true dup cursor? (bye) \ exit 0 
; 
: askinput?                    \ user keyboard inputs
    32 colorize cr ."  Day     ? " 0 colorize
    input# if dup day ! else ." Must be a valid day number " cr exitprog then
    32 colorize cr ."  Month   ? " 0 colorize
    input# if dup month ! else ." Must be a valid month number " cr exitprog then
    32 colorize cr ."  Year    ? " 0 colorize
    input# if dup year ! else ." Must be a valid year number " cr exitprog then
    cr
    valid? if
        ?weekday \ if all input are valid calculate
    else
        31 colorize cr ." One or more input was not valid" cr 
		0 colorize 1 (bye)	\ else exit on error
    then
;
: main ( -- ) 
		page false cursor?	\ clear screen & disable cursor
		askinput? exitprog	\ let user input values, calculate & display results then exit 0 	
;
main
