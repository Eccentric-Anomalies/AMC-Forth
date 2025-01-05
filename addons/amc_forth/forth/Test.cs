using System;
using Godot;

// Utility for running Forth test suite and saving output

namespace Forth
{
    [GlobalClass]
    public partial class Test : Node
    {
        private FileAccess outFile;
        private AMCForth _Forth;
        private SceneTreeTimer timer;

        public override void _Ready()
        {
            base._Ready();
            RunTests();
        }

        public void RunTests()
        {
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
            outFile.StoreString(outText);
            timer.Timeout -= TimerExpired;
            StartTimer();
        }

        protected void TimerExpired()
        {
            outFile.Close();
            _Forth.Cleanup();
            QueueFree();
        }
    }
}
