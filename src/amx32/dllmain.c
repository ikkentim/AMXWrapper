/*  Abstract Machine DLL interface functions
*
*  Copyright (c) ITB CompuPhase, 1999-2010
*
*  Licensed under the Apache License, Version 2.0 (the "License"); you may not
*  use this file except in compliance with the License. You may obtain a copy
*  of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
*  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
*  License for the specific language governing permissions and limitations
*  under the License.
*/
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <assert.h>
#include <ctype.h>      /* for isdigit() */
#include <limits.h>
#include <malloc.h>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>     /* for atoi() */
#include <string.h>

/* redirect amx_Init() to a special version */
#if !defined amx_Init
#error "amx_Init" must be defined when compiling these sources
#endif
#undef amx_Init /* redirection is over here */

#include "osdefs.h"
#include "amx.h"


int AMXAPI amx_InitAMX(AMX *amx, void *program);
int AMXAPI amx_CoreInit(AMX *amx);
int AMXAPI amx_FixedInit(AMX *amx);


int AMXAPI amx_Init(AMX *amx, void *program)
{
    int err;

    if ((err = amx_InitAMX(amx, program)) != AMX_ERR_NONE)
        return err;

    /* load standard libraries */
    amx_CoreInit(amx);
    amx_FixedInit(amx);

    return err;
}
