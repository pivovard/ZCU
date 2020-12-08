include "emu8086.inc"
; START-OF-PROGRAM
org 100h
jmp start  
    
; Memory variables
First	dw	0

prompt1	db	0dh, 0ah,"Enter first integer (-128...127): ",0
prompt2	db	0dh, 0ah,"Enter second integer (-128...127): ",0
product	db	0dh, 0ah,"Product = ",0
interror1	db	0dh, 0ah, "Number is too big!", 0dh, 0ah,0
interror2	db	0dh, 0ah, "Number is too small!", 0dh, 0ah,0
Result  	db	0dh, 0ah, "The product is: ",0

START:	
RD1:    lea	SI, prompt1	; Address of prompt1: “Enter first int”	
    call	PRINT_STRING ; Print prompt1  
   	call	SCAN_NUM	; Read the first integer into CX
	cmp     cx,127		; Is it too big (>127)?
	jg      errG1		; print error
	cmp     cx,-128		; Is it too small (<-128)?
	jl      errL1		; print error
	
CONT:   mov First,cx		; Move the first number into ‘First’
RD2:	lea	SI, prompt2		; Address of prompt2: “Enter second int”
	call	PRINT_STRING	; Print prompt2
	call	SCAN_NUM		; Read the second integer into CX  
	cmp     cx,127		; Is it too big (>127)?
	jg      errG2		; print error
	cmp     cx,-128		; Is it too small (<-128)?
	jl      errL2		; print error
CONT2:	mov ax,First	; Move the first number into AX
	mul     cx		    ; Multiply
	lea	    SI, Result	; Address of string: “The product is”
	call	PRINT_STRING ; Print string
 	CALL	PRINT_NUM	; Print the result
bort:	mov	ax,4c00h	; code for return to ms-dos
	int	21h	; call ms-dos terminate program
	ret
	
errG1: lea	    SI,interror1    ; Address of error message 1
	call	PRINT_STRING	; Print interror1
	jmp     RD1     		; Read again (jump to RD1:)
errL1: lea	    SI,interror2    ; Address of error message 1
	call	PRINT_STRING	; Print interror1
	jmp     RD1     		; Read again (jump to RD1:)
errG2: lea	    SI,interror1    ; Address of error message 1
	call	PRINT_STRING	; Print interror1
	jmp     RD2     		; Read again (jump to RD1:)
errL2: lea	    SI,interror2    ; Address of error message 1
	call	PRINT_STRING	; Print interror1
	jmp     RD2     		; Read again (jump to RD1:)   

; Macro definitions
DEFINE_PRINT_STRING
DEFINE_SCAN_NUM
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
end 					;END-OF-PROGRAM