#Included for completeness
#Not actually used

.intel_syntax

.extern _FindFirstFileA@8
.extern _CreateFileA@28
.extern _WriteFile@20
.extern _ExitProcess@4
.extern _CreateThread@24

.data
    _path:    .ascii    "C:\\testfile\\test.txt\0"

.global _main
_main:
    push    ebp
    mov    ebp, esp
    push    0           #file found flag
    sub     esp, 4      #threadid
    mov     eax, ebp
    sub     eax, 8
    push    eax
    push    0
    mov     esi, ebp
    sub     esi, 4
    push    esi
    lea     eax, monitort
    push    eax
    push    0
    push    0
    xor     eax, eax
    call    _CreateThread@24
loopstart:
    inc     eax
    mov     edi, [esi]
    cmp     edi, 1
    jnz     loopstart

    mov     edi, eax

    #open file and get handle
    push    0
    push    0x80
    push    2
    push    0
    push    7
    push    0x102
    push    offset _path
    call    _CreateFileA@28

    #write
    sub     esp, 4
    mov     edx, esp
    add     edx, 4
    push    0
    push    edx
    push    1
    mov     [esi], edi
    push    esi
    push    eax
    call    _WriteFile@20
    add     esp, 4

    push    0
    call    _ExitProcess@4


monitort:
    push    ebp
    mov     ebp, esp
    sub     esp, 0x140
    mov     edi, esp
    push    esi
    mov     esi, [ebp+8]
cont:
    push    edi
    push    offset _path
    call    _FindFirstFileA@8
    cmp     eax, -1
    jz      cont
    mov     eax, 1
    mov     [esi], eax
    xor     eax, eax
    mov     esi, [ebp-0x144]
    add     esp, 0x144
    mov     esp, ebp
    pop     ebp
    ret
