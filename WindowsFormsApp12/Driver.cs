using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public class Driver
    {
        public Guid? Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patromic { get; set; }
        public string Passport { get; set; }

        public string RegistrationCity { get; set; }
        public string RegistrationStreetAddress { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceStreetAddress { get; set; }

        public string PlaceWork { get; set; }
        public string Post { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string Remarks { get; set; }
    }
}
