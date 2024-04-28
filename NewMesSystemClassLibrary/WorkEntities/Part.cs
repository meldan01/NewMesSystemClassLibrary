using NewMasApp.ExternalComponents;
using NewMASMAnagementApplication.WorkEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMasApp.WorkEntities
{
    public class Part : GeneralEntity
    {
        public string m_catalogNumber { get; set; }
        public string m_description { get; set; }
        private Logger logInstance = Logger.getInstance();

        public Part(DateTime creationDate, string createdBy, string languageCode, string catalogNumber, string description)
            : base(creationDate, createdBy, languageCode)
        {
            m_catalogNumber = catalogNumber;
            m_description = description;
        }


        /// <summary>
        /// partExists returns true if part catalogName already exists in the list
        /// </summary>
        /// <param name="catalogName"></param>
        /// <returns></returns>
        public static bool partExists(string catalogName)
        {
            return DBConnectionManager.isCatalogIDExists(catalogName);
        }

        /// <summary>
        /// Adds an object to the DB
        /// </summary>
        /// <returns></returns>
        public bool insertPartIntoDB()
        {
            return DBConnectionManager.insertPartIntoDB(m_catalogNumber, m_description, creationDate, createdBy, languageCode);
        }


        public static bool updatePart(string catalogID, string itemDescription, DateTime? selectedDate, string creatorID, string languageCode)
        {
            return DBConnectionManager.updatePart(catalogID, itemDescription, selectedDate, creatorID, languageCode);
        }


        /// <summary>
        /// getPartsInfo - build a string of info of all the Part DB table
        /// </summary>
        /// <returns></returns>
        public static string fetchPartsInfo()
        {
            string totalParts = string.Empty;
            totalParts = DBConnectionManager.buildPartsString();
            if (totalParts == string.Empty)
                totalParts = "No data in the DataBase.";
            return totalParts;
        }

        /// <summary>
        /// deletePart - deletes part by primary key catalog ID
        /// </summary>
        /// <param name="catalogID"></param>
        /// <returns></returns>
        public static bool deletePart(string catalogID)
        {
            return DBConnectionManager.deletePart(catalogID);
        }

        /// <summary>
        /// deleteWorkOrdersByCatalogID - gets a part catalog ID and deletes all the orders related.
        /// </summary>
        /// <param name="catalogID"></param>
        /// <returns></returns>
        public static bool deleteWorkOrdersByCatalogID(string catalogID)
        {
            return DBConnectionManager.deleteWorkOrdersByCatalogID(catalogID);
        }

    }
}
