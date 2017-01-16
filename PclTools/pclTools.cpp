#include "stdafx.h"

#define PCL_NO_PRECOMPILE // put it before any 

#include "pcl/point_types.h"
#include "pcl/impl/instantiate.hpp"
#include "pclTools.h"
#include "impl/pclTools.hpp"


	// Instantiations of specific point types
//PCL_INSTANTIATE(pclTools, PCL_XYZ_POINT_TYPES);

// add more points type here if you like
template class pclTools<pcl::PointXYZI>;
template class pclTools<pcl::PointXYZ>;
