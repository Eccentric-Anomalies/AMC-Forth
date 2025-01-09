using System;
using Godot;

namespace Forth
{
    public partial class Map : Godot.RefCounted
    {
        // Memory Map
        public const int RamSize = 0x20000;

        // BYTES
        // Dictionary
        public const int DictStart = 0x0100;

        // BYTES
        public const int DictSize = 0x08000;
        public const int DictTop = DictStart + DictSize;

        // Dictionary scratch space
        public const int DictBuffSize = 0x040;

        // word-sized
        public const int DictBuffStart = DictTop;
        public const int DictBuffTop = DictBuffStart + DictBuffSize;

        // Input Buffer
        public const int BuffSourceSize = 0x0100;

        // bytes
        public const int BuffSourceStart = DictBuffTop;
        public const int BuffSourceTop = BuffSourceStart + BuffSourceSize;

        // File Buffers
        public const int FileBuffQty = 8;

        // number of simultaneous open files possible
        public const int FileBuffIdOffset = 0;

        // offset in buffer to fileid
        public const int FileBuffPtrOffset = RAM.CellSize;

        // location of pointer
        public const int FileBuffDataOffset = RAM.CellSize * 3; // Leave a cell between buff ptr and data

        // location of buff data
        public const int FileBuffSize = 0x0100;

        // bytes, overall
        public const int FileBuffDataSize = FileBuffSize - FileBuffDataOffset;
        public const int FileBuffStart = BuffSourceTop;
        public const int FileBuffTop = FileBuffStart + FileBuffSize * Map.FileBuffQty;

        // Pointer to the parse position in the TERMINAL buffer
        public const int BuffToIn = FileBuffTop;
        public const int BuffToInTop = BuffToIn + RAM.CellSize;

        // BASE cell
        public const int Base = BuffToInTop;

        // DICT_TOP_PTR cell
        public const int DictTopPtr = Base + RAM.CellSize;

        // DICT_PTR
        public const int DictPtr = DictTopPtr + RAM.CellSize;

        // IO SPACE - cell-sized ports identified by port # ranging from 0 to 255
        public const int IoOutPortQty = 0x0100;
        public const int IoOutTop = RamSize;
        public const int IoOutStart = IoOutTop - IoOutPortQty * RAM.CellSize;
        public const int IoInPortQty = 0x0100;
        public const int IoInTop = IoOutStart;
        public const int IoInStart = IoInTop - IoInPortQty * RAM.CellSize;
        public const int IoInMapTop = IoInStart;

        // (xt, QueueMode) for every port that is being listened on (double cell entries)
        public const int IoInMapStart = IoInMapTop - IoInPortQty * 2 * RAM.CellSize;

        // PERIODIC TIMER SPACE
        public const int PeriodicTimerQty = 0x080;

        // Timer IDs 0-127, stored as @addr: msec, xt
        public const int PeriodicTop = IoInStart;

        public const int PeriodicStart = (PeriodicTop - PeriodicTimerQty * RAM.CellSize * 2);
    }
}
