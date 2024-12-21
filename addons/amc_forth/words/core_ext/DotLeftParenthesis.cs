using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class DotLeftParenthesis : Forth.Words
    {
        public DotLeftParenthesis(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = ".(";
            Description =
                "Begin parsing a comment, terminated by ')'. Comment text "
                + "will emit to the terminal when it is compiled.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Stack.Push(")".ToAsciiBuffer()[0]);
            Forth.CoreExtWords.Parse.Call();
            Forth.CoreWords.Type.Call();
        }
    }
}
