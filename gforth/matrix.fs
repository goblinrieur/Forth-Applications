\

: matrix
\ define a #rows by #cols matrix
\ finds the addr of the element at raw# & col#
\ limits the row# and col# to li withon the matrix
	create ( #rows #col -- ) 
		2dup , , ( remember the dimension ) 
		* cells allot ( #rows * #col elements )
	does> ( #rwo #col -- ^element ) 
		dup >r @ min 0 max ( limit col# )
		swap r@ cell+ @ min 0 max ( limit row# )
		swap r@ @ ( retrieve # col = bytes/row )
		rot * + ( apply formula for offset ) 
		2+ cells r> + ( addr of first element ) 
; 
