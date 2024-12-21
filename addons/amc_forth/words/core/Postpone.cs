using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Postpone : Forth.Words
    {
        public Postpone(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "POSTPONE";
            Description =
                "At compile time, add the compilation behavior of the following "
                + "name, rather than its execution behavior.";
            StackEffect = "( 'name' - )";
            Immediate = true;
        }

        public override void Call()
        {
            Forth.CoreExtWords.ParseName.Call(); // parse for the next token
            var len = Stack.Pop();
            var caddr = Stack.Pop();
            var word = Forth.Util.StrFromAddrN(caddr, len);
            // obtain and push the compile time xt for this word
            Stack.Push(FromName(word).Xt);
            Forth.CoreWords.Comma.Call(); // then store it in the current definition
        }
    }
}
