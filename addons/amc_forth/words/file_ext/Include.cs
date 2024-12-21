using Godot;

namespace Forth.FileExt
{
    [GlobalClass]
    public partial class Include : Forth.Words
    {
        public Include(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "INCLUDE";
            Description = "Parse the following word and use as file name with INCLUDED.";
            StackEffect = "( i*x 'filename' - j*x )";
        }

        public override void Call()
        {
            Forth.CoreExtWords.ParseName.Call();
            Forth.FileWords.Included.Call();
        }
    }
}
