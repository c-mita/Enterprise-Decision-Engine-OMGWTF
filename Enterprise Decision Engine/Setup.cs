using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Enterprise_Decision_Engine
{
    static class Setup
    {
        static bool IsReady()
        {
            return File.Exists("engine\\rng.exe");
        }

        static public bool SetupRNG(string sDataFile)
        {
            /*I do not believe it's come to this
             *The PM _insists_ the functionality and behaviour of that ancient POS is preserved _exactly_
             *Well I'll do that. And those other idiots won't have a fucking clue how this works.
            */
            Assemble(sDataFile);

            return true;

        }

        static bool Assemble(string sDataFile)
        {
            string sDir = Directory.GetCurrentDirectory();
            sDir = sDir.TrimEnd('\\');
            bool gcc = false;
            string sMSVSPath = "";
            sMSVSPath = MSVSPath();
            //sMSVSPath = "";
            string sgccPath = "";
            if ("" == sMSVSPath) { 
                sgccPath = GCCPath();
                if ("" == sgccPath) {
                    return false; 
                }
                gcc = true;
            }
            StreamWriter streamASM = File.CreateText(gcc ? sDir+"\\engine\\RNG.s" : sDir+"\\engine\\RNG.asm");
            streamASM.Write(GetASM(sDir + "\\" + sDataFile,gcc).ToString());
            streamASM.Close();

            Process proc = new Process();
            proc.StartInfo.FileName = gcc ? sgccPath +"\\bin\\gcc.exe" : "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;

            if (!gcc) {
                proc.StartInfo.Arguments = "/k \"" + sMSVSPath + "\\VC\\vcvarsall.bat\"";
                /*Neither my manager nor CTO have VS on their machines. 
                 * But I think I can get gcc on them*/
                proc.Start();
                StreamWriter streamIn = proc.StandardInput;
                string ml = "ml /c /Fo\"" + sDir + "\\engine\\RNG.obj\" \"" + sDir + "\\engine\\RNG.asm\"";
                string link = "link /OUT:\"" + sDir + "\\engine\\RNG.exe\" /ENTRY:\"main\" \"" + sDir + "\\engine\\RNG.obj\"";
                streamIn.WriteLine(ml);
                streamIn.WriteLine(link);
                streamIn.Close();

            } else {
                proc.StartInfo.Arguments = "\"" + sDir + "\\engine\\RNG.s\" -o \"" + sDir + "\\engine\\RNG.exe\"";
                proc.Start();
            }
            proc.WaitForExit();
            proc.Close();
            File.Delete(gcc ? sDir+"\\engine\\RNG.s" : sDir+"\\engine\\RNG.asm");
            return true;
        }

        static string MSVSPath()
        {
            string[] paths = { "C:\\", "C:\\Program Files (x86)", "C:\\Program Files" };
            foreach(string sBase in paths){
                if (!Directory.Exists(sBase)) continue;
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(sBase));
                foreach(string sDir in dirs){
                    string sDirT = sDir.TrimEnd('\\');
                    sDirT = sDirT.Substring(sDirT.LastIndexOf('\\'));
                    if(sDirT.ToLower().Contains("microsoft visual studio")){
                        if (File.Exists(sDir+"\\VC\\vcvarsall.bat") 
                            && File.Exists (sDir + "\\VC\\bin\\ml.exe")
                            && File.Exists(sDir + "\\VC\\bin\\link.exe")) return sDir;
                    }
                }
            }
            return "";
        }
        
        static string GCCPath()
        {
            string[] paths = { "C:\\", "C:\\Program Files (x86)", "C:\\Program Files" };
            foreach (string sBase in paths) {
                if (!Directory.Exists(sBase)) continue;
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(sBase));
                foreach (string sDir in dirs) {
                    string sDirT = sDir.TrimEnd('\\');
                    sDirT = sDirT.Substring(sDirT.LastIndexOf('\\'));
                    if (sDirT.ToLower().Contains("mingw")) {
                        if (File.Exists(sDir + "\\bin\\gcc.exe")) return sDir;
                    }
                }
            }
            return "";
        }

        public static StringBuilder GetASM(string sDataFile, bool gcc)
        {
            StringBuilder sbASM = new StringBuilder(2048);
            if (gcc) {
                sbASM.AppendLine(".intel_syntax");
                sbASM.AppendLine(".extern _FindFirstFileA@8");
                sbASM.AppendLine(".extern _CreateFileA@28");
                sbASM.AppendLine(".extern _WriteFile@20");
                sbASM.AppendLine(".extern _ExitProcess@4");
                sbASM.AppendLine(".extern _CreateThread@24");
                sbASM.Append(".data\r\n_path: .ascii \"");
                sbASM.Append(sDataFile.Replace("\\","\\\\"));
                sbASM.AppendLine("\\0\"");
                sbASM.AppendLine(".global _main\r\n_main:");
            } else {
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
                sbASM.Append('\'');
                sbASM.Append(sDataFile);
                sbASM.AppendLine("\',0");
                sbASM.AppendLine(".code\r\nmain:");
            }

            //string[] ops = { "push", "pop", "mov", "lea", "sub", "add", "xor" };
            //string[] regs = { "ebp", "esp", "eax", "edx", "edi", "esi", "" , "h" };
            string code = "A a C a,b A 0 E b,4 C c,a E c,8 A c A 0 C f,a E f,4 A f";
            string ASM = ParseASM(code,gcc);
            sbASM.AppendLine(ASM);
            sbASM.AppendLine("lea eax, monitort");
            code = "A c A 0 A 0 G c,c";
            sbASM.AppendLine(ParseASM(code,gcc));
            sbASM.AppendLine(gcc ? "call _CreateThread@24" : "call CreateThread");
            sbASM.AppendLine("loopstart:\r\ninc eax");
            sbASM.AppendLine("mov edi,[esi]\r\ncmp edi,1\r\njnz loopstart");
            code = "C e,c A 0 A x80h A 2 A 0 A 7 A x102h";
            sbASM.AppendLine(ParseASM(code,gcc));
            sbASM.Append("push offset ");
            sbASM.Append(gcc ? "_path\r\ncall " : "path\r\ncall ");
            sbASM.AppendLine(gcc ? "_CreateFileA@28" : "CreateFileA");
            code = "E b,4 C d,b F d,4 A 0 A d A 1 C [f],e A f A c";
            sbASM.AppendLine(ParseASM(code,gcc));
            sbASM.AppendLine(gcc ? "call _WriteFile@20" : "call WriteFile");
            sbASM.AppendLine("add esp,4\r\npush 0");
            sbASM.AppendLine(gcc ? "call _ExitProcess@4" : "call ExitProcess");
            sbASM.AppendLine("monitort:");
            code = "A a C a,b E b,x140h C e,b A f C f,[a+8]";
            sbASM.AppendLine(ParseASM(code,gcc));
            sbASM.Append("cont:\r\npush edi\r\npush offset ");
            //sbASM.Append(gcc ? "_path\r\ncall " : "path\r\ncall " );
            sbASM.AppendLine(gcc ? "_path\r\ncall _FindFirstFileA@8" : "path\r\ncall FindFirstFileA");
            sbASM.AppendLine("cmp eax,-1\r\njz cont");
            code = "C c,1 C [f],c G c,c C f,[a-x144h] F b,x144h C b,a B a";
            sbASM.AppendLine(ParseASM(code,gcc));
            sbASM.AppendLine("ret");
            if (!gcc) sbASM.AppendLine("end main");
            /*System.IO.StreamWriter stream = new System.IO.StreamWriter("testfile.txt");
            stream.Write(sbASM.ToString());
            stream.Close();*/
            return sbASM;
        }

        static string ParseASM(string code, bool gcc)
        {
            StringBuilder sbASMSegment = new StringBuilder();
            string[] ops = { "push", "pop", "mov", "lea", "sub", "add", "xor" };
            string[] regs = { "ebp", "esp", "eax", "edx", "edi", "esi","","h" };
            for (int i = 0; i < code.Length; i++) {
                if (0x68 == code[i]) {
                    if (gcc) continue;
                }
                if (0x78 == code[i]) {
                    if (gcc) {
                        sbASMSegment.Append("0x");
                    }
                    continue;
                }
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

