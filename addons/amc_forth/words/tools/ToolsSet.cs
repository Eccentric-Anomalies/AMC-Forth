using Godot;

// Forth TOOLS word set

namespace Forth.Tools
{
    [GlobalClass]
    public partial class ToolsSet : Godot.RefCounted
    {
        public Question Question;
        public DotS DotS;
        public Tools.Words Words;
        private const string Wordset = "TOOLS";

        public ToolsSet(AMCForth _forth, Stack stack)
        {
            Question = new(_forth, stack, Wordset);
            DotS = new(_forth, stack, Wordset);
            Words = new(_forth, stack, Wordset);
        }
    }
}
