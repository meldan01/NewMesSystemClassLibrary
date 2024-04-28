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
            this.creationDate = returnValidatedDate(creationDate);
            this.createdBy = createdBy;
            this.languageCode = languageCode;
        }

        private DateTime returnValidatedDate(DateTime? creationDate)
        {
            if (creationDate.HasValue)
                return creationDate.Value;
            return DateTime.Now;
        }

        private string returnValidatedCreator(string creatorID)
        {
            if (string.IsNullOrEmpty(creatorID) )
                return "000000000";
            return creatorID;
        }




    }
}
