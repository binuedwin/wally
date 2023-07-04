using System;
using System.Data;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using nTouchCoreLibrary.DAL.Utilities;
using nTouchCoreLibrary.DOL.Financial;

namespace nTouchCoreLibrary.DAL.Financial
{
    internal class PREMIUMINVOICES_DAL : BASEOBJECTS
    {
        #region LoadById
        public PREMIUMINVOICES_DOL LoadById(Int64 PRMINVOICEID)
        {
            PREMIUMINVOICES_DOL oTPREMIUMINVOICES = new PREMIUMINVOICES_DOL();
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_LOAD";

            _cmdCommand.Parameters.Add("P_PRMINVOICEID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICEID;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _dataReader = _cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                _conConnection.Close();
                if (_dataReader != null)
                    _dataReader.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }
            if (_dataReader.Read())
            {
                oTPREMIUMINVOICES.FillObject(_dataReader);
            }
            else
            {
                _dataReader.Close();
                return null;
            }
            _dataReader.Close();
            return oTPREMIUMINVOICES;
        }
        #endregion

        #region Load All
        public List<PREMIUMINVOICES_DOL> LoadAll(int? SOURCE, int? OWNERID, int? SOURCENTITYID, int? TARGETENTITYID, string INVOICETYPE, int SPROVIDERID, int? PAYERID, int? PRODUCTLINESIDZ, string PRMINVOICENBR, DateTime? FROMDUEDATE, DateTime? TODUEDATE, string STATUS,string POLICYNO,int? MCONTRACTID)
        {
            OBJECTLIST<PREMIUMINVOICES_DOL> colTPREMIUMINVOICES_DOL = new OBJECTLIST<PREMIUMINVOICES_DOL>();

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_LOAD_ALL";

            _cmdCommand.Parameters.Add("P_SOURCE", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCE.HasValue ? SOURCE.Value : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_OWNERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = OWNERID.HasValue ? OWNERID.Value : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_SOURCENTITYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCENTITYID.HasValue ? SOURCENTITYID.Value : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_TARGETENTITYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TARGETENTITYID.HasValue ? TARGETENTITYID.Value : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_INVOICETYPE", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICETYPE;

            _cmdCommand.Parameters.Add("P_SPROVIDERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SPROVIDERID;

            _cmdCommand.Parameters.Add("P_PAYERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PAYERID.HasValue ? PAYERID.Value : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_PRODUCTLINESIDZ", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRODUCTLINESIDZ.HasValue ? PRODUCTLINESIDZ.Value : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_INVOICENBR", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICENBR;

            _cmdCommand.Parameters.Add("P_FROMDUEDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = FROMDUEDATE;

            _cmdCommand.Parameters.Add("P_TODUEDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TODUEDATE;

            _cmdCommand.Parameters.Add("P_STATUS", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = STATUS;

            _cmdCommand.Parameters.Add("P_POLICYNO", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = POLICYNO;

            _cmdCommand.Parameters.Add("P_MCONTRACTID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = MCONTRACTID.HasValue ? MCONTRACTID.Value : (object)DBNull.Value; ;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _dataReader = _cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
                colTPREMIUMINVOICES_DOL.FillList(_dataReader);
                return colTPREMIUMINVOICES_DOL;
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                if (_dataReader != null)
                    _dataReader.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region Insert
        public void Insert(ref Int64 PRMINVOICEID, int SPROVIDERID, int PAYERID, string PRMINVOICENBR, int INVOICETYPE, string DISCRIPTION, DateTime FROMDATE, DateTime TODATE, DateTime DUEDATE, int SOURCE, int SOURCENTITYID, Int64? SOURCECOAID, int? TARGETENTITYID, Int64? TARGETCOAID, int CURRENCYID, Decimal CVFACTOR, Decimal AMOUNT, Decimal ADJUSTMENT, Decimal TAX, Decimal FINALAMOUNT, string ADJUSTMENTNOTES, string INTERNALNOTES, string EXTERNALNOTES, string ATTACHMENT1, string ATTACHMENT2, string ATTACHMENT3, string VALIDATED_BY, DateTime? VALIDATION_DATE, string APPROVED_BY, DateTime? APPROVED_DATE, int STATUS, string PRODUCTLINESIDZ, string BRANCHIDZ, string TRANSACTIONTYPES,int ISDELETED, string CREATED_BY)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_INSERT";

            _cmdCommand.Parameters.Add("P_PRMINVOICEID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            _cmdCommand.Parameters.Add("P_SPROVIDERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SPROVIDERID;

            _cmdCommand.Parameters.Add("P_PAYERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PAYERID;

            _cmdCommand.Parameters.Add("P_PRMINVOICENBR", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICENBR;

            _cmdCommand.Parameters.Add("P_INVOICETYPE", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICETYPE;

            _cmdCommand.Parameters.Add("P_DISCRIPTION", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = DISCRIPTION;

            _cmdCommand.Parameters.Add("P_FROMDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = FROMDATE;

            _cmdCommand.Parameters.Add("P_TODATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TODATE;

            _cmdCommand.Parameters.Add("P_DUEDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = DUEDATE;

            _cmdCommand.Parameters.Add("P_SOURCE", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCE;

            _cmdCommand.Parameters.Add("P_SOURCENTITYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCENTITYID;

            _cmdCommand.Parameters.Add("P_SOURCECOAID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (SOURCECOAID.HasValue ? (object)SOURCECOAID.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_TARGETENTITYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (TARGETENTITYID.HasValue ? (object)TARGETENTITYID.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_TARGETCOAID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (TARGETCOAID.HasValue ? (object)TARGETCOAID.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_CURRENCYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CURRENCYID;

            _cmdCommand.Parameters.Add("P_CVFACTOR", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CVFACTOR;

            _cmdCommand.Parameters.Add("P_AMOUNT", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = AMOUNT;

            _cmdCommand.Parameters.Add("P_ADJUSTMENT", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = ADJUSTMENT;

            _cmdCommand.Parameters.Add("P_TAX", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TAX;

            _cmdCommand.Parameters.Add("P_FINALAMOUNT", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = FINALAMOUNT;

            _cmdCommand.Parameters.Add("P_ADJUSTMENTNOTES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ADJUSTMENTNOTES) ? (object)ADJUSTMENTNOTES : string.Empty);

            _cmdCommand.Parameters.Add("P_INTERNALNOTES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(INTERNALNOTES) ? (object)INTERNALNOTES : string.Empty);

            _cmdCommand.Parameters.Add("P_EXTERNALNOTES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(EXTERNALNOTES) ? (object)EXTERNALNOTES : string.Empty);

            _cmdCommand.Parameters.Add("P_ATTACHMENT1", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ATTACHMENT1) ? (object)ATTACHMENT1 : string.Empty);

            _cmdCommand.Parameters.Add("P_ATTACHMENT2", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ATTACHMENT2) ? (object)ATTACHMENT2 : string.Empty);

            _cmdCommand.Parameters.Add("P_ATTACHMENT3", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ATTACHMENT3) ? (object)ATTACHMENT3 : string.Empty);

            _cmdCommand.Parameters.Add("P_VALIDATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(VALIDATED_BY) ? (object)VALIDATED_BY : string.Empty);

            _cmdCommand.Parameters.Add("P_VALIDATION_DATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (VALIDATION_DATE.HasValue ? (object)VALIDATION_DATE.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_APPROVED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(APPROVED_BY) ? (object)APPROVED_BY : string.Empty);

            _cmdCommand.Parameters.Add("P_APPROVED_DATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (APPROVED_DATE.HasValue ? (object)APPROVED_DATE.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_STATUS", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = STATUS;

            _cmdCommand.Parameters.Add("P_PRODUCTLINESIDZ", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(PRODUCTLINESIDZ) ? (object)PRODUCTLINESIDZ : string.Empty);

            _cmdCommand.Parameters.Add("P_BRANCHIDZ", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(BRANCHIDZ) ? (object)BRANCHIDZ : string.Empty);

            _cmdCommand.Parameters.Add("P_TRANSACTIONTYPES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(TRANSACTIONTYPES) ? (object)TRANSACTIONTYPES : string.Empty);

            _cmdCommand.Parameters.Add("P_ISDELETED", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = ISDELETED;

            _cmdCommand.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CREATED_BY;

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                _conConnection.Close();
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    _conConnection.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }

            if (_cmdCommand.Parameters["P_PRMINVOICEID"].Status != OracleParameterStatus.NullFetched)
            {
                PRMINVOICEID = Int64.Parse(_cmdCommand.Parameters["P_PRMINVOICEID"].Value.ToString());
            }
        }
        #endregion

        #region Update
        public void Update(Int64 PRMINVOICEID, int SPROVIDERID, int PAYERID, string PRMINVOICENBR, int INVOICETYPE, string DISCRIPTION, DateTime FROMDATE, DateTime TODATE, DateTime DUEDATE, int SOURCE, int SOURCENTITYID, Int64? SOURCECOAID, int? TARGETENTITYID, Int64? TARGETCOAID, int CURRENCYID, Decimal CVFACTOR, Decimal AMOUNT, Decimal ADJUSTMENT, Decimal TAX, Decimal FINALAMOUNT, string ADJUSTMENTNOTES, string INTERNALNOTES, string EXTERNALNOTES, string ATTACHMENT1, string ATTACHMENT2, string ATTACHMENT3, string VALIDATED_BY, DateTime? VALIDATION_DATE, string APPROVED_BY, DateTime? APPROVED_DATE, int STATUS, string PRODUCTLINESIDZ, string BRANCHIDZ, string TRANSACTIONTYPES,int ISDELETED, string MODIFIED_BY, DateTime? MODIFICATION_DATE)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_UPDATE";

            _cmdCommand.Parameters.Add("P_PRMINVOICEID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICEID;

            _cmdCommand.Parameters.Add("P_SPROVIDERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SPROVIDERID;

            _cmdCommand.Parameters.Add("P_PAYERID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PAYERID;

            _cmdCommand.Parameters.Add("P_PRMINVOICENBR", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICENBR;

            _cmdCommand.Parameters.Add("P_INVOICETYPE", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICETYPE;

            _cmdCommand.Parameters.Add("P_DISCRIPTION", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = DISCRIPTION;

            _cmdCommand.Parameters.Add("P_FROMDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = FROMDATE;

            _cmdCommand.Parameters.Add("P_TODATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TODATE;

            _cmdCommand.Parameters.Add("P_DUEDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = DUEDATE;

            _cmdCommand.Parameters.Add("P_SOURCE", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCE;

            _cmdCommand.Parameters.Add("P_SOURCENTITYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCENTITYID;

            _cmdCommand.Parameters.Add("P_SOURCECOAID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (SOURCECOAID.HasValue ? (object)SOURCECOAID.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_TARGETENTITYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (TARGETENTITYID.HasValue ? (object)TARGETENTITYID.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_TARGETCOAID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (TARGETCOAID.HasValue ? (object)TARGETCOAID.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_CURRENCYID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CURRENCYID;

            _cmdCommand.Parameters.Add("P_CVFACTOR", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CVFACTOR;

            _cmdCommand.Parameters.Add("P_AMOUNT", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = AMOUNT;

            _cmdCommand.Parameters.Add("P_ADJUSTMENT", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = ADJUSTMENT;

            _cmdCommand.Parameters.Add("P_TAX", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TAX;

            _cmdCommand.Parameters.Add("P_FINALAMOUNT", OracleDbType.Decimal);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = FINALAMOUNT;

            _cmdCommand.Parameters.Add("P_ADJUSTMENTNOTES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ADJUSTMENTNOTES) ? (object)ADJUSTMENTNOTES : string.Empty);

            _cmdCommand.Parameters.Add("P_INTERNALNOTES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(INTERNALNOTES) ? (object)INTERNALNOTES : string.Empty);

            _cmdCommand.Parameters.Add("P_EXTERNALNOTES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(EXTERNALNOTES) ? (object)EXTERNALNOTES : string.Empty);

            _cmdCommand.Parameters.Add("P_ATTACHMENT1", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ATTACHMENT1) ? (object)ATTACHMENT1 : string.Empty);

            _cmdCommand.Parameters.Add("P_ATTACHMENT2", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ATTACHMENT2) ? (object)ATTACHMENT2 : string.Empty);

            _cmdCommand.Parameters.Add("P_ATTACHMENT3", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(ATTACHMENT3) ? (object)ATTACHMENT3 : string.Empty);

            _cmdCommand.Parameters.Add("P_VALIDATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(VALIDATED_BY) ? (object)VALIDATED_BY : string.Empty);

            _cmdCommand.Parameters.Add("P_VALIDATION_DATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (VALIDATION_DATE.HasValue ? (object)VALIDATION_DATE.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_APPROVED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(APPROVED_BY) ? (object)APPROVED_BY : string.Empty);

            _cmdCommand.Parameters.Add("P_APPROVED_DATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (APPROVED_DATE.HasValue ? (object)APPROVED_DATE.Value : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_STATUS", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = STATUS;

            _cmdCommand.Parameters.Add("P_PRODUCTLINESIDZ", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(PRODUCTLINESIDZ) ? (object)PRODUCTLINESIDZ : string.Empty);

            _cmdCommand.Parameters.Add("P_BRANCHIDZ", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(BRANCHIDZ) ? (object)BRANCHIDZ : string.Empty);

            _cmdCommand.Parameters.Add("P_TRANSACTIONTYPES", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(TRANSACTIONTYPES) ? (object)TRANSACTIONTYPES : string.Empty);

            _cmdCommand.Parameters.Add("P_ISDELETED", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = ISDELETED;

            _cmdCommand.Parameters.Add("P_MODIFIED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (!string.IsNullOrEmpty(MODIFIED_BY) ? (object)MODIFIED_BY : string.Empty);

            _cmdCommand.Parameters.Add("P_MODIFICATION_DATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (MODIFICATION_DATE.HasValue ? (object)MODIFICATION_DATE.Value : (object)DBNull.Value);

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                _conConnection.Close();
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    _conConnection.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region Delete
        public void Delete(Int64 PRMINVOICEID, ref string ERR_MSG)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_DELETE";

            _cmdCommand.Parameters.Add("P_PRMINVOICEID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICEID;

            _cmdCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Size = 400;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                _conConnection.Close();
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    _conConnection.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }

            if (_cmdCommand.Parameters["P_ERR_MSG"].Status != OracleParameterStatus.NullFetched)
            {
                ERR_MSG = _cmdCommand.Parameters["P_ERR_MSG"].Value.ToString();
            }
        }
        #endregion

        #region CheckForExistingInvoiceNo
        public int CheckForExistingInvoiceNo(string INVOICENBR)
        {
            Int16 adminCount = 0;

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();

            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_INVEXISTS";

            _cmdCommand.Parameters.Add("P_INVOICENBR", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICENBR;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                adminCount = Convert.ToInt16(_cmdCommand.ExecuteScalar());
                Utilities_DAL.Close_Connection(_conConnection);
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                throw new Exception(ex.Message, ex.InnerException);
            }

            return (int)adminCount;
        }
        #endregion

        #region MoveTransaction
        public void MoveTransaction(int MCONTRACTID, int CONTRACTID, int ENDORSEMENTID, int SOURCEID, int INVOICEID, string CREATEDBY)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.DBP_MOVE_PREMIUM_TRANSACTION";

            _cmdCommand.Parameters.Add("P_MCONTRACTID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = MCONTRACTID;

            _cmdCommand.Parameters.Add("P_CONTRACTID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CONTRACTID;

            _cmdCommand.Parameters.Add("P_ENDORSEMENTID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = ENDORSEMENTID;

            _cmdCommand.Parameters.Add("P_SOURCEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCEID;

            _cmdCommand.Parameters.Add("P_INVOICEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICEID;

            _cmdCommand.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CREATEDBY;

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                Utilities_DAL.Close_Connection(_conConnection);
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    Utilities_DAL.Close_Connection(_conConnection);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region CutoffTransaction
        public void CutoffTransaction(int SOURCEID, int INVOICEID, DateTime CUTOFFDATE, string CREATEDBY)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.DBP_CUTOFF_PREMIUM_TRANSACTION";

            _cmdCommand.Parameters.Add("P_SOURCEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = SOURCEID;

            _cmdCommand.Parameters.Add("P_INVOICEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICEID;

            _cmdCommand.Parameters.Add("P_CUTOFFDATE", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CUTOFFDATE;

            _cmdCommand.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CREATEDBY;

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                Utilities_DAL.Close_Connection(_conConnection);
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    Utilities_DAL.Close_Connection(_conConnection);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region TaxInvoiceReport
        public DataTable TaxInvoiceReport(Int64 INVOICEID)
        {
            DataTable dtTaxInvoiceReport = new DataTable();

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _adpDataAdapter = new OracleDataAdapter();

            _conConnection.ConnectionString = _ConnectionString;

            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.DBP_PREMIUM_TAX_INVOICE";

            _cmdCommand.Parameters.Add("P_INVOICEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICEID;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _adpDataAdapter.SelectCommand = _cmdCommand;
                _adpDataAdapter.Fill(dtTaxInvoiceReport);
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return dtTaxInvoiceReport;
        }

        public DataTable CignaInvoiceReport(Int64 INVOICEID)
        {
            DataTable dtTaxInvoiceReport = new DataTable();

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _adpDataAdapter = new OracleDataAdapter();

            _conConnection.ConnectionString = _ConnectionString;

            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.DBP_PREMIUM_CIGNA_INVOICE";

            _cmdCommand.Parameters.Add("P_INVOICEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICEID;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _adpDataAdapter.SelectCommand = _cmdCommand;
                _adpDataAdapter.Fill(dtTaxInvoiceReport);
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return dtTaxInvoiceReport;
        }

        #endregion

        #region Load All Premium 
        public List<PREMIUMINVOICES_DOL> LoadAllPremiumInvoices(Int64 COAID)
        {
            OBJECTLIST<PREMIUMINVOICES_DOL> colTPREMIUMINVOICES_DOL = new OBJECTLIST<PREMIUMINVOICES_DOL>();

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_ACCOUNTS.TPREMINV_LOAD_INVOICES";

            _cmdCommand.Parameters.Add("P_COAID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = COAID;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _dataReader = _cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
                colTPREMIUMINVOICES_DOL.FillList(_dataReader);
                return colTPREMIUMINVOICES_DOL;
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                if (_dataReader != null)
                    _dataReader.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region Load_By_McontractID
        public List<PREMIUMINVOICES_DOL> Load_By_McontractID(int? MCONTRACTID, int? INVOICETYPE, DateTime? ISSUED_FROM, DateTime? ISSUED_TO, int? CONTRACTID, int? TRANSTYPEID)
        {
            OBJECTLIST<PREMIUMINVOICES_DOL> colTPREMIUMINVOICES_DOL = new OBJECTLIST<PREMIUMINVOICES_DOL>();

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMINV_LOAD_BY_MCONTRACTID";

            _cmdCommand.Parameters.Add("P_MCONTRACTID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (MCONTRACTID.HasValue ? MCONTRACTID : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_INVOICETYPE", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (INVOICETYPE.HasValue ? INVOICETYPE : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_ISSUED_FROM", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (ISSUED_FROM.HasValue ? ISSUED_FROM : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_ISSUED_TO", OracleDbType.Date);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = (ISSUED_TO.HasValue ? ISSUED_TO : (object)DBNull.Value);

            _cmdCommand.Parameters.Add("P_CONTRACTID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CONTRACTID.HasValue ? CONTRACTID : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_TRANSTYPEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = TRANSTYPEID.HasValue ? TRANSTYPEID : (object)DBNull.Value;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _dataReader = _cmdCommand.ExecuteReader(CommandBehavior.CloseConnection);
                colTPREMIUMINVOICES_DOL.FillList(_dataReader);
                return colTPREMIUMINVOICES_DOL;
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                if (_dataReader != null)
                    _dataReader.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region GenerateInvoices
        public void GenerateInvoices(string CREATED_BY,string INSTALLENTID,ref string ERR_MSG)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_INSTALLMENTS.DBP_PAYER_DUE_INVOICES";

            _cmdCommand.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CREATED_BY;

            _cmdCommand.Parameters.Add("P_INSTALLENTID", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INSTALLENTID;

            _cmdCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Size = 400;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                _conConnection.Close();
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    _conConnection.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }

            if (_cmdCommand.Parameters["P_ERR_MSG"].Status != OracleParameterStatus.NullFetched)
            {
                ERR_MSG = _cmdCommand.Parameters["P_ERR_MSG"].Value.ToString();
            }
        }
        #endregion

        #region RemoveInvoices
        public void RemoveInvoices(Int64 PRMINVOICEID,string REASON,string CREATED_BY, ref string ERR_MSG)
        {
            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _conConnection.ConnectionString = _ConnectionString;
            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.TPREMIUMINVOICES_REMOVE";

            _cmdCommand.Parameters.Add("P_PRMINVOICEID", OracleDbType.Int64);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = PRMINVOICEID;

            _cmdCommand.Parameters.Add("P_REASON", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = REASON;

            _cmdCommand.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = CREATED_BY;

            _cmdCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Size = 400;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _conConnection.Open();
                _cmdCommand.ExecuteNonQuery();
                _conConnection.Close();
            }
            catch (Exception ex)
            {
                if (_conConnection.State != ConnectionState.Closed)
                    _conConnection.Close();
                throw new Exception(ex.Message, ex.InnerException);
            }

            if (_cmdCommand.Parameters["P_ERR_MSG"].Status != OracleParameterStatus.NullFetched)
            {
                ERR_MSG = _cmdCommand.Parameters["P_ERR_MSG"].Value.ToString();
            }
        }
        #endregion

        #region MemberPremiumReport
        public DataTable MemberPremiumReport(Int64 INVOICEID)
        {
            DataTable dtTaxInvoiceReport = new DataTable();

            _conConnection = new OracleConnection();
            _cmdCommand = new OracleCommand();
            _adpDataAdapter = new OracleDataAdapter();

            _conConnection.ConnectionString = _ConnectionString;

            _cmdCommand.Connection = _conConnection;
            _cmdCommand.CommandType = CommandType.StoredProcedure;
            _cmdCommand.CommandText = "DBP_FIN_PRODUCTIONS.DBP_MEMBER_PREMIUM_INVOICES";

            _cmdCommand.Parameters.Add("P_INVOICEID", OracleDbType.Int32);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Input;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = INVOICEID;

            _cmdCommand.Parameters.Add("P_REF_CURSOR", OracleDbType.RefCursor);
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Direction = ParameterDirection.Output;
            _cmdCommand.Parameters[_cmdCommand.Parameters.Count - 1].Value = null;

            try
            {
                _adpDataAdapter.SelectCommand = _cmdCommand;
                _adpDataAdapter.Fill(dtTaxInvoiceReport);
            }
            catch (Exception ex)
            {
                Utilities_DAL.Close_Connection(_conConnection);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return dtTaxInvoiceReport;
        }

        #endregion
    }
}
