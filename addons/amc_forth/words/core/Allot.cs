using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Allot : Forth.Words
    {
        public Allot(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "ALLOT";
            Description = "Allocate u bytes of data space beginning at the next location.";
            StackEffect = "( u - )";
        }

        public override void Call()
        {
            Forth.DictTopP += Stack.Pop();
            Forth.SaveDictTop(); // preserve dictionary state
        }
    }
}
