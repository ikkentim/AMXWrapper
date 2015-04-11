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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AMXWrapper
{
    /// <summary>
    ///     Represents an AMX virtual machine.
    /// </summary>
    public class AMX : Disposable
    {
        /// <summary>
        ///     A function to call when a native has been called.
        /// </summary>
        /// <param name="amx">The amx.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public delegate int AMXNativeFunction(AMX amx, AMXArgumentList arguments);

        private const int Magic = 0xf1e0;
        private const int MainEntryPointIndex = -1;
        private const int ContinueEntryPointIndex = -2;

        private readonly IntPtr _code;
        private readonly int _codeLength;
        private readonly List<AMXNativeCall> _natives = new List<AMXNativeCall>();
        private AMXStruct _amx;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AMX" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        public AMX(string path) : this(File.ReadAllBytes(path))
        {
            if (path == null) throw new ArgumentNullException("path");
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AMX" /> class.
        /// </summary>
        /// <param name="pCodeStream">The p code stream.</param>
        /// <exception cref="System.ArgumentNullException">pCodeStream</exception>
        public AMX(Stream pCodeStream) : this(StreamToBytes(pCodeStream))
        {
            if (pCodeStream == null) throw new ArgumentNullException("pCodeStream");
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AMX" /> class.
        /// </summary>
        /// <param name="pCode">The p code.</param>
        /// <exception cref="System.ArgumentNullException">pCode</exception>
        /// <exception cref="AMXWrapper.AMXException">
        /// </exception>
        public AMX(byte[] pCode)
        {
            UnmanagedLibrariesLoader.Load();
            if (pCode == null) throw new ArgumentNullException("pCode");
            var amx = new AMXStruct();
            GCHandle handle = GCHandle.Alloc(pCode, GCHandleType.Pinned);

            var header = (AMXHeader) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof (AMXHeader));
            handle.Free();
            Align(ref header.magic);
            Align(ref header.size);
            Align(ref header.stackTop);

            if (header.magic != Magic)
            {
                throw new AMXException(AMXError.InvalidFormat);
            }

            _code = Marshal.AllocHGlobal(_codeLength = header.stackTop);
            FillMemory(_code, (uint) header.stackTop, 0);
            Marshal.Copy(pCode, 0, _code, header.size);

            //var sb = new StringBuilder();
            //var h = Marshal.AllocHGlobal(1);
            //AMXCall.GetString(sb, h, 0, 0);

            var err = (AMXError) AMXCall.Init(ref amx, _code);

            if (err != AMXError.None)
            {
                throw new AMXException(err);
            }
            _amx = amx;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AMX" /> class.
        /// </summary>
        /// <param name="amx">The amx.</param>
        /// <param name="code">The code.</param>
        /// <param name="length">The length.</param>
        private AMX(AMXStruct amx, IntPtr code, int length)
        {
            _amx = amx;
            _code = code;
            _codeLength = length;
        }

        /// <summary>
        ///     Gets the flags.
        /// </summary>
        public AMXFlag Flags
        {
            get
            {
                ushort flags;

                AssertNotDisposed();
                AssertNoError(AMXCall.Flags(ref _amx, out flags));

                return (AMXFlag) flags;
            }
        }

        /// <summary>
        ///     Gets the maximum length of a name.
        /// </summary>
        public int NameLength
        {
            get
            {
                int length;

                AssertNotDisposed();
                AssertNoError(AMXCall.NameLength(ref _amx, out length));

                return length;
            }
        }

        /// <summary>
        ///     Gets the native count.
        /// </summary>
        public int NativeCount
        {
            get
            {
                int count;

                AssertNotDisposed();
                AssertNoError(AMXCall.NumberOfNatives(ref _amx, out count));

                return count;
            }
        }

        /// <summary>
        ///     Gets the public count.
        /// </summary>
        public int PublicCount
        {
            get
            {
                int count;

                AssertNotDisposed();
                AssertNoError(AMXCall.NumberOfPublics(ref _amx, out count));

                return count;
            }
        }

        /// <summary>
        ///     Gets the public variable count.
        /// </summary>
        public int PublicVarCount
        {
            get
            {
                int count;

                AssertNotDisposed();
                AssertNoError(AMXCall.NumberOfPublicVars(ref _amx, out count));

                return count;
            }
        }

        /// <summary>
        ///     Gets the tag count.
        /// </summary>
        public int TagCount
        {
            get
            {
                int count;

                AssertNotDisposed();
                AssertNoError(AMXCall.NumberOfTags(ref _amx, out count));

                return count;
            }
        }

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            Marshal.Release(_code);
        }

        /// <summary>
        ///     Registers a function with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="function">The function.</param>
        /// <returns>Abstract machine error.</returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public AMXError Register(string name, AMXNativeFunction function)
        {
            if (name == null) throw new ArgumentNullException("name");
            AssertNotDisposed();

            // Keep the native in this list so the GC won't wipe it out
            AMXNativeCall del = (ref AMXStruct amx, AMXArgumentList args) => function(this, args);
            _natives.Add(del);

            return
                (AMXError)
                    AMXCall.Register(ref _amx,
                        new[] {new AMXNativeInfo(name, del)}, 1);
        }

        /// <summary>
        ///     Gets errors related to function registration.
        /// </summary>
        /// <returns>Abstract machine error.</returns>
        public AMXError Register()
        {
            return (AMXError) AMXCall.Register(ref _amx, null, 0);
        }

        /// <summary>
        ///     Executes the the function at the specified <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The return value.</returns>
        public int Execute(int index)
        {
            int retval;

            AssertNotDisposed();
            AssertNoError(AMXCall.Exec(ref _amx, out retval, index));

            return retval;
        }

        /// <summary>
        ///     Executes the main entry point function.
        /// </summary>
        /// <returns>The return value.</returns>
        public int ExecuteMain()
        {
            return Execute(MainEntryPointIndex);
        }

        /// <summary>
        ///     Continues executing where it left off last.
        /// </summary>
        /// <returns>The return value.</returns>
        public int ExecuteContinue()
        {
            return Execute(ContinueEntryPointIndex);
        }

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public AMX Clone()
        {
            var clone = new AMXStruct();

            AssertNotDisposed();
            AssertNoError(AMXCall.Clone(ref clone, _amx, _code));

            IntPtr code = Marshal.AllocHGlobal(_codeLength);

            var tmp = new byte[_codeLength];
            Marshal.Copy(code, tmp, 0, _codeLength);
            Marshal.Copy(tmp, 0, code, _codeLength);

            return new AMX(clone, code, _codeLength);
        }

        /// <summary>
        ///     Finds the public with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public AMXPublic FindPublic(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            int index;

            AssertNotDisposed();
            AssertNoError(AMXCall.FindPublic(ref _amx, name, out index));

            return new AMXPublic(this, index);
        }

        /// <summary>
        ///     Gets the public at the specified <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IntPtr GetPublic(int index, out string name)
        {
            IntPtr ptr;
            var sb = new StringBuilder();

            AssertNotDisposed();
            AssertNoError(AMXCall.GetPublic(ref _amx, index, sb, out ptr));

            name = sb.ToString();
            return ptr;
        }

        /// <summary>
        ///     Finds the native with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public int FindNative(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            int index;

            AssertNotDisposed();
            AssertNoError(AMXCall.FindNative(ref _amx, name, out index));

            return index;
        }

        /// <summary>
        ///     Gets the native at the specified <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public string GetNative(int index)
        {
            var sb = new StringBuilder();
            AssertNotDisposed();
            AssertNoError(AMXCall.GetNative(ref _amx, index, sb));

            return sb.ToString();
        }

        /// <summary>
        ///     Gets the tag at the specified <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int GetTag(int index, out string name)
        {
            int id;
            var sb = new StringBuilder();

            AssertNotDisposed();
            AssertNoError(AMXCall.GetTag(ref _amx, index, sb, out id));

            name = sb.ToString();
            return id;
        }

        /// <summary>
        ///     Finds the tag with the specified <paramref name="id" />.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string FindTag(int id)
        {
            var sb = new StringBuilder();
            AssertNotDisposed();
            AssertNoError(AMXCall.FindTagId(ref _amx, id, sb));

            return sb.ToString();
        }

        /// <summary>
        ///     Finds the public variable with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public IntPtr FindPublicVar(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            IntPtr ptr;
            AssertNotDisposed();
            AssertNoError(AMXCall.FindPublicVar(ref _amx, name, out ptr));

            return ptr;
        }

        /// <summary>
        ///     Gets the public variable at the specified <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IntPtr GetPublicVar(int index, out string name)
        {
            IntPtr ptr;
            var sb = new StringBuilder();

            AssertNotDisposed();
            AssertNoError(AMXCall.GetPublicVar(ref _amx, index, sb, out ptr));

            name = sb.ToString();
            return ptr;
        }

        /// <summary>
        ///     Pushes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Push(int value)
        {
            AssertNotDisposed();
            AssertNoError(AMXCall.Push(ref _amx, value));
        }

        /// <summary>
        ///     Pushes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pack">if set to <c>true</c> packs the string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public IntPtr Push(string value, bool pack = false)
        {
            if (value == null) throw new ArgumentNullException("value");
            IntPtr address;

            AssertNotDisposed();
            AssertNoError(AMXCall.PushString(ref _amx, out address, value, pack ? 1 : 0, 0));

            return address;
        }

        /// <summary>
        ///     Gets the length of the string at the specified <paramref name="pointer" />.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns>The length of the string at the specified <paramref name="pointer" />.</returns>
        public static int GetStringLength(IntPtr pointer)
        {
            int len;
            AssertNoError(AMXCall.StrLen(pointer, out len));

            return len;
        }

        /// <summary>
        ///     Gets the string at the specified <paramref name="pointer" />.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="length">The length.</param>
        /// <returns>The string at the specified <paramref name="pointer" />.</returns>
        public static string GetString(IntPtr pointer, int length)
        {
            var sb = new StringBuilder();
            AssertNoError(AMXCall.GetString(sb, pointer, 0, length));

            return sb.ToString();
        }

        /// <summary>
        ///     Gets the string at the specified <paramref name="pointer" />.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns>The string at the specified <paramref name="pointer" />.</returns>
        public static string GetString(IntPtr pointer)
        {
            int len = GetStringLength(pointer);
            return GetString(pointer, len);
        }

        /// <summary>
        ///     Sets the specified <paramref name="value" /> to the string at the specified <paramref name="pointer" />.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <param name="pack">if set to <c>true</c> packs the string.</param>
        /// <param name="length">The length.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static void SetString(IntPtr pointer, string value, bool pack, int length)
        {
            if (value == null) throw new ArgumentNullException("value");
            AssertNoError(AMXCall.SetString(pointer, value, pack ? 1 : 0, 0, length));
        }

        /// <summary>
        ///     Sets the specified <paramref name="value" /> to the string at the specified <paramref name="pointer" />.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <param name="pack">if set to <c>true</c> packs the string.</param>
        public static void SetString(IntPtr pointer, string value, bool pack)
        {
            SetString(pointer, value, pack, value.Length);
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Push(IntPtr value)
        {
            AssertNotDisposed();
            AssertNoError(AMXCall.PushAddress(ref _amx, value));
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Push(ref Cell value)
        {
            AssertNotDisposed();
            AssertNoError(AMXCall.PushAddress(ref _amx, ref value));
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Push(Cell value)
        {
            Push(value.AsInt32());
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Push(float value)
        {
            Push(Cell.FromFloat(value));
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public void Push(out IntPtr address, IEnumerable<int> value)
        {
            if (value == null) throw new ArgumentNullException("value");
            AssertNotDisposed();

            if (value is int[])
            {
                var array = value as int[];
                AssertNoError(AMXCall.PushArray(ref _amx, out address, array, array.Length));
            }
            else
            {
                AssertNoError(AMXCall.PushArray(ref _amx, out address, value.ToArray(), value.Count()));
            }
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public void Push(out IntPtr address, IEnumerable<Cell> value)
        {
            if (value == null) throw new ArgumentNullException("value");
            AssertNotDisposed();
            AssertNoError(AMXCall.PushArray(ref _amx, out address, value.Select(c => c.AsInt32()).ToArray(),
                value.Count()));
        }

        /// <summary>
        ///     Pushes the specified <paramref name="value" />.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public void Push(out IntPtr address, IEnumerable<float> value)
        {
            if (value == null) throw new ArgumentNullException("value");
            Push(out address, value.Select(Cell.FromFloat));
        }

        /// <summary>
        ///     Gets information about the memory in the machine.
        /// </summary>
        /// <param name="codesize">The codesize.</param>
        /// <param name="datasize">The datasize.</param>
        /// <param name="stackheap">The stackheap.</param>
        public void MemoryInfo(out long codesize, out long datasize, out long stackheap)
        {
            AssertNotDisposed();
            AssertNoError(AMXCall.MemoryInfo(ref _amx, out codesize, out datasize, out stackheap));
        }

        /// <summary>
        ///     Raises the specified <paramref name="error" /> in the abstract machine.
        /// </summary>
        /// <param name="error">The error.</param>
        public void RaiseError(AMXError error)
        {
            AssertNotDisposed();
            AssertNoError(AMXCall.RaiseError(ref _amx, (int) error));
        }

        /// <summary>
        ///     Aligns the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static void Align(ref Int16 value)
        {
            if (BitConverter.IsLittleEndian) return;
            byte[] d = BitConverter.GetBytes(value);
            Array.Reverse(d);
            value = BitConverter.ToInt16(d, 0);
        }

        /// <summary>
        ///     Aligns the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static void Align(ref UInt16 value)
        {
            if (BitConverter.IsLittleEndian) return;
            byte[] d = BitConverter.GetBytes(value);
            Array.Reverse(d);
            value = BitConverter.ToUInt16(d, 0);
        }

        /// <summary>
        ///     Aligns the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static void Align(ref Int32 value)
        {
            if (BitConverter.IsLittleEndian) return;
            byte[] d = BitConverter.GetBytes(value);
            Array.Reverse(d);
            value = BitConverter.ToInt32(d, 0);
        }

        /// <summary>
        ///     Aligns the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static void Align(ref UInt32 value)
        {
            if (BitConverter.IsLittleEndian) return;
            byte[] d = BitConverter.GetBytes(value);
            Array.Reverse(d);
            value = BitConverter.ToUInt32(d, 0);
        }

        /// <summary>
        ///     Allocates the specified cells.
        /// </summary>
        /// <param name="cells">The cells.</param>
        /// <returns>The address at which the cells have been allocated.</returns>
        public IntPtr Allot(int cells)
        {
            IntPtr address;
            AssertNotDisposed();
            AssertNoError(AMXCall.Allot(ref _amx, cells, out address));
            return address;
        }

        /// <summary>
        ///     Releases the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        public void Release(IntPtr address)
        {
            AssertNotDisposed();
            AssertNoError(AMXCall.Release(ref _amx, address));
        }


        [DllImport("kernel32.dll", EntryPoint = "RtlFillMemory", SetLastError = false)]
        private static extern void FillMemory(IntPtr destination, uint length, byte fill);

        private static byte[] StreamToBytes(Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static void AssertNoError(AMXError error)
        {
            if (error != AMXError.None)
            {
                throw new AMXException(error);
            }
        }

        private static void AssertNoError(int error)
        {
            AssertNoError((AMXError) error);
        }
    }
}