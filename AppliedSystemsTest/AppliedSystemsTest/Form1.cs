using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AppliedSystemsTest
{
    public partial class PleaseEnterYourDetails : Form
    {
        private const double StartingPremium = 500;

        public PleaseEnterYourDetails()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label8_Click(object sender, EventArgs e)
        {
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
        }

        private void numOfClaims_ValueChanged(object sender, EventArgs e)
        {
            CreateNewClaim();
        }

        private void Claim1Label_Click(object sender, EventArgs e)
        {
        }


        private void PolicyStartDate_ValueChanged(object sender, EventArgs e)
        {
            var currentdate = DateTime.Now.Date;
            var policyDate = PolicyStartDate.Value.Date;
            var date = DateTime.Compare(policyDate, currentdate);

            switch (date)
            {
                case 1:
                    PolicyDateWarningLabel.Visible = false;
                    break;
                case -1:
                    PolicyDateWarningLabel.Visible = true;
                    CalcPremiumBtn.Enabled = false;
                    break;
            }
        }


        public void CreateNewClaim()
        {
            var pointX = 90;
            var pointY = 50;

            for (var i = 0; i < numOfClaims.Value; i++)
            {
                var date = new DateTimePicker
                {
                    Location = new Point(pointX, pointY),
                    Name = $@"ClaimDate{i + 1}",
                };

                var claimLabel = new Label
                {
                    Location = new Point(pointX - 60, pointY + 2),
                    Text = $@"Claim: {i + 1}",
                    Size = new Size(60, 20)
                };

                Driver1Panel.Panel2.Controls.Add(claimLabel);
                Driver1Panel.Panel2.Controls.Add(date);
                Driver1Panel.Panel2.Show();
                pointY += 20;
            }
        }

        public double CalculatePremium()
        {
            // - Age calculator: 21-25yr/old(+20%), 26-75yrs/old(- 10%)
            // - Claim calculator: ~ 1 yr of policy date(+20%), 2-5 yrs of policy date (+10%)
            // - Occupation calc: accountant (-10%), Chauffeur (+10%)

            double premium = 0.0;
            var driver = GetDriverDetails();
            var age = GetYears(driver.DateOfBirth.Date);

            // AGE
            if (age >= 21 && age <= 25)
            {
                premium = StartingPremium + (StartingPremium * 20 / 100);
            }
            else if (age > 25 && age <= 75)
            {
                premium = StartingPremium - (StartingPremium * 10 / 100);
            }
            else if (age < 21)
            {
                DeclineMessage.Text = $@"Age of Youngest driver";
                DeclineMessage.Visible = true;
                premium = 0;
            }
            else if (age > 75)
            {
                DeclineMessage.Text = $@"Age of Oldest driver";
                DeclineMessage.Visible = true;
                premium = 0;
            }

            // OCCUPATION
            if (OccupationSelection.Text == "Accountant")
            {
                premium = premium - (StartingPremium * 10 / 100);
            }
            else if (OccupationSelection.Text == "Chauffeur")
            {
                premium = premium + (StartingPremium * 10 / 100);
            }

            // CLAIMS
            foreach (var claim in driver.Claims)
            {
                if (claim.ClaimDate.Date < DateTime.Now)
                {
                    var years = GetClaimYears(driver.PolicyDate.Date, claim.ClaimDate.Date);
                    if (years <= 1)
                    {
                        premium = premium + (StartingPremium * 20 / 100);
                    }
                    else if (years >= 2 && years <= 5)
                    {
                        premium = premium + (StartingPremium * 10 / 100);
                    }

                    if (driver.Claims.Count > 2)
                    {
                        DeclineMessage.Text = $@"Driver has more than 2 claims - {driver.Name}";
                        DeclineMessage.Visible = true;
                        premium = 0;
                    }
                }
            }

            return premium;
        }

        public DriverDetails GetDriverDetails()
        {
            var driver = new DriverDetails
            {
                Name = DriverName.Text,
                Occupation = OccupationSelection?.Text,
                DateOfBirth = DateOfBirth.Value.Date,
                PolicyDate = PolicyStartDate.Value.Date,
                Claims = new List<Claim>()
            };

            foreach (Control control in Driver1Panel.Panel2.Controls)
            {
                if (control is DateTimePicker picker && control.Name != " ")
                {
                    var claim = new Claim {ClaimDate = picker.Value.Date};
                    if (claim != null && claim.ClaimDate.Date >= DateTime.Now.Date)
                        driver.Claims.Add(claim);
                }
            }

            return driver;
        }

        public int GetYears(DateTime date)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - date.Year;

            return years;
        }

        public int GetClaimYears(DateTime policyDate, DateTime claimDate)
        {
            int years = policyDate.Year - claimDate.Year;

            return years;
        }

        private void DriverName_TextChanged(object sender, EventArgs e)
        {

        }

        private void CalcPremiumBtn_Click(object sender, EventArgs e)
        {
            CheckRequiredFields();
            GetDriverDetails();
            CalculatePremium();
            CalculatedPremiumLabel.Text = $@"{CalculatePremium()}";
        }

        private void CheckRequiredFields()
        {
            if (OccupationSelection.Text == "Accountant" || OccupationSelection.Text == "Chauffeur")
            {
                requiredoccupation.Visible = false;
            }
            else
            {
                requiredoccupation.Visible = true;
            }

            if (string.IsNullOrWhiteSpace(DriverName.Text) || DriverName.Text == "")
            {
                namerequired.Visible = true;
            }
            else
            {
                namerequired.Visible = false;
            }


            if (DateOfBirth.Value.Date == DateTime.Now.Date)
            {
                dobrequired.Visible = true;
            }
            else
            {
                dobrequired.Visible = false;
            }



        }

        private void label49_Click(object sender, EventArgs e)
        {
        }

        private void CalculatedPremiumLabel_Click(object sender, EventArgs e)
        {
        }

        private void OccupationSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void namerequired_Click(object sender, EventArgs e)
        {

        }

        private void DateOfBirth_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dobrequired_Click(object sender, EventArgs e)
        {

        }

        private void PolicyDateWarningLabel_Click(object sender, EventArgs e)
        {

        }

        private void requiredoccupation_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            switch (numericUpDown1.Value)
            {
                case 1:
                    Driver2.Visible = true;
                    break;
                case 2:
                    Driver2.Visible = true;
                    Driver3.Visible = true;
                    break;
                case 3:
                    Driver2.Visible = true;
                    Driver3.Visible = true;
                    Driver4.Visible = true;
                    break;
                case 4:
                    Driver2.Visible = true;
                    Driver3.Visible = true;
                    Driver4.Visible = true;
                    Driver5.Visible = true;
                    break;
            }
        }

        //TODO:
        // - Age calculator: 21-25yr/old(+20%), 26-75yrs/old(- 10%)
        // - Claim calculator: ~ 1 yr of policy date(+20%), 2-5 yrs of policy date (+10%)
        // - Occupation calc: accountant (-10%), Chauffeur (+10%)

        // DECLINE policy
        // - if policy date is BEFORE today's date
        // - if age is less that 21
        // - IF age is over 75
        // - if number of claim is over 3
    }
}
