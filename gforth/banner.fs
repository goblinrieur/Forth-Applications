\


\ ---8<---
\ BANNER.SEQ Compliments of F83X mod to sequential by Tom Zimmer

CREATE CHAR-MATRIX \ build the character generator
HEX ( ) 00 C, 00 C, 00 C, 00 C, 00 C, 00 C, 00 C, 00 C,
( !) 20 C, 20 C, 20 C, 20 C, 20 C, 00 C, 20 C, 00 C,
( ") 50 C, 50 C, 50 C, 00 C, 00 C, 00 C, 00 C, 00 C,
( #) 50 C, 50 C, F8 C, 50 C, F8 C, 50 C, 50 C, 00 C,
( $) 20 C, 78 C, A0 C, 70 C, 28 C, F0 C, 20 C, 00 C,
( %) C0 C, C8 C, 10 C, 20 C, 40 C, 98 C, 18 C, 00 C,
( &) 40 C, A0 C, A0 C, 40 C, A8 C, 90 C, 68 C, 00 C,
( ') 30 C, 30 C, 10 C, 20 C, 00 C, 00 C, 00 C, 00 C,
( () 20 C, 40 C, 80 C, 80 C, 80 C, 40 C, 20 C, 00 C,
( ) 20 C, 10 C, 08 C, 08 C, 08 C, 10 C, 20 C, 00 C,
( *) 20 C, a8 C, 70 C, 20 C, 70 C, a8 C, 20 C, 00 C,
( +) 00 C, 20 C, 20 C, 70 C, 20 C, 20 C, 00 C, 00 C,
( ,) 00 C, 00 C, 00 C, 30 C, 30 C, 10 C, 20 C, 00 C,
( -) 00 C, 00 C, 00 C, 70 C, 00 C, 00 C, 00 C, 00 C,
( .) 00 C, 00 C, 00 C, 00 C, 00 C, 30 C, 30 C, 00 C,
( /) 00 C, 08 C, 10 C, 20 C, 40 C, 80 C, 00 C, 00 C,
( 0) 70 C, 88 C, 98 C, A8 C, C8 C, 88 C, 70 C, 00 C,
( 1) 20 C, 60 C, 20 C, 20 C, 20 C, 20 C, 70 C, 00 C,
( 2) 70 C, 88 C, 08 C, 30 C, 40 C, 80 C, F8 C, 00 C,
( 3) F8 C, 10 C, 20 C, 30 C, 08 C, 88 C, 70 C, 00 C,
( 4) 10 C, 30 C, 50 C, 90 C, F8 C, 10 C, 10 C, 00 C,
( 5) F8 C, 80 C, F0 C, 08 C, 08 C, 88 C, 70 C, 00 C,
( 6) 38 C, 40 C, 80 C, F0 C, 88 C, 88 C, 70 C, 00 C,
( 7) F8 C, 08 C, 10 C, 20 C, 40 C, 40 C, 40 C, 00 C,
( 8) 70 C, 88 C, 88 C, 70 C, 88 C, 88 C, 70 C, 00 C,
( 9) 70 C, 88 C, 88 C, 78 C, 08 C, 10 C, E0 C, 00 C,
( :) 00 C, 60 C, 60 C, 00 C, 60 C, 60 C, 00 C, 00 C,
( ;) 00 C, 60 C, 60 C, 00 C, 60 C, 60 C, 40 C, 00 C,
( <) 10 C, 20 C, 40 C, 80 C, 40 C, 20 C, 10 C, 00 C,
( =) 00 C, 00 C, F8 C, 00 C, F8 C, 00 C, 00 C, 00 C,
( >) 40 C, 20 C, 10 C, 08 C, 10 C, 20 C, 40 C, 00 C,
( ?) 70 C, 88 C, 10 C, 20 C, 20 C, 00 C, 20 C, 00 C,
( @) 70 C, 88 C, A8 C, B8 C, B0 C, 80 C, 78 C, 00 C,
( A) 20 C, 70 C, 88 C, 88 C, F8 C, 88 C, 88 C, 00 C,
( B) F0 C, 88 C, 88 C, F0 C, 88 C, 88 C, F0 C, 00 C,
( C) 70 C, 88 C, 80 C, 80 C, 80 C, 88 C, 70 C, 00 C,
( D) F0 C, 48 C, 48 C, 48 C, 48 C, 48 C, F0 C, 00 C,
( E) F8 C, 80 C, 80 C, F0 C, 80 C, 80 C, F8 C, 00 C,
( F) F8 C, 80 C, 80 C, F0 C, 80 C, 80 C, 80 C, 00 C,
( G) 78 C, 80 C, 80 C, 80 C, 98 C, 88 C, 78 C, 00 C,
( H) 88 C, 88 C, 88 C, F8 C, 88 C, 88 C, 88 C, 00 C,
( I) 70 C, 20 C, 20 C, 20 C, 20 C, 20 C, 70 C, 00 C,
( J) 08 C, 08 C, 08 C, 08 C, 08 C, 88 C, 78 C, 00 C,
( K) 88 C, 90 C, A0 C, C0 C, A0 C, 90 C, 88 C, 00 C,
( L) 80 C, 80 C, 80 C, 80 C, 80 C, 80 C, F8 C, 00 C,
( M) 88 C, D8 C, A8 C, A8 C, 88 C, 88 C, 88 C, 00 C,
( N) 88 C, 88 C, C8 C, A8 C, 98 C, 88 C, 88 C, 00 C,
( O) 70 C, 88 C, 88 C, 88 C, 88 C, 88 C, 70 C, 00 C,
( P) F0 C, 88 C, 88 C, F0 C, 80 C, 80 C, 80 C, 00 C,
( Q) 70 C, 88 C, 88 C, 88 C, A8 C, 90 C, 68 C, 00 C,
( R) F0 C, 88 C, 88 C, F0 C, A0 C, 90 C, 88 C, 00 C,
( S) 70 C, 88 C, 80 C, 70 C, 08 C, 88 C, 70 C, 00 C,
( T) F8 C, 20 C, 20 C, 20 C, 20 C, 20 C, 20 C, 00 C,
( U) 88 C, 88 C, 88 C, 88 C, 88 C, 88 C, 70 C, 00 C,
( V) 88 C, 88 C, 88 C, 88 C, 88 C, 50 C, 20 C, 00 C,
( W) 88 C, 88 C, 88 C, A8 C, A8 C, D8 C, 88 C, 00 C,
( X) 88 C, 88 C, 50 C, 20 C, 50 C, 88 C, 88 C, 00 C,
( Y) 88 C, 88 C, 50 C, 20 C, 20 C, 20 C, 20 C, 00 C,
( Z) F8 C, 08 C, 10 C, 20 C, 40 C, 80 C, F8 C, 00 C,
( [) 78 C, 40 C, 40 C, 40 C, 40 C, 40 C, 78 C, 00 C,
( \) 00 C, 80 C, 40 C, 20 C, 10 C, 08 C, 00 C, 00 C,
( ]) F0 C, 10 C, 10 C, 10 C, 10 C, 10 C, F0 C, 00 C,
( ^) 00 C, 00 C, 20 C, 50 C, 88 C, 00 C, 00 C, 00 C,
( _) 00 C, 00 C, 00 C, 00 C, 00 C, 00 C, 00 C, F8 C,

DECIMAL

CREATE BITS ( --- a1 )
128 C, 64 C, 32 C, 16 C, 8 C, 4 C, 2 C, 1 C,

: BIT ( N1 --- F1 )
BITS + C@ AND 0= 1+ ;



: LC>UC ( c -- )
DUP 96 128 WITHIN 32 AND - ;

: BANNER ( a n -- )
BOUNDS 8 0
DO CR 2DUP
?DO I C@ 127 AND LC>UC 32 -
8 * CHAR-MATRIX + J + C@
7 0
DO DUP I BIT
IF [CHAR] #
ELSE BL
THEN EMIT
LOOP DROP
LOOP
LOOP 2DROP ;

: DEMO ( --- ) \ print demonstration message
CR
" WELCOME" BANNER
" TO F-PC" BANNER
CR
" BANNER" BANNER
" PROGRAM" BANNER
" FROM F83X" BANNER ;
DEMO 
cr 0 (bye)
