#ifndef GEODESY_H_
#define GEODESY_H_

template <typename T>
static inline T rad2deg(T radvalue)
{
    return radvalue/3.14159265358979*180.0;
}
template <typename T>
static inline T deg2rad(T degvalue)
{
    return degvalue/180.0*3.14159265358979;
}

class Geodesy
{
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
	void GeodeticToEcef(double lat, double lon, double h, double &x, double &y, double &z)
	{		
		double  N = RN(lat);
		
        double  sin_lambda = sin( deg2rad(lon));
        double  cos_lambda = cos( deg2rad(lon));
        double  cos_phi = cos(deg2rad(lat));
        double  sin_phi = sin(deg2rad(lat));

		x = (h + N) * cos_phi * cos_lambda;
		y = (h + N) * cos_phi * sin_lambda;
		z = (h + (1 - e2) * N) * sin_phi;

	}
	void EcefToGeodetic(double &lat, double &lon, double &h, double x, double y, double z, double tol=0.001)
	{
		lon = atan2(y,x);		
		const double p = sqrt(x*x + y*y);
		lat = atan2(p, z);
		double prev_h = 999999;
		int nIterations = 0;
    h=0;
		do
		{
			++nIterations;
            double RN_ = RN(rad2deg(lat));
			prev_h = h;
			h = p / cos(lat) -RN_;
			lat = atan(z /p/(1.0 - e2*RN_ / (RN_ + h)));
		} while (abs(h - prev_h) > tol && nIterations<10);

        lat =rad2deg(lat);
        lon =rad2deg(lon);
	}
	
};





#endif
