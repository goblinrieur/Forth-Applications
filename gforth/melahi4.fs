\ Article: 119881 of comp.lang.forth
\ X-Received: by 2002:a05:620a:345:b0:742:8868:bfd1 with SMTP id t5-20020a05620a034500b007428868bfd1mr6295942qkm.7.1678364461409;
\         Thu, 09 Mar 2023 04:21:01 -0800 (PST)
\ X-Received: by 2002:ac8:409e:0:b0:3bf:b829:46ca with SMTP id
\  p30-20020ac8409e000000b003bfb82946camr5972556qtl.1.1678364461088; Thu, 09 Mar
\  2023 04:21:01 -0800 (PST)
\ Path: eternal-september.org!news.eternal-september.org!reader01.eternal-september.org!news.uzoreto.com!peer02.ams4!peer.am4.highwinds-media.com!peer03.iad!feed-me.highwinds-media.com!news.highwinds-media.com!news-out.google.com!nntp.google.com!postnews.google.com!google-groups.googlegroups.com!not-for-mail
\ Newsgroups: comp.lang.forth
\ Date: Thu, 9 Mar 2023 04:21:00 -0800 (PST)
\ In-Reply-To: <2023Feb23.172649@mips.complang.tuwien.ac.at>
\ Injection-Info: google-groups.googlegroups.com; posting-host=154.121.86.103; posting-account=KJSw4AoAAACRkUCek5r_78mFj6sHzH4C
\ NNTP-Posting-Host: 154.121.86.103
\ References: <7e7a9acd-81f6-4022-b12a-753f3b418308n@googlegroups.com>
\  <2023Feb20.222550@mips.complang.tuwien.ac.at> <2023Feb21.163734@mips.complang.tuwien.ac.at>
\  <37f0f9dd-9498-4411-883b-28d91fa5cbafn@googlegroups.com> <2023Feb22.092548@mips.complang.tuwien.ac.at>
\  <29ace813-b44e-4591-bbc0-9ddfb0e6f542n@googlegroups.com> <2023Feb23.172649@mips.complang.tuwien.ac.at>
\ User-Agent: G2/1.0
\ MIME-Version: 1.0
\ Message-ID: <ed2892e3-a25d-45ed-8cde-f8c08f02b122n@googlegroups.com>
\ Subject: Re: Magic Hexagon
\ From: Ahmed MELAHI <ahmed.melahi@univ-bejaia.dz>
\ Injection-Date: Thu, 09 Mar 2023 12:21:01 +0000
\ Content-Type: text/plain; charset="UTF-8"
\ Content-Transfer-Encoding: quoted-printable
\ X-Received-Bytes: 17753
\ Xref: reader01.eternal-september.org comp.lang.forth:119881
\ Le jeudi 23 février 2023 à 16:37:16 UTC, Anton Ertl a écrit :
\ > Ahmed MELAHI <ahmed....@univ-bejaia.dz> writes: 
\ > >Here, The last version of the program, I removed superfluous consecutive un=
\ > >marks and and marks that consumed time.
\ > >I tested it on my PC: Intel(R) Celeron(R) CPU 3867U @ 1.80GHz 1.80 GHz, = 
\ > >12GB:=20 
\ > > - gforth-fast:=20 
\ > > - 1 solution: about 2.3 ms (the same result if I use A<C=
\ > >,...) 
\ > > - 12 solutions (all): 70 ms 
\ > > - gforth: 
\ > > - 1 solution: 5 ms 
\ > > - 12 solutions: 146 ms
\ > Again gforth-fast on Ryzen 5800X: 
\ > 
\ > overhead e-s A e-s B melahi3 
\ > 25_905_373 32_745_355 33_870_576 35_218_929 cycles:u 
\ > 70_131_630 81_093_523 83_096_994 89_686_731 instructions:u 
\ > 0.007722082 0.009659665 0.009792036 0.010334186 seconds time elapsed
\ > "overhead" is just the startup overhead of gforth-fast.
\ > "melahi3" is your newest version.
\ > "e-s A" is ertl-simple modified to just stop after finding the solution 
\ > "e-s B" is e-s A modified to not eliminated rotated and mirrored sols.
\ > They are very close to each other now.
\ > - anton 
\ > -- 
\ > M. Anton Ertl http://www.complang.tuwien.ac.at/anton/home.html 
\ > comp.lang.forth FAQs: http://www.complang.tuwien.ac.at/forth/faq/toc.html 
\ > New standard: https://forth-standard.org/ 
\ > EuroForth 2022: https://euro.theforth.net
\ HI,
\ Thanks for testing.
\ Here is the final version of the program magic_hexagon.
\ Here, there is no tables to fill, the search is applied directly.
\ The program is now reduced in size, and faster.
\ ----------------------Here begins the listing
\ Place the integers 1..19 in the following Magic Hexagon of rank 3
\ __A_B_C__
\ _D_E_F_G_
\ H_I_J_K_L
\ _M_N_O_P_
\ __Q_R_S__
\ so that the sum of all numbers in a straight line (horizontal and diagonal)
\ is equal to 38.
\ here begins the application
0 value vA 0 value vB 0 value vC 0 value vD 0 value vE 0 value vF 
0 value vG 0 value vH 0 value vI 0 value vJ 0 value vK 0 value vL 
0 value vM 0 value vN 0 value vO 0 value vP 0 value vQ 0 value vR 
0 value vS 0 value n_sol
create marked 20 allot
marked 20 erase
create solutions 20 20 * allot
: -- 2 .r 2 spaces ;
: .mag_hex
cr n_sol . ." solutions found."
  n_sol 1+ 1 
  ?do
      0 i 20 * + solutions + c@ to vA
      1 i 20 * + solutions + c@ to vB
      2 i 20 * + solutions + c@ to vC
      3 i 20 * + solutions + c@ to vD
      4 i 20 * + solutions + c@ to vE
      5 i 20 * + solutions + c@ to vF
      6 i 20 * + solutions + c@ to vG
      7 i 20 * + solutions + c@ to vH
      8 i 20 * + solutions + c@ to vI
      9 i 20 * + solutions + c@ to vJ
     10 i 20 * + solutions + c@ to vK
     11 i 20 * + solutions + c@ to vL
     12 i 20 * + solutions + c@ to vM
     13 i 20 * + solutions + c@ to vN
     14 i 20 * + solutions + c@ to vO
     15 i 20 * + solutions + c@ to vP
     16 i 20 * + solutions + c@ to vQ
     17 i 20 * + solutions + c@ to vR
     18 i 20 * + solutions + c@ to vS
    cr
    cr 
    4 spaces       vA -- vB -- vC -- cr 
    2 spaces    vD -- vE -- vF -- vG -- cr 
         vH -- vI -- vJ -- vK -- vL -- cr 
    2 spaces    vM -- vN -- vO -- vP -- cr 
    4 spaces       vQ -- vR -- vS -- 
  loop cr
;
: .solution 
  cr n_sol . ." solutions found." n_sol 1+ 1 
  ?do
      0 i 20 * + solutions + c@ to vA
      1 i 20 * + solutions + c@ to vB
      2 i 20 * + solutions + c@ to vC
      3 i 20 * + solutions + c@ to vD
      4 i 20 * + solutions + c@ to vE
      5 i 20 * + solutions + c@ to vF
      6 i 20 * + solutions + c@ to vG
      7 i 20 * + solutions + c@ to vH
      8 i 20 * + solutions + c@ to vI
      9 i 20 * + solutions + c@ to vJ
     10 i 20 * + solutions + c@ to vK
     11 i 20 * + solutions + c@ to vL
     12 i 20 * + solutions + c@ to vM
     13 i 20 * + solutions + c@ to vN
     14 i 20 * + solutions + c@ to vO
     15 i 20 * + solutions + c@ to vP
     16 i 20 * + solutions + c@ to vQ
     17 i 20 * + solutions + c@ to vR
     18 i 20 * + solutions + c@ to vS
     cr                       
     ." A=" vA 2 .r space ." B=" vB 2 .r space ." C=" vC 2 .r space
     ." D=" vD 2 .r space ." E=" vE 2 .r space ." F=" vF 2 .r space
     ." G=" vG 2 .r space ." H=" vH 2 .r space ." I=" vI 2 .r space
     ." J=" vJ 2 .r space ." K=" vK 2 .r space ." L=" vL 2 .r space
     ." M=" vM 2 .r space ." N=" vN 2 .r space ." O=" vO 2 .r space
     ." P=" vP 2 .r space ." Q=" vQ 2 .r space ." R=" vR 2 .r space
     ." S=" vS 2 .r 
  loop 
;
: solve
    0 to n_sol
    marked 20 erase
    \ A
    20 1 
    do
      i to vA                                  
      1 vA marked + c!
      \ B
      20 1 
      do 
        i to vB
        vB marked + c@ 0= 
        if 
          1 vB marked + c!
          38 vA vB + - to vC
          vC 0> 
          vC 20 < and
          vC marked + c@ 0= and
          if \ C
            1 vC marked + c!
            \ G
            20 1 
            do
              i to vG
              vG marked + c@ 0=
              if 
                1 vG marked + c! 
                38 vC vG + - to vL
                vL 0> 
                vL 20 < and
                vL marked + c@ 0= and
                  if 
                  1 vL marked + c!
                  \ PS
                  20 1 
                  do
                    i to vP
                    vP marked + c@ 0=
                    if 
                      1 vP marked + c!
                      38 vL vP + - to vS
                      vS 0> 
                      vS 20 < and
                      vS marked + c@ 0= and
                      if 
                        1 vS marked + c!
                        \ RQ
                        20 1 
                        do
                          i to vR
                          vR marked + c@ 0=
                          if 
                            1 vR marked + c! 
                            38 vS vR + - to vQ
                            vQ 0> 
                            vQ 20 < and
                            vQ marked + c@ 0= and
                            if 
                              1 vQ marked + c!
                              \ MH
                              20 1 
                              do
                                i to vM
                                vM marked + c@ 0=
                                if 
                                  1 vM marked + c!
                                  38 vQ vM + - to vH
                                  vH 0>
                                  vH 20 < and
                                  vH marked + c@ 0= and
                                  if 
                                    1 vH marked + c!
 
                                    \ calculate D (38-A-H = D)
                                    38 vA vH + - to vD 
                                    vD 0>
                                    vD 20 < and
                                    vD marked + c@ 0= and 
                                    if
                                      1 vD marked + c!
                                      20 1 
                                      do
                                        i to vE
                                        vE marked + c@ 0=
                                        if 
                                          1 vE marked + c!
               
                                          38 vD vE + vG + - to vF
                                          vF 0> 
                                          vF 20 < and 
                                          vF marked + c@ 0= and
                                          if 
                                            1 vF marked + c!
     
                                            \ calculate K (K = 38-B-F-P)
                                            38 vB vF + vP + - to vK 
                                            vK 0> 
                                            vK 20 < and
                                            vK marked + c@ 0= and
                                            if
                                              \ calculate O (O = 38-G-K-R)
                                              1 vK marked + c! 
 
                                              38 vG vK + vR + - to vO                                              
                                              vO 0> 
                                              vO 20 < and
                                              vO marked + c@ 0= and
                                              if 
                                                \ calculate N (N = 38-P-O-M)
                                                1 vO marked + c!
                                                38 vP vO + vM + - to vN
                                                vN 0> 
                                                vN 20 < and
                                                vN marked + c@ 0= and 
                                                if
                                                  \ calculate I (I = 38-R-N-D)
                                                  1 vN marked + c!
                                                  38 vR vN + vD + - to vI
                                                  vI 0> 
                                                  vI 20 < and
                                                  vI marked + c@ 0= and
                                                  if 
                                                    \ calculate J (J = 38-H-I-K-L)
                                                    1 vI marked + c!
                                                    38 vH vI + vK + vL + - to vJ
                                                    vJ 0> 
                                                    vJ 20 < and
                                                    vJ marked + c@ 0= and
                                                    if
                                                      \ 1 vJ marked + c!
                                                      n_sol 1+ to n_sol                                   
                                                      vA  0 n_sol 20 * + solutions + c!
                                                      vB  1 n_sol 20 * + solutions + c!
                                                      vC  2 n_sol 20 * + solutions + c!
                                                      vD  3 n_sol 20 * + solutions + c!
                                                      vE  4 n_sol 20 * + solutions + c!
                                                      vF  5 n_sol 20 * + solutions + c!
                                                      vG  6 n_sol 20 * + solutions + c!
                                                      vH  7 n_sol 20 * + solutions + c!
                                                      vI  8 n_sol 20 * + solutions + c!
                                                      vJ  9 n_sol 20 * + solutions + c!
                                                      vK 10 n_sol 20 * + solutions + c!
                                                      vL 11 n_sol 20 * + solutions + c!
                                                      vM 12 n_sol 20 * + solutions + c!
                                                      vN 13 n_sol 20 * + solutions + c!
                                                      vO 14 n_sol 20 * + solutions + c!
                                                      vP 15 n_sol 20 * + solutions + c!
                                                      vQ 16 n_sol 20 * + solutions + c!
                                                      vR 17 n_sol 20 * + solutions + c!
                                                      vS 18 n_sol 20 * + solutions + c!
\ +-----------------------------------------------------------------------------------------------------------------------+
\ | to get just one solution uncomment out the line hereafter, to get all solutions (12) comment out the line hereafter.  |
\ +-----------------------------------------------------------------------------------------------------------------------+
                                                      unloop unloop unloop unloop unloop unloop unloop exit
       
                                                      \ 0 vJ marked + c! 
                                                    then                   \ vJ
                                                    0 vI marked + c!   
                                                  then                     \ vI
                                                  0 vN marked + c! 
                                                then                       \ vN
                                                0 vO marked + c!   
                                              then                         \ vO
                                              0 vK marked + c!    
                                            then                           \ vK
                                            0 vF marked + c!
                                          then                             \ vF
                                          0 vE marked + c!
                                        then
                                      loop                                 \ vE
                                      0 vD marked + c!        
                                    then                                   \ vD 
                                    0 vH marked + c! 
                                  then                                     \ vH
                                  0 vM marked + c!
                                then
                              loop                                         \ vM 
                              0 vQ marked + c! 
                            then                                           \ vQ  
                            0 vR marked + c!
                          then
                        loop                                               \ vR  
                        0 vS marked + c! 
                      then                                                 \ vS                    
                      0 vP marked + c!
                    then
                  loop                                                     \ vP
                  0 vL marked + c!  
                then                                                       \ vL
                0 vG marked + c!
              then
            loop                                                           \ vG 
            0 vC marked + c! 
          then                                                             \ vC
          0 vB marked + c!  
        then
      loop       
      0 vA marked + c!
    loop                                                                   \ vA
;
: timing_1000 
    utime 1000 0 do 
      solve 
    loop 
    utime d>f d>f f- 1000e f/ cr cr ." Mean execution time : " f. ." micro seconds."  
; 
 utime solve utime d>f d>f f- cr cr ." execution time : " f. ."  micro seconds." cr cr .solution cr cr .mag_hex
\ timing_10000
\ -----------Here, the listing finishes.
bye
\ https://github.com/AntonErtl/magic-hexagon/tree/main
