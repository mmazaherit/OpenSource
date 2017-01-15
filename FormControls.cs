using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Reflection;
//mehdi
    class FormControls
    {
        static string ProgramName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static void Savesettings(params object[] cts)
        {
            

            foreach (object ct in cts)
            {
                switch (ct.GetType().ToString())
                {
                    
                    case "System.Windows.Forms.TextBox":
                        TextBox t = (TextBox)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, t.TopLevelControl.Name, t.Name, t.Text);
                        break;
                    case "System.Windows.Forms.CheckBox":
                        CheckBox c = (CheckBox)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, c.TopLevelControl.Name, c.Name, c.Checked.ToString());
                        break;
                    case "System.Windows.Forms.ToolStripComboBox":
                    case "System.Windows.Forms.ComboBox":
                        ComboBox cmb = (ComboBox)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, cmb.TopLevelControl.Name, cmb.Name, cmb.Text);
                        break;
                    case "System.Windows.Forms.NumericUpDown":
                        NumericUpDown nud = (NumericUpDown)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, nud.TopLevelControl.Name, nud.Name, nud.Value.ToString());
                        break;
                    case "System.Windows.Forms.RadioButton":
                        RadioButton rb = (RadioButton)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, rb.TopLevelControl.Name, rb.Name, rb.Checked.ToString());
                        break;

                    case "System.Windows.Forms.TrackBar":
                        TrackBar trb = (TrackBar)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, trb.TopLevelControl.Name, trb.Name, trb.Value.ToString());
                        break;                        
                    case "System.Windows.Forms.ToolStripMenuItem":
                        ToolStripMenuItem tsm = (ToolStripMenuItem)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, "ToolStrip"  , tsm.Name, tsm.Checked.ToString());
                        break;
                    case "System.Windows.Forms.ToolStripTextBox": 
                         TextBox tst = (TextBox)ct;
                        Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, "ToolStrip", tst.Name, tst.Text);
                        break;


                }

            }
        }
        public static void Getsettings(params object[] cts)
        {
            foreach (object ct in cts)
                Getsetting(ct);
        }
        public static void Getsetting( object ctrl, object DefVal = null)
        {
            string  RegVal="";

            if (ctrl.GetType().ToString().Contains("ToolStrip"))
            {
                ToolStripItem  control = (ToolStripItem)ctrl;
                RegVal = Microsoft.VisualBasic.Interaction.GetSetting(ProgramName, "ToolStrip", control.Name, DefVal == null ? "" : DefVal.ToString());
            }
            else
            {
                Control control = (Control)ctrl;
                RegVal = Microsoft.VisualBasic.Interaction.GetSetting(ProgramName, control.TopLevelControl.Name, control.Name, DefVal == null ? "" : DefVal.ToString());

            }
            if (RegVal == "")
                return;
            switch (ctrl.GetType().ToString())
            {
                
                case "System.Windows.Forms.TextBox":
                    TextBox t = (TextBox)ctrl;
                    t.Text = RegVal;
                    break;             
                case "System.Windows.Forms.CheckBox":
                    CheckBox c = (CheckBox)ctrl;                    
                    c.Checked = Convert.ToBoolean(RegVal);
                    break;
                
                case "System.Windows.Forms.ComboBox" :
                    ComboBox cmb = (ComboBox)ctrl;                    
                        cmb.Text= RegVal.ToString();
                    break;                    
                case "System.Windows.Forms.NumericUpDown":
                    NumericUpDown nud = (NumericUpDown)ctrl; 
                    decimal ndval=Convert.ToDecimal(Convert.ToDouble( RegVal));
                    if(ndval>=nud.Minimum & ndval <=nud.Maximum)   nud.Value=ndval;
                    break;
                case "System.Windows.Forms.RadioButton":
                    RadioButton rb = (RadioButton)ctrl;
                    rb.Checked = Convert.ToBoolean(RegVal);
                    break;
                case "System.Windows.Forms.TrackBar":
                    TrackBar trb = (TrackBar)ctrl;                    
                        trb.Value = Convert.ToInt32(RegVal);
                    break;
                case "System.Windows.Forms.ToolStripMenuItem":
                    ToolStripMenuItem tsm = (ToolStripMenuItem)ctrl;
                    tsm.Checked = Convert.ToBoolean(RegVal);
                    break;
                case "System.Windows.Forms.ToolStripTextBox":
                    ToolStripTextBox tstb = (ToolStripTextBox)ctrl;
                    tstb.Text = RegVal.ToString();
                    break;
                case "System.Windows.Forms.ToolStripComboBox":
                    ToolStripComboBox tscb = (ToolStripComboBox)ctrl;
                    tscb.Text = RegVal.ToString();
                    break;

            }
        }
        public static void SaveString(string Key, string Value) 
        {
            Microsoft.VisualBasic.Interaction.SaveSetting(ProgramName, "Settings", Key, Value);
        }
        public static string  GetString(string Key, string Default="") 
        {
            return Microsoft.VisualBasic.Interaction.GetSetting(ProgramName, "Settings", Key,Default);
        } 
        
        public static void HandleListBoxesKeyDownEvent(params ListBox[] LBS) 
        {
            foreach (ListBox lb in LBS)
                lb.KeyDown += new KeyEventHandler(lb_KeyDown);

        }
        static void lb_KeyDown(object sender, KeyEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            if (e.KeyCode == Keys.Delete) 
            {                
                while (lb.SelectedIndices.Count>0) 
                {
                    lb.Items.RemoveAt(lb.SelectedIndices[0]);
                }
            }
            
            if(e.KeyCode==Keys.A & e.Control==true)
            {
                for (int i = 0; i < lb.Items.Count; i++) 
                {
                    lb.SetSelected(i, true);
                }
            }
        }
        
        //static string[] Check<T>(Expression<Func<T>> expr)
        //{
        //    /* using
        //    string varname = "varval";
        //    string[] ans = Check(() => varname);
             
        //    */

        //    string[] ans = new string[2];
        //    var body = ((MemberExpression)expr.Body);
        //    ans[0] = body.Member.Name; //varname
        //    ans[1] = ((FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value).ToString();
        //    return ans;


        //}
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }

        



    }

