using Godot;

// Forth FILE word set

namespace Forth.File
{
    [GlobalClass]
    public partial class FileSet : Godot.RefCounted
    {
        public CloseFile CloseFile;
        public Included Included;
        public IncludeFile IncludeFile;
        public OpenFile OpenFile;
        public RO RO;
        public RW RW;
        public ReadLine ReadLine;
        public WO WO;
        private const string Wordset = "FILE";

        public FileSet(AMCForth _forth, Stack stack)
        {
            CloseFile = new(_forth, stack, Wordset);
            Included = new(_forth, stack, Wordset);
            IncludeFile = new(_forth, stack, Wordset);
            OpenFile = new(_forth, stack, Wordset);
            RO = new(_forth, stack, Wordset);
            RW = new(_forth, stack, Wordset);
            ReadLine = new(_forth, stack, Wordset);
            WO = new(_forth, stack, Wordset);
        }
    }
}
