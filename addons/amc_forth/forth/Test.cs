using System;
using System.Text;
using Godot;

// Utility for running Forth test suite and saving output

namespace Forth
{
    [GlobalClass]
    public partial class Test : Node
    {
        private const string header_text = "WORDS TESTED FOR AMCFORTH";
        private FileAccess outFile;
        private AMCForth _Forth;
        private SceneTreeTimer timer;
        private StringBuilder buffer;
        private bool header_found;

        public override void _Ready()
        {
            base._Ready();
            RunTests();
        }

        public void RunTests()
        {
            buffer = new();
            header_found = false;
            _Forth = new();
            _Forth.Initialize(this);
            outFile = FileAccess.Open(
                "res://addons/amc_forth/tests/tests_out.txt",
                FileAccess.ModeFlags.Write
            );
            _Forth.ClientConnected();
            StartTimer();
            _Forth.TerminalIn("INCLUDE addons/amc_forth/tests/tests.fth" + Terminal.CR);
            _Forth.TerminalOut += OutputHandler;
        }

        protected void StartTimer()
        {
            timer = GetTree().CreateTimer(1);
            timer.Timeout += TimerExpired;
        }

        protected void OutputHandler(string outText)
        {
            buffer.Append(outText);
            if (! header_found && buffer.ToString().EndsWith(header_text))
            {
                header_found = true;
                buffer = new(header_text);
            }
            if (header_found)
            {
                outFile.StoreString(buffer.ToString());
                timer.Timeout -= TimerExpired;
                StartTimer();
                buffer.Clear();                
            }
        }

        protected void TimerExpired()
        {
            outFile.Close();
            _Forth.Cleanup();
            QueueFree();
        }
    }
}
