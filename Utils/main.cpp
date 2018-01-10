#include "stdUtils.h"
#include "Geodesy.h"
#include "rotationEx.h"

int TestGeodesy()
{
	double x = 0, y = 0, z = 0, lon, lat, h;
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

int TestEulerAnglesToRzRyRx()
{
	double eulerinput1[3]{ 10,20,30 };
	double Rinput1[9];
	ceres::CreateRotationRzRyRx(eulerinput1, ceres::RowMajorAdapter3x3(Rinput1));

	double eulers1[3], eulers2[3], testR1[9], testR2[9], diff[9];

	ceres::DecomposeRotationToRzRyRx(ceres::RowMajorAdapter3x3(Rinput1), eulers1, eulers2);

	ceres::CreateRotationRzRyRx(eulers1, ceres::RowMajorAdapter3x3(testR1));
	ceres::CreateRotationRzRyRx(eulers2, ceres::RowMajorAdapter3x3(testR2));

	double q1[4], q2[4],dq[4];
	ceres::RotationMatrixToQuaternion(ceres::RowMajorAdapter3x3<const double>(testR1),q1);
	ceres::RotationMatrixToQuaternion(ceres::RowMajorAdapter3x3<const double>(testR2),q2);
	Subtractn<double, 4>(dq, q1, q2);
	double n41 = Norm41(dq);
	if (n41 > 0.0000001)
		return -1;

	double eulerinputambig[3]{ -10,50,20 };
	double Rinputamb[9];
	ceres::CreateRotationRzRyRx(eulerinputambig, ceres::RowMajorAdapter3x3(Rinputamb));
	ceres::DecomposeRotationToRzRyRx(ceres::RowMajorAdapter3x3(Rinputamb), eulers1, eulers2);

	for (int i = 0; i < 3; ++i)
	{
		if (abs(eulers1[i] - eulerinputambig[i]) > 0.000001)
			return -1;
	}
	
	return 0;
}

int main()
{
  TestEulerAnglesToRzRyRx();
  TestGeodesy();

  std::vector<int> vec1;
  vec1.push_back(1);
  vec1.push_back(2);
  vec1.push_back(3);
  vec1.push_back(4);


  if (*FindClosestValueInAscendingVector(vec1, 0.1) != 1)
	  return -1;
  if (*FindClosestValueInAscendingVector(vec1, 3.1) != 3)
	  return -1;
  if (*FindClosestValueInAscendingVector(vec1, 2) != 2)
	  return -1;
  if (*FindClosestValueInAscendingVector(vec1, 2.6) != 3)
	  return -1;
  if (*FindClosestValueInAscendingVector(vec1, 2.5) != 3)
	  return -1;
  if (*FindClosestValueInAscendingVector(vec1, 2.3) != 2)
	  return -1;

  



  return 0;
}