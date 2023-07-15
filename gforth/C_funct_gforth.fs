c-library cstrings

\c #include <string.h>
c-function strdup strdup a -- a ( c-string -- duped string )
c-function strlen strlen a -- n ( c-string -- length )

end-c-library

\ convenience function (not used here)
: c-string ( addr u -- addr' )
    tuck  pad swap move  pad + 0 swap c!  pad ;

create test s" testing" mem, 0 c,

test strdup value duped

test 7 type 
cr ." ----" cr
test strlen . 
cr ." ----"

cr cr

bye
