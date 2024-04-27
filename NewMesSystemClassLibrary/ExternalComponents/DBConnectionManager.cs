using NewMesSystemClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMasApp.ExternalComponents
{
    class DBConnectionManager
    {
        private static DBConnectionManager instance = null;
        private static NewManagementDBTablesEntities dbConnection = null;
        private static Logger loggerInstance = Logger.getInstance();


        /*Establish DB connection(Constructor)*/
        private DBConnectionManager()
        {
            dbConnection = new NewManagementDBTablesEntities();
            if (dbConnection == null)
                EndProgram();
        }

        public static void TryPost()
        {
            var machine = new Machine()
            {
                MachineName = "Eldan TestMachine"
      ,
                DateOfCreation = DateTime.Now
      ,
                CreatorID = "304873219"
      ,
                LanguageCode = "1"
            };
            dbConnection.Machines.Add(machine);
            dbConnection.SaveChanges();
        }
        /*EndProgram function - In case of failure on sql connection close the program*/
        private void EndProgram()
        {
            loggerInstance.Log("Error" + "- Could not establish DB connection, Exit program with code 1.");
            //Environment.Exit(0);
        }


        /// <summary>
        /// Public static method to get the singeltone of the db connection
        /// </summary>
        /// <returns></returns>
        public static DBConnectionManager getInstance()
        {
            if (instance == null)
            {
                instance = new DBConnectionManager();
            }
            return instance;
        }

        #region workOrderSQLQuerys
        /// <summary>
        /// dbWorkOrderContent - returns all the object in the workOrder Database table
        /// </summary>
        /// <returns></returns>
        public static string getAllWorkOrders()
        {
            StringBuilder OrdersInfo = new StringBuilder();
            try
            {
                var orderNumbers = dbConnection.WorkOrders.OrderBy(p => p.OrderNumber).ToList();

                foreach (var order in orderNumbers)
                {
                    OrdersInfo.AppendLine($"Order number: {order.OrderNumber}");
                    OrdersInfo.AppendLine($"Catalog ID: {order.CatalogID}");
                    OrdersInfo.AppendLine($"Machine name: {order.MachineName}");
                    OrdersInfo.AppendLine($"Quantity: {order.AmountToProduce}");
                    OrdersInfo.AppendLine($"Created at: {order.DateOfCreation.ToString("dd/MM/yyyy")}");
                    OrdersInfo.AppendLine($"Creator ID: {order.CreatorID}");
                    OrdersInfo.AppendLine($"Language Code: {order.LanguageCode}");
                    OrdersInfo.AppendLine(); // This adds an empty line after each product
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log("Error - " + ex.Message);
                return string.Empty;
            }

            return OrdersInfo.ToString();
        }

        /// <summary>
        /// OrderNumberExists - Checks if order exists in the db by primaryKey orderNumber
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public static bool orderNumberExists(string orderNumber)
        {
            try
            {
                var existingOrder = dbConnection.WorkOrders.FirstOrDefault(o => o.OrderNumber == orderNumber);
                return existingOrder != null;
            }
            catch (Exception ex)
            {
                loggerInstance.Log("Error - " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// addWorkOrderToDB - Adds order to the DB
        /// </summary>
        /// <param name="workOrderNumber"></param>
        /// <param name="partCatalogNumber"></param>
        /// <param name="machineName"></param>
        /// <param name="amountToProduce"></param>
        /// <param name="creationDate"></param>
        /// <param name="createdBy"></param>
        /// <param name="languageCode"></param>
        public static bool sendWorkOrderToDB(string workOrderNumber, string partCatalogNumber, string machineName, string amountToProduce, DateTime creationDate, string createdBy, string languageCode)
        {
            try
            {
                var newWorkOrder = new WorkOrder
                {
                    OrderNumber = workOrderNumber,
                    CatalogID = partCatalogNumber,
                    MachineName = machineName,
                    AmountToProduce = amountToProduce,
                    DateOfCreation = creationDate,
                    CreatorID = createdBy,
                    LanguageCode = languageCode
                };
                dbConnection.WorkOrders.Add(newWorkOrder);
                dbConnection.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                loggerInstance.Log("Error - " + $"An error occurred while adding the work order to the database: { ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// DeleteWorkOrderByOrderNumber - deletes order by given primaryKey from DB
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public static bool deleteWorkOrderByOrderNumber(string orderNumber)
        {
            try
            {
                var workOrderToDelete = dbConnection.WorkOrders.FirstOrDefault(o => o.OrderNumber == orderNumber);
                if (workOrderToDelete != null)
                {
                    dbConnection.WorkOrders.Remove(workOrderToDelete);
                    dbConnection.SaveChanges();
                    loggerInstance.Log("Debug" + $" - Work orders with order number '{orderNumber}' deleted successfully.");
                    return true;
                }
                else
                {
                    loggerInstance.Log("Debug" + $" - Work order with order number '{orderNumber}' not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log("Error" + $" - An error occurred while deleting the work order: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// UpdateWorkOrder - Gets new values from the UI and updates
        /// the fields that are not null in the DB 
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="catalogID"></param>
        /// <param name="machineName"></param>
        /// <param name="amountToProduce"></param>
        /// <param name="dateOfCreation"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updateWorkOrder(string orderNumber, string catalogID, string machineName, string amountToProduce, DateTime? dateOfCreation, string creatorID, string languageCode)
        {
            try
            {
                var existingWorkOrder = dbConnection.WorkOrders.FirstOrDefault(o => o.OrderNumber == orderNumber);
                if (existingWorkOrder != null)
                {
                    if (!string.IsNullOrEmpty(catalogID))
                    {
                        existingWorkOrder.CatalogID = catalogID;
                    }
                    if (!string.IsNullOrEmpty(machineName))
                    {
                        existingWorkOrder.MachineName = machineName;
                    }
                    if (!string.IsNullOrEmpty(amountToProduce))
                    {
                        existingWorkOrder.AmountToProduce = amountToProduce;
                    }
                    if (dateOfCreation != null)
                    {
                        existingWorkOrder.DateOfCreation = dateOfCreation.Value;
                    }
                    if (!string.IsNullOrEmpty(creatorID))
                    {
                        existingWorkOrder.CreatorID = creatorID;
                    }
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        existingWorkOrder.LanguageCode = languageCode;
                    }

                    dbConnection.SaveChanges();
                    loggerInstance.Log($"Debug - Updated work order with order number '{orderNumber}'.");
                    return true;
                }
                else
                {
                    loggerInstance.Log($"Debug - Work order with order number '{orderNumber}' not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - An error occurred while updating the work order: {ex.Message}");
                return false;
            }
        }

        #endregion workOrderSQLQuerys

        #region PartQuery

        /// <summary>
        /// dbPartsContent - returns all the content of Parts DB table
        /// </summary>
        /// <returns></returns>
        public static string getAllPartsInfo()
        {
            try
            {
                var allParts = dbConnection.Parts.ToList();

                StringBuilder partsInfo = new StringBuilder();
                foreach (var part in allParts)
                {
                    partsInfo.AppendLine($"Catalog ID: {part.CatalogID}");
                    partsInfo.AppendLine($"Item Description: {part.ItemDescription}");
                    partsInfo.AppendLine($"Date Of Creation: {part.DateOfCreation.ToString("dd/MM/yyyy")}");
                    partsInfo.AppendLine($"Creator ID: {part.CreatorID}");
                    partsInfo.AppendLine($"Language Code: {part.LanguageCode}");
                    partsInfo.AppendLine(); // Add an empty line between each part
                }

                return partsInfo.ToString();
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - An error occurred while retrieving parts: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// catalogIDExists - gets a catalog ID code and returns if it exists in the parts DB table
        /// </summary>
        /// <param name="catalogID"></param>
        /// <returns></returns>
        public static bool catalogIDExists(string catalogID)
        {
            try
            {
                int count = dbConnection.Parts.Count(p => p.CatalogID == catalogID);
                return count > 0;
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///UpdateParts - Gets new values from the UI and updates
        /// the fields that are not null in the DB 
        /// <param name="catalogId"></param>
        /// <param name="itemDescription"></param>
        /// <param name="dateOfCreation"></param>
        /// <param name="creatorId"></param>
        /// <param name="languageCode"></param>
        public static bool updatePart(string catalogID, string itemDescription, DateTime? dateOfCreation, string creatorID, string languageCode)
        {
            try
            {
                var existingPart = dbConnection.Parts.FirstOrDefault(p => p.CatalogID == catalogID);
                if (existingPart != null)
                {
                    if (!string.IsNullOrEmpty(itemDescription))
                    {
                        existingPart.ItemDescription = itemDescription;
                    }
                    if (dateOfCreation != null)
                    {
                        existingPart.DateOfCreation = dateOfCreation.Value;
                    }
                    if (!string.IsNullOrEmpty(creatorID))
                    {
                        existingPart.CreatorID = creatorID;
                    }
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        existingPart.LanguageCode = languageCode;
                    }

                    dbConnection.SaveChanges();

                    loggerInstance.Log($"Debug - Updated part with catalog ID '{catalogID}'.");
                    return true;
                }
                else
                {
                    loggerInstance.Log($"Debug - Part with catalog ID '{catalogID}' not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - An error occurred while updating the part: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// DeletePart - Deletes part in the DB if it exists, if there is a related 
        /// workOrder, it will delete it also.
        /// </summary>
        /// <param name="catalogId"></param>
        public static bool deletePart(string catalogId)
        {
            try
            {
                var partToDelete = dbConnection.Parts.FirstOrDefault(p => p.CatalogID == catalogId);
                if (partToDelete != null)
                {
                    dbConnection.Parts.Remove(partToDelete);
                    dbConnection.SaveChanges();
                    loggerInstance.Log($"Debug - Deleted part with CatalogID {catalogId}.");
                    return true;
                }
                else
                {
                    loggerInstance.Log($"Debug - No part found with CatalogID {catalogId}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// addPartsToDb - Gets a part details and adds it to the DB table
        /// </summary>
        /// <param name="catalogNumber"></param>
        /// <param name="description"></param>
        /// <param name="creationDate"></param>
        /// <param name="createdBy"></param>
        /// <param name="languageCode"></param>
        public static bool addPartToDb(string catalogNumber, string description, DateTime creationDate, string createdBy, string languageCode)
        {
            try
            {
                var newPart = new Part
                {
                    CatalogID = catalogNumber,
                    ItemDescription = description,
                    DateOfCreation = creationDate,
                    CreatorID = createdBy,
                    LanguageCode = languageCode
                };

                dbConnection.Parts.Add(newPart);
                dbConnection.SaveChanges();
                loggerInstance.Log($"Debug - Part {catalogNumber} inserted successfully to DB.");
                return true;
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - Failed to insert Part {catalogNumber} to DB. {ex.Message}");
                return false;
            }
        }

        #endregion PartQuery
        #region machineSQLQuerys

        /// <summary>
        /// dbMachinesContent - returns all the content of machines table in the DB
        /// </summary>
        /// <returns></returns>
        public static string getAllMachinesInfo()
        {
            StringBuilder contentString = new StringBuilder();
            try
            {
                var machines = dbConnection.Machines.ToList();
                foreach (var machine in machines)
                {
                    contentString.AppendLine($"Machine name: {machine.MachineName},");
                    contentString.AppendLine($"Date of creation: {machine.DateOfCreation.ToString("dd/MM/yyyy")},");
                    contentString.AppendLine($"Creator ID: {machine.CreatorID},");
                    contentString.AppendLine($"Language code: {machine.LanguageCode}");
                    contentString.AppendLine(); // Add an empty line between each machine
                }
                return contentString.ToString();
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// MachineExists - returns if machine exists in the db according to the primaryKey machineName
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool machineExists(string machineName)
        {
            try
            {
                int count = dbConnection.Machines.Count(m => m.MachineName == machineName);
                return count > 0;
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// addMachineToDb - add machine to the machines DB table
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="creationDateLocal"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        public static bool addMachineToDb(string machineName, DateTime creationDateLocal, string creatorID, string languageCode)
        {
            try
            {
                var machine = new Machine()
                {
                    MachineName = machineName,
                    DateOfCreation = creationDateLocal,
                    CreatorID = creatorID,
                    LanguageCode = languageCode
                };

                dbConnection.Machines.Add(machine);
                dbConnection.SaveChanges();
                loggerInstance.Log("Debug - Machine added to DB successfully.");
                return true;
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - Failed to insert data to DB. {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// DeleteMachine - deletes machine in the DB by primaryKey machineName
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool deleteMachine(string machineName)
        {
            try
            {
                var machineToDelete = dbConnection.Machines.FirstOrDefault(m => m.MachineName == machineName);
                if (machineToDelete != null)
                {
                    dbConnection.Machines.Remove(machineToDelete);
                    dbConnection.SaveChanges();
                    loggerInstance.Log($"Debug - Deleted machine with name '{machineName}'.");
                    return true;
                }
                else
                {
                    loggerInstance.Log($"Debug - No machine found with name '{machineName}'.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// DeleteItemsByMachineName - delete orders that machine name corresponds to the machineName
        /// used to delete all orders in case of machine delete
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static bool DeleteOrdersByMachineName(string machineName)
        {
            try
            {
                var ordersToDelete = dbConnection.WorkOrders.Where(w => w.MachineName == machineName).ToList();
                dbConnection.WorkOrders.RemoveRange(ordersToDelete);
                dbConnection.SaveChanges();

                int rowsAffected = ordersToDelete.Count;
                if (rowsAffected > 0)
                {
                    loggerInstance.Log($"Debug - Deleted {rowsAffected} work order(s) with machine name '{machineName}'.");
                    return true;
                }
                else
                {
                    loggerInstance.Log($"Debug - No work orders found with machine name '{machineName}'.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///UpdateMachine - Gets new values from the UI and updates
        /// the fields that are not null in the DB 
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="dateOfCreation"></param>
        /// <param name="creatorID"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static bool updateMachine(string machineName, DateTime? dateOfCreation, string creatorID, string languageCode)
        {
            try
            {
                var machineToUpdate = dbConnection.Machines.FirstOrDefault(m => m.MachineName == machineName);
                if (machineToUpdate != null)
                {
                    if (dateOfCreation != null)
                    {
                        machineToUpdate.DateOfCreation = dateOfCreation.Value;
                    }
                    if (!string.IsNullOrEmpty(creatorID))
                    {
                        machineToUpdate.CreatorID = creatorID;
                    }
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        machineToUpdate.LanguageCode = languageCode;
                    }

                    dbConnection.SaveChanges();
                    loggerInstance.Log($"Debug - Updated machine: {machineName}");
                    return true;
                }
                else
                {
                    loggerInstance.Log($"Debug - No machine found for update: {machineName}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerInstance.Log($"Error - {ex.Message}");
                return false;
            }
        }

        #endregion machineSQLQuerys
    }
}
