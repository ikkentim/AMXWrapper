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
    ///     Contains all available status flags of a pawn abstract machine.
    /// </summary>
    [Flags]
    public enum AMXFlag : ushort
    {
        /// <summary>
        ///     all function calls use overlays
        /// </summary>
        Overlay = 0x01,

        /// <summary>
        ///     symbolic info. available
        /// </summary>
        Debug = 0x02,

        /// <summary>
        ///     no array bounds checking; no BREAK opcodes
        /// </summary>
        NoChecks = 0x04,

        /// <summary>
        ///     script uses the sleep instruction (possible re-entry or power-down mode)
        /// </summary>
        Sleep = 0x08,

        /// <summary>
        ///     file is encrypted
        /// </summary>
        Crypt = 0x10,

        /// <summary>
        ///     data section is explicitly initialized
        /// </summary>
        DataSectionInit = 0x20,

        /// <summary>
        ///     script uses new (optimized) version of SYSREQ opcode
        /// </summary>
        SysReqNew = 0x800,

        /// <summary>
        ///     all native functions are registered
        /// </summary>
        NativesRegistered = 0x1000,

        /// <summary>
        ///     abstract machine is JIT compiled
        /// </summary>
        JitCompiled = 0x2000,

        /// <summary>
        ///     busy verifying P-code
        /// </summary>
        Verify = 0x4000,

        /// <summary>
        ///     AMX has been initialized
        /// </summary>
        Init = 0x8000,
    }
}