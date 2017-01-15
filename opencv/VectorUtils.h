#ifndef Algorithm_h__
#define Algorithm_h__

#include <vector>


template <typename T>
inline std::vector<size_t> sort_indexes_asc(const std::vector<T> &v) {

	// initialize original index locations
	std::vector<std::size_t> idx(v.size());
	std::iota(idx.begin(), idx.end(), 0);

	// sort indexes based on comparing values in v
	std::sort(idx.begin(), idx.end(),
		[&v](std::size_t i1, std::size_t i2) {return v[i1] < v[i2]; });

	return idx;
}
template <typename T>
inline std::vector<size_t> sort_indexes_des(const std::vector<T> &v) {

	// initialize original index locations
	std::vector<std::size_t> idx(v.size());
	std::iota(idx.begin(), idx.end(), 0);

	// sort indexes based on comparing values in v
	std::sort(idx.begin(), idx.end(),
		[&v](std::size_t i1, std::size_t i2) {return v[i1] > v[i2]; });

	return idx;
}
#endif // Algorithm_h__
