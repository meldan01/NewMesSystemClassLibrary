using NewMasApp.ExternalComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMasApp.WorkEntities
{
    public class Machine : GeneralEntity
    {
        private static Logger logInstace = Logger.getInstance();
        public string m_machineName { get; set; }

        public Machine(DateTime creationDate, string createdBy, string languageCode, string machineName)
            : base(creationDate, createdBy, languageCode)
        {
            this.m_machineName = machineName;
        }

        /// <summary>
        /// machineExists - check if machine exists in the machines DB table
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool machineExists(string machineName)
        {
            return DBConnectionManager.machineExists(machineName);
        }

        /// <summary>
        /// sendMachineToDB - adds machine to the DB table
        /// </summary>
        public void sendMachineToDB()
        {
            DBConnectionManager.addMachineToDb(m_machineName, creationDate, createdBy, languageCode);
        }

        /// <summary>
        /// getMachinesInfo - builds a string of the entire machines in th DB 
        /// </summary>
        /// <returns></returns>
        public static string getMachinesInfo()
        {
            string totalMachines = string.Empty;

            totalMachines = DBConnectionManager.getAllMachinesInfo();
            if (totalMachines == string.Empty)
                totalMachines = "No data in the DataBase";
            return totalMachines;
        }

        /// <summary>
        /// updateMachine - updates machine row in the table
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="selectedDate"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updateMachine(string machineName, DateTime? selectedDate, string creatorID, string languageCode)
        {
            return DBConnectionManager.updateMachine(machineName, selectedDate, creatorID, languageCode);
        }


        /// <summary>
        /// deleteMachine - deletes machine row in the DB
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool deleteMachine(string machineName)
        {
            return DBConnectionManager.deleteMachine(machineName);
        }

        /// <summary>
        /// DeleteOrdersByMachineName - deletes all the orders that related to the machine that deleted
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        internal static bool DeleteOrdersByMachineName(string machineName)
        {
            return DBConnectionManager.DeleteOrdersByMachineName(machineName);
        }

        /// <summary>
        /// isUpdateMachineFieldsValid - validates the fields of machine before update
        /// </summary>
        /// <param name="creatorID"></param>
        /// <returns></returns>
        public static bool isUpdateMachineFieldsValid(string creatorID)
        {
            return (creatorID.Length == 9) && creatorID.All(char.IsDigit);
        }
    }
}
