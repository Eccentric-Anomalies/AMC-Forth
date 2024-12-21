using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class SQuote : Forth.Words
    {
        public SQuote(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "S\"";
            Description =
                "Return the address and length of the following string, terminated by \", "
                + "which is in a temporary buffer.";
            StackEffect = "( 'string' - c-addr u )";
            Immediate = true;
        }

        public override void Call()
        {
            Stack.Push("\"".ToAsciiBuffer()[0]);
            Forth.CoreExtWords.Parse.Call();
            var l = Stack.Pop();
            var src = Stack.Pop();
            if (Forth.State) // different compilation behavior
            {
                Forth.Ram.SetInt(Forth.DictTopP, XtX);
                Forth.DictTopP += RAM.CellSize; // store the value
                Forth.Ram.SetByte(Forth.DictTopP, l); // store the length
                Forth.DictTopP += 1; // beginning of string characters
                // compile the string into the dictionary
                for (int i = 0; i < l; i++)
                {
                    Forth.Ram.SetByte(Forth.DictTopP, Forth.Ram.GetByte(src + i));
                    Forth.DictTopP += 1;
                }
                Forth.CoreWords.Align.Call(); // this will align the dict top and save it
            }
            else
            {
                for (int i = 0; i < l; i++) // just copy it at the end of the dictionary as a temporary area
                {
                    Forth.Ram.SetByte(Forth.DictTopP + i, Forth.Ram.GetByte(src + i));
                }
                // push the return values back on
                Stack.Push(Forth.DictTopP);
                Stack.Push(l);
            }
        }

        public override void CallExec()
        {
            var l = Forth.Ram.GetByte(Forth.DictIp + RAM.CellSize);
            Stack.Push(Forth.DictIp + RAM.CellSize + 1); // address of the string start
            Stack.Push(l); // length of the string
            // moves to string cell for l in 0..3, then one cell past for l in 4..7, etc.
            Forth.DictIp += ((l / RAM.CellSize) + 1) * RAM.CellSize;
        }
    }
}
