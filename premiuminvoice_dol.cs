using System;
using nTouchCoreLibrary.DOL.Utilities;

namespace nTouchCoreLibrary.DOL.Financial
{
    public class PREMIUMINVOICES_DOL : BASEOBJECTS
    {
        #region Variables

        private Int64 _PRMINVOICEID;
        private int _SPROVIDERID;
        private int _PAYERID;
        private string _PRMINVOICENBR;
        private int _INVOICETYPE;
        private string _DISCRIPTION;
        private DateTime _FROMDATE;
        private DateTime _TODATE;
        private DateTime _DUEDATE;
        private int _SOURCE;
        private int _SOURCENTITYID;
        private Int64? _SOURCECOAID;
        private int? _TARGET_TYPE;
        private int? _TARGETENTITYID;
        private Int64? _TARGETCOAID;
        private int _CURRENCYID;
        private Decimal _CVFACTOR;
        private Decimal _AMOUNT;
        private Decimal _ADJUSTMENT;
        private Decimal _TAX;
        private Decimal _FINALAMOUNT;
        private string _ADJUSTMENTNOTES;
        private string _INTERNALNOTES;
        private string _EXTERNALNOTES;
        private string _ATTACHMENT1;
        private string _ATTACHMENT2;
        private string _ATTACHMENT3;
        private string _VALIDATED_BY;
        private DateTime? _VALIDATION_DATE;
        private string _APPROVED_BY;
        private DateTime? _APPROVED_DATE;
        private int _STATUS;
        private string _PRODUCTLINESIDZ;
        private string _BRANCHIDZ;
        private string _TRANSACTIONTYPES;
        private int _ISDELETED;
        private string _COL2;
        private string _COL3;
        private string _COL4;
        private string _COL5;

        private string _PAYER_NAME;
        private string _INVOICETYPE_NAME;
        private string _STATUS_NAME;
        private string _PRODUCTLINE_NAME;
        private string _BRANCH_NAME;
        private string _TRANSACTIONS_NAME;
        private string _ENDO_COUNT;
        private string _CURRENCY_NAME;
        private string _AMOUNT_CURR;
        private string _ADJUSTMENT_CURR;
        private string _TAX_CURR;
        private string _FINALAMOUNT_CURR;
        private string _DUE_TPA_AMOUNT;
        private string _SOURCE_NAME;
        private string _SOURCENTITY_NAME;
        private string _SOURCECOA_NAME;
        private string _TARGET_NAME;
        private string _TARGETENTITY_NAME;
        private string _TARGETCOA_NAME;

        #endregion

        #region Properties

        #region PRMINVOICEID
        public Int64 PRMINVOICEID
        {
            get { return _PRMINVOICEID; }
            set { _PRMINVOICEID = value; }
        }
        #endregion

        #region SPROVIDERID
        public int SPROVIDERID
        {
            get { return _SPROVIDERID; }
            set { _SPROVIDERID = value; }
        }
        #endregion

        #region PAYERID
        public int PAYERID
        {
            get { return _PAYERID; }
            set { _PAYERID = value; }
        }
        #endregion

        #region PRMINVOICENBR
        public string PRMINVOICENBR
        {
            get { return _PRMINVOICENBR; }
            set { _PRMINVOICENBR = value; }
        }
        #endregion

        #region INVOICETYPE
        public int INVOICETYPE
        {
            get { return _INVOICETYPE; }
            set { _INVOICETYPE = value; }
        }
        #endregion

        #region DISCRIPTION
        public string DISCRIPTION
        {
            get { return _DISCRIPTION; }
            set { _DISCRIPTION = value; }
        }
        #endregion

        #region FROMDATE
        public DateTime FROMDATE
        {
            get { return _FROMDATE; }
            set { _FROMDATE = value; }
        }
        #endregion

        #region TODATE
        public DateTime TODATE
        {
            get { return _TODATE; }
            set { _TODATE = value; }
        }
        #endregion

        #region DUEDATE
        public DateTime DUEDATE
        {
            get { return _DUEDATE; }
            set { _DUEDATE = value; }
        }
        #endregion

        #region SOURCE
        public int SOURCE
        {
            get { return _SOURCE; }
            set { _SOURCE = value; }
        }
        #endregion

        #region SOURCENTITYID
        public int SOURCENTITYID
        {
            get { return _SOURCENTITYID; }
            set { _SOURCENTITYID = value; }
        }
        #endregion

        #region SOURCECOAID
        public Int64? SOURCECOAID
        {
            get { return _SOURCECOAID; }
            set { _SOURCECOAID = value; }
        }
        #endregion

        #region TARGET_TYPE
        public int? TARGET_TYPE
        {
            get { return _TARGET_TYPE; }
            set { _TARGET_TYPE = value; }
        }
        #endregion

        #region TARGETENTITYID
        public int? TARGETENTITYID
        {
            get { return _TARGETENTITYID; }
            set { _TARGETENTITYID = value; }
        }
        #endregion

        #region TARGETCOAID
        public Int64? TARGETCOAID
        {
            get { return _TARGETCOAID; }
            set { _TARGETCOAID = value; }
        }
        #endregion

        #region CURRENCYID
        public int CURRENCYID
        {
            get { return _CURRENCYID; }
            set { _CURRENCYID = value; }
        }
        #endregion

        #region CVFACTOR
        public Decimal CVFACTOR
        {
            get { return _CVFACTOR; }
            set { _CVFACTOR = value; }
        }
        #endregion

        #region AMOUNT
        public Decimal AMOUNT
        {
            get { return _AMOUNT; }
            set { _AMOUNT = value; }
        }
        #endregion

        #region ADJUSTMENT
        public Decimal ADJUSTMENT
        {
            get { return _ADJUSTMENT; }
            set { _ADJUSTMENT = value; }
        }
        #endregion

        #region TAX
        public Decimal TAX
        {
            get { return _TAX; }
            set { _TAX = value; }
        }
        #endregion

        #region FINALAMOUNT
        public Decimal FINALAMOUNT
        {
            get { return _FINALAMOUNT; }
            set { _FINALAMOUNT = value; }
        }
        #endregion

        #region ADJUSTMENTNOTES
        public string ADJUSTMENTNOTES
        {
            get { return _ADJUSTMENTNOTES; }
            set { _ADJUSTMENTNOTES = value; }
        }
        #endregion

        #region INTERNALNOTES
        public string INTERNALNOTES
        {
            get { return _INTERNALNOTES; }
            set { _INTERNALNOTES = value; }
        }
        #endregion

        #region EXTERNALNOTES
        public string EXTERNALNOTES
        {
            get { return _EXTERNALNOTES; }
            set { _EXTERNALNOTES = value; }
        }
        #endregion

        #region ATTACHMENT1
        public string ATTACHMENT1
        {
            get { return _ATTACHMENT1; }
            set { _ATTACHMENT1 = value; }
        }
        #endregion

        #region ATTACHMENT2
        public string ATTACHMENT2
        {
            get { return _ATTACHMENT2; }
            set { _ATTACHMENT2 = value; }
        }
        #endregion

        #region ATTACHMENT3
        public string ATTACHMENT3
        {
            get { return _ATTACHMENT3; }
            set { _ATTACHMENT3 = value; }
        }
        #endregion

        #region VALIDATED_BY
        public string VALIDATED_BY
        {
            get { return _VALIDATED_BY; }
            set { _VALIDATED_BY = value; }
        }
        #endregion

        #region VALIDATION_DATE
        public DateTime? VALIDATION_DATE
        {
            get { return _VALIDATION_DATE; }
            set { _VALIDATION_DATE = value; }
        }
        #endregion

        #region APPROVED_BY
        public string APPROVED_BY
        {
            get { return _APPROVED_BY; }
            set { _APPROVED_BY = value; }
        }
        #endregion

        #region APPROVED_DATE
        public DateTime? APPROVED_DATE
        {
            get { return _APPROVED_DATE; }
            set { _APPROVED_DATE = value; }
        }
        #endregion

        #region STATUS
        public int STATUS
        {
            get { return _STATUS; }
            set { _STATUS = value; }
        }
        #endregion

        #region PRODUCTLINESIDZ
        public string PRODUCTLINESIDZ
        {
            get { return _PRODUCTLINESIDZ; }
            set { _PRODUCTLINESIDZ = value; }
        }
        #endregion

        #region BRANCHIDZ
        public string BRANCHIDZ
        {
            get { return _BRANCHIDZ; }
            set { _BRANCHIDZ = value; }
        }
        #endregion

        #region TRANSACTIONTYPES
        public string TRANSACTIONTYPES
        {
            get { return _TRANSACTIONTYPES; }
            set { _TRANSACTIONTYPES = value; }
        }
        #endregion

        #region ISDELETED
        public int ISDELETED
        {
            get { return _ISDELETED; }
            set { _ISDELETED = value; }
        }
        #endregion

        #region COL2
        public string COL2
        {
            get { return _COL2; }
            set { _COL2 = value; }
        }
        #endregion

        #region COL3
        public string COL3
        {
            get { return _COL3; }
            set { _COL3 = value; }
        }
        #endregion

        #region COL4
        public string COL4
        {
            get { return _COL4; }
            set { _COL4 = value; }
        }
        #endregion

        #region COL5
        public string COL5
        {
            get { return _COL5; }
            set { _COL5 = value; }
        }
        #endregion

        #region PAYER_NAME
        public string PAYER_NAME
        {
            get { return _PAYER_NAME; }
            set { _PAYER_NAME = value; }
        }
        #endregion

        #region INVOICETYPE_NAME
        public string INVOICETYPE_NAME
        {
            get { return _INVOICETYPE_NAME; }
            set { _INVOICETYPE_NAME = value; }
        }
        #endregion

        #region STATUS_NAME
        public string STATUS_NAME
        {
            get { return _STATUS_NAME; }
            set { _STATUS_NAME = value; }
        }
        #endregion

        #region PRODUCTLINE_NAME
        public string PRODUCTLINE_NAME
        {
            get { return _PRODUCTLINE_NAME; }
            set { _PRODUCTLINE_NAME = value; }
        }
        #endregion

        #region BRANCH_NAME
        public string BRANCH_NAME
        {
            get { return _BRANCH_NAME; }
            set { _BRANCH_NAME = value; }
        }
        #endregion

        #region TRANSACTIONS_NAME
        public string TRANSACTIONS_NAME
        {
            get { return _TRANSACTIONS_NAME; }
            set { _TRANSACTIONS_NAME = value; }
        }
        #endregion

        #region ENDO_COUNT
        public string ENDO_COUNT
        {
            get { return _ENDO_COUNT; }
            set { _ENDO_COUNT = value; }
        }
        #endregion

        #region CURRENCY_NAME
        public string CURRENCY_NAME
        {
            get { return _CURRENCY_NAME; }
            set { _CURRENCY_NAME = value; }
        }
        #endregion

        #region AMOUNT_CURR
        public string AMOUNT_CURR
        {
            get { return _AMOUNT_CURR; }
            set { _AMOUNT_CURR = value; }
        }
        #endregion

        #region ADJUSTMENT_CURR
        public string ADJUSTMENT_CURR
        {
            get { return _ADJUSTMENT_CURR; }
            set { _ADJUSTMENT_CURR = value; }
        }
        #endregion

        #region TAX_CURR
        public string TAX_CURR
        {
            get { return _TAX_CURR; }
            set { _TAX_CURR = value; }
        }
        #endregion

        #region FINALAMOUNT_CURR
        public string FINALAMOUNT_CURR
        {
            get { return _FINALAMOUNT_CURR; }
            set { _FINALAMOUNT_CURR = value; }
        }
        #endregion

        #region DUE_TPA_AMOUNT
        public string DUE_TPA_AMOUNT
        {
            get { return _DUE_TPA_AMOUNT; }
            set { _DUE_TPA_AMOUNT = value; }
        }
        #endregion

        #region SOURCE_NAME
        public string SOURCE_NAME
        {
            get { return _SOURCE_NAME; }
            set { _SOURCE_NAME = value; }
        }
        #endregion

        #region SOURCENTITY_NAME
        public string SOURCENTITY_NAME
        {
            get { return _SOURCENTITY_NAME; }
            set { _SOURCENTITY_NAME = value; }
        }
        #endregion

        #region SOURCECOA_NAME
        public string SOURCECOA_NAME
        {
            get { return _SOURCECOA_NAME; }
            set { _SOURCECOA_NAME = value; }
        }
        #endregion

        #region TARGET_NAME
        public string TARGET_NAME
        {
            get { return _TARGET_NAME; }
            set { _TARGET_NAME = value; }
        }
        #endregion

        #region TARGETENTITY_NAME
        public string TARGETENTITY_NAME
        {
            get { return _TARGETENTITY_NAME; }
            set { _TARGETENTITY_NAME = value; }
        }
        #endregion

        #region TARGETCOA_NAME
        public string TARGETCOA_NAME
        {
            get { return _TARGETCOA_NAME; }
            set { _TARGETCOA_NAME = value; }
        }
        #endregion

        #endregion
    }
}
