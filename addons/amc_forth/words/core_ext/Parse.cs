using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class Parse : Forth.Words
    {
        public Parse(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "PARSE";
            Description =
                "Parse text to the first instance of char, returning the address "
                + "and length of a temporary location containing the parsed text. "
                + "Returns a counted string. Consumes the final delimiter.";
            StackEffect = "( char - c_addr n )";
        }

        public override void Call()
        {
            var count = 0;
            var ptr = Map.WordBuffStart + 1;
            var delim = Stack.Pop();
            Forth.CoreWords.Source.Call();
            var source_size = Stack.Pop();
            var source_start = Stack.Pop();
            Forth.CoreWords.ToIn.Call();
            var ptraddr = Stack.Pop();
            Stack.Push(ptr);
            // parsed text begins here
            while (true)
            {
                var t = Forth.Ram.GetByte(source_start + Forth.Ram.GetInt(ptraddr));
                // increment the input pointer
                if (t != 0)
                {
                    Forth.Ram.SetInt(ptraddr, Forth.Ram.GetInt(ptraddr) + 1);
                }
                // a null character also stops the parse
                if (t != 0 && t != delim)
                {
                    Forth.Ram.SetByte(ptr, t);
                    ptr += 1;
                    count += 1;
                }
                else
                {
                    break;
                }
            }
            Stack.Push(count);
        }
    }
}
