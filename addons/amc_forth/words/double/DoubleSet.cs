using Godot;

// Forth STRING word set

namespace Forth.Double
{
    [GlobalClass]
    public partial class DoubleSet : Godot.RefCounted
    {
        public DDot DDot;
        public DMinus DMinus;
        public DPlus DPlus;
        public DLessThan DLessThan;
        public DEquals DEquals;
        public DZeroLess DZeroLess;
        public DZeroEqual DZeroEqual;
        public DTwoStar DTwoStar;
        public DTwoSlash DTwoSlash;
        public DToS DToS;
        public DAbs DAbs;
        public DMax DMax;
        public DMin DMin;
        public DNegate DNegate;
        public MStarSlash MStarSlash;
        public MPlus MPlus;
        public TwoConstant TwoConstant;
        public TwoLiteral TwoLiteral;
        public TwoVariable TwoVariable;
        private const string Wordset = "DOUBLE";

        public DoubleSet(AMCForth _forth, Stack stack)
        {
            DDot = new(_forth, stack, Wordset);
            DMinus = new(_forth, stack, Wordset);
            DPlus = new(_forth, stack, Wordset);
            DLessThan = new(_forth, stack, Wordset);
            DEquals = new(_forth, stack, Wordset);
            DZeroLess = new(_forth, stack, Wordset);
            DZeroEqual = new(_forth, stack, Wordset);
            DTwoStar = new(_forth, stack, Wordset);
            DTwoSlash = new(_forth, stack, Wordset);
            DToS = new(_forth, stack, Wordset);
            DAbs = new(_forth, stack, Wordset);
            DMax = new(_forth, stack, Wordset);
            DMin = new(_forth, stack, Wordset);
            DNegate = new(_forth, stack, Wordset);
            MStarSlash = new(_forth, stack, Wordset);
            MPlus = new(_forth, stack, Wordset);
            TwoConstant = new(_forth, stack, Wordset);
            TwoLiteral = new(_forth, stack, Wordset);
            TwoVariable = new(_forth, stack, Wordset);
        }
    }
}
