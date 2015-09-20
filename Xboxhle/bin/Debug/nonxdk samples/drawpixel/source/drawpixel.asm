[ORG 0x10000]
[BITS 32]

; assemble using 'nasm' assembler
; C:>nasm header.asm -o simple.xbe

; XBE HEADER PART ------------------------------------------------------------

db 	'XBEH'
resb    0x100 		; reserve 100 bytes for the xbe security signature
                        ; This only matters on signed xbe's which are
                        ; ms authorised :)

dd 	0x10000         ; Base address of image
dd	0x760           ; Size of header
dd	0x7000		; Size of image
dd      0x178		; Size of image header
dd	0x3f16d3ce	; Time Date Stamp

dd	cert_header	; Certificate Address (plus offset)


dd	0x1		; Number of sections
dd	section_header	; Sectrion headers Address(plus offset)

dd	0x1		; Initialisation Flags
dd	0x11100 ^ 0xA8FC57AB	
                        ; Entry point (XOR with 0xA8FC57AB)
                        ; ***** Very important - this will be our entry
                        ; point for our code - which is the base address of
                        ; our file added to the actual file offset.

dd	0x16000		; Thread local storage directory address
dd	0x0		; Size of stack commit
dd	0x0		; Size of heap reserve
dd	0x0		; Size of heap commit


dd	0x0		; Original base address
dd	0x0		; Original size of image
dd	0x0		; Original checksum
dd	0x3f16d3ce	; Original time date stamp
dd	debugpath	; Debug path name address
dd	debugpath
dd	szname

dd	0x11000 ^ 0x5B6D40B6		
                        ; Kernel image thunk address
                        ; We XOR the original address with 0x5B6D40B6
dd	0x0		; Non-kernel import directory address
dd	0x0		; Number of library versions
dd	0x0		; Library versions area addresses
dd	0x3		; Kernel library version address
dd	0x0		; XAPI library version address
dd	0		; Logo bitmap address
dd	0	        ; Logo bitmap size
                        ; Not included a logo bitmap for this simple example
                        ; and will still run without it, but you could encode
                        ; a really cool image logo for you app maybe :)


; Note: - a lot of assemblers use "[ORG 0x354]" to align data within the assembled
;         file, but for nasm, you have to use: TIMES 0x178-($-$$), which implies
;         that everything from this line is from 0x178 in the file.


; XBE CERTIFICATION ----------------------------------------------------------

TIMES 0x178-($-$$) DB 0 ; I've aligned the certificate to exactly 0x178 in
                        ; the file, but you could exclude this if you want.

cert_header:

dd	0x1dc		; Size of Certification
dd	0x3f16d3ce	; Date Stamp
dd	0x0		; Title ID
resb    0x50		; Title name null terminated string
resb	0x40		; Alt Title
dd	0x0		; Allowed Media
dd	0x0		; Game Region
dd	0x0		; Game Rating
dd	0x0		; Disk Number
dd	0x0		; version
resb    0x10		; Lan
resb	0x10		; Signature Key
resb	0x100		; Alt Sig


; SECTION HEADERS ------------------------------------------------------------
TIMES 0x354-($-$$) DB 0 ; Similar to the xbe cert header, I've aligned the
                        ; single section header to offset 0x354 exactly, but
                        ; you don't have to.

section_header:

dd	0x07		; Flags
dd	0x11000		; Virtual Address (remember offset of 0x10000)
dd	0x6000		; Virutal Size
dd	0x1000		; File pointer to raw data
dd	0x6000		; Size of raw data
dd	section_name	; Address of the section name (Null terminated)
dd	0x0		;
dd	rc1		; head_count_address 
dd	rc2		; tail_count_address 


debugpath:
	db 0x0,0x0,0x0,0x0,0x0,0,0,0,0,0,0,0,0,0,0
szname:
	dw 'x','b','d','e','v',0,0,0

rc1:
  dd 0
rc2:
  dd 0

section_name:
  db '.','t','e','x','t',0,0


; 'OUR CODE'
; THE SECTION WE DEFINED -----------------------------------------------------

; This is the start of our section.. the start of our simple
; code.
; Its offset in the file is at 0x1000
TIMES 0x1000-($-$$) DB 0


section_1_start:		; this should be 0x11000 in memory when loaded
; kernel thunk table
MmAllocateContiguousMemoryEx:
	dd	0x80000000 + 166
MmGetPhysicalAddress:
	dd	0x80000000 + 173
NtAllocateVirtualMemory:
	dd	0x80000000 + 184

	dd	0		; end of table

			

; Entry point of our code - offset 0x1100 in the file.
TIMES 0x1100-($-$$) DB 0x90	; this should be 0x11100 in memory when loaded


jmp   start
 
 
framebuffer:
dd    0xf0010000 + 640*320 + 80;
 
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
DrawPixel:
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
; Setup Function
      push  ebp                           ; Setup our function
      mov   ebp, esp
 
      ; ecx = y;
      ; ebx = x
      ; ecx*=640
      ; ecx = x+y*640
      mov   ecx, DWORD [ebp+12]                 ; put y pos in ecx
      imul  ecx, 640   
      mov   ebx, DWORD [ebp+8]                  ; put x pos int edx
      add   ecx, ebx                      ; hence ecx = x+y*640
     
      mov   ebx, DWORD [framebuffer]            ; Get the location in memory
                                          ; where we will put this pixel
                                          ; e.g. framebuffer
 
; Put blue pixel into memory
      mov   eax, DWORD [ebp+16]                 ; put pixel colour in eax
      and   eax, 255
     
      mov   BYTE [ebx+ecx*4], al
 
; Put green pixel into memory
      mov   eax, DWORD [ebp+16]                 ; put pixel colour in edx
      and   eax, 65280
      shr   eax, 8
 
      mov   BYTE [ebx+ecx*4+1], al
 
; Put red pixel into memory
      mov   eax, DWORD [ebp+16]
      and   eax, 16711680
      shr   eax, 16
 
      mov   BYTE [ebx+ecx*4+2], al
 
; tidy up and return from function call
      pop   ebp
      ret   0
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
; End of DrawPixel
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 
 
 
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
; Globals
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
xpos:
dd    310                     ; width/2 = 640/2 = 310
ypos:
dd    240                     ; height/2 = 480/2 = 240     
colour:
dd    0xff0000              ; Red colour pixel
 
xloop:                           ; We do a simple loop and draw a small line
dd    0x50
 
 
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
start:
;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     
a_loop:
 
      ; This is the same as DrawPixel(x, y, colour)
      push  DWORD [colour]
      push DWORD [ypos]
      push  DWORD [xpos]
      call  DrawPixel
      add   esp, 12
 
      ; Increment the ypos.
      mov   eax, [ypos]
      add   eax, 1
      mov   [ypos], eax
 
      ; Loop around 100 times back to loopx - as sometimes people say...I can't see
      ; the red pixel...so using this simple asm loop... we've managed to draw a
      ; simple line :)
      mov eax, [xloop]
      dec eax
      mov [xloop],eax
      jne a_loop
 
 
      ; Program has now finished, so we simply just stay here and loop :)
bbb: 
      jmp bbb;


TIMES 0x6000-($-$$) DB 0x90	; this will make our section finish at 0x6000
                                ; from the start of the file.

TIMES 0x7000-($-$$) DB 0x0	; And of course, this will make our file size
                                ; equal to 0x7000 a nice round number -
                                ; 0x7000 is 28672bytes... so if you assemble the file
                                ; you should find its that size exactly.