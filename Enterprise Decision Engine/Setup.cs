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
            sbASM.AppendLine("include kernel32.lib");
            sbASM.AppendLine("FindFirstFileA PROTO, lpFileName:DWORD, lpFindFileData:DWORD");
            sbASM.Append("CreateFileA PROTO, lpFileName:DWORD, dwDesiredAccess:DWORD, dwShareMode:DWORD,");
            sbASM.Append(" lpSecurityAttributes:DWORD, dwCreationDisposition:DWORD, dwFlagsAndAttributes:DWORD, hTemplateFile:DWORD\n");
            sbASM.Append("WriteFile PROTO, hFile:DWORD, lpBuffer:DWORD, nNumberOfBytesToWrite:DWORD, lpNumberOfBytesWritten:DWORD, lpOverlapped:DWORD");
            sbASM.AppendLine("ExitProcess PROTO, dwExitCode:DWORD");
            sbASM.Append("CreateThread PROTO, lpThreadAttributes:DWORD, dwStackSize:DWORD,");
            sbASM.Append("lpStartAddress:DWORD, lpParameter:DWORD, dwCreationFlags:DWORD, lpThreadID:DWORD\n");
            sbASM.Append(".data \n path byte ");
            sbASM.AppendLine(sDataFile);
            sbASM.AppendLine(".code\nmain:");

            string[] ops = { "push", "pop", "mov", "lea", "sub", "add", "xor" };
            string[] regs = { "ebp", "esp", "eax", "edx", "edi", "esi" };

            string code = "A a C a,b A 0 E b,4 C c,a E c,8 A c A 0 C f,a E f,4 A f";
            string ASM = ParseASM(code);
            sbASM.AppendLine(ASM);
            sbASM.AppendLine("lea eax, monitort");

            

            return sbASM;
        }

        static string ParseASM(string code)
        {
            StringBuilder sbASMSegment = new StringBuilder();
            string[] ops = { "push", "pop", "mov", "lea", "sub", "add", "xor" };
            string[] regs = { "ebp", "esp", "eax", "edx", "edi", "esi" };
            for (int i = 0; i < code.Length; i++) {
                //if (' ' == code[i]) continue;
                //if (',' == code[i]) {
                //    sbASMSegment.Append(",");
                //    continue;
                //}
                if ((code[i] & 64) != 0) {
                    if ((code[i] & 32) == 0) {
                        sbASMSegment.Append("\n" + ops[code[i] - 0x41]);
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

