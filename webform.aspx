<%@ Page Title="" Language="C#" MasterPageFile="~/NAS.Master" AutoEventWireup="true" CodeBehind="PremiumInvoices.aspx.cs" Inherits="nTouchCoreWeb.Financial.Payers.PremiumInvoices" %>

<%@ Import Namespace="nTouchCoreLibrary.BLL.Utilities" %>

<%@ Register Assembly="nTouchCoreWebControls" Namespace="nTouchCoreWebControls" TagPrefix="NAS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Src="~/Controls/AutoComplete.ascx" TagName="AutoComplete" TagPrefix="NAS" %>
<%@ Register Src="~/Controls/Date.ascx" TagName="Date" TagPrefix="NAS" %>
<%@ Register Src="~/Controls/MultiSelect.ascx" TagName="MultiSelect" TagPrefix="NAS" %>
<%@ Register Src="~/Controls/ConfirmDialog.ascx" TagName="ConfirmDialog" TagPrefix="NAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .FileUploadClass > div > input[type=file] { width: 100% !important; height: 30px; z-index: 100 !important; }

        .ace-file-input .ace-file-container.selected { right: 0px !important; }

        .ace-file-input .ace-file-container.selected .ace-file-name .ace-icon { background-color: #d1d1d1 !important; }

        .input-xxlarge { width: 540px; max-width: 100%; }

        .StickyHeader th { position: sticky; top: 0; height: 20px; background-color: #cccccc; }

        .infobox { width: 100% !important; }

        .infobox > .infobox-data { width: 100% !important; padding-left: 0px !important; }

        .infobox .infobox-content { color: inherit !important; max-width: 100% !important; font-size: 15px; text-align: center; }

        .ellipsis { max-width: 100%; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; float: left; }

        .infobox.selector { height: 35px !important; margin: 10px !important; border: none !important; width: 150px !important; border: 1px dotted !important; padding: 4px 0px 0px !important; }

        .infobox.selector .infobox-content { max-width: none; }

        .infobox-red.infobox-dark { background-color: #c38889 !important; width: 48% !important; }


        .infobox.selector .infobox-content:first-child { font-weight: normal !important; }

        .infobox.selector .infobox-content.infobox-amount:nth-child(2) { font-weight: bolder !important; font-size: 20px !important; padding-top: 4px !important; max-width: 98% !important; }
    </style>

    <script type="text/javascript">
        function CustomValidateToEffectiveDate(val, args) {
            var strStartDate = document.getElementById("<%=this.dpFromEffecctiveDateSearch.ClientID%>").value;
            var strStopDate = document.getElementById("<%=this.dpToEffecctiveDateSearch.ClientID%>").value;

            if (new Date(strStopDate.substr(6, 4), (strStopDate.substr(3, 2) - 1), strStopDate.substr(0, 2)) > new Date(strStartDate.substr(6, 4), (strStartDate.substr(3, 2) - 1), strStartDate.substr(0, 2))) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }

        function CheckFileSource(sender) {
            var srcClass = "";

            if ($("#" + sender._element.getAttribute("id")).hasClass("source-file1")) {
                srcClass = ".source-file1";
            }
            else if ($("#" + sender._element.getAttribute("id")).hasClass("source-file2")) {
                srcClass = ".source-file2";
            }
            else {
                srcClass = ".source-file3";
            }

            return srcClass;
        }

        function AttachmentClearContents(sender) {

            var srcClass = CheckFileSource(sender);
            var AsyncFileUpload;

            if (srcClass == ".source-file1") {
                AsyncFileUpload = $get("<%=afuFile1.ClientID%>")
            }
            else if (srcClass == ".source-file2") {
                AsyncFileUpload = $get("<%=afuFile2.ClientID%>")
            }
            else {
                AsyncFileUpload = $get("<%=afuFile3.ClientID%>")
            }

            var txts = AsyncFileUpload.getElementsByTagName("input");
            for (var i = 0; i < txts.length; i++) {
                if (txts[i].type == "file") {
                    txts[i].value = "";
                }
            }
        }

        function AttachmentComplete(sender, args) {

            var srcClass = CheckFileSource(sender);

            var myClass = ".ace-file-name" + srcClass;

            $(myClass).attr("data-title", args.get_fileName()).removeClass("red");
            $(".ace-file-container " + srcClass).attr("data-title", args.get_fileName()).addClass('selected');
            $(myClass + " .file").show();
            $(myClass + " .loading").hide();

            var _class = 'ace-icon file ';

            if ((/\.(jpe?g|png|gif)$/i).test(args.get_fileName())) {
                _class += 'fa fa-picture-o file-image green';
            }
            else if ((/\.(pdf)$/i).test(args.get_fileName())) {
                _class += 'fa fa-file-pdf light-red';
            }
            else if ((/\.(xlsx?)$/i).test(args.get_fileName())) {
                _class += 'fa fa-file-excel-o green';
            }
            else if ((/\.(docx?)$/i).test(args.get_fileName())) {
                _class += 'fa fa-file-word-o blue';
            }
            else _class += 'fi-rr-file';

            $(myClass + " i.file").attr("class", _class);
        }

        function AttachmentError(sender, args) {
            var srcClass = CheckFileSource(sender);

            var myClass = ".ace-file-name" + srcClass;

            $(myClass + " .file").show();
            $(myClass + " .loading").hide();
            AttachmentClearContents(sender);
        }

        function AttachmentStarted(sender, args) {

            var srcClass = CheckFileSource(sender);

            var myClass = ".ace-file-name" + srcClass;

            var size = Math.round((sender._inputFile.files[0].size / 1048576) * 100) / 100;

            if (!(/\.(jpe?g|png|gif|pdf|docx?|xlsx?)$/i).test(args.get_fileName())) {

                $(myClass).attr("data-title", "File type not allowed").addClass("red");
                $(myClass + " i.file").attr("class", "ace-icon file fa fa-exclamation-circle red");
                sender._stopLoad();
            }
            else if (size > 2) {
                $(myClass).attr("data-title", "File should be less than 10 MB").addClass("red");
                $(myClass + " i.file").attr("class", "ace-icon file fa fa-exclamation-circle red");
                sender._stopLoad();
            }
            else {
                $(myClass).attr("data-title", "Uploading File ...");
                $(myClass + " .file").hide();
                $(myClass + " .loading").show();
            }
        }

        function SetAttchIcoOnPostBack(srcClass, filename) {

            var myClass = ".ace-file-name" + srcClass;
            var _class = 'ace-icon file ';

            if ((/\.(jpe?g|png|gif)$/i).test(filename)) {
                _class += 'fa fa-picture-o file-image green';
            }
            else if ((/\.(pdf)$/i).test(filename)) {
                _class += 'fa fa-file-pdf light-red';
            }
            else if ((/\.(xlsx?)$/i).test(filename)) {
                _class += 'fa fa-file-excel-o green';
            }
            else if ((/\.(docx?)$/i).test(filename)) {
                _class += 'fa fa-file-word-o blue';
            }
            else _class += 'fi-rr-cloud-upload';    //fi-rr-file

            $(myClass + " i.file").attr("class", _class);
        }

        function CustomValidateActionDate(val, args) {
            var strActionDate = document.getElementById("<%=this.dpActionDate.ClientID%>").value;

            var dTargetDate1 = new Date();
            dTargetDate1.setMonth(dTargetDate1.getMonth() + 1);

            var dTargetDate2 = new Date();
            dTargetDate2.setMonth(dTargetDate2.getMonth() - 1);

            if (new Date(strActionDate.substr(6, 4), (strActionDate.substr(3, 2) - 1), strActionDate.substr(0, 2)) > new Date(dTargetDate1) || new Date(strActionDate.substr(6, 4), (strActionDate.substr(3, 2) - 1), strActionDate.substr(0, 2)) < new Date(dTargetDate2)) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function InitPrmInvActions() {
            $('input.hdr').click(function () {
                if ($(this).prop('checked')) {
                    $('input.itms').prop('checked', true);
                } else {
                    $('input.itms').prop('checked', false);
                }
            });

            $('input.itms').click(function () {
                $('input.hdr').prop('checked', false);
                CheckInputQueue();
            });

            CheckInputQueue();
        }

        function CheckInputQueue() {
            if ($('input.itms').length == $('input.itms:checked').length)
                $('input.hdr').prop('checked', true);
        }
        function LoadScroll() {
            $('.Installments_scroll').slimScroll({
                height: '350px',
                color: '#4383b4'
            });
        }
        function ActivateDiv(divClassName, divActiveCode) {
            $('.' + divClassName + '').css("background-color", "");
            $('.' + divClassName + '[activecode=' + divActiveCode + ']').css("background-color", "#dff0d8", "important");

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Premium Invoices search and view details starts here --%>
    <div class="widget-container-col ui-sortable">
        <%-- Premium Invoices search and filling grid starts here --%>
        <asp:UpdatePanel runat="server" ID="upAccountTransactions" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="widget-box transparent">
                    <div class="widget-header inheader">
                        <div class="pagetitle">
                            <i class="ace-icon fi-rr-crown lighter bigger-130" runat="server" id="dvSearchHeader"></i>
                            <button causesvalidation="false" class="icon_btn" type="button" id="btnBackToMain" runat="server" onserverclick="btnBackToMain_ServerClick" visible="false" style="padding-left:0;">
                                <i class="ace-icon fi-rr-angle-double-small-left bigger-200 orange2"></i>
                            </button>
                            <h4 class="widget-title" runat="server" id="GridTitle">Premium Invoices</h4>
                        </div>
                        <div>
                            <span class="widget-toolbar" runat="server" id="spnDetailsInvoiceSearch">
                                <button class="icon_btn" runat="server" id="btnSearchAccountTransactions" onserverclick="btnSearchAccountTransactions_ServerClick" type="button" validationgroup="AccTxn">
                                    <i class="ace-icon fi-rr-search bigger-160 green"></i>
                                </button>
                                <button class="icon_btn" runat="server" id="btnDownloadFile" onserverclick="btnDownloadFile_ServerClick" type="button">
                                    <i class="ace-icon fa fa-file-excel-o bigger-160 green"></i>
                                </button>
                            </span>
                            <span class="widget-toolbar" runat="server" id="spEndoSearch" visible="false">
                                <button class="icon_btn" runat="server" id="btnInvoiceDetailsSearch" onserverclick="btnInvoiceDetailsSearch_ServerClick" type="button">
                                    <i class="ace-icon fi-rr-search bigger-160 green"></i>
                                </button>
                                <button class="icon_btn" runat="server" id="btnDownloadEndorsements" onserverclick="btnDownloadEndorsements_ServerClick" type="button">
                                    <i class="ace-icon fa fa-file-excel-o bigger-160 green"></i>
                                </button>
                            </span>
                            <span class="widget-toolbar no-border">
                                <button class="btn btn-success btn-action btn-sm" runat="server" type="button" id="btnGeneratePrmInvoices" onserverclick="btnGeneratePrmInvoices_ServerClick" causesvalidation="false" style="padding-right:revert;">
                                    <i class="ace-icon fi-rr-add bigger-160 white" aria-hidden="true" style="font-size: 14px !important;margin-right:3px;    height: 13px;"></i>
                                    Generate Invoices
                                </button>
                            </span>
                        </div>
                    </div>
                    <div class="search-query" runat="server" id="divPremiumSearchHeader">
                        <div class="ItemGroup-row">
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblServiceProv" Text="Service Provider" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:AutoComplete runat="server" ID="autoServiceProv" Css="AutoFill input-large req" isRequired="true" causevalidation="true" Value='<%#Eval("ID")%>' Text='<%#Eval("NAME")%>' AutoPostBack="true" OnTextChanged="autoServiceProv_TextChanged" validationgroup="AccTxn"></NAS:AutoComplete>
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblInvoiceType" Text="Invoice Type" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:MultiSelect runat="server" ID="autoInvoiceType" CssClass="input-large" isRequired="false" Multiple="true" SetEmpty="true" Value='<%#Eval("ID")%>' Text='<%#Eval("NAME")%>'></NAS:MultiSelect>
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblInvoiceNumber" Text="Invoice Number" CssClass="lbl" Width="110px"></asp:Label>
                                <input class="txt input-large" id="txtInvNumber" type="text" runat="server" maxlength="32" />
                            </div>
                            <div class="ItemGroup-320" runat="server" id="dvPolicyNumber">
                                <asp:Label runat="server" ID="lblPolicyNumber" Text="Policy Number" CssClass="lbl" Width="110px"></asp:Label>
                                <input class="txt input-large" id="txtPolicyNumber" type="text" runat="server" maxlength="15" />
                            </div>
                        </div>
                        <div class="ItemGroup-row">
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblSourceEntityTypeSearch" Text="Source Entity Type" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:AutoComplete runat="server" id="autoSourceEntityType" css="AutoFill input-large" isrequired="false" causevalidation="false" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' autopostback="true" ontextchanged="autoSourceEntityType_TextChanged"></NAS:AutoComplete>
                            </div>
                            <div class="ItemGroup-660 autofull">
                                <asp:Label runat="server" ID="lblSourceEntitySearch" Text="Source Entity" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:AutoComplete runat="server" id="autoSourceEntitySearch" css="AutoFill input-xxlarge" isRequired="false" causevalidation="false" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>'></NAS:AutoComplete>
                            </div>
                        </div>
                        <div class="ItemGroup-row">
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblTargetEntityTypeSearch" Text="Target Entity Type" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:AutoComplete runat="server" id="autoTargetEntityType" css="AutoFill input-large" isRequired="false" causevalidation="false" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' autopostback="true" ontextchanged="autoTargetEntityType_TextChanged"></NAS:AutoComplete>
                            </div>
                            <div class="ItemGroup-660 autofull">
                                <asp:Label runat="server" ID="Label2" Text="Target Entity" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:AutoComplete runat="server" id="autoTaretEntitySearch" css="AutoFill input-xxlarge" isRequired="false" causevalidation="false" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>'></NAS:AutoComplete>
                            </div>

                        </div>
                        <div class="ItemGroup-row">
                            <div class="ItemGroup-320" runat="server" id="dvMcontract">
                                <asp:Label runat="server" ID="lblMContractSearch" Text="Master Contract" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:AutoComplete runat="server" id="autoMcontractSearch" css="AutoFill input-large" isRequired="false" causevalidation="false" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>'></NAS:AutoComplete>
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblInvoiceStatus" Text="Invoice Status" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:MultiSelect runat="server" ID="autoInvoiceStatus" CssClass="input-large" isRequired="false" Multiple="true" SetEmpty="true" Value='<%#Eval("ID")%>' Text='<%#Eval("NAME")%>'></NAS:MultiSelect>
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblFromEffecctiveDateSearch" Text="From Issue Date" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:Date runat="server" ID="dpFromEffecctiveDateSearch" CssClass="Date input-large" GrpCssWidth="input-large" requred="false" DateIsRequired="false" />
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblToEffecctiveDateSearch" Text="To Issue Date" CssClass="lbl" Width="110px"></asp:Label>
                                <NAS:Date runat="server" ID="dpToEffecctiveDateSearch" CssClass="Date input-large" GrpCssWidth="input-large" requred="false" DateIsRequired="false" ValidationGroup="AccTxn" />
                                <asp:CustomValidator ID="cvToEffecctiveDateSearch" runat="server" ValidationGroup="AccTxn" ControlToValidate="dpToEffecctiveDateSearch"
                                    ClientValidationFunction="CustomValidateToEffectiveDate" ErrorMessage="To Effective date must be more than from effectivedate"
                                    Display="None"></asp:CustomValidator>
                                <AJAX:ValidatorCalloutExtender ID="vceStopDate" runat="server" TargetControlID="cvToEffecctiveDateSearch"
                                    Enabled="True">
                                </AJAX:ValidatorCalloutExtender>
                            </div>
                            <%--<div runat="server" id="dvGenerateInvoices">
                                    <button class="btn btn-danger pull-right" runat="server" type="button" style="min-width: 150px; height: 40px;" id="btnGeneratePrmInvoices" onserverclick="btnGeneratePrmInvoices_ServerClick" causesvalidation="false">
                                        <i class="ace-icon fa fa-arrows bigger-120"></i>
                                        Generate Invoices
                                    </button>
                                </div>--%>
                        </div>
                    </div>
                    <div runat="server" id="divPremiumInvSearch">
                        <div class="widget-body">
                            <div class="widget-main">
                                <NAS:GridView ID="gvAccountTransactions" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    AllowSorting="True" RowHoverCssClass="GridHoverRowStyle" DataKeyNames="PRMINVOICEID" LinkableRows="false" ShowCheckBox="false" OnPageIndexChanging="gvAccountTransactions_PageIndexChanging"
                                    OnSorting="gvAccountTransactions_Sorting" Width="100%" PageSize="10">
                                    <SelectedRowStyle CssClass="GridSelectedRowStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="18%" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" HeaderText="Invoice Info" SortExpression="PRMINVOICENBR">
                                            <ItemTemplate>
                                                <span style="width: 60px"><b>Invoice Number :</b></span>
                                                <span style="width: 60px"><%#Eval("PRMINVOICENBR") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Invoice Type :</b></span>
                                                <span style="width: 60px"><%#Eval("INVOICETYPE_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Period :</b></span>
                                                <span style="width: 60px"><%# Convert.ToDateTime(Eval("FROMDATE")).ToString("dd-MMM-yyyy") %></span>
                                                <span style="width: 60px">To <%# Convert.ToDateTime(Eval("TODATE")).ToString("dd-MMM-yyyy") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Issue Date :</b></span>
                                                <span style="width: 60px"><%# Convert.ToDateTime(Eval("CREATION_DATE")).ToString("dd-MMM-yyyy") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Due Date :</b></span>
                                                <span style="width: 60px"><%# Convert.ToDateTime(Eval("DUEDATE")).ToString("dd-MMM-yyyy") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="18%" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" HeaderText="Source" SortExpression="SOURCENTITY_NAME">
                                            <ItemTemplate>
                                                <span style="width: 60px"><b>Entity Type :</b></span>
                                                <span style="width: 60px"><%#Eval("SOURCE_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Entity Name :</b></span>
                                                <span style="width: 60px"><%#Eval("SOURCENTITY_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Account :</b></span>
                                                <span style="width: 60px"><%#Eval("SOURCECOA_NAME") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="18%" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" HeaderText="Target" SortExpression="TARGETENTITY_NAME">
                                            <ItemTemplate>
                                                <span style="width: 60px"><b>Entity Type :</b></span>
                                                <span style="width: 60px"><%#Eval("TARGET_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Entity Name :</b></span>
                                                <span style="width: 60px"><%#Eval("TARGETENTITY_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Account :</b></span>
                                                <span style="width: 60px"><%#Eval("TARGETCOA_NAME") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="18%" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" HeaderText="Invoice Details" SortExpression="ENDO_COUNT">
                                            <ItemTemplate>
                                                <span style="width: 60px"><b>Transaction Count :</b></span>
                                                <span style="width: 60px"><%#Eval("ENDO_COUNT") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Transaction Type :</b></span>
                                                <span style="width: 60px"><%#Eval("TRANSACTIONS_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Product Lines :</b></span>
                                                <span style="width: 60px"><%#Eval("PRODUCTLINE_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Branchs :</b></span>
                                                <span style="width: 60px"><%#Eval("BRANCH_NAME") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="18%" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" HeaderText="Amount" SortExpression="AMOUNT_CURR">
                                            <ItemTemplate>
                                                <span style="width: 60px"><b>Amount :</b></span>
                                                <span style="width: 120px; float: right;"><%# Eval("AMOUNT_CURR") +" "+ Eval("CURRENCY_NAME") %></span>
                                                <br />
                                                <span style="width: 60px"><b>Adjustment :</b></span>
                                                <span style="width: 120px; float: right;"><%# Eval("ADJUSTMENT_CURR") +" "+ Eval("CURRENCY_NAME")%></span>
                                                <br />
                                                <span style="width: 60px"><b>Tax :</b></span>
                                                <span style="width: 120px; float: right;"><%# Eval("TAX_CURR") +" "+ Eval("CURRENCY_NAME")%></span>
                                                <br />
                                                <span style="width: 60px"><b>Final Amount :</b></span>
                                                <span style="width: 120px; float: right;"><%# Eval("FINALAMOUNT_CURR") +" "+ Eval("CURRENCY_NAME")%></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STATUS_NAME" SortExpression="STATUS_NAME" HeaderText="Status">
                                            <HeaderStyle Width="7%" />
                                            <ItemStyle Width="7%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div class="inline pos-rel" style="cursor: pointer">
                                                    <label data-toggle="dropdown" data-position="auto">
                                                        <i class="ace-icon fi-rr-apps bigger-130 orange"></i>
                                                    </label>
                                                    <ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close">
                                                        <li>
                                                            <button class="grid_btn" type="button" id="btnEditInvoice" runat="server" onserverclick="btnEditInvoice_ServerClick" invoiceid='<%#Eval("PRMINVOICEID") %>' invoicetypeid='<%#Eval("INVOICETYPE") %>'>
                                                                <i class="ace-icon fi-rr-edit bigger-130 blue"></i>
                                                                Edit
                                                            </button>
                                                        </li>
                                                        <li>
                                                            <button class="grid_btn" type="button" id="btnViewInvoiceDetails" runat="server" onserverclick="btnViewInvoiceDetails_ServerClick" invoiceid='<%#Eval("PRMINVOICEID") %>' invoicetypeid='<%#Eval("INVOICETYPE") %>' payerid='<%#Eval("PAYERID") %>'>
                                                                <i class="ace-icon fi-rr-eye bigger-130 blue"></i>
                                                                View Details
                                                            </button>
                                                        </li>
                                                        <li>
                                                            <button class="grid_btn" type="button" id="btnDownloadTaxInv" runat="server" onserverclick="btnDownloadTaxInv_ServerClick" invoiceid='<%#Eval("PRMINVOICEID") %>' invoicetypeid='<%#Eval("INVOICETYPE") %>'>
                                                                <i class="ace-icon fi-rr-download bigger-130 green"></i>
                                                                Download Invoice
                                                            </button>
                                                        </li>
                                                        <li>
                                                            <button class="grid_btn" type="button" id="btnGenMemberPremium" runat="server" onserverclick="btnGenMemberPremium_ServerClick" invoiceid='<%#Eval("PRMINVOICEID") %>' invoicetypeid='<%#Eval("INVOICETYPE") %>'>
                                                                <i class="ace-icon fa fa-file-excel-o bigger-130 green"></i>
                                                                Member Premium
                                                            </button>
                                                        </li>
                                                        <li>
                                                            <button class="grid_btn" type="button" id="btnDelPremiumInvoices" runat="server" onserverclick="btnDelPremiumInvoices_ServerClick">
                                                                <i class="ace-icon fi-rr-trash bigger-130 red"></i>
                                                                Delete
                                                            </button>
                                                        </li>
                                                        <%-- <li>
                                                                <button class="grid_btn" type="button" id="btnRegenInvoice" runat="server" onserverclick="btnRegenInvoice_ServerClick" invoiceid='<%#Eval("PRMINVOICEID") %>' invoicetypeid='<%#Eval("INVOICETYPE") %>' visible='<%# checkViewPossibility(Convert.ToInt32(Eval("STATUS"))) %>'>
                                                                    <i class="ace-icon fa fa-sign-in bigger-130 green"></i>
                                                                    Regenerate Invoice
                                                                </button>
                                                            </li>--%>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </NAS:GridView>
                                <NAS:GridView ID="gvAccountTransactionsTemp" runat="server" AllowPaging="True"
                                    CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    AllowSorting="True" RowHoverCssClass="GridHoverRowStyle"
                                    DataKeyNames="PRMINVOICEID" LinkableRows="false" ShowCheckBox="false" Width="100%" PageSize="10" Visible="false">
                                    <SelectedRowStyle CssClass="GridSelectedRowStyle" />
                                    <Columns>
                                        <asp:BoundField DataField="PRMINVOICENBR" SortExpression="PRMINVOICENBR" HeaderText="Invoice No">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FROMDATE" SortExpression="FROMDATE" HeaderText="From Date" DataFormatString="{0:dd/MMM-yyyy}">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TODATE" SortExpression="TODATE" HeaderText="To Date" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DUEDATE" SortExpression="DUEDATE" HeaderText="Due Date" DataFormatString="{0:MMM-yyyy}">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PAYER_NAME" SortExpression="PAYER_NAME" HeaderText="Payer">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRODUCTLINE_NAME" SortExpression="PRODUCTLINE_NAME" HeaderText="Product Line">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BRANCH_NAME" SortExpression="BRANCH_NAME" HeaderText="Branch">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TRANSACTIONS_NAME" SortExpression="TRANSACTIONS_NAME" HeaderText="Transaction Type">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENDO_COUNT" SortExpression="ENDO_COUNT" HeaderText="Count">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CURRENCY_NAME" SortExpression="CURRENCY_NAME" HeaderText="Currency">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AMOUNT_CURR" SortExpression="AMOUNT_CURR" HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ADJUSTMENT_CURR" SortExpression="ADJUSTMENT_CURR" HeaderText="Adjustment">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TAX_CURR" SortExpression="TAX_CURR" HeaderText="Tax">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FINALAMOUNT_CURR" SortExpression="FINALAMOUNT_CURR" HeaderText="Final Amount">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="STATUS_NAME" SortExpression="STATUS_NAME" HeaderText="Status">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                    </Columns>
                                </NAS:GridView>
                            </div>
                            <%--<div style="float: right; margin-bottom: 10px;">
                                    <button class="btn btn-success btn-action" runat="server" type="button" style="min-width: 150px; height: 40px;" id="btnGenerateInvoices" onserverclick="btnGenerateInvoices_ServerClick">
                                        <i class="fa fa-sign-in bigger-120"></i>
                                        Generate Invoices
                                    </button>
                                </div>--%>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- Premium Invoices search and filling grid ends here --%>

        <%-- Premium Invoices view details starts here --%>
        <asp:UpdatePanel runat="server" ID="updPremiumInvDetailsView" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="widget-box transparent">

                    <div class="search-query" runat="server" id="divPremiumInvViewSearchDetails" visible="false">
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewPayerName" Text="Payer" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewPayerName" type="text" readonly="true" runat="server" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewAccountName" Text="Product Line" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewAccountName" type="text" readonly="true" runat="server" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewBranchNames" Text="Branch" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewBranchNames" type="text" readonly="true" runat="server" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewTxnTypes" Text="Transaction Types" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewTxnTypes" type="text" readonly="true" runat="server" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewInvNbr" Text="Invoice Number" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewInvNbr" type="text" runat="server" readonly="true" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewFromInvDate" Text="From Invoice Date" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewFromInvDate" type="text" runat="server" readonly="true" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblViewToInvDate" Text="To Invoice Date" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtViewToInvDate" type="text" runat="server" readonly="true" />
                        </div>
                        <div id="dvInvTypeAdmin" runat="server">
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblAmount" Text="Amount" CssClass="lbl" Width="120px"></asp:Label>
                                <input class="txt input-large" id="txtAmount" type="text" runat="server" readonly="true" />
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblAdjustment" Text="Adjustment" CssClass="lbl" Width="120px"></asp:Label>
                                <input class="txt input-large" id="txtAdjustment" type="text" runat="server" readonly="true" maxlength="32" />
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lbTax" Text="Tax" CssClass="lbl" Width="120px"></asp:Label>
                                <input class="txt input-large" id="txtTax" type="text" runat="server" readonly="true" />
                            </div>
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblFinalAmount" Text="Final Amount" CssClass="lbl" Width="120px"></asp:Label>
                                <input class="txt input-large" id="txtFinalAmount" type="text" runat="server" readonly="true" />
                            </div>
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblStatus" Text="Status" CssClass="lbl" Width="120px"></asp:Label>
                            <input class="txt input-large" id="txtStatus" type="text" runat="server" readonly="true" />
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblMasterContracts" Text="Master Contract" CssClass="lbl" Width="120px"></asp:Label>
                            <NAS:AutoComplete runat="server" ID="autoMasterContractInvDetails" Css="AutoFill input-large" isRequired="false" causevalidation="false" Value='<%#Eval("ID")%>' Text='<%#Eval("NAME")%>' AutoPostBack="false"></NAS:AutoComplete>
                        </div>
                        <div class="ItemGroup-320">
                            <asp:Label runat="server" ID="lblEndoTypes" Text="Endorsement Type" CssClass="lbl" Width="120px"></asp:Label>
                            <NAS:MultiSelect runat="server" ID="multiEndoTypes" CssClass="input-large" isRequired="false" Multiple="true" SetEmpty="true"></NAS:MultiSelect>
                        </div>
                    </div>
                    <div runat="server" id="divPremiumInvViewDetails" visible="false">
                        <div class="widget-body">
                            <div class="widget-main">
                                <NAS:GridView ID="gvPremiumInvoiceView" runat="server" AllowPaging="True"
                                    CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" AllowSorting="True" RowHoverCssClass="GridHoverRowStyle"
                                    DataKeyNames="MCONTRACTID,CONTRACTID,ENDORSEMENTID" LinkableRows="false" ShowCheckBox="true" OnPageIndexChanging="gvPremiumInvoiceView_PageIndexChanging" OnSorting="gvPremiumInvoiceView_Sorting"
                                    Width="100%" PageSize="10">
                                    <SelectedRowStyle CssClass="GridSelectedRowStyle" />
                                    <Columns>
                                        <asp:BoundField>
                                            <HeaderStyle Width="2%" />
                                            <ItemStyle Width="2%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENDORSEMENTID" SortExpression="ENDORSEMENTID" HeaderText="Endo.#">
                                            <HeaderStyle Width="6%" />
                                            <ItemStyle Width="6%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CONTRACT_DESC" SortExpression="CONTRACT_DESC" HeaderText="Contract">
                                            <HeaderStyle Width="16%" />
                                            <ItemStyle Width="16%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENDOTYPE_DESC" SortExpression="ENDOTYPE_DESC" HeaderText="Endorsement Type">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TRANSTYPE_DESC" SortExpression="TRANSTYPE_DESC" HeaderText="Transaction Type">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ISSUE_DATE" SortExpression="ISSUE_DATE" HeaderText="Issue Date" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DUEDATE" SortExpression="DUEDATE" HeaderText="Due Date" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOTES" SortExpression="NOTES" HeaderText="Notes">
                                            <HeaderStyle Width="16%" />
                                            <ItemStyle Width="16%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CURRENCY_DESC" SortExpression="CURRENCY_DESC" HeaderText="Currency">
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                    </Columns>
                                </NAS:GridView>
                            </div>
                            <div class="action-box" runat="server" id="dvInvDetailsAction">
                                <button class="btn btn-danger" runat="server" type="button" style="min-width: 150px; height: 40px;" id="btnPayerCutoff" onserverclick="btnPayerCutoff_ServerClick" causesvalidation="false">
                                    <i class="ace-icon fi-rr-scissors bigger-120"></i>
                                    Cutoff
                                </button>
                                <button class="btn btn-success" runat="server" type="button" style="min-width: 150px; height: 40px;" id="btnMoveTransaction" onserverclick="btnMoveTransaction_ServerClick" causesvalidation="false">
                                    <i class="ace-icon fi-rr-cross-circle bigger-120"></i>
                                    Remove Transaction
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- Premium Invoices view details ends here --%>
    </div>
    <%-- Premium Invoices search and view details ends here --%>

    <%--Generate Invoices starts here--%>
    <asp:UpdatePanel runat="server" ID="upGenerateInvoices" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button TabIndex="3" ID="btnShowGenerateInvoices" runat="server" Text="Show"
                Width="1px" CausesValidation="False" Style="display: none" />
            <asp:Button TabIndex="3" ID="btnCloseGenerateInvoices" runat="server" Text="Close"
                Width="1px" CausesValidation="False" Style="display: none" />
            <AJAX:ModalPopupExtender ID="mpeGenerateInvoices" runat="server" TargetControlID="btnShowGenerateInvoices" CancelControlID="btnCloseGenerateInvoices"
                PopupControlID="pnlGenerateInvoices" DropShadow="false" Drag="true" BackgroundCssClass="MainPopup">
            </AJAX:ModalPopupExtender>
            <asp:Panel ID="pnlGenerateInvoices" runat="server" CssClass="popup col-3" Style="min-width: 500px;">
                <div class="widget-box transparent">
                    <div class="widget-header">
                        <i class="ace-icon  fi-rr-edit  lighter bigger-170"></i>
                        <h6 class="widget-title  bigger-125" runat="server" id="hdEmergencyCoverTitle">Generate Invoice</h6>
                        <span class="widget-toolbar">
                            <button class="btn btn-info btn-xs smaller btn-corner" style="background-color: transparent !important; border: 0px !important" runat="server" id="btnClosePopUPGenerateAccounts" onserverclick="btnClosePopUPGenerateAccounts_ServerClick" type="button">
                                <i class="ace-icon fa fa-times red bigger-170"></i>
                            </button>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main" style="margin-bottom: 5px;">
                            <fieldset runat="server" id="fsGetMonth" style="margin-top: 10px;">
                                <div>
                                    <asp:Label runat="server" ID="lblYearRadio" Text="" CssClass="lbl" Width="120px"></asp:Label>
                                    <asp:RadioButton ID="rbNone" runat="server" GroupName="LoadMonths" Visible="false" />
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Panel runat="server" ID="pnlActionFlags">
                                        <label>
                                            <asp:RadioButton ID="rbLastMonth" runat="server" GroupName="LoadMonths" AutoPostBack="true" OnCheckedChanged="rb_CheckedChanged" />
                                            <span class="lbl" runat="server" id="sLastMonth">Last Month</span>
                                        </label>
                                        <span runat="server" id="spSpaceAfterCurrentMonth">&nbsp;&nbsp;&nbsp;</span>
                                        <label>
                                            <asp:RadioButton ID="rbCurrentMonth" runat="server" GroupName="LoadMonths" AutoPostBack="true" OnCheckedChanged="rb_CheckedChanged" />
                                            <span class="lbl" runat="server" id="sCurrentMonth">Current Month</span>
                                        </label>


                                    </asp:Panel>
                                </div>
                            </fieldset>
                            <fieldset runat="server" id="fsDetails" style="margin-top: 10px;">
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblYear" Text="Year" CssClass="lbl" Width="120px"></asp:Label>
                                    <NAS:NumberBox runat="server" Enabled="false" ID="txtYear" CssClass="txt input-large" FieldType="Integer" CausesValidation="false"></NAS:NumberBox>
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblMonth" Text="Month" CssClass="lbl" Width="120px"></asp:Label>
                                    <input class="txt input-large" disabled="disabled" id="txtMonth" type="text" runat="server" />
                                </div>
                            </fieldset>

                            <div class="action-box">
                                <button class="btn btn-info" runat="server" validationgroup="GenerateInvoices" type="button" id="btnSaveGenerateInvoices" onserverclick="btnSaveGenerateInvoices_ServerClick">
                                    <i class="fa fa-sign-in bigger-120"></i>
                                    Generate
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Generate Invoices ends here--%>

    <%--Edit Invoices starts here--%>
    <asp:UpdatePanel runat="server" ID="upNewInvoice" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button TabIndex="3" ID="btnShowInvoice" runat="server" Text="Show"
                Width="1px" CausesValidation="False" Style="display: none" />
            <asp:Button TabIndex="3" ID="btnCloseInvoice" runat="server" Text="Close"
                Width="1px" CausesValidation="False" Style="display: none" />
            <AJAX:ModalPopupExtender ID="mpeNewInvoice" runat="server" TargetControlID="btnShowInvoice" CancelControlID="btnCloseInvoice"
                PopupControlID="plnNewInvoice" DropShadow="false" Drag="true" BackgroundCssClass="MainPopup">
            </AJAX:ModalPopupExtender>
            <asp:Panel ID="plnNewInvoice" runat="server" CssClass="popup col-sm-6 draggable" Width="1000px" Style="">
                <div class="widget-box transparent">
                    <div class="widget-header">
                        <i class="ace-icon fi-rr-edit lighter bigger-170"></i>
                        <h6 class="widget-title bigger-125">Edit Invoice</h6>
                        <span class="widget-toolbar">
                            <button class="btn btn-info btn-xs smaller btn-corner" style="background-color: transparent !important; border: 0px !important" runat="server" id="btnCloseNewInvoce" onserverclick="btnCloseNewInvoice_ServerClick" type="button">
                                <i class="ace-icon fa fa-times red bigger-170"></i>
                            </button>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main padding-8">
                            <asp:HiddenField ID="hdnInvoiceID" runat="server" />
                            <div class="ItemGroup-320">
                                <asp:Label runat="server" ID="lblInvoiceNumberForEdit" Text="Invoice No" CssClass="lbl" Width="120px"></asp:Label>
                                <asp:TextBox runat="server" ID="txtInvoiceNoForEdit" CssClass="txt input-large req"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvInvoiceNoForEdit" runat="server" ErrorMessage="This field is required" ControlToValidate="txtInvoiceNoForEdit" Display="None" SetFocusOnError="true" ValidationGroup="Accounts" />
                                <AJAX:ValidatorCalloutExtender ID="vceInvoiceNoForEdit" runat="server" TargetControlID="rfvInvoiceNoForEdit" />
                            </div>
                            <div class="ItemGroup-row">
                                <asp:Label runat="server" ID="lblInternalNotes" Text="Internal Notes" CssClass="lbl" Width="120px"></asp:Label>
                                <asp:TextBox runat="server" ID="txtInternalNotes" CssClass="txt input-xlarge" Width="80%" MaxLength="1024" TextMode="MultiLine" Height="20%"></asp:TextBox>
                            </div><div class="space-2"></div>
                            <div class="ItemGroup-row">
                                <asp:Label runat="server" ID="lblExternalNotes" Text="External Notes" CssClass="lbl" Width="120px"></asp:Label>
                                <asp:TextBox runat="server" ID="txtExternalNotes" CssClass="txt input-xlarge" Width="80%" MaxLength="1024" TextMode="MultiLine" Height="20%"></asp:TextBox>
                            </div><div class="space-6"></div>
                            <div class="widget-container-col ui-sortable">
                                <div class="tabbable">
                                    <ul class="nav nav-tabs" id="myTab">
                                        <li>
                                            <a aria-expanded="false" href="#Accounts" data-toggle="tab" style="min-width: 100px;" class="active">
                                                <i class="red ace-icon fi-rr-dollar lighter bigger-130"></i>
                                                Accounts 
                                            </a>
                                        </li>
                                        <li>
                                            <a aria-expanded="true" href="#Attachments" data-toggle="tab" style="min-width: 100px;">
                                                <i class="blue ace-icon fi-rr-clip bigger-130 "></i>
                                                Attachments 
                                            </a>
                                        </li>
                                        <li>
                                            <a aria-expanded="true" href="#Adjustments" data-toggle="tab" style="min-width: 100px;">
                                                <i class="green ace-icon fi-rr-label bigger-130 "></i>
                                                Adjustments 
                                            </a>
                                        </li>
                                        <li>
                                            <a aria-expanded="true" href="#Validation" data-toggle="tab" style="min-width: 100px;">
                                                <i class="red2 ace-icon fi-rr-briefcase bigger-130 "></i>
                                                Validation
                                            </a>
                                        </li>
                                        <li>
                                            <a aria-expanded="true" href="#Approved" data-toggle="tab" style="min-width: 100px;">
                                                <i class="blue ace-icon fi-rr-label bigger-130"></i>
                                                Approved   
                                            </a>
                                        </li>
                                    </ul>
                                    <div class="tab-content" style="height: 300px;">
                                        <div class="tab-pane fade in active" id="Accounts" style="min-height: 260px;">
                                            <asp:UpdatePanel runat="server" ID="upAccounts" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblSource" Text="Source" CssClass="lbl" Width="125px"></asp:Label>
                                                        <NAS:autocomplete runat="server" id="autoSource" css="AutoFill input-large req" isrequired="true" causevalidation="true" validationgroup="Accounts" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' autopostback="true" ontextchanged="autoSource_TextChanged"></NAS:autocomplete>
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblSourceEntity" Text="Entity" CssClass="lbl" Width="125px"></asp:Label>
                                                        <NAS:AutoComplete runat="server" id="autoSourceEntity" css="AutoFill input-large req" isRequired="true" causevalidation="true" ValidationGroup="Accounts" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' AutoPostBack="true" OnTextChanged="autoSourceEntity_TextChanged"></NAS:AutoComplete>
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row autofull">
                                                        <asp:Label runat="server" ID="lblSourceCOAAccount" Text="Source Account" CssClass="lbl" Width="125px"></asp:Label>
                                                        <NAS:autocomplete width="540px" runat="server" id="autoSourceCOAAccount" css="AutoFill input-xxlarge req" isrequired="true" ValidationGroup="Accounts" causevalidation="true" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' autopostback="false"></NAS:autocomplete>
                                                    </div><div class="space-2"></div>
                                                    <%--<div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblEntityTypeEdit" Text="Entity Type" CssClass="lbl" Width="125px"></asp:Label>
                                                        <NAS:AutoComplete runat="server" id="autoTargetEntityTypeEdit" css="AutoFill input-large req" isRequired="true" causevalidation="true" ValidationGroup="Accounts" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' AutoPostBack="true" OnTextChanged="autoTargetEntityTypeEdit_TextChanged"></NAS:AutoComplete>
                                                    </div>--%>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblTargetEntity" Text="Entity" CssClass="lbl" Width="125px"></asp:Label>
                                                        <NAS:AutoComplete runat="server" id="autoTargetEntity" css="AutoFill input-large req" isRequired="true" causevalidation="true" ValidationGroup="Accounts" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' AutoPostBack="true" OnTextChanged="autoTargetEntity_TextChanged"></NAS:AutoComplete>
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row autofull">
                                                        <asp:Label runat="server" ID="lblTargetCOAAccount" Text="Target Account" CssClass="lbl" Width="125px"></asp:Label>
                                                        <NAS:autocomplete width="540px" runat="server" id="autoTargetCOAAccount" css="AutoFill input-xxlarge req" isrequired="true" ValidationGroup="Accounts" causevalidation="true" value='<%#Eval("ID")%>' text='<%#Eval("NAME")%>' autopostback="false"></NAS:autocomplete>
                                                    </div><div class="space-2"></div>
                                                    <div class="action-box">
                                                        <button class="btn btn-info" runat="server" validationgroup="Accounts" type="button" id="btnSaveAccounts" onserverclick="btnSaveAccounts_ServerClick">
                                                            <i class="ace-icon fi-rr-disk bigger-120"></i>
                                                            Save
                                                        </button>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane fade" id="Attachments" style="min-height: 260px;">
                                            <asp:UpdatePanel runat="server" ID="upAttachments" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblAttachment1" Text="Attachment 1" CssClass="lbl" Width="130px"></asp:Label>
                                                        <button class="icon_btn pull-right" runat="server" id="btnDownloadFile1" onserverclick="btnDownloadInvoiceFile_ServerClick" type="button">
                                                            <i class="ace-icon fi-rr-download bigger-160 green"></i>
                                                        </button>
                                                        <div class="pull-left" style="width: 65%;">
                                                            <label class="ace-file-input">
                                                                <AJAX:AsyncFileUpload runat="server" ID="afuFile1" CssClass="FileUploadClass source-file1" OnUploadedComplete="afuFile1_UploadedComplete" OnClientUploadComplete="AttachmentComplete" OnClientUploadStarted="AttachmentStarted" OnClientUploadError="AttachmentError" />
                                                                <span class="ace-file-container" data-title="Choose">
                                                                    <span class="ace-file-name source-file1" data-title="No File ..." runat="server" id="spnSourceFile1"><i class=" ace-icon file fi-rr-cloud-upload"></i><i class="fa fa-spinner fa-spin fa-2x loading"></i></span>
                                                                </span>
                                                            </label>
                                                        </div>
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblAttachment2" Text="Attachment 2" CssClass="lbl" Width="130px"></asp:Label>
                                                        <button class="icon_btn pull-right" runat="server" id="btnDownloadFile2" onserverclick="btnDownloadInvoiceFile_ServerClick" type="button">
                                                            <i class="ace-icon fi-rr-download bigger-160 green"></i>
                                                        </button>
                                                        <div class="pull-left" style="width: 65%;">
                                                            <label class="ace-file-input">
                                                                <AJAX:AsyncFileUpload runat="server" ID="afuFile2" CssClass="FileUploadClass source-file2" OnUploadedComplete="afuFile2_UploadedComplete" OnClientUploadComplete="AttachmentComplete" OnClientUploadStarted="AttachmentStarted" OnClientUploadError="AttachmentError" />
                                                                <span class="ace-file-container" data-title="Choose">
                                                                    <span class="ace-file-name source-file2" data-title="No File ..." runat="server" id="spnSourceFile2"><i class=" ace-icon file fi-rr-cloud-upload"></i><i class="fa fa-spinner fa-spin fa-2x loading"></i></span>
                                                                </span>
                                                            </label>
                                                        </div>
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblAttachment3" Text="Attachment 3" CssClass="lbl" Width="130px"></asp:Label>
                                                        <button class="icon_btn pull-right" runat="server" id="btnDownloadFile3" onserverclick="btnDownloadInvoiceFile_ServerClick" type="button">
                                                            <i class="ace-icon fi-rr-download bigger-160 green"></i>
                                                        </button>
                                                        <div class="pull-left" style="width: 65%;">
                                                            <label class="ace-file-input">
                                                                <AJAX:AsyncFileUpload runat="server" ID="afuFile3" CssClass="FileUploadClass source-file3" OnUploadedComplete="afuFile3_UploadedComplete" OnClientUploadComplete="AttachmentComplete" OnClientUploadStarted="AttachmentStarted" OnClientUploadError="AttachmentError" />
                                                                <span class="ace-file-container" data-title="Choose">
                                                                    <span class="ace-file-name source-file3" data-title="No File ..." runat="server" id="spnSourceFile3"><i class=" ace-icon file fi-rr-cloud-upload"></i><i class="fa fa-spinner fa-spin fa-2x loading"></i></span>
                                                                </span>
                                                            </label>
                                                        </div>
                                                    </div><div class="space-2"></div>
                                                    <div class="action-box">
                                                        <button class="btn btn-info" runat="server" type="button" id="btnSaveInvoice" onserverclick="btnSaveInvoice_ServerClick">
                                                            <i class="ace-icon fi-rr-disk bigger-120"></i>
                                                            Save
                                                        </button>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane fade" id="Adjustments" style="min-height: 200px;">
                                            <asp:UpdatePanel runat="server" ID="upAdjustments" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <div class="ItemGroup-320">
                                                        <asp:Label runat="server" ID="lblAdjustments" Text="Adjustments" CssClass="lbl" Width="120px"></asp:Label>
                                                        <NAS:NumberBox runat="server" ID="nbAdjustments" CssClass="txt input-large req" FieldType="Decimal" CausesValidation="true" ValidationGroup="Adjustments"></NAS:NumberBox>
                                                        <asp:RequiredFieldValidator ID="rfvAdjustments" runat="server" ControlToValidate="nbAdjustments" Display="None" ErrorMessage="This field is required" SetFocusOnError="true" ValidationGroup="Adjustments"></asp:RequiredFieldValidator>
                                                        <AJAX:ValidatorCalloutExtender ID="vceAdjustments" runat="server" TargetControlID="rfvAdjustments" />
                                                    </div>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblAdjustmentsNotes" Text="Adjustments Notes" CssClass="lbl" Width="120px"></asp:Label>
                                                        <asp:TextBox runat="server" ID="txtAdjustmentsNotes" CssClass="txt input-xlarge req" MaxLength="1024" Width="70%" TextMode="MultiLine"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvAdjustmentsNotes" runat="server" ErrorMessage="This field is required" ControlToValidate="txtAdjustmentsNotes" Display="None" SetFocusOnError="true" ValidationGroup="Adjustments"></asp:RequiredFieldValidator>
                                                        <AJAX:ValidatorCalloutExtender ID="vceAdjustmentsNotes" runat="server" TargetControlID="rfvAdjustmentsNotes" />
                                                    </div><div class="space-2"></div>
                                                    <div class="action-box">
                                                        <button class="btn btn-info" runat="server" validationgroup="Adjustments" type="button" id="btnSaveAdjustments" onserverclick="btnSaveAdjustments_ServerClick">
                                                            <i class="ace-icon fi-rr-disk bigger-120"></i>
                                                            Save
                                                        </button>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane fade" id="Validation" style="min-height: 200px;">
                                            <asp:UpdatePanel runat="server" ID="upValidation" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblUserName" Text="User Name" CssClass="lbl" Width="120px"></asp:Label>
                                                        <input class="txt input-large" id="txtUserName" type="text" runat="server" maxlength="32" readonly="true" />
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblCurrentDate" Text="Date" CssClass="lbl" Width="120px"></asp:Label>
                                                        <input class="txt input-large" id="txtCurrentDate" type="text" runat="server" maxlength="32" readonly="true" />
                                                    </div><div class="space-2"></div>
                                                    <div class="action-box">
                                                        <button class="btn btn-info" runat="server" type="button" id="btnValidateInvoice" onserverclick="btnValidateInvoice_ServerClick" validationgroup="Accounts">
                                                            <i runat="server" id="iBtnUnvalidateImageClass" class="ace-icon fi-rr-checkbox bigger-120 white"></i>
                                                            <span runat="server" id="spBtnValidateText">Validate</span>
                                                        </button>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="tab-pane fade" id="Approved" style="min-height: 200px;">
                                            <asp:UpdatePanel runat="server" ID="upApproved" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lbltxtUserName1" Text="User Name" CssClass="lbl" Width="120px"></asp:Label>
                                                        <input class="txt input-large" id="txtUserName1" type="text" runat="server" maxlength="32" readonly="true" />
                                                    </div><div class="space-2"></div>
                                                    <div class="ItemGroup-row">
                                                        <asp:Label runat="server" ID="lblCurrentDate2" Text="Date" CssClass="lbl" Width="120px"></asp:Label>
                                                        <input class="txt input-large" id="txtCurrentDate1" type="text" runat="server" maxlength="32" readonly="true" />
                                                    </div><div class="space-2"></div>
                                                    <div class="action-box">
                                                        <button class="btn btn-info" runat="server" type="button" id="btnApproveInvoice" onserverclick="btnApproveInvoice_ServerClick">
                                                            <i class="ace-icon fi-rr-check bigger-120 white"></i>
                                                            Approve
                                                        </button>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Edit Invoices ends here--%>

    <%--Cutoff and move txn panel starts here---%>
    <asp:UpdatePanel runat="server" ID="upPayerAction" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button TabIndex="3" ID="btnShowPayerAction" runat="server" Text="Show"
                Width="1px" CausesValidation="False" Style="display: none" />
            <asp:Button TabIndex="3" ID="btnClosePayerAction" runat="server" Text="Close"
                Width="1px" CausesValidation="False" Style="display: none" />
            <AJAX:ModalPopupExtender ID="mpeNewPayerAction" runat="server" TargetControlID="btnShowPayerAction" CancelControlID="btnClosePayerAction"
                PopupControlID="pnlPayerAction" DropShadow="false" Drag="true" BackgroundCssClass="MainPopup">
            </AJAX:ModalPopupExtender>
            <asp:Panel ID="pnlPayerAction" runat="server" CssClass="popup col-12 widget-container-col ui-sortable" Width="720">

                <div class="widget-box transparent">
                    <div class="widget-header">
                        <i class="ace-icon fi-rr-edit  lighter bigger-125"></i>
                        <h6 class="widget-title bigger-125" runat="server">Cutt-Off Date</h6>
                        <span class="widget-toolbar">
                            <button class="btn btn-info btn-xs smaller btn-corner" style="background-color: transparent !important; border: 0px !important" runat="server" id="btnCloseNewPayerAction" onserverclick="btnCloseNewPayerAction_ServerClick" type="button" causesvalidation="false">
                                <i class="ace-icon fa fa-times red bigger-170"></i>
                            </button>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main padding-8" style="display: inline-block">
                            <div class="ItemGroup-row">
                                <asp:Label runat="server" ID="lblActionDate" Text="Cutt-Off Date" CssClass="lbl" Width="120px"></asp:Label>
                                <NAS:Date runat="server" ID="dpActionDate" CssClass="Date input-xlarge req" GrpCssWidth="input-xlarge" requred="true" DateIsRequired="true" ValidationGroup="PayerAction" />
                                <asp:CustomValidator ID="cvActionDate" runat="server" ValidationGroup="PayerAction" ControlToValidate="dpActionDate"
                                    ClientValidationFunction="CustomValidateActionDate" ErrorMessage="Date must be plus, minus 1 month of today date"
                                    Display="None"></asp:CustomValidator>
                                <AJAX:ValidatorCalloutExtender ID="vceActionDate" runat="server" TargetControlID="cvActionDate"
                                    Enabled="True">
                                </AJAX:ValidatorCalloutExtender>
                            </div><div class="space-2"></div>
                            <div class="ItemGroup-row">
                                <asp:Label runat="server" ID="lblDescription" Text="Description" CssClass="lbl" Width="120px"></asp:Label>
                                <asp:TextBox runat="server" ID="txtDescription" CssClass="txt input-xlarge req" Width="80%" MaxLength="520" TextMode="MultiLine" Height="20%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="This field is required" ControlToValidate="txtDescription" Display="None" SetFocusOnError="true" ValidationGroup="PayerAction" />
                                <AJAX:ValidatorCalloutExtender ID="vceDescription" runat="server" TargetControlID="rfvDescription" />
                            </div><div class="space-2"></div>
                        </div>
                        <div class="action-box">
                            <button class="btn btn-info" runat="server" validationgroup="PayerAction" type="button" id="btnSaveNewPayerAction" onserverclick="btnSaveNewPayerAction_ServerClick">
                                <i class="ace-icon fi-rr-disk bigger-120"></i>
                                Save
                            </button>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Cutoff and move txn panel ends here---%>

    <%--Installments starts here---%>
    <asp:UpdatePanel runat="server" ID="upPrmInvInstallments" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button TabIndex="3" ID="btnShowPrmInvInstallmentsConfirmDialog" runat="server" Text="Show"
                Width="1px" CausesValidation="False" Style="display: none" />
            <asp:Button TabIndex="3" ID="btnClosePrmInvInstallmentsConfirmDialog" runat="server" Text="Close"
                Width="1px" CausesValidation="False" Style="display: none" />
            <AJAX:ModalPopupExtender ID="mpePrmInvInstallments" runat="server" TargetControlID="btnShowPrmInvInstallmentsConfirmDialog" CancelControlID="btnClosePrmInvInstallmentsConfirmDialog"
                PopupControlID="pnlPrmInvInstallments" DropShadow="false" Drag="true" BackgroundCssClass="MainPopup">
            </AJAX:ModalPopupExtender>
            <asp:Panel ID="pnlPrmInvInstallments" runat="server" CssClass="popup col-12 draggable" Width="1100px">
                <div class="widget-box transparent">
                    <div class="widget-header">
                        <h6 class="widget-title bigger-125">Installments</h6>
                        <span class="widget-toolbar" style="color: #69aa46 !important;">
                            <button style="color: #69aa46 !important;background-color: transparent !important; border: 0px !important" class="btn btn-info btn-xs smaller btn-corner" runat="server" id="btnSearchPrmInvoices" onserverclick="btnSearchPrmInvoices_ServerClick" validationgroup="PrmInvoiceSearch" tabindex="-1" type="button">
                                <i style="color: green !important;" class="ace-icon fi-rr-search bigger-160 green"></i>
                            </button>
                            <button style="color: #69aa46 !important;background-color: transparent !important; border: 0px !important" class="btn btn-info btn-xs smaller btn-corner" runat="server" id="btnDownloadPrmInvInstallments" onserverclick="btnDownloadPrmInvInstallments_ServerClick" type="button" validationgroup="PrmInvoiceSearch">
                                <i style="color: green !important;" class="ace-icon fa fa-file-excel-o bigger-160 green"></i>
                            </button>
                            <button class="btn btn-info btn-xs smaller btn-corner" style="background-color: transparent !important; border: 0px !important" runat="server" id="btnPrmInvHideInstallments" onserverclick="btnPrmInvHideInstallments_ServerClick" type="button" tabindex="-1">
                                <i class="ace-icon fa fa-times red bigger-160"></i>
                            </button>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="search-query">
                            <div class="ItemGroup-row">
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblPrmInvPayer" Text="Payer" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:AutoComplete runat="server" ID="autoPrmInvPayer" Css="AutoFill input-large req" AutoPostBack="true" OnTextChanged="autoPrmInvPayer_TextChanged" isRequired="true" TabIndex="0" Value='<%#Eval("ID")%>' Text='<%#Eval("NAME")%>' validationgroup="PrmInvoiceSearch"></NAS:AutoComplete>
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblPrmInvMasterContract" Text="Master Contract" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:AutoComplete runat="server" ID="autoPrmInvMasterContract" Css="AutoFill input-large req" AutoPostBack="true" OnTextChanged="autoPrmInvMasterContract_TextChanged" TabIndex="1" isRequired="true" Value='<%#Eval("ID")%>' Text='<%#Eval("NAME")%>' validationgroup="PrmInvoiceSearch"></NAS:AutoComplete>
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblPrmInvContract" Text="Contract" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:AutoComplete runat="server" ID="autoPrmInvContract" Css="AutoFill input-large" AutoPostBack="false" isRequired="false" Value='<%#Eval("ID")%>' TabIndex="2" Text='<%#Eval("NAME")%>'></NAS:AutoComplete>
                                </div>
                            </div>
                            <div class="ItemGroup-row">
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblInstTransactionType" Text="Transaction Type" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:AutoComplete runat="server" ID="autoPrmInvTransactionType" Css="AutoFill input-large" AutoPostBack="false" isRequired="false" Value='<%#Eval("ID")%>' TabIndex="5" Text='<%#Eval("NAME")%>'></NAS:AutoComplete>
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblPolicyStartDate" Text="Effective Date" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:Date runat="server" ID="dpPolicyStartDate" CssClass="Date input-large" GrpCssWidth="input-large" DateIsRequired="false" Enabled="false" />
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblPolicyEndDate" Text="Expiry Date" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:Date runat="server" ID="dpPolicyEndDate" CssClass="Date input-large" GrpCssWidth="input-large" DateIsRequired="false" Enabled="false" />
                                </div>
                            </div><div class="space-2"></div>
                            <div class="ItemGroup-row">
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblFromIssueDate" Text="From Due Date" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:Date runat="server" ID="dpPrmInvFromIssueDate" CssClass="Date input-large" GrpCssWidth="input-large" DateIsRequired="false" TabIndex="3" />
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="lblToIssueDate" Text="To Due Date" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:Date runat="server" ID="dpPrmInvToIssueDate" CssClass="Date input-large" GrpCssWidth="input-large" DateIsRequired="false" TabIndex="4" />
                                </div>
                                <div class="ItemGroup-320">
                                    <asp:Label runat="server" ID="Label1" Text="Invoiced" CssClass="lbl" Width="100px"></asp:Label>
                                    <NAS:AutoComplete runat="server" ID="autoIsInvoiced" Css="AutoFill input-large" AutoPostBack="false" isRequired="false" Value='<%#Eval("ID")%>' TabIndex="2" Text='<%#Eval("NAME")%>'></NAS:AutoComplete>
                                </div>
                                <%--<div class="ItemGroup-320">
                                <button class="btn btn-info btn-xs smaller" runat="server" id="btnSearchPrmInvoices" onserverclick="btnSearchPrmInvoices_ServerClick" validationgroup="PrmInvoiceSearch" type="button">
                                    Search
                                </button>
                                </div>--%>
                                <%--<button class="btn btn-primary btn-action btn-x pull-right" style="margin-right: 10px;" runat="server" type="button" id="btnSavePrmInvTxns" onserverclick="btnSavePrmInvTxns_ServerClick" validationgroup="PrmInvoiceSearch">
                                    <i class="ace-icon fa fa-spinner bigger-140 white" aria-hidden="true"></i>
                                    Generate
                                </button>--%>
                            </div><div class="space-2"></div>
                            <div class="ItemGroup-row">
                                <button class="btn btn-primary btn-action btn-x pull-right" style="line-height: revert;" runat="server" type="button" id="btnSavePrmInvTxns" onserverclick="btnSavePrmInvTxns_ServerClick" validationgroup="PrmInvoiceSearch">
                                    <i class="ace-icon fi-rr-add bigger-140 white" aria-hidden="true" style="height:13px;"></i>
                                    Generate
                                </button>
                            </div><div class="space-2"></div>
                        </div>

                        <div class="widget-main">
                            <asp:Repeater ID="rptPrmInvInstallments" runat="server" Visible="true">
                                <ItemTemplate>
                                    <div class="infobox colorfulSPDiv InstallPremiumType selector" runat="server" activecode='<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Key")%>' title="Premium Type">
                                        <div class="infobox-data">
                                            <div class="infobox-content">
                                                <button runat="server" id="btnPrmInvPremiumType" class="btn btn-link btn-sm colorfulSubSPDiv" onserverclick="btnPrmInvPremiumType_ServerClick" refid='<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Key")%>'><span><%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Value")%></span></button>
                                            </div>
                                        </div>
                                    </div>
<%--                                    <div class="infobox colorfulSPDiv InstallPremiumType selector" runat="server" activecode='<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Key")%>' title="Premium Type">
                                        <div class="infobox-data">
                                            <div class="infobox-content">
                                                <button runat="server" id="Button1" class="btn btn-link btn-sm colorfulSubSPDiv" onserverclick="btnPrmInvPremiumType_ServerClick" refid='<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Key")%>'><span><%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Value")%></span></button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="infobox colorfulSPDiv InstallPremiumType selector" runat="server" activecode='<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Key")%>' title="Premium Type">
                                        <div class="infobox-data">
                                            <div class="infobox-content">
                                                <button runat="server" id="Button2" class="btn btn-link btn-sm colorfulSubSPDiv" onserverclick="btnPrmInvPremiumType_ServerClick" refid='<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Key")%>'><span><%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<int, string>)Container.DataItem, "Value")%></span></button>
                                            </div>
                                        </div>
                                    </div>--%>
                                </ItemTemplate>
                            </asp:Repeater>

                            <div class="Installments_scroll">
                                <NAS:GridView ID="gvPrmInvInstallments" runat="server" AllowPaging="True"
                                    HeaderStyle-CssClass="StickyHeader" HeaderStyle-BackColor="#cccccc"
                                    CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" AllowSorting="True" RowHoverCssClass="GridHoverRowStyle"
                                    LinkableRows="false" OnPageIndexChanging="gvPrmInvInstallments_PageIndexChanging" OnSorting="gvPrmInvInstallments_Sorting" DataKeyNames="INSTALLMENTID"
                                    OnRowDataBound="gvPrmInvInstallments_RowDataBound"
                                    Width="100%" PageSize="10000">
                                    <SelectedRowStyle CssClass="GridSelectedRowStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="2%" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="cbPrmInvHdr" class="hdr" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="checkbox" runat="server" id="cbPrmInv" class="itms" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CONTRACT_DESC" SortExpression="CONTRACT_DESC" HeaderText="Contract">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REGION" SortExpression="REGION" HeaderText="Region">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENDOTYPE_DESC" SortExpression="ENDOTYPE_DESC" HeaderText="Endorsement Type">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PREMIUM_TYPE" SortExpression="PREMIUM_TYPE" HeaderText="Type">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="TRANSTYPE_DESC" SortExpression="TRANSTYPE_DESC" HeaderText="Transaction Type">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Width="12%" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="ISSUE_DATE" SortExpression="ISSUE_DATE" HeaderText="Issue Date" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DUEDATE" SortExpression="DUEDATE" HeaderText="Due Date" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="NOTES" SortExpression="NOTES" HeaderText="Notes">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="CURRENCY_DESC" SortExpression="CURRENCY_DESC" HeaderText="Currency">
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PAYER_INVNBR" SortExpression="PAYER_INVNBR" HeaderText="Invoice No">
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PAYER_INVSTATUS" SortExpression="PAYER_INVSTATUS" HeaderText="Status">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                    </Columns>
                                </NAS:GridView>
                            </div>
                        </div>
                        <%--<div class="action-box">
                            <button class="btn btn-info" runat="server" type="button" id="btnSavePrmInvTxns" onserverclick="btnSavePrmInvTxns_ServerClick">
                                <i class="ace-icon fi-rr-add bigger-120"></i>
                                Generate
                            </button>
                        </div>--%>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Installments ends here---%>

    <%--------------------------------------Delete Premium Invoice Starts Here------------------------------%>
    <asp:UpdatePanel runat="server" ID="upDelPremiumInvoice" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button TabIndex="3" ID="btnShowDelPremiumInvoice" runat="server" Text="Show"
                Width="1px" CausesValidation="False" Style="display: none" />
            <asp:Button TabIndex="3" ID="btnCloseDelPremiumInvoice" runat="server" Text="Close"
                Width="1px" CausesValidation="False" Style="display: none" />
            <AJAX:ModalPopupExtender ID="mpeDelPremiumInvoice" runat="server" TargetControlID="btnShowDelPremiumInvoice" CancelControlID="btnCloseDelPremiumInvoice"
                PopupControlID="pnlDelPremiumInvoice" DropShadow="false" Drag="true" BackgroundCssClass="MainPopup">
            </AJAX:ModalPopupExtender>
            <asp:Panel ID="pnlDelPremiumInvoice" runat="server" CssClass="popup col-12 col-sm-4 widget-container-col ui-sortable draggable" Style="min-width: 500px">
                <div class="widget-box transparent">
                    <div class="widget-header">
                        <h6 class="widget-title bigger-125">Delete Invoice</h6>
                        <span class="widget-toolbar">
                            <button class="btn btn-info btn-xs smaller btn-corner" style="background-color: transparent !important; border: 0px !important" runat="server" id="btnCloseDelPremiumInvoicePopUp" onserverclick="btnCloseDelPremiumInvoicePopUp_ServerClick" type="button">
                                <i class="ace-icon fa fa-times red bigger-170"></i>
                            </button>
                        </span>
                    </div>
                    <div class="widget-body divexclude">
                        <div class="widget-main padding-8">
                            <div class="ItemGroup-row">
                                <asp:Label runat="server" ID="lblDeletionNotes" Text="Remarks" CssClass="lbl" Width="100px"></asp:Label>
                                <asp:TextBox runat="server" ID="txtDeletionNotes" CssClass="txt input-xlarge req" Width="70%" MaxLength="32" TextMode="MultiLine" Height="10%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvInvDeletion" runat="server" ErrorMessage="This field is required" ControlToValidate="txtDeletionNotes" Display="None" SetFocusOnError="true" ValidationGroup="InvoiceDeletion" />
                                <AJAX:ValidatorCalloutExtender ID="vceInvDeletion" runat="server" TargetControlID="rfvInvDeletion" />
                            </div><div class="space-2"></div>
                            <div class="action-box">
                                <button class="btn btn-danger" runat="server" validationgroup="InvoiceDeletion" type="button" id="btnDeleteInvoice" onserverclick="btnDeleteInvoice_ServerClick">
                                    <i class="ace-icon fi-rr-trash bigger-120 white"></i>
                                    Delete
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hdnBenefAccountStatus" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------------------Delete Premium Invoice Ends Here------------------------------%>

    <%------------------User Controls-------%>
    <NAS:ConfirmDialog runat="server" ID="ucConfirmDialog"></NAS:ConfirmDialog>
    <NAS:ConfirmDialog runat="server" ID="ucRegenConfirmDialog"></NAS:ConfirmDialog>
    <NAS:ConfirmDialog runat="server" ID="ucPremiumInvDelete"></NAS:ConfirmDialog>
</asp:Content>

