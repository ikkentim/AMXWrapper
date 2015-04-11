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

namespace AMXWrapper
{
    /// <summary>
    /// Represents errors that occur in a pawn abstract machine.
    /// </summary>
    public class AMXException : Exception
    {
        private static readonly string[] Messages =
        {
            /* AMX_ERR_NONE      */ "(none)",
            /* AMX_ERR_EXIT      */ "Forced exit",
            /* AMX_ERR_ASSERT    */ "Assertion failed",
            /* AMX_ERR_STACKERR  */ "Stack/heap collision (insufficient stack size)",
            /* AMX_ERR_BOUNDS    */ "Array index out of bounds",
            /* AMX_ERR_MEMACCESS */ "Invalid memory access",
            /* AMX_ERR_INVINSTR  */ "Invalid instruction",
            /* AMX_ERR_STACKLOW  */ "Stack underflow",
            /* AMX_ERR_HEAPLOW   */ "Heap underflow",
            /* AMX_ERR_CALLBACK  */ "No (valid) native function callback",
            /* AMX_ERR_NATIVE    */ "Native function failed",
            /* AMX_ERR_DIVIDE    */ "Divide by zero",
            /* AMX_ERR_SLEEP     */ "(sleep mode)",
            /* AMX_ERR_INVSTATE  */ "Invalid state",
            /* 14 */                "(reserved)",
            /* 15 */                "(reserved)",
            /* AMX_ERR_MEMORY    */ "Out of memory",
            /* AMX_ERR_FORMAT    */ "Invalid/unsupported P-code file format",
            /* AMX_ERR_VERSION   */ "File is for a newer version of the AMX",
            /* AMX_ERR_NOTFOUND  */ "File or function is not found",
            /* AMX_ERR_INDEX     */ "Invalid index parameter (bad entry point)",
            /* AMX_ERR_DEBUG     */ "Debugger cannot run",
            /* AMX_ERR_INIT      */ "AMX not initialized (or doubly initialized)",
            /* AMX_ERR_USERDATA  */ "Unable to set user data field (table full)",
            /* AMX_ERR_INIT_JIT  */ "Cannot initialize the JIT",
            /* AMX_ERR_PARAMS    */ "Parameter error",
            /* AMX_ERR_DOMAIN    */ "Domain error, expression result does not fit in range",
            /* AMX_ERR_GENERAL   */ "General error (unknown or unspecific error)",
            /* AMX_ERR_OVERLAY   */ "Overlays are unsupported (JIT) or uninitialized"
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="AMXException" /> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public AMXException(AMXError error)
            : base((int) error >= 0 && (int) error < Messages.Length ? Messages[(int) error] : "Unknown AMX error")
        {
            Error = error;
        }

        /// <summary>
        ///     Gets the error.
        /// </summary>
        public AMXError Error { get; private set; }
    }
}