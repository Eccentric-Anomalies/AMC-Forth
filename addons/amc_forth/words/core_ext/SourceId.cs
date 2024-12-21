using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class SourceId : Forth.Words
    {
        public SourceId(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "SOURCE-ID";
            Description =
                "Return a value indicating current input source. "
                + "Value is 0 for default user input, -1 for character string.";
            StackEffect = "( - n )";
        }

        public override void Call()
        {
            Stack.Push(Forth.SourceId);
        }
    }
}
