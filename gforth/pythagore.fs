#!  /usr/bin/env gforth

cell 8 <> [if] s" 64-bit system required" exception throw [then]
\ parce que j'accepte des nombres larges  mais bon 
\ qui a encore du 32 ou du 16bits ... hors collection comme ordi

DECIMAL
27 constant ESC

: COLORIZE ( n -- ) \ just like terminal ascii \e[31m, here use 31 colorize 
	ESC EMIT ." [" base @ >R 
	0 <# #S #> type 
	R> base ! ." m"  
; 

: >segments  		( -- x y -- sqrt[ x²+y² ] )             
	cr ." valeur du premier segment : " 	\ x
	pad dup 20 accept >float fdup f*
	cr ." valeur du second segment : " 	\ y 
	pad dup 20 accept >float fdup f* 
	f+ 		\ x²+y²
	fsqrt		\ sqrt 
;

: hypotenuse.		( sqrt[ x²+y² -- ) 
	5 set-precision		\ juste pour afficher proprement 5 chiffres dans le nombre
	CR ." 	hypotenuse : " f. 
	CR CR 
;

: #hypotenuse	( segment a segment b -- segment c )
	>segments		\ saisie des cotés de l'angle droit 
	hypotenuse.		\ affiche en précision 10 la longueur de l'hypoténuse
;

: exitcode ( -- ) 		\ quitter
	27 emit s\" [0m" type 	\ restorer le terminal
	cr cr 
	bye 			\ rendre la main au terminal 
;  

: #shortsegment ( a h -- b )	\ le petit segment
	cr ." valeur du long segment : " 	\ A
	pad dup 20 accept >float fdup f* 
	cr ." valeur du second (hypotenuse) : "	\ H 
	pad dup 20 accept >float fdup f*
	f- fabs					\ h² - a²
	fsqrt					
	5 set-precision				\ juste pour afficher proprement
	CR ." 	segment court : " f. 
	cr cr
;
	
: #longsegment ( b h -- a )	\ le long segment
	cr ." valeur du court segment : " 	\ B
	pad dup 20 accept >float fdup f*
	cr ." valeur du second (hypotenuse) : "	\ H 
	pad dup 20 accept >float fdup f*
	f- fabs					
	fsqrt					
	5 set-precision
	CR ." 	segment long : " f. 
	cr cr
;

: main 
	key case 
		[CHAR] H OF #hypotenuse endof
		[CHAR] h OF #hypotenuse endof
		[CHAR] s OF #shortsegment endof
		[CHAR] S OF #shortsegment endof
		[CHAR] L OF #longsegment endof
		[CHAR] l OF #longsegment endof
		[CHAR] Q OF exitcode endof
		[CHAR] q OF exitcode endof
		false swap
	endcase
;

: 33colormenu ( -- ) \ juste un menu 
	cr 
	." Quelle est le segment manquant :" 
	cr
	33 colorize
	." H 	-	hypotenuse" cr
	." S 	-	segment court" cr
	." L 	-	segment long" cr
	." Q 	-	quitter de suite" cr
	cr
; 

33colormenu main exitcode  \ main code 
