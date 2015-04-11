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
    internal struct AMXStruct
    {
        private readonly /*cell*/ Int32 alt;
        private readonly IntPtr _base;
        private readonly /*AMX_CALLBACK*/ IntPtr callback;
        private readonly /*cell*/ Int32 currentInstructionPointer;
        private readonly IntPtr code;
        private readonly Int64 codeSize;
        private readonly IntPtr data;
        private readonly /*AMX_DEBUG*/ IntPtr debug;
        private readonly Int32 error;
        private readonly Int32 flags;
        private readonly Int32 userdata0; /* Placeholder for usertags and userdata */
        private readonly Int32 userdata1;
        private readonly Int32 userdata2;
        private readonly Int32 userdata3;
        private readonly Int32 userdata4;
        private readonly Int32 userdata5;
        private readonly /*cell*/ Int32 frame;
        private readonly /*cell*/ Int32 heap;
        private readonly /*cell*/ Int32 heapLow;
        private readonly /*AMX_OVERLAY*/ IntPtr overlay;
        private readonly Int32 overlayIndex;
        private readonly Int32 paramCount;
        private readonly /*cell*/ Int32 primary;
        private readonly /*cell*/ Int32 resetHeap;
        private readonly /*cell*/ Int32 resetStack;
        private readonly /*cell*/ Int32 stack;
        private readonly /*cell*/ Int32 stackTop;
        private readonly /*cell*/ Int32 sysReqD;
    }
}