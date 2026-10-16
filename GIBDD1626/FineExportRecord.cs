using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIBDD1626
{
    public sealed class FineExportRecord
    {
        public Guid FineId { get; set; }
        public Guid DriverId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Passport { get; set; }
        public string RegistrationCity { get; set; }
        public string RegistrationAddress { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceAddress { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime ViolationDate { get; set; }
        public string Article { get; set; }
        public decimal Amount { get; set; }
        public string ExternalId { get; set; }
        public string AccidentType { get; set; }
    }
}
