using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using Mazaheri;
using System.IO;
namespace Photo
{
        public  class DPoint
        {
            public double x=new double();
            public double y = new double();
            public double z = new double();
            public DPoint(){this.x=0;this.y=0;this.z=0;}
            public DPoint(Matrix M)
            {

                if ((M.Cols != 1 || M.Rows != 3) && (M.Rows != 1 || M.Cols != 3))
                    throw (new ApplicationException("Cannot assign Matrix to point"));
                this.x = M[1, 1];
                this.y = (M.Cols > 1) ? M[1, 2] : M[2, 1];
                this.z = (M.Cols > 1) ? M[1, 3] : M[3, 1];
            }
            public DPoint(double  x, double  y, double  z) { this.x = x; this.y = y; this.z = z; }
            public DPoint(object  x, object  y, object  z) { this.x = Convert.ToDouble( x); this.y = Convert.ToDouble(y); this.z = Convert.ToDouble(z); }
            public DPoint(double x, double y, double z, Color CL) { this.x = x; this.y = y; this.z = z; this.Color = CL; }
            public Matrix MatrixForm
            {
                get 
                {
                    Matrix tmp = new Matrix(3, 1);
                    tmp[1,1] = x; tmp[2,1] = y; tmp[3,1] = z;
                    return tmp;
                }
                set 
                {
                    if ((value.Cols != 1 || value.Rows != 3) && (value.Rows != 1 || value.Cols != 3))
                        throw (new ApplicationException("Cannot assign Matrix to point"));
                    this.x = value[1, 1];
                    this.y = (value.Cols > 1) ? value[1, 2] : value[2,1];
                    this.z = (value.Cols > 1) ? value[1,3] : value[3, 1];


                    
                }
            } 

              
        public  Color  Color;

        }
    
    
        public struct Ctrlpoint
        {
            public DPoint Pic;
            public DPoint Earth;
        }


    public class Camera
    {
        public Camera()
        {
            this.Omega = this.Kapa = this.Fi = 0;
        }
        private double omega = 0, fi = 0, kapa = 0;
        public double FocalLength = 0;
        public SizeD PixelSize = new SizeD(0, 0);
        public DPoint Position = new DPoint(0, 0, 0);
        public Matrix R = new Matrix(3, 3);
        /// <summary>
        /// Rotation marix  R(k).R(phi).R(omega)
        /// </summary>
        public Size Resolution = new Size(0, 0);
        
 
        public DPoint PP
        {
            get
            {
                return new DPoint(Resolution.Width / 2, Resolution.Height / 2, 0);
            }
        }
        /// <summary>
        /// returns principal Point of a camera according to resulotion
        /// </summary>
        public double Omega
        {

            set
            {
                this.omega = value;
                this.R = Transforms.R(value, this.Fi, this.Kapa);
            }
            get { return this.omega; }
        }
        public double Fi
        {
            set
            {
                this.fi = value;
                this.R = Transforms.R(this.Omega, value, this.Kapa);
            }
            get { return fi; }
        }
        public double Kapa
        {
            set
            {
                this.kapa = value;
                this.R = Transforms.R(this.Omega, this.Fi, value);
            }
            get { return this.kapa; }
        }

        // Get and set 6 exterior elements in matrix form 
        public Matrix Orientation6Elements
        {
            get
            {
                Matrix ans = new Matrix(6, 1);
                ans[1, 1] = this.Omega;
                ans[2, 1] = this.Fi;
                ans[3, 1] = this.Kapa;
                ans[4, 1] = this.Position.x;
                ans[5, 1] = this.Position.y;
                ans[6, 1] = this.Position.z;
                return ans;
            }
            set
            {
                if (value.Rows != 6 || value.Cols != 1)
                    throw (new ApplicationException("Matrix must be 6*1 to set orientation Elements"));
                this.Omega = value[1, 1];
                this.Fi = value[2, 1];
                this.Kapa = value[3, 1];
                this.Position.x = value[4, 1];
                this.Position.y = value[5, 1];
                this.Position.z = value[6, 1];
            }
        }

        // Save a camera settings in a Text file
        public void SaveCamera(string Path)
        {
            StreamWriter St = new StreamWriter(Path);
            St.WriteLine("FocalLength=" + this.FocalLength.ToString());
            St.WriteLine("PixelSize=" + this.PixelSize.Width.ToString() + '*' + this.PixelSize.Height.ToString());
            St.WriteLine("Resolution=" + this.Resolution.Width.ToString() + '*' + this.Resolution.Height.ToString());
            St.WriteLine("Omega=" + this.Omega.ToString());
            St.WriteLine("Phi=" + this.Fi.ToString());
            St.WriteLine("Kapa=" + this.Kapa.ToString());
            St.WriteLine("XC=" + this.Position.x.ToString());
            St.WriteLine("YC=" + this.Position.y.ToString());
            St.Write("ZC=" + this.Position.z.ToString());
            St.Close();
        }
        // Load a camera setting
        public void LoadCamera(string Path)
        {
            StreamReader St = new StreamReader(Path);
            string t = St.ReadLine().Split('=')[1];
            this.FocalLength = Convert.ToDouble(t);
            t = St.ReadLine().Split('=')[1];
            this.PixelSize = new SizeD(Convert.ToDouble(t.Split('*')[0]), Convert.ToDouble(t.Split('*')[1]));
            t = St.ReadLine().Split('=')[1];
            this.Resolution = new Size(Convert.ToInt32(t.Split('*')[0]), Convert.ToInt32(t.Split('*')[1]));
            this.Omega = Convert.ToDouble(St.ReadLine().Split('=')[1]);
            this.Fi = Convert.ToDouble(St.ReadLine().Split('=')[1]);
            this.Kapa = Convert.ToDouble(St.ReadLine().Split('=')[1]);
            this.Position.x = Convert.ToDouble(St.ReadLine().Split('=')[1]);
            this.Position.y = Convert.ToDouble(St.ReadLine().Split('=')[1]);
            this.Position.z = Convert.ToDouble(St.ReadLine().Split('=')[1]);

            St.Close();
        }
    }

       
    public class  SizeD 
    {
        public  double Width = 0;
        public  double Height = 0;
        public SizeD(double Width,double Height) 
        {
            this.Width = Width;
            this.Height = Height;
        }
    }
        class Transforms
        {
       

            public static  int[] xp = new int[] { 0, 1, 0, 2, 0, 2, 1, 2, 3, 0, 3, 1, 3, 2, 3, 4, 0, 4, 1, 4, 2, 4, 3, 4 };
            public static int[] yp = new int[] { 0, 0, 1, 0, 2, 1, 2, 2, 0, 3, 1, 3, 2, 3, 3, 0, 4, 1, 4, 2, 4, 3, 4, 4 };

            public static Matrix CalcAffineFactors(ref Matrix space1, ref Matrix space2)
        {
            int m, n;
            m = space1.Rows;
            n = space2.Rows;
            if (m != n)
                throw (new ApplicationException("Space 1,2 points number are not equal"));
            Matrix F = new Matrix(2 * m, 6);
            Matrix A = new Matrix(2 * m, 1);
            Matrix ans = new Matrix(2 * m, 1);

            int i, j;
            //filling Matrix f in m row and 6 columns
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                F[i, 1] = F[i + 1, 3] = space1[j].x;
                F[i, 2] = F[i + 1, 4] = space1[j].y;
                F[i, 5] = F[i + 1, 6] = 1;

                A[i, 1] = space2[j].x; A[i + 1, 1] = space2[j].y;
            }
            ans = !(~F * F) * ~F * A;
            return ans;
        }
            public static Matrix AffineTrans(ref Matrix space1, ref Matrix Affinefacs)
        {
            int m;
            m = space1.Rows;

            Matrix F = new Matrix(2 * m, 6);
            Matrix A = new Matrix(2 * m, 1);
            Matrix ans = new Matrix(m,3);

            int i, j;
            //filling Matrix f in m row and 6 columns
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                F[i, 1] = F[i + 1, 3] = space1[j].x;
                F[i, 2] = F[i + 1, 4] = space1[j].y;
                F[i, 5] = F[i + 1, 6] = 1;


            }

            A = F * Affinefacs;
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                ans[j]=new DPoint(A[i, 1],A[i + 1, 1],0);
            }

            return ans;
        }
            public static Matrix CalcDistortionFactors(ref Matrix Distances, ref Matrix Distortions)
        {
            int m, n, i;
            m = Distances.Rows;
            n = Distortions.Rows;
            if (m != n)
                throw (new ApplicationException("distances and distortions are not equal"));
            Matrix F = new Matrix(m, 4);
            Matrix A = Distortions;
            Matrix ans = new Matrix(5, 1);
            for (i = 1; i <= m; i++)
            {
                F[i, 1] = Distances[i, 1];
                F[i, 2] = Math.Pow(F[i, 1], 3);
                F[i, 3] = Math.Pow(F[i, 1], 5);
                F[i, 4] = Math.Pow(F[i, 1], 7);

            }
            ans = !(~F * F) * ~F * A;
            return ans;
        }
            public static Matrix DistortionCorrections(ref Matrix points, ref Matrix DistortionFactors)
        {
            int m, i;
            double r, c1, c2, c3, c4, C;
            c1 = DistortionFactors[1, 1]; c2 = DistortionFactors[2, 1]; c3 = DistortionFactors[3, 1]; c4 = DistortionFactors[4, 1];
            m = points.Rows;
            Matrix ans = new Matrix (m ,3);

            //Applynig the distortion factors
            for (i = 1; i <= m; i++)
            {
                r = Math.Sqrt(points[i].x * points[i].x + points[i].y * points[i].y);
                C = 1 - c1 - c2 * r * r - c3 * Math.Pow(r, 4) - c4 * Math.Pow(r, 6);
                ans[i] =new DPoint(  points[i].x * C,points[i].y * C,0);
                
            }
            return ans;
        }
            public static Matrix CalcProjectiveFactors(ref Matrix space1, ref Matrix space2)
        {
            int m, n;
            m = space1.Rows;
            n = space2.Rows;
            if (m != n)
                throw (new ApplicationException("Space 1,2 points number are not equal"));
            Matrix F = new Matrix(2 * m, 8);
            Matrix A = new Matrix(2 * m, 1);
            Matrix ans = new Matrix(2 * m, 1);

            int i, j;
            //filling Matrix f in m row and 8 columns
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                F[i, 1] = F[i + 1, 2] = space1[j].x;
                F[i, 4] = F[i + 1, 5] = space1[j].y;
                F[i, 3] = -space1[j].x * space2[j].x;
                F[i + 1, 6] = -space1[j].y * space2[j].y;
                F[i, 6] = -space1[j].x * space2[j].y;
                F[i + 1, 3] = -space2[j].x * space1[j].y;
                F[i, 7] = F[i + 1, 8] = 1;

                A[i, 1] = space2[j].x; A[i + 1, 1] = space2[j].y;
            }
            ans = !(~F * F) * ~F * A;
            return ans;
        }
            public static Matrix ProjectiveTrans(ref Matrix space1, ref Matrix projectivefacs)
        {
            double a1 = projectivefacs[1, 1];
            double a2 = projectivefacs[2, 1];
            double a3 = projectivefacs[3, 1];
            double b1 = projectivefacs[4, 1];
            double b2 = projectivefacs[5, 1];
            double b3 = projectivefacs[6, 1];
            double c1 = projectivefacs[7, 1];
            double c2 = projectivefacs[8, 1];
            int m;
            m = space1.Rows;
            Matrix ans = new Matrix(m ,3);
            int j;
            //filling Matrix f in m row and 6 columns

            for (j = 1; j <= m; j++)
            {

                 double  x = (a1 * space1[j].x + b1 * space1[j].y + c1) / (a3 * space1[j].x + b3 * space1[j].y + 1);
                 double  y  = (a2 * space1[j].x + b2 * space1[j].y + c2) / (a3 * space1[j].x + b3 * space1[j].y + 1);
                 ans[j] = new DPoint(x, y, 0);
            }

            return ans;
        }
            public static Matrix CalcConformalFactors(ref Matrix space1, ref Matrix space2)
        {
            int m, n;
            m = space1.Rows;
            n = space2.Rows;
            if (m != n)
                throw (new ApplicationException("Space 1,2 points number are not equal"));
            Matrix F = new Matrix(2 * m, 4);
            Matrix A = new Matrix(2 * m, 1);
            Matrix ans = new Matrix(2 * m, 1);

            int i, j;
            //filling Matrix f in m row and 6 columns
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                F[i, 1] = F[i + 1, 2] = space1[j].x;
                F[i, 2] = -space1[j].y;
                F[i + 1, 1] = space1[j].y;
                F[i, 3] = F[i + 1, 4] = 1;

                A[i, 1] = space2[j].x; A[i + 1, 1] = space2[j].y;
            }
            ans = !(~F * F) * ~F * A;
            return ans;
        }
            public static Matrix ConformalTrans(ref Matrix space1, ref Matrix Conformalfacs)
        {
            int m;
            m = space1.Rows;

            Matrix F = new Matrix(2 * m, 4);
            Matrix A = new Matrix(2 * m, 1);
            Matrix ans = new Matrix (m ,3);

            int i, j;
            //filling Matrix f in m row and 6 columns
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                F[i, 1] = F[i + 1, 2] = space1[j].x;
                F[i, 2] = -space1[j].y;
                F[i + 1, 1] = space1[j].y;
                F[i, 3] = F[i + 1, 4] = 1;

            }

            A = F * Conformalfacs;
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                ans[j]=new DPoint(A[i, 1], A[i + 1, 1],0);
            }

            return ans;
        }
            public static Matrix CalcPolynomialFactors(ref Matrix space1, ref Matrix space2, int Terms)
        {

            int m, n;
            m = space1.Rows;
            n = space2.Rows;
            if (m != n)
                throw (new ApplicationException("Space 1,2 points number are not equal"));

            Matrix F = new Matrix(2 * m, 2 * Terms);
            Matrix A = new Matrix(2 * m, 1);
            Matrix ans = new Matrix(2 * m, 1);

            int i, j, k;
            //filling Matrix f in m row and 6 columns
            for (j = 1; j <= m; j++)
            {
                i = 2 * j - 1;
                for (k = 1; k <= Terms; k++)
                {
                    F[i, k] = Math.Pow(space1[j].x, xp[k - 1]) * Math.Pow(space1[j].y, yp[k - 1]);
                    F[i + 1, k + Terms] = Math.Pow(space1[j].x, xp[k - 1]) * Math.Pow(space1[j].y, yp[k - 1]);
                }

                A[i, 1] = space2[j].x; A[i + 1, 1] = space2[j].y;
            }
            ans = !(~F * F) * ~F * A;
            return ans;
        }
            public static Matrix PolynomialTrans(ref Matrix space1, ref Matrix Polynomialfacs)
        {
            int m;
            m = space1.Rows;
            int Terms = Polynomialfacs.Rows / 2;

            Matrix ans = new Matrix (m ,3);
            int j, k;
            //filling Matrix f in m row and 6 columns

            for (j = 1; j <= m; j++)
            {
                for (k = 1; k <= Terms; k++)
                {
                    double  x = ans[j].x + Math.Pow(space1[j].x, xp[k - 1]) * Math.Pow(space1[j].y, yp[k - 1]) * Polynomialfacs[k, 1];
                    double   y = ans[j].y + Math.Pow(space1[j].x, xp[k - 1]) * Math.Pow(space1[j].y, yp[k - 1]) * Polynomialfacs[k + Terms, 1];
                    ans[j] = new DPoint(x, y, 0);
                }

            }

            return ans;
        }
            public static Matrix EarthCurvatureCorrections(ref Matrix points, float EarthRadius, float FocalDist, float FlightHeight)
        {
            int m = points.Rows;
            int i; double H = FlightHeight; float f = FocalDist; float R = EarthRadius; double r, Deltar;
            Matrix ans = new Matrix (m ,3);
            for (i = 1; i <= m; i++)
            {
                r = Math.Sqrt(points[i].x * points[i].x + points[i].y * points[i].y);
                Deltar = H * r * r * r / (2 * R * f * f);
                double  x = points[i].x * (1 + Deltar / r);
                double  y = points[i].y * (1 + Deltar / r);
                ans[i]=new DPoint(x,y,0);
            }
            return ans;
        }
            public static Matrix RefrcationCorrections(ref Matrix points, float FocalDist, float FlightHeight, float h)
        {
            int m = points.Rows;
            int i; double H = FlightHeight; float f = FocalDist; double r, Deltar;
            double K = 7.4E-4 * (H - h) * (1 - 0.02 * (2 * H - h));

            Matrix ans = new Matrix (m,3);
            for (i = 1; i <= m; i++)
            {
                r = Math.Sqrt(points[i].x * points[i].x + points[i].y * points[i].y);
                Deltar = Math.Sqrt(r * r + f * f) * K * r / (f * f);
                 double x = points[i].x * (1 - Deltar / r);
                 double y = points[i].y * (1 - Deltar / r);
                 ans[i] = new DPoint(x,y,0);
            }
            return ans;
        }
            public static DPoint[]  XYZ2Dpoint(Matrix XYZ)
        {
            int m, n, i;
            Boolean z = new Boolean();
            m = XYZ.Rows;
            n = XYZ.Cols;
            if (n > 2) z = true;
            DPoint[] ans = new DPoint[m+1];
            for (i = 1; i <= m; i++)
            {
                ans[i]=new DPoint( XYZ[i, 1],XYZ[i, 2],z?XYZ[i, 3]:0);
                
            }

            return ans;
        }
            public static Matrix Dpoint2XYZ(Matrix Space)
        {
            int i, m;
            m = Space.Rows;
            Matrix ans = new Matrix(m, 3);
            for (i = 1; i <= m; i++)
            {
                ans[i, 1] = Space[i].x;
                ans[i, 2] = Space[i].y;
                ans[i, 3] = Space[i].z;
            }
            return ans;

        }
            public static Matrix RotX(double Angle)
        {
            Matrix ans = new Matrix(3, 3);
            ans[1, 1] = 1; ans[1, 2] = 0; ans[1, 3] = 0;
            ans[2, 1] = 0; ans[2, 2] = Math.Cos(Angle); ans[2, 3] = Math.Sin(Angle);
            ans[3, 1] = 0; ans[3, 2] = -Math.Sin(Angle); ans[3, 3] = Math.Cos(Angle);
            return ans;
        }
            public static Matrix RotY(double Angle)
        {
            Matrix ans = new Matrix(3, 3);
            ans[1, 1] = Math.Cos(Angle); ans[1, 2] = 0; ans[1, 3] = -Math.Sin(Angle);
            ans[2, 1] = 0; ans[2, 2] = 1; ans[2, 3] = 0;
            ans[3, 1] = Math.Sin(Angle); ans[3, 2] = 0; ans[3, 3] = Math.Cos(Angle);
            return ans;
        }
            public static Matrix RotZ(double Angle)
        {
            Matrix ans = new Matrix(3, 3);
            ans[1, 1] = Math.Cos(Angle); ans[1, 2] = Math.Sin(Angle); ans[1, 3] = 0;
            ans[2, 1] = -Math.Sin(Angle); ans[2, 2] = Math.Cos(Angle); ans[2, 3] = 0;
            ans[3, 1] = 0; ans[3, 2] = 0; ans[3, 3] = 1;
            return ans;
        }
            public static Matrix R(double Omega, double Fi, double Kapa) 
            {
                Matrix ans = new Matrix(3, 3);
                double sinw = Math.Sin(Omega); double cosw = Math.Cos(Omega);
                double sinfi = Math.Sin(Fi); double cosfi = Math.Cos(Fi);
                double sink = Math.Sin(Kapa); double cosk = Math.Cos(Kapa);
                // it is RZ*RY*RX
                ans[1, 1] = cosfi * cosk;
                ans[1, 2] = cosw * sink + sinw * sinfi * cosk;
                ans[1, 3] = sinw * sink - cosw * sinfi * cosk;

                ans[2, 1] = -cosfi * sink;
                ans[2, 2] = cosw * cosk - sinw * sinfi * sink;
                ans[2, 3] = sinw * cosk + cosw * sinfi * sink;

                ans[3, 1] = sinfi;
                ans[3, 2] = -sinw * cosfi;
                ans[3, 3] = cosw * cosfi;
                return ans;
            }
            public static Matrix ExteriorOrientation(ref Matrix PicCPoitns, ref Matrix EarthCPoints, double FocalLength, out Matrix PicResiduals, out Matrix EarthResiduals)
        {
            //part Aproximate Values
            Matrix tmp = CalcConformalFactors(ref PicCPoitns, ref EarthCPoints);
            Matrix ApproximateValues = new Matrix(6, 1);
            ApproximateValues[1, 1] = 0.5;
            ApproximateValues[2, 1] = 0.5;
            ApproximateValues[3, 1] = Math.Atan(tmp[2, 1] / tmp[1, 1]);
            ApproximateValues[4, 1] = tmp[3, 1];
            ApproximateValues[5, 1] = tmp[4, 1];
            int i1;
            double dh = 0;
            for (i1 = 1; i1 <= EarthCPoints.Rows; i1++)
                dh = dh + EarthCPoints[i1].z;

            dh = dh / EarthCPoints.Rows;
            ApproximateValues[6, 1] = Math.Sqrt(tmp[1, 1] * tmp[1, 1] + tmp[2, 1] * tmp[2, 1]) * FocalLength + dh;

            int m, n, i, j;
            m = PicCPoitns.Rows;
            n = EarthCPoints.Rows;
            if (m != n)
                throw (new ApplicationException("Number of Pics point And Earth Points is not equal"));

            double w, fe, k, XC, YC, ZC, XA, YA, ZA, F, G, x, y, f;
            Matrix ans = new Matrix(1, 1);
            Matrix J = new Matrix(2, 6);
            Matrix funcs = new Matrix(2, 1);
            Matrix M; Matrix evalj; Matrix C = new Matrix(2 * m, 6);
            Matrix R;
            Matrix Cte = new Matrix(2 * m, 1); Matrix DX;
            f = FocalLength;
            Matrix earthResiduals = new Matrix(2 * m, 1);
            do
            {
                for (i = 1; i <= m; i++)
                {
                    x = PicCPoitns[i].x; y = PicCPoitns[i].y;
                    XA = EarthCPoints[i].x; YA = EarthCPoints[i].y; ZA = EarthCPoints[i].z;
                    w = ApproximateValues[1, 1]; fe = ApproximateValues[2, 1]; k = ApproximateValues[3, 1];
                    XC = ApproximateValues[4, 1]; YC = ApproximateValues[5, 1]; ZC = ApproximateValues[6, 1];

                    M = ~(RotZ(k) * (RotY(fe) * RotX(w)));
                    F = x + f * (M[1, 1] * (XA - XC) + M[1, 2] * (YA - YC) + M[1, 3] * (ZA - ZC)) / (M[3, 1] * (XA - XC) + M[3, 2] * (YA - YC) + M[3, 3] * (ZA - ZC));
                    G = y + f * (M[2, 1] * (XA - XC) + M[2, 2] * (YA - YC) + M[2, 3] * (ZA - ZC)) / (M[3, 1] * (XA - XC) + M[3, 2] * (YA - YC) + M[3, 3] * (ZA - ZC));
                    evalj = JacExterior(ApproximateValues, new DPoint(XA, YA, ZA), f);
                    for (j = 1; j <= 6; j++)
                    {
                        C[2 * i - 1, j] = evalj[1, j];
                        C[2 * i, j] = evalj[2, j];
                    }

                    Cte[2 * i - 1, 1] = -F;
                    Cte[2 * i, 1] = -G;


                }
                DX = (!(~C * C)) * ~C * Cte;
                //DX = mth.Multiple(mth.Multiple(mth.Inv(mth.Multiple(mth.Transpose(C), C)), mth.Transpose(C)), Cte);
                ApproximateValues = ApproximateValues + DX;
            } while (Math.Abs(DX[1, 1]) > 1E-17);

            //part calcluate residuals
            PicResiduals = -1 * Cte;
            for (i = 1; i <= m; i++)
            {
                x = PicCPoitns[i].x; y = PicCPoitns[i].y;
                XA = EarthCPoints[i].x; YA = EarthCPoints[i].y; ZA = EarthCPoints[i].z;
                w = ApproximateValues[1, 1]; fe = ApproximateValues[2, 1]; k = ApproximateValues[3, 1];
                XC = ApproximateValues[4, 1]; YC = ApproximateValues[5, 1]; ZC = ApproximateValues[6, 1];

                R = RotZ(k) * (RotY(fe) * RotX(w));
                F = XA - (ZA - ZC) * (R[1, 1] * x + R[1, 2] * y + R[1, 3] * -f) / (R[3, 1] * x + R[3, 2] * y + R[3, 3] * -f) - XC;
                G = YA - (ZA - ZC) * (R[2, 1] * x + R[2, 2] * y + R[2, 3] * -f) / (R[3, 1] * x + R[3, 2] * y + R[3, 3] * -f) - YC;
                earthResiduals[2 * i - 1, 1] = F;
                earthResiduals[2 * i, 1] = G;
            }
            EarthResiduals = earthResiduals;
            return ApproximateValues;


        }
            
            private static Matrix JacExterior(Matrix Eleman6, DPoint EarthPoint, double Focallength)
        {
            double sinw = Math.Sin(Eleman6[1, 1]); double sinw2 = sinw * sinw;
            double cosw = Math.Cos(Eleman6[1, 1]); double cosw2 = cosw * cosw;
            double sinfe = Math.Sin(Eleman6[2, 1]); double sinfe2 = sinfe * sinfe;
            double cosfe = Math.Cos(Eleman6[2, 1]); double cosfe2 = cosfe * cosfe;
            double sink = Math.Sin(Eleman6[3, 1]); double sink2 = sink * sink;
            double cosk = Math.Cos(Eleman6[3, 1]); double cosk2 = cosk * cosk;
            double XC = Eleman6[4, 1]; double YC = Eleman6[5, 1]; double ZC = Eleman6[6, 1];
            double XA = EarthPoint.x; double YA = EarthPoint.y; double ZA = EarthPoint.z;
            double f = Focallength;
            Matrix j = new Matrix(2, 6);

            j[1, 1] = -f * (cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-(-sink * cosfe2 * cosw - cosk * sinfe * sinw - sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC));
            j[1, 2] = f * (-cosk * sinfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) + sink * sinfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + cosfe / (cosfe2 + sinfe2) * (ZA - ZC)) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * (cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-cosk * cosfe * cosw / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + sink * cosfe * cosw / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - sinfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC));
            j[1, 3] = f * (-sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC)) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * (cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-(-cosk * cosfe2 * sinw - sink * sinfe * cosw - cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC));
            j[1, 4] = -f * cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * (cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[1, 5] = f * sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) + f * (cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[1, 6] = -f * sinfe / (cosfe2 + sinfe2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) + f * (cosk * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + sink2 * sinfe2 + sink2 * cosfe2 + cosk2 * sinfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2);
            j[2, 1] = f * ((-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (-cosk * cosfe2 * sinw - sink * sinfe * cosw - cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-(-sink * cosfe2 * cosw - cosk * sinfe * sinw - sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC));
            j[2, 2] = f * (cosk * cosfe * sinw / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) - sink * cosfe * sinw / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + sinfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-cosk * cosfe * cosw / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + sink * cosfe * cosw / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - sinfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC));
            j[2, 3] = f * ((cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (-sink * cosfe2 * cosw - cosk * sinfe * sinw - sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC)) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-(-cosk * cosfe2 * sinw - sink * sinfe * cosw - cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC));
            j[2, 4] = -f * (sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[2, 5] = -f * (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) + f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[2, 6] = f * cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) + f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2);
            return j;
        }
            private static Matrix JacExterior2(Matrix Eleman6, double XA, double YA, double ZA, double Focallength)
        {
            double sina = Math.Sin(Eleman6[1, 1]); double sina2 = sina * sina;
            double cosa = Math.Cos(Eleman6[1, 1]); double cosa2 = cosa * cosa;
            double sins = Math.Sin(Eleman6[2, 1]); double sins2 = sins * sins;
            double coss = Math.Cos(Eleman6[2, 1]); double coss2 = coss * coss;
            double sint = Math.Sin(Eleman6[3, 1]); double sint2 = sint * sint;
            double cost = Math.Cos(Eleman6[3, 1]); double cost2 = cost * cost;
            double XC = Eleman6[4, 1]; double YC = Eleman6[5, 1]; double ZC = Eleman6[6, 1];
            double f = Focallength;
            Matrix j = new Matrix(2, 6);

            j[1, 1] = f * (-(sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (-coss * cost * cosa - cost2 * sins * sina - sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC));
            j[1, 2] = f * (-(coss * cost * sina - sins * cost2 * cosa - sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC)) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * (-coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) + sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC));
            j[1, 3] = f * (sins * sint * sina / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + coss * sint * sina / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - cost * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * (-sins * cost / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * cost / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) - sint / (cost2 + sint2) * (ZA - ZC));
            j[1, 4] = f * (sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2);
            j[1, 5] = -f * (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2);
            j[1, 6] = f * sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) + f * (-(sins * cost * sina + coss * cost2 * cosa + coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * cost / (cost2 + sint2);
            j[2, 1] = f * (-(-sins * cost * sina - coss * cost2 * cosa - coss * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (-coss * cost * sina + sins * cost2 * cosa + sins * sint2 * cosa) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) + sint * sina / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC));
            j[2, 2] = f * (-(coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (-sins * cost * cosa + cost2 * coss * sina + coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC)) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * (-coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) + sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC));
            j[2, 3] = f * (sins * sint * cosa / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) + coss * sint * cosa / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - cost * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * (-sins * cost / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * cost / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) - sint / (cost2 + sint2) * (ZA - ZC));
            j[2, 4] = f * (sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2);
            j[2, 5] = f * (coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) - f * (-(sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2);
            j[2, 6] = f * sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) / (-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)) + f * (-(sins * cost * cosa - cost2 * coss * sina - coss * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (XA - XC) - (coss * cost * cosa + cost2 * sins * sina + sins * sint2 * sina) / (coss2 * cosa2 * cost2 + coss2 * cosa2 * sint2 + cost2 * sins2 * sina2 + sins2 * cosa2 * cost2 + sins2 * cosa2 * sint2 + cost2 * coss2 * sina2 + sint2 * sina2 * coss2 + sint2 * sina2 * sins2) * (YA - YC) - sint * cosa / (cost2 * sina2 + cosa2 * cost2 + cosa2 * sint2 + sint2 * sina2) * (ZA - ZC)) / Math.Pow((-sins * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (XA - XC) - coss * sint / (coss2 * cost2 + sins2 * sint2 + coss2 * sint2 + sins2 * cost2) * (YA - YC) + cost / (cost2 + sint2) * (ZA - ZC)), 2) * cost / (cost2 + sint2);
            return j;
        }
            public  static  DPoint Intersection(ref Matrix L6Elements, ref Matrix R6Elements, DPoint LPicPoint, DPoint RPicPoint, double FocalLength)
        {

            Matrix j; double xa, ya, f; Matrix A = new Matrix(4, 3);
            f = FocalLength;
            //calculating approximate values

            DPoint ApproximateValue = new DPoint();
            double B, Px, Py, XCL, YCL, ZCL, XCR, YCR, ZCR;

            Px = LPicPoint.x - RPicPoint.x;
            Py = LPicPoint.y - RPicPoint.y;

            B = R6Elements[4, 1] - L6Elements[4, 1];

            ApproximateValue.x = L6Elements[4, 1] + B * LPicPoint.x / Px;
            ApproximateValue.y = L6Elements[5, 1] + B * LPicPoint.y / Px;
            ApproximateValue.z = L6Elements[6, 1] - B * f / Px;
            XCL = L6Elements[4, 1]; YCL = L6Elements[5, 1]; ZCL = L6Elements[6, 1];
            XCR = R6Elements[4, 1]; YCR = R6Elements[5, 1]; ZCR = R6Elements[6, 1];

            Matrix Cte = new Matrix(4, 1); Matrix tmp = new Matrix(3, 1); Matrix R = new Matrix(3, 3);
            do
            {

                xa = LPicPoint.x; ya = LPicPoint.y;
                //j=JacIntersection2(L6Elements ,xa,ya,FocalLength);
                j = JacIntersection(L6Elements, ApproximateValue, FocalLength);
                A[1, 2, ':'] = j;
                Cte[1, 2, 1, 1] = -1 * ColiniaryEqu(L6Elements, LPicPoint, ApproximateValue, FocalLength);

                xa = RPicPoint.x; ya = RPicPoint.y;

                //  j = JacIntersection2(R6Elements, xa, ya, FocalLength);
                j = JacIntersection(R6Elements, ApproximateValue, FocalLength);
                A[3, 4, ':'] = j;
                Cte[3, 4, 1, 1] = -1 * ColiniaryEqu(R6Elements, RPicPoint, ApproximateValue, FocalLength);

                tmp = !(~A * A) * ~A * Cte;
                ApproximateValue.x = ApproximateValue.x + tmp[1, 1];
                ApproximateValue.y = ApproximateValue.y + tmp[2, 1];
                ApproximateValue.z = ApproximateValue.z + tmp[3, 1];


            } while (tmp[3, 1] > 10E-12);
            return ApproximateValue;
        }
            private static Matrix JacIntersection2(Matrix Eleman6, double xa, double ya, double Focallength)
        {
            double sinw = Math.Sin(Eleman6[1, 1]); double sinw2 = sinw * sinw;
            double cosw = Math.Cos(Eleman6[1, 1]); double cosw2 = cosw * cosw;
            double sinfe = Math.Sin(Eleman6[2, 1]); double sinfe2 = sinfe * sinfe;
            double cosfe = Math.Cos(Eleman6[2, 1]); double cosfe2 = cosfe * cosfe;
            double sink = Math.Sin(Eleman6[3, 1]); double sink2 = sink * sink;
            double cosk = Math.Cos(Eleman6[3, 1]); double cosk2 = cosk * cosk;
            double XC = Eleman6[4, 1]; double YC = Eleman6[5, 1]; double ZC = Eleman6[6, 1];
            double f = Focallength;
            Matrix j = new Matrix(2, 3);
            /*j[1,1]=f*cosk*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)/(-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))+f*(cosk*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)*(XA-XC)-sink*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)*(YA-YC)+sinfe/(cosfe2+sinfe2)*(ZA-ZC))/Math.Pow ((-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC)),2)*(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2);
            j[1,2]=-f*sink*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)/(-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))-f*(cosk*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)*(XA-XC)-sink*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)*(YA-YC)+sinfe/(cosfe2+sinfe2)*(ZA-ZC))/Math.Pow ((-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC)),2)*(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2);
            j[1,3]=f*sinfe/(cosfe2+sinfe2)/(-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))-f*(cosk*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)*(XA-XC)-sink*cosfe/(cosk2*cosfe2+cosk2*sinfe2+sink2*sinfe2+sink2*cosfe2)*(YA-YC)+sinfe/(cosfe2+sinfe2)*(ZA-ZC))/Math.Pow ((-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC)),2)*cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2);
            j[2,1]=f*(sink*cosfe2*cosw+cosk*sinfe*sinw+sink*sinfe2*cosw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)/(-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))+f*((sink*cosfe2*cosw+cosk*sinfe*sinw+sink*sinfe2*cosw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*cosw-sink*sinfe*sinw+cosk*sinfe2*cosw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)-cosfe*sinw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))/Math.Pow ((-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC)),2)*(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2);
            j[2,2]=f*(cosk*cosfe2*cosw-sink*sinfe*sinw+cosk*sinfe2*cosw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)/(-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))-f*((sink*cosfe2*cosw+cosk*sinfe*sinw+sink*sinfe2*cosw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*cosw-sink*sinfe*sinw+cosk*sinfe2*cosw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)-cosfe*sinw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC))/Math.Pow ((-(-sink*cosfe2*sinw+cosk*sinfe*cosw-sink*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(XA-XC)+(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2)*(YA-YC)+cosfe*cosw/(cosfe2*cosw2+cosfe2*sinw2+sinfe2*sinw2+sinfe2*cosw2)*(ZA-ZC)),2)*(cosk*cosfe2*sinw+sink*sinfe*cosw+cosk*sinfe2*sinw)/(cosk2*cosfe2*cosw2+cosk2*cosfe2*sinw2+sink2*cosfe2*cosw2+sink2*cosfe2*sinw2+sink2*cosw2*sinfe2+cosk2*sinfe2*sinw2+sink2*sinw2*sinfe2+cosk2*sinfe2*cosw2);
            j[2, 3] = -f * cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow  ((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) , 2) * cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2);
 */

            j[1, 1] = 1;
            j[1, 2] = 0;
            j[1, 3] = -(cosk * cosfe * xa + (sink * cosw + cosk * sinfe * sinw) * ya - (sink * sinw - cosk * sinfe * cosw) * f) / (sinfe * xa - cosfe * sinw * ya - cosfe * cosw * f);
            j[2, 1] = 0;
            j[2, 2] = 1;
            j[2, 3] = -(-sink * cosfe * xa + (cosk * cosw - sink * sinfe * sinw) * ya - (cosk * sinw + sink * sinfe * cosw) * f) / (sinfe * xa - cosfe * sinw * ya - cosfe * cosw * f);
            return j;
        }
            private static Matrix JacIntersection(Matrix Eleman6, DPoint EarthPoint, double Focallength)
        {
            double sinw = Math.Sin(Eleman6[1, 1]); double sinw2 = sinw * sinw;
            double cosw = Math.Cos(Eleman6[1, 1]); double cosw2 = cosw * cosw;
            double sinfe = Math.Sin(Eleman6[2, 1]); double sinfe2 = sinfe * sinfe;
            double cosfe = Math.Cos(Eleman6[2, 1]); double cosfe2 = cosfe * cosfe;
            double sink = Math.Sin(Eleman6[3, 1]); double sink2 = sink * sink;
            double cosk = Math.Cos(Eleman6[3, 1]); double cosk2 = cosk * cosk;
            double XC = Eleman6[4, 1]; double YC = Eleman6[5, 1]; double ZC = Eleman6[6, 1];
            double XA = EarthPoint.x; double YA = EarthPoint.y; double ZA = EarthPoint.z;
            double f = Focallength;
            Matrix j = new Matrix(2, 3);
            j[1, 1] = f * cosk * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) + f * (cosk * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[1, 2] = -f * sink * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * (cosk * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[1, 3] = f * sinfe / (cosfe2 + sinfe2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * (cosk * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) * (XA - XC) - sink * cosfe / (cosk2 * cosfe2 + cosk2 * sinfe2 + sink2 * sinfe2 + sink2 * cosfe2) * (YA - YC) + sinfe / (cosfe2 + sinfe2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2);
            j[2, 1] = f * (sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) + f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[2, 2] = f * (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2);
            j[2, 3] = -f * cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) / (-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) - f * ((sink * cosfe2 * cosw + cosk * sinfe * sinw + sink * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * cosw - sink * sinfe * sinw + cosk * sinfe2 * cosw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) - cosfe * sinw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)) / Math.Pow((-(-sink * cosfe2 * sinw + cosk * sinfe * cosw - sink * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (XA - XC) + (cosk * cosfe2 * sinw + sink * sinfe * cosw + cosk * sinfe2 * sinw) / (cosk2 * cosfe2 * cosw2 + cosk2 * cosfe2 * sinw2 + sink2 * cosfe2 * cosw2 + sink2 * cosfe2 * sinw2 + sink2 * cosw2 * sinfe2 + cosk2 * sinfe2 * sinw2 + sink2 * sinw2 * sinfe2 + cosk2 * sinfe2 * cosw2) * (YA - YC) + cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2) * (ZA - ZC)), 2) * cosfe * cosw / (cosfe2 * cosw2 + cosfe2 * sinw2 + sinfe2 * sinw2 + sinfe2 * cosw2);

            return j;
        }
            public  static void ResecIntersection(double FocalLength, ref Ctrlpoint[] LCtrlPts, ref Ctrlpoint[] RCtrlPts, ref Matrix LCheckPts, ref Matrix RCheckPts, out Matrix L6Elements, out Matrix R6Elements, out Matrix EarthPts)
        {
            int ncl, ncr, nckl, nckr;
            ncl = LCtrlPts.GetUpperBound(0);//num of control points
            ncr = RCtrlPts.GetUpperBound(0);
            if (ncl != ncr || ncl < 3)
                throw (new ApplicationException("Not Enough Or not Equal Control Points"));
            nckl = LCheckPts.Rows;//numod check points
            nckr = RCheckPts.Rows;
            if (nckr != nckl)
                throw (new ApplicationException("The check points Are not equal"));
            int i,  k;


            //part Calculating Approximate Values
            Matrix ApproximateValues = new Matrix(12 + nckl * 3, 1);
            Matrix tmpLctrlPicPts = new Matrix (ncl,3);
            Matrix tmpRctrlPicPts = new Matrix (ncl ,3);
            Matrix tmpLctrlEarthPts = new Matrix(ncl, 3);
            Matrix tmpRctrlEarthPts = new Matrix (ncl ,3);
            double DHL = 0, DHR = 0;

            for (i = 1; i <= ncl; i++)
            {
                tmpLctrlPicPts[i] = LCtrlPts[i].Pic;
                tmpRctrlPicPts[i] = RCtrlPts[i].Pic;
                tmpLctrlEarthPts[i] = LCtrlPts[i].Earth;
                tmpRctrlEarthPts[i] = RCtrlPts[i].Earth;
                DHL = LCtrlPts[i].Earth.z + DHL;
                DHR = RCtrlPts[i].Earth.z + DHR;


            }

            Matrix tmpLElemets = CalcConformalFactors(ref tmpLctrlPicPts, ref tmpLctrlEarthPts);
            Matrix tmpRElemets = CalcConformalFactors(ref tmpRctrlPicPts, ref tmpRctrlEarthPts);
            ApproximateValues[1, 1] = 0;
            ApproximateValues[2, 1] = 0;
            ApproximateValues[3, 1] = Math.Atan(tmpLElemets[2, 1] / tmpLElemets[1, 1]);
            ApproximateValues[4, 1] = tmpLElemets[3, 1];
            ApproximateValues[5, 1] = tmpLElemets[4, 1];
            ApproximateValues[6, 1] = Math.Sqrt(tmpLElemets[1, 1] * tmpLElemets[1, 1] + tmpLElemets[2, 1] * tmpLElemets[2, 1]) * FocalLength + DHL / ncl;

            ApproximateValues[7, 1] = 0;
            ApproximateValues[8, 1] = 0;
            ApproximateValues[9, 1] = Math.Atan(tmpRElemets[2, 1] / tmpRElemets[1, 1]);
            ApproximateValues[10, 1] = tmpRElemets[3, 1];
            ApproximateValues[11, 1] = tmpRElemets[4, 1];
            ApproximateValues[12, 1] = Math.Sqrt(tmpRElemets[1, 1] * tmpRElemets[1, 1] + tmpRElemets[2, 1] * tmpRElemets[2, 1]) * FocalLength + DHR / ncl;
            double Px, Py, B;
            for (i = 1; i <= nckl; i++)
            {


                Px = LCheckPts[i].x - RCheckPts[i].x;
                Py = LCheckPts[i].y - RCheckPts[i].y;

                B = ApproximateValues[10, 1] - ApproximateValues[4, 1];

                ApproximateValues[12 + 3 * i - 2, 1] = ApproximateValues[4, 1] + B * LCheckPts[i].x / Px;
                ApproximateValues[12 + 3 * i - 1, 1] = ApproximateValues[5, 1] + B * LCheckPts[i].y / Px;
                ApproximateValues[12 + 3 * i, 1] = ApproximateValues[6, 1] - B * FocalLength / Px; ///ApproximateValues[12, 1]- B * FocalLength / Px)/2;

            }

            Matrix A = new Matrix(2 * (ncl + nckl), 12 + nckl * 3);
            Matrix Cte = new Matrix(12 + nckl * 3, 1);
            Matrix tmp = new Matrix(12 + nckl * 3, 1);
            DPoint earthpoint = new DPoint();


            do
            {//filling Matrix A for control points
                for (i = 1; i <= ncl; i++)
                {
                    k = 2 * i - 1;
                    //left pic
                    A[k, k + 1, 1, 6] = JacExterior(ApproximateValues[1, 6, 1, 1], LCtrlPts[i].Earth, FocalLength);
                    //Right Pic
                    A[k + 2 * (ncl + nckl), k + 2 * (ncl + nckl) + 1, 7, 12] = JacExterior(ApproximateValues[7, 12, 1, 1], RCtrlPts[i].Earth, FocalLength);


                    Cte[k, k + 1, 1, 1] = -1 * ColiniaryEqu(ApproximateValues[1, 6, 1, 1], LCtrlPts[i].Pic, LCtrlPts[i].Earth, FocalLength);
                    Cte[k + 2 * (ncl + nckl), k + 2 * (ncl + nckl) + 1, 1, 1] = -1 * ColiniaryEqu(ApproximateValues[7, 12, 1, 1], RCtrlPts[i].Pic, RCtrlPts[i].Earth, FocalLength);
                }
                //filling Matrix A for check points
                for (i = 1; i <= nckl; i++)
                {
                    k = 2 * i - 1;
                    //part exterior elements points
                    A[k + 2 * ncl, k + 2 * ncl + 1, 1, 6] = JacExterior(ApproximateValues[1, 6, 1, 1], new DPoint(ApproximateValues[12 + 3 * i - 2, 12 + 3 * i, 1, 1]), FocalLength);
                    A[k + 2 * (2 * ncl + nckl), k + 2 * (2 * ncl + nckl) + 1, 7, 12] = JacExterior(ApproximateValues[7, 12, 1, 1], new DPoint(ApproximateValues[12 + 3 * i - 2, 12 + 3 * i, 1, 1]), FocalLength);


                    earthpoint.x = ApproximateValues[12 + 3 * i - 2, 1]; earthpoint.y = ApproximateValues[12 + 3 * i - 1, 1]; earthpoint.z = ApproximateValues[12 + 3 * i, 1];

                    A[k + 2 * ncl, k + 2 * ncl + 1, 12 + 3 * i - 2, 12 + 3 * i] = JacIntersection(ApproximateValues[1, 6, 1, 1], earthpoint, FocalLength);
                    A[k + 2 * (2 * ncl + nckl), k + 2 * (2 * ncl + nckl) + 1, 12 + 3 * i - 2, 12 + 3 * i] = JacIntersection(ApproximateValues[7, 12, 1, 1], earthpoint, FocalLength);


                    Cte[k + 2 * ncl, k + 2 * ncl + 1, 1, 1] = -1 * ColiniaryEqu(ApproximateValues[1, 6, 1, 1], LCheckPts[i], earthpoint, FocalLength);
                    Cte[k + 2 * (2 * ncl + nckl), k + 2 * (2 * ncl + nckl) + 1, 1, 1] = -1 * ColiniaryEqu(ApproximateValues[7, 12, 1, 1], RCheckPts[i], earthpoint, FocalLength);
                }
                tmp = !(~A * A) * ~A * Cte;
                ApproximateValues = ApproximateValues + tmp;
            } while (Math.Abs(tmp[1, 1]) > 1E-7);
            L6Elements = ApproximateValues[1, 6, 1, 1];
            R6Elements = ApproximateValues[7, 12, 1, 1];
            EarthPts = new Matrix (nckl,3) ;
            for (i = 1; i <= nckl; i++)
              EarthPts[i] = new DPoint( ApproximateValues[12 + 3 * i - 2, 1],ApproximateValues[12 + 3 * i - 1, 1],ApproximateValues[12 + 3 * i, 1]);
                
            


        }
            public  static Matrix InvColiniaryEqu(Matrix Elements6, DPoint PicPoint, DPoint EarthPoint, double FocalLength)
        {
            double w = Elements6[1, 1];
            double fe = Elements6[2, 1];
            double k = Elements6[3, 1];
            double XC = Elements6[4, 1];
            double YC = Elements6[5, 1];
            double ZC = Elements6[6, 1];
            double f = FocalLength;
            double x = PicPoint.x;
            double y = PicPoint.y;
            double XA = EarthPoint.x;
            double YA = EarthPoint.y;
            double ZA = EarthPoint.z;
            double F, G;
            Matrix R = new Matrix(3, 3);
            R = RotZ(k) * (RotY(fe) * RotX(w));
            F = (XA - XC) - (ZA - ZC) * ((R[1, 1] * x + R[1, 2] * y + R[1, 3] * -f) / (R[3, 1] * x + R[3, 2] * y + R[3, 3] * -f));
            G = (YA - YC) - (ZA - ZC) * ((R[2, 1] * x + R[2, 2] * y + R[2, 3] * -f) / (R[3, 1] * x + R[3, 2] * y + R[3, 3] * -f));
            Matrix ans = new Matrix(2, 1);
            ans[1, 1] = F; ans[2, 1] = G;
            return ans;
        }
            public  static Matrix ColiniaryEqu(Matrix Elements6, DPoint PicPoint, DPoint EarthPoint, double FocalLength)
        {
            double w = Elements6[1, 1];
            double fe = Elements6[2, 1];
            double k = Elements6[3, 1];
            double XC = Elements6[4, 1];
            double YC = Elements6[5, 1];
            double ZC = Elements6[6, 1];
            double f = FocalLength;
            double x = PicPoint.x;
            double y = PicPoint.y;
            double XA = EarthPoint.x;
            double YA = EarthPoint.y;
            double ZA = EarthPoint.z;
            double F, G;

            Matrix R = RotZ(k) * (RotY(fe) * RotX(w));
            Matrix M = ~R;
            F = x + f * (M[1, 1] * (XA - XC) + M[1, 2] * (YA - YC) + M[1, 3] * (ZA - ZC)) / (M[3, 1] * (XA - XC) + M[3, 2] * (YA - YC) + M[3, 3] * (ZA - ZC));
            G = y + f * (M[2, 1] * (XA - XC) + M[2, 2] * (YA - YC) + M[2, 3] * (ZA - ZC)) / (M[3, 1] * (XA - XC) + M[3, 2] * (YA - YC) + M[3, 3] * (ZA - ZC));
            Matrix ans = new Matrix(2, 1);
            ans[1, 1] = F; ans[2, 1] = G;
            return ans;
        }
            public  static  Matrix  RelativeOrientatuinCL(Matrix LPicPts, Matrix RPicPts, double FocalLength, double BX, out Matrix ModelPts)
        {
            int NL, NR;
            NL = LPicPts.Rows;
            NR = RPicPts.Rows;
            if (NL != NR || NL < 5 || NR < 5)
                throw (new ApplicationException("Not Enough Vang Grouber Points"));
            Matrix ApproximateValues = new Matrix(5 + 3 * NL, 1);
            Matrix A = new Matrix(4 * NL, 5 + 3 * NL);
            Matrix Cte = new Matrix(4 * NL, 1); Matrix Cte2 = new Matrix(4 * NL, 1);
            Matrix LElement6 = new Matrix(6, 1);
            Matrix RElement6 = new Matrix(6, 1);
            Matrix ans = new Matrix(4 * NL, 1);
            Matrix TmpJacEx = new Matrix(2, 6); Matrix TmpJacEx2 = new Matrix(2, 5);
            DPoint[]  TmpMyPt = new DPoint[1];
            int i, j;
            double Px;
            LElement6[6, 1] = -FocalLength;
            for (i = 1; i <= NL; i++)
            {
                Px = LPicPts[i].x - RPicPts[i].x;
                ApproximateValues[5 + 3 * i - 2, 1] = LElement6[4, 1] + BX * LPicPts[i].x / Px;
                ApproximateValues[5 + 3 * i - 1, 1] = LElement6[5, 1] + BX * LPicPts[i].y / Px;
                ApproximateValues[5 + 3 * i, 1] = LElement6[6, 1] - BX * FocalLength / Px;

            }
            // ApproximateValues[1, 1] = 0.1; ApproximateValues[2, 1] = 0.1;
            //ApproximateValues[3, 1] = 0.1; ApproximateValues[4, 1] = 1; ApproximateValues[5, 1] = 1;

            do
            {
                RElement6[1, 3, 1, 1] = ApproximateValues[1, 3, 1, 1];
                RElement6[4, 1] = BX;
                RElement6[5, 1] = ApproximateValues[4, 1];
                RElement6[6, 1] = ApproximateValues[5, 1];

                for (i = 1; i <= NL; i++)
                {
                    j = 2 * i - 1;

                    A[j, j + 1, 5 + 3 * i - 2, 5 + 3 * i] = JacIntersection(LElement6, new DPoint(ApproximateValues[5 + 3 * i - 2, 5 + 3 * i, 1, 1]), FocalLength);
                    TmpJacEx = JacExterior(RElement6, new DPoint(ApproximateValues[5 + 3 * i - 2, 5 + 3 * i, 1, 1]), FocalLength);
                    TmpJacEx2[1, 2, 1, 3] = TmpJacEx[1, 2, 1, 3]; TmpJacEx2[1, 2, 4, 4] = TmpJacEx[1, 2, 5, 5]; TmpJacEx2[1, 2, 5, 5] = TmpJacEx[1, 2, 6, 6];
                    A[j + 2 * NL, j + 2 * NL + 1, 1, 5] = TmpJacEx2;
                    A[j + 2 * NL, j + 2 * NL + 1, 5 + 3 * i - 2, 5 + 3 * i] = JacIntersection(RElement6, new DPoint(ApproximateValues[5 + 3 * i - 2, 5 + 3 * i, 1, 1]), FocalLength);
                    TmpMyPt = XYZ2Dpoint(~ApproximateValues[5 + 3 * i - 2, 5 + 3 * i, 1, 1]);

                    Cte[j, j + 1, 1, 1] = -1 * ColiniaryEqu(LElement6, LPicPts[i], TmpMyPt[1], FocalLength);
                    Cte[j + 2 * NL, j + 2 * NL + 1, 1, 1] = -1 * ColiniaryEqu(RElement6, RPicPts[i], TmpMyPt[1], FocalLength);


                }
                ans = (!(~A * A)) * ~A * Cte;
                ApproximateValues = ApproximateValues + ans;

            } while (Math.Abs(ans[4, 1]) > 1E-5 || Math.Abs(ans[5, 1]) > 1E-5 || Math.Abs(ans[6, 1]) > 1E-5);

            ModelPts = new Matrix (NL ,3);
            for (i = 1; i <= NL; i++)
            {
                ModelPts[i] = new DPoint(ApproximateValues[5 + 3 * i - 2, 1], ApproximateValues[5 + 3 * i - 1, 1], ApproximateValues[5 + 3 * i, 1]);
                
            }
            RElement6[1, 1] = ApproximateValues[1, 1];
            RElement6[2, 1] = ApproximateValues[2, 1];
            RElement6[3, 1] = ApproximateValues[3, 1];

            RElement6[4, 1] = BX;
            RElement6[5, 1] = ApproximateValues[4, 1];
            RElement6[6, 1] = ApproximateValues[5, 1];
            return  RElement6;
        }
            private static double fixangle(double p)
        {
            while (p < 0) p += 2 * Math.PI;
            p = p % Math.PI;
            return p;
        }
            public static Matrix  RelativeOrientatuinCPOld(Matrix LPicPts, Matrix RPicPts, double FocalLength, double BX, out Matrix ModelPts)
        {
                //****  this relative orientation is by Dr azizi method
            int NL, NR;
            NL = LPicPts.Rows;
            NR = RPicPts.Rows;
            if (NL != NR || NL < 5 || NR < 5)
                throw (new ApplicationException("Not Enough Vang Grouber Points"));

            Matrix ApproximateValues = new Matrix(5, 1);
            Matrix A = new Matrix(NL, 5);
            Matrix Cte = new Matrix(NL, 1);
            Matrix DetMatrix = new Matrix(3, 3);
            Matrix R = new Matrix(3, 3), Rc = new Matrix(3, 3);
            Matrix ans = new Matrix(5, 1);
            Matrix Tmp = new Matrix(3, 1);
            Matrix Elements6 = new Matrix(6, 1);
            double dx, dy = 0, dz = 0, x1, y1, z1, x2, y2, z2;
            double Lastw;
            double[] C = new double[6];
            dx = BX;
            int i;

            double w = 0; double fe = 0; double k = 0;
            Matrix R2PicPts = new Matrix((double[][])RPicPts.Data.Clone());
            for (int q = 1; q <= RPicPts.Rows; q++)
                R2PicPts[q] = new DPoint(R2PicPts[q].x,R2PicPts[q].y, -FocalLength);



            do
            {
                dy = ApproximateValues[1, 1];
                dz = ApproximateValues[2, 1];

                for (i = 1; i <= NL; i++)
                {
                    x1 = LPicPts[i].x; y1 = LPicPts[i].y; ; z1 = -FocalLength;

                    R = new Matrix(1, -ApproximateValues[5, 1], ApproximateValues[4, 1],
                                   ApproximateValues[5, 1], 1, -ApproximateValues[3, 1],
                                  -ApproximateValues[4, 1], ApproximateValues[3, 1], 1);

                    Tmp = ((DPoint )R2PicPts[i]).MatrixForm ;;
                    Tmp = R * Tmp;
                    R2PicPts[i]= (~Tmp)[1];
                    x2 = Tmp[1, 1]; y2 = Tmp[2, 1]; z2 = Tmp[3, 1];

                    // x2 = R2PicPts[i].x; y2 = R2PicPts[i].y; z2 = R2PicPts[i].z;

                    C[1] = -(x1 * z2 - x2 * z1);
                    C[2] = x1 * y2 - x2 * y1;

                    DetMatrix= new Matrix(dx, dy, dz, x1, y1, z1, z2, 0, -x2);
                    C[4] = DetMatrix.Det;
                    DetMatrix[3, 1] = 0; DetMatrix[3, 2] = -z2; DetMatrix[3, 3] = y2;
                    C[3] = DetMatrix.Det;
                    DetMatrix[3, 1] = -y2; DetMatrix[3, 2] = x2; DetMatrix[3, 3] = 0;
                    C[5] = DetMatrix.Det;

                    for (int t = 1; t <= 5; t++)
                        A[i, t] = C[t];

                    Cte[i, 1] = -dx * (y1 * z2 - y2 * z1);

                }
                ans = !(~A * A) * ~A * Cte;
                ApproximateValues = ans;

                Lastw = w;
                w = w - ans[3, 1];
                fe = fe -ans[4, 1];
                k = k - ans[5, 1];



            } while (Math.Abs(w - Lastw) > 1E-5);
            Elements6 = new Matrix(6, 1);
            Elements6[1, 1] = w;
            Elements6[2, 1] = fe;
            Elements6[3, 1] = k;

            Elements6[4, 1] = dx;
            Elements6[5, 1] = dy;
            Elements6[6, 1] = dz;


            for (int u = 1; u <= R2PicPts.Rows; u++)
            {
                Rc = RotZ(k) * (RotY(fe) * RotX(w));


                //Rc = new Matrix(1, dz, -dy,
                //            -dz, 1, dx, 
                //             dy, -dx, 1);
                Tmp[1, 1] = RPicPts[u].x;
                Tmp[2, 1] = RPicPts[u].y;
                Tmp[3, 1] = -FocalLength;

                Tmp = Rc * Tmp;
                R2PicPts[u] = (~Tmp)[1];
                

            }
            double Lambda;
            ModelPts = new Matrix (NL ,3);
            for (i = 1; i <= NL; i++)
            {

                Lambda = (R2PicPts[i].z * dx + R2PicPts[i].x * -dz) / (LPicPts[i].x * R2PicPts[i].z - R2PicPts[i].x * -FocalLength);
               double x = LPicPts[i].x * Lambda;
               double z = -FocalLength - FocalLength * Lambda;
               double  y = (LPicPts[i].y * Lambda + R2PicPts[i].y * Lambda - dy) / 2;
               ModelPts[i] = new DPoint(x, y, z);
            }

            return Elements6;


        }
            public static Matrix AbsoluteOrientaton(Matrix ModelPts, Matrix EarthPts)
        {
            int m, n;
            m = ModelPts.Rows;
            n = EarthPts.Rows;
            if (m != n || m < 3)
                throw (new ApplicationException("not enough control points"));

            Matrix Diffs = new Matrix(7, 1);
            double Lambda, w = 0, fe = 0, k, Xt, Yt, Zt;
            Matrix Appvals = CalcConformalFactors(ref ModelPts, ref EarthPts);
            Xt = Appvals[3, 1];
            Yt = Appvals[4, 1];
            Lambda = Math.Sqrt(Appvals[1, 1] * Appvals[1, 1] + Appvals[2, 1] * Appvals[2, 1]);
            k = Math.Atan(Appvals[2, 1] / Appvals[1, 1]);
            Zt = EarthPts[1].z - Lambda * ModelPts[1].z;

            Matrix A = new Matrix(3 * m, 7);
            Matrix F1 = new Matrix(3 * m, 1);
            Matrix F2 = new Matrix(3 * m, 1);
            Matrix F = new Matrix(3 * m, 1);
            Matrix ans = new Matrix(7, 1);
            Matrix Shift = new Matrix(3, 1);
            int i, j;
            Matrix R = new Matrix(3, 3);
            for (i = 1; i <= m; i++)
            {
                F2[3 * i - 2, 1] = EarthPts[i].x;
                F2[3 * i - 1, 1] = EarthPts[i].y;
                F2[3 * i, 1] = EarthPts[i].z;

            }
            double x, y, z;
            do
            {
                R = RotZ(-k) * (RotY(-fe) * RotX(-w));

                for (i = 1; i <= m; i++)
                {
                    j = 3 * i - 2;

                    Shift[1, 1] = Xt; Shift[2, 1] = Yt; Shift[3, 1] = Zt;
                    F1[j, j + 2, ':'] = Lambda * (R * (Matrix)ModelPts[i]) + Shift;
                    x = F1[j, 1]; y = F1[j + 1, 1]; z = F1[j + 2, 1];

                    A[j, 1] = A[j + 1, 4] = x; A[j + 2, 3] = -x;
                    A[j, 4] = -y; A[j + 1, 1] = A[j + 2, 2] = y;
                    A[j, 3] = z; A[j + 1, 2] = -z; A[j + 2, 1] = z;
                    A[j, 5] = A[j + 1, 6] = A[j + 2, 7] = 1;

                    //  F1[j,j+2,':'] = (Matrix) ModelPts[i];


                }
                F = F2 - F1;
                ans = !(~A * A) * ~A * F;
                Lambda += ans[1, 1]; w += ans[2, 1]; fe += ans[3, 1];
                k += ans[4, 1]; Xt += ans[5, 1]; Yt += ans[6, 1]; Zt += ans[7, 1];

            } while (Math.Abs(ans[1, 1]) > 1E-10 || Math.Abs(ans[2, 1]) > 1E-10 || Math.Abs(ans[3, 1]) > 1E-10 || Math.Abs(ans[4, 1]) > 1E-10);
            ans[1, 1] = Lambda; ans[2, 1] = w; ans[3, 1] = fe; ans[4, 1] = k;
            ans[5, 1] = Xt; ans[6, 1] = Yt; ans[7, 1] = Zt;
            return ans ;
        }
            public static Matrix AbsoluteOrientaton2(Matrix ModelPts, Matrix EarthPts)
        {
            int m, n;
            m = ModelPts.Rows;
            n = EarthPts.Rows;
            if (m != n || m < 3)
                throw (new ApplicationException("not enough control points"));
            Matrix MdlPoints = new Matrix((double[][])ModelPts.Data.Clone());
            //Matrix Diffs = new Matrix(7, 1);
            double Lambda=0,Lambda2=0, w = 0, fe = 0, k=0, Xt=0, Yt=0, Zt=0;
            int i, j; 
            Matrix A = new Matrix(m, 3);
            Matrix F = new Matrix(m, 1);
            Matrix G = new Matrix(m, 1);
            Matrix tmp = new Matrix(3, 1);
            Matrix tmp2 = new Matrix(3, 1);
            Matrix tmp3 = new Matrix(3, 1);
            Matrix ans = new Matrix(7, 1); 
            Matrix R = new Matrix(3, 3);
            Matrix Appvals = new Matrix(4, 1);
            Lambda2 = 1;
            do
            {
                Appvals = CalcConformalFactors(ref MdlPoints, ref EarthPts);
                
                Xt += Appvals[3, 1];
                Yt += Appvals[4, 1];
                Lambda = Math.Sqrt(Appvals[1, 1] * Appvals[1, 1] + Appvals[2, 1] * Appvals[2, 1]);
                k += Math.Atan(Appvals[2, 1] / Appvals[1, 1]);
                
                tmp = new Matrix(Appvals[1, 1], -Appvals[2, 1], 0,
                                    Appvals[2, 1], Appvals[1, 1], 0,
                      0, 0, Lambda);

                for (i = 1; i <= m; i++)
                {
                    MdlPoints[i] = new DPoint(tmp * (Matrix)MdlPoints[i]);
                    
                }

                for (i = 1; i <= m; i++)
                {
                    A[i, 1] = MdlPoints[i].y;
                    A[i, 2] = -MdlPoints[i].x;
                    A[i, 3] = 1;
                    F[i, 1] = (EarthPts[i].z - MdlPoints[i].z);
                }



                tmp3 = !(~A * A) * ~A * F;
                w += tmp3[1, 1]; fe += tmp3[2, 1]; Zt += tmp3[3, 1];
                Lambda2 *= Lambda;
                for (i = 1; i <= m; i++)
                {
                    
                    R = RotZ(-k) * (RotY(-fe) * RotX(-w));
                    tmp2[1, 1] = Xt; tmp2[2, 1] = Yt; tmp2[3, 1] = Zt;
                   MdlPoints[i] = new DPoint(Lambda2* R * (Matrix)ModelPts[i] + tmp2);
                }


            } while (Math.Abs(F[1,1]) > 1E-10);
            ans[1, 1] = Lambda2; ans[2, 1] = w; ans[3, 1] = fe; ans[4, 1] = k;
            ans[5, 1] = Xt; ans[6, 1] = Yt; ans[7, 1] = Zt;
            return ans;
        }
            public static DPoint[] HelmertTransform(Matrix Space, Matrix Parameters7) 
        {
            DPoint [] ans =new DPoint [Space.Rows+1];
            Matrix tmp=new Matrix (3,1);
            Matrix R = RotZ(Parameters7[4, 1]) * RotY(Parameters7[3, 1]) * RotX(Parameters7[2, 1]);
            for (int i = 1; i <= Space.Rows; i++) 
            {
                tmp = Parameters7[1, 1] * R * (Matrix)Space[i] + Parameters7[5, 7,':'];
                ans[i]=new DPoint(tmp);
                

            }
            return ans;
        }
            public static double  Deg2Rad(double DecimalDeg)
            {
                return DecimalDeg * Math.PI / 180;
            }
            public static double Rad2Deg(double Rad)
            {
                return Rad  *  180/Math.PI ;
            }
            public static Matrix Point2DtoMatri(Matrix  Pts)
            {
                int n = Pts.Rows;
                Matrix ans = new Matrix(n, 2);
                for (int i = 1; i <= n; i++) 
                {
                    ans[i, 1] = Pts[i].x;
                    ans[i, 2] = Pts[i].y;
                }
                return ans;
            }
            public static Matrix  JacRelativeCP(Camera RCam,DPoint LPoint,DPoint RPoint,double LCamFocalLength)
            {// this is jacobian of relative orientation 
                double FL = LCamFocalLength;
                double FR = RCam.FocalLength;
                double XL = LPoint.x; double YL = LPoint.y;
                double xr = RPoint.x; double yr = RPoint.y;
                double sinw = Math.Sin(RCam.Omega); double sinfe = Math.Sin(RCam.Fi); double sink = Math.Sin(RCam.Kapa );
                double cosw = Math.Cos(RCam.Omega); double cosfe = Math.Cos(RCam.Fi ); double cosk = Math.Cos(RCam.Kapa);
                double BX = RCam.Position.x;
                double BY = RCam.Position.y;
                double BZ = RCam.Position.z;
                Matrix Ans = new Matrix(1, 5);
                Ans[1,1] = FR * cosk * sinfe * sinw * BZ * YL + BX * FL * FR * sink * sinfe * sinw - XL * BZ * yr * cosk * sinw - BX * YL * cosfe * cosw * yr + BX * YL * cosfe * sinw * FR - BX * FL * yr * cosk * sinw - BX * FL * yr * sink * sinfe * cosw - BX * FL * FR * cosk * cosw + XL * BY * cosfe * cosw * yr - XL * BY * cosfe * sinw * FR - XL * BZ * yr * sink * sinfe * cosw - XL * BZ * FR * cosk * cosw + XL * BZ * FR * sink * sinfe * sinw + yr * sink * sinw * BY * FL + yr * sink * sinw * BZ * YL - yr * cosk * sinfe * cosw * BY * FL - yr * cosk * sinfe * cosw * BZ * YL + FR * sink * cosw * BY * FL + FR * sink * cosw * BZ * YL + FR * cosk * sinfe * sinw * BY * FL;
                Ans[1,2] =-FR*cosk*cosfe*cosw*BZ*YL-BX*FL*FR*sink*cosfe*cosw+BX*YL*cosfe*xr-XL*BY*cosfe*xr+BX*YL*sinfe*sinw*yr+BX*YL*sinfe*cosw*FR+BX*FL*sink*sinfe*xr-BX*FL*yr*sink*cosfe*sinw-XL*BY*sinfe*sinw*yr-XL*BY*sinfe*cosw*FR+XL*BZ*sink*sinfe*xr-XL*BZ*yr*sink*cosfe*sinw-XL*BZ*FR*sink*cosfe*cosw+cosk*sinfe*xr*BY*FL+cosk*sinfe*xr*BZ*YL-yr*cosk*cosfe*sinw*BY*FL-yr*cosk*cosfe*sinw*BZ*YL-FR*cosk*cosfe*cosw*BY*FL;
                Ans[1,3] =FR*sink*sinfe*cosw*BZ*YL-BX*FL*FR*cosk*sinfe*cosw-XL*BZ*yr*sink*cosw-BX*FL*cosk*cosfe*xr-BX*FL*yr*sink*cosw-BX*FL*yr*cosk*sinfe*sinw+BX*FL*FR*sink*sinw-XL*BZ*cosk*cosfe*xr-XL*BZ*yr*cosk*sinfe*sinw+XL*BZ*FR*sink*sinw-XL*BZ*FR*cosk*sinfe*cosw+sink*cosfe*xr*BY*FL+sink*cosfe*xr*BZ*YL-yr*cosk*cosw*BY*FL-yr*cosk*cosw*BZ*YL+yr*sink*sinfe*sinw*BY*FL+yr*sink*sinfe*sinw*BZ*YL+FR*cosk*sinw*BY*FL+FR*cosk*sinw*BZ*YL+FR*sink*sinfe*cosw*BY*FL;
                Ans[1,4] =-XL*sinfe*xr+XL*cosfe*sinw*yr+XL*cosfe*cosw*FR-cosk*cosfe*xr*FL-yr*sink*cosw*FL-yr*cosk*sinfe*sinw*FL+FR*sink*sinw*FL-FR*cosk*sinfe*cosw*FL;
                Ans[1,5] = -FR * cosk * sinfe * cosw * YL + XL * yr * cosk * cosw - XL * sink * cosfe * xr - XL * yr * sink * sinfe * sinw - XL * FR * cosk * sinw - XL * FR * sink * sinfe * cosw - cosk * cosfe * xr * YL - yr * sink * cosw * YL - yr * cosk * sinfe * sinw * YL + FR * sink * sinw * YL;
                return Ans;
 

            }
            public static double CoPlanearityEq(Camera RCam, DPoint Lpoint, DPoint Rpoint, double LCamFocalLength) 
            {
                Rpoint.z=-RCam.FocalLength;
                Matrix R = Transforms.R(RCam.Omega, RCam.Fi, RCam.Kapa); 
                double  XR=R[1,':']*Rpoint.MatrixForm;
                double  YR=R[2,':']*Rpoint.MatrixForm;
                double  ZR=R[3,':']*Rpoint.MatrixForm;
                Matrix CPEQ=new Matrix(RCam.Position.x,RCam.Position.y,RCam.Position.z,Lpoint.x,Lpoint.y,-LCamFocalLength,XR,YR,ZR );
                return  CPEQ.Det;
            }
            public static Camera  RelativeOrientationCP(Matrix LPicPts, Matrix RPicPts,double BX,  double LCamFocalLength,double RCamFocalLength,out Matrix Residuals)
            {

                if (LPicPts.Rows != RPicPts.Rows)
                    throw (new ApplicationException("Not Equal Points"));
                if (LPicPts.Rows < 5)
                    throw (new ApplicationException("Points must be More than 5"));
                int n = LPicPts.Rows;
                Matrix A = new Matrix(n, 5);
                Matrix W = new Matrix(n, 1);
                Camera RCam = new Camera(); RCam.FocalLength = RCamFocalLength; RCam.Position.x = BX;
                Matrix  dx=new Matrix(5,1);
                int count = 0;
                do
                {
                    count++;  
                    for (int i = 1; i <= n; i++)
                    {
                        Matrix jcp = JacRelativeCP(RCam, LPicPts[i], RPicPts[i], LCamFocalLength);
                        A[i, ':'] = jcp;
                        W[i, 1] = CoPlanearityEq(RCam, LPicPts[i], RPicPts[i], LCamFocalLength);

                    }
                   dx=!(~A*A)*(~A*(-W));
                   RCam.Omega += dx[1, 1];
                   RCam.Fi += dx[2, 1];
                   RCam.Kapa += dx[3, 1];
                   RCam.Position.y += dx[4, 1];
                   RCam.Position.z += dx[5, 1]; 

                }while(dx[1,1]*dx[1,1]+dx[2,1]*dx[2,1]+dx[3,1]*dx[3,1]>1e-14);
                Matrix r = new Matrix(5, 1);
                r[1,3,1,1]=RCam.Orientation6Elements[1,3,1,1];
                r[4,1]=RCam.Orientation6Elements[5,1];
                r[5, 1] = RCam.Orientation6Elements[6, 1];
                Residuals = A * r - W;
                return RCam;

            }

        }
   
}