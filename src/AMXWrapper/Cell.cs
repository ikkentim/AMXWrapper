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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace AMXWrapper
{
    /// <summary>
    /// Represents a cell in a pawn abstract machine.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Cell
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Cell(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets a <see cref="Cell"/> representation of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Cell"/> representation of the specified <paramref name="value"/></returns>
        public static Cell FromInt32(int value)
        {
            return new Cell(value);
        }

        /// <summary>
        /// Gets a <see cref="Cell"/> representation of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Cell"/> representation of the specified <paramref name="value"/></returns>
        public static Cell FromIntPtr(IntPtr ptr)
        {
            return new Cell(Marshal.ReadInt32(ptr));
        }

        /// <summary>
        /// Gets a <see cref="Cell"/> representation of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Cell"/> representation of the specified <paramref name="value"/></returns>
        public static Cell FromFloat(float value)
        {
            return new Cell(BitConverter.ToInt32(BitConverter.GetBytes(value), 0));
        }

        /// <summary>
        /// Gets an integer representation of this instance.
        /// </summary>
        /// <returns>An integer representation of this instance.</returns>
        public int AsInt32()
        {
            return _value;
        }

        /// <summary>
        /// Gets an pointer representation of this instance.
        /// </summary>
        /// <returns>An pointer representation of this instance.</returns>
        public IntPtr AsIntPtr()
        {
            return new IntPtr(_value);
        }

        /// <summary>
        /// Gets an float representation of this instance.
        /// </summary>
        /// <returns>An float representation of this instance.</returns>
        public float AsFloat()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(_value), 0);
        }

        /// <summary>
        /// Gets the string this instance points to.
        /// </summary>
        /// <returns>The string this instance points to.</returns>
        public string AsString()
        {
            int length;

            AMXCall.StrLen(AsIntPtr(), out length);

            var sb = new StringBuilder();
            AMXCall.GetString(sb, AsIntPtr(), 0, length + 1);

            return sb.ToString();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="IntPtr"/> to <see cref="Cell"/>.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Cell(IntPtr ptr)
        {
            return FromIntPtr(ptr);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="Cell"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Cell(int value)
        {
            return FromInt32(value);
        }

        #region Overrides of ValueType

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}