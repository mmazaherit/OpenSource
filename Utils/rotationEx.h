#pragma  once
#include "rotation.h"
namespace ceres
{
  // ceres default rotation is Rz*Ry*Rx
  //************************************
  // Method:    CreateRzRyRx
  // Qualifier:
  // Parameter: const T * t_xyz , array of sequential rotation in degrees around x , y , z axis
  //************************************
  template <typename T, int row_stride, int col_stride>
  static void CreateRotationRzRyRx(
    const T *t_xyz,
    const MatrixAdapter<T, row_stride, col_stride>& R)
  {
    const double kPi = 3.14159265358979323846;
    const T degrees_to_radians(kPi / 180.0);
    
    const T omega(t_xyz[0] * degrees_to_radians);
    const T phi(t_xyz[1] * degrees_to_radians);
    const T kapa(t_xyz[2] * degrees_to_radians);
    
    const T c1 = cos(kapa);
    const T s1 = sin(kapa);
    const T c2 = cos(phi);
    const T s2 = sin(phi);
    const T c3 = cos(omega);
    const T s3 = sin(omega);
    
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
  
  // photogrammetric convention M=Mz*My*Mx  where M=R'  
  //************************************
  // Method:    EulerAnglesToMzMyMx
  // FullName:  ceres::EulerAnglesToMzMyMx
  // Access:    public static
  // Returns:   void
  // Qualifier:
  // Parameter: const T * t_xyz array of sequential rotation in degrees around x , y , z axis
  //************************************
  template <typename T, int row_stride, int col_stride>
  static void CreateRotationMzMyMx(
    const T *t_xyz,
    const MatrixAdapter<T, row_stride, col_stride>& MzMyMx)
  {
    const double kPi = 3.14159265358979323846;
    const T degrees_to_radians(kPi / 180.0);
    
    const T tx(t_xyz[0] * degrees_to_radians);
    const T ty(t_xyz[1] * degrees_to_radians);
    const T tz(t_xyz[2] * degrees_to_radians);
    
    const T costz = cos(tz);
    const T sintz = sin(tz);
    const T costy = cos(ty);
    const T sinty = sin(ty);
    const T costx = cos(tx);
    const T sintx = sin(tx);
    
    MzMyMx(0, 0) = costz*costy;
    MzMyMx(0, 1) = sintz*costx + costz*sinty*sintx;
    MzMyMx(0, 2) = sintz*sintx - costz*sinty*costx;
    
    MzMyMx(1, 0) = -sintz*costy;
    MzMyMx(1, 1) = costz*costx - sintz*sinty*sintx;
    MzMyMx(1, 2) = costz*sintx + sintz*sinty*costx;
    
    MzMyMx(2, 0) = sinty;
    MzMyMx(2, 1) = -costy*sintx;
    MzMyMx(2, 2) = costy*costx;
  }
  
  //two sets of Euler angles corresponds to the same rotation matrix
  //************************************
  // Method:    RotationToRzRyRx, decomposes a rotation matrix to sequential 2D rotations RzRyRx=Rz*Ry*Rx
  // Parameter: const T * RzRyRx
  // Parameter: T * t1_xyz  array of sequential rotation in degrees around x , y , z axis
  // Parameter: T * t2_xyz  array of sequential rotation in degrees around x , y , z axis
  //************************************
  template <typename T, int row_stride, int col_stride>
  static inline bool DecomposeRotationToRzRyRx(const MatrixAdapter<T, row_stride, col_stride>& RzRyRx, T *t1_xyz, T *t2_xyz)
  {
    //http://www.staff.city.ac.uk/~sbbh653/publications/euler.pdf
    const double kPi = 3.14159265358979323846;
    const T radians2degreee(180.0 / kPi);
    
    if (RzRyRx(2, 0) != T(1.0) && RzRyRx(2, 0) != T(-1.0))
    {
      //no ambigiuty
      
      t1_xyz[1] = -asin(RzRyRx(2, 0))*radians2degreee;
      t2_xyz[1] = T(180.0) - t1_xyz[1];
      
      T cosPhi1 = cos(t1_xyz[1] / radians2degreee);
      T cosPhi2 = cos(t2_xyz[1] / radians2degreee);
      
      t1_xyz[0] = atan2(RzRyRx(2, 1) / cosPhi1, RzRyRx(2, 2) / cosPhi1)*radians2degreee;
      t2_xyz[0] = atan2(RzRyRx(2, 1) / cosPhi2, RzRyRx(2, 2) / cosPhi2)*radians2degreee;
      
      t1_xyz[2] = atan2(RzRyRx(1, 0) / cosPhi1, RzRyRx(0, 0) / cosPhi1)*radians2degreee;
      t2_xyz[2] = atan2(RzRyRx(1, 0) / cosPhi2, RzRyRx(0, 0) / cosPhi2)*radians2degreee;
      return true;
    }
    else if (RzRyRx(2, 0) == T(-1.0))
    {
      t1_xyz[2]= t2_xyz[2] = T(0.0);// can be anything
      t1_xyz[1]= t2_xyz[1] = T(90.0);
      t1_xyz[0]= t2_xyz[0] = t1_xyz[2] +atan2(RzRyRx(0,1),  RzRyRx(0,2))*radians2degreee;
      return false;
    }
    else if(RzRyRx(2, 0) == T(1.0))
    {
      t1_xyz[2] = t2_xyz[2] = T(0.0);// can be anything
      t1_xyz[1] = t2_xyz[1] = T(-90.0);
      t1_xyz[0] = t2_xyz[0] = -t1_xyz[2] + atan2(-RzRyRx(0, 1), -RzRyRx(0, 2))*radians2degreee;
      return false;
    }
    
    return false;
  }

  //************************************
  // Method:    DecomposeRotationToNovatelRPY  
  // Access:    public static 
  // Returns:   void
  // Qualifier: 
  // Parameter: col_stride> & R  rotation from child to parent (body to local frame)
  // Parameter: T * RPY in degrees
  //************************************
  template <typename T, int row_stride, int col_stride>
  static inline void  DecomposeRotationToNovatelRPY(const MatrixAdapter<T, row_stride, col_stride>& R, T *RPY)
  {
    const double kPi = 3.14159265358979323846;
    const T radians2degreee(180.0 / kPi);
    
    RPY[0] = atan2(-R(2, 0),  R(2, 2))*radians2degreee;
    RPY[1] = asin( R(2, 1))*radians2degreee;
    RPY[2] = atan2(-R(0, 1), R(1, 1))*radians2degreee;
  }
  //************************************
  // Method:    NovatelRPYToRotation
  // FullName:  ceres::NovatelRPYToRotation
  // Access:    public static
  // Returns:   void
  // Qualifier:
  // Parameter: const T * RPY roll pitch yaw in degrees based on novatel output https://www.novatel.com/assets/Documents/Bulletins/apn037.pdf
  // Roll is around Y, Pitch is around X, Yaw is around Z all in degrees, Novatel azimuth is positive in clockwise, so it must be negated to become Yaw
  // Parameter: const int row_stride_parameter
  // Parameter: T * R   Rotaton of body to loval level frame (Rb2l)
  //************************************
  template <typename T, int row_stride, int col_stride>
  static void CreateRotationNovatelRPY(const T *RPY,const MatrixAdapter<T, row_stride, col_stride>& R)
  {
    const double kPi = 3.14159265358979323846;
    const T degrees_to_radians(kPi / 180.0);
    
    const T Roll(RPY[0] * degrees_to_radians);
    const T Pitch(RPY[1] * degrees_to_radians);
    const T Yaw(RPY[2] * degrees_to_radians);
    
    const T cosyaw = cos(Yaw);
    const T sinyaw = sin(Yaw);
    const T cospitch = cos(Pitch);
    const T sinpitch = sin(Pitch);
    const T cosroll = cos(Roll);
    const T sinroll = sin(Roll);
    
    R(0, 0) = cosroll*cosyaw - sinpitch*sinroll*sinyaw;
    R(0, 1) = -cospitch*sinyaw;
    R(0, 2) = cosyaw*sinroll + cosroll*sinpitch*sinyaw;
    
    R(1, 0) = cosroll*sinyaw + cosyaw*sinpitch*sinroll;
    R(1, 1) = cospitch*cosyaw;
    R(1, 2) = sinroll*sinyaw - cosroll*cosyaw*sinpitch;
    
    R(2, 0) = -cospitch*sinroll;
    R(2, 1) = sinpitch;
    R(2, 2) = cospitch*cosroll;
  }
  
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
  static inline void TransposeRotation(const T *const R, T *Rt)
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
  
  //R is row major
  template <typename T>
  static inline void RotatePoint(const T *const R_rowMajor, const T *const Pt, T *RotatedPoint)
  {
    RotatedPoint[0] = R_rowMajor[0] * Pt[0] + R_rowMajor[1] * Pt[1] + R_rowMajor[2] * Pt[2];
    RotatedPoint[1] = R_rowMajor[3] * Pt[0] + R_rowMajor[4] * Pt[1] + R_rowMajor[5] * Pt[2];
    RotatedPoint[2] = R_rowMajor[6] * Pt[0] + R_rowMajor[7] * Pt[1] + R_rowMajor[8] * Pt[2];
  }
  //https://www.mathworks.com/help/aeroblks/quaternionmultiplication.html
  template <typename T>
  static inline void RotationQuaternionProduct(const T *const q, const T *const r, T *qr)
  {
    qr[0] = r[0] * q[0] - r[1] * q[1] - r[2] * q[2] - r[3] * q[3];
    qr[1] = r[0] * q[1] + r[1] * q[0] - r[2] * q[3] + r[3] * q[2];
    qr[2] = r[0] * q[2] + r[1] * q[3] + r[2] * q[0] - r[3] * q[1];
    qr[3] = r[0] * q[3] - r[1] * q[2] + r[2] * q[1] + r[3] * q[0];
    
    T nrmqr = Norm41(qr);
    Scale41(T(1.0) / nrmqr, qr);
  }
  
}


