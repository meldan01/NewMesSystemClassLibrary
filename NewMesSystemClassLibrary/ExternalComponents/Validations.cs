using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMesSystemClassLibrary.ExternalComponents
{
    public class Validations
    {
        private static Validations instance;

        private Validations()
        {
        }

        public static Validations GetInstance()
        {
            if (instance == null)
            {
                instance = new Validations();
            }
            return instance;
        }


        #region save
        public static bool validateOrderNumber(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber) ||
                (!orderNumber.All(char.IsDigit)) || orderNumber.Length > 50)
            {
                return false;
            }
            else
                return true;
        }

        public static bool validateCreatorID(string creatorID)
        {
            if (string.IsNullOrEmpty(creatorID) ||
                (!creatorID.All(char.IsDigit)) || creatorID.Length != 9)
            {
                return false;
            }
            else
                return true;
        }

        public static bool validateQuantity(string quantity)
        {
            if (string.IsNullOrEmpty(quantity) ||
                (!quantity.All(char.IsDigit)) || quantity.Length > 50)
                return false;
            else
                return true;
        }

        /// <summary>
        /// machineNameOrderValidation - Validates the machine name
        /// </summary>
        /// <returns></returns>
        public static bool validateMachineName(string machineName)
        {
            if (string.IsNullOrEmpty(machineName) || machineName.Length > 50)
                return false;
            else
                return true;
        }

        /// <summary>
        /// catalogIDValidation - validates the catalog ID field
        /// </summary>
        /// <returns></returns>
        public static bool validateCatalogNumber(string catalogID)
        {
            if (string.IsNullOrEmpty(catalogID) ||
                (!catalogID.All(char.IsDigit)) || catalogID.Length > 50)
                return false;
            else
                return true;
        }

        /// <summary>
        /// validateCreationDate - validates the general creationDate field
        /// </summary>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        public static bool validateCreationDate(DateTime? creationDate)
        {
            if (creationDate == null || !creationDate.HasValue)
                return false;
            return true;
        }

        /// <summary>
        /// /// validateLanguageCode - validates the general languageCode field
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool validateLanguageCode(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode) ||
                (!languageCode.All(char.IsDigit) || languageCode.Length > 5))
                return false;
            else
                return true;
        }

        public static bool validateDescription(string description)
        {
            if (description.Length < 25 || description.Length > 254)
                return false;
            else
                return true;
        }
        #endregion


        #region update
        /// <summary>
        /// descriptionLengthValidation - makes sure that the user added some minimal proper description
        /// </summary>
        /// <returns></returns>
        public static bool updateDescriptionLengthCheck(string description)
        {
            if (!string.IsNullOrEmpty(description))
                if (description.Length < 25 || description.Length > 254)
                    return false;
            return true;
        }

        /// <summary>
        /// validateUpdateCreator - validates the creator ID field in update
        /// </summary>
        /// <param name="creatorID"></param>
        /// <returns></returns>
        public static bool updateValidateCreator(string creatorID)
        {
            if (!string.IsNullOrEmpty(creatorID) && (creatorID.Length != 9 || !creatorID.All(char.IsDigit)))
                return false;
            return true;
        }

        /// <summary>
        /// validateUpdateLanguageCode - validates the General languageCode field in update
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updateValidateLanguageCode(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode) && ( !languageCode.All(char.IsDigit) || languageCode.Length > 5))
                return false;
            return true;
        }

        /// <summary>
        /// validateUpdateOrderQuantity -  Validates the quantity field in case of update
        /// </summary>
        /// <returns></returns>
        public static bool updateValidateOrderQuantity(string quantity)
        {
            if (!string.IsNullOrEmpty(quantity) &&
                (!quantity.All(char.IsDigit)) || quantity.Length > 50)
                return false;
            else
                return true;
        }

        /// <summary>
        /// isUpdateMachineExist - implements the logic of MachineExists when 
        /// catalog id might be null in update
        /// </summary>
        /// <returns></returns>
        public static bool updateIsMachineExist(string machineName)
        {
            if (!string.IsNullOrEmpty(machineName) && !(NewMasApp.WorkEntities.Machine.machineExists(machineName)))
                return false;
            return true;
        }

        /// <summary>
        /// orderNumberValidation - Validates the order number field
        /// </summary>
        /// <returns></returns>
        public static bool updateValidateOrderNumber(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber) ||
                (!orderNumber.All(char.IsDigit)) || orderNumber.Length > 50)
            {
                return false;
            }
            else
                return true;
        }

        public static bool updateValidateMachineName(string machineName)
        {
            if (string.IsNullOrEmpty(machineName) || machineName.Length > 50)
            {
                return false;
            }
            return true;
        }
        
        public static bool updateValidatecatalodID(string catalogNumber)
        {
            if (!string.IsNullOrEmpty(catalogNumber) && ((!catalogNumber.All(char.IsDigit) || catalogNumber.Length > 50)))
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
