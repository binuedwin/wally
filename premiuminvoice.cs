using System;
using System.Data;
using System.Threading;
using System.Collections.Generic;
using nTouchCoreLibrary.DOL.Financial;
using nTouchCoreLibrary.DAL.Financial;

namespace nTouchCoreLibrary.BLL.Financial
{
    public class PREMIUMINVOICES
    {
        private static string _ERROR_MESSAGE = string.Empty;

        public static string ERROR_MESSAGE
        {
            get
            {
                return _ERROR_MESSAGE;
            }
            set
            {
                _ERROR_MESSAGE = value;
            }
        }

        #region LoadById
        public static PREMIUMINVOICES_DOL LoadById(Int64 PRMINVOICEID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES.LoadById(PRMINVOICEID);
        }
        #endregion

        #region LoadALL
        public static List<PREMIUMINVOICES_DOL> LoadAll(int? SOURCE, int? OWNERID, int? SOURCENTITYID, int? TARGETENTITYID, string INVOICETYPE, int SPROVIDERID, int? PAYERID, int? PRODUCTLINESIDZ, string PRMINVOICENBR, DateTime? FROMDUEDATE, DateTime? TODUEDATE, string STATUS,string POLICYNO,int? MCONTRACTID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES.LoadAll(SOURCE, OWNERID, SOURCENTITYID, TARGETENTITYID, INVOICETYPE, SPROVIDERID, PAYERID, PRODUCTLINESIDZ, PRMINVOICENBR, FROMDUEDATE, TODUEDATE, STATUS, POLICYNO,MCONTRACTID);
        }
        #endregion

        #region Insert
        public static bool Insert(PREMIUMINVOICES_DOL oPREMIUMINVOICES_DOL)
        {
            Int64 PRMINVOICEID = 0;
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES.Insert(ref PRMINVOICEID, oPREMIUMINVOICES_DOL.SPROVIDERID, oPREMIUMINVOICES_DOL.PAYERID, oPREMIUMINVOICES_DOL.PRMINVOICENBR, oPREMIUMINVOICES_DOL.INVOICETYPE, oPREMIUMINVOICES_DOL.DISCRIPTION, oPREMIUMINVOICES_DOL.FROMDATE, oPREMIUMINVOICES_DOL.TODATE, oPREMIUMINVOICES_DOL.DUEDATE, oPREMIUMINVOICES_DOL.SOURCE, oPREMIUMINVOICES_DOL.SOURCENTITYID, oPREMIUMINVOICES_DOL.SOURCECOAID, oPREMIUMINVOICES_DOL.TARGETENTITYID, oPREMIUMINVOICES_DOL.TARGETCOAID, oPREMIUMINVOICES_DOL.CURRENCYID, oPREMIUMINVOICES_DOL.CVFACTOR, oPREMIUMINVOICES_DOL.AMOUNT, oPREMIUMINVOICES_DOL.ADJUSTMENT, oPREMIUMINVOICES_DOL.TAX, oPREMIUMINVOICES_DOL.FINALAMOUNT, oPREMIUMINVOICES_DOL.ADJUSTMENTNOTES, oPREMIUMINVOICES_DOL.INTERNALNOTES, oPREMIUMINVOICES_DOL.EXTERNALNOTES, oPREMIUMINVOICES_DOL.ATTACHMENT1, oPREMIUMINVOICES_DOL.ATTACHMENT2, oPREMIUMINVOICES_DOL.ATTACHMENT3, oPREMIUMINVOICES_DOL.VALIDATED_BY, oPREMIUMINVOICES_DOL.VALIDATION_DATE, oPREMIUMINVOICES_DOL.APPROVED_BY, oPREMIUMINVOICES_DOL.APPROVED_DATE, oPREMIUMINVOICES_DOL.STATUS, oPREMIUMINVOICES_DOL.PRODUCTLINESIDZ, oPREMIUMINVOICES_DOL.BRANCHIDZ, oPREMIUMINVOICES_DOL.TRANSACTIONTYPES,oPREMIUMINVOICES_DOL.ISDELETED, oPREMIUMINVOICES_DOL.CREATED_BY);
            oPREMIUMINVOICES_DOL.PRMINVOICEID = PRMINVOICEID;
            return true;
        }
        #endregion

        #region Update
        public static void Update(PREMIUMINVOICES_DOL oPREMIUMINVOICES_DOL)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES.Update(oPREMIUMINVOICES_DOL.PRMINVOICEID, oPREMIUMINVOICES_DOL.SPROVIDERID, oPREMIUMINVOICES_DOL.PAYERID, oPREMIUMINVOICES_DOL.PRMINVOICENBR, oPREMIUMINVOICES_DOL.INVOICETYPE, oPREMIUMINVOICES_DOL.DISCRIPTION, oPREMIUMINVOICES_DOL.FROMDATE, oPREMIUMINVOICES_DOL.TODATE, oPREMIUMINVOICES_DOL.DUEDATE, oPREMIUMINVOICES_DOL.SOURCE, oPREMIUMINVOICES_DOL.SOURCENTITYID, oPREMIUMINVOICES_DOL.SOURCECOAID, oPREMIUMINVOICES_DOL.TARGETENTITYID, oPREMIUMINVOICES_DOL.TARGETCOAID, oPREMIUMINVOICES_DOL.CURRENCYID, oPREMIUMINVOICES_DOL.CVFACTOR, oPREMIUMINVOICES_DOL.AMOUNT, oPREMIUMINVOICES_DOL.ADJUSTMENT, oPREMIUMINVOICES_DOL.TAX, oPREMIUMINVOICES_DOL.FINALAMOUNT, oPREMIUMINVOICES_DOL.ADJUSTMENTNOTES, oPREMIUMINVOICES_DOL.INTERNALNOTES, oPREMIUMINVOICES_DOL.EXTERNALNOTES, oPREMIUMINVOICES_DOL.ATTACHMENT1, oPREMIUMINVOICES_DOL.ATTACHMENT2, oPREMIUMINVOICES_DOL.ATTACHMENT3, oPREMIUMINVOICES_DOL.VALIDATED_BY, oPREMIUMINVOICES_DOL.VALIDATION_DATE, oPREMIUMINVOICES_DOL.APPROVED_BY, oPREMIUMINVOICES_DOL.APPROVED_DATE, oPREMIUMINVOICES_DOL.STATUS, oPREMIUMINVOICES_DOL.PRODUCTLINESIDZ, oPREMIUMINVOICES_DOL.BRANCHIDZ, oPREMIUMINVOICES_DOL.TRANSACTIONTYPES, oPREMIUMINVOICES_DOL.ISDELETED, oPREMIUMINVOICES_DOL.MODIFIED_BY, oPREMIUMINVOICES_DOL.MODIFICATION_DATE);
        }
        #endregion

        #region Delete
        public static bool Delete(Int64 PRMINVOICEID)
        {
            _ERROR_MESSAGE = string.Empty;
            string ERR_MSG = string.Empty;
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES.Delete(PRMINVOICEID, ref ERR_MSG);

            if (string.IsNullOrEmpty(ERR_MSG))
            {
                _ERROR_MESSAGE = string.Empty;
                return true;
            }
            else
            {
                _ERROR_MESSAGE = SystemMessages.ResourceManager.GetString(ERR_MSG, Thread.CurrentThread.CurrentCulture);
                if (string.IsNullOrEmpty(_ERROR_MESSAGE))
                {
                    _ERROR_MESSAGE = ERR_MSG;
                }
                return false;
            }
        }
        #endregion

        #region CheckForExistingInvoiceNo
        public static int CheckForExistingInvoiceNo(string INVOICENBR)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES_DAL = new PREMIUMINVOICES_DAL();
            int nCount = oPREMIUMINVOICES_DAL.CheckForExistingInvoiceNo(INVOICENBR);
            return nCount;
        }
        #endregion

        #region MoveTransaction
        public static void MoveTransaction(int MCONTRACTID, int CONTRACTID, int ENDORSEMENTID, int SOURCEID, int INVOICEID, string CREATEDBY)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES_DAL = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES_DAL.MoveTransaction(MCONTRACTID, CONTRACTID, ENDORSEMENTID, SOURCEID, INVOICEID, CREATEDBY);
        }
        #endregion

        #region CutoffTransaction
        public static void CutoffTransaction(int SOURCEID, int INVOICEID, DateTime CUTOFFDATE, string CREATEDBY)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES_DAL = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES_DAL.CutoffTransaction(SOURCEID, INVOICEID, CUTOFFDATE, CREATEDBY);
        }
        #endregion

        #region TaxInvoiceReport
        public static DataTable TaxInvoiceReport(Int64 INVOICEID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES_DAL = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES_DAL.TaxInvoiceReport(INVOICEID);
        }

        public static DataTable CignaInvoiceReport(Int64 INVOICEID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES_DAL = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES_DAL.CignaInvoiceReport(INVOICEID);
        }
        #endregion

        #region LoadAllPremiumInvoices
        public static List<PREMIUMINVOICES_DOL> LoadAllPremiumInvoices(Int64 COAID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES.LoadAllPremiumInvoices(COAID);
        }
        #endregion

        #region Load_By_McontractID
        public static List<PREMIUMINVOICES_DOL> Load_By_McontractID(int? MCONTRACTID, int? INVOICETYPE, DateTime? ISSUED_FROM, DateTime? ISSUED_TO, int? CONTRACTID, int? TRANSTYPEID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES.Load_By_McontractID(MCONTRACTID, INVOICETYPE, ISSUED_FROM, ISSUED_TO, CONTRACTID, TRANSTYPEID);
        }
        #endregion

        #region GenerateInvoices
        public static bool GenerateInvoices(string CREATED_BY, string INSTALLENTID, ref string ERR_MSG)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES.GenerateInvoices(CREATED_BY, INSTALLENTID,ref ERR_MSG);
            return true;
        }
        #endregion

        #region RemoveInvoices
        public static bool RemoveInvoices(Int64 PRMINVOICEID, string REASON,string CREATED_BY)
        {
            _ERROR_MESSAGE = string.Empty;
            string ERR_MSG = string.Empty;
            PREMIUMINVOICES_DAL oPREMIUMINVOICES = new PREMIUMINVOICES_DAL();
            oPREMIUMINVOICES.RemoveInvoices(PRMINVOICEID, REASON, CREATED_BY, ref ERR_MSG);

            if (string.IsNullOrEmpty(ERR_MSG))
            {
                _ERROR_MESSAGE = string.Empty;
                return true;
            }
            else
            {
                _ERROR_MESSAGE = SystemMessages.ResourceManager.GetString(ERR_MSG, Thread.CurrentThread.CurrentCulture);
                if (string.IsNullOrEmpty(_ERROR_MESSAGE))
                {
                    _ERROR_MESSAGE = ERR_MSG;
                }
                return false;
            }
        }
        #endregion

        #region MemberPremiumReport
        public static DataTable MemberPremiumReport(Int64 INVOICEID)
        {
            PREMIUMINVOICES_DAL oPREMIUMINVOICES_DAL = new PREMIUMINVOICES_DAL();
            return oPREMIUMINVOICES_DAL.MemberPremiumReport(INVOICEID);
        }
        #endregion
    }
}
