;Included for completeness
;Not actually used

.386
.model flat, stdcall

includelib kernel32.lib

FindFirstFileA PROTO, lpFileName:DWORD, lpFindFileData:DWORD
CreateFileA PROTO, lpFileName:DWORD, dwDesiredAccess:DWORD, dwShareMode:DWORD, lpSecurityAttributes:DWORD, dwCreationDisposition:DWORD, dwFlagsAndAttributes:DWORD, hTemplateFile:DWORD
WriteFile PROTO, hFile:DWORD, lpBuffer:DWORD, nNumberOfBytesToWrite:DWORD, lpNumberOfBytesWritten:DWORD, lpOverlapped:DWORD
ExitProcess PROTO, dwExitCode:DWORD
CreateThread PROTO, lpThreadAttributes:DWORD, dwStackSize:DWORD, lpStartAddress:DWORD, lpParameter:DWORD, dwCreationFlags:DWORD, lpThreadID:DWORD

.data
    path    byte    'C:\testfile\test.txt',0
.code 
main:
    push    ebp
    mov	    ebp, esp
    push    0           ;file found flag
    sub     esp, 4      ;threadid
    mov     eax, ebp
    sub     eax, 8
    push    eax
    push    0
    mov	    esi, ebp
    sub	    esi, 4
    push    esi
    lea	    eax, monitort
    push    eax
    push    0
    push    0
    xor	    eax, eax
    call    CreateThread
loopstart:
    inc	    eax
    mov	    edi, [esi]
    cmp	    edi, 1
    jnz	    loopstart

    mov	    edi, eax

    ;open file and get handle
    push    0
    push    80h
    push    2
    push    0
    push    7
    push    102h
    push    offset path
    call    CreateFileA

    ;write
    sub	    esp, 4
    mov	    edx, esp
    add	    edx, 4
    push    0
    push    edx
    push    1
    mov	    [esi], edi
    push    esi
    push    eax
    call    WriteFile
    add	    esp, 4

    push    0
    call    ExitProcess


monitort:
    push    ebp
    mov	    ebp, esp
    sub     esp, 140h
    mov	    edi, esp
    push    esi
    mov	    esi, [ebp+8]
cont:
    push    edi
    push    offset path
    call    FindFirstFileA
    cmp	    eax, -1
    jz	    cont
    mov	    eax, 1
    mov	    [esi], eax
    xor	    eax, eax
    mov	    esi, [ebp-144h]
    add     esp, 144h
    mov	    esp, ebp
    pop	    ebp
    ret
end main
