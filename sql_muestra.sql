select MESAS.Descripcion, MESAS.Id from MESAS
inner join LOCALES on MESAS.Id_Local=LOCALES.Id
where LOCALES.Id=1 and MESAS.Id  in (
select RESERVAS.Id_Mesa
from RESERVAS inner join MESAS on RESERVAS.Id_Mesa=MESAS.Id
inner join LOCALES on MESAS.Id_Local=LOCALES.Id
where (RESERVAS.Hora!='12:00' and RESERVAS.Dia!='02/21/2020')
)