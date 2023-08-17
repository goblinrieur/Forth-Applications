\ morse translator from text
: exitprog cr cr 0 (bye) ; 	\ exit with 0 return value to system
: dot ." . " ; 		\ displays short
: dash ." _ " ; 	\ displays long
\ define only official morse code (no ponctuations) use stop word with all 4 letters
: a dot dash ;
: b dash dot dot dot ;
: c dash dot dot dot ;
: d dash dot dot ;
: e dot ;
: f dot dot dash dot ;
: g dash dash dot ;
: h dot dot dot dot ;
: ii dot dot ;
: jj dot dash dash dash ;
: kk dash dot dash ;
: l dot dash dot dot ;
: m dash dash ;
: n dash dot ;
: o dash dash dash ;
: p dot dash dash dot ;
: q dash dash dot dash ;
: r dot dash dot ;
: s dot dot dot ;
: t dash ;
: u dot dot dash ;
: v dot dot dash ;
: w dot dash dash ;
: x dash dot dash ;
: y dash dot dash dash ;
: z dash dash dot dot ;
: n1 dot dash dash dash dash ;
: n2 dot dot dash dash dash ;
: n3 dot dot dot dash dash ;
: n4 dot dot dot dot dash ;
: n5 dot dot dot dot dot ;
: n6 dash dot dot dot dot ;
: n7 dash dash dot dot dot ;
: n8 dash dash dash dot dot ;
: n9 dash dash dash dash dot ;
: n0 dash dash dash dash dash ;
\ loop over reading keyboard until ESC is pressed twice
: morseit  ( loop[c] -- c ) 
	begin
		key dup 64 91 within if 
			32 +	\ convert upper char to lower char to get similar coding
		then case
			[char] a of a endof 
			[char] b of b endof 
			[char] c of c endof 
			[char] d of d endof 
			[char] e of e endof 
			[char] f of f endof 
			[char] g of g endof 
			[char] h of h endof 
			[char] i of ii endof 
			[char] j of jj endof 
			[char] k of kk endof 
			[char] l of l endof 
			[char] m of m endof 
			[char] n of n endof 
			[char] o of o endof 
			[char] p of p endof 
			[char] q of q endof 
			[char] r of r endof 
			[char] s of s endof 
			[char] t of t endof 
			[char] u of u endof 
			[char] v of v endof 
			[char] w of w endof 
			[char] x of x endof 
			[char] y of y endof 
			[char] z of z endof 
			[char] 0 of n0 endof 	\ do not forget to get numbers too
			[char] 1 of n1 endof 
			[char] 2 of n2 endof 
			[char] 3 of n3 endof 
			[char] 4 of n4 endof 
			[char] 5 of n5 endof 
			[char] 6 of n6 endof 
			[char] 7 of n7 endof 
			[char] 8 of n8 endof 
			[char] 9 of n9 endof 
			27 of exitprog endof	\ specific to exit function
			32 of ."     " endof 	\ specific to space words
			13 of cr endof 			\ specific to carriage return 
			false swap
		endcase
	again
; 
page .\" Press Escape twice to end\nInput here : " morseit  \ clear screen & start 
