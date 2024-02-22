dotnet ef --startup-project ../Server/ migrations add V1.0.0 --context RdpzsdDbContext

dotnet ef --startup-project ../Server/ database update --context RdpzsdDbContext

$server = "localhost"
$username = "postgres"
$port = 5432
$db = "NacidRdpzsd2"

SET PGCLIENTENCODING=utf-8
chcp 65001

psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/1.Countries.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/2.ResearchAreasRND.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/3.EducationalQualificationsRND.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/4.DistrictEMS.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/5.MunicipalityEMS.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/6.SettlementEMS.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/7.EducationalFormRND.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/8.StudentStatus.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/9.StudentEvent.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/10.StudentEventQualification.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/11.Period.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/12.School.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/13.AdmissionReason.pg.sql"
psql -h $server -U $username -d $db -p $port -f "../Database/InsertScripts/14.EducationFeeType.pg.sql"

#$migrationScript = ('C:\projects\nacidrdpzsd2\NacidRdpzsd2Migration\MigrationScript.ps1')
#& $migrationScript

$migrationFullPath = ('C:\projects\nacidrdpzsd2\NacidRdpzsd2Migration\NacidRdpzsd2Migration\bin\Debug\net5.0\NacidRdpzsd2Migration.exe')

& $migrationFullPath
