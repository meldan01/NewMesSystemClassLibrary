using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewMesSystemClassLibrary.ExternalComponents;

namespace NewMasApp.WorkEntities
{
    public class WorkOrder : NewMASMAnagementApplication.WorkEntities.GeneralEntity
    {
        private string m_workOrderNumber;
        private string m_partCatalogNumber;
        private string m_machineName;
        private WorkEntities.Machine m_machine;
        private Validations validations;
        private string m_amountToProduce;

        public string WorkOrderNumber
        {
            get { return m_workOrderNumber; }
            set { m_workOrderNumber = value; }
        }

        public string PartCatalogNumber
        {
            get { return m_partCatalogNumber; }
            set { m_partCatalogNumber = value; }
        }

        public string MachineName
        {
            get { return m_machineName; }
            set { m_machineName = value; }
        }

        public string AmountToProduce
        {
            get { return m_amountToProduce; }
            set { m_amountToProduce = value; }
        }

        private static ExternalComponents.Logger logInstance = ExternalComponents.Logger.getInstance();

        public WorkOrder(DateTime creationDate, string createdBy, string languageCode, string workOrderNumber, string partCatalogNumber, string machineName, string quantity)
            : base(creationDate, createdBy, languageCode)
        {
            WorkOrderNumber = workOrderNumber;
            PartCatalogNumber = partCatalogNumber;
            MachineName = machineName;
            AmountToProduce = quantity;
            m_machine =  Machine.getMachineFromDb(machineName);
            validations = Validations.GetInstance();
        }

        public static bool orderExists(string orderNumber)
        {
            return ExternalComponents.DBConnectionManager.isOrderNumberExists(orderNumber);
        }

        public bool insertWorkOrderIntoDB()
        {
            if (!validateFieldsNotNullOrEmpty() || !validateFields(WorkOrderNumber, PartCatalogNumber, MachineName,
                AmountToProduce, CreationDate, CreatedBy, LanguageCode))
                return false;
            return ExternalComponents.DBConnectionManager.sendWorkOrderToDB(WorkOrderNumber, PartCatalogNumber, MachineName,
                AmountToProduce, CreationDate, CreatedBy, LanguageCode);
        }

        private bool validateFields(string workOrderNumber, string partCatalogNumber, string machineName, string amountToProduce, DateTime creationDate, string createdBy, string languageCode)
        {
            if (!Validations.validateOrderNumber(workOrderNumber))
                return false;
            if (!Validations.validateCatalogNumber(partCatalogNumber))
                return false;
            if (!Validations.validateMachineName(machineName))
                return false;
            if (!Validations.validateQuantity(amountToProduce))
                return false;
            if (!Validations.validateCreationDate(creationDate))
                return false;
            if (!Validations.validateCreatorID(createdBy))
                return false;
            if (!Validations.validateLanguageCode(languageCode))
                return false;
            if (!Machine.machineExists(machineName))
                return false;
            if (!Part.partExists(partCatalogNumber))
                return false;
            if (orderExists(workOrderNumber))
                return false;
            return true;
        }

        private bool validateFieldsNotNullOrEmpty()
        {
            if (string.IsNullOrEmpty(WorkOrderNumber) || string.IsNullOrEmpty(PartCatalogNumber) || string.IsNullOrEmpty(MachineName) ||
                string.IsNullOrEmpty(AmountToProduce) || string.IsNullOrEmpty(CreatedBy) || string.IsNullOrEmpty(LanguageCode) || CreationDate == DateTime.MinValue)
                return false;
            return true;
        }

        public static string fetchOrdersInfo()
        {
            string totalWorkOrders = ExternalComponents.DBConnectionManager.buildWorkOrdersString();
            if (totalWorkOrders == string.Empty)
                totalWorkOrders = "No data in the DataBase";
            return totalWorkOrders;
        }

        public static bool deleteWorkOrder(string orderNumber)
        {
            return ExternalComponents.DBConnectionManager.deleteWorkOrder(orderNumber);
        }

        public static bool updateWorkOrder(string orderNumber, string catalogID, string machineName, string quantity, DateTime? selectedDate, string creatorID, string languageCode)
        {
            if (!validateUpdate(orderNumber, catalogID, machineName,
                quantity, selectedDate, creatorID, languageCode))
                return false;
            return ExternalComponents.DBConnectionManager.updateWorkOrder(orderNumber, catalogID, machineName,
                quantity, selectedDate, creatorID, languageCode);
        }

        private static bool validateUpdate(string orderNumber, string catalogID, string machineName, string quantity, DateTime? selectedDate, string creatorID, string languageCode)
        {
            if (!Validations.updateValidateOrderNumber(orderNumber))
                return false;
            if (!Validations.updateValidatecatalodID(catalogID))
                return false;
            if (!Validations.updateValidateMachineName(machineName))
                return false; 
            if (!Validations.updateValidateOrderQuantity(quantity))
                return false;
            if (!Validations.updateValidateCreator(creatorID))
                return false;
            if (!Validations.updateValidateLanguageCode(languageCode))
                return false;
            if (!Machine.machineExists(machineName))
                return false;
            if (!Part.partExists(catalogID))
                return false;
            if (!orderExists(orderNumber))
                return false;
            return true;
        }
    }
}