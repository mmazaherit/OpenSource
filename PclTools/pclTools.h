#ifndef pclTools_H_
#define pclTools_H_


	//http://pointclouds.org/documentation/tutorials/adding_custom_ptype.php

#define PCL_INSTANTIATE_pclTools(T) template ... pclTools<T> (...);

	template<typename PointT>
	class pclTools
	{
	public:							
		std::vector<int>  pclTools<PointT>::DetectLocalMaxima(boost::shared_ptr<pcl::PointCloud<PointT>> cloud, const double Radius);		
		std::vector<int> pclTools<PointT>::DetectLocalMaximaHeightMap2D(boost::shared_ptr<pcl::PointCloud<PointT>> &cloud, pcl::KdTreeFLANN<pcl::PointXY>::Ptr &kdtree2d, const double Radius);
	};


#endif
