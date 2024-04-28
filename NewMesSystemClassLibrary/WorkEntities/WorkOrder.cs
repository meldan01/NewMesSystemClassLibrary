using NewMasApp.ExternalComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMasApp.WorkEntities
{
    public class WorkOrder : NewMASMAnagementApplication.WorkEntities.GeneralEntity
    {
        public string m_workOrderNumber { get; set; }
        public string m_partCatalogNumber { get; set; }
        public string m_machineName { get; set; }
        public string m_amountToProduce { get; set; }

        private static Logger logInstance = Logger.getInstance();

        public WorkOrder(DateTime creationDate, string createdBy, string languageCode, string workOrderNumber, string partCatalogNumber, string machineName, string quantity)
            : base(creationDate, createdBy, languageCode)
        {
            this.m_workOrderNumber = workOrderNumber;
            this.m_partCatalogNumber = partCatalogNumber;
            this.m_machineName = machineName;
            this.m_amountToProduce = quantity;
        }

        /// <summary>
        /// orderExists - return true if order with the same order number exists in DB
        /// else false
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public static bool orderExists(string orderNumber)
        {
            return DBConnectionManager.isOrderNumberExists(orderNumber);
        }


        /// <summary>
        /// sendWorkOrderToDB - Sends workOrder to the DB
        /// </summary>
        public bool insertWorkOrderIntoDB()
        {
            return DBConnectionManager.sendWorkOrderToDB(m_workOrderNumber, m_partCatalogNumber, m_machineName,
            m_amountToProduce, creationDate, createdBy, languageCode);
        }


        /// <summary>
        /// getOrdersInfo - return all the orders information from the DB
        /// </summary>
        /// <returns></returns>
        public static string fetchOrdersInfo()
        {
            string totalWorkOrders = string.Empty;
            totalWorkOrders = DBConnectionManager.buildWorkOrdersString();
            if (totalWorkOrders == string.Empty)
                totalWorkOrders = "No data in the DataBase";
            return totalWorkOrders;
        }

        /// <summary>
        /// deleteWorkOrderByOrderNumber - gets an order number and delete it from the WorkOrder table
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public static bool deleteWorkOrder(string orderNumber)
        {
            return DBConnectionManager.deleteWorkOrder(orderNumber);
        }

        /// <summary>
        /// updateWorkOrder - gets the entire properties of workOrder(some might be null) 
        /// and updates order in the db, by primary key
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="catalogID"></param>
        /// <param name="machineName"></param>
        /// <param name="quantity"></param>
        /// <param name="selectedDate"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updateWorkOrder(string orderNumber, string catalogID, string machineName, string quantity, DateTime? selectedDate, string creatorID, string languageCode)
        {
            return DBConnectionManager.updateWorkOrder(orderNumber, catalogID, machineName,
                quantity, selectedDate, creatorID, languageCode);
        }
    }
}
