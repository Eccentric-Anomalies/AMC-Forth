using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class RShift : Forth.Words
    {
        public RShift(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "RSHIFT";
            Description =
                "Perform a logical right shift of u places on x1, giving x2. "
                + "Fill the vacated MSB bits with zeros.";
            StackEffect = "( x1 u - x2 )";
        }

        public override void Call()
        {
            var u = Stack.Pop();
            Stack.DataStack[Stack.DsP] = (int)(((uint)Stack.DataStack[Stack.DsP]) >> u);
        }
    }
}
