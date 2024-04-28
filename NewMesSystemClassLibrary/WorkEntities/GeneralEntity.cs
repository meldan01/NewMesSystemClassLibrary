using NewMasApp.ExternalComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMASMAnagementApplication.WorkEntities
{
    public class GeneralEntity
    {
        public DateTime creationDate { get; set; }
        public string createdBy { get; set; }
        public string languageCode { get; set; }

        public GeneralEntity(DateTime creationDate, string createdBy, string languageCode)
        {
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.languageCode = languageCode;
        }
    }
}
