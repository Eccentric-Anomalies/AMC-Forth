using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Here : Forth.Words
    {
        public Here(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "HERE";
            Description = "Return address of the next available location in data-space.";
            StackEffect = "( - addr )";
        }

        public override void Call()
        {
            Stack.Push(Forth.DictTopP);
        }
    }
}
