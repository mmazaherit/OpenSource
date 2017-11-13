#ifndef DEG2RAD
#define DEG2RAD  0.0174532925199433
#endif
#ifndef RAD2DEG
#define RAD2DEG  57.2957795130823
#endif
#ifndef PI
#define PI  3.14159265358979
#endif

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
		const double sinphi2 = sin(phi*DEG2RAD)*sin(phi*DEG2RAD);
		return a*(1.0 - e2) / pow(1.0 - e2*sinphi2, 1.50);
	}
	inline double RN(double phi)
	{		
		const double sinphi2 = sin(phi*DEG2RAD)*sin(phi*DEG2RAD);
		return a / sqrt(1.0 - e2*sinphi2);
	}
   //http://clynchg3c.com/Technote/geodesy/coordcvt.pdf
   // Converts WGS-84 Geodetic point (lat, lon, h) to the
   // Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z).
   //lat long are degrees, h is meter
	void GeodeticToEcef(double lat, double lon, double h, double &x, double &y, double &z)
	{		
		double  N = RN(lat);

		
		double  sin_lambda = sin(lon*DEG2RAD);
		double  cos_lambda = cos(lon*DEG2RAD);
		double  cos_phi = cos(lat*DEG2RAD);
		double  sin_phi = sin(lat*DEG2RAD);

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
		do
		{
			++nIterations;
			double RN_ = RN(lat*RAD2DEG);
			prev_h = h;
			h = p / cos(lat) -RN_;
			lat = atan(z /p/(1.0 - e2*RN_ / (RN_ + h)));
		} while (abs(h - prev_h) > tol && nIterations<10);

		lat *= RAD2DEG;
		lon *= RAD2DEG;
	}
	
};


int TestGeodesy() 
{
	double x=0, y=0, z=0, lon, lat ,h ;
	double input1[6] = { 37.8043722, -122.2708026,0.0 };
	double output1[3] = { -2694044.4111565403, -4266368.805493665, 3888310.602276871 };

	Geodesy geodesy;
	geodesy.GeodeticToEcef(input1[0], input1[1], input1[2], x, y, z);
	
	double dx = abs(x - output1[0]);
	double dy = abs(y - output1[1]);
	double dz = abs(z - output1[2]);
	if (dx > 0.001 || dy > 0.001 || dz > 0.001)
		return -1;

	geodesy.EcefToGeodetic(lat, lon, h, x, y, z);

	if (abs(lat - input1[0]) > 1e-6 || abs(lon - input1[1]) > 1e-6 || abs(h - input1[2]) > 1e-6)
		return -1;

	return 0;
}
