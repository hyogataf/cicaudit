select a.age, (select lib1 from bknom where cacc = a.age and ctab = '001') Agence, c.cli, b.cha, 
a.ncp "N° Compte", b.inti, a.dco "Date Comptable", a.sen, a.mon "Montant", a.lib "Libellé",
b.sde "Solde à J", a.uti "Utilisateur", a.utf "Uti Ayant Forçé", c.ges,
(select lib1 from bknom where ctab='035' and cacc = c.ges) Gestionnaire, count(*) nmbre,
case when b.cha in ('251100','253110') then nid
when b.cha = '251101' then nrc end identification, (select lib1 from bknom where ctab='071' and cacc=c.sec) secteur,
(select lib1 from bknom where ctab = '033' and cacc = c.nat) Nationalité,
(select aa.lib1 from bknom aa, bkprfcli bb where aa.cacc = bb.prf and ctab='045' and bb.cli = c.cli ) profession,
(select sum(mon) from bkhis where age = a.age and ncp = a.ncp and sen = 'C' and dco between trunc(ADD_MONTHS(SYSDATE, -3)) and sysdate) "Mvts au crédit",
(select sum(mon) from bkhis where age = a.age and ncp = a.ncp and sen = 'D' and dco between trunc(ADD_MONTHS(SYSDATE, -3)) and sysdate) "Mvts au dédit"
from bkhis a, bkcom b, bkcli c
where a.ncp=b.ncp
and b.cli=c.cli
and b.cha in ('251100','251101','253110') and substr(c.ges,1,1) not in ('4')
and dco = (SELECT TO_DATE (LPAD (TO_CHAR (mnt1), 8, '0'),'dd/mm/yyyy') FROM bknom WHERE ctab = '001' AND cacc = '99000')
and mon >=5000000
and ope not in ('413','406','507','511','599','560')
and b.cli not in ('1062931','0150196','0150474','0159539','0150550','0158328','0159830',
'0159246','0601080','0882884','0714598','1010132','1026581','0160234','1039781','1037564',
'0954568','0977118','1020121','1054902','1002435','0150016','0150277','0153997','0156137',
'0158428','0159403','0160757','0202186','0201952','0203631','0600928','0714535','0871287',
'0878729','0967497','0963665','0967497','0967897','0968116','1006550','1007288','1020121',
'1054256','0159429','1504357','0160635','0881174','0150021','1507465','1073898','0600778',
'0150021')
group by pie,a.age, c.cli, b.cha, a.ncp,b.inti, a.dco,a.sen, a.mon, a.lib,b.sde, a.uti, a.utf , c.ges,nid, nrc, sec, c.nat