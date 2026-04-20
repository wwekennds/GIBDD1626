using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public class ViolationDto
    {
        public string ExternalId { get; set; }    
        public string VehicleNumber { get; set; }   
        public DateTime ViolationDate { get; set; }
        public string Article { get; set; }         
        public decimal Amount { get; set; }
        public string AccidentTypeRaw { get; set; } 

        // Для UI и БД
        public string SelectedAccidentType { get; set; } 
        public bool IsProcessed { get; set; } = false;
        public string Status { get; set; } = "Новый";
    }
}
