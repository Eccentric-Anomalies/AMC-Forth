using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Word : Forth.Words
    {
        public Word(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "WORD";
            Description =
                "Skip leading occurrences of the delimiter char. Parse text delimited by"
                + " char. Return the address of a temporary location containing the passed text as a"
                + " counted string.";
            StackEffect = "( char - c-addr )";
        }

        public override void Call()
        {
            Forth.CoreWords.Dup.Call();
            var delim = Stack.Pop();
            Forth.CoreWords.Source.Call();
            var source_size = Stack.Pop();
            var source_start = Stack.Pop();
            Forth.CoreWords.ToIn.Call();
            var ptraddr = Stack.Pop();
            while (true)
            {
                var t = Forth.Ram.GetByte(source_start + Forth.Ram.GetInt(ptraddr));
                if (t == delim)
                {
                    // increment the input pointer
                    Forth.Ram.SetInt(ptraddr, Forth.Ram.GetInt(ptraddr) + 1);
                }
                else
                {
                    break;
                }
            }
            Forth.CoreExtWords.Parse.Call();
            var count = Stack.Pop();
            var straddr = Stack.Pop();
            var ret = straddr - 1;
            Forth.Ram.SetByte(ret, count);
            Stack.Push(ret);
        }
    }
}
