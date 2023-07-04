
using nTouchCoreLibrary.DOL.Contracts;
using nTouchCoreLibrary.BLL.Contracts;
using nTouchCoreLibrary.DOL.Settings;
using nTouchCoreLibrary.BLL.Settings;
using nTouchCoreLibrary.BLL.Utilities;
using nTouchCoreLibrary.DOL.Entities;
using nTouchCoreLibrary.BLL.Entities;
using nTouchCoreLibrary.DOL.Financial;
using nTouchCoreLibrary.DAL.Financial;
using nTouchCoreLibrary.BLL.Financial;
using nTouchCoreLibrary.BLL.SECURITY;
using nTouchCoreLibrary.DOL.SECURITY;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nTouchCoreWeb.UtilitiesWeb;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;

using nTouchCoreLibrary.BLL.Setup;
using nTouchCoreLibrary.DOL.Setup;
using nTouchCoreLibrary.DOL.PolicyRules;
using nTouchCoreLibrary.DOL.Utilities;

namespace nTouchCoreWeb.Financial.Payers
{
    public partial class PremiumInvoices : UtilitiesWeb.WebBase
    {
        #region ======================= Properties

        private int vSERVICEPROVIDERID
        {
            set
            {
                ViewState["vSERVICEPROVIDERID"] = value;
            }
            get
            {
                if (ViewState["vSERVICEPROVIDERID"] != null && !string.IsNullOrEmpty(ViewState["vSERVICEPROVIDERID"].ToString()))
                {
                    return Convert.ToInt32(ViewState["vSERVICEPROVIDERID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        private int vsINVOICEID
        {
            set
            {
                ViewState["vsINVOICEID"] = value;
            }
            get
            {
                if (ViewState["vsINVOICEID"] != null && !string.IsNullOrEmpty(ViewState["vsINVOICEID"].ToString()))
                {
                    return Convert.ToInt32(ViewState["vsINVOICEID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        private int vsINVOICETYPEID
        {
            set
            {
                ViewState["vsINVOICETYPEID"] = value;
            }
            get
            {
                if (ViewState["vsINVOICETYPEID"] != null && !string.IsNullOrEmpty(ViewState["vsINVOICETYPEID"].ToString()))
                {
                    return Convert.ToInt32(ViewState["vsINVOICETYPEID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        private int vsPAYERID
        {
            set
            {
                ViewState["vsPAYERID"] = value;
            }
            get
            {
                if (ViewState["vsPAYERID"] != null && !string.IsNullOrEmpty(ViewState["vsPAYERID"].ToString()))
                {
                    return Convert.ToInt32(ViewState["vsPAYERID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        private int? vsOWNERID
        {
            set
            {
                ViewState["vsOWNERID"] = value;
            }
            get
            {
                if (ViewState["vsOWNERID"] != null && !string.IsNullOrEmpty(ViewState["vsOWNERID"].ToString()))
                {
                    return Convert.ToInt32(ViewState["vsOWNERID"].ToString());
                }
                else
                {
                    return null;
                }
            }
        }

        private string[] SelectedKeys
        {
            set
            {
                ViewState["SelectedKeys"] = value;
            }
            get
            {
                if (ViewState["SelectedKeys"] != null)
                {
                    return (string[])ViewState["SelectedKeys"];
                }
                else
                {
                    return null;
                }
            }
        }

        private string afuFile1_Temp
        {
            set
            {
                Session["afuFile1_Temp"] = value;
            }
            get
            {
                if (Session["afuFile1_Temp"] != null)
                {
                    return Session["afuFile1_Temp"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string afuFile2_Temp
        {
            set
            {
                Session["afuFile2_Temp"] = value;
            }
            get
            {
                if (Session["afuFile2_Temp"] != null)
                {
                    return Session["afuFile2_Temp"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string afuFile3_Temp
        {
            set
            {
                Session["afuFile3_Temp"] = value;
            }
            get
            {
                if (Session["afuFile3_Temp"] != null)
                {
                    return Session["afuFile3_Temp"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private enum PageActionType
        {
            UpdateAccounts = 1,
            UpdateAttachments = 2,
            UpdateAdjustments = 3,
            Validate = 4,
            Approve = 5
        }

        #region PREMIUMTYPE

        private int PREMIUMTYPE
        {
            set
            {
                ViewState["PREMIUMTYPE"] = value;
            }
            get
            {
                if (ViewState["PREMIUMTYPE"] != null && !string.IsNullOrEmpty(ViewState["PREMIUMTYPE"].ToString()))
                {
                    return Convert.ToInt32(ViewState["PREMIUMTYPE"]);
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion

        #endregion

        #region ======================= Events

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionInfo.LoggedInUserID != null && SessionInfo.LoggedInUserID != 0)
            {
                ucConfirmDialog.DeleteButtonClicked += new EventHandler(ConfirmDialogue_DeleteButtonClicked);
                ucPremiumInvDelete.DeleteButtonClicked += new EventHandler(PremiumInvDelete_DeleteButtonClicked);
                ucRegenConfirmDialog.DeleteButtonClicked += new EventHandler(ConfirmDialogue_RegenButtonClicked);
                if (!Page.IsPostBack)
                {
                    InitialActions();
                }
                rbCurrentMonth.InputAttributes.Add("class", "ace");
                rbLastMonth.InputAttributes.Add("class", "ace");

                //cbValidated.InputAttributes.Add("class", "ace");
                //cbSubmitted.InputAttributes.Add("class", "ace");
            }
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "InitPrmInvActions", "InitPrmInvActions();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "LoadScroll", "LoadScroll();", true);
        }

        #endregion

        #region Search Invoices

        protected void btnSearchAccountTransactions_ServerClick(object sender, EventArgs e)
        {
            Load_EndoInvoices();
        }

        protected void autoPayerSearch_TextChanged(object sender, EventArgs e)
        {
            //autoAccountSearch.Clear();

            //autoAccountSearch.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_PAYERACCOUNTS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_PAYERID," + -999 + ";P_PAYERBRANCHID," + null;

            //if (!string.IsNullOrEmpty(autoPayerSearch.Value))
            //{
            //    autoAccountSearch.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_PAYERACCOUNTS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_PAYERID," + autoPayerSearch.Value + ";P_PAYERBRANCHID," + null;
            //    autoAccountSearch.txtControl.Focus();
            //}
            //else
            //{
            //    autoPayerSearch.txtControl.Focus();
            //}
        }

        protected void autoServiceProv_TextChanged(object sender, EventArgs e)
        {
            //autoPayerSearch.Clear();
            //autoAccountSearch.Clear();
            //autoPayerSearch.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_PAYERS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + SessionInfo.ServiceProviderID;

            //if (!string.IsNullOrEmpty(autoServiceProv.Value))
            //{
            //    autoPayerSearch.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_PAYERS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + autoServiceProv.Value;
            //    autoPayerSearch.txtControl.Focus();
            //}
            //else
            //{
            //    autoServiceProv.txtControl.Focus();
            //}
        }

        protected void gvAccountTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAccountTransactions.PageIndex = e.NewPageIndex;
            Load_EndoInvoices();
        }

        protected void gvAccountTransactions_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvAccountTransactions.SortExp = e.SortExpression;
            Load_EndoInvoices();
        }

        protected void autoSourceEntityType_TextChanged(object sender, EventArgs e)
        {
            autoSourceEntitySearch.Clear();
            int nEntityType = 0;
            if (!string.IsNullOrEmpty(autoSourceEntityType.Value))
            {
                if (autoSourceEntityType.Value == "1")
                    nEntityType = 3;
                else
                    nEntityType = 2;
                autoSourceEntitySearch.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE," + nEntityType + ";P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + autoServiceProv.Value;
                autoSourceEntitySearch.txtControl.Focus();
            }
            upAccountTransactions.Update();
        }

        protected void autoTargetEntityType_TextChanged(object sender, EventArgs e)
        {
            autoTaretEntitySearch.Clear();
            if (!string.IsNullOrEmpty(autoTargetEntityType.Value))
            {
                autoTaretEntitySearch.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE," + autoTargetEntityType.Value + ";P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + autoServiceProv.Value;
                autoTaretEntitySearch.txtControl.Focus();
            }
            upAccountTransactions.Update();
        }

        //protected void autoTargetEntityTypeEdit_TextChanged(object sender, EventArgs e)
        //{
        //    autoTargetEntity.Clear();
        //    autoTargetCOAAccount.Clear();
        //    if (!string.IsNullOrEmpty(autoTargetEntityTypeEdit.Value))
        //    {
        //        autoTargetEntity.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE," + autoTargetEntityTypeEdit.Value + ";P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + autoServiceProv.Value;
        //        autoTargetEntity.txtControl.Focus();
        //    }
        //}

        #endregion

        #region Generate Invoices

        protected void btnGenerateInvoices_ServerClick(object sender, EventArgs e)
        {
            InitialValuesForGenerateInvoice();
            mpeGenerateInvoices.Show();
            upGenerateInvoices.Update();
        }

        protected void btnSaveGenerateInvoices_ServerClick(object sender, EventArgs e)
        {
            int nSproviderID = Convert.ToInt32(autoServiceProv.Value);
            if (!string.IsNullOrEmpty(txtMonth.Value) && !string.IsNullOrEmpty(txtYear.Text))
            {
                int monthInDigit = DateTime.ParseExact(txtMonth.Value, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;

                // PREMIUMINVOICES.GenerateInvoices(null, Convert.ToInt32(txtYear.Text), monthInDigit, null, nSproviderID, null, null, SessionInfo.LoggedInUser);

                if (string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                {
                    Alert(WebMessages.MsgGenerateInvoices, AlertType.Success);
                    Load_EndoInvoices();
                    mpeGenerateInvoices.Hide();
                    upGenerateInvoices.Update();
                }
                else
                {
                    Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
                    mpeGenerateInvoices.Show();
                    upGenerateInvoices.Update();
                }
            }
        }

        protected void btnClosePopUPGenerateAccounts_ServerClick(object sender, EventArgs e)
        {
            mpeGenerateInvoices.Hide();
            upGenerateInvoices.Update();
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            txtYear.Text = string.Empty;
            txtMonth.Value = string.Empty;

            if (rbCurrentMonth.Checked == true)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Value = DateTime.Now.ToString("MMMM");
            }
            else if (rbLastMonth.Checked == true)
            {
                txtYear.Text = DateTime.Now.AddMonths(-1).ToString("yyyy");
                txtMonth.Value = DateTime.Now.AddMonths(-1).ToString("MMMM");
            }

            mpeGenerateInvoices.Show();
            upGenerateInvoices.Update();
        }

        #endregion

        #region View Invoice

        protected void btnViewInvoiceDetails_ServerClick(object sender, EventArgs e)
        {
            int nPremiumInvoiceID = 0;
            int nInvoiceTypeID = 0;
            HtmlButton btnViewInvoiceDetails = (HtmlButton)sender;
            GridViewRow gvSelectedRow = (GridViewRow)((btnViewInvoiceDetails).Parent.Parent);
            if (!string.IsNullOrEmpty(btnViewInvoiceDetails.Attributes["invoicetypeid"].ToString())) { nInvoiceTypeID = Convert.ToInt32(btnViewInvoiceDetails.Attributes["invoicetypeid"].ToString()); }
            if (!string.IsNullOrEmpty(btnViewInvoiceDetails.Attributes["invoiceid"].ToString())) { nPremiumInvoiceID = Convert.ToInt32(btnViewInvoiceDetails.Attributes["invoiceid"].ToString()); }
            if (!string.IsNullOrEmpty(btnViewInvoiceDetails.Attributes["payerid"].ToString())) { vsPAYERID = Convert.ToInt32(btnViewInvoiceDetails.Attributes["payerid"].ToString()); }

            LoadHeaderDataForInvoiceDetails(nPremiumInvoiceID, nInvoiceTypeID);

            vsINVOICEID = nPremiumInvoiceID;
            vsINVOICETYPEID = nInvoiceTypeID;
            gvPremiumInvoiceView.SelectedRowIndex = gvSelectedRow.RowIndex;
            updPremiumInvDetailsView.Update();
            LoadEndoresmentsView(nPremiumInvoiceID, vsINVOICETYPEID);
        }

        protected void btnBackToMain_ServerClick(object sender, EventArgs e)
        {
            gvPremiumInvoiceView.DataSource = null;
            gvPremiumInvoiceView.DataBind();

            divPremiumInvSearch.Visible = true;
            divPremiumSearchHeader.Visible = true;

            divPremiumInvViewDetails.Visible = false;
            divPremiumInvViewSearchDetails.Visible = false;

            GridTitle.InnerText = "Invoices";
            btnBackToMain.Visible = false;
            dvSearchHeader.Visible = true;

            spnDetailsInvoiceSearch.Visible = true;
            spEndoSearch.Visible = false;

            dvInvDetailsAction.Visible = false;
            if (vsOWNERID.HasValue)
                btnGeneratePrmInvoices.Visible = true;
            updPremiumInvDetailsView.Update();
            upAccountTransactions.Update();
        }

        protected void gvPremiumInvoiceView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPremiumInvoiceView.PageIndex = e.NewPageIndex;
            LoadEndoresmentsView(vsINVOICEID, vsINVOICETYPEID);
        }

        protected void gvPremiumInvoiceView_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvPremiumInvoiceView.SortExp = e.SortExpression;
            LoadEndoresmentsView(vsINVOICEID, vsINVOICETYPEID);
        }

        protected void btnInvoiceDetailsSearch_ServerClick(object sender, EventArgs e)
        {
            LoadEndoInvDetails(vsINVOICEID, vsINVOICETYPEID);
            upAccountTransactions.Update();
            updPremiumInvDetailsView.Update();
        }

        #endregion

        #region Cutt Off

        protected void btnPayerCutoff_ServerClick(object sender, EventArgs e)
        {
            mpeNewPayerAction.Show();
            upPayerAction.Update();
        }

        #endregion

        #region Regenerate Invoice

        protected void btnRegenInvoice_ServerClick(object sender, EventArgs e)
        {
            HtmlButton btnRegenInvoice = (HtmlButton)sender;
            GridViewRow gvSelectedRow = (GridViewRow)((btnRegenInvoice).Parent.Parent);
            int nPremiumInvoiceID = Convert.ToInt32(btnRegenInvoice.Attributes["invoiceid"].ToString());
            int nInvoiceTypeId = Convert.ToInt32(btnRegenInvoice.Attributes["invoicetypeid"].ToString());

            vsINVOICEID = nPremiumInvoiceID;
            vsINVOICETYPEID = nInvoiceTypeId;
            gvAccountTransactions.SelectedRowIndex = gvSelectedRow.RowIndex;
            upAccountTransactions.Update();

            ucRegenConfirmDialog.showDialogBox(WebMessages.lblRegenInvoice, WebMessages.lblRegenInvoiceConfirm);
            ucRegenConfirmDialog.ConfirmationMessage = WebMessages.MsgProceedConfirmation;
            ucRegenConfirmDialog.hideDeleteMessage();
        }

        private void ConfirmDialogue_RegenButtonClicked(object sender, EventArgs e)
        {
            int nSproviderID = Convert.ToInt32(autoServiceProv.Value);
            //PREMIUMINVOICES.RegenerateInvoice(vsINVOICEID, vsINVOICETYPEID, nSproviderID, SessionInfo.LoggedInUser);

            if (string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
            {
                Alert(WebMessages.MsgRegenInvoiceSuccessfully, AlertType.Success);
                ucRegenConfirmDialog.hideDialogBox();
                Load_EndoInvoices();
            }
            else
            {
                Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
            }
        }

        #endregion

        #region Edit Invoice

        protected void btnEditInvoice_ServerClick(object sender, EventArgs e)
        {
            HtmlButton btnEditInvoice = (HtmlButton)sender;
            GridViewRow gvSelectedRow = (GridViewRow)((btnEditInvoice).Parent.Parent);
            vsINVOICEID = Convert.ToInt32(btnEditInvoice.Attributes["invoiceid"].ToString());
            gvAccountTransactions.SelectedRowIndex = gvSelectedRow.RowIndex;
            upAccountTransactions.Update();
            EditMode(vsINVOICEID);
        }

        protected void btnCloseNewInvoice_ServerClick(object sender, EventArgs e)
        {
            mpeNewInvoice.Hide();
            upNewInvoice.Update();
        }

        protected void afuFile1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string strFilePath = EssentialsBLL.sFolderPath(null, null, UtilitiesWeb.SessionInfo.LoggedInUser) + "\\" + DateTime.Now.ToString("mmss") + afuFile1.FileName;
            if (!string.IsNullOrEmpty(afuFile1_Temp))
            {
                File.Delete(afuFile1_Temp);
            }
            afuFile1.SaveAs(strFilePath);
            afuFile1_Temp = strFilePath;
        }

        protected void afuFile2_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string strFilePath = EssentialsBLL.sFolderPath(null, null, UtilitiesWeb.SessionInfo.LoggedInUser) + "\\" + DateTime.Now.ToString("mmss") + afuFile2.FileName;
            if (!string.IsNullOrEmpty(afuFile2_Temp))
            {
                File.Delete(afuFile2_Temp);
            }
            afuFile2.SaveAs(strFilePath);
            afuFile2_Temp = strFilePath;
        }

        protected void afuFile3_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string strFilePath = EssentialsBLL.sFolderPath(null, null, UtilitiesWeb.SessionInfo.LoggedInUser) + "\\" + DateTime.Now.ToString("mmss") + afuFile3.FileName;
            if (!string.IsNullOrEmpty(afuFile3_Temp))
            {
                File.Delete(afuFile3_Temp);
            }
            afuFile3.SaveAs(strFilePath);
            afuFile3_Temp = strFilePath;
        }

        protected void btnSaveInvoice_ServerClick(object sender, EventArgs e)
        {
            SaveInvoice(PageActionType.UpdateAttachments);
        }

        protected void btnValidateInvoice_ServerClick(object sender, EventArgs e)
        {
            SaveInvoice(PageActionType.Validate);
        }

        protected void btnApproveInvoice_ServerClick(object sender, EventArgs e)
        {
            SaveInvoice(PageActionType.Approve);
        }

        protected void btnDownloadInvoiceFile_ServerClick(object sender, EventArgs e)
        {
            int nPremiumInvoiceID = Convert.ToInt32(hdnInvoiceID.Value);

            HtmlButton btnDownloadInvoice = (HtmlButton)sender;
            string sFile = btnDownloadInvoice.Attributes["file"].ToString();

            string sPath = EssentialsBLL.sFolderPath(SystemEnums.ObjectType.Invoice, nPremiumInvoiceID, null);

            ShowFileDownLoader(sPath + "\\" + sFile, true);
        }


        protected void autoSource_TextChanged(object sender, EventArgs e)
        {
            int? AutoSource = null;
            autoSourceEntity.Clear();
            autoSourceEntity.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE,0;P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + autoServiceProv.Value;
            if (!string.IsNullOrEmpty(autoSource.Value))
            {
                if (Convert.ToInt32(autoSource.Value) == SystemEnums.AgreementParty.ServiceProvider.GetHashCode())
                {
                    AutoSource = 3;
                }
                else
                {
                    AutoSource = Convert.ToInt32(autoSource.Value);
                }
                autoSourceEntity.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE," + AutoSource + ";P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + autoServiceProv.Value;
                autoSourceEntity.txtControl.Focus();
            }
            upAccounts.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + plnNewInvoice.ClientID + "','div.widget-body');", true);
        }

        protected void autoSourceEntity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(autoSourceEntity.Value))
                FillSourceAccountAutoList();
            autoSourceCOAAccount.txtControl.Focus();
            upAccounts.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + plnNewInvoice.ClientID + "','div.widget-body');", true);
        }

        protected void autoTargetEntity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(autoTargetEntity.Value))
                FillTargetAccountAutoList();
            autoTargetCOAAccount.txtControl.Focus();
            upAccounts.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + plnNewInvoice.ClientID + "','div.widget-body');", true);
        }

        protected void btnSaveAccounts_ServerClick(object sender, EventArgs e)
        {
            SaveInvoice(PageActionType.UpdateAccounts);
        }

        protected void btnSaveAdjustments_ServerClick(object sender, EventArgs e)
        {
            SaveInvoice(PageActionType.UpdateAdjustments);
        }
        #endregion

        #region Mmeber Premium
        protected void btnGenMemberPremium_ServerClick(object sender, EventArgs e)
        {
            HtmlButton htmlButton = (HtmlButton)sender;
            GridViewRow gvSelectedRow = (GridViewRow)((htmlButton).Parent.Parent);
            gvAccountTransactions.SelectedRowIndex = gvSelectedRow.RowIndex;
            upAccountTransactions.Update();
            
            GridViewRow row = (GridViewRow)htmlButton.NamingContainer;

            int nPremiumInvID = Convert.ToInt32(gvAccountTransactions.DataKeys[row.RowIndex].Values[0].ToString());
            string sParamValues = string.Empty;
            string sParamText = string.Empty;

            PREMIUMINVOICES_DOL oPremiumInvoices = new PREMIUMINVOICES_DOL();
            oPremiumInvoices = PREMIUMINVOICES.LoadById(nPremiumInvID);

            //sParamValues = "JET.DBP_FIN_REPORTS.DBP_CIGNA_MEMBER_INVOICE;P_USER_ID," + SessionInfo.LoggedInUserID + ";P_LANG_ID," + SessionInfo.LANGID + ";P_INVOICEID," + oPremiumInvoices.PRMINVOICEID;
            //sParamText = "JET.DBP_FIN_REPORTS.DBP_CIGNA_MEMBER_INVOICE;Invoice Number," + oPremiumInvoices.PRMINVOICENBR;

            //AddReport(sParamValues, sParamText);

            //DataTable dtMemberPremInvoices = PREMIUMINVOICES.MemberPremiumReport(nPremiumInvID);
            /*
            try
            {
                if (dtMemberPremInvoices.Rows.Count > 0)
                {
                    using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                    {
                        wb.Worksheets.Add(dtMemberPremInvoices, "Member Premium Invoices");
                        string strFilePath = EssentialsBLL.sFolderPath(null, null, UtilitiesWeb.SessionInfo.LoggedInUser) + "\\" + Guid.NewGuid().ToString() + ".xlsx";
                        wb.SaveAs(strFilePath);
                        ShowFileDownLoader(strFilePath, true);
                    }
                }
                else
                    Alert(WebMessages.MsgNoDataFound, AlertType.Warrning);
            }
            catch { }
            */
            string sReportName = "MEMBER_PREMIUM_INVOICES";
            string sParams = "P_INVOICEID," + nPremiumInvID;
            string sColumns = string.Empty;
            string sFilename = "Member Premium Invoices";
            BuildExcelReport(sReportName, sParams, sColumns, sFilename);
        }
        #endregion

        #region Delete Invoice
        protected void btnDelPremiumInvoices_ServerClick(object sender, EventArgs e)
        {
            HtmlButton btnDelPremiumInvoices = (HtmlButton)sender;

            GridViewRow gvSelectedRow = (GridViewRow)((btnDelPremiumInvoices).Parent.Parent);
            gvAccountTransactions.SelectedRowIndex = gvSelectedRow.RowIndex;
            upAccountTransactions.Update();

            GridViewRow row = (GridViewRow)btnDelPremiumInvoices.NamingContainer;
            vsINVOICEID = Convert.ToInt32(gvAccountTransactions.DataKeys[row.RowIndex].Values[0].ToString());
            ucPremiumInvDelete.showDialogBox(WebMessages.MsgDeleteTitle, WebMessages.MsgDeleteConfirmation);
            ucPremiumInvDelete.hideDeleteMessage();
        }
        #endregion

        private void PremiumInvDelete_DeleteButtonClicked(object sender, EventArgs e)
        {
            InvoiceDeletion();
        }

        #region MoveTransaction

            protected void btnMoveTransaction_ServerClick(object sender, EventArgs e)
        {
            string[] cbSelected = gvPremiumInvoiceView.SelectedKeys;
            SelectedKeys = gvPremiumInvoiceView.SelectedKeys;

            if (cbSelected != null)
            {
                ucConfirmDialog.showDialogBox(WebMessages.lblMoveTransactions, WebMessages.lblMoveTransactionsConfirm);
                ucConfirmDialog.ConfirmationMessage = WebMessages.MsgProceedConfirmation;
                ucConfirmDialog.hideDeleteMessage();
            }
            else
            {
                Alert(WebMessages.MsgRecordSelect, AlertType.Error);
            }
        }

        private void ConfirmDialogue_DeleteButtonClicked(object sender, EventArgs e)
        {
            PREMIUMINVOICES_DOL oInvoice = PREMIUMINVOICES.LoadById(vsINVOICEID);

            int nMContractid = 0;
            int nContractID = 0;
            int nEndorsementID = 0;

            string[] cbSelected = SelectedKeys;

            foreach (string cbSelect in cbSelected)
            {
                string[] saSelect = cbSelect.Split(';');
                nMContractid = Convert.ToInt32(saSelect[0]);
                nContractID = Convert.ToInt32(saSelect[1]);
                nEndorsementID = Convert.ToInt32(saSelect[2]);

                PREMIUMINVOICES.MoveTransaction(nMContractid, nContractID, nEndorsementID, oInvoice.SOURCE, vsINVOICEID, SessionInfo.LoggedInUser);
            }

            if (string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
            {
                Alert(WebMessages.MsgMoveTransactions, AlertType.Success);
                ucConfirmDialog.hideDialogBox();
                LoadHeaderDataForInvoiceDetails(vsINVOICEID, vsINVOICETYPEID);
                LoadEndoresmentsView(vsINVOICEID, vsINVOICETYPEID);
                FillInvoicesGrid();
            }
            else
            {
                Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
            }
        }

        #endregion

        #region Cutoff Transaction

        protected void btnCloseNewPayerAction_ServerClick(object sender, EventArgs e)
        {
            mpeNewPayerAction.Hide();
            upPayerAction.Update();
        }

        protected void btnSaveNewPayerAction_ServerClick(object sender, EventArgs e)
        {
            PREMIUMINVOICES_DOL oInvoice = PREMIUMINVOICES.LoadById(vsINVOICEID);
            PAYERS_DOL OPAYER = PAYERS.LoadById(oInvoice.PAYERID, SystemEnums.Language.English.GetHashCode());

            PAYERCUTOFF_DOL oCutoff = new PAYERCUTOFF_DOL
            {
                PAYERID = OPAYER.PAYERID,
                //ACCOUNTID = oInvoice.ACCOUNTID,
                ACTION_TYPE = Convert.ToInt32(SystemEnums.ActionType.Cutoff.GetHashCode()),
                ACTION_DATE = dpActionDate.CalendarDate.Value,
                DESCRIPTION = txtDescription.Text,
                VALIDATED = 0,
                CREATED_BY = SessionInfo.LoggedInUser
            };
            PAYERCUTOFF.Insert(oCutoff);

            PREMIUMINVOICES.CutoffTransaction(oInvoice.SOURCE, vsINVOICEID, dpActionDate.CalendarDate.Value, SessionInfo.LoggedInUser);


            if (string.IsNullOrEmpty(PAYERCUTOFF.ERROR_MESSAGE))
            {
                Alert(WebMessages.MsgRecordInserted, AlertType.Success);
                LoadEndoresmentsView(vsINVOICEID, vsINVOICETYPEID);
                mpeNewPayerAction.Hide();
            }
            else
            {
                Alert(PAYERCUTOFF.ERROR_MESSAGE, AlertType.Error);
                mpeNewPayerAction.Show();
            }
            FillInvoicesGrid();
            upPayerAction.Update();
        }

        #endregion

        #region Download Grid Contents

        #region Download Premium Invoices Grid

        protected void btnDownloadFile_ServerClick(object sender, EventArgs e)
        {
            if (gvAccountTransactions.Rows.Count > 0)
            {
                int nSproviderID = Convert.ToInt32(autoServiceProv.Value);
                string nInvoiceType = autoInvoiceType.Value;
                int? nPayerID = null;
                int? nPayerAccount = null;
                int? nSource = EssentialsBLL.StringToNullable<int>(autoSource.Value);
                int? nSourceEntity = EssentialsBLL.StringToNullable<int>(autoSourceEntitySearch.Value);
                int? nTargetEntity = EssentialsBLL.StringToNullable<int>(autoTaretEntitySearch.Value);
                int? nMContractID = EssentialsBLL.StringToNullable<int>(autoMcontractSearch.Value);
                DateTime? dtFromEffDate = dpFromEffecctiveDateSearch.CalendarDate;
                DateTime? dtToEffDate = dpToEffecctiveDateSearch.CalendarDate;
                string nStatus = autoInvoiceStatus.Value;
                
                string sReportName = "PREMINVOICES_LIST";
                string sParams = "P_SOURCE," + nSource + ";P_OWNERID," + vsOWNERID + ";P_SOURCENTITYID," + nSourceEntity + ";P_TARGETENTITYID," + nTargetEntity + ";P_INVOICETYPE," + nInvoiceType + ";P_SPROVIDERID," + nSproviderID + ";P_PAYERID," + nPayerID + ";P_PRODUCTLINESIDZ," + nPayerAccount + ";P_INVOICENBR," + txtInvNumber.Value.Trim() + ";";
                sParams += "P_FROMDUEDATE," + UtilitiesBLL.FormatDate(dtFromEffDate, "dd-MMM-yyyy") + ";P_TODUEDATE," + UtilitiesBLL.FormatDate(dtToEffDate, "dd-MMM-yyyy") + ";P_STATUS," + nStatus + ";P_POLICYNO," + txtPolicyNumber.Value.Trim() + ";P_MCONTRACTID," + nMContractID;
                string sColumns = BuildExcelColumns(gvAccountTransactionsTemp);
                string sFilename = "Premium Invoices List";
                BuildExcelReport(sReportName, sParams, sColumns, sFilename);
            }
        }

        #endregion

        #region Download Invoice Details Grid

        protected void btnDownloadEndorsements_ServerClick(object sender, EventArgs e)
        {
            PREMIUMINVOICES_DOL oPremium = PREMIUMINVOICES.LoadById(vsINVOICEID);
            int? nMasterContractID = EssentialsBLL.StringToNullable<int>(autoMasterContractInvDetails.Value);
            int sProviderId = 0;
            if (!string.IsNullOrEmpty(autoServiceProv.Value))
            {
                sProviderId = Convert.ToInt32(autoServiceProv.Value);
            }
            else
            {
                sProviderId = Convert.ToInt32(SessionInfo.ServiceProviderID);
            }
            vSERVICEPROVIDERID = sProviderId;
            string sEndoType = string.Empty;
            if (!string.IsNullOrEmpty(multiEndoTypes.Value))
                sEndoType = multiEndoTypes.Value;

            if (gvPremiumInvoiceView.Rows.Count > 0)
            {
                string sReportName = "ENDOINSTALLMENTS_LIST";
                string sParams = "P_MCONTRACTID," + nMasterContractID + ";P_PREMIUMTYPE,;P_ISSUED_FROM,;P_ISSUED_TO,;P_CONTRACTID,;P_TRANSTYPEID,;P_ISINVOICED,;P_PRMINVOICEID," + oPremium.PRMINVOICEID + ";P_ENDOTYPE," + sEndoType;
                string sColumns = BuildExcelColumns(gvPremiumInvoiceView);
                string sFilename = "Premium Invoices";
                BuildExcelReport(sReportName, sParams, sColumns, sFilename);
            }
        }

        #endregion

        protected void btnDownloadTaxInv_ServerClick(object sender, EventArgs e)
        {
            HtmlButton btnEditInvoice = (HtmlButton)sender;
            GridViewRow gvSelectedRow = (GridViewRow)((btnEditInvoice).Parent.Parent);

            vsINVOICEID = Convert.ToInt32(btnEditInvoice.Attributes["invoiceid"].ToString());
            PREMIUMINVOICES_DOL oInv = PREMIUMINVOICES.LoadById(vsINVOICEID);
            gvAccountTransactions.SelectedRowIndex = gvSelectedRow.RowIndex;
            upAccountTransactions.Update();

            PAYERS_DOL oPayer = PAYERS.LoadById(625, 1);
            DataTable dtReportData = new DataTable();

            string sCRpt_Name = string.Empty;
            string sReportName = string.Empty;
            string sParams = "P_INVOICEID," + vsINVOICEID;
            string sFilename = string.Empty;

            if (oInv.SOURCENTITYID == oPayer.ENTITYID)
            {
                sReportName = "PREMIUM_CIGNA_INVOICE";
                sFilename = "Premium_Invoice_Tax_CIGNA" + vsINVOICEID;
                sCRpt_Name = "crpt_PremiumInvoices_Cigna.rpt";
            }
            else
            {
                sReportName = "PREMIUM_TAX_INVOICE";
                sFilename = "Premium_Invoice_Tax_" + vsINVOICEID;
                sCRpt_Name = "crpt_PremiumInvoices.rpt";
            }
            BuildPDFReport(sCRpt_Name, sReportName, sParams, sFilename);

        }
        #endregion

        #region Invoice Generation
        protected void autoPrmInvPayer_TextChanged(object sender, EventArgs e)
        {
            autoPrmInvMasterContract.Clear();
            autoPrmInvContract.Clear();

            autoPrmInvMasterContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID,0;P_PAYERID,0;P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";
            autoPrmInvContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_CONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_MCONTRACTID,0;P_VALIDATE,1";

            if (!string.IsNullOrEmpty(autoPrmInvPayer.Value))
            {
                int nPayerId = Convert.ToInt32(autoPrmInvPayer.Value);

                if ((SystemEnums.EntityTypes)SessionInfo.EntityType == SystemEnums.EntityTypes.PolicyHolder)
                    autoPrmInvMasterContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT_PHOLDER;P_LANG_ID," + SessionInfo.LANGID + ";P_PAYERID," + nPayerId + ";P_ENTITYID," + SessionInfo.UserEntityID;
                else
                    autoPrmInvMasterContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + null + ";P_PAYERID," + nPayerId + ";P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";

                autoPrmInvMasterContract.txtControl.Focus();
            }
            else
            {
                autoPrmInvPayer.txtControl.Focus();
            }

            mpePrmInvInstallments.Show();
            upPrmInvInstallments.Update();
        }
        protected void autoPrmInvMasterContract_TextChanged(object sender, EventArgs e)
        {
            dpPolicyStartDate.ClearDate();
            dpPolicyEndDate.ClearDate();
            autoPrmInvContract.Clear();
            autoPrmInvContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_CONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_MCONTRACTID,0;P_VALIDATE,1";

            if (!string.IsNullOrEmpty(autoPrmInvMasterContract.Value))
            {
                if ((SystemEnums.EntityTypes)SessionInfo.EntityType == SystemEnums.EntityTypes.PolicyHolder)
                    autoPrmInvContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_CONTRACT_PHOLDER;P_LANG_ID," + SessionInfo.LANGID + ";P_MCONTRACTID," + autoPrmInvMasterContract.Value + ";P_ENTITYID," + SessionInfo.UserEntityID;
                else
                    autoPrmInvContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_CONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_MCONTRACTID," + autoPrmInvMasterContract.Value + ";P_VALIDATE,1";
                autoPrmInvContract.txtControl.Focus();
                MASTERCONTRACTS_DOL oMasterContracts = MASTERCONTRACTS.LoadById(Convert.ToInt32(autoPrmInvMasterContract.Value));
                if (oMasterContracts != null)
                {
                    dpPolicyStartDate.CalendarDate = oMasterContracts.EFFDATE;
                    dpPolicyEndDate.CalendarDate = oMasterContracts.EXPDATE;
                }
            }
            else
            {
                autoPrmInvMasterContract.txtControl.Focus();
            }

            mpePrmInvInstallments.Show();
            upPrmInvInstallments.Update();
        }

        protected void btnGeneratePrmInvoices_ServerClick(object sender, EventArgs e)
        {
            ShowPremiumInvInstallemnts();
        }

        protected void btnPrmInvHideInstallments_ServerClick(object sender, EventArgs e)
        {
            mpePrmInvInstallments.Hide();
            upPrmInvInstallments.Update();
        }
        protected void btnSearchPrmInvoices_ServerClick(object sender, EventArgs e)
        {
            PREMIUMTYPE = SystemEnums.PayerFeeType.Premium.GetHashCode();
            ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "colorfulDivsSP", "colorfulDivs('colorfulSPDiv','colorfulSubDiv');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + pnlPrmInvInstallments.ClientID + "','div.widget-body');", true);
            FillInstallments();
            mpePrmInvInstallments.Show();
            upPrmInvInstallments.Update();
        }

        protected void btnDownloadPrmInvInstallments_ServerClick(object sender, EventArgs e)
        {
            PREMIUMTYPE = SystemEnums.PayerFeeType.Premium.GetHashCode();
            //DataTable dTable = new DataTable();
            int? nPayerID = EssentialsBLL.StringToNullable<int>(autoPrmInvPayer.Value);
            int? nMasterContractID = EssentialsBLL.StringToNullable<int>(autoPrmInvMasterContract.Value);
            int? nContractID = EssentialsBLL.StringToNullable<int>(autoPrmInvContract.Value);
            int? nTransactionType = EssentialsBLL.StringToNullable<int>(autoPrmInvTransactionType.Value);
            int? nInvoiced = EssentialsBLL.StringToNullable<int>(autoIsInvoiced.Value);
            DateTime? dtFromIssueDate = dpPrmInvFromIssueDate.CalendarDate;
            DateTime? dtToIssueDate = dpPrmInvToIssueDate.CalendarDate;

            string sReportName = "ENDOINSTALLMENTS_LIST";
            string sParams = "P_MCONTRACTID," + nMasterContractID.Value + ";P_PREMIUMTYPE," + PREMIUMTYPE + ";P_ISSUED_FROM," + UtilitiesBLL.FormatDate(dtFromIssueDate, "dd-MMM-yyyy") + ";P_ISSUED_TO," + UtilitiesBLL.FormatDate(dtToIssueDate, "dd-MMM-yyyy") + ";P_CONTRACTID," + nContractID + ";P_TRANSTYPEID," + nTransactionType + ";P_ISINVOICED," + nInvoiced + ";P_PRMINVOICEID,;P_ENDOTYPE,";
            string sColumns = BuildExcelColumns(gvPrmInvInstallments);
            string sFilename = "Installment List";
            BuildExcelReport(sReportName, sParams, sColumns, sFilename);

            ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "colorfulDivsSP", "colorfulDivs('colorfulSPDiv','colorfulSubDiv');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + pnlPrmInvInstallments.ClientID + "','div.widget-body');", true);

            mpePrmInvInstallments.Show();
            upPrmInvInstallments.Update();
        }

        protected void btnSavePrmInvTxns_ServerClick(object sender, EventArgs e)
        {
            bool bSelected = false;
            long nPrmInvID = 0;
            string sErrorMessage = string.Empty;
            foreach (GridViewRow row in gvPrmInvInstallments.Rows)
            {
                HtmlInputCheckBox cbPrmInv = (HtmlInputCheckBox)row.FindControl("cbPrmInv");
                if (cbPrmInv.Checked && cbPrmInv.Visible)
                {
                    bSelected = true;
                    break;
                }
            }
            if (bSelected)
            {
                string sInstallemntIds = string.Empty;
                List<string> lstInstallmentIDs = new List<string>();
                foreach (GridViewRow row in gvPrmInvInstallments.Rows)
                {
                    HtmlInputCheckBox cbPrmInv = (HtmlInputCheckBox)row.FindControl("cbPrmInv");
                    if (cbPrmInv.Checked && cbPrmInv.Visible)
                    {
                        nPrmInvID = 0;
                        nPrmInvID = Convert.ToInt64(gvPrmInvInstallments.DataKeys[row.RowIndex].Values[0].ToString());
                        lstInstallmentIDs.Add(nPrmInvID.ToString());
                    }
                }
                sInstallemntIds = string.Join(",", lstInstallmentIDs.ToArray());
                PREMIUMINVOICES.GenerateInvoices(SessionInfo.LoggedInUser, sInstallemntIds,ref sErrorMessage);
                if (!string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE) || !string.IsNullOrEmpty(sErrorMessage))
                {
                    Alert(PREMIUMINVOICES.ERROR_MESSAGE + " " + sErrorMessage, AlertType.Error);
                }
                else
                {
                    Alert("Invoices generated successfully", AlertType.Success);
                    Load_EndoInvoices();
                }
                mpePrmInvInstallments.Hide();
                upPrmInvInstallments.Update();
            }
            else
            {
                Alert("No Invoices selected", AlertType.Error);
                mpePrmInvInstallments.Show();
                upPrmInvInstallments.Update();
            }
        }

        protected void gvPrmInvInstallments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrmInvInstallments.PageIndex = e.NewPageIndex;
            FillInstallments();
            ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "colorfulDivsSP", "colorfulDivs('colorfulSPDiv','colorfulSubDiv');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + pnlPrmInvInstallments.ClientID + "','div.widget-body');", true);
        }

        protected void gvPrmInvInstallments_Sorting(object sender, GridViewSortEventArgs e)
        {
            gvPrmInvInstallments.SortExp = e.SortExpression;
            FillInstallments();
            ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "colorfulDivsSP", "colorfulDivs('colorfulSPDiv','colorfulSubDiv');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + pnlPrmInvInstallments.ClientID + "','div.widget-body');", true);
        }

        protected void gvPrmInvInstallments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow drPremiumInvoices = ((DataRowView)e.Row.DataItem).Row;
                Int64? nSPAYERINVID = null;
                if (!string.IsNullOrEmpty(drPremiumInvoices["SPAYERINVID"].ToString()))
                    nSPAYERINVID = Convert.ToInt64(drPremiumInvoices["SPAYERINVID"]);
                HtmlInputCheckBox hcbPrmInv = (HtmlInputCheckBox)e.Row.FindControl("cbPrmInv");
                if (nSPAYERINVID.HasValue)
                    hcbPrmInv.Visible = false;
                else
                    hcbPrmInv.Visible = true;
            }
        }

        protected void btnPrmInvPremiumType_ServerClick(object sender, EventArgs e)
        {
            HtmlControl btn = (HtmlControl)sender;
            PREMIUMTYPE = Convert.ToInt32(btn.Attributes["refid"].ToString());
            FillInstallments();
            ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "colorfulDivsSP", "colorfulDivs('colorfulSPDiv','colorfulSubDiv');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + pnlPrmInvInstallments.ClientID + "','div.widget-body');", true);
        }
        #endregion

        #region Invoice Deletion
        protected void btnCloseDelPremiumInvoicePopUp_ServerClick(object sender, EventArgs e)
        {
            mpeDelPremiumInvoice.Hide();
            upDelPremiumInvoice.Update();
        }

        protected void btnDeleteInvoice_ServerClick(object sender, EventArgs e)
        {
            if (vsINVOICEID != 0)
            {
                PREMIUMINVOICES.RemoveInvoices(vsINVOICEID,txtDeletionNotes.Text,SessionInfo.LoggedInUser);
                if (string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                {
                    Alert(WebMessages.MsgRecordDeleted, AlertType.Success);
                    Load_EndoInvoices();
                }
                else
                    Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
            }

            mpeDelPremiumInvoice.Hide();
            upDelPremiumInvoice.Update();
        }

        #endregion

        #endregion

        #region ======================= Methods

        #region Initial AutoFill
        private void InitialActions()
        {
            btnGeneratePrmInvoices.Visible = false;
            dvPolicyNumber.Visible = false;
            dvMcontract.Visible = false;
            int? nPayerID = null;
            SetDefaultServiceProvider();
            vsOWNERID = COA.Load_Owner_ID(Convert.ToInt32(SessionInfo.LoggedInUserID));

            autoServiceProv.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_SPROVIDERS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID;
            autoInvoiceType.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,838;P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoSourceEntityType.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,581;P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoSourceEntitySearch.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE,0;P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoTargetEntityType.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,104;P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoTaretEntitySearch.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE,0;P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoInvoiceStatus.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,830;P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoInvoiceStatus.Value = SystemEnums.PremiumInvoiceStatus.Initial.GetHashCode() + "," + SystemEnums.PremiumInvoiceStatus.Approved.GetHashCode() + "," + SystemEnums.PremiumInvoiceStatus.Paid.GetHashCode() + "," + SystemEnums.PremiumInvoiceStatus.PartiallyPaid.GetHashCode() + "," + SystemEnums.PremiumInvoiceStatus.Validated.GetHashCode() + "," + SystemEnums.PremiumInvoiceStatus.Reversed.GetHashCode();
            autoMasterContractInvDetails.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + autoServiceProv.Value + ";P_PAYERID,0" + ";P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";
            multiEndoTypes.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,52;P_SPROVIDERID," + SessionInfo.CurrSProviderID;

            autoSource.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,581;P_SPROVIDERID," + SessionInfo.CurrSProviderID;

            autoSourceEntity.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE,0;P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoTargetEntity.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE,0;P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            FillSourceAccountAutoList();
            FillTargetAccountAutoList();

            DateTime now = DateTime.Now;
            dpFromEffecctiveDateSearch.CalendarDate = new DateTime(now.Year, now.Month, 1).AddDays(-7);
            //dpToEffecctiveDateSearch.CalendarDate = new DateTime(now.Year, now.Month, 1).AddDays(7);

            if (!string.IsNullOrEmpty(autoServiceProv.Value))
            {
                vSERVICEPROVIDERID = Convert.ToInt32(autoServiceProv.Value);
            }
            autoPrmInvPayer.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_PAYERS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoPrmInvMasterContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID,0;P_PAYERID,0;P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";
            autoPrmInvContract.SpName = "DBP_CONTRACT.DBP_AUTO_GET_CONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_MCONTRACTID,0;P_VALIDATE,1";
            autoPrmInvTransactionType.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,599;P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            autoIsInvoiced.SpName = "DBP_COMMON.DBP_AUTO_GET_REFVALUES;P_LANG_ID," + SessionInfo.LANGID + ";P_CLASS_ID,157;P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            if (vsOWNERID.HasValue)
            {
                btnGeneratePrmInvoices.Visible = true;
                dvPolicyNumber.Visible = true;
                dvMcontract.Visible = true;
                PAYERS_DOL oPayerDOL = PAYERS.LoadByEntityId(Convert.ToInt32(vsOWNERID));
                if (oPayerDOL != null)
                {
                    nPayerID = oPayerDOL.PAYERID; 
                }

                autoMcontractSearch.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID,"+ autoServiceProv.Value + ";P_PAYERID,"+ nPayerID + ";P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";
            }
            divPremiumInvViewDetails.Visible = false;
        }
        #endregion

        #region Load_EndoInvoices

        private void Load_EndoInvoices()
        {
            FillInvoicesGrid();

            btnBackToMain.Visible = false;
            divPremiumInvViewDetails.Visible = false;
            dvInvDetailsAction.Visible = false;

            upAccountTransactions.Update();
        }

        private void FillInvoicesGrid()
        {
            int nSproviderID = Convert.ToInt32(autoServiceProv.Value);
            string nInvoiceType = autoInvoiceType.Value;
            int? nPayerID = null;
            int? nPayerAccount = null;
            int? nSource = EssentialsBLL.StringToNullable<int>(autoSource.Value);
            int? nSourceEntity = EssentialsBLL.StringToNullable<int>(autoSourceEntitySearch.Value);
            int? nTargetEntity = EssentialsBLL.StringToNullable<int>(autoTaretEntitySearch.Value);
            int? nMContractID = EssentialsBLL.StringToNullable<int>(autoMcontractSearch.Value);
            DateTime? dtFromEffDate = dpFromEffecctiveDateSearch.CalendarDate;
            DateTime? dtToEffDate = dpToEffecctiveDateSearch.CalendarDate;
            vSERVICEPROVIDERID = nSproviderID;
            string nStatus = autoInvoiceStatus.Value;
            DataView dvPreInvoices = new DataView(EssentialsBLL.ToDataTable<PREMIUMINVOICES_DOL>(PREMIUMINVOICES.LoadAll(nSource, vsOWNERID, nSourceEntity, nTargetEntity, nInvoiceType, nSproviderID, nPayerID, nPayerAccount, txtInvNumber.Value.Trim(), dtFromEffDate, dtToEffDate, nStatus, txtPolicyNumber.Value.Trim(), nMContractID)));

            dvPreInvoices.Sort = gvAccountTransactions.SortExp;
            gvAccountTransactions.DataSource = dvPreInvoices;
            gvAccountTransactions.DataBind();
        }

        #endregion

        #region InitialValuesForGenerateInvoice

        private void InitialValuesForGenerateInvoice()
        {
            txtYear.Text = string.Empty;
            txtMonth.Value = string.Empty;
            rbCurrentMonth.Checked = false;
            rbLastMonth.Checked = true;
            txtYear.Text = DateTime.Now.AddMonths(-1).ToString("yyyy");
            txtMonth.Value = DateTime.Now.AddMonths(-1).ToString("MMMM");
        }

        #endregion

        #region View Invoice Deatils

        private void LoadEndoresmentsView(int nPremiumInvoiceID, int nInvoiceTypeID)
        {
            btnGeneratePrmInvoices.Visible = false;
            string sFromInvDate = string.Empty;
            string sToInvDate = string.Empty;

            btnPayerCutoff.Disabled = true;
            btnMoveTransaction.Disabled = true;
            btnPayerCutoff.Attributes["class"] = ValidationButtonEnableDisableStyle(true);
            btnMoveTransaction.Attributes["class"] = ValidationButtonEnableDisableStyle(true);

            LoadEndoInvDetails(nPremiumInvoiceID, nInvoiceTypeID);
            PREMIUMINVOICES_DOL oPremium = PREMIUMINVOICES.LoadById(nPremiumInvoiceID);

            divPremiumInvSearch.Visible = false;
            divPremiumSearchHeader.Visible = false;

            divPremiumInvViewDetails.Visible = true;
            divPremiumInvViewSearchDetails.Visible = true;
            btnBackToMain.Visible = true;
            dvSearchHeader.Visible = false;

            dvInvDetailsAction.Visible = true;

            GridTitle.InnerText = "Invoice Details";
            spEndoSearch.Visible = true;

            txtViewToInvDate.Value = oPremium.FROMDATE.ToString("dd-MM-yyyy");
            txtViewFromInvDate.Value = oPremium.TODATE.ToString("dd-MM-yyyy");

            spnDetailsInvoiceSearch.Visible = false;

            if (oPremium.STATUS == SystemEnums.PremiumInvoiceStatus.Initial.GetHashCode())
            {
                btnPayerCutoff.Attributes["class"] = "btn btn-danger";
                btnMoveTransaction.Attributes["class"] = "btn btn-success";
                btnPayerCutoff.Disabled = false;
                btnMoveTransaction.Disabled = false;
            }

            upAccountTransactions.Update();
            updPremiumInvDetailsView.Update();
        }

        private void LoadEndoInvDetails(int nPremiumInvoiceID, int nInvoiceTypeID)
        {
            gvPremiumInvoiceView.DataSource = null;
            gvPremiumInvoiceView.DataBind();

            PREMIUMINVOICES_DOL oPremium = PREMIUMINVOICES.LoadById(nPremiumInvoiceID);
            int? nMasterContractID = EssentialsBLL.StringToNullable<int>(autoMasterContractInvDetails.Value);
            int sProviderId = 0;
            if (!string.IsNullOrEmpty(autoServiceProv.Value))
            {
                sProviderId = Convert.ToInt32(autoServiceProv.Value);
            }
            else
            {
                sProviderId = Convert.ToInt32(SessionInfo.ServiceProviderID);
            }
            vSERVICEPROVIDERID = sProviderId;
            string sEndoType = string.Empty;
            if (!string.IsNullOrEmpty(multiEndoTypes.Value))
                sEndoType = multiEndoTypes.Value;
            DataView dvInstallments = new DataView(EssentialsBLL.ToDataTable<INSTALLMENTS_DOL>(INSTALLMENTS.Load_All(nMasterContractID, null, null, null, null, null, null,oPremium.PRMINVOICEID, sEndoType)));
            if (dvInstallments.Count > 0)
            {
                dvInstallments.Sort = gvPremiumInvoiceView.SortExp;
                gvPremiumInvoiceView.DataSource = dvInstallments;
                gvPremiumInvoiceView.DataBind();
            }
            updPremiumInvDetailsView.Update();
        }

        private void LoadHeaderDataForInvoiceDetails(int nPremiumInvoiceID, int nInvoiceTypeID)
        {
            PREMIUMINVOICES_DOL oPremInvoices = PREMIUMINVOICES.LoadById(nPremiumInvoiceID);

            txtViewPayerName.Value = oPremInvoices.PAYER_NAME;
            txtViewInvNbr.Value = oPremInvoices.PRMINVOICENBR;
            txtViewAccountName.Value = oPremInvoices.PRODUCTLINE_NAME;
            txtViewBranchNames.Value = oPremInvoices.BRANCH_NAME;
            txtViewTxnTypes.Value = oPremInvoices.TRANSACTIONS_NAME;
            dvInvTypeAdmin.Visible = false;

            dvInvTypeAdmin.Visible = true;
            txtAmount.Value = oPremInvoices.AMOUNT_CURR;
            txtAdjustment.Value = oPremInvoices.ADJUSTMENT_CURR;
            txtTax.Value = oPremInvoices.TAX_CURR;
            txtFinalAmount.Value = oPremInvoices.FINALAMOUNT_CURR;
            txtStatus.Value = oPremInvoices.STATUS_NAME;

            //Fill Mastercontract
            autoMasterContractInvDetails.Value = string.Empty;
            autoMasterContractInvDetails.Text = string.Empty;
            multiEndoTypes.Value = string.Empty;

            autoMasterContractInvDetails.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + vSERVICEPROVIDERID + ";P_PAYERID,0" + ";P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";

            if (vsPAYERID != 0)
            {
                PAYERS_DOL oPayer = PAYERS.LoadById(vsPAYERID, SystemEnums.Language.English.GetHashCode());
                if (oPayer != null)
                {
                    autoMasterContractInvDetails.SpName = "DBP_CONTRACT.DBP_AUTO_GET_MCONTRACT;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + vSERVICEPROVIDERID + ";P_PAYERID," + oPayer.PAYERID + ";P_PAYERBRANCHID," + null + ";P_ACCOUNTID," + null + ";P_VALIDATE,1";
                    autoMasterContractInvDetails.txtControl.Focus();
                }
            }
            else
            {
                autoMasterContractInvDetails.txtControl.Focus();
            }

        }

        #endregion

        #region Edit Invoice

        #region EditMode

        private void EditMode(int nPremiumInvoiceID)
        {
            ActionsBasedOnUserType(nPremiumInvoiceID);
            hdnInvoiceID.Value = nPremiumInvoiceID.ToString();


            PREMIUMINVOICES_DOL oPREMIUMINVOICES = PREMIUMINVOICES.LoadById(nPremiumInvoiceID);

            txtInvoiceNoForEdit.Text = oPREMIUMINVOICES.PRMINVOICENBR;
            txtInternalNotes.Text = oPREMIUMINVOICES.INTERNALNOTES;
            txtExternalNotes.Text = oPREMIUMINVOICES.EXTERNALNOTES;

            autoSource.Value = oPREMIUMINVOICES.SOURCE.ToString();
            autoSource.Text = oPREMIUMINVOICES.SOURCE_NAME;
            autoSource_TextChanged(null, null);

            autoSourceEntity.Value = oPREMIUMINVOICES.SOURCENTITYID.ToString();
            autoSourceEntity.Text = oPREMIUMINVOICES.SOURCENTITY_NAME;
            autoSourceEntity_TextChanged(null, null);

            autoSourceCOAAccount.Value = oPREMIUMINVOICES.SOURCECOAID.ToString();
            autoSourceCOAAccount.Text = oPREMIUMINVOICES.SOURCECOA_NAME;

            autoTargetEntity.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_ENTITIES;P_LANG_ID," + SessionInfo.LANGID + ";P_ENTITY_TYPE," + oPREMIUMINVOICES.TARGET_TYPE + ";P_OWNERID," + vsOWNERID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;


            autoTargetEntity.Value = oPREMIUMINVOICES.TARGETENTITYID.ToString();
            autoTargetEntity.Text = oPREMIUMINVOICES.TARGETENTITY_NAME;
            autoTargetEntity_TextChanged(null, null);

            autoTargetCOAAccount.Value = oPREMIUMINVOICES.TARGETCOAID.ToString();
            autoTargetCOAAccount.Text = oPREMIUMINVOICES.TARGETCOA_NAME;

            afuFile1_Temp = oPREMIUMINVOICES.ATTACHMENT1;
            afuFile2_Temp = oPREMIUMINVOICES.ATTACHMENT2;
            afuFile3_Temp = oPREMIUMINVOICES.ATTACHMENT3;

            if (oPREMIUMINVOICES.ADJUSTMENT != 0)
            {
                nbAdjustments.Text = oPREMIUMINVOICES.ADJUSTMENT.ToString();
                txtAdjustmentsNotes.Text = oPREMIUMINVOICES.ADJUSTMENTNOTES;
            }

            txtUserName.Value = !string.IsNullOrEmpty(oPREMIUMINVOICES.VALIDATED_BY) ? oPREMIUMINVOICES.VALIDATED_BY : SessionInfo.LoggedInUser;
            txtUserName1.Value = !string.IsNullOrEmpty(oPREMIUMINVOICES.APPROVED_BY) ? oPREMIUMINVOICES.APPROVED_BY : SessionInfo.LoggedInUser;
            txtCurrentDate.Value = oPREMIUMINVOICES.VALIDATION_DATE.HasValue ? oPREMIUMINVOICES.VALIDATION_DATE.Value.ToString("dd-MMM-yyyy") : DateTime.Now.ToString("dd-MMM-yyyy");
            txtCurrentDate1.Value = oPREMIUMINVOICES.APPROVED_DATE.HasValue ? oPREMIUMINVOICES.APPROVED_DATE.Value.ToString("dd-MMM-yyyy") : DateTime.Now.ToString("dd-MMM-yyyy");

            CheckFiles(oPREMIUMINVOICES.ATTACHMENT1, oPREMIUMINVOICES.ATTACHMENT2, oPREMIUMINVOICES.ATTACHMENT3);

            mpeNewInvoice.Show();
            upNewInvoice.Update();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DivDraggable", "DivDraggable('" + plnNewInvoice.ClientID + "','div.widget-body');", true);
        }

        #endregion

        #region SaveInvoice
        private void SaveInvoice(PageActionType enumAction)
        {
            PREMIUMINVOICES_DOL oPREMIUMINVOICES = PREMIUMINVOICES.LoadById(Convert.ToInt32(hdnInvoiceID.Value));
            int nCountInv = 0;
            int nSProviderId = Convert.ToInt32(autoServiceProv.Value);
            oPREMIUMINVOICES.SPROVIDERID = nSProviderId;
            switch ((PageActionType)enumAction)
            {
                case PageActionType.UpdateAccounts:
                    UpdateAccounts(oPREMIUMINVOICES);
                    break;
                case PageActionType.UpdateAttachments:
                    if (txtInvoiceNoForEdit.Text.Trim() != oPREMIUMINVOICES.PRMINVOICENBR)
                    {
                        nCountInv = PREMIUMINVOICES.CheckForExistingInvoiceNo(txtInvoiceNoForEdit.Text.Trim());
                    }
                    if (nCountInv == 0)
                    {
                        oPREMIUMINVOICES.PRMINVOICENBR = txtInvoiceNoForEdit.Text.Trim();
                        oPREMIUMINVOICES.INTERNALNOTES = txtInternalNotes.Text;
                        oPREMIUMINVOICES.EXTERNALNOTES = txtExternalNotes.Text;

                        if (!string.IsNullOrEmpty(afuFile1_Temp))
                        {
                            oPREMIUMINVOICES.ATTACHMENT1 = Path.GetFileName(new FileInfo(afuFile1_Temp).Name);
                        }
                        if (!string.IsNullOrEmpty(afuFile2_Temp))
                        {
                            oPREMIUMINVOICES.ATTACHMENT2 = Path.GetFileName(new FileInfo(afuFile2_Temp).Name);
                        }
                        if (!string.IsNullOrEmpty(afuFile3_Temp))
                        {
                            oPREMIUMINVOICES.ATTACHMENT3 = Path.GetFileName(new FileInfo(afuFile3_Temp).Name);
                        }
                        oPREMIUMINVOICES.MODIFIED_BY = SessionInfo.LoggedInUser;
                        oPREMIUMINVOICES.MODIFICATION_DATE = DateTime.Now;
                        oPREMIUMINVOICES.STATUS = SystemEnums.PremiumInvoiceStatus.Initial.GetHashCode();
                        PREMIUMINVOICES.Update(oPREMIUMINVOICES);
                        if (!string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                        {
                            Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
                        }
                        else
                        {
                            string sPath = EssentialsBLL.sFolderPath(SystemEnums.ObjectType.Invoice, oPREMIUMINVOICES.PRMINVOICEID, null);
                            EssentialsBLL.CreateFolder(sPath);
                            if (!string.IsNullOrEmpty(afuFile1_Temp))
                            {
                                if (File.Exists(afuFile1_Temp))
                                {
                                    File.Move(afuFile1_Temp, sPath + "\\" + oPREMIUMINVOICES.ATTACHMENT1);
                                }
                            }
                            if (!string.IsNullOrEmpty(afuFile2_Temp))
                            {
                                if (File.Exists(afuFile2_Temp))
                                {
                                    File.Move(afuFile2_Temp, sPath + "\\" + oPREMIUMINVOICES.ATTACHMENT2);
                                }
                            }
                            if (!string.IsNullOrEmpty(afuFile3_Temp))
                            {
                                if (File.Exists(afuFile3_Temp))
                                {
                                    File.Move(afuFile3_Temp, sPath + "\\" + oPREMIUMINVOICES.ATTACHMENT3);
                                }

                            }
                            Alert(WebMessages.MsgRecordUpdated, AlertType.Success);
                        }
                    }
                    else
                    {
                        Alert(WebMessages.MsgInvAlreadyExists, AlertType.Error);
                        mpeNewInvoice.Show();
                        upNewInvoice.Update();
                    }
                    break;
                case PageActionType.UpdateAdjustments:
                    if (txtInvoiceNoForEdit.Text.Trim() != oPREMIUMINVOICES.PRMINVOICENBR)
                    {
                        nCountInv = PREMIUMINVOICES.CheckForExistingInvoiceNo(txtInvoiceNoForEdit.Text.Trim());
                    }
                    if (nCountInv == 0)
                    {
                        oPREMIUMINVOICES.PRMINVOICENBR = txtInvoiceNoForEdit.Text.Trim();
                        oPREMIUMINVOICES.INTERNALNOTES = txtInternalNotes.Text;
                        oPREMIUMINVOICES.EXTERNALNOTES = txtExternalNotes.Text;
                        oPREMIUMINVOICES.ADJUSTMENT = nbAdjustments.Value;
                        oPREMIUMINVOICES.ADJUSTMENTNOTES = txtAdjustmentsNotes.Text;
                        oPREMIUMINVOICES.MODIFIED_BY = SessionInfo.LoggedInUser;
                        oPREMIUMINVOICES.MODIFICATION_DATE = DateTime.Now;
                        oPREMIUMINVOICES.STATUS = SystemEnums.PremiumInvoiceStatus.Initial.GetHashCode();
                        PREMIUMINVOICES.Update(oPREMIUMINVOICES);
                        if (!string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                        {
                            Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
                        }
                        else
                        {
                            Alert(WebMessages.MsgRecordUpdated, AlertType.Success);
                        }
                    }
                    else
                    {
                        Alert(WebMessages.MsgInvAlreadyExists, AlertType.Error);
                        mpeNewInvoice.Show();
                        upNewInvoice.Update();
                    }
                    break;
                case PageActionType.Validate:
                    if (UpdateAccounts(oPREMIUMINVOICES))
                    {
                        if (spBtnValidateText.InnerText == "Validate")
                        {
                            oPREMIUMINVOICES.STATUS = SystemEnums.PremiumInvoiceStatus.Validated.GetHashCode();
                            oPREMIUMINVOICES.VALIDATED_BY = SessionInfo.LoggedInUser;
                            oPREMIUMINVOICES.VALIDATION_DATE = DateTime.Now;
                        }
                        else if (spBtnValidateText.InnerText == "Unvalidate")
                        {
                            oPREMIUMINVOICES.STATUS = SystemEnums.PremiumInvoiceStatus.Initial.GetHashCode();
                            oPREMIUMINVOICES.VALIDATED_BY = string.Empty;
                            oPREMIUMINVOICES.VALIDATION_DATE = null;
                        }
                        PREMIUMINVOICES.Update(oPREMIUMINVOICES);

                        if (!string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                        {
                            Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
                        }
                        else
                        {
                            if (spBtnValidateText.InnerText == "Validate")
                                Alert(WebMessages.MsgRecordValidated, AlertType.Success);
                            else if (spBtnValidateText.InnerText == "Unvalidate")
                                Alert(WebMessages.MsgUnvalidationSuccessfull, AlertType.Success);
                        }
                    }
                    break;
                case PageActionType.Approve:
                    oPREMIUMINVOICES = PREMIUMINVOICES.LoadById(Convert.ToInt32(hdnInvoiceID.Value));
                    if (oPREMIUMINVOICES.VALIDATED_BY.Trim().ToLower() != SessionInfo.LoggedInUser.Trim().ToLower())
                    {
                        oPREMIUMINVOICES.STATUS = SystemEnums.PremiumInvoiceStatus.Approved.GetHashCode();
                        oPREMIUMINVOICES.APPROVED_BY = SessionInfo.LoggedInUser;
                        oPREMIUMINVOICES.APPROVED_DATE = DateTime.Now;
                        PREMIUMINVOICES.Update(oPREMIUMINVOICES);
                        if (!string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                        {
                            Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
                        }
                        else
                        {
                            Alert(WebMessages.MsgInvApproved, AlertType.Success);
                        }
                    }
                    else
                    {
                        Alert(WebMessages.MsgApproveInvoice, AlertType.Error);
                    }
                    break;
                default:
                    break;
            }

            if (nCountInv == 0)
            {
                mpeNewInvoice.Hide();
                upNewInvoice.Update();
                Load_EndoInvoices();
            }
            EditMode(vsINVOICEID);
        }
        #endregion

        #region CheckFiles

        private void CheckFiles(string sFile1, string sFile2, string sFile3)
        {
            spnSourceFile1.Attributes.Add("data-title", Path.GetFileName(sFile1));
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "SetAttchIcoOnPostBack1", "SetAttchIcoOnPostBack('.source-file1'," + "'" + Path.GetFileName(sFile1) + "'" + ");", true);

            spnSourceFile2.Attributes.Add("data-title", Path.GetFileName(sFile2));
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "SetAttchIcoOnPostBack2", "SetAttchIcoOnPostBack('.source-file2'," + "'" + Path.GetFileName(sFile2) + "'" + ");", true);

            spnSourceFile3.Attributes.Add("data-title", Path.GetFileName(sFile3));
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "SetAttchIcoOnPostBack3", "SetAttchIcoOnPostBack('.source-file3'," + "'" + Path.GetFileName(sFile3) + "'" + ");", true);

            btnDownloadFile1.Visible = false;
            btnDownloadFile2.Visible = false;
            btnDownloadFile3.Visible = false;
            if (!string.IsNullOrEmpty(sFile1))
            {
                btnDownloadFile1.Visible = true;
                btnDownloadFile1.Attributes.Add("file", sFile1);
            }
            if (!string.IsNullOrEmpty(sFile2))
            {
                btnDownloadFile2.Visible = true;
                btnDownloadFile2.Attributes.Add("file", sFile2);
            }
            if (!string.IsNullOrEmpty(sFile3))
            {
                btnDownloadFile3.Visible = true;
                btnDownloadFile3.Attributes.Add("file", sFile3);
            }
        }

        #endregion

        #endregion

        #region checkViewPossibility
        protected bool checkViewPossibility(int nStatus)
        {
            if (nStatus == SystemEnums.PremiumInvoiceStatus.Validated.GetHashCode())
            {
                return false;
            }
            else
            {
                return true;

            }
        }
        #endregion

        #region Button Actions Based On UserType
        private void ActionsBasedOnUserType(int nPremiumInvoiceID)
        {
            if (nPremiumInvoiceID != 0)
            {
                PREMIUMINVOICES_DOL oPremInvoices = PREMIUMINVOICES.LoadById(nPremiumInvoiceID);

                if (oPremInvoices.STATUS >= SystemEnums.PremiumInvoiceStatus.PartiallyPaid.GetHashCode())
                {
                    ValidateButtonStyle(true);
                    DisableControlsStyle(true);
                    btnValidateInvoice.Disabled = true;
                    btnValidateInvoice.Attributes["class"] = ValidationButtonEnableDisableStyle(true);
                }
                else if (oPremInvoices.STATUS == SystemEnums.PremiumInvoiceStatus.Approved.GetHashCode())
                {
                    ValidateButtonStyle(true);
                    DisableControlsStyle(true);
                }
                else if (oPremInvoices.STATUS == SystemEnums.PremiumInvoiceStatus.Validated.GetHashCode())
                {
                    ValidateButtonStyle(true);
                    DisableControlsStyle(true);
                    btnApproveInvoice.Disabled = false;
                    btnApproveInvoice.Attributes["class"] = ValidationButtonEnableDisableStyle(false);
                }
                else
                {
                    ValidateButtonStyle(false);
                    DisableControlsStyle(false);
                    btnApproveInvoice.Disabled = true;
                    btnApproveInvoice.Attributes["class"] = ValidationButtonEnableDisableStyle(true);
                }
            }
            upNewInvoice.Update();
            upAccounts.Update();
            upAttachments.Update();
            upAdjustments.Update();
            upValidation.Update();
            upApproved.Update();
        }

        private void ValidateButtonStyle(bool bStatus)
        {
            if (bStatus == true)
            {
                spBtnValidateText.InnerText = "Unvalidate";
                iBtnUnvalidateImageClass.Attributes["class"] = "ace-icon fi-rr-rotate-right bigger-120 white";
                btnValidateInvoice.Attributes["class"] = "btn btn-danger";
            }
            else
            {
                spBtnValidateText.InnerHtml = "Validate";
                iBtnUnvalidateImageClass.Attributes["class"] = "ace-icon fi-rr-checkbox bigger-120 white";
            }
        }

        private void DisableControlsStyle(bool bIsValidated)
        {
            if (bIsValidated == true)
            {
                btnSaveInvoice.Disabled = btnSaveAccounts.Disabled = btnSaveAdjustments.Disabled = true;
                btnApproveInvoice.Disabled = true;
                btnSaveInvoice.Attributes["class"] = btnSaveAccounts.Attributes["class"] = btnSaveAdjustments.Attributes["class"] = SaveButtonEnableDisableStyle(true);
                btnApproveInvoice.Attributes["class"] = SaveButtonEnableDisableStyle(true);

                txtInvoiceNoForEdit.ReadOnly = txtInternalNotes.ReadOnly = txtExternalNotes.ReadOnly = true;

                autoSource.ReadOnly = autoSourceEntity.ReadOnly = autoSourceCOAAccount.ReadOnly = true;
                autoTargetEntity.ReadOnly = autoTargetCOAAccount.ReadOnly = true;

                nbAdjustments.ReadOnly = txtAdjustmentsNotes.ReadOnly = true;
            }
            else if (bIsValidated == false)
            {
                btnSaveInvoice.Disabled = btnSaveAccounts.Disabled = btnSaveAdjustments.Disabled = false;
                btnApproveInvoice.Disabled = false;
                btnSaveInvoice.Attributes["class"] = btnSaveAccounts.Attributes["class"] = btnSaveAdjustments.Attributes["class"] = SaveButtonEnableDisableStyle(false);
                btnApproveInvoice.Attributes["class"] = SaveButtonEnableDisableStyle(false);

                txtInvoiceNoForEdit.ReadOnly = txtInternalNotes.ReadOnly = txtExternalNotes.ReadOnly = false;

                autoSource.ReadOnly = autoSourceEntity.ReadOnly = autoSourceCOAAccount.ReadOnly = false;
                autoTargetEntity.ReadOnly = autoTargetCOAAccount.ReadOnly = false;

                nbAdjustments.ReadOnly = txtAdjustmentsNotes.ReadOnly = false;
            }
        }
        #endregion

        #region SetDefaultServiceProvider
        private void SetDefaultServiceProvider()
        {
            GetServiceProvider(autoServiceProv);
        }
        #endregion

        #region FillSourceAccountAutoList
        private void FillSourceAccountAutoList()
        {
            autoSourceCOAAccount.Clear();
            string Param = string.Empty;
            if (!string.IsNullOrEmpty(autoSource.Value))
            {
                Param = ";P_ENTITY_TYPE," + autoSource.Value;
            }
            else
            {
                Param = ";P_ENTITY_TYPE,";
            }
            if (!string.IsNullOrEmpty(autoSourceEntity.Value))
            {
                Param = Param + ";P_ENTITYID," + autoSourceEntity.Value;
            }
            else
            {
                Param = Param + ";P_ENTITYID,";
            }
            Param = Param + ";P_COUNTRYID,";
            Param = Param + ";P_ACCOUNTTYPE,";
            if (vsOWNERID.HasValue)
            {
                Param = Param + ";P_SPROVIDERID," + null + ";P_OWNERID," + vsOWNERID;
            }
            else
            {
                if (!string.IsNullOrEmpty(autoServiceProv.Value))
                {
                    Param = Param + ";P_SPROVIDERID," + autoServiceProv.Value + ";P_OWNERID," + null;
                }
                else
                {
                    Param = Param + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID + ";P_OWNERID," + null;
                }

            }
            autoSourceCOAAccount.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_COA;P_LANG_ID," + SessionInfo.LANGID + Param;
            upAccounts.Update();
        }
        #endregion

        #region FillTargetAccountAutoList
        private void FillTargetAccountAutoList()
        {
            autoTargetCOAAccount.Clear();
            string Param1 = string.Empty;
            if (!string.IsNullOrEmpty(autoTargetEntity.Value))
            {
                int nType = ENTITIES.LoadById(Convert.ToInt32(autoTargetEntity.Value)).TYPE;
                Param1 = Param1 + ";P_ENTITY_TYPE," + nType;
                Param1 = Param1 + ";P_ENTITYID," + autoTargetEntity.Value;
            }
            else
            {
                Param1 = Param1 + ";P_ENTITY_TYPE,";
                Param1 = Param1 + ";P_ENTITYID,";
            }
            Param1 = Param1 + ";P_COUNTRYID,";
            Param1 = Param1 + ";P_ACCOUNTTYPE,";

            if (vsOWNERID.HasValue)
            {
                Param1 = Param1 + ";P_SPROVIDERID," + null + ";P_OWNERID," + vsOWNERID;
            }
            else
            {
                if (!string.IsNullOrEmpty(autoServiceProv.Value))
                {
                    Param1 = Param1 + ";P_SPROVIDERID," + autoServiceProv.Value + ";P_OWNERID," + null;
                }
                else
                {
                    Param1 = Param1 + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID + ";P_OWNERID," + null;
                }

            }
            autoTargetCOAAccount.SpName = "DBP_FIN_ACCOUNTS.DBP_AUTO_GET_COA;P_LANG_ID," + SessionInfo.LANGID + Param1;
            upAccounts.Update();
        }
        #endregion

        #region UpdateAccounts
        bool UpdateAccounts(PREMIUMINVOICES_DOL oPREMIUMINVOICES)
        {
            int nCountInv = 0;
            bool flag = false;

            if (txtInvoiceNoForEdit.Text.Trim() != oPREMIUMINVOICES.PRMINVOICENBR)
            {
                nCountInv = PREMIUMINVOICES.CheckForExistingInvoiceNo(txtInvoiceNoForEdit.Text.Trim());
            }
            if (nCountInv == 0)
            {
                oPREMIUMINVOICES.PRMINVOICENBR = txtInvoiceNoForEdit.Text.Trim();
                oPREMIUMINVOICES.INTERNALNOTES = txtInternalNotes.Text;
                oPREMIUMINVOICES.EXTERNALNOTES = txtExternalNotes.Text;
                oPREMIUMINVOICES.SOURCE = Convert.ToInt32(autoSource.Value);
                oPREMIUMINVOICES.SOURCENTITYID = Convert.ToInt32(autoSourceEntity.Value);
                oPREMIUMINVOICES.SOURCECOAID = Convert.ToInt32(autoSourceCOAAccount.Value);
                oPREMIUMINVOICES.TARGETENTITYID = Convert.ToInt32(autoTargetEntity.Value);
                oPREMIUMINVOICES.TARGETCOAID = Convert.ToInt32(autoTargetCOAAccount.Value);
                oPREMIUMINVOICES.MODIFIED_BY = SessionInfo.LoggedInUser;
                oPREMIUMINVOICES.MODIFICATION_DATE = DateTime.Now;
                oPREMIUMINVOICES.STATUS = SystemEnums.PremiumInvoiceStatus.Initial.GetHashCode();
                PREMIUMINVOICES.Update(oPREMIUMINVOICES);
                if (!string.IsNullOrEmpty(PREMIUMINVOICES.ERROR_MESSAGE))
                {
                    Alert(PREMIUMINVOICES.ERROR_MESSAGE, AlertType.Error);
                }
                else
                {
                    Alert(WebMessages.MsgRecordUpdated, AlertType.Success);
                    flag = true;
                }
            }
            else
            {
                Alert(WebMessages.MsgInvAlreadyExists, AlertType.Error);
                mpeNewInvoice.Show();
                upNewInvoice.Update();
            }
            return flag;
        }
        #endregion

        #region Invoice Generation

        private void ShowPremiumInvInstallemnts()
        {
            autoPrmInvPayer.Clear();
            autoPrmInvMasterContract.Clear();
            autoPrmInvContract.Clear();
            autoIsInvoiced.Clear();
            dpPrmInvFromIssueDate.ClearDate();
            dpPrmInvToIssueDate.ClearDate();
            autoPrmInvTransactionType.Clear();
            autoIsInvoiced.Clear();
            rptPrmInvInstallments.DataSource = null;
            rptPrmInvInstallments.DataBind();
            gvPrmInvInstallments.DataSource = null;
            gvPrmInvInstallments.DataBind();
            autoPrmInvPayer.SpName = "DBP_JET_ENTITIES.DBP_AUTO_GET_PAYERS;P_LANG_ID," + SessionInfo.LANGID + ";P_USER_ID," + SessionInfo.LoggedInUserID + ";P_SPROVIDERID," + SessionInfo.CurrSProviderID;
            if (SessionInfo.EntityType == SystemEnums.EntityTypes.Payer.GetHashCode())
            {
                PAYERS_DOL oPayer = PAYERS.LoadByEntityId(SessionInfo.UserEntityID.Value);
                if(oPayer != null && oPayer.PAYERID > 0)
                {
                    autoPrmInvPayer.Value = oPayer.PAYERID.ToString();
                    autoPrmInvPayer.Text = oPayer.PAYER_NAME;
                    autoPrmInvPayer_TextChanged(null, null);
                }
            }
            autoPrmInvPayer.Focus();
            FillPayerFeeTypes();
            mpePrmInvInstallments.Show();
            upPrmInvInstallments.Update();
        }

        private void FillInstallments()
        {
            DataView dvInstallments = new DataView();
            int? nPayerID = EssentialsBLL.StringToNullable<int>(autoPrmInvPayer.Value);
            int? nMasterContractID = EssentialsBLL.StringToNullable<int>(autoPrmInvMasterContract.Value);
            int? nContractID = EssentialsBLL.StringToNullable<int>(autoPrmInvContract.Value);
            int? nInvoiced = EssentialsBLL.StringToNullable<int>(autoIsInvoiced.Value);
            int? nTransactionType = EssentialsBLL.StringToNullable<int>(autoPrmInvTransactionType.Value);
            DateTime? dtFromIssueDate = dpPrmInvFromIssueDate.CalendarDate;
            DateTime? dtToIssueDate = dpPrmInvToIssueDate.CalendarDate;
            gvPrmInvInstallments.DataSource = null;
            gvPrmInvInstallments.DataBind();
            if (nPayerID.HasValue && nMasterContractID.HasValue)
                dvInstallments = new DataView(EssentialsBLL.ToDataTable<INSTALLMENTS_DOL>(INSTALLMENTS.Load_All(nMasterContractID.Value, PREMIUMTYPE, dtFromIssueDate, dtToIssueDate, nContractID, nTransactionType,nInvoiced, null, null)));
            if (dvInstallments.Count > 0)
            {
                dvInstallments.Sort = gvPrmInvInstallments.SortExp;
                gvPrmInvInstallments.DataSource = dvInstallments;
                gvPrmInvInstallments.DataBind();
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ActivateDiv", "ActivateDiv('InstallPremiumType','" + PREMIUMTYPE + "');", true);
            mpePrmInvInstallments.Show();
            upPrmInvInstallments.Update();
        }

        private void FillPayerFeeTypes()
        {
            Dictionary<int, string> oDic = new Dictionary<int, string>();
            //oDic.Add(SystemEnums.PayerFeeType.TPA.GetHashCode(), SystemEnums.PayerFeeType.TPA.ToString());
            oDic.Add(SystemEnums.PayerFeeType.Premium.GetHashCode(), SystemEnums.PayerFeeType.Premium.ToString());
            oDic.Add(SystemEnums.PayerFeeType.Commission.GetHashCode(), SystemEnums.PayerFeeType.Commission.ToString());
            oDic.Add(SystemEnums.PayerFeeType.Fees.GetHashCode(), SystemEnums.PayerFeeType.Fees.ToString());
            

            rptPrmInvInstallments.DataSource = oDic;
            rptPrmInvInstallments.DataBind();
        }

        #endregion

        #region AddReport
        private void AddReport(string sParamValues, string sParamText, bool bDisplayAlert = true)
        {
            int nReportId = Convert.ToInt32(PREFERENCES.GetPreferenceValue("MemberPremiumReportID"));
            
            RESULT oResult = PushToReport(
                new
                {
                    REPORTID = nReportId,
                    PARAMVALUES = sParamValues,
                    PARAMTEXT = sParamText,
                    REPORTSTATUS = SystemEnums.RequestQueueStatus.Pending.GetHashCode(),
                    STATUSDATE = DateTime.Now,
                    CREATED_BY = SessionInfo.LoggedInUser
                }
            );

            if (bDisplayAlert)
            {
                if (oResult.Success)
                    Alert(WebMessages.MsgReportToQueue, AlertType.Success);
                else
                    Alert(oResult.ErrorMessage, AlertType.Error);
            }
        }

        #endregion

        #region InvoiceDeletion
        private void InvoiceDeletion()
        {
            txtDeletionNotes.Text = string.Empty;
            mpeDelPremiumInvoice.Show();
            upDelPremiumInvoice.Update();
        }
        #endregion

        #endregion
    }
}
