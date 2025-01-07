using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class ParseName : Forth.Words
    {
        public ParseName(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "PARSE-NAME";
            Description = "Skip leading space delimiters. Parse name delimited by space.";
            StackEffect = "( name - c-addr u )";
        }

        public override void Call()
        {
            Stack.Push(Terminal.BL.ToAsciiBuffer()[0]);
            Forth.CoreWords.Word.Call();
            Forth.CoreWords.Count.Call();
        }
    }
}
