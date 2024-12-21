using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DNegate : Forth.Words
    {
        public DNegate(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "DNEGATE";
            Description = "Change the sign of the top stack value.";
            StackEffect = "( d - -d )";
        }

        public override void Call()
        {
            Stack.SetDint(0, -Stack.GetDint(0));
        }
    }
}
