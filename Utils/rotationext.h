#pragma  once
#include "ceres/rotation.h"
namespace ceres
{

	// apply a RX+T where R is generated by 3 angle-axis parameters and then point is translated by the last 3 elements of angle_axis_translation
	//angle_axis_translation =[3 angle-axis parameters, 3 translation]
	template<typename T> inline
		static void AngleAxisTransformPoint(const T angle_axis_translation[6], const T pt[3], T result[3])
	{
		ceres::AngleAxisRotatePoint(angle_axis_translation, pt, result);
		result[0] += angle_axis_translation[3];
		result[1] += angle_axis_translation[4];
		result[2] += angle_axis_translation[5];

	}

	template <typename T>
	static inline void EulerAnglesToRzRyRx(const T* euler,
		const int row_stride_parameter,
		T* R) {
		CHECK_EQ(row_stride_parameter, 3);
		EulerAnglesToRzRyRx(euler, RowMajorAdapter3x3(R));
	}
	// ceres default rotation is Rz*Ry*Rx
	template <typename T, int row_stride, int col_stride>
	static void EulerAnglesToRzRyRx(
		const T* euler,
		const MatrixAdapter<T, row_stride, col_stride>& R) {
		const double kPi = 3.14159265358979323846;
		const T degrees_to_radians(kPi / 180.0);

		const T pitch(euler[0] * degrees_to_radians);
		const T roll(euler[1] * degrees_to_radians);
		const T yaw(euler[2] * degrees_to_radians);

		const T c1 = cos(yaw);
		const T s1 = sin(yaw);
		const T c2 = cos(roll);
		const T s2 = sin(roll);
		const T c3 = cos(pitch);
		const T s3 = sin(pitch);

		R(0, 0) = c1*c2;
		R(0, 1) = -s1*c3 + c1*s2*s3;
		R(0, 2) = s1*s3 + c1*s2*c3;

		R(1, 0) = s1*c2;
		R(1, 1) = c1*c3 + s1*s2*s3;
		R(1, 2) = -c1*s3 + s1*s2*c3;

		R(2, 0) = -s2;
		R(2, 1) = c2*s3;
		R(2, 2) = c2*c3;
	}

	// photogrammetric convention Mz*My*Mx  where M=R'
	template <typename T>
	static inline void EulerAnglesToMzMyMx(const T* euler,
		const int row_stride_parameter,
		T* R) {
		CHECK_EQ(row_stride_parameter, 3);
		EulerAnglesToMzMyMx(euler, RowMajorAdapter3x3(R));
	}


	template <typename T, int row_stride, int col_stride>
	static void EulerAnglesToMzMyMx(
		const T* euler,
		const MatrixAdapter<T, row_stride, col_stride>& M) {
		const double kPi = 3.14159265358979323846;
		const T degrees_to_radians(kPi / 180.0);

		const T omega(euler[0] * degrees_to_radians);
		const T phi(euler[1] * degrees_to_radians);
		const T kappa(euler[2] * degrees_to_radians);

		const T coskappa = cos(kappa);
		const T sinkappa = sin(kappa);
		const T cosphi = cos(phi);
		const T sinphi = sin(phi);
		const T cosomega = cos(omega);
		const T sinomega = sin(omega);

		M(0, 0) = coskappa*cosphi;
		M(0, 1) = sinkappa*cosomega + coskappa*sinphi*sinomega;
		M(0, 2) = sinkappa*sinomega - coskappa*sinphi*cosomega;

		M(1, 0) = -sinkappa*cosphi;
		M(1, 1) = coskappa*cosomega - sinkappa*sinphi*sinomega;
		M(1, 2) = coskappa*sinomega + sinkappa*sinphi*cosomega;

		M(2, 0) = sinphi;
		M(2, 1) = -cosphi*sinomega;
		M(2, 2) = cosphi*cosomega;
	}


	template <typename T>
	static inline void RotationMzMyMxToEuler(const int row_stride_parameter, const T* M, T*euler) {
		CHECK_EQ(row_stride_parameter, 3);
		EulerAnglesToMzMyMx(RowMajorAdapter3x3(M), euler);
	}

	template <typename T, int row_stride, int col_stride>
	static inline void	RotationMzMyMxToEuler(const MatrixAdapter<T, row_stride, col_stride>& M, T* euler)
	{
		const double kPi = 3.14159265358979323846;
		const T radians2degreee(180.0 / kPi);

		euler[0] = atan(-M(2, 1) / M(2, 2))*radians2degreee;
		euler[1] = asin(M(2, 0))*radians2degreee;
		euler[2] = atan(-M(1, 0) / M(0, 0))*radians2degreee;
	}

	template <typename T>
	static inline void TransposeRotation(const T* const R, T* Rt)
	{
		Rt[0] = R[0];
		Rt[1] = R[3];
		Rt[2] = R[6];

		Rt[3] = R[1];
		Rt[4] = R[4];
		Rt[5] = R[7];

		Rt[6] = R[2];
		Rt[7] = R[5];
		Rt[8] = R[8];

	}

	template <typename T>
	static inline void RotatePoint(const T* const R, const T* const Pt, T* RotatedPoint)
	{
		RotatedPoint[0] = R[0] * Pt[0] + R[1] * Pt[1] + R[2] * Pt[2];
		RotatedPoint[1] = R[3] * Pt[0] + R[4] * Pt[1] + R[5] * Pt[2];
		RotatedPoint[2] = R[6] * Pt[0] + R[7] * Pt[1] + R[8] * Pt[2];
	}

https://www.mathworks.com/help/aeroblks/quaternionmultiplication.html
	template <typename T>
	static inline void quatmul(const T* const q, const T* const r, T* qr)
	{
		qr[0] = r[0] * q[0] - r[1] * q[1] - r[2] * q[2] - r[3] * q[3];
		qr[1] = r[0] * q[1] + r[1] * q[0] - r[2] * q[3] + r[3] * q[2];
		qr[2] = r[0] * q[2] + r[1] * q[3] + r[2] * q[0] - r[3] * q[1];
		qr[3] = r[0] * q[3] - r[1] * q[2] + r[2] * q[1] + r[3] * q[0];		
	}
}