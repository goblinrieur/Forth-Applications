: th-root { F: a F: n -- a^1/n } \ nth root
	a begin				\ apply mathematical method
    	a fover n 1e f- f** f/
    	fover n 1e f- f*
    	f+ n f/ fswap fover 
	1e-5 f~ until 
;

cr 34e 5e th-root f.   \ 2.02439745849989
cr 34e 5e 1/f f** f.   \ 2.02439745849989
cr 0 (bye)
