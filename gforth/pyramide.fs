\ 
variable numero_ligne
variable hauteur_pyramide
variable nombre_de_blancs

: valorise_hauteur_pyramide 
        argc @ 2 < if
		cr
		0 hauteur_pyramide ! 
		begin 
			." no argument using user input : "
			PAD dup 2 accept s>number? rot dup hauteur_pyramide !
			cr
		hauteur_pyramide @ 2 >= until
	else
               	1 arg s>number drop hauteur_pyramide !
	endif
;

: valorise_numero_ligne_a_0 
	0 numero_ligne ! 
;

: incremente_numero_ligne 
	numero_ligne @ 1+ numero_ligne ! 
;

: valorise_nombre_de_blancs 
	hauteur_pyramide @ 1- nombre_de_blancs ! 
;

: decremente_nombre_de_blancs 
	nombre_de_blancs @ 1- nombre_de_blancs ! 
;

: efface_ecran 
	page 
;

: passe_a_la_ligne_suivante 
	cr 
;

: dessine_les_blancs 
	nombre_de_blancs @ 0 
        do
		 ."  " 
	loop 
;

: calcul_nombre_d_etoiles_et_dessine_les_etoiles 
	numero_ligne @ 2 * 1+ 0
	do 
		." *" 
        loop 
;

: compare_nombre_de_linges_faites_avec_hauteur_pyramide 
	numero_ligne @ hauteur_pyramide @ 2 - 
;

: dessinebordgauche
	." /"
;

: dessineborddroit
	." \"
;

: pyramide 
efface_ecran
valorise_hauteur_pyramide
valorise_numero_ligne_a_0
valorise_nombre_de_blancs
passe_a_la_ligne_suivante
BEGIN
	dessine_les_blancs
	dessinebordgauche
	calcul_nombre_d_etoiles_et_dessine_les_etoiles
	dessineborddroit
	incremente_numero_ligne
	decremente_nombre_de_blancs
	passe_a_la_ligne_suivante
	compare_nombre_de_linges_faites_avec_hauteur_pyramide 
> UNTIL
passe_a_la_ligne_suivante ;

pyramide bye
