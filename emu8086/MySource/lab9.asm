include "emu8086.inc"
; START-OF-PROGRAM
	org 100h
    	jmp start  

; Memory variables:
msg1	db	"Enter a string (max 128 characters): ",0 
msg2	db 	0ah,0dh,"The string is NOT a palindrome.",0 
msg3    db  	0ah,0dh,"The string is a palindrome.",0

; the buffer to save the entered string    
mystr	db  	128 dup (0),0
mystrREV	db  	128 dup (0),0
endl  	db  	0dh,0ah,0  
length	db	0

start:  lea 	SI, msg1    	; Message address
    	CALL	PRINT_STRING	; Print message from [SI]
		lea     DI,mystr  	    ; String mystring: Read string here!
		mov     dx,128	        ; String length
		call    GET_STRING	    ; Read string into [DI]
		lea 	si,endl	        ; cr+lf buffer address
		call    PRINT_STRING    ; Print cr+lf [SI]
		;lea 	SI, mystr	; String mystring buffer address
		;call	PRINT_STRING	; Print string from [SI]
		
		
; count the number of characters in the buffer mystr into CX:
	mov	cx,0	; start from 0
	lea	SI,mystr	; Point SI to mystr
tess:	mov	al,[SI],0	; End of data?
	cmp	al,0	; -“-
	je	seur	; Proceed to next step
	inc	cl	; Increment data counter
	inc	SI	; Increment data pointer
	jmp	tess	; Check next
; copy mystr into mystrREV in reverse order
seur:	mov	length,cl	; Store # of characters in length
        lea si,mystr	; Source buffer address id SI(decremented)
		lea	di,mystrREV	; Result buffer address into DI
		add di,cx       ; move to last position
		sub di,1
coop:	;mov al,[si]		; Copy character from source 
		;mov [di],al	    ; Copy character to destination 
		movsb           ; copy from si to di 
		sub di,2          ; decrement di (-2 because movsb increment both si and di)
		loop coop    	; Take next if not done		
        
; print both buffers
	lea 	si,mystr
	call	print_string	; Print mystr
	lea 	si,endl
	call	print_string	; Print cr+lf
	
	lea 	si,mystrREV
	call	print_string	; Print mystrREV
	lea 	si,endl
	call	print_string	;print cr+lf

; compare strings. If equal => palindrome
	mov	cl,length	; # of characters in buffers 
	lea si,mystr	; address of first buffer
	lea	di,mystrREV	; address of second buffer
niis:	cmp	cl,0	; test if end-of-comparison/buffer
		je  positive	; jump to ok, palindrome/empty buffer	
		mov al,[si]	; Source buffer address
		mov bl,[di]	; Result buffer address
		cmp	al,bl   ; Are same, still chance?
		jne negative	; Nop, jump to print NOT-message and exit
		inc si	    ;increment source pointer
		inc di  	; increment destination pointer
		dec cl	    ; decrement counter
	    jmp niis	; Try next

positive: lea	SI,msg3	; Yess, palindrome
	call	PRINT_STRING	; Print it
	jmp	    abort	        ; and exit

negative: lea	si,msg2	; NOT a palindrome
	call	PRINT_STRING	; Print it and exit		
		
abort:	mov	ax,4c00h	; code for return to ms-dos
	int	21h	; call ms-dos terminate program
	ret

; Macro definitions
DEFINE_GET_STRING
DEFINE_PRINT_STRING
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
end 					;END-OF-PROGRAM
