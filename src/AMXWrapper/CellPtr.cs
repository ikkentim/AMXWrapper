// AIWorld
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
    ///     Represents a cell in a pawn abstract machine.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CellPtr
    {
        private readonly IntPtr _value;

        public CellPtr(IntPtr value)
        {
            _value = value;
        }

        public IntPtr Value
        {
            get { return _value; }
        }

        public Cell this[int offset]
        {
            get { return Get(offset); }
            set { Set(offset, value); }
        }

        public Cell Get()
        {
            return Cell.FromIntPtr(_value);
        }

        public Cell Get(int offset)
        {
            return Cell.FromIntPtr(IntPtr.Add(_value, offset*Marshal.SizeOf(typeof (Cell))));
        }

        public void Set(int offset, int value)
        {
            Marshal.WriteInt32(IntPtr.Add(_value, offset * Marshal.SizeOf(typeof(Cell))), value);
        }

        public void Set(int value)
        {
            Marshal.WriteInt32(_value, value);
        }

        public void Set(int offset, Cell value)
        {
            Marshal.WriteInt32(IntPtr.Add(_value, offset * Marshal.SizeOf(typeof(Cell))), value.AsInt32());
        }

        public void Set(Cell value)
        {
            Set(value.AsInt32());
        }

        public Cell AsCell()
        {
            return new Cell(_value.ToInt32());
        }

        public static CellPtr operator +(CellPtr cellPtr, int offset)
        {
            return new CellPtr(IntPtr.Add(cellPtr.Value, offset*Marshal.SizeOf(typeof (Cell))));
        }

        public static implicit operator CellPtr(Cell cell)
        {
            return cell.AsCellPtr();
        }
    }
}