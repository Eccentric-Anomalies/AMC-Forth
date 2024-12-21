using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class Nip : Forth.Words
    {
        public Nip(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "NIP";
            Description = "Drop second stack item, leaving top unchanged.";
            StackEffect = "( x1 x2 - x2 )";
        }

        public override void Call()
        {
            Forth.CoreWords.Swap.Call();
            Forth.CoreWords.Drop.Call();
        }
    }
}
