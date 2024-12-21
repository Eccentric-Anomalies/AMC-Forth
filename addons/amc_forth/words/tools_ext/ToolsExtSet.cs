using Godot;

// Forth TOOLS EXT word set

namespace Forth.ToolsExt
{
    [GlobalClass]
    public partial class ToolsExtSet : Godot.RefCounted
    {
        public Ahead Ahead;
        public CsPick CsPick;
        public CsRoll CsRoll;
        private const string Wordset = "TOOLS EXT";

        public ToolsExtSet(AMCForth _forth, Stack stack)
        {
            Ahead = new(_forth, stack, Wordset);
            CsPick = new(_forth, stack, Wordset);
            CsRoll = new(_forth, stack, Wordset);
        }
    }
}
