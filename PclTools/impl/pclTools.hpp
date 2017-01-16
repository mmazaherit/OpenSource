#ifndef  pclTools_HPP_
#define  pclTools_HPP_


#include "pclTools.h"
#include <pcl/filters/local_maximum.h>
#include <pcl/people/height_map_2d.h>
#include <Eigen/Dense>
	template<typename PointT>
	std::vector<int>  pclTools<PointT>::DetectLocalMaxima(boost::shared_ptr<pcl::PointCloud<PointT>> cloud, const double Radius)
	{
		pcl::LocalMaximum<PointT> LM;
	
		LM.setNegative(true);// set negative return the local MAXIMA NOT THE FILTERED ONES
		std::vector<int> FilteredIndices;
		LM.setInputCloud(cloud);
		LM.setRadius(Radius);
		
		LM.filter(FilteredIndices);
		return FilteredIndices;

	};


	template<typename PointT>
	std::vector<int> pclTools<PointT>::DetectLocalMaximaHeightMap2D(boost::shared_ptr<pcl::PointCloud<PointT>> &cloud, pcl::KdTreeFLANN<pcl::PointXY>::Ptr &kdtree2d, const double Radius)
	{
		std::vector<int> result;
		pcl::PointXY pt;
		for (int i = 0; i < cloud->size(); i++)
		{
			std::vector<int> ind;
			std::vector<float> dist;
			auto &PT = cloud->at(i);
			pt.x = PT.x;
			pt.y = PT.y;

			unsigned int max_nn=1000;

			//int n = kdtree2d->nearestKSearch(pt,100,ind, dist);
			int n = kdtree2d->radiusSearch(pt,Radius,ind, dist,max_nn);

			float maxz = std::numeric_limits<float>::min();
			float minz = std::numeric_limits<float>::max();

			for (int j = 0; j < n; j++)
			{
				int k = ind[j];
				float z = cloud->at(k).z;

				if (z > maxz)
					maxz = z;
				if (z < minz)
					minz = z;

			}

			if (PT.z>= maxz)
				result.push_back(i);
			
			if (PT.z <= minz)
				result.push_back(i);
			
		}

		return result;

	}

#endif