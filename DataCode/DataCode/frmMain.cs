using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataCode
{
    // now only support calculate integer 
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            UpdateFirstValueCode();
            UpdateSecondValueCode();
        }

        private void UpdateFirstValueCode()
        {
            txt1stY.Text = CalculateTrueForm(Convert.ToInt32(nud1st.Value));
            txt1stF.Text = CalculateRadixMinusOneComplement(txt1stY.Text);
            txt1stB.Text = CalculateComplement(txt1stF.Text);
        }

        private void UpdateSecondValueCode()
        {
            txt2stY.Text = CalculateTrueForm(Convert.ToInt32(nud2st.Value));
            txt2stF.Text = CalculateRadixMinusOneComplement(txt2stY.Text);
            txt2stB.Text = CalculateComplement(txt2stF.Text);
        }

        private void nud1st_ValueChanged(object sender, EventArgs e)
        {
            UpdateFirstValueCode();
            UpdateResult();
        }

        private void nud2st_ValueChanged(object sender, EventArgs e)
        {
            UpdateSecondValueCode();
            UpdateResult();
        }

        private void UpdateResult()
        {
            txtResult.Text = Convert.ToString(Convert.ToInt32(nud1st.Value) + Convert.ToInt32(nud2st.Value));
            txtResultY.Text = CalculateCodeResult(txt1stY.Text, txt2stY.Text);
            txtResultF.Text = CalculateCodeResult(txt1stF.Text, txt2stF.Text);
            txtResultB.Text = CalculateCodeResult(txt1stB.Text, txt2stB.Text);
        }

        private class CalculateCathe
        {
            public bool IsCarry = false;
            public char Result;
        }

        private CalculateCathe CalculateBitResult(char c1, char c2, bool carry)
        {
            CalculateCathe cathe = new CalculateCathe();

            int intResult=c1-'0'+c2-'0'+(carry?1:0);
            cathe.IsCarry=intResult>1;
            cathe.Result=Convert.ToChar((intResult%2)+'0');
            
            return cathe;
        }

        private bool IsCarry(char c1, char c2)
        {
            return (c1 == SECONT_CHAR_B) &&
                (c2 == SECONT_CHAR_B);
        }

        public string CalculateCodeResult(string data1st,string data2st)
        {
            StringBuilder result = new StringBuilder();
            CalculateCathe buffer = new CalculateCathe();

            for (int i = data1st.Length - 1; i >= 0; i--)
            {
                buffer = CalculateBitResult(data1st[i], data2st[i], buffer.IsCarry);                
                result.Insert(0, buffer.Result);                
            }

            return result.ToString();
        }

        private string CalculateTrueForm(int originalValue)
        {
            StringBuilder buffer = new StringBuilder();

            int quotient = 0;
            int remainder = 0;

            int tmp=Math.Abs(originalValue);

            do
            {
                quotient = tmp / 2;
                remainder = tmp % 2;

                buffer.Insert(0,Convert.ToString(remainder));

                tmp = quotient;
            } while (tmp != 0);

            string result = buffer.ToString().TrimStart(FIRST_CHAR_B).PadLeft(7, FIRST_CHAR_B);

            return (Convert.ToString(originalValue < 0 ? SECONT_CHAR_B : FIRST_CHAR_B)) + result;
        }

        private const char POSITIVE_SIGN = '0';
        private const char NEGATIVE_SIGN = '1';
        private const char FIRST_CHAR_B = '0';
        private const char SECONT_CHAR_B = '1';

        public string CalculateComplement(string dataF)
        {
            if (dataF[0] == POSITIVE_SIGN)
            {
                return dataF;
            }

            StringBuilder result = new StringBuilder();
            
            bool carry = dataF.Last() == SECONT_CHAR_B;
            result.Append(carry ? FIRST_CHAR_B : SECONT_CHAR_B);

            for (int i = dataF.Length - 2; i >= 0; i--)
            {
                if (carry)
                {
                    carry = dataF[i] == SECONT_CHAR_B;
                    result.Insert(0,carry ? FIRST_CHAR_B : SECONT_CHAR_B);

                    continue;
                }

                result.Insert(0, dataF[i]);
            }

            return result.ToString();
        }

        public string CalculateRadixMinusOneComplement(string dataY)
        {
            if (dataY[0] == POSITIVE_SIGN)
            {
                return dataY;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(dataY[0]);

            for (int i = 1; i < dataY.Length; i++)
            {
                sb.Append(dataY[i] == FIRST_CHAR_B ? SECONT_CHAR_B : FIRST_CHAR_B);
            }

            return sb.ToString();
        }
    }
}
