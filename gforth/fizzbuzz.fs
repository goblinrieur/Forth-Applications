: FIZZ?  3 MOD 0 = DUP IF ." FIZZ" THEN ; \ fizz (3mod) 
: BUZZ?  5 MOD 0 = DUP IF ." BUZZ" THEN ; \ buzz (5 mod)
: FIZZ-BUZZ?  DUP FIZZ? SWAP BUZZ? OR INVERT ; \ fizzbuzz 
: DO-FIZZ-BUZZ  25 1 DO CR i FIZZ-BUZZ? IF i . THEN LOOP ; \ classical coding exercise
DO-FIZZ-BUZZ
cr bye

