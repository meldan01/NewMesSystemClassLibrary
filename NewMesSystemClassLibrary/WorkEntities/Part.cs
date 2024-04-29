using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewMASMAnagementApplication.WorkEntities;
using NewMesSystemClassLibrary.ExternalComponents;

namespace NewMasApp.WorkEntities
{
    public class Part : GeneralEntity
    {
        private string m_catalogNumber;
        private string m_description;
        private Validations validations;

        public string CatalogNumber
        {
            get { return m_catalogNumber; }
            set { m_catalogNumber = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public Part(DateTime creationDate, string createdBy, string languageCode, string catalogNumber, string description)
            : base(creationDate, createdBy, languageCode)
        {
            CatalogNumber = catalogNumber;
            Description = description;
            validations = Validations.GetInstance();
        }


        /// <summary>
        /// partExists returns true if part catalogName already exists in the list
        /// </summary>
        /// <param name="catalogName"></param>
        /// <returns></returns>
        public static bool partExists(string catalogName)
        {
            return ExternalComponents.DBConnectionManager.isCatalogIDExists(catalogName);
        }

        /// <summary>
        /// Adds an object to the DB
        /// </summary>
        /// <returns></returns>
        public bool insertPartIntoDB()
        {
            if (!validateFieldsNotNullOrEmpty() || !validateSavePart(CatalogNumber, Description, CreationDate, CreatedBy, LanguageCode))
                return false;
            return ExternalComponents.DBConnectionManager.insertPartIntoDB(CatalogNumber, Description, CreationDate, CreatedBy, LanguageCode);
        }

        private bool validateSavePart(string catalogNumber, string description, DateTime creationDate, string createdBy, string languageCode)
        {
            if (!Validations.validateCatalogNumber(catalogNumber))
                return false;
            if (!Validations.validateDescription(description))
                return false;
            if (!Validations.validateCreationDate(creationDate))
                return false;
            if (!Validations.validateCreatorID(createdBy))
                return false;
            if (!Validations.validateLanguageCode(languageCode))
                return false;
            if (partExists(catalogNumber))
                return false;
            return true;
        }

        /// <summary>
        /// validateFieldsNotNullOrEmpty - one last validation in addition to the UI checks that no fields 
        /// will enter null or umpty to the DB
        /// </summary>
        /// <returns></returns>
        private bool validateFieldsNotNullOrEmpty()
        {
            if (string.IsNullOrEmpty(CatalogNumber) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(CreatedBy) ||
                string.IsNullOrEmpty(LanguageCode) || CreationDate == DateTime.MinValue)
                return false;
            return true;
        }

        /// <summary>
        /// updatePart - Updates the fields that are not null or empty in the DB
        /// </summary>
        /// <param name="catalogID"></param>
        /// <param name="itemDescription"></param>
        /// <param name="selectedDate"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updatePart(string catalogID, string itemDescription, DateTime? selectedDate, string creatorID, string languageCode)
        {
            if (!validatePartUpdate(catalogID, itemDescription, selectedDate, creatorID, languageCode))
                return false;
            return ExternalComponents.DBConnectionManager.updatePart(catalogID, itemDescription, selectedDate, creatorID, languageCode);
        }

        /// <summary>
        /// validatePartUpdate - backend validations
        /// </summary>
        /// <param name="catalogID"></param>
        /// <param name="itemDescription"></param>
        /// <param name="selectedDate"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private static bool validatePartUpdate(string catalogID, string itemDescription, DateTime? selectedDate, string creatorID, string languageCode)
        {
            if (!Validations.updateValidatecatalodID(catalogID))
                return false;
            if (!Validations.updateDescriptionLengthCheck(itemDescription))
                return false;
            if (!Validations.updateValidateCreator(creatorID))
                return false;
            if (!Validations.updateValidateLanguageCode(languageCode))
                return false;
            if (!partExists(catalogID))
                return false;
            return true;
        }


        /// <summary>
        /// getPartsInfo - build a string of info of all the Part DB table
        /// </summary>
        /// <returns></returns>
        public static string fetchPartsInfo()
        {
            string totalParts = string.Empty;
            totalParts = ExternalComponents.DBConnectionManager.buildPartsString();
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
            if (!partExists(catalogID))
                return false;
            return ExternalComponents.DBConnectionManager.deletePart(catalogID);
        }

        /// <summary>
        /// deleteWorkOrdersByCatalogID - gets a part catalog ID and deletes all the orders related.
        /// </summary>
        /// <param name="catalogID"></param>
        /// <returns></returns>
        public static bool deleteWorkOrdersByCatalogID(string catalogID)
        {
            return ExternalComponents.DBConnectionManager.deleteWorkOrdersByCatalogID(catalogID);
        }

    }
}
