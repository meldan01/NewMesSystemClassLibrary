using NewMasApp.ExternalComponents;
using NewMASMAnagementApplication.WorkEntities;
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
            return DBConnectionManager.isMachineExists(machineName);
        }

        /// <summary>
        /// sendMachineToDB - adds machine to the DB table
        /// </summary>
        public bool insertMachineIntoDB()
        {
            return DBConnectionManager.insertMachineIntoDB(m_machineName, creationDate, createdBy, languageCode);
        }

        /// <summary>
        /// getMachinesInfo - builds a string of the entire machines in th DB 
        /// </summary>
        /// <returns></returns>
        public static string fetchMachinesInfo()
        {
            string totalMachines = string.Empty;

            totalMachines = DBConnectionManager.buildMachinesString();
            if (totalMachines == string.Empty)
                totalMachines = "No data in the DataBase";
            return totalMachines;
        }

        /// <summaryuserValidationDeleteMachine
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
        public static bool DeleteOrdersByMachineName(string machineName)
        {
            return DBConnectionManager.deleteOrdersByMachineName(machineName);
        }


    }
}
