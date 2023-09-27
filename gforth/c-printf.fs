#! /usr/bin/env gforth

c-library stdio
s" stdio" add-lib
\c #include <stdio.h>
c-function printf- printf a -- n
end-c-library

: drop-cstr-length drop ;
cr
s" hello world" drop-cstr-length printf-
cr cr
0 (bye)


