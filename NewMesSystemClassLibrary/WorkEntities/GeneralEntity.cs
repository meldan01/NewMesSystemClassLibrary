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
        private DateTime m_creationDate;
        private string m_createdBy;
        private string m_languageCode;

        public DateTime CreationDate
        {
            get { return m_creationDate; }
            set { m_creationDate = value; }
        }

        public string CreatedBy
        {
            get { return m_createdBy; }
            set { m_createdBy = value; }
        }

        public string LanguageCode
        {
            get { return m_languageCode; }
            set { m_languageCode = value; }
        }

        public GeneralEntity(DateTime creationDate, string createdBy, string languageCode)
        {
            CreationDate = creationDate;
            CreatedBy = createdBy;
            LanguageCode = languageCode;
        }
    }
}
