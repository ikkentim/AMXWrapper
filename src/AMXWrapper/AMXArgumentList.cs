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
    /// <summary>
    /// Represents an function arguments list.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AMXArgumentList
    {
        private readonly IntPtr _ptr;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMXArgumentList"/> struct.
        /// </summary>
        /// <param name="ptr">A pointer to the list.</param>
        public AMXArgumentList(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length
        {
            get { return Marshal.ReadInt32(_ptr)/Marshal.SizeOf(typeof (Cell)); }
        }

        /// <summary>
        /// Gets the <see cref="Cell"/> at the specified 0-based index.
        /// </summary>
        /// <value>
        /// The <see cref="Cell"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Cell this[int index]
        {
            get { return Cell.FromIntPtr(IntPtr.Add(_ptr, (index + 1)*Marshal.SizeOf(typeof (Cell)))); }
        }
    }
}