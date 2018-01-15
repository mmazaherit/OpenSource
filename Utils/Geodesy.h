#ifndef GEODESY_H_
#define GEODESY_H_

#include <math.h>
#include <rotation>


class Geodesy
{
	template <typename T>
	static inline T rad2deg(T radvalue)
	{
		return radvalue / 3.14159265358979*180.0;
	}
	template <typename T>
	static inline T deg2rad(T degvalue)
	{
		return degvalue / 180.0*3.14159265358979;
	}

	 const double We = 7.292115e-5; // earth angular velocity rad/s
	 const double WGS84SemiMajorAxis = 6378137.0; //m
	 const double WGS84Flattening = 1.0 / 298.257223563;

	 const double a = WGS84SemiMajorAxis;
	 const double f = WGS84Flattening;
     const double e2 = f*(2.0 - f);

		
public:
	// earth curvatures
	inline double RM(double phi)
	{		
        const double sinphi2 = sin(deg2rad(phi))*sin(deg2rad(phi));
		return a*(1.0 - e2) / pow(1.0 - e2*sinphi2, 1.50);
	}
	inline double RN(double phi)
	{		
        const double sinphi2 = sin(deg2rad(phi))*sin(deg2rad(phi));
		return a / sqrt(1.0 - e2*sinphi2);
	}
   //http://clynchg3c.com/Technote/geodesy/coordcvt.pdf
   // Converts WGS-84 Geodetic point (lat, lon, h) to the
   // Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z).
   //lat long are degrees, h is meter
	template <typename T>
	void GeodeticToEcef(T lat, T lon, T h, T &x, T &y, T &z)
	{		
		T  N = RN(lat);
		
        T  sin_lambda = sin( deg2rad(lon));
        T  cos_lambda = cos( deg2rad(lon));
        T  cos_phi = cos(deg2rad(lat));
        T  sin_phi = sin(deg2rad(lat));

		x = (h + N) * cos_phi * cos_lambda;
		y = (h + N) * cos_phi * sin_lambda;
		z = (h + (1 - e2) * N) * sin_phi;

	}
	template <typename T>
	void EcefToGeodetic(T &lat, T &lon, T &h, T x, T y, T z, T tol=0.001)
	{
		lon = atan2(y,x);		
		const T p = sqrt(x*x + y*y);
		lat = atan2(p, z);
		T prev_h = 999999;
		int nIterations = 0;
    h=0;
		do
		{
			++nIterations;
            T RN_ = RN(rad2deg(lat));
			prev_h = h;
			h = p / cos(lat) -RN_;
			lat = atan(z /p/(1.0 - e2*RN_ / (RN_ + h)));
		} while (abs(h - prev_h) > tol && nIterations<10);

        lat =rad2deg(lat);
        lon =rad2deg(lon);
	}
	//based on the book Fundamentals of Inertial navigation, satellite based
	//formula 2.70
	template <typename T, int row_stride, int col_stride>
	void R_llf2ecef(T &lat, T &lon, ceres::MatrixAdapter<T, row_stride, col_stride>& R)
	{
		const T sinlambda = sin(deg2rad(lon));
		const T coslambda = cos(deg2rad(lon));
		const T sinphi = sin(deg2rad(lat));
		const T cosphi = cos(deg2rad(lat));

		R(0, 0) = -sinlambda;
		R(0, 1) = -sinphi * coslambda;
		R(0, 2) = cosphi * coslambda;
		
		R(1, 0) = coslambda;
		R(1, 1) = -sinphi * sinlambda;
		R(1, 2) = cosphi * sinlambda;

		R(2, 0) = T(0.0);
		R(2, 1) = cosphi;
		R(2, 2) = sinphi;
	}
	//formula 2.73
	template <typename T, int row_stride, int col_stride>
	void R_wander2llf(T wanderAngleDeg, ceres::MatrixAdapter<T, row_stride, col_stride>& R)
	{
		const T sinalpha = sin(deg2rad(wanderAngleDeg));
		const T cosalpha = cos(deg2rad(wanderAngleDeg));

		R(0, 0) = cosalpha;
		R(0, 1) = -sinalpha;
		R(0, 2) = T(0.0);

		R(1, 0) = sinalpha;
		R(1, 1) = cosalpha;
		R(1, 2) = T(0.0);

		R(2, 0) = T(0.0);
		R(2, 1) = T(0.0);
		R(2, 2) = T(1.0);
	}
	
};





#endif
