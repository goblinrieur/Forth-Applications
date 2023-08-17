\ ***********************************************
\ LE LABO - PROGRAMME GW BASIC
\ Convert in gnuforth for fun
\ Auteurs : PERIN S. translate Francois.P
\ Silicium 2023
\ 
\ ************************************************
require random.fs			\ to lazy to write 3 lines of random seed
variable NB variable NG		\ NB to be guessed NG user guess
: bouclejeu rnd 10 random 1+ NB ! page 
	cr ." Vous avez 3 essais pour trouver un nombre entre 1 et 10." cr
	3 0 do ." Quel est le nombre ? " 
	PAD dup 2 accept s>number? rot dup NG ! 	\ get input
	NG @ NB @ = if cr ." C'est gagné" cr 0 (bye) else
		NG @ NB @ > if cr ." C'est moins ; "  else cr ." C'est plus ; " then 
	then
	I 2 = if cr ." Perdu, il fallait trouver : " NB ? cr 0 (bye) then
	loop ; 
bouclejeu
