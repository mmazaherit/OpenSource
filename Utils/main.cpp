#include "stdUtils.h"
#include "Geodesy.h"
#include "rotationEx.h"

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

	double eulerinputambig[3]{ -10,90,0 };
	double Rinputamb[9];
	ceres::CreateRotationRzRyRx(eulerinputambig, ceres::RowMajorAdapter3x3(Rinputamb));
	ceres::DecomposeRotationToRzRyRx(ceres::RowMajorAdapter3x3(Rinputamb), eulers1, eulers2);

	ceres::CreateRotationRzRyRx(eulers1, ceres::RowMajorAdapter3x3(testR1));
	return 0;
}

int main()
{
  TestEulerAnglesToRzRyRx();
  TestGeodesy();
  return 0;
}