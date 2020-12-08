;START-OF-PROGRAM
	    org 	100h 
    	jmp 	start
; define your variables below
msg db  'Enter 4 digit number:',0dh,0ah,'$'        
ab  dw  0   ;declare variable for first 2 numbers
cd  dw  0   ;declare variable for last 2 numbers


start: 
    	lea  	dx,msg 	; load string address to dx
    	call 	puts   	; output string  
    	
;      Write your code here!

    ;Read 4 digit number     
    mov     bx,10

    call    getcb   ;read 1. digit
    mov     ah,0    ;reset ah reg
    sub     al,30h   ;sub ascii code of 0
    mul     bx      ;multiply 1. digit by 10
    mov     ab, ax  ;store 1.digit in ab
    
    call    getcb   ;read 2.digit
    mov     ah,0    ;reset ah reg
    sub     al,30h   ;sub ascii code of 0
    add     ab,ax   ;add 2.digit to ab
    
    call    getcb   ;read 3. digit
    mov     ah,0    ;reset ah reg
    sub     al,30h   ;sub ascii code of 0
    mul     bx      ;multiply 3. digit by 10
    mov     cd, ax  ;store 3.digit in ab
    
    call    getcb   ;read 4.digit
    mov     ah,0    ;reset ah reg
    sub     al,30h   ;sub ascii code of 0
    add     cd,ax   ;add 4.digit to ab
    
    ;calc
    mov ax,ab
    add ax,cd   ;ab+cd
    
    mov bx,ab
    add bx,5    ;5+ab
    
    mul bx      ;(ab+cd)*(5+ab)
    
    mov bx,2             
    mul bx      ;*2
    
    sub ax,5    ;-5

        call    exitus  ;terminate program
    	ret 
; *****************************************************************
;                User-defined subroutines
;        Always paste these at the end of your program
; *****************************************************************
;***** puts
;The subroutine to display a string terminated by '$' 
;dx contains address of string
puts:  mov	ah,9	; output string subprogram
    	int	21h	; call ms-dos output string
    	ret		;
;***** gets
;The subroutine to input a character with echo 
getc:  mov	ah,1; keyboard input subprogram
    	int	21h	; call ms-dos input character
    	ret		; the character input is stored in al
;***** getcb
;The subroutine to input a character without echo 
getcb: mov	ah,8	; keyboard input subprogram
    	int	21h	; call ms-dos input character
    	ret		; the character input is stored in al
;***** putc
;The subroutine to display a character on screen
;dl contains ascii code of the character, e.g. 'a' or 0dh
putc:  mov	ah,2	;character output subprogram
    	int	21h	;call ms-dos output character
    	ret
;***** exitus
;The subroutine to terminate a program and return to ms-dos
exitus: mov	ax,4c00h	;code for return to ms-dos
    	int	21h	;call ms-dos terminate program
   	ret 
;******************* End-of-subroutines *****************************
    	ret
;END-OF-PROGRAM
