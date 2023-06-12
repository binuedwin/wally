with rst as
(SELECT 
split_part(dt.chartofaccountsid,'-',1) as company_code  ----get the text before the first '-'
,case when dt.transactiontypecode='CNE' then claimline.policyid 		   
	  when dt.transactiontypecode='CLP' then claimpayment.policyid 
	  when dt.transactiontypecode='CLR' then claimprecovery.policyid 			  
	  when dt.transactiontypecode='COM' then commissiondue.policyid 	  
	  when dt.transactiontypecode='COL' then commissiontrans.policyid 	  
	  when dt.transactiontypecode='COP' then commissionpay.policyid 	  
	  when dt.transactiontypecode='COR' then commissionrecovery.policyid 	  
	  when dt.transactiontypecode='PRL' then premiumdetail.policyid   
	  when dt.transactiontypecode='PPP' then premiumpartnerpayment.policyid 
	  when dt.transactiontypecode='PRA' then premiumwriteoff.policyid 
	  when dt.transactiontypecode='PRC' then premiumcollection.policyid 		   
	  when dt.transactiontypecode='PRR' then premiumrefundpayment.policyid else null end pol_num
,'MEGEH' prdt_code
,'MEGEH' base_plan_code
,'MEGEH' rider_plan_code
,null currency
,dt.transactioncurrencycode as trans_currency 
,case when dt.transactiontypecode='PRL' then premiumdetail.prem_freq   
	  when dt.transactiontypecode='PPP' then premiumpartnerpayment.prem_freq 
	  when dt.transactiontypecode='PRA' then premiumwriteoff.prem_freq 
	  when dt.transactiontypecode='PRC' then premiumcollection.prem_freq 		   
	  when dt.transactiontypecode='PRR' then premiumrefundpayment.prem_freq else null end prem_freq

,/*case when dt.transactiontypecode='PRL' then premiumdetail.totalallocatedreceivedamount   
	  when dt.transactiontypecode='PPP' then premiumpartnerpayment.totalallocatedreceivedamount 
	  when dt.transactiontypecode='PRA' then premiumwriteoff.totalallocatedreceivedamount 
	  when dt.transactiontypecode='PRC' then premiumcollection.totalallocatedreceivedamount 		   
	  when dt.transactiontypecode='PRR' then premiumrefundpayment.totalallocatedreceivedamount else null end total_prem_paid */ null as total_prem_paid
,1 as cov_num
,case when dt.transactiontypecode='PRL' then format_datetime(premiumdetail.billingperiodenddate,'yyyyMMdd') 	   
	  when dt.transactiontypecode='PPP' then format_datetime(premiumpartnerpayment.billingperiodenddate,'yyyyMMdd') 	 
	  when dt.transactiontypecode='PRA' then format_datetime(premiumwriteoff.billingperiodenddate,'yyyyMMdd') 	 
	  when dt.transactiontypecode='PRC' then format_datetime(premiumcollection.billingperiodenddate,'yyyyMMdd') 	 		   
	  when dt.transactiontypecode='PRR' then format_datetime(premiumrefundpayment.billingperiodenddate,'yyyyMMdd')else null end paid_to_date
,dt.transactiontypecode as trans_code
,format_datetime(dt.transactiondate,'yyyyMMdd') as trans_eff_date
,format_datetime(dt.generalledgerentereddate,'yyyyMMdd') as trans_entry_date
, case when dt.transactiontypecode='COM' and commissiondue.financetransactionid is not null then 
	case when commissiondue.split_com_debit <> 0 then commissiondue.split_com_debit else commissiondue.split_com_credit*-1 end
 else case when dt.transactiondebitamount <> 0 then dt.transactiondebitamount else dt.transactioncreditamount*-1 end end as trans_amt
,split_part(dt.chartofaccountsid,'-',3) as account_code
,null as acc_description
,null as pol_issue_date
,case when dt.transactiontypecode='CNE' then format_datetime(claimline.policystartdate,'yyyyMMdd') 		   
	  when dt.transactiontypecode='CLP' then format_datetime(claimpayment.policystartdate,'yyyyMMdd') 
	  when dt.transactiontypecode='CLR' then format_datetime(claimprecovery.policystartdate,'yyyyMMdd') 			  
	  when dt.transactiontypecode='COM' then format_datetime(commissiondue.policystartdate,'yyyyMMdd') 	  
	  when dt.transactiontypecode='COL' then format_datetime(commissiontrans.policystartdate,'yyyyMMdd') 	  
	  when dt.transactiontypecode='COP' then format_datetime(commissionpay.policystartdate,'yyyyMMdd') 	  
	  when dt.transactiontypecode='COR' then format_datetime(commissionrecovery.policystartdate,'yyyyMMdd') 	  
	  when dt.transactiontypecode='PRL' then format_datetime(premiumdetail.policystartdate,'yyyyMMdd')   
	  when dt.transactiontypecode='PPP' then format_datetime(premiumpartnerpayment.policystartdate,'yyyyMMdd') 
	  when dt.transactiontypecode='PRA' then format_datetime(premiumwriteoff.policystartdate,'yyyyMMdd') 
	  when dt.transactiontypecode='PRC' then format_datetime(premiumcollection.policystartdate,'yyyyMMdd') 		   
	  when dt.transactiontypecode='PRR' then format_datetime(premiumrefundpayment.policystartdate,'yyyyMMdd') else null end pol_incept_date

,case when dt.transactiontypecode='CNE' then format_datetime(claimline.policystartdate,'yyyyMMdd') 	 		   
	  when dt.transactiontypecode='CLP' then format_datetime(claimpayment.policystartdate,'yyyyMMdd') 	 
	  when dt.transactiontypecode='CLR' then format_datetime(claimprecovery.policystartdate,'yyyyMMdd') 	 			  
	  when dt.transactiontypecode='COM' then format_datetime(commissiondue.policystartdate,'yyyyMMdd') 	 	  
	  when dt.transactiontypecode='COL' then format_datetime(commissiontrans.policystartdate,'yyyyMMdd') 		  
	  when dt.transactiontypecode='COP' then format_datetime(commissionpay.policystartdate,'yyyyMMdd') 	 	  
	  when dt.transactiontypecode='COR' then format_datetime(commissionrecovery.policystartdate,'yyyyMMdd') 	 	  
	  when dt.transactiontypecode='PRL' then format_datetime(premiumdetail.policystartdate,'yyyyMMdd') 	   
	  when dt.transactiontypecode='PPP' then format_datetime(premiumpartnerpayment.policystartdate,'yyyyMMdd') 	 
	  when dt.transactiontypecode='PRA' then format_datetime(premiumwriteoff.policystartdate,'yyyyMMdd') 	 
	  when dt.transactiontypecode='PRC' then format_datetime(premiumcollection.policystartdate,'yyyyMMdd') 	 		   
	  when dt.transactiontypecode='PRR' then format_datetime(premiumrefundpayment.policystartdate,'yyyyMMdd') 	 else null end pol_eff_date

,case when dt.transactiontypecode='PRL' then format_datetime(premiumdetail.premiumsettlementdate,'yyyyMMdd') 	    
	  when dt.transactiontypecode='PPP' then format_datetime(premiumpartnerpayment.premiumsettlementdate,'yyyyMMdd') 	  
	  when dt.transactiontypecode='PRA' then format_datetime(premiumwriteoff.premiumsettlementdate,'yyyyMMdd') 	  
	  when dt.transactiontypecode='PRC' then format_datetime(premiumcollection.premiumsettlementdate,'yyyyMMdd') 	  		   
	  when dt.transactiontypecode='PRR' then format_datetime(premiumrefundpayment.premiumsettlementdate,'yyyyMMdd') else null end payment_date
,null as pol_loan_num
,null as loan_eff_date
,null as pol_loan_amt
,null as fund_value
,null as cash_surrender_value
,null as return_of_prem_value
,case when dt.transactiontypecode='CNE' then cast(claimline.claimlineid as varchar) 		   
	  when dt.transactiontypecode='CLP' then cast(claimpayment.claiminvoiceid as varchar)
	  when dt.transactiontypecode='CLR' then cast(claimprecovery.claimlineid as varchar) else null end claim_id_num 

,case when dt.transactiontypecode='CNE' then claimline.internalclaimstatuscode 		   
	  when dt.transactiontypecode='CLP' then claimpayment.internalclaimstatuscode 
	  when dt.transactiontypecode='CLR' then claimprecovery.internalclaimstatuscode else null end claim_status 
,null as claim_ben_codes
,case when dt.transactiontypecode='CNE' then format_datetime(claimline.servicedate,'yyyyMMdd')  		   
	  when dt.transactiontypecode='CLP' then format_datetime(claimpayment.servicedate,'yyyyMMdd')  
	  when dt.transactiontypecode='CLR' then format_datetime(claimprecovery.servicedate,'yyyyMMdd')  else null end claim_event_date 

,case when dt.transactiontypecode='CNE' then format_datetime(claimline.reporteddate,'yyyyMMdd')   		   
	  when dt.transactiontypecode='CLP' then format_datetime(claimpayment.reporteddate,'yyyyMMdd')   
	  when dt.transactiontypecode='CLR' then format_datetime(claimprecovery.reporteddate,'yyyyMMdd') else null end claim_approve_date 

,case when dt.transactiontypecode='CNE' then format_datetime(claimline.paymentdate,'yyyyMMdd')   	 		   
	  when dt.transactiontypecode='CLP' then format_datetime(claimpayment.paymentdate,'yyyyMMdd')   	 
	  when dt.transactiontypecode='CLR' then format_datetime(claimprecovery.paymentdate,'yyyyMMdd')  else null end claim_payment_date 
,null as recur_claim_flag
,null as actual_incurred_claim
,null as ri_claim_recoverable_id
,null as ncb_amt
,null as perm_discount_no_claim_discount
,null as prem_discount_prem_refund_for_incurred_cov
,null as prem_discount_others
,null as prem_refund_for_remain_cov
,null as agent_acc_value

,case when dt.transactiontypecode='CNE' then claimline.deductibleamount 		   
	  when dt.transactiontypecode='CLP' then claimpayment.deductibleamount 
	  when dt.transactiontypecode='CLR' then claimprecovery.deductibleamount else null end Initial_excess_deduction 
,split_part(dt.chartofaccountsid,'-',1) as r11_company   ----get the text before the first '-'
,split_part(dt.chartofaccountsid,'-',6) as r11_prdt      ----get the text between the fifth and sixth '-'
,case when split_part(dt.chartofaccountsid,'-',3)='51500' then '40990' else split_part(dt.chartofaccountsid,'-',3) end as r11_prime     ----same logic as above. just number difference
,null as r11_prime_desc	  
,split_part(dt.chartofaccountsid,'-',4) as r11_sub_prime  ----same logic as above. just number difference
,split_part(dt.chartofaccountsid,'-',5) as r11_expense_center  ----same logic as above. just number difference
,null as r11_afflilate
,split_part(dt.chartofaccountsid,'-',7) as r11_ri_ind  ----same logic as above. just number difference
,split_part(dt.chartofaccountsid,'-',8) as r11_dis_channel ----same logic as above. just number difference
,split_part(dt.chartofaccountsid,'-',11) as r11_sponsor ----same logic as above. just number difference
,null as r12_company
,null as r12_product
,null as r12_prime
,null as r12_prime_description
,null as r12_sub_prime
,null as r12_expense_center
,null as r12_affiliate
,null as r12_ri_ind
,null as r12_location
,null as r12_dis_channel
,null as r12_sponsor
,null as r12_future1
,null as r12_future2
,null as ri_covered
,null as sus_acc_value

,case when dt.transactiontypecode='CNE' then claimline.distributionchannelcode 		   
	  when dt.transactiontypecode='CLP' then claimpayment.distributionchannelcode 
	  when dt.transactiontypecode='CLR' then claimprecovery.distributionchannelcode 			  
	  when dt.transactiontypecode='COM' then commissiondue.distributionchannelcode 	  
	  when dt.transactiontypecode='COL' then commissiontrans.distributionchannelcode 	  
	  when dt.transactiontypecode='COP' then commissionpay.distributionchannelcode 	  
	  when dt.transactiontypecode='COR' then commissionrecovery.distributionchannelcode 	  
	  when dt.transactiontypecode='PRL' then premiumdetail.distributionchannelcode   
	  when dt.transactiontypecode='PPP' then premiumpartnerpayment.distributionchannelcode 
	  when dt.transactiontypecode='PRA' then premiumwriteoff.distributionchannelcode 
	  when dt.transactiontypecode='PRC' then premiumcollection.distributionchannelcode 		   
	  when dt.transactiontypecode='PRR' then premiumrefundpayment.distributionchannelcode else null end dis_channel

,null as sponor_code
,null as campaign_code
,null as campaign_yr
,null as prem_waiver_start_date
,null as prem_waiver_end_date
,null as prem_inv_no
,cast(dt.financetransactionid as varchar)||'-'||cast(dt.financetransactionlineid as varchar) as source_record_id_journal_batch_name
FROM
	detailtransaction dt
LEFT JOIN
	(
		select distinct
			   cl.financetransactionid,
			   cl.deductibleamount,
			   cl.claimlineid,
			   cl.servicedate,
			   cl.internalclaimstatuscode,
			   ci.reporteddate,
			   cp.paymentdate,
			   substr(cl.policyid,1,6)||substr(cl.policyid,10,4) policyid,	
			   pol.policystartdate,
			   pol.distributionchannelcode,
			   cl.sourcesystemid
			   			   
			from
				claimline cl
			left join
				claiminvoice ci
			on  cl.claiminvoiceid=ci.claiminvoiceid and
                cl.sourcesystemid=ci.sourcesystemid and
				upper(ci.currentrecordindicator)='TRUE' 			
			left join
				claimpayment cp
			on  cl.paymentid=cp.paymentid and
                cl.sourcesystemid=cp.sourcesystemid	and
				upper(cp.currentrecordindicator)='TRUE'		
			left join
				policy pol
			on  cl.policyid=pol.policyid and
				ci.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
			where upper(cl.currentrecordindicator)='TRUE' 		  
	)claimline
	ON dt.financetransactionid=claimline.financetransactionid and
	   dt.sourcesystemid=claimline.sourcesystemid and
	   dt.transactiontypecode='CNE'
LEFT JOIN
	(
		select distinct
			   cp.financetransactionid,
			   cl.deductibleamount,
			   cl.claiminvoiceid,
			   cl.servicedate,
			   cp.internalclaimstatuscode,
			   ci.reporteddate,
			   cp.paymentdate,
			   substr(cl.policyid,1,6)||substr(cl.policyid,10,4) policyid,	
			   pol.policystartdate,
			   pol.distributionchannelcode,
			   cp.sourcesystemid,
			   row_number() over (partition by cp.financetransactionid order by cp.PaymentID) as sequence
			   			   
			from
				claimpayment cp
			left join
				claimline cl
			on  cp.paymentid=cl.paymentid and
                cp.sourcesystemid=cl.sourcesystemid	and
				upper(cl.currentrecordindicator)='TRUE'
			left join
				claiminvoice ci
			on  cl.claiminvoiceid=ci.claiminvoiceid and
                cl.sourcesystemid=ci.sourcesystemid and
				upper(ci.currentrecordindicator)='TRUE' 			
			left join
				policy pol
			on  cl.policyid=pol.policyid and
				ci.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
			where 
				upper(cp.currentrecordindicator)='TRUE' 			  
	)claimpayment
	ON dt.financetransactionid=claimpayment.financetransactionid and
	   dt.sourcesystemid=claimpayment.sourcesystemid and
	   dt.transactiontypecode='CLP' and
       claimpayment.sequence=1	   
LEFT JOIN
	(
		select distinct
			   cr.financetransactionid,
			   cl.deductibleamount,
			   cl.claimlineid,
			   cl.servicedate,
			   cl.internalclaimstatuscode,
			   ci.reporteddate,
			   cp.paymentdate,
			   substr(cl.policyid,1,6)||substr(cl.policyid,10,4) policyid,				   
			   pol.policystartdate,
			   pol.distributionchannelcode,
			   cr.sourcesystemid
			   			   
			from
			    claimrecovery cr
			left join
				invoicerecovery ir
			on  cr.recoveryid =ir.recoveryid and
				cr.sourcesystemid =cast(ir.sourcesystemid as varchar) and
				upper(ir.currentrecordindicator)='TRUE' 
			left join
				claiminvoice ci	
			on  ir.claiminvoiceid=ci.claiminvoiceid and
                cast(ir.sourcesystemid as varchar) =ci.sourcesystemid and
				upper(ci.currentrecordindicator)='TRUE'		
			left join
				(select *, row_number() over (partition by claiminvoiceid order by claimlineid,paymentid desc) seq from claimline) cl ---one invoice id may tag to multiple claimline id
			on  ci.claiminvoiceid=cl.claiminvoiceid and
                ci.sourcesystemid=cl.sourcesystemid and
				upper(cl.currentrecordindicator)='TRUE' 		
				and cl.seq =1				
			left join
				claimpayment cp
			on  cl.paymentid=cp.paymentid and
                cl.sourcesystemid=cp.sourcesystemid	and
				upper(cp.currentrecordindicator)='TRUE'										
			left join
				policy pol
			on  cl.policyid=pol.policyid and
				ci.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
			where 
			    upper(cr.currentrecordindicator)='TRUE' 				 		  
	)claimprecovery
	ON dt.financetransactionid=claimprecovery.financetransactionid and
	   dt.sourcesystemid=claimprecovery.sourcesystemid and
	   dt.transactiontypecode='CLR'
LEFT JOIN
/* updated on 2022-Oct-04 because 1 detail transaction could have multiple policy commissiondue, this cause GL amount duplicated. to fix, join with detail transaction first and deduplicate by compare the total commissiondue amount per policy */
	(
		select
			financetransactionid,
			financetransactionlineid,
			policyid,
			policystartdate,
			distributionchannelcode,
			sourcesystemid,
			case when com_seq=ttl_com_cnt then transactiondebitamount-coalesce(lag(roll_debit) over (partition by financetransactionid,financetransactionlineid order by com_seq),0) else split_debit_amount end split_com_debit,
			case when com_seq=ttl_com_cnt then transactioncreditamount-coalesce(lag(roll_credit) over (partition by financetransactionid,financetransactionlineid order by com_seq),0) else split_credit_amount end split_com_credit
		from
			(select
				*,
				round(transactiondebitamount*allot_rate ,3) split_debit_amount,
				round(transactioncreditamount*allot_rate ,3) split_credit_amount,
				sum(round(transactiondebitamount*allot_rate ,3)) over (partition by financetransactionid,financetransactionlineid order by com_seq rows between unbounded preceding and current row) roll_debit,
				sum(round(transactioncreditamount*allot_rate ,3)) over (partition by financetransactionid,financetransactionlineid order by com_seq rows between unbounded preceding and current row) roll_credit
			from
				(select *,
				case when ttl_com_amt=0 then 1 else commissiondueamount/ttl_com_amt end allot_rate -- add sub query to derive rate first
				from
					(select dt.financetransactionid,
						dt.financetransactionlineid,
						substr(comdt.policyid,1,6)||substr(comdt.policyid,10,4) policyid,	
						--  comdt.planid, remove as caused duplication because of multiple plan
						--  comdt.packageid, remove as caused duplication because of multiple plan
						pol.policystartdate,
						pol.distributionchannelcode,
						comdue.sourcesystemid,
						dt.transactiondebitamount,
						dt.transactioncreditamount,
						comdue.commissiondueamount,
						sum(comdue.commissiondueamount) over (partition by dt.financetransactionid, dt.financetransactionlineid) ttl_com_amt,
						count(coalesce(substr(comdt.policyid,1,6)||substr(comdt.policyid,10,4),'')) over (partition by dt.financetransactionid, dt.financetransactionlineid) ttl_com_cnt,
						row_number() over (partition by dt.financetransactionid, dt.financetransactionlineid order by comdue.commissiondueamount desc) com_seq
					
					from 
						detailtransaction dt
					inner join
						commissiondue comdue
					on dt.financetransactionid=comdue.financetransactionid and
						dt.sourcesystemid=comdue.sourcesystemid
					left join
						commissiondetail comdt
					on  comdue.commissiondueid=comdt.commissiondueid	and
						comdue.sourcesystemid=cast(comdt.sourcesystemid as varchar) and
						upper(comdt.currentrecordindicator) = 'TRUE'		   
					left join
							policy pol
					on  comdt.policyid=pol.policyid and
						cast(comdt.sourcesystemid as varchar)=pol.sourcesystemid and
						upper(pol.currentrecordindicator)='TRUE'
					where upper(comdue.currentrecordindicator)='TRUE'
					) com_due
				) rate
			) roll_com
    )commissiondue
	ON dt.financetransactionid=commissiondue.financetransactionid and
	   dt.financetransactionlineid=commissiondue.financetransactionlineid and
	   dt.sourcesystemid=commissiondue.sourcesystemid and
	   dt.transactiontypecode='COM'
LEFT JOIN
	(
		select distinct
			   comtrans.financetransactionid,
			   substr(comdt.policyid,1,6)||substr(comdt.policyid,10,4) policyid,	
		       -- comdt.planid, remove as caused duplication because of multiple plan
			   -- comdt.packageid, remove as caused duplication because of multiple plan
			   pol.policystartdate,
			   pol.distributionchannelcode,				
			   comtrans.sourcesystemid
		
		   from 
		       commissiontransfer comtrans
		   left join		   
			   commissiondue comdue
		   on  comtrans.commissiondueid=comdue.commissiondueid and
			   cast(comtrans.sourcesystemid as varchar)=comdue.sourcesystemid and
			   upper(comdue.currentrecordindicator)='TRUE'
		   left join
			   commissiondetail comdt
		   on  comdue.commissiondueid=comdt.commissiondueid	and
               comdue.sourcesystemid=cast(comdt.sourcesystemid as varchar) and
			   upper(comdt.currentrecordindicator)='TRUE'  
		   left join
				policy pol
		   on  comdt.policyid=pol.policyid and
			   cast(comdt.sourcesystemid as varchar)=pol.sourcesystemid and
			   upper(pol.currentrecordindicator)='TRUE'
		   where upper(comtrans.currentrecordindicator)='TRUE'			
    )commissiontrans
	ON dt.financetransactionid=commissiontrans.financetransactionid and
	   dt.sourcesystemid=cast(commissiontrans.sourcesystemid as varchar) and
	   dt.transactiontypecode='COL'	   
LEFT JOIN
	(
		select distinct
			   compay.financetransactionid,
			   substr(comdt.policyid,1,6)||substr(comdt.policyid,10,4) policyid,	
		       -- comdt.planid, remove as caused duplication because of multiple plan
			   -- comdt.packageid, remove as caused duplication because of multiple plan
			   pol.policystartdate,
			   pol.distributionchannelcode,				
			   compay.sourcesystemid
		
		   from 
			   commissionpayment compay
		   left join		   
		       commissiontransfer comtrans
		   on  compay.commissionpaymentid=comtrans.commissionpaymentid and
			   compay.sourcesystemid=cast(comtrans.sourcesystemid as varchar) and
			   upper(comtrans.currentrecordindicator)='TRUE'		   
		   left join		   
			   commissiondue comdue
		   on  comtrans.commissiondueid=comdue.commissiondueid and
			   cast(comtrans.sourcesystemid as varchar)=comdue.sourcesystemid and
			   upper(comdue.currentrecordindicator)='TRUE' 
		   left join
			   commissiondetail comdt
		   on  comdue.commissiondueid=comdt.commissiondueid	and
               comdue.sourcesystemid=cast(comdt.sourcesystemid as varchar)  and
			   upper(comdt.currentrecordindicator)='TRUE'		   
		   left join
				policy pol
		   on  comdt.policyid=pol.policyid and
			   cast(comdt.sourcesystemid as varchar)=pol.sourcesystemid and
			   upper(pol.currentrecordindicator)='TRUE'
		   where upper(compay.currentrecordindicator)='TRUE' 
    )commissionpay
	ON dt.financetransactionid=commissionpay.financetransactionid and
	   dt.sourcesystemid=commissionpay.sourcesystemid and
	   dt.transactiontypecode='COP'	   
LEFT JOIN
	(
		select distinct
			   comrev.financetransactionid,
			   substr(comdt.policyid,1,6)||substr(comdt.policyid,10,4) policyid,				   
		       -- comdt.planid, remove as caused duplication because of multiple plan
			   -- comdt.packageid, remove as caused duplication because of multiple plan
			   pol.policystartdate,
			   pol.distributionchannelcode,				
			   comrev.sourcesystemid
		
		   from 
			   commissionrecovery comrev
		   left join		   
	   		   commissionrecoveryallocation comreval
		   on  comrev.commissionrecoveryid=comreval.commissionrecoveryid and
			   comrev.sourcesystemid=cast(comreval.sourcesystemid as varchar) and
			   comreval.currentrecordindicator=TRUE
		   left join		   
			   commissiondue comdue
		   on  comreval.commissiondueid=comdue.commissiondueid and
			   cast(comreval.sourcesystemid as varchar)=comdue.sourcesystemid and
			   upper(comdue.currentrecordindicator)='TRUE' 
		   left join
			   commissiondetail comdt
		   on  comdue.commissiondueid=comdt.commissiondueid	and
               comdue.sourcesystemid=cast(comdt.sourcesystemid as varchar) and
			   upper(comdt.currentrecordindicator)='TRUE'		   
		   left join
				policy pol
		   on  comdt.policyid=pol.policyid and
			   cast(comdt.sourcesystemid as varchar)=pol.sourcesystemid and
			   upper(pol.currentrecordindicator)='TRUE'
		   where upper(comrev.currentrecordindicator)='TRUE' 
    )commissionrecovery
	ON dt.financetransactionid=commissionrecovery.financetransactionid and
	   dt.sourcesystemid=commissionrecovery.sourcesystemid and
	   dt.transactiontypecode='COR'	   
LEFT JOIN
	(
		select distinct
			   pd.financetransactionid,
			   substr(pd.policyid,1,6)||substr(pd.policyid,10,4) policyid,			   
			   case when preminv.billingfrequencycode='ANN' then 1 
					when preminv.billingfrequencycode='SEM' then 2 
					when preminv.billingfrequencycode='QRT' then 4 
					when preminv.billingfrequencycode='MTH' then 12 
			   else null end prem_freq,   
			   preminv.totalallocatedreceivedamount,
			   preminv.billingperiodenddate,
			   preminv.premiumsettlementdate,
			   pol.policystartdate,
			   pol.distributionchannelcode,		   
			   pd.sourcesystemid,
			   row_number() over (partition by pd.financetransactionid order by pd.policyid desc) as sequence 
		   from 
				premiumdetail pd
		   left join
                premiuminvoice preminv		
		   on   pd.premiuminvoiceid=preminv.premiuminvoiceid and
				pd.sourcesystemid=preminv.sourcesystemid and
				upper(preminv.currentrecordindicator)='TRUE'
		   left join
				policy pol
		   on   pd.policyid=pol.policyid and
				pd.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
		   where upper(pd.currentrecordindicator)='TRUE' 	
	)premiumdetail	   
	ON dt.financetransactionid=premiumdetail.financetransactionid and
	   dt.sourcesystemid=premiumdetail.sourcesystemid and
	   dt.transactiontypecode='PRL' and
	   premiumdetail.sequence=1
LEFT JOIN
	(
		select distinct
			   prempartner.financetransactionid,
			   substr(pd.policyid,1,6)||substr(pd.policyid,10,4) policyid,	
			   case when preminv.billingfrequencycode='ANN' then 1 
					when preminv.billingfrequencycode='SEM' then 2 
					when preminv.billingfrequencycode='QRT' then 4 
					when preminv.billingfrequencycode='MTH' then 12 
			   else null end prem_freq,   
			   preminv.totalallocatedreceivedamount,
			   preminv.billingperiodenddate,
			   preminv.premiumsettlementdate,
			   pol.policystartdate,
			   pol.distributionchannelcode,		   
			   prempartner.sourcesystemid
		   from 
			    premiumpartnerpayments prempartner
		   left join
                premiuminvoice preminv		
		   on   prempartner.premiumpartnerpaymentid=preminv.premiumpartnerpaymentid and
				cast(prempartner.sourcesystemid as varchar) =preminv.sourcesystemid and
				upper(preminv.currentrecordindicator)='TRUE'
		   left join
				(select distinct policyid,premiuminvoiceid,currentrecordindicator,sourcesystemid,row_number() over (partition by premiuminvoiceid order by policyid desc) as sequence
                 from premiumdetail) pd
			on	preminv.premiuminvoiceid=pd.premiuminvoiceid and
				pd.sequence=1 and
				upper(pd.currentrecordindicator)='TRUE'
		   left join
				policy pol
		   on   pd.policyid=pol.policyid and
				pd.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
		   where prempartner.currentrecordindicator=TRUE
	)premiumpartnerpayment	   
	ON dt.financetransactionid=premiumpartnerpayment.financetransactionid and
	   dt.sourcesystemid=cast(premiumpartnerpayment.sourcesystemid as varchar) and
	   dt.transactiontypecode='PPP'	   
LEFT JOIN
	(
		select distinct
			   premwriteoff.financetransactionid,
			   substr(pd.policyid,1,6)||substr(pd.policyid,10,4) policyid,	
			   case when preminv.billingfrequencycode='ANN' then 1 
					when preminv.billingfrequencycode='SEM' then 2 
					when preminv.billingfrequencycode='QRT' then 4 
					when preminv.billingfrequencycode='MTH' then 12 
			   else null end prem_freq,   
			   preminv.totalallocatedreceivedamount,
			   preminv.billingperiodenddate,
			   preminv.premiumsettlementdate,
			   pol.policystartdate,
			   pol.distributionchannelcode,		   
			   premwriteoff.sourcesystemid
		   from 
			    premiumwriteoff premwriteoff
		   left join
				premiuminvoicewriteoff  preminvwriteoff
		   on 	premwriteoff.premiumwriteoffid=preminvwriteoff.premiumwriteoffid and
				premwriteoff.sourcesystemid=preminvwriteoff.sourcesystemid and
		        upper(preminvwriteoff.currentrecordindicator)='TRUE'
		   left join
                premiuminvoice preminv		
		   on   preminvwriteoff.premiuminvoiceid=preminv.premiuminvoiceid and
				preminvwriteoff.sourcesystemid=preminv.sourcesystemid and
				upper(preminv.currentrecordindicator)='TRUE'
		   left join
				(select distinct policyid,premiuminvoiceid,currentrecordindicator,sourcesystemid,row_number() over (partition by premiuminvoiceid order by policyid desc) as sequence
                 from premiumdetail) pd
			on	preminv.premiuminvoiceid=pd.premiuminvoiceid and
				pd.sequence=1 and
				upper(pd.currentrecordindicator)='TRUE'
		   left join
				policy pol
		   on   pd.policyid=pol.policyid and
				pd.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
		   where 
		         upper(premwriteoff.currentrecordindicator)='TRUE' 
	)premiumwriteoff   
	ON dt.financetransactionid=premiumwriteoff.financetransactionid and
	   dt.sourcesystemid=premiumwriteoff.sourcesystemid	and
	   dt.transactiontypecode='PRA' 	   
LEFT JOIN
	(
		select distinct
			   premcollect.financetransactionid,
			   substr(pd.policyid,1,6)||substr(pd.policyid,10,4) policyid,	
			   case when preminv.billingfrequencycode='ANN' then 1 
					when preminv.billingfrequencycode='SEM' then 2 
					when preminv.billingfrequencycode='QRT' then 4 
					when preminv.billingfrequencycode='MTH' then 12 
			   else null end prem_freq,   
--			   preminv.totalallocatedreceivedamount,
			   preminv.billingperiodenddate,
			   preminv.premiumsettlementdate,
			   pol.policystartdate,
			   pol.distributionchannelcode,		   
			   premcollect.sourcesystemid,
			   row_number() over (partition by premcollect.financetransactionid order by premcollect.receiptid desc) as sequence
			   
		   from 
			    premiumcollection premcollect
		   left join
				(select distinct receiptid,sourcesystemid,premiuminvoiceid,currentrecordindicator
				from
				premiumallocation)  premallocate
		   on 	premcollect.receiptid=premallocate.receiptid and
				premcollect.sourcesystemid=premallocate.sourcesystemid and
		        upper(premallocate.currentrecordindicator)='TRUE'
		   left join
                premiuminvoice preminv		
		   on   premallocate.premiuminvoiceid=preminv.premiuminvoiceid and
				premallocate.sourcesystemid=preminv.sourcesystemid and
				upper(preminv.currentrecordindicator)='TRUE'
		   left join
				(select distinct policyid,premiuminvoiceid,currentrecordindicator,sourcesystemid,row_number() over (partition by premiuminvoiceid order by policyid desc) as sequence
                 from premiumdetail) pd
			on	preminv.premiuminvoiceid=pd.premiuminvoiceid and
				pd.sequence=1 and
				upper(pd.currentrecordindicator)='TRUE'
		   left join
				policy pol
		   on   pd.policyid=pol.policyid and
				pd.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
		   where 
				 upper(premcollect.currentrecordindicator)='TRUE' 
	)premiumcollection
	ON dt.financetransactionid=premiumcollection.financetransactionid and
	   dt.sourcesystemid=premiumcollection.sourcesystemid and
	   dt.transactiontypecode='PRC'	and
	   premiumcollection.sequence=1
	   and premiumcollection.policyid is not null
LEFT JOIN
	(
		select distinct
			   premrefund.financetransactionid,
			   substr(pd.policyid,1,6)||substr(pd.policyid,10,4) policyid,	
			   case when preminv.billingfrequencycode='ANN' then 1 
					when preminv.billingfrequencycode='SEM' then 2 
					when preminv.billingfrequencycode='QRT' then 4 
					when preminv.billingfrequencycode='MTH' then 12 
			   else null end prem_freq,   
			   preminv.totalallocatedreceivedamount,
			   preminv.billingperiodenddate,
			   preminv.premiumsettlementdate,
			   pol.policystartdate,
			   pol.distributionchannelcode,		   
			   premrefund.sourcesystemid
		   from 
			    premiumrefundpayment  premrefund
		   left join
				(select distinct premiumrefundpaymentid,sourcesystemid,premiuminvoiceid,currentrecordindicator
				from
				premiumallocation)  premallocate
		   on 	premrefund.premiumrefundpaymentid =premallocate.premiumrefundpaymentid  and
				premrefund.sourcesystemid=premallocate.sourcesystemid and
				upper(premallocate.currentrecordindicator)='TRUE'
		   left join
                premiuminvoice preminv		
		   on   premallocate.premiuminvoiceid=preminv.premiuminvoiceid and
				premallocate.sourcesystemid=preminv.sourcesystemid and
				upper(preminv.currentrecordindicator)='TRUE'
		   left join
				(select distinct policyid,premiuminvoiceid,currentrecordindicator,sourcesystemid,row_number() over (partition by premiuminvoiceid order by policyid desc) as sequence
                 from premiumdetail) pd
			on	preminv.premiuminvoiceid=pd.premiuminvoiceid and
				pd.sequence=1 and
				upper(pd.currentrecordindicator)='TRUE'
		   left join
				policy pol
		   on   pd.policyid=pol.policyid and
				pd.sourcesystemid=pol.sourcesystemid and
				upper(pol.currentrecordindicator)='TRUE'
		   where 
				 upper(premrefund.currentrecordindicator)='TRUE' 
	)premiumrefundpayment
	ON dt.financetransactionid=premiumrefundpayment.financetransactionid and
	   dt.sourcesystemid=premiumrefundpayment.sourcesystemid and
	   dt.transactiontypecode='PRR'
where upper(dt.currentrecordindicator)='TRUE' and 
	 split_part(dt.chartofaccountsid,'-',1) in ('53152','53153','53154','42060','42041','42061','42042','42062','42043','42044','42063','42064') and
	 cast(format_datetime(dt.generalledgerentereddate,'yyyyMM') as varchar)='202101'   -- fitler to select 202101 data only
)
select
'DTA'||'|'||
coalesce(cast(company_code as varchar),'')||'|'||
coalesce(cast(pol_num as varchar),'')||'|'||
coalesce(cast(prdt_code as varchar),'')||'|'||
coalesce(cast(base_plan_code as varchar),'')||'|'||
coalesce(cast(rider_plan_code as varchar),'')||'|'||
coalesce(cast(currency as varchar),'')||'|'||
coalesce(cast(trans_currency as varchar),'')||'|'||
coalesce(cast(prem_freq as varchar),'')||'|'||
coalesce(cast(total_prem_paid as varchar),'')||'|'||
coalesce(cast(cov_num as varchar),'')||'|'||
coalesce(cast(paid_to_date as varchar),'')||'|'||
coalesce(cast(trans_code as varchar),'')||'|'||
coalesce(cast(trans_eff_date as varchar),'')||'|'||
coalesce(cast(trans_entry_date as varchar),'')||'|'||
coalesce(cast(trans_amt as varchar),'')||'|'||
coalesce(cast(account_code as varchar),'')||'|'||
coalesce(cast(acc_description as varchar),'')||'|'||
coalesce(cast(pol_issue_date as varchar),'')||'|'||
coalesce(cast(pol_incept_date as varchar),'')||'|'||
coalesce(cast(pol_eff_date as varchar),'')||'|'||
coalesce(cast(payment_date as varchar),'')||'|'||
coalesce(cast(pol_loan_num as varchar),'')||'|'||
coalesce(cast(loan_eff_date as varchar),'')||'|'||
coalesce(cast(pol_loan_amt as varchar),'')||'|'||
coalesce(cast(fund_value as varchar),'')||'|'||
coalesce(cast(cash_surrender_value as varchar),'')||'|'||
coalesce(cast(return_of_prem_value as varchar),'')||'|'||
coalesce(cast(claim_id_num as varchar),'')||'|'||
coalesce(cast(claim_status as varchar),'')||'|'||
coalesce(cast(claim_ben_codes as varchar),'')||'|'||
coalesce(cast(claim_event_date as varchar),'')||'|'||
coalesce(cast(claim_approve_date as varchar),'')||'|'||
coalesce(cast(claim_payment_date as varchar),'')||'|'||
coalesce(cast(recur_claim_flag as varchar),'')||'|'||
coalesce(cast(actual_incurred_claim as varchar),'')||'|'||
coalesce(cast(ri_claim_recoverable_id as varchar),'')||'|'||
coalesce(cast(ncb_amt as varchar),'')||'|'||
coalesce(cast(perm_discount_no_claim_discount as varchar),'')||'|'||
coalesce(cast(prem_discount_prem_refund_for_incurred_cov as varchar),'')||'|'||
coalesce(cast(prem_discount_others as varchar),'')||'|'||
coalesce(cast(prem_refund_for_remain_cov as varchar),'')||'|'||
coalesce(cast(agent_acc_value as varchar),'')||'|'||
coalesce(cast(Initial_excess_deduction as varchar),'')||'|'||
coalesce(cast(r11_company as varchar),'')||'|'||
coalesce(cast(r11_prdt as varchar),'')||'|'||
coalesce(cast(r11_prime as varchar),'')||'|'||
coalesce(cast(r11_prime_desc as varchar),'')||'|'||
coalesce(cast(r11_sub_prime as varchar),'')||'|'||
coalesce(cast(r11_expense_center as varchar),'')||'|'||
coalesce(cast(r11_afflilate as varchar),'')||'|'||
coalesce(cast(r11_ri_ind as varchar),'')||'|'||
coalesce(cast(r11_dis_channel as varchar),'')||'|'||
coalesce(cast(r11_sponsor as varchar),'')||'|'||
coalesce(cast(r12_company as varchar),'')||'|'||
coalesce(cast(r12_product as varchar),'')||'|'||
coalesce(cast(r12_prime as varchar),'')||'|'||
coalesce(cast(r12_prime_description as varchar),'')||'|'||
coalesce(cast(r12_sub_prime as varchar),'')||'|'||
coalesce(cast(r12_expense_center as varchar),'')||'|'||
coalesce(cast(r12_affiliate as varchar),'')||'|'||
coalesce(cast(r12_ri_ind as varchar),'')||'|'||
coalesce(cast(r12_location as varchar),'')||'|'||
coalesce(cast(r12_dis_channel as varchar),'')||'|'||
coalesce(cast(r12_sponsor as varchar),'')||'|'||
coalesce(cast(r12_future1 as varchar),'')||'|'||
coalesce(cast(r12_future2 as varchar),'')||'|'||
coalesce(cast(ri_covered as varchar),'')||'|'||
coalesce(cast(sus_acc_value as varchar),'')||'|'||
coalesce(cast(dis_channel as varchar),'')||'|'||
coalesce(cast(sponor_code as varchar),'')||'|'||
coalesce(cast(campaign_code as varchar),'')||'|'||
coalesce(cast(campaign_yr as varchar),'')||'|'||
coalesce(cast(prem_waiver_start_date as varchar),'')||'|'||
coalesce(cast(prem_waiver_end_date as varchar),'')||'|'||
coalesce(cast(prem_inv_no as varchar),'')||'|'||
coalesce(cast(source_record_id_journal_batch_name as varchar),'')
from rst
union all
select 'FTR'||'|'||cast(count(*) as varchar)||'|'||cast(coalesce(sum(cast(case when trans_amt<0 then 0 else trans_amt end as decimal(18,2))),0) as varchar)||'|'||cast(coalesce(sum(cast(case when trans_amt>0 then 0 else trans_amt end as decimal(18,2))),0) as varchar)
from rst
