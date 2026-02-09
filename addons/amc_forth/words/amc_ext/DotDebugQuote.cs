using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class DotDebugQuote : Forth.Words
    {
        public DotDebugQuote(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = ".D\"";
            Description = "Print the string to console when the containing word is executed.";
            StackEffect = "( 'string' - c-addr u )";
            Immediate = true;
        }

        public override void Call()
        {
            // compilation behavior
            if (Forth.State)
            {
                Stack.Push("\"".ToAsciiBuffer()[0]);
                Forth.CoreExtWords.Parse.Call();
                // copy the execution token
                Forth.Ram.SetInt(Forth.DictTopP, XtX);
                var l = Stack.Pop();
                var src = Stack.Pop();
                Forth.DictTopP += RAM.CellSize;
                Forth.Ram.SetByte(Forth.DictTopP, l); // store the length
                Forth.DictTopP += 1;
                // compile the string into the dictionary
                for (int i = 0; i < l; i++)
                {
                    Forth.Ram.SetByte(Forth.DictTopP, Forth.Ram.GetByte(src + i));
                    Forth.DictTopP += 1;
                }
                // this will align the dict top and save it
                Forth.CoreWords.Align.Call();
            }
        }

        public override void CallExec()
        {
            var l = Forth.Ram.GetByte(Forth.DictIp + RAM.CellSize);
            var s = Forth.DictIp + RAM.CellSize + 1;
            // send to the console
            List<byte> bytes = [];
            for (int i = 0; i < l; i++)
            {
                bytes.Add((byte)Forth.Ram.GetByte(s + i));
            }
            GD.Print(System.Text.Encoding.ASCII.GetString([.. bytes]));
            // moves to string cell for l in 0..3, then one cell past for l in 4..7, etc.
            Forth.DictIp += ((l / RAM.CellSize) + 1) * RAM.CellSize;
        }
    }
}
