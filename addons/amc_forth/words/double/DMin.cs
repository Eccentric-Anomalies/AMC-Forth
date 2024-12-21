using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DMin : Forth.Words
    {
        public DMin(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "DMIN";
            Description = "Return d3, the lesser of d1 and d2.";
            StackEffect = "( d1 d2 - d3 )";
        }

        public override void Call()
        {
            var d2 = Stack.PopDint();
            if (d2 < Stack.GetDint(0))
            {
                Stack.SetDint(0, d2);
            }
        }
    }
}
