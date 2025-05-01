using System;
using Godot;

namespace Forth
{
    public partial class Map : Godot.RefCounted
    {
        // Memory Map

        // Memory Map Version - increment for incompatible changes!
        public const int Version = 1;

        // DICTIONARY SPACE
        public const int DictSize = 0x020000; // total RAM allocated to the Forth machine (128k)
        public const int DictStart = 0x0100; // Start of dictionary memory
        public const int DictTop = DictStart + DictSize; // top of the dictionary space

        // Dictionary scratch space
        public const int DictBuffSize = 0x040;
        public const int DictBuffStart = DictTop;
        public const int DictBuffTop = DictBuffStart + DictBuffSize;

        // INPUT BUFFER
        public const int BuffSourceSize = 0x0100; // bytes
        public const int BuffSourceStart = DictBuffTop;
        public const int BuffSourceTop = BuffSourceStart + BuffSourceSize;

        // FILE BUFFERS
        public const int FileBuffQty = 8; // number of simultaneous open files possible
        public const int FileBuffIdOffset = 0; // offset in buffer to fileid
        public const int FileBuffPtrOffset = RAM.CellSize; // location of buffer pointer
        public const int FileBuffDataOffset = RAM.CellSize * 3; // Leave a cell between buff ptr and data
        public const int FileBuffSize = 0x0100; // each buffer, bytes overall
        public const int FileBuffDataSize = FileBuffSize - FileBuffDataOffset; // net bytes available
        public const int FileBuffStart = BuffSourceTop;
        public const int FileBuffTop = FileBuffStart + FileBuffSize * FileBuffQty; // TOP of the file buffers

        // POINTER to parse position in the TERMINAL buffer
        public const int BuffToIn = FileBuffTop;
        public const int BuffToInTop = BuffToIn + RAM.CellSize;

        // BASE keeps the current base (typ. 10 or 16)
        public const int Base = BuffToInTop;

        // DICT_TOP_PTR cell keeps track of the top of dictionary entry
        public const int DictTopPtr = Base + RAM.CellSize;

        // DICT_PTR cell keeps track of the growing top of dictionary data
        public const int DictPtr = DictTopPtr + RAM.CellSize;

        // NUMBER FORMAT BUFFER for constructing formatted number strings
        public const int NumFormatBuffSize = 0x80;
        public const int NumFormatBuffer = DictPtr + RAM.CellSize;

        // TOP OF ALLOCATED RAM for dictionary and assorted buffers (update as necessary)
        // PeriodicStart must be greater than TopOfAllocatedRam
        public const int TopOfAllocatedRam = NumFormatBuffer + NumFormatBuffSize;

        // Dedicated IO Port and Timer data follows RAM

        // IO SPACE - permanently placed at the top of memory space
        // IO Ports - cell-sized ports identified by port # ranging from 0 to 255

        // Timer IDs 0-127, stored as @addr: msec, xt

        // PERIODIC TIMER SPACE
        public const int PeriodicTimerQty = 0x080;

        // Arrange to place the following on a page boundary
        public const int PeriodicStart = (int)(((uint)TopOfAllocatedRam + 0x100) & 0xFFFFFF00);
        public const int PeriodicTop = PeriodicStart + PeriodicTimerQty * RAM.CellSize * 2;

        // INPUT PORT SPACE
        public const int IoInPortQty = 0x0100;
        public const int IoInStart = PeriodicTop;
        public const int IoInTop = IoInStart + IoInPortQty * RAM.CellSize;

        // (xt, QueueMode) for every port that is being listened on (double cell entries)
        public const int IoInMapStart = IoInTop;
        public const int IoInMapTop = IoInMapStart + IoInPortQty * 2 * RAM.CellSize;

        // OUTPUT PORT SPACE
        public const int IoOutPortQty = 0x0100;
        public const int IoOutStart = IoInMapTop;
        public const int IoOutTop = IoOutStart + IoOutPortQty * RAM.CellSize;

        public const int RamSize = IoOutTop; // Total memory space in bytes
    }
}
