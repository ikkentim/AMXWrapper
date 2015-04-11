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
    /// Represents a pawn public function.
    /// </summary>
    public class AMXPublic
    {
        private readonly int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMXPublic"/> class.
        /// </summary>
        /// <param name="amx">The abstract machine this public is a member of.</param>
        /// <param name="index">The index.</param>
        public AMXPublic(AMX amx, int index)
        {
            AMX = amx;
            _index = index;
        }

        /// <summary>
        /// Gets the abstract machine this public is a member of.
        /// </summary>
        public AMX AMX { get; private set; }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>The return value.</returns>
        public int Execute()
        {
            int retval = AMX.Execute(_index);

            return retval;
        }
    }
}