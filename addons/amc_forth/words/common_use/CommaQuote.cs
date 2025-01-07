using Godot;

namespace Forth.CommonUse
{
    [GlobalClass]
    public partial class CommaQuote : Forth.Words
    {
        public CommaQuote(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = ",\"";
            Description =
                "Compile the following string, terminated by double quote, to "
                + "the end of the dictionary.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Stack.Push("\"".ToAsciiBuffer()[0]);
            Forth.CoreExtWords.Parse.Call();
            var l = Stack.Pop();
            var src = Stack.Pop();
            Forth.Ram.SetByte(Forth.DictTopP, l); // store the length
            Forth.DictTopP += 1;
            // compile the string into the dictionary
            for (int i = 0; i < l; i++)
            {
                Forth.Ram.SetByte(Forth.DictTopP, Forth.Ram.GetByte(src + i));
                Forth.DictTopP += 1;
            }
            // preserve dictionary state (not necessarily aligned)
            Forth.SaveDictTop();
        }
    }
}
