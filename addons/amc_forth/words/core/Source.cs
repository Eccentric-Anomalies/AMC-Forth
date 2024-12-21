using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Source : Forth.Words
    {
        public Source(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "SOURCE";
            Description = "Return the address and length of the input buffer.";
            StackEffect = "( - c-addr u )";
        }

        public override void Call()
        {
            if (Forth.SourceId == -1)
            {
                Stack.Push(Map.BuffSourceStart);
                Stack.Push(Map.BuffSourceSize);
            }
            else if (Forth.SourceId != 0)
            {
                Stack.Push(Forth.SourceId + Map.FileBuffDataOffset);
                Stack.Push(Map.FileBuffDataSize);
            }
        }
    }
}
