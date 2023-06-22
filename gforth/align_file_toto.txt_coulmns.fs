\ align columns

variable column
0 value num-columns
0 value column-widths

: split ( addr len char -- addr len1 addr len-len1 )
  >r 2dup r> scan 2swap 2 pick - ; \ splits on char

: for-each-line ( file len xt -- )
  >r begin #lf split r@ execute 1 /string dup 0<= until 2drop rdrop ;

: for-each-field ( line len xt -- )
  0 column !
  >r begin '$ split r@ execute 1 column +! 1 /string dup 0<= until 2drop rdrop ;

: count-columns ( line len -- )
  ['] 2drop for-each-field
  num-columns column @ max to num-columns ;
: find-num-columns ( file len -- )
  0 to num-columns
  ['] count-columns for-each-line ;

: column-width ( field len -- )
  column-widths column @ + c@
  max
  column-widths column @ + c!
  drop ;
: measure-widths ( line len -- )
  ['] column-width for-each-field ;
: find-column-widths ( file len -- )
  num-columns allocate throw to column-widths
  column-widths num-columns erase
  ['] measure-widths for-each-line ;

\ type aligned, same naming convention as standard numeric U.R, .R
: type.l ( addr len width -- )
  over -               >r type r>       spaces ;
: type.c ( addr len width -- )
  over - dup 2/ spaces >r type r> 1+ 2/ spaces ;
: type.r ( addr len width -- )
  over -        spaces    type ;

defer type.aligned

: print-field ( field len -- )
  column-widths column @ + c@ type.aligned space ;
: print-line   ( line len -- ) cr ['] print-field for-each-field ;
: print-fields ( file len -- )    ['] print-line  for-each-line ;


\ read file
s" toto.txt" slurp-file  ( file len )

\  scan once to determine num-columns
2dup find-num-columns

\  scan again to determine column-widths
2dup find-column-widths

\  print columns, once for each alignment type
' type.l is type.aligned  2dup print-fields cr
' type.c is type.aligned  2dup print-fields cr
' type.r is type.aligned  2dup print-fields cr

\ cleanup
\ nip free throw
column-widths free throw
bye

\ Given      a          text       file   of     many      lines,     where    fields  within  a      line 
\ are        delineated by         a      single 'dollar'  character, write    a       program 
\ that       aligns     each       column of     fields    by         ensuring that    words   in     each 
\ column     are        separated  by     at     least     one        space.   
\ Further,   allow      for        each   word   in        a          column   to      be      either left 
\ justified, right      justified, or     center justified within     its      column. 

\   Given        a         text     file    of     many      lines,    where   fields  within    a    line 
\    are     delineated     by       a    single 'dollar'  character,  write      a    program 
\    that      aligns      each    column   of    fields       by     ensuring  that    words    in   each 
\   column      are     separated    by     at     least      one      space.  
\  Further,    allow       for      each   word     in         a       column    to      be    either left 
\ justified,   right    justified,   or   center justified   within     its    column. 

\      Given          a       text   file     of      many     lines,    where  fields  within      a line 
\        are delineated         by      a single  'dollar' character,    write       a program 
\       that     aligns       each column     of    fields         by ensuring    that   words     in each 
\     column        are  separated     by     at     least        one   space. 
\   Further,      allow        for   each   word        in          a   column      to      be either left 
\ justified,      right justified,     or center justified     within      its column. 
