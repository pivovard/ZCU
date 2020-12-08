include "emu8086.inc"
; START-OF-PROGRAM
	org 100h
    jmp start  
    
; Memory variables
f0	dw	0	; Previous number
f1	dw 	1	; Second etc. number
n	equ	22	; Loop count (22 first numbers)
prompt1	db	0dh, 0ah,"The first 23 Fibonacci numbers: ",0

start:	lea	SI, prompt1	; Address of prompt1: “The first...”
	    mov 	cx,22	; Loop counter
RD1:	call	PRINT_STRING ; Print prompt1  
	mov	ax,f0    	; First Fibonacci number to ax
	call PRINT_NUM_UNS	; Print first number
	PRINT ' '		; Print space character
;calculate next Fibonacchi number
up1:mov bx,f0		; LOAD f1 to BX
	mov ax,f1		; Load f0 to AX
	call PRINT_NUM_UNS	; PRINT it
	PRINT ' '		; Print space character 
	add bx,ax		; Add: BX=f1+f0
	mov f0,ax		; Update “Previous number”
	mov f1,bx		; Update “Second number”
	;inc cx		; Increment loop counter
	loop up1		; Print next (jmp to up1:)	 
    
abort:	mov	ax,4c00h	; code for return to ms-dos
	int	21h	; call ms-dos terminate program
	ret

; Macro definitions
DEFINE_PRINT_STRING
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
end 					;END-OF-PROGRAM
