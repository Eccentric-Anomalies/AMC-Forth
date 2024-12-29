// Forth Data and Return Stacks and Utility Methods

using System;
using Godot;

namespace Forth
{
    public partial class Files : Godot.RefCounted
    {
        //# Create with a reference to AMCForth
        protected AMCForth Forth;
        protected RAM Ram;

        public void Initialize(AMCForth forth)
        {
            Forth = forth;
            Ram = Forth.Ram;
        }

        // File access
        // map Forth fileid to FileAccess objects
        // file_id is the address of the file's buffer structure
        // the first cell in the structure is the file access mode bits
        //protected Dictionary _FileIdDict = new Dictionary{};
        protected System.Collections.Generic.Dictionary<int, Godot.FileAccess> _FileIdDict = new();

        // allocate a buffer for the provided file handle and mode
        // return the file id or zero if none available
        public int AssignFileId(Godot.FileAccess file, int new_mode)
        {
            for (int i = 0; i < Map.FileBuffQty; i++)
            {
                var addr = i * Map.FileBuffSize + Map.FileBuffStart;
                var mode = Ram.GetInt(addr + Map.FileBuffIdOffset);
                if (mode == 0)
                {
                    // available file handle
                    Ram.SetInt(addr + Map.FileBuffIdOffset, new_mode);
                    Ram.SetInt(addr + Map.FileBuffPtrOffset, 0);
                    _FileIdDict[addr] = file;
                    return addr;
                }
                addr += Map.FileBuffSize;
            }
            return 0;
        }

        public Godot.FileAccess GetFileFromId(int id)
        {
            if (_FileIdDict.ContainsKey(id))
            {
                return _FileIdDict[id];
            }
            else
            {
                return null;
            }
        }

        // releases an file buffer, and closes the associated file, if open
        public void FreeFileId(int id)
        {
            var file = _FileIdDict[id];
            if (file.IsOpen())
            {
                file.Close();
            }

            // clear the buffer entry
            Ram.SetInt(id + Map.FileBuffIdOffset, 0);

            // erase the dictionary entry
            _FileIdDict.Remove(id);
        }

        public void CloseAllFiles()
        {
            foreach (int id in _FileIdDict.Keys)
            {
                FreeFileId(id);
            }
        }
    }
}
