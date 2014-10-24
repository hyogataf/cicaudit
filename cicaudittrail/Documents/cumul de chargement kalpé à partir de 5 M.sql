select  a.first_name PRENOM,a.family_name NOM,a.legal_id CNI,c.description ACT_PROF
,a.address_of_sponsor ADRESSE,a.phone_2 TEL,a.email MAIL,d.description ACTIVITE
,(substr(a.card_number,1,6)||'XXXXXX'||substr(a.card_number,13,4)) N_CARTE,b.billing_amount MT_CHGT
,(select sum(g.billing_amount) from transaction_hist  g 
where g.reversal_flag='N' and g.transaction_code in ('N4')
and g.transaction_date>=to_date($P{DATE_DEB},'DD/MM/YY HH24:MI:SS') 
and g.transaction_date<=to_date($P{DATE_FIN},'DD/MM/YY HH24:MI:SS') 
and g.card_number(+)=b.card_number) CUM_CHGT_PER
,b.transaction_date DATE_CHRGT
,b.user_create ID_OPE,e.lib OPERATEUR
from powercard.prepaid_card_request_hist a,transaction_hist b ,socioprof_list c,CLIENT_ACTIVITY_SET d,bank.evuti@db_link_stby_delta e
where  a.card_product_code in ('901','906')
and b.reversal_flag='N' and transaction_code in ('N4')
and b.transaction_date>=to_date($P{DATE_DEB},'DD/MM/YY HH24:MI:SS')
and b.transaction_date<=to_date($P{DATE_FIN},'DD/MM/YY HH24:MI:SS') 
and a.card_number(+)=b.card_number
and a.socio_prof_code=c.socio_prof_code (+)
and a.activity_code=c.activity_code (+)
and c.bank_code='020012'
and c.activity_code=d.activity_code (+)
and d.bank_code='020012'
and substr(b.user_create,1,4)=substr(e.cuti(+),1,4)
group by b.card_number, a.first_name,a.family_name,a.legal_id,c.description,a.address_of_sponsor,a.phone_2,a.email,a.card_number,b.transaction_date,d.description,b.billing_amount,b.user_create,e.lib
having b.billing_amount>$P{seuil}
order by 3,9 desc