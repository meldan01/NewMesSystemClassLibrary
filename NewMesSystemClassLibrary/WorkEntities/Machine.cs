using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewMesSystemClassLibrary.ExternalComponents;
using NewMASMAnagementApplication.WorkEntities;

namespace NewMasApp.WorkEntities
{
    public class Machine : GeneralEntity
    {
        private static ExternalComponents.Logger logInstace = ExternalComponents.Logger.getInstance();
        private string m_machineName;
        private Validations validations;
        public string MachineName
        {
            get { return m_machineName; }
            set { m_machineName = value; }
        }

        public Machine(DateTime creationDate, string createdBy, string languageCode, string machineName)
            : base(creationDate, createdBy, languageCode)
        {
            MachineName = machineName;
            validations = Validations.GetInstance();
        }

        /// <summary>
        /// machineExists - check if machine exists in the machines DB table
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool machineExists(string machineName)
        {
            return ExternalComponents.DBConnectionManager.isMachineExists(machineName);
        }

        /// <summary>
        /// sendMachineToDB - adds machine to the DB table
        /// </summary>
        public bool insertMachineIntoDB()
        {
            if (!validateFieldsNotNullOrEmpty() || !validateFields(MachineName, CreationDate, CreatedBy, LanguageCode))
                return false;
            return ExternalComponents.DBConnectionManager.insertMachineIntoDB(MachineName, CreationDate, CreatedBy, LanguageCode);
        }

        private bool validateFields(string machineName, DateTime? creationDate,string creatorID,string languageCode)
        {
            if (!Validations.validateMachineName(machineName))
                return false;
            if (!Validations.validateCreationDate(creationDate))
                return false;
            if (!Validations.validateCreatorID(creatorID))
                return false; 
            if (!Validations.validateLanguageCode(languageCode))
                return false;
            if (machineExists(machineName))
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
            if (string.IsNullOrEmpty(MachineName) || string.IsNullOrEmpty(CreatedBy) || string.IsNullOrEmpty(LanguageCode) || CreationDate == DateTime.MinValue)
                return false;
            return true;
        }

        /// <summary>
        /// getMachinesInfo - builds a string of the entire machines in th DB 
        /// </summary>
        /// <returns></returns>
        public static string fetchMachinesInfo()
        {
            string totalMachines = string.Empty;

            totalMachines = ExternalComponents.DBConnectionManager.buildMachinesString();
            if (totalMachines == string.Empty)
                totalMachines = "No data in the DataBase";
            return totalMachines;
        }

        /// <summary>
        /// updateMachine - updates machine information in the DB
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="selectedDate"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updateMachine(string machineName, DateTime? selectedDate, string creatorID, string languageCode)
        {
            if (!validateUpdate(machineName, selectedDate, creatorID, languageCode))
                return false;
            return ExternalComponents.DBConnectionManager.updateMachine(machineName, selectedDate, creatorID, languageCode);
        }

        private static bool validateUpdate(string machineName, DateTime? selectedDate, string creatorID, string languageCode)
        {
            if (!Validations.updateValidateCreator(creatorID))
                return false;
            if (!Validations.updateValidateLanguageCode(languageCode))
                return false;
            if (!Validations.updateValidateMachineName(machineName))
                return false;
            if (!Validations.validateCreationDate(selectedDate))
                return false;
            if (!machineExists(machineName))
                return false;
            return true;
        }


        /// <summary>
        /// deleteMachine - deletes machine row in the DB
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool deleteMachine(string machineName)
        {
            return ExternalComponents.DBConnectionManager.deleteMachine(machineName);
        }

        /// <summary>
        /// DeleteOrdersByMachineName - deletes all the orders that related to the machine that deleted
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool deleteOrdersByMachineName(string machineName)
        {
            return ExternalComponents.DBConnectionManager.deleteOrdersByMachineName(machineName);
        }
        
        public static WorkEntities.Machine getMachineFromDb(string machineName)
        {
            return ExternalComponents.DBConnectionManager.getMachine(machineName);
        }
    }
}
