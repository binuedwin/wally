SELECT
    'DTA' + '|' +
    COALESCE(CAST(rst.company_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.policyid AS varchar), '') + '|' +
    COALESCE(CAST(rst.product_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.baseplan_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.riderplan_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.currency AS varchar), '') + '|' +
    COALESCE(CAST(rst.trans_currency AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_freq AS varchar), '') + '|' +
    COALESCE(CAST(rst.total_prem_paid AS varchar), '') + '|' +
    COALESCE(CAST(rst.cov_num AS varchar), '') + '|' +
    COALESCE(CAST(rst.paid_to_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.trans_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.trans_eff_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.trans_entry_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.trans_amt AS varchar), '') + '|' +
    COALESCE(CAST(rst.account_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.acc_description AS varchar), '') + '|' +
    COALESCE(CAST(rst.pol_issue_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.pol_incept_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.pol_eff_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.payment_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.pol_loan_num AS varchar), '') + '|' +
    COALESCE(CAST(rst.loan_eff_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.pol_loan_amt AS varchar), '') + '|' +
    COALESCE(CAST(rst.fund_value AS varchar), '') + '|' +
    COALESCE(CAST(rst.cash_surrender_value AS varchar), '') + '|' +
    COALESCE(CAST(rst.return_of_prem_value AS varchar), '') + '|' +
    COALESCE(CAST(rst.claim_id_num AS varchar), '') + '|' +
    COALESCE(CAST(rst.claim_status AS varchar), '') + '|' +
    COALESCE(CAST(rst.claim_ben_codes AS varchar), '') + '|' +
    COALESCE(CAST(rst.claim_event_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.claim_approve_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.claim_payment_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.recur_claim_flag AS varchar), '') + '|' +
    COALESCE(CAST(rst.actual_incurred_claim AS varchar), '') + '|' +
    COALESCE(CAST(rst.ri_claim_recoverable_id AS varchar), '') + '|' +
    COALESCE(CAST(rst.ncb_amt AS varchar), '') + '|' +
    COALESCE(CAST(rst.perm_discount_no_claim_discount AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_discount_prem_refund_for_incurred_cov AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_discount_others AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_refund_for_remain_cov AS varchar), '') + '|' +
    COALESCE(CAST(rst.agent_acc_value AS varchar), '') + '|' +
    COALESCE(CAST(rst.Initial_excess_deduction AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_company AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_prdt AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_prime AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_prime_desc AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_sub_prime AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_expense_center AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_afflilate AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_ri_ind AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_dis_channel AS varchar), '') + '|' +
    COALESCE(CAST(rst.r11_sponsor AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_company AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_product AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_prime AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_prime_description AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_sub_prime AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_expense_center AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_affiliate AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_ri_ind AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_location AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_dis_channel AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_sponsor AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_future1 AS varchar), '') + '|' +
    COALESCE(CAST(rst.r12_future2 AS varchar), '') + '|' +
    COALESCE(CAST(rst.ri_covered AS varchar), '') + '|' +
    COALESCE(CAST(rst.sus_acc_value AS varchar), '') + '|' +
    COALESCE(CAST(rst.dis_channel AS varchar), '') + '|' +
    COALESCE(CAST(rst.sponor_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.campaign_code AS varchar), '') + '|' +
    COALESCE(CAST(rst.campaign_yr AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_waiver_start_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_waiver_end_date AS varchar), '') + '|' +
    COALESCE(CAST(rst.prem_inv_no AS varchar), '') + '|' +
    COALESCE(CAST(rst.source_record_id_journal_batch_name AS varchar), '')
FROM
    (
        SELECT DISTINCT
            dt.chartofaccountsid AS company_code,
            pd.policyid,
            pd.product_code,
            pd.base_plan_code,
            pd.rider_plan_code,
            dt.currencycode AS currency,
            dt.transactioncurrencycode AS trans_currency,
            dt.frequencepayment AS prem_freq,
            dt.premiumamout AS total_prem_paid,
            dt.certificate_no AS cov_num,
            dt.paymentduedate AS paid_to_date,
            dt.transactioncode AS trans_code,
            dt.effectivedate AS trans_eff_date,
            dt.transactionentrydate AS trans_entry_date,
            dt.amount AS trans_amt,
            dt.generaljournalaccount AS account_code,
            dt.generaljournalaccountname AS acc_description,
            pd.policyissuedate AS pol_issue_date,
            pd.inceptiondate AS pol_incept_date,
            pd.effective_date AS pol_eff_date,
            dt.paymentdate AS payment_date,
            pd.loan_no AS pol_loan_num,
            pd.loan_effective_date AS loan_eff_date,
            pd.loan_amount AS pol_loan_amt,
            dt.fundvalue AS fund_value,
            dt.cashsurrendervalue AS cash_surrender_value,
            dt.returnofpremiumvalue AS return_of_prem_value,
            clm.claimid AS claim_id_num,
            clm.claimstatus AS claim_status,
            clm.beneficiarycodes AS claim_ben_codes,
            clm.eventdate AS claim_event_date,
            clm.approvedate AS claim_approve_date,
            clm.paymentdate AS claim_payment_date,
            clm.recurringclaimflag AS recur_claim_flag,
            clm.actualincurredclaim AS actual_incurred_claim,
            clm.ri_claim_recoverable_id,
            clm.ncbamount AS ncb_amt,
            clm.pd_discount_no_claim_discount AS perm_discount_no_claim_discount,
            clm.pd_discount_premiumrefundforincurredcov AS prem_discount_prem_refund_for_incurred_cov,
            clm.pd_discount_others,
            clm.pd_refundforremainingcoverage AS prem_refund_for_remain_cov,
            agt.accountvalue AS agent_acc_value,
            clm.InitialExcessDeduction AS Initial_excess_deduction,
            r11.company AS r11_company,
            r11.prdt AS r11_prdt,
            r11.prime AS r11_prime,
            r11.prime_description AS r11_prime_desc,
            r11.sub_prime AS r11_sub_prime,
            r11.expense_center AS r11_expense_center,
            r11.affiliate AS r11_afflilate,
            r11.ri_ind AS r11_ri_ind,
            r11.dis_channel AS r11_dis_channel,
            r11.sponsor AS r11_sponsor,
            r12.company AS r12_company,
            r12.product AS r12_product,
            r12.prime AS r12_prime,
            r12.prime_description AS r12_prime_description,
            r12.sub_prime AS r12_sub_prime,
            r12.expense_center AS r12_expense_center,
            r12.affiliate AS r12_affiliate,
            r12.ri_ind AS r12_ri_ind,
            r12.location AS r12_location,
            r12.dis_channel AS r12_dis_channel,
            r12.sponsor AS r12_sponsor,
            r12.future1 AS r12_future1,
            r12.future2 AS r12_future2,
            rcv.covered AS ri_covered,
            agt1.accountvalue AS sus_acc_value,
            pd.dis_channel,
            pd.sponor_code,
            pd.campaign_code,
            pd.campaign_yr,
            pd.prem_waiver_start_date,
            pd.prem_waiver_end_date,
            pd.prem_inv_no,
            dt.source_record_id_journal_batch_name
        FROM
            dimtransaction dt
        JOIN dimpolicy pd ON dt.policyid = pd.policyid
        LEFT JOIN dimclaim clm ON dt.policyid = clm.policyid
        LEFT JOIN dimaccount agt ON pd.agent_code = agt.agentcode
        LEFT JOIN dimcompany r11 ON pd.company_code = r11.company AND pd.product_code = r11.prdt
        LEFT JOIN dimcompany r12 ON pd.company_code = r12.company AND pd.product_code = r12.product
        LEFT JOIN dimrecoverable rcv ON clm.claimid = rcv.claim_id AND clm.claimseqno = rcv.claim_seq_no
        LEFT JOIN dimaccount agt1 ON clm.suspense_account = agt1.accountcode
    ) rst
