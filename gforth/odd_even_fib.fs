( Warm-up 1 )
: say-odd 100 dup dup 11 + 3 0 do emit loop ;			\ displays odd
: say-even 110 101 118 over 4 0 do emit loop ;			\ displays even

( Warm-up 2 )
: say-odd-or-even dup . 2 mod 0 = if say-even else say-odd then ;
( Warm-up 3 )
: say-odd-or-even-up-to 0 do i 1 + say-odd-or-even cr loop ;
( Grand-challenge )
( With do-loop )
: fib 1 1 rot 2 - 0 do dup rot + loop swap drop ;
( With recursion )
: fib-recurse 
  dup 3 <
  if drop 1
  else
    dup 1 - recurse
    swap
    2 - recurse
    +
  then
;
