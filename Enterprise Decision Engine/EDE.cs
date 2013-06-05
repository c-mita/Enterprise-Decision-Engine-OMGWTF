using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Enterprise_Decision_Engine
{
    static class EDE
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EDEForm());
        }

        static public int RNG()
        {
            Directory.CreateDirectory("engine");
            Setup.SetupRNG("engine\\test.txt");
            //Setup.GetASM("C:\\testfile\\test.txt");
            if (File.Exists("engine\\test.txt")) {
                File.Delete("engine\\test.txt");
            }
            
            ProcessStartInfo info = new ProcessStartInfo("engine\\RNG.exe");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            System.Threading.Thread thread = new System.Threading.Thread(GenerateFile);
            Process proc = Process.Start(info);
            thread.Start();
            proc.WaitForExit(int.MaxValue);
            thread.Join();
            FileStream stream = File.Open("engine\\test.txt",FileMode.Open);
            int n = stream.ReadByte();
            stream.Close();
            return (n & 1) ^ 1; //this was getting inverted somewhere, so I just flipped it here - Tom
        }

        static void GenerateFile()
        {
            System.Threading.Thread.Sleep(100);
            File.Create("engine\\test.txt").Close();
            /*TODO - Fix race condition!*/
        }

        
    }

    public class Option
    {
        #region "vars"
        private char p_symbols;
        private bool p_active;
        private bool p_sign;
        #endregion
        #region "properties"
        public string Symbol
        {
            get
            {
                switch (p_symbols) {
                    case 'N':
                        //return sign ? "1" : "0";
                        return p_sign ? "0" : "1";
                    case 'C':
                        return p_sign ? "Heads" : "Tails";
                    case 'H':
                        return p_sign ? "Left" : "Right";
                    case 'B':
                    default:
                        return p_sign ? "True" : "False";
                }
            }
        }
        public bool Sign
        {
            get { return p_sign; }
            private set { p_sign = value; }
        }
        public bool Set
        {
            get { return p_active; }
            private set { p_active = value; }
        }
        #endregion

        /// <summary>
        /// Flags: 2/3 for 1/0, 4/5 for True/False, 8/9 for Heads/Tails, 16/17 for Left/Right.
        /// </summary>
        /// <param name="flags"></param>
        public Option(int flags)
        {
            // - this works! don't touch it! - Tom
            p_sign = ((flags & 1) == 1);
            for (int i = 0; i < 3; i++) {
                if (((2 << i) & flags) != 0) {
                    switch (i) {
                        case 0:
                            p_symbols = 'N';
                            break;
                        case 1:
                            p_symbols = 'B';
                            break;
                        case 2:
                            p_symbols = 'C';
                            break;
                        case 3:
                            p_symbols = 'H';
                            break;
                    }
                }
            }
        }

        public static void ChangeFlags(int flags, Option[] options)
        {
            // - how does this work? - Mark
            // - Miles wrote it so it probably uses some weird C stuff - Alice
            // - its buggy the result is always wrong - Mark

            //options[0].sign = 1 == (0 ^ flags & 1);
            //options[1].sign = 1 == (1 ^ flags & 1);
            //
            //flags &= -2;
            //options[0].p_symbols = (flags & 2) != 0 ? 'N' : (flags & 4) != 0 ? 'B' : (flags & 8) != 0 ? 'C' : 'H';
            //options[1].p_symbols = options[0].p_symbols;


            // - rewrote - Mark
            bool Swap = false;
            foreach (Option Option in options) {
                if ((flags / 2) * 2 == flags) {
                    Option.Sign = false;
                } else {
                    Option.Sign = true;
                }
                if (Swap) Option.Sign = !Option.Sign;
                Swap = true;
                char Type;
                if (flags == 2 || flags == 3) {
                    Type = 'N';
                } else if (flags == 4 || flags == 5) {
                    Type = 'B';
                } else if (flags == 8 || flags == 9) {
                    Type = 'C';
                } else {
                    Type = 'H';
                }
                Option.p_symbols = Type;
            }
        }
    }
}
