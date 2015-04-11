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
using System.Threading;
using AMXWrapper;

namespace WrapTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
             * 
             * native println(txt[]);

main()
{
	mainCall();
}

mainCall() <mainCalled>
{
	println("main was called AGAIN!!!");
	println("just so you know.");
}

mainCall() <>
{
	println("main was called.");
	println("just so you know.");
	state mainCalled;
}

forward OnStart();
public OnStart()
{
    println("OnStart was called.");
    return 1;
}

forward OnMessage(msg[]);
public OnMessage(msg[])
{
    println("OnMessage was called with:");
    println(msg);
    return 1;
}


             */
            var pcode =
                Convert.FromBase64String(
                    "vAIAAODxCwsEAAgAcAAAADACAAC8AgAAvEIAADQAAAA8AAAATAAAAFQAAABUAAAAVAAAAFQAAABU" +
                    "AAAAYAEAAFYAAAAoAQAAYAAAAAAAAABoAAAAHwBPbk1lc3NhZ2UAT25TdGFydABwcmludGxuAEMA" +
                    "AAAAAAAAQwAAAA0AAAABAAAAAAAAAEYAAAAIAAAASgAAAAEAAACUAAAAAQAAACwAAAAeAAAACQAA" +
                    "AAAAAAAWAAAAIQAAAMz///8JAAAAAAAAACAAAAAeAAAACQAAAAQAAAAYAAAACQAAAAQAAAAWAAAA" +
                    "RQAAAAAAAAAcAAAACAAAAAkAAAAgAAAAGAAAAAkAAAAEAAAAFgAAAEUAAAAAAAAAHAAAAAgAAAAJ" +
                    "AAAAAAAAACAAAAAeAAAACQAAADQAAAAYAAAACQAAAAQAAAAWAAAARQAAAAAAAAAcAAAACAAAAAkA" +
                    "AABIAAAAGAAAAAkAAAAEAAAAFgAAAEUAAAAAAAAAHAAAAAgAAAAJAAAAAQAAAA0AAAAAAAAACQAA" +
                    "AAAAAAAgAAAAHgAAAAkAAABcAAAAGAAAAAkAAAAEAAAAFgAAAEUAAAAAAAAAHAAAAAgAAAAJAAAA" +
                    "AQAAACAAAAAeAAAACQAAAHAAAAAYAAAACQAAAAQAAAAWAAAARQAAAAAAAAAcAAAACAAAAAMAAAAM" +
                    "AAAAGAAAAAkAAAAEAAAAFgAAAEUAAAAAAAAAHAAAAAgAAAAJAAAAAQAAACAAAAAAAAAAbmlhbXNh" +
                    "dyBsYWMgIGRlbElBR0EhISFOAAAAAHRzdWogb3MgIHVveXdvbmsAAAAubmlhbXNhdyBsYWMgLmRl" +
                    "bAAAAAB0c3VqIG9zICB1b3l3b25rAAAALnRTbk8gdHJhIHNhd2xsYWMALmRlZU1uT2dhc3NhdyBl" +
                    "YWMgc2RlbGx0aXcgAAA6aA==");

            var amx = new AMX(pcode/*"E:/test.amx"*/);

            amx.Register("println", (amx1, args1) =>
            {
                Console.WriteLine("PRINTLN: {0}", args1[0].AsString());
                return 1;
            });
            amx.ExecuteMain();

            AMXPublic p = amx.FindPublic("OnStart");
            for (int i = 0; i < 5; i++) p.Execute();

            AMXPublic m = amx.FindPublic("OnMessage");

            for (int i = 0;; i += 100)
            {
                string str = string.Format("Caller {0} dialed in", i);

                IntPtr ptr = amx.Push(str);

                m.Execute();

                amx.Release(ptr);

                GC.Collect(); // To check for leaks

                Thread.Sleep(10);
            }
        }
    }
}