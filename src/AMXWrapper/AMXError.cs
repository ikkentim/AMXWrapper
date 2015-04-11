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

namespace AMXWrapper
{
    /// <summary>
    /// Contains every error the pawn abstract machine can return.
    /// </summary>
    public enum AMXError
    {
        /// <summary>
        ///     no error
        /// </summary>
        None,

        /// <summary>
        ///     forced exit
        /// </summary>
        ForcedExit,

        /// <summary>
        ///     assert failed
        /// </summary>
        AssertFailed,

        /// <summary>
        ///     stack/heap collision
        /// </summary>
        StackCollision,

        /// <summary>
        ///     index out of bounds
        /// </summary>
        IndexOutOfBounds,

        /// <summary>
        ///     invalid memory access
        /// </summary>
        InvalidMemoryAccess,

        /// <summary>
        ///     invalid instruction
        /// </summary>
        InvalidInstruction,

        /// <summary>
        ///     stack underflow
        /// </summary>
        StackUnderflow,

        /// <summary>
        ///     heap underflow
        /// </summary>
        HeapUnderflow,

        /// <summary>
        ///     no callback, or invalid callback
        /// </summary>
        CallbackError,

        /// <summary>
        ///     native function failed
        /// </summary>
        NativeError,

        /// <summary>
        ///     divide by zero
        /// </summary>
        DivideByZero,

        /// <summary>
        ///     go into sleepmode - code can be restarted
        /// </summary>
        Sleep,

        /// <summary>
        ///     no implementation for this state, no fall-back
        /// </summary>
        InvalidState,

        /// <summary>
        ///     out of memory
        /// </summary>
        OutOfMemory = 16,

        /// <summary>
        ///     invalid file format
        /// </summary>
        InvalidFormat,

        /// <summary>
        ///     file is for a newer version of the AMX
        /// </summary>
        InvalidVersion,

        /// <summary>
        ///     function not found
        /// </summary>
        FunctionNotFound,

        /// <summary>
        ///     invalid index parameter (bad entry point)
        /// </summary>
        InvalidIndex,

        /// <summary>
        ///     debugger cannot run
        /// </summary>
        DebugFailure,

        /// <summary>
        ///     AMX not initialized (or doubly initialized)
        /// </summary>
        InitFailure,

        /// <summary>
        ///     unable to set user data field (table full)
        /// </summary>
        UserDataFailure,

        /// <summary>
        ///     cannot initialize the JIT
        /// </summary>
        InitJitFailure,

        /// <summary>
        ///     parameter error
        /// </summary>
        ParameterError,

        /// <summary>
        ///     domain error, expression result does not fit in range
        /// </summary>
        DomainError,

        /// <summary>
        ///     general error (unknown or unspecific error)
        /// </summary>
        GeneralError,

        /// <summary>
        ///     overlays are unsupported (JIT) or uninitialized
        /// </summary>
        OverlayError,
    }
}