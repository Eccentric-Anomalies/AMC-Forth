using Godot;

// Forth FILE EXT word set

namespace Forth.FileExt
{
    [GlobalClass]
    public partial class FileExtSet : Godot.RefCounted
    {
        public FileStatus FileStatus;
        public Include Include;
        private const string Wordset = "FILE EXT";

        public FileExtSet(AMCForth _forth)
        {
            FileStatus = new(_forth, Wordset);
            Include = new(_forth, Wordset);
        }
    }
}
