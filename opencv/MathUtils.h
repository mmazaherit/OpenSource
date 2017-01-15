#ifndef MathUtils_h__
#define MathUtils_h__
#include <limits>

template<typename T>
static inline T DotProduct31(const T *const vec1, const T *const vec2)
{
    return vec1[0] * vec2[0] + vec1[1] * vec2[1] + vec1[2] * vec2[2];
}
template<typename T>
static inline void Subtract31(const T *const vec1, const T *const vec2, T *vec1Minusvec2)
{
    vec1Minusvec2[0] = vec1[0] - vec2[0];
    vec1Minusvec2[1] = vec1[1] - vec2[1];
    vec1Minusvec2[2] = vec1[2] - vec2[2];
    
}

template<typename T>
static inline T Distance31(const T *const vec1, const T *const vec2)
{
    T res[3];
    Subtract31(vec1, vec2, res);
    return sqrt(DotProduct31(res,res));
}


template<typename T>
inline void ComputeMean(int n, const T* const Values, double MeanMinMax[3])
{
	double Sum=0;
	T Max = std::numeric_limits<T>::min();
	T Min = std::numeric_limits<T>::max();
	
	for (int i = 0; i < n; i++)
	{
		if (Values[i] > Max)
		{
			Max = Values[i];
		}
		else 
			if(Values[i] < Min)
			{
				Min = Values[i];
			}
				
		Sum += Values[i];
	}

	MeanMinMax[0]= Sum / n;
	MeanMinMax[1]=  Min;
	MeanMinMax[2]=  Max;
	
}
template<typename T>
inline double ComputeMean(int n, const T* const Values)
{
	double Sum=0;
	for (int i = 0; i < n; i++)
		Sum += Values[i];

	return Sum / n;
}

#endif // MathUtils_h__
