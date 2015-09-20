%include "kernel.inc"
%include "xbe.inc"
 
IMPORT	KeTickCount
IMPORT	HalReturnToFirmware
 
;##### XBOX Program Entry Point #####
XBE_START
 
	;#### Paint the Screen ####
	mov	ebx,VIDEO_MEMORY
	mov	ecx,VIDEO_LIMIT
fill:
	mov	BYTE[ebx+1],0xFF
	add	ebx,4
	cmp	ebx,ecx
	jl	fill
 
	;#### Small Wait Loop (5 seconds) ####
	mov	ebx,DWORD[KeTickCount]
	mov	ecx,DWORD[ebx]
	add	ecx,5000
.wait_loop:
	cmp	DWORD[ebx],ecx
	jl	.wait_loop
 
	;#### Return to Dashboard ####
	push	DWORD	0x02
	call	[HalReturnToFirmware]
jmp $
 
;##### End of XBOX Program #####
XBE_END