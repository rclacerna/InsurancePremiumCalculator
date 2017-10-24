using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppliedSystemsTest
{
    public class DriverDetails
    {
        public DateTime PolicyDate { get; set; }
        public string Name { get; set; }
        public string Occupation { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Claim> Claims { get; set; }
    }

    public class Claim
    {
        public int ClaimId { get; set; }
        public DateTime ClaimDate { get; set; }
    }
}
