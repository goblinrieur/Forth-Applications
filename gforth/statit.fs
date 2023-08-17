\ get stat from psql base

\ colorize needs escape sequences
27 constant ESC
5 constant SIZE

\ needs a buffer for strings to be used as tables names or column names
\ so define a vardefine word
: vardefine ( -- varname ) 
	\ vardefine jxtable names it 
	\ s" jx" jxtable 2! store its value
	\ jxtable 2@ type extract it & typ it to terminal
	create 0 , 0 , 
; 

\ defined variables 
vardefine champs
vardefine psqlcall
s" psql -c " psqlcall 2!	\ sets main psql call command line
\ psqlcall 2@ pad place pad count
\ buffers & arrays
create buffer 1024 allot 	\ for general purpose usage
create outputgraph SIZE cells allot	\ general purpose array
\ global variables
2variable out 			\ to manage outputs
\ store command output to out variable
: commandout ( command string -- out variable )
	r/o open-pipe throw dup buffer swap 256 swap read-file throw  swap close-pipe throw drop buffer swap out 2!
;
\ force array to align correctly value on address
: [] ( n addr -- addr[n] )
	 swap cells + 
; 
\ colorize terminal
: colorize ( n -- )
	esc EMIT ." [" base @ >R \ only if needed to save the current workbase
	0 <# #S #> type R> base ! ." m"  
;
\ create a minigrpah from array 
: minigraph ( x[n] -- "[###....]" n* ) 
	0 do 
	33 colorize
	cr ."  [" 	\ displays which index
	0 colorize
	i outputgraph [] @ 100 > if
		33 colorize
		." ++" ." ]"		\ too big make a special mark 
		."  too high to be correctly displayed" 
		0 colorize
	else
		i outputgraph [] @ 0 > IF     \ if value of outpurgraph[out] is > 0 
				0 colorize
				i outputgraph [] @ 0 DO	\ from outputgraph[out] to 0 do 
					.\" #"		\ display pattern
				LOOP
		i outputgraph [] @ ."  " . 33 colorize ."  ]"	\ if too high or to low menton value
		ELSE   
			33 colorize
			." --"
			." ]" 
			."  too low to be visible"	\ if too low
			0 colorize
		then
         THEN        
  LOOP      
; 
\ stat over experience table
: experience ( -- exp ) 
	0 colorize
	clearstack
	0 17 at-xy ." Your Language experience level is" 
	s\" psql -c \qselect sum(exp) from experience where champs ilike 'langues' ;\q | sed '3!d' " commandout 
	34 17 at-xy 33 colorize out 2@ type 0 colorize 
        out 2@ evaluate 1000 / 0 outputgraph [] !
	0 18 at-xy ." Your Computing experience level is" 
	s\" psql -c \qselect sum(exp) from experience where champs ilike 'informatique' ;\q | sed '3!d' " commandout 
	out 2@ evaluate 1000 / 1 outputgraph [] !
	34 18 at-xy 33 colorize out 2@ type 0 colorize 
	0 19 at-xy ." Your Modeling experience level is" 
	s\" psql -c \qselect sum(exp) from experience where champs ilike 'modelisme' ;\q | sed '3!d' " commandout 
	out 2@ evaluate 1000 / 2 outputgraph [] !
	34 19 at-xy 33 colorize out 2@ type 0 colorize 
	0 20 at-xy ." Your DIY experience level is" 
	s\" psql -c \qselect sum(exp) from experience where champs ilike 'bricolage' ;\q | sed '3!d' " commandout 
	out 2@ evaluate 1000 / 3 outputgraph [] !
	34 20 at-xy 33 colorize out 2@ type 0 colorize 
	0 21 at-xy ." Your cooking experience level is" 
	s\" psql -c \qselect sum(exp) from experience where champs ilike 'cuisine' ;\q | sed '3!d' " commandout 
	out 2@ evaluate 1000 / 4 outputgraph [] !
	34 21 at-xy 33 colorize out 2@ type 0 colorize 
	0 22 at-xy
	5 minigraph
	cr 31 colorize ." press a key to get back to menu" key
; 
\ needs a delimiter function to be reused in statistic graph for games
: DELIMITED  
	>r 2dup r> scan nip - 
; 
\ bar
: bar ( -- ) 
	33 colorize
	." [" 100 spaces ." ]"
	0 colorize
;
\ stat over JX table
: games ( -- AVG% ) \ how % people are you over ?
        clearstack
	0 colorize
	0 17 at-xy ." Your gaming experience level is" 
	s" psql -c 'select avg(prct)*100 from jx ;' | sed '3!d;s/ //g' " commandout
	32 17 at-xy 33 colorize out 2@ type
	0 colorize  43 17 at-xy ." % better than all other over the world."
	0 18 at-xy	\ now display a proportionnal block
	bar
	1 18 at-xy	\ inside draw a bargraph
	\ doing a second pass to avoid conversion issues 
	s" psql -c 'select avg(prct)*100 from jx ;' | sed '3!d;s/\..*$//;s/ //' " commandout
	out 2@ evaluate 0 do
		." #"
	loop 
	cr 31 colorize ." press a key to get back to menu" key
; 
\ stat over etatmedical table
: medical ( -- AVG% ) \ how % are  you fine or not
	clearstack
	0 colorize
	0 17 at-xy ." Your medical state is about " 
	s\" psql -c \qselect avg(prct)*100 from etatmedical where description ilike '%epuisement%' ;\q | sed '3!d' " commandout 
	32 17 at-xy 33 colorize out 2@ type
	0 colorize  43 17 at-xy ." % of exhaution.                          "
	0 18 at-xy ." Your medical state is about " 
	s\" psql -c \qselect avg(prct)*100 from etatmedical where description ilike '%stress%' ;\q | sed '3!d' " commandout
	32 18 at-xy 33 colorize out 2@ type 
	0 colorize  43 18 at-xy ." % of stress.                             " 
	0 19 at-xy ." Your medical state is about " 
	s\" psql -c \qselect avg(prct)*100 from etatmedical where description ilike '%angoisse%' ;\q | sed '3!d' " commandout
	32 19 at-xy 33 colorize out 2@ type
	0 colorize  43 19 at-xy ." % of anguish.                             " 
	cr 31 colorize ." press a key to get back to menu" key
; 

\ stat over richness
: accounting ( -- euros )
	0 colorize
	0 17 at-xy ." Your current available :" 
	s" psql -c 'select avg(cc+cr+la++boursoramacc+boursorama_ep) from suivirichesse where date > current_date-90 ;' | sed '3!d' " 
	commandout
	27 17 at-xy 33 colorize out 2@ type 0 colorize 38 17 at-xy ." € adding all dispatches."
	31 colorize cr 
	." press a key to get back to menu" key
; 
\ stat over richness
: richness ( -- euros )
	0 colorize
	0 17 at-xy ." Your current wealth is :" 
	s" psql -c 'select avg(immob+cc+cr+la+pel+millevieper+nuance3d+perp+boursoramacc+boursorama_ep+boursorama_ord+klanik_ep+klanik_ep_creditmut) from suivirichesse where date > current_date-90 ;' | sed '3!d' " 
	commandout
	27 17 at-xy 33 colorize out 2@ type 0 colorize 38 17 at-xy ." € including loans."
	31 colorize cr 
	." press a key to get back to menu" key
; 

\ exit program
: exitprogram ( -- exit )
	cr 
	0 dup colorize
	cr
	(bye) 
;

\ get user input of the date he want to test
: showselection ( file -- menudisplayed ) 
	page
	31 colorize
	3 0 at-xy
	s" ./data/title.txt" slurp-file type
	33 colorize
	3 7 at-xy
	s" ./data/menu.txt" slurp-file type
	cr
	32 colorize ." Your choice : " 33 colorize
; 

\ check if pgnuplot is installed in recent version
: testgnuplot ( -- ok/exit ) 
	s" gnuplot --version | sed 's/.*\([0-9]\)\..*/\1/'" 
	commandout
	out 2@ s>number 5 swap drop  >= if 
		cr	\ cr is visual only during dev/debug
	else
		." gnuplot not present or too old" cr 
		exitprogram
	then 
;

\ check if psql is installed in recent version
: testsql ( -- ok/exit ) 
	s" psql --version | sed 's/.*\([0-9][0-9]\)\..*/\1/'" 
	commandout
	out 2@ s>number 15 swap drop  >= if 
		cr	\ cr is visual only during dev/debug
	else
		." psql not present or too old" cr 
		exitprogram
	then 
;

\ menu 
: selection ( menu key -- subprogram )
	page
	s" tput cinv" system
	begin
	showselection
		KEY CASE
			[CHAR] A OF games ENDOF
			[CHAR] B OF richness ENDOF
			[CHAR] C OF experience ENDOF
			[CHAR] D OF accounting ENDOF
			[CHAR] M OF medical ENDOF
			[CHAR] Q OF exitprogram ENDOF 	\ exit
			[CHAR] a OF games ENDOF
			[CHAR] b OF richness ENDOF
			[CHAR] c OF experience ENDOF
			[CHAR] d OF accounting ENDOF
			[CHAR] m OF medical ENDOF
			[CHAR] q OF exitprogram ENDOF	\ exit
			FALSE SWAP
		ENDCASE
	AGAIN
;

testsql		\ if psql is not present or recent then exit
testgnuplot	\ if pgnuplot is not present or recent then exit
selection	\ main program
