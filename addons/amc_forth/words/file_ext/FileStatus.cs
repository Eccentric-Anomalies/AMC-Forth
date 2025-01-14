using Godot;

namespace Forth.FileExt
{
    [GlobalClass]
    public partial class FileStatus : Forth.Words
    {
        public FileStatus(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "FILE-STATUS";
            Description =
                "Get status of the file whose name is given by c-addr of length u. "
                + "Return ior zero if the file exists. Value x is 1 if the file exists "
                + " in user:// and 0 if in res://.";
            StackEffect = "(c-addr u - x ior )";
        }

        public override void Call()
        {
            var ior = 0; // file exists
            var x = 1; // in user://
            var u = Stack.Pop();
            var fname = Forth.Util.StrFromAddrN(Stack.Pop(), u);
            var fexists = FileAccess.FileExists("user://" + fname);
            if (!fexists)
            {
                x = 0; // in res://
                fexists = FileAccess.FileExists("res://" + fname);
                if (!fexists)
                {
                    ior = 1; // file not found
                }
            }
            Stack.Push(x);
            Stack.Push(ior);
        }
    }
}
