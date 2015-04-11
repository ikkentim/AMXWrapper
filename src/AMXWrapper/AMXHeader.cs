// AMXWrapper
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Runtime.InteropServices;

namespace AMXWrapper
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AMXHeader
    {
        public Int32 size;
        public UInt16 magic;
        public char file_version;
        public char amx_version;
        public Int16 flags;
        public Int16 definitionSize;
        public Int32 cod;
        public Int32 data;
        public Int32 heap;
        public Int32 stackTop;
        public Int32 currentInstructionPointer;
        public Int32 publics;
        public Int32 natives;
        public Int32 libraries;
        public Int32 publicVars;
        public Int32 tags;
        public Int32 nameTable;
        public Int32 overlays;
    }
}