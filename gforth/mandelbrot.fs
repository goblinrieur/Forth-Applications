fvariable ci 
fvariable c 
fvariable zi 
fvariable z

\ a+bi imaginary number usage/easiest way to manage x/y representation on
\ loop calculations 

: >2? 
	z f@ fdup f* zi f@ fdup f* f+ 4.0e f> 
;

: nextr 
	z f@ fdup f* zi f@ fdup f* f- c f@ f+ 
;

: nexti 
	z f@ zi f@ f* 2.0e f* ci f@ f+ 
;

: pixel 
	c f! ci f! 0e z f! 0e zi f! 
	150 50 do 
		nextr nexti zi f! z f! 
		>2? if 
			i unloop exit 
		then 
	loop bl 
;

: left->right 
	-1.5e 80 0 do 
		fover fover pixel emit 0.026e f+ 
	loop fdrop 
;

: top->bottom 
	cr -1e 40 0 do 
		left->right cr 0.05e f+ 
	loop fdrop 
;

top->bottom bye



\ 4444444444444444444444444445555555555555666666788::CJYIA;98776665555555544444444
\ 4444444444444444444444444555555555555566666777:>UA@FJ  C?=B:77666665555555444444
\ 44444444444444444444445555555555555566667777889;>p      ^ ?;88766666665555544444
\ 4444444444444444444455555555555556667777778889:?C        �S:98877776666665554444
\ 4444444444444444445555555555566667889::9999:::;<>        B=;:::98777778<86555444
\ 4444444444444445555555555666666778;A�D>=;;NiI@XE Po �  } }^C[>AU=:9999:>@8765544
\ 4444444444444555555566666666677788;>N   ]A_P                    A><=FB@@CP866554
\ 4444444444555556666666666667777889:>H   g                         E    O<:766655
\ 44444445555666666666666667777789;?<?CX                                PG:8776655
\ 444555567777777666666777777889:CSc                                    z=:8876655
\ 555566779<;9888888888888888899;<@O                                     D<;:D7665
\ 56666677;?<;::99:;A;:9999999:;Gc                                          EC8665
\ 666667789:;>[O@?<>@P^>D<;:::;<HJ                                         @;97665
\ 666667888::?G  c~ v    ]I><<=?S                                          ?<97665
\ 66678888;<<?KU            J@@C                                           XE87665
\ 778=;:::<>E                 EU                                           C976665
\ 889;>@>A>AO                                                             H9876665
\ 9;;J?F   y                                                             =98776665
\                                                                     G>;:88776665
\ 9;;J?F   y                                                             =98776665
\ 889;>@>A>AO                                                             H9876665
\ 778=;:::<>E                 EU                                           C976665
\ 66678888;<<?KU            J@@C                                           XE87665
\ 666667888::?G  c~ v    ]I><<=?S                                          ?<97665
\ 666667789:;>[O@?<>@P^>D<;:::;<HJ                                         @;97665
\ 56666677;?<;::99:;A;:9999999:;Gc                                          EC8665
\ 555566779<;9888888888888888899;<@O                                     D<;:D7665
\ 444555567777777666666777777889:CSc                                    z=:8876655
\ 44444445555666666666666667777789;?<?CX                                PG:8776655
\ 4444444444555556666666666667777889:>H   g                         E    O<:766655
\ 4444444444444555555566666666677788;>N   ]A_P                    A><=FB@@CP866554
\ 4444444444444445555555555666666778;A�D>=;;NiI@XE Po �  } }^C[>AU=:9999:>@8765544
\ 4444444444444444445555555555566667889::9999:::;<>        B=;:::98777778<86555444
\ 4444444444444444444455555555555556667777778889:?C        �S:98877776666665554444
\ 44444444444444444444445555555555555566667777889;>p      ^ ?;88766666665555544444
\ 4444444444444444444444444555555555555566666777:>UA@FJ  C?=B:77666665555555444444
\ 4444444444444444444444444445555555555555666666788::CJYIA;98776665555555544444444
\ 4444444444444444444444444444455555555555556666677899;>B=:88766555555555444444443
