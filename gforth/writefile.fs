\ sample test
0 Value fd-out
\ this just open a file
: getfname  ( -- addr n) type pad 64 accept pad swap ;
: open-output ( addr u -- )  w/o create-file throw to fd-out ;
: close-output ( -- )  fd-out close-file throw ;

: read s" filename ? " getfname open-output close-output ; 
read
