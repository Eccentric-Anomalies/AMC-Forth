using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class DotQuote : Forth.Words
    {
        public DotQuote(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = ".\"";
            Description = "Type the string when the containing word is executed.";
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
            Stack.Push(Forth.DictIp + RAM.CellSize + 1);
            // address of the string start
            Stack.Push(l);
            // length of the string
            // send to the terminal
            Forth.CoreWords.TypeF.Call();
            // moves to string cell for l in 0..3, then one cell past for l in 4..7, etc.
            Forth.DictIp += ((l / RAM.CellSize) + 1) * RAM.CellSize;
        }
    }
}
