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
using System.Text;

namespace AMXWrapper
{
    internal static class AMXCall
    {
        private const string Library = "amx32.dll";


        [DllImport(Library, EntryPoint = "amx_Flags")]
        public static extern int Flags(ref AMXStruct amx, out UInt16 flags);

        //[DllImport(library, EntryPoint = "amx_Callback")]
        //public static extern int Callback(ref AMXStruct amx, int index, IntPtr result, AMXArgumentList parms);

        [DllImport(Library, EntryPoint = "amx_Init")]
        public static extern int Init(ref AMXStruct amx, IntPtr program);

        [DllImport(Library, EntryPoint = "amx_Cleanup")]
        public static extern int Cleanup(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_Clone")]
        public static extern int Clone(ref AMXStruct amxClone, AMXStruct amxSource, IntPtr data);

        [DllImport(Library, EntryPoint = "amx_MemInfo")]
        public static extern int MemoryInfo(ref AMXStruct amx, out long codesize, out long datasize, out long stackheap);

        [DllImport(Library, EntryPoint = "amx_NameLength")]
        public static extern int NameLength(ref AMXStruct amx, out int length);

        [DllImport(Library, EntryPoint = "amx_NumNatives")]
        public static extern int NumberOfNatives(ref AMXStruct amx, out int number);

        [DllImport(Library, EntryPoint = "amx_GetNative")]
        public static extern int GetNative(ref AMXStruct amx, int index, StringBuilder name);

        [DllImport(Library, EntryPoint = "amx_FindNative")]
        public static extern int FindNative(ref AMXStruct amx, string name, out int index);

        [DllImport(Library, EntryPoint = "amx_NumPublics")]
        public static extern int NumberOfPublics(ref AMXStruct amx, out int number);

        [DllImport(Library, EntryPoint = "amx_GetPublic")]
        public static extern int GetPublic(ref AMXStruct amx, int index, StringBuilder name, out IntPtr address);

        [DllImport(Library, EntryPoint = "amx_FindPublic")]
        public static extern int FindPublic(ref AMXStruct amx, string name, out int index);

        [DllImport(Library, EntryPoint = "amx_NumPubVars")]
        public static extern int NumberOfPublicVars(ref AMXStruct amx, out int number);

        [DllImport(Library, EntryPoint = "amx_GetPubVar")]
        public static extern int GetPublicVar(ref AMXStruct amx, int index, StringBuilder name, out IntPtr address);

        [DllImport(Library, EntryPoint = "amx_FindPubVar")]
        public static extern int FindPublicVar(ref AMXStruct amx, string name, out IntPtr address);

        [DllImport(Library, EntryPoint = "amx_NumTags")]
        public static extern int NumberOfTags(ref AMXStruct amx, out int number);

        [DllImport(Library, EntryPoint = "amx_GetTag")]
        public static extern int GetTag(ref AMXStruct amx, int index, StringBuilder tagname, out Int32 tagID);

        [DllImport(Library, EntryPoint = "amx_FindTagId")]
        public static extern int FindTagId(ref AMXStruct amx, Int32 tagID, StringBuilder tagname);

        //private static extern int amx_GetUserData(ref AMX amx, long tag, void **ptr);

        //private static extern int amx_SetUserData(ref AMX amx, long tag, void *ptr);

        [DllImport(Library, EntryPoint = "amx_Register")]
        public static extern int Register(ref AMXStruct amx, [MarshalAs(UnmanagedType.LPArray)] AMXNativeInfo[] list,
            int count);

        [DllImport(Library, EntryPoint = "amx_Push")]
        public static extern int Push(ref AMXStruct amx, Int32 value);

        [DllImport(Library, EntryPoint = "amx_PushAddress")]
        public static extern int PushAddress(ref AMXStruct amx, IntPtr address);

        [DllImport(Library, EntryPoint = "amx_PushAddress")]
        public static extern int PushAddress(ref AMXStruct amx, ref Cell cell);

        [DllImport(Library, EntryPoint = "amx_PushArray")]
        public static extern int PushArray(ref AMXStruct amx, out CellPtr address, int[] array, int numcells);

        [DllImport(Library, EntryPoint = "amx_PushString")]
        public static extern int PushString(ref AMXStruct amx, out IntPtr address, string str, int pack, int useWchar);

        [DllImport(Library, EntryPoint = "amx_Exec")]
        public static extern int Exec(ref AMXStruct amx, out int retval, int index);

        //[DllImport(library, EntryPoint = "amx_SetCallback")]
        //private static extern int AmxSetCallback(ref AMXStruct amx, AmxCallback callback);

        //[DllImport(library, EntryPoint = "amx_SetDebugHook")]
        //private static extern int AmxSetDebugHook(ref AMX amx,AmxDebug debug);

        [DllImport(Library, EntryPoint = "amx_RaiseError")]
        public static extern int RaiseError(ref AMXStruct amx, int error);

        [DllImport(Library, EntryPoint = "amx_Allot")]
        public static extern int Allot(ref AMXStruct amx, int cells, out IntPtr address);

        [DllImport(Library, EntryPoint = "amx_Release")]
        public static extern int Release(ref AMXStruct amx, IntPtr address);

        [DllImport(Library, EntryPoint = "amx_StrLen")]
        public static extern int StrLen(IntPtr cstr, out int length);

        [DllImport(Library, EntryPoint = "amx_SetString")]
        public static extern int SetString(IntPtr dest, string source, int pack, int useWchar, int size);

        [DllImport(Library, EntryPoint = "amx_GetString")]
        public static extern int GetString(StringBuilder dest, IntPtr source, int useWchar, int size);

        //public static extern int amx_UTF8Get(const char *string, const char **endptr, cell *value)

        //public static extern int amx_UTF8Put(char *string, char **endptr, int maxchars, cell value)

        //public static extern int amx_UTF8Check(const char *string, int *length)

        //public static extern int amx_UTF8Len(const cell *cstr, int *length)


        [DllImport(Library, EntryPoint = "amx_ConsoleInit")]
        public static extern int ConsoleInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_ConsoleCleanup")]
        public static extern int ConsoleCleanup(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_CoreInit")]
        public static extern int CoreInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_CoreCleanup")]
        public static extern int CoreCleanup(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_DGramInit")]
        public static extern int DGramInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_DGramCleanup")]
        public static extern int DGramCleanup(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_StringInit")]
        public static extern int StringInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_StringCleanup")]
        public static extern int StringCleanup(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_TimeInit")]
        public static extern int TimeInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_TimeCleanup")]
        public static extern int TimeClean(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_FixedInit")]
        public static extern int FixedInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_FixedCleanup")]
        public static extern int FixedCleanup(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_FloatInit")]
        public static extern int FloatInit(ref AMXStruct amx);

        [DllImport(Library, EntryPoint = "amx_FloatCleanup")]
        public static extern int FloatCleanup(ref AMXStruct amx);


    }
}