\ from proposal http://www.forth200x.org/fvalue.html
variable %var
: to 1 %var ! ;
: fvalue create f, does> %var @ if f! else f@ then 0 %var ! ;

0e0 fvalue i3
0e0 fvalue r3

59 value x1
21 value y1
-1e0 fvalue i1 
1e0 fvalue i2
-2e0 fvalue r1
1e0 fvalue r2
r2 r1 f- x1 s>f f/ fvalue s1 \ L30
i2 i1 f- y1 s>f f/ fvalue s2 \ L31

0e0 fvalue a
0e0 fvalue b
: single_iter { F: z1 F: z2 } ( F: z1 F: z2 -- F: z1' F: z2' F: mag )
  z1 fdup f* to a \ L90
  z2 fdup f* to b \ L91
  a b f- r3  f+ \ z1 \ L111
  2e0 z1 z2 f* f* i3 f+ \ z2 L110
  a b f+ \ mag \ line 100
;

: print_char ( F: x F: y -- )
  30 \ push the max in case we don't exit early
  30 0 do                          \ L80
    single_iter
    4e0 f> if drop i leave then
  loop                             \ L120
  fdrop fdrop \ clean z1 and z2
  62 swap - emit                   \ L130
;

: calc_i3 { y }
  i1 s2 y s>f f* f+ to i3 \ L50
;

: calc_r3 { x }
  r1 s1 x s>f f* f+ to r3 \ L70
;

: mandel
cr \ always start on a fresh clean line
y1 0 do                         \ L40
  i calc_i3
  x1 0 do                       \ L60
    i calc_r3
    r3 i3 print_char
  loop                            \ L140
  cr                              \ L150
loop                              \ L160
;

mandel bye 

\ >>>>>>=====<<<<<<<<<<<<<<<;;;;;;:::96032:;;;;<<<<==========
\ >>>>>===<<<<<<<<<<<<<<<<;;;;;;;:::873*079::;;;;<<<<<=======
\ >>>>===<<<<<<<<<<<<<<<;;;;;;;::9974    (.9::::;;<<<<<======
\ >>>==<<<<<<<<<<<<<<<;;;;;;:98888764     5789999:;;<<<<<====
\ >>==<<<<<<<<<<<<<;;;;::::996. &2           45335:;<<<<<<===
\ >>=<<<<<<<<<<<;;;::::::999752                 *79:;<<<<<<==
\ >=<<<<<<<<;;;:599999999886                    %78:;;<<<<<<=
\ ><<<<;;;;;:::972456-567763                      +9;;<<<<<<<
\ ><;;;;;;::::9875&      .3                       *9;;;<<<<<<
\ >;;;;;;::997564'        '                       8:;;;<<<<<<
\ >::988897735/                                 &89:;;;<<<<<<
\ >::988897735/                                 &89:;;;<<<<<<
\ >;;;;;;::997564'        '                       8:;;;<<<<<<
\ ><;;;;;;::::9875&      .3                       *9;;;<<<<<<
\ ><<<<;;;;;:::972456-567763                      +9;;<<<<<<<
\ >=<<<<<<<<;;;:599999999886                    %78:;;<<<<<<=
\ >>=<<<<<<<<<<<;;;::::::999752                 *79:;<<<<<<==
\ >>==<<<<<<<<<<<<<;;;;::::996. &2           45335:;<<<<<<===
\ >>>==<<<<<<<<<<<<<<<;;;;;;:98888764     5789999:;;<<<<<====
\ >>>>===<<<<<<<<<<<<<<<;;;;;;;::9974    (.9::::;;<<<<<======
\ >>>>>===<<<<<<<<<<<<<<<<;;;;;;;:::873*079::;;;;<<<<<=======
