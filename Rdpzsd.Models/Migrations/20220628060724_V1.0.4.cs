using Microsoft.EntityFrameworkCore.Migrations;

namespace Rdpzsd.Models.Migrations
{
    public partial class V104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into studentstatus(id, version, name, namealt, alias, isactive, vieworder)
                values(5, 0, 'Дипломиран', 'Graduated', 'graduated', true, 5);");

            migrationBuilder.Sql(@"insert into studentevent(id, studentstatusid, version, name, namealt, alias, isactive, vieworder)
				values(501, 5, 0, 'С диплома', 'Graduated With Diploma', 'graduatedWithDiploma', true, 40),
					  (502, 5, 0, 'Без диплома', 'Graduated Without Diploma', 'graduatedWithoutDiploma', true, 41);
			");

            migrationBuilder.Sql(@"insert into studenteventqualification(studenteventid, educationalqualificationid, version)
				values(501, 1, 0),
					  (501, 2, 0),
                      (501, 3, 0),
                      (501, 4, 0),
                      (502, 1, 0),
					  (502, 2, 0),
                      (502, 3, 0),
                      (502, 4, 0);
			");

            migrationBuilder.Sql(@"update personstudent set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update personstudent set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");
            migrationBuilder.Sql(@"update personstudentsemester set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update personstudentsemester set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");
            migrationBuilder.Sql(@"update personstudenthistory set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update personstudenthistory set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");
            migrationBuilder.Sql(@"update personstudentsemesterhistory set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update personstudentsemesterhistory set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");

            migrationBuilder.Sql(@"update persondoctoral set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update persondoctoral set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");
            migrationBuilder.Sql(@"update persondoctoralsemester set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update persondoctoralsemester set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");
            migrationBuilder.Sql(@"update persondoctoralhistory set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update persondoctoralhistory set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");
            migrationBuilder.Sql(@"update persondoctoralsemesterhistory set studenteventid = 501, studentstatusid = 5 where studenteventid = 401;");
            migrationBuilder.Sql(@"update persondoctoralsemesterhistory set studenteventid = 502, studentstatusid = 5 where studenteventid = 402;");

            migrationBuilder.Sql(@"delete from studenteventqualification where studenteventid = 401 or studenteventid = 402;");
            migrationBuilder.Sql(@"delete from studentevent where id = 401 or id = 402;");
            migrationBuilder.Sql(@"delete from studentstatus where id = 4;");

            migrationBuilder.Sql(@"insert into studentstatus(id, version, name, namealt, alias, isactive, vieworder)
                values(4, 0, 'В процес на дипломиране', 'In the process of graduation', 'processGraduation', true, 4);");

            migrationBuilder.Sql(@"insert into studentevent(id, studentstatusid, version, name, namealt, alias, isactive, vieworder)
				values(401, 4, 0, 'Семестриално завършил', 'Graduated course', 'graduatedCourse', true, 34),
					  (402, 4, 0, 'Отчислен с право на защита', 'Deducted with the right to defense', 'deductedWithDefense', true, 35),
					  (403, 4, 0, 'Отлагане на дипломиране поради невзет изпит', 'Postponement of graduation due to failure to take an exam', 'postponementFailedExam', true, 36),
					  (404, 4, 0, 'Отлагане на дипломиране по собствено желание', 'Postponement of graduation of own will', 'postponementOwnWill', true, 37),
                      (405, 4, 0, 'Отлагане на дипломиране по болест', 'Postponement of graduation due to illness', 'postponementIllness', true, 38),
                      (406, 4, 0, 'Отлагане на дипломиране поради бременност и майчинство', 'Postponement of graduation due to pregnancy', 'postponementPregnancy', true, 39);
			");

            migrationBuilder.Sql(@"insert into studenteventqualification(studenteventid, educationalqualificationid, version)
				values(403, 1, 0),
					  (403, 2, 0),
                      (403, 3, 0),
                      (403, 4, 0),
                      (403, 5, 0),
                      (404, 1, 0),
					  (404, 2, 0),
                      (404, 3, 0),
                      (404, 4, 0),
                      (404, 5, 0),
                      (405, 1, 0),
					  (405, 2, 0),
                      (405, 3, 0),
                      (405, 4, 0),
                      (405, 5, 0),
                      (406, 1, 0),
					  (406, 2, 0),
                      (406, 3, 0),
                      (406, 4, 0),
                      (406, 5, 0);
			");

            migrationBuilder.Sql(@"update studenteventqualification set studenteventid = 401 where studenteventid = 301;");
            migrationBuilder.Sql(@"update studenteventqualification set studenteventid = 402 where studenteventid = 302;");

            migrationBuilder.Sql(@"update personstudent set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update personstudent set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");
            migrationBuilder.Sql(@"update personstudentsemester set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update personstudentsemester set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");
            migrationBuilder.Sql(@"update personstudenthistory set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update personstudenthistory set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");
            migrationBuilder.Sql(@"update personstudentsemesterhistory set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update personstudentsemesterhistory set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");

            migrationBuilder.Sql(@"update persondoctoral set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update persondoctoral set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");
            migrationBuilder.Sql(@"update persondoctoralsemester set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update persondoctoralsemester set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");
            migrationBuilder.Sql(@"update persondoctoralhistory set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update persondoctoralhistory set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");
            migrationBuilder.Sql(@"update persondoctoralsemesterhistory set studenteventid = 401, studentstatusid = 4 where studenteventid = 301;");
            migrationBuilder.Sql(@"update persondoctoralsemesterhistory set studenteventid = 402, studentstatusid = 4 where studenteventid = 302;");

            migrationBuilder.Sql(@"delete from studentevent where id = 301 or id = 302;");

            migrationBuilder.Sql(@"insert into studenteventqualification(studenteventid, educationalqualificationid, version) values(107, 5, 0);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
