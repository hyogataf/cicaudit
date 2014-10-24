select distinct  a.ncp, a.age, (select lib1 from bknom@rep_to_stby where ctab = '001' and cacc = a.age) agence,
b.ges, (select lib1 from bknom@rep_to_stby where ctab = '035' and cacc = b.ges) gestionnaire,
a.cha, (select lib from bkchap@rep_to_stby where cha = a.cha) chapitre, a.dev, a.clc, a.cli, a.inti, a.uti, a.utic,
a.ife, a.cfe, a.arr, a.dou, a.dfe, a.ddd, a.ddc, a.ddm,a.sde
from bkcom@rep_to_stby a, bkcli@rep_to_stby b 
where a.cli = b.cli and (cha in ('251100','251101','251104') or substr(cha,1,5) in ('25331','25311')) and cfe = 'N' and ife='N' and a.dou <= '22/08/2014'
and not exists (select compte from cm_signatures_cptes_bis@rep_to_stby where compte=a.ncp)
and substr(ges,1,1) in ('1','2','3','6','8')