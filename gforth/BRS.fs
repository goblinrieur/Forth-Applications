\ BRS.fs gforth toy to learn gforth
variable solarflag
variable alienflag
variable rockflag
variable transmitflag
variable armflag
variable xpos
variable xpiege
variable ypiege
variable ypos
variable mapxsize
variable mapysize
variable mapextractionpoint_Y
variable mapextractionpoint_X
variable battery
variable arm
variable (rnd)  
variable got
variable alienx
variable alieny
variable rockx
variable rocky
\ variables

27 constant ESC 
time&date  * + - * * (rnd) ! \ seed
100 battery !

: COLORIZE ( n -- ) 
	\ just like terminal ascii \e[31m, here use 31 colorize 
	ESC EMIT ." [" base @ >R 
	0 <# #S #> type 
	R> base ! ." m"  
; 
: help
	page
	CR CR 
	33 colorize
	s" help.txt" slurp-file type 
	0 colorize
	CR 
;
\ randomize & random functions
: rnd ( -- n)
	(rnd) @ dup 13 lshIFt xor
	dup 17 rshIFt xor
	dup DUP 5 lshIFt xor (rnd) !
;
 
: 10rnd 
	rnd BEGIN dup 0 < IF -1 * THEN
	10 / dup 10 < UNTIL got ! 
;
 
: 100rnd 
	rnd BEGIN dup 0 < IF -1 * THEN
	10 / dup 100 < UNTIL got ! 
;
 
: 1000rnd 
	rnd BEGIN 
	dup 0 < IF -1 * THEN
	10 / dup 1000 < UNTIL got ! 
;
 
: energiedec 
	>R battery @ R> - battery ! 
;

\ functions for map management and user actions
: detectmapborder 
	31 colorize
	xpos @ mapxsize @ > IF ." ROBOT IS LOST : YOU LOOSE" CR bye THEN
	xpos @ 0 < IF ." ROBOT IS LOST : : YOU LOOSE" CR bye THEN
	ypos @ 0 < IF ." ROBOT IS LOST : : YOU LOOSE" CR bye THEN
	ypos @ mapysize @ > IF ." ROBOT IS LOST : : YOU LOOSE" CR bye THEN
	0 colorize
	1 energiedec 
;

\ we need to know our coordonates 
: where 
	32 colorize
	xpos @ ." X : " . ypos @ ." Y : " . CR detectmapborder 1 energiedec 
	0 colorize
;

: wait  
	MS where 
;

: pause 
	800 MS 
;

\ we need to check often battery state of the rover
: batterystatus 
	33 colorize
	battery @ dup CR ." BATTERY STATE :" . ." %" CR 1 < IF ." ERROR BATTERY EMPTY : YOU LOOSE !" CR bye THEN
	0 colorize
	1 energiedec 
;

\ we need to deploy solar pannels before charging batteries
: solarpaneldeploy 
	32 colorize
	." .DEPLOYING CHARGING PANEL. " 
	80 0 DO 
		." ." 10rnd got @ MS 
	LOOP CR 1 solarflag ! 
	0 colorize
;

\ solar energy can grow ?
: solarcapture 
	solarflag @ 
	31 colorize
	0 = IF 
		CR ." ERROR : solarpanel not deployed" CR 
	ELSE
		CR ." CHARGING PLEASE WAIT "  pause
		battery @ 
		100 < IF 
			BEGIN
				battery @ 30 < IF 
					BEGIN
						battery @ 3 + battery ! ." ."  battery @ . ." %.." 200 MS
						battery @ 35 > UNTIL
					THEN
					battery @ 50 < IF 
						BEGIN
							battery @ 2 + battery ! ." ."  battery @ . ." %.." 500 MS
							battery @ 87 
						> UNTIL
					THEN
					battery @ 1+ battery !
					." ." battery @ . ." %.." 1000 MS
			battery @ 100 = UNTIL
			pause
		THEN
		CR 
		battery @ 100 > IF 
			pause
			100 battery !
		THEN
	CR ." you may retract solar panel now : solarpaneloff command" CR 
	THEN 
	0 colorize
;

: solarpaneloff 
	cr
	32 colorize
	solarflag @ 
	1 = IF 
		." .STOPPING CHARGING PANEL. " 
		80 0 DO 
			." ." 250 MS 
		LOOP 0 solarflag ! CR 
	ELSE
		0 solarflag ! 
	THEN
	0 colorize
; 


\ we need functions for the arm of the rover
: armdeploy 
	armflag @ 
	1 = IF 
		cr
		." already deployed" CR 
        ELSE 
		cr
		." .ARM DEPLOYING. " 
		80 0 DO 
			." ." 250 MS 
		LOOP 
		CR 1 armflag ! battery @ 4 - battery ! 1 armflag ! 
	THEN
;

: armoff 
	armflag @ 
	0 = IF 
		cr
		." already retracted" CR 
	    ELSE 
		cr
		." .ARM RETRACTION. " 
          	80 0 DO 
			." ." 250 MS 
		LOOP 
	    CR 0 armflag ! battery @ 4 - battery ! 
	THEN
;

: catchobject 
	armflag @ 1 = IF 
		xpos @ alienx @ = IF 
			ypos @ alieny @ = IF ." alien catched" 1 alienflag ! cR THEN
		THEN
		xpos @ xpiege @ = IF 
			ypos @ ypiege @ = IF 
				31 colorize
				." IT'S A TRAP"  cR CR CR CR 
				." CONTACT LOST" CR CR
				s" tput cnorm" system 0 colorize BYE 
			THEN
		THEN
		xpos @ rockx @ = IF 
			ypos @ rocky @ = IF ." rock catched"  THEN
		ELSE
			cr ." nothing here"
		THEN
	ELSE
		." arm not deployed" CR ." energy consumtion HIGH"  battery @ 10 - battery !
	THEN
;

: captureobject 
	cr
	armflag @ 
	1 = IF 
		." TRYING TO CATCH OBJECT" CR catchobject 
	ELSE
		." ARM FAILURE : CANNOT USE ARM" CR 
	THEN
	battery @ 5 - battery ! 
;

: reset 
	CR CR ." REBOOT : " 64 0 DO ." .." 30 10rnd got @ - MS  LOOP CR 
;

\ is move possible ?
: checkmove 
	cr
	solarflag @ 
	1 = IF ." cannot move please retract solarpanner " cr 
		rnd 100rnd got @ MS ." automatic retractation : battery consumtion is high" CR
		solarpaneloff battery @ 10 - battery ! 
	THEN
	armflag @ 
	1 = IF ." cannot move please retract arm " cr 
		rnd 100rnd got @ MS ." automatic retractation : battery consumtion is high" CR
		armoff battery @ 10 - battery ! 
	THEN
	battery @ 25 < IF ." battery low" CR THEN
	battery @ 0 < IF ." ROBOT LOST : too low battery" CR BYE THEN
;

\ directions for movements
: north 
	checkmove ypos @ 1 + ypos ! 4000 ." .moving. " wait 2 energiedec 
;
 
: south 
	checkmove ypos @ 1 - ypos ! 4000 ." .moving. " wait 2 energiedec 
;

: east 
	checkmove xpos @ 1 + xpos ! 4000 ." .moving. " wait 2 energiedec 
;

: west 
	checkmove xpos @ 1 - xpos ! 4000 ." .moving. " wait 2 energiedec 
;

: northeast 
	north east 4000 ." .moving. " wait 1 energiedec 
;

: southeast 
	east south 4000 ." .moving. " wait 1 energiedec 
;
 
: southwest 
	south west 4000 ." .moving. " wait 1 energiedec 
;

: northwest 
	west north 4000 ." .moving. " wait 1 energiedec 
;

\ fake ping to the rover
: ping CR ." PING ?" CR 
	5 0 DO 1000rnd got @ dup MS dup 
		0 = IF ." SIGNAL LOST" bye THEN
		. ."  ms"  CR 
	LOOP 
;

\ intro text for the player wiht very minimal animation
: history
	page
	5 0 DO CR LOOP
	ping
	CR
	31 colorize
	." ROVER IS DETECTING SOMETHING " CR rnd 1000rnd got @ 2* MS
	10 0 DO ." ." rnd 100rnd got @ MS LOOP CR CR CR CR
	." WARNING WARNING ALIEN DETECTED /!\ ALIEN SHOT /!\ " CR rnd 1000rnd got @ MS 
	." WARNING WARNING CAMERA FAILURE /!\ ALIEN SHOT /!\ " CR rnd 1000rnd got @ MS 
	." WARNING WARNING MIC FAILURE    /!\ ?????????? /!\ " CR rnd 1000rnd got @ MS 
	." WARNING WARNING REBOOT NEEDED  /!\ EMERGENCY  /!\ " CR rnd 1000rnd got @ MS 
	." WARNING WARNING BLIND MODE ==  /!\ BLIND OK   /!\ " CR CR CR  rnd 1000rnd got @ MS 
	0 colorize
	reset
	." shell HELP can be used" CR
	page help
;

\ we can transmit data 
: transmit 
	36 colorize
	CR ." Transmission " CR  100rnd got @ 10 * MS ping ." Transmission .." CR 
	alienflag @ rockflag @ = IF 
		." OK" CR
		1 transmitflag ! 
	ELSE
		." OK BUT NO DATA" CR
	THEN
	0 colorize
;

\ main & final functions 
: genmap
	time&date  * + - * * (rnd) ! \ seed
	history
	." RADAR ON MAP: " 
	BEGIN
		rnd 10rnd got @ 5 + mapxsize ! mapxsize @ mapysize !
		rnd 10rnd got @ 1+ 10rnd got @ 1+ alienx ! alieny !
		rnd 10rnd got @ 1+ 10rnd got @ 1+ rockx ! rocky !
		rnd 10rnd got @ 1+ 10rnd got @ 1+ ypos ! xpos !
		rnd 10rnd got @ 1+ 10rnd got @ 1+ ypiege ! xpiege !
		mapxsize @ 
	10 > UNTIL 
	mapxsize @ .  ."  " mapysize @ . CR
	mapxsize @ 10rnd got @ - mapextractionpoint_X !
	mapysize @ 10rnd got @ - mapextractionpoint_Y !
;

: extract 
	33 colorize
	transmitflag @ 1 = IF
		xpos @ mapextractionpoint_X @ = IF 
			ypos @ mapextractionpoint_Y @ = IF 
				CR
				CR
				." YOU TRANSMIT DATAS JUST IN TIME BEFORE SIGNAL LOST ! " CR
				." VICTORY" CR
				bye
			THEN
		THEN
	ELSE
		CR ." cannot extract datas : alien or data not found yet or tranmission failed"
	THEN
	0 colorize
;

\ offering suicide to player is always a good idea
: autodestroy 
	CR CR CR ." ...." 10 MS ." ...." 100 MS ." ...." 
	100 MS ." ...." CR CR 31 colorize ." B*O*O*M" 0 colorize CR CR CR i
	s" tput cnorm" system bye 
;


\ few aliases
: n north ;
: s south ;
: e east ;
: w west ;
: ne northeast ;
: nw northwest ;
: se southeast ;
: sw southwest ;
: ady autodestroy ;
: co captureobject ;
: ca catchobject ;
: ad armdeploy ;
: ao armoff ;
: spo solarpaneloff ;
: sc solarcapture ;
: spd solarpaneldeploy ;
: bs batterystatus ;

: loopgame ( -- )
begin
	key case
		[CHAR] u OF nw ENDOF
		[CHAR] U OF nw ENDOF
		[CHAR] I OF n ENDOF
		[CHAR] i OF n ENDOF
		[CHAR] o OF ne ENDOF
		[CHAR] O OF ne ENDOF
		[CHAR] j OF w ENDOF
		[CHAR] J OF w ENDOF
		[CHAR] h OF help ENDOF
		[CHAR] H OF help ENDOF
		[CHAR] k OF e ENDOF
		[CHAR] K OF e ENDOF
		[CHAR] n OF sw ENDOF
		[CHAR] N OF sw ENDOF
		[CHAR] , OF s ENDOF
		[CHAR] ; OF se ENDOF
		[CHAR] Q OF ady cr s" tput cnorm" system bye ENDOF
		[CHAR] q OF ady cr s" tput cnorm" system bye ENDOF
		[CHAR] c OF co ENDOF
		[CHAR] C OF co ENDOF
		[CHAR] x OF ca ENDOF
		[CHAR] X OF ca ENDOF
		[CHAR] d OF ad ENDOF
		[CHAR] D OF ad ENDOF
		[CHAR] f OF ao ENDOF
		[CHAR] F OF ao ENDOF
		[CHAR] s OF spd sc spo ENDOF
		[CHAR] S OF spd sc spo ENDOF
		[CHAR] t OF bs ENDOF
		[CHAR] T OF bs ENDOF
		[CHAR] E OF transmit ENDOF
		[CHAR] e OF transmit ENDOF
		[CHAR] p OF ping ENDOF
		[CHAR] P OF ping ENDOF
		[CHAR] z OF extract ENDOF
		[CHAR] Z OF extract ENDOF
		FALSE SWAP
	ENDCASE
again
;

\ start game
page 
s" tput civis" system
33 colorize 
s" ./BRStitle.txt" slurp-file type key
page
32 colorize
genmap
loopgame

