#! /usr/local/bin/gforth-fast
: array         create 0 do 0 , loop does> swap cells + ;	
0 value str
0 value size
variable pos
30000 array mem		\ arrey to get string to interpret
variable cur

\ helpers
: incr           1 swap +! ;
: decr          -1 swap +! ;
: goto          1- pos ! ;
: cur-char      str pos @ + c@ ;
: mem-null?     cur @ mem @ 0= ;
: end-of-loop?  cur-char [char] ] = ;

\ go to the corresponding ]
: until         postpone 0= postpone while ; immediate
: skip[         [char] [ = if 1+ then ;
: skip]         [char] ] = if 1- then ;
: skip-char     cur-char skip[ cur-char skip] ;
: skip-loop     -1 begin dup 0= end-of-loop? and until skip-char pos incr repeat drop ;

\ switch like macro
: exit-if-not   postpone if postpone else postpone exit postpone then ; immediate
: ===>          postpone over postpone = postpone exit-if-not ; immediate

\ evaluate a bf instruction
\ convertion is easy as all the needed are nearly existing
: eval+         [char] +  ===>  cur @ mem incr ;	\ increment
: eval-         [char] -  ===>  cur @ mem decr ;	\ decrement
: eval.         [char] .  ===>  cur @ mem c@ emit ;	\ print
: eval,         [char] ,  ===>  key cur @ mem c! ;	\ cell 
: eval>         [char] >  ===>  cur incr ;		\ cur+1
: eval<         [char] <  ===>  cur decr ;		\ cur-1
: eval[         [char] [  ===>  mem-null? if skip-loop else pos @ swap then ;	\ block start
: eval]         [char] ]  ===>  swap mem-null? if drop else goto then ;		\ block ends
: eval-char     eval+ eval- eval. eval, eval> eval< eval[ eval] ( and finally ) drop ;

\ main
: init          to size to str 0 pos ! 0 cur ! ;	
: continue?     pos @ size < ;
: bf            init begin continue? while cur-char eval-char pos incr repeat ;
page
cr
cr
s" >+++++++++[<++++++++>-]<.>+++++++[<++++>-]<+.+++++++..+++.[-]>++++++++[<++++>-]<.>+++++++++++[<+++++>-]<.>++++++++[<+++>-]<.+++.------.--------.[-]>++++++++[<++++>-]<+.[-]++++++++++."
bf \ interpret example string
cr
cr
bye

