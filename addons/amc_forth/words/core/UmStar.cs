using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class UmStar : Forth.Words
    {
        public UmStar(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "UM*";
            Description = "Multiply u1 by u2, leaving the double-precision result ud.";
            StackEffect = "( u1 u2 - ud )";
        }

        public override void Call()
        {
            Stack.PushDword((ulong)Stack.Pop() * (ulong)Stack.Pop());
        }
    }
}
