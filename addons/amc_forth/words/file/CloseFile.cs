using Godot;

namespace Forth.File
{
    [GlobalClass]
    public partial class CloseFile : Forth.Words
    {
        public CloseFile(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "CLOSE-FILE";
            Description =
                "Close the file identified by fileid. Return an I/O result code. "
                + "Result code, ior, is zero for success.";
            StackEffect = "( fileid - ior )";
        }

        public override void Call()
        {
            Files.FreeFileId(Stack.Pop());
            Stack.Push(0);
        }
    }
}
