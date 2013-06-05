using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise_Decision_Engine
{
    static class Setup
    {
        static bool IsReady()
        {
            return System.IO.File.Exists("engine\bin\rng.exe");
        }

        static bool SetupRNG(string sDataFile)
        {
            /*I do not believe it's come to this
             *The PM _insists_ the functionality and behaviour of that ancient POS is preserved _exactly_
             *Well I'll do that. And those other idiots won't have a fucking clue how this works.
            */


            return true;

        }

        static void Assemble()
        {

        }

        public static StringBuilder GetASM(string sDataFile)
        {
            StringBuilder sbASM = new StringBuilder(2048);
            sbASM.AppendLine(".386");
            sbASM.AppendLine(".model flat, stdcall");
            sbASM.AppendLine("includelib kernel32.lib");
            sbASM.AppendLine("FindFirstFileA PROTO, lpFileName:DWORD, lpFindFileData:DWORD");
            sbASM.Append("CreateFileA PROTO, lpFileName:DWORD, dwDesiredAccess:DWORD, dwShareMode:DWORD,");
            sbASM.Append(" lpSecurityAttributes:DWORD, dwCreationDisposition:DWORD, dwFlagsAndAttributes:DWORD, hTemplateFile:DWORD\r\n");
            sbASM.AppendLine("WriteFile PROTO, hFile:DWORD, lpBuffer:DWORD, nNumberOfBytesToWrite:DWORD, lpNumberOfBytesWritten:DWORD, lpOverlapped:DWORD");
            sbASM.AppendLine("ExitProcess PROTO, dwExitCode:DWORD");
            sbASM.Append("CreateThread PROTO, lpThreadAttributes:DWORD, dwStackSize:DWORD,");
            sbASM.Append("lpStartAddress:DWORD, lpParameter:DWORD, dwCreationFlags:DWORD, lpThreadID:DWORD\r\n");
            sbASM.Append(".data\r\npath byte ");
            sbASM.AppendLine(sDataFile);
            sbASM.AppendLine(".code\r\nmain:");

            string[] ops = { "push", "pop", "mov", "lea", "sub", "add", "xor" };
            string[] regs = { "ebp", "esp", "eax", "edx", "edi", "esi", "" , "h" };
            string code = "A a C a,b A 0 E b,4 C c,a E c,8 A c A 0 C f,a E f,4 A f";
            string ASM = ParseASM(code);
            sbASM.AppendLine(ASM);
            sbASM.AppendLine("lea eax, monitort");
            code = "A c A 0 A 0 G c,c";
            sbASM.AppendLine(ParseASM(code));
            sbASM.AppendLine("call CreateThread");
            sbASM.AppendLine("loopstart:\r\ninc eax");
            sbASM.AppendLine("mov edi,[esi]\r\ncmp edi,1\r\njnz loopstart");
            code = "C e,c A 0 A 80h A 2 A 0 A 7 A 102h";
            sbASM.AppendLine(ParseASM(code));
            sbASM.AppendLine("push offset path\r\ncall CreateFileA");
            code = "E b,4 C d,b F d,4 A 0 A d A 1 C [f],e A f A c";
            sbASM.AppendLine(ParseASM(code));
            sbASM.AppendLine("call WriteFile");
            sbASM.AppendLine("add esp,4\r\npush 0\r\ncall ExitProcess\r\nmonitort:");
            code = "A a C a,b E b,140h C e,b A f C f,[a+8]";
            sbASM.AppendLine(ParseASM(code));
            sbASM.AppendLine("cont:\r\npush edi\r\npush offset path\r\ncall FindFirstFileA");
            sbASM.AppendLine("cmp eax,-1\r\njz cont");
            code = "C c,1 C [f],c G c,c C f,[a-144h] F b,144h C b,a B a";
            sbASM.AppendLine(ParseASM(code));
            sbASM.AppendLine("ret\r\nend main");
            /*System.IO.StreamWriter stream = new System.IO.StreamWriter("testfile.txt");
            stream.Write(sbASM.ToString());
            stream.Close();*/
            return sbASM;
        }

        static string ParseASM(string code)
        {
            StringBuilder sbASMSegment = new StringBuilder();
            string[] ops = { "push", "pop", "mov", "lea", "sub", "add", "xor" };
            string[] regs = { "ebp", "esp", "eax", "edx", "edi", "esi","","h" };
            for (int i = 0; i < code.Length; i++) {
                //if (' ' == code[i]) continue;
                //if (',' == code[i]) {
                //    sbASMSegment.Append(",");
                //    continue;
                //}
                if (0 != (code[i] & 64)
                    && (0x5b != code[i]) && (0x5d != code[i])) {
                    if (0 == (code[i] & 32)) {
                        sbASMSegment.Append("\r\n" + ops[code[i] - 0x41]);
                    } else {
                        sbASMSegment.Append(regs[code[i] - 0x61]);
                    }
                    continue;
                } 
                sbASMSegment.Append(code[i]);
            }
            return sbASMSegment.ToString();
        }
    }
}

