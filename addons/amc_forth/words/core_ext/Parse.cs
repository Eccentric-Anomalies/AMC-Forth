using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class Parse : Forth.Words
    {
        public Parse(AMCForth forth, string wordset)
            : base(forth, wordset)
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
            var delim = Stack.Pop();
            // Get the input buffer location
            Forth.CoreWords.Source.Call();
            var source_size = Stack.Pop();
            var source_start = Stack.Pop();
            // Get the input buffer index address
            Forth.CoreWords.ToIn.Call();
            var ptraddr = Stack.Pop();
            // Returned string starts right here
            Stack.Push(source_start + Forth.Ram.GetInt(ptraddr));
            while (true)
            {
                // get the next character in the input buffer
                var t = Forth.Ram.GetByte(source_start + Forth.Ram.GetInt(ptraddr));
                // increment the input pointer on a non-zero character
                if (t != 0)
                {
                    Forth.Ram.SetInt(ptraddr, Forth.Ram.GetInt(ptraddr) + 1);
                }
                // a null character also stops the parse
                if (t != 0 && t != delim)
                {
                    count += 1;
                }
                else
                {
                    break;
                }
            }
            // return found string length
            Stack.Push(count);
        }
    }
}
