#pragma once
#include "CommonHeaders.h"
#include "..\Platform\Window.h"

namespace spoon::graphics {

	class surface
	{
	};

	struct render_surface
	{
		platform::window window{};
		surface surface{};
	};

}