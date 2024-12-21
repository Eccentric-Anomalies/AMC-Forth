using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class MStarSlash : Forth.Words
    {
        public MStarSlash(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "M*/";
            Description =
                "Multiply d1 by n1 producing a triple cell intermediate result t. "
                + "Divide t by n2, giving quotient d2.";
            StackEffect = "( d1 n1 +n2 - d2 )";
        }

        public override void Call()
        {
            var n2 = Stack.Pop();
            var n1 = Stack.Pop();
            var d1 = Stack.PopDint();
            var t = (decimal)d1 * n1;
            Stack.PushDint((long)t / n2);
        }
    }
}
