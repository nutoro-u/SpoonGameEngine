#pragma once

#pragma warning(disable: 4530) // disable exception warning

#include <stdint.h>
#include <assert.h>
#include <typeinfo>

#if defined(_WIN64)
#include <DirectXMath.h>
#endif

#include "PrimitiveTypes.h"
#include "..\Util\MathTypes.h"
#include "..\Util\Util.h"