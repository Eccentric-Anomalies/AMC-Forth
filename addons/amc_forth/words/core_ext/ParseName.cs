using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class ParseName : Forth.Words
    {
        public ParseName(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "PARSE-NAME";
            Description = "If the data-space pointer is not aligned, reserve space to align it.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Stack.Push(Terminal.BL.ToAsciiBuffer()[0]);
            Forth.CoreWords.Word.Call();
            Forth.CoreWords.Count.Call();
        }
    }
}
