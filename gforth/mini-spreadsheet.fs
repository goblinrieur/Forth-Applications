\ Mini Spreadsheet for ASCII color compatible terminal 
\ ANSI control char must be supported ideal to
\ be used on linux/MacOSX terminals/xterminals emulators
\   in Gforth 0.6.2 Gforth 0.7.3 and still compatible also 0.7.9
\ small improvements by Anton Ertl:
\ small improvement from Francois Pussault
\ yield different numbers and you'll need to change the constants.
\     * If you accidentally quit without saving your work, type
\       main-loop to get back to your sheet.
\     * Labels: begin with "'".
\     * Formulas:
\           o begin with "=";
\           o a3:b12 sums a range;
\           o all operators have equal precedence, so to figure area
\             of circle with radius of 5, use 3.14*(5^2).
\     * Commands: 
\     ESC q 	Quit
\     ESC s 	Save
\     ESC l 	Load
\     ESC c 	Recalculate
\     ESC p 	Set decimal places
\     ; 	Copy a cell
\     , 	Paste and move right
\     / 	Paste and move down
decimal   \ Initialize constants.
form constant winwd  constant winht
11 constant slotwd   31 constant strsize
99 constant maxrow   25 constant maxcol
27 constant ESC  \ to manage escape sequences used as coloration
winht 2 -              constant visrows
winwd 2 - slotwd / 1+  constant viscols
maxcol viscols  - 2 +  constant maxcorner_x
maxrow winht    - 3 +  constant maxcorner_y
maxrow 1+ maxcol 1+ * dup
\ creates & variables
create farrayadr       floats allot
create sarrayadr strsize 1+ * allot
create copied             256 allot
create mypad              256 allot
2variable from
0 Value fd-out \ for saving file
2 value decimalplaces  2variable currentpos
0 value corner_x  0 value corner_y \ The cell shown in upper, left corner.
\
: miplace over >r rot over 1+ r> move c! ; \ place replacement as it will be deprecated soon
: COLORIZE ESC EMIT ." [" base @ >R 0 <# #S #> type R> base ! ." m" ; \ ASCII TERMINAL 
: isdigit?  ( c--flag) '0 '9 1+ within ;
: farray ( x y -- address) 
	maxcol 1+ * + floats farrayadr + ;
: sarray ( x y -- address) 
	maxcol 1+ * + strsize 1+ * sarrayadr + ;
: cfind ( c cadr n -- n)   { c adr n }  -1
	adr n bounds ?do 
		i c@ c = if 
			drop i adr - leave 
		then 
	loop 
;
: tidy ( n1 n2 -- larger+1 smaller) 2dup max 1+ -rot min ;
: letter? ( c--flag) toupper 'A 'Z 1+ within ;
: outside? ( x y -- flag) 0 maxrow 1+ within 0= swap 0 maxcol 1+ within 0= or ;
: bottom 0 winht 1- 2dup at-xy  winwd 1- spaces  at-xy ;
: error  bottom type 31 colorize ."  Press a key:" 0 colorize ekey drop ;
: sumrange ( x y x y -- F:sum) rot tidy { a b } tidy { c d }
  0.0e  a b do  c d  
	do  
		i j farray  f@  f+  
	loop 
loop  ;
\ REF words
: ref? ( cadr n -- cadr n flag) over c@ letter? ;
\ Convert slot reference (e.g., "j35") to x,y.
: ref  ( cadr n -- x y)  'A 10 { adr n offset mul } n 2 < throw
  adr n bounds ?do  
	i c@  toupper offset -  '0 to offset 
 loop
  n 2 - 0 ?do  
	swap mul * +  mul 10 * to mul  
 loop
  2dup outside? if 33 colorize s" Out-of-bounds reference." 0 colorize error 1 throw then ;
: doref ( cadr n -- F:f)  -1 { adr n p }
  ': adr n cfind to p ( Range?)
  p -1 > if  adr p ref  adr p + 1+  n p - 1-  ref? 0= throw  ref  sumrange
  else  adr n ref  farray f@ then ;
\ formulas 
: ops s" */+-^()" ; \ manage formulas operators
create optable ' f* , ' f/ , ' f+ , ' f- , ' f** ,  \ assign forth float manipulations operators
: opfind ( c -- n)  ops cfind ;
: op? ( c -- c flag) dup opfind -1 > ; \ which one ?
create opstack 256 cells allot  opstack cell - value sp
\ push copy run doop
: push sp cell+ to sp  sp ! ;
: copy sp @ ;  : pop  copy  sp cell - to sp ;
0 push
: run ( c --) fdepth 2 < throw  opfind cells optable + @ execute ;
: doop  { op }   
	op '( = if 
		op push exit 
	then
    begin  copy dup '( <> and  while  pop run  repeat
	op ') = if  
		pop drop  \ if op close pop
	else  
		op push  \ else open
	then 
;
: doval ( op adr -- )   0 { op adr len }  here adr -  to len
  len if  
	adr len ref? if  
		doref  
	else  >float  0= throw  then
  len negate allot  then  op doop ;
: infix ( cadr n -- F:f)  here { mark  }
  bounds ?do i c@ op?
     (  ) over  s" +-" cfind 0<   here mark -  or  and ( For unary +-.)
	if  mark doval  
	else  dup BL = if 
			drop 
		else c, 
		then  
	then
  loop   0 mark doval  pop drop
  \ Error if operator left on op-stack.
  copy if begin pop drop copy 0= until 1 throw then ;
\ array for cells management
defer afunc
: doarray ( xt --) is afunc maxrow 1+ 
	0 do maxcol 1+ 
		0 do i j afunc 
		loop 
	loop ;
: fa!        ( F:f x y    --) farray fdup f! ;
: fillfarray ( f          --) ['] fa! doarray fdrop ;
: sa!        ( cadr n x y --) sarray miplace ;
: emptyslot  ( x y        --) sarray 0 swap ! ;
: emptysarray  ['] emptyslot  doarray ;
: .empty   
	36 colorize 
	slotwd 1- 0 ?do  
		." ." 
	loop 
	0 colorize 
; \ draw dots in empty cells
: pos@ currentpos 2@ ; \ current position
: pos! currentpos 2! ;
: >indices ( col row -- i j)  1- corner_y + swap 1- corner_x + swap ;
: ind@  pos@ >indices ;
: .val ( f --) slotwd 1-  decimalplaces 2 f.rdp ;
: head ( col --) ?dup if \ column spacer \ alignment
	corner_x + 64 + slotwd 2/ 
    else 
		32 1 
	then spaces emit ;
: at-slot ( col row --)  swap 1- slotwd * 2 + 0 max swap at-xy ; \ place X/Y cursor on X/Y cell
: disp ( x y --)  { x y }  space  x y sarray count ?dup
  if  over c@ [char] ' =  \ Label.
    if pad slotwd blank  1 /string pad swap move pad slotwd 1- type
    else  2drop  x y farray f@ .val  then
  else  drop  .empty  then ;
: (show_slot) { col row } 
	33 colorize 
	col row >indices  col 
	if
		( Print cell value.) disp
	else
		( Print row number.)  2 .r drop  
	then 
	0 colorize 
;
: show_slot ( col row --)  2dup at-slot  ?dup  if (show_slot) else head then ;
: showrow ( row --)  0 over at-xy  viscols 0 do  i over show_slot  loop drop ;
: at-current pos@ at-slot ;
: show  33 colorize 
	winht 1- 0 do  
		i showrow  
	loop  
	0 colorize at-current 
;
: bounded ( n dn minimum ceiling offset maxoffset -- n offset)
  { n dn minimum ceiling offset maxoffset }
  n dn +  minimum ceiling within if  
		n dn +  offset  
	else  
		n  offset dn +  0 max  maxoffset min  
then ;
: move_rel ( dx dy --) \ Move to another cell.
  pos@ { dx dy x y }  corner_x corner_y  \ Save current view on stack.
  x dx 1 viscols  corner_x maxcorner_x bounded to corner_x
  y dy 1 winht 1- corner_y maxcorner_y bounded to corner_y
  pos!  corner_x corner_y d<> if ( View has shifted.)  show  then
  at-current  ;
: set-dec-places  ( -- ) bottom ." Decimal places? " pad 1 accept pad swap
  -trailing dup
  if  0. 2swap  >number   nip
	\ check integer else error displayed to user
	\ defines number of decimal right of decimal point 
	if 33 colorize s" Bad integer." 0 colorize error 2drop  
	else  d>s to decimalplaces  then
  else 2drop then ;
: calcstr ( cadr n -- F:f)   0 { adr n ch }   n
  if  adr c@   to ch
    [char] '  ch =  if   0.0e   exit  then
    '=        ch =  if  adr n  1 /string  infix  exit   then
    adr n >float  0= throw
  else   0.0e   then ;
: calcslot ( x y --) 2dup sarray count calcstr farray f! ;
: inedit ( c--) bottom  ind@  2dup sarray { x y sadr } x 65 + emit y 1 .r 31 colorize ." >" 0 colorize 
  ?dup if 1 sadr c! sadr 1+ c! ( 1 char. has been typed.) then
  sadr count strsize swap edit-line  sadr c!  x y ['] calcslot catch  if 2drop
	  33 colorize s" Bad cell (use = for formulas, ' for labels)." 0 colorize error  0 recurse
  then  pos@ show_slot ;
: calc ( col row --) 2dup sarray  count  calcstr  farray f! ;
: calc_all ( -- ) \ refresh all calculating all formulas
	bottom 31 colorize ." Calculating..." 0 colorize ['] calc doarray
  show bottom 31 colorize ." ...calculated." 0 colorize ;
: update-current  ind@ calcslot  at-current  pos@ show_slot ;
: tocorner  1 1 pos!  at-current ;
: page_  { p1 sign p2 } pos@ drop dup p1 pos! 0 visrows sign * move_rel
  p2 pos! ;
: page_down  visrows 1 1 page_ ;  : page_up  1 -1 visrows page_ ;
: 2>str ( n1 n2--cadr u) s>d <# #s bl hold 2drop s>d #s #> ;
0 value handle
: wr ( cadr n--) handle write-line throw ;
: wr-slot { x y } x y sarray count ?dup if 
		x y 2>str wr wr 
	else 
		drop 
	then ;
: wr-array ['] wr-slot  doarray ;
\ open \ save \ close files 
: getfname  ( -- addr n) type pad 64 accept pad swap ;
: open-output ( addr u -- )  w/o create-file throw to fd-out ;
: open ( cadr n mode--ior) open-file ?dup if  nip 31 colorize s" Cannot open file." 0 colorize error else  to handle 0  then ;
: close-output ( -- )  fd-out close-file throw ;
: close ( --ior) handle close-file
  if 33 colorize s" Error while closing file." 0 colorize error then ;
\ fix unknown filename saving by creating it empty first then writing in it
: read s" filename ? " getfname 2dup open-output close-output ;
: save bottom 31 colorize read w/o open if exit then ['] wr-array catch 0 colorize if 33 colorize s" Error while writing to file." 0 colorize error then  close ; 
: fload  1 { x }  31 colorize s" Load file: " 0 colorize getfname r/o open if exit then
  begin  pad 80 handle read-line ( n flag ior)
    if  drop 0 33 colorize  s" Error while reading file." 0 colorize error  then
  while pad swap -trailing  x 1 and if evaluate else 2swap sa! then x 1+ to x
  repeat  drop  close  calc_all ;
\ slot ref
variable ,,held   2variable form-offset
: ,,  ( c --)   dup  \ As characters are tacked on, look for slot reference.
  ,,held @
  if  isdigit?
    if 1 ,,held +!
    else  pad 1+ ,,held @  ref ( x y)
       form-offset 2@ d+   2dup outside?
       if 2drop 33 colorize s" Adjusted reference would be out of bounds." 0 colorize error 1 throw
       then    swap 'a + c, s>d <# #s #>
       begin dup while over c@ c, 1 /string repeat  2drop  ,,held off
    then
  else  letter? if  1 ,,held !  then
  then   ,,held @ ?dup if  pad + c! else  ?dup if c, then  then ;
\  Adjust slot-references when pasting so that they have the same relation
\  to the current slot that they did to the source slot.
: fix-formula ( cadr n -- cadr n)    here { mark }  ,,held off
  ind@  from 2@  d-  form-offset 2!  bounds  do  i c@  ,,  loop  0 ,,
  mark  here mark -   mypad miplace    mark here - allot   mypad count ;
: copy-slot  ind@ 2dup from 2! sarray count copied miplace bottom ." Copied." ;
: paste  copied count
  ['] fix-formula catch if 2drop copied count then
  ind@ sarray miplace  update-current ;
: mainloop 0 256 0 { x m mess }  0 copied !  tocorner  show
  \ catch ctrl+c ( once but usable as double ctrl+c ) : so it exit code without gforth internal ctrl+c error message
  begin ['] ekey catch dup -28 = if drop then throw ( Get key.) x or 0 to mess
    case
      #esc      of  m to x                     endof
      'q  m or  of  page exit	               endof
      's  m or  of  0 to x  save               endof
      'l  m or  of  0 to x  fload              endof
      'p  m or  of  0 to x  set-dec-places     endof
      'c  m or  of  0 to x  calc_all 1 to mess endof
      ';        of  copy-slot        1 to mess endof
      #cr       of  0 inedit  0  1 move_rel    endof
      k-left    of         -1  0 move_rel      endof
      k-right   of          1  0 move_rel      endof
      k-up      of          0 -1 move_rel      endof
      k-down    of          0  1 move_rel      endof
      ',        of  paste   1  0 move_rel      endof
      '/        of  paste   0  1 move_rel      endof
      k-prior   of  page_down                  endof
      k-next    of  page_up                    endof
      dup 33 128 within  if dup inedit 0 1 move_rel  then
    endcase  mess 0= if  bottom ind@ sarray count type  then  at-current
  again ;
: start \ displays help at startup 
	s" title.dat" slurp-file cr cr type cr	\ draws figlet style title 
	32 colorize
	s" help.dat" slurp-file type cr	\ draws help
	0 colorize ." press a space to start" cr
	\ to start press space bar \ ctrl+c will do the job too 
    begin ['] key catch dup -28 = if drop exit then throw 32 = until 
;
: main start page 0.0e fillfarray emptysarray 0 to corner_x 0 to corner_y mainloop ;
main 0 (bye) \ exits giving 0 exit status to system
