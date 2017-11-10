class Geodesy
{
 // WGS-84 geodetic constants
 static double constexpr a = 6378137;           // WGS-84 Earth semimajor axis (m)
 static	double constexpr b = 6356752.3142;      // WGS-84 Earth semiminor axis (m)
 static	double constexpr f = (a - b) / a;           // Ellipsoid Flatness
 static	double constexpr e_sq = f * (2 - f);    // Square of Eccentricity

//http://clynchg3c.com/Technote/geodesy/coordcvt.pdf
// Converts WGS-84 Geodetic point (lat, lon, h) to the
// Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z).
//lat long are degrees, h is meter
void Geodesy::GeodeticToEcef(double lat, double lon, double h, double &x, double &y, double &z)
{
    // Convert to radians in notation consistent with the paper:
    double  lambda = lat*DEG2RAD;
    double  phi = lon*DEG2RAD;
    double  s = sin(lambda);
    double  N = a / sqrt(1 - e_sq * s * s);

    double  sin_lambda = sin(lambda);
    double  cos_lambda = cos(lambda);
    double  cos_phi = cos(phi);
    double  sin_phi = sin(phi);

    x = (h + N) * cos_lambda * cos_phi;
    y = (h + N) * cos_lambda * sin_phi;
    z = (h + (1 - e_sq) * N) * sin_lambda;
}
void Geodesy::EcefToGeodetic(double &lat, double &lon, double &h, double x, double y, double z)
{
    lon=atan2(x,y);
    const double r=sqrt(x*x+y*y+z*z);
    const double p=sqrt(x*x+y*y);
    lat=atan2(p,z);

    while()
    {

    }

}

}

