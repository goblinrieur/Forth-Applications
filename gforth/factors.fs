: .factors ( n -- )
  2
  begin  2dup dup * >=
  while  2dup /mod swap
         if   drop  1+ 1 or    		\ next odd number
         else -rot nip  dup . ." x "	
         then
  repeat
  drop . ;

: main ( n -- )
  ." 1 : 1" cr				\ displays results
  1+ 2 ?do i . ." : " i .factors cr loop ;

cr 15 main cr 0 (bye)
