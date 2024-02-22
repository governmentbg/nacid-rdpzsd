﻿namespace Infrastructure.Constants
{
    public class StudentEventConstants
    {
        public const string NextSemesterAfterRelocation = "nextSemesterAfterRelocation";
        public const string NextSemesterAfterRelocationAbroad = "nextSemesterAfterRelocationAbroad";

        public const string IndividualPlanTwoYears = "individualPlanTwoYears";
        public const string IndividualPlanTwoSemesters = "individualPlanTwoSemesters";

        public const string Relocation = "relocation";

        public const string GraduatedCourse = "graduatedCourse";
        public const string DeductedWithDefense = "deductedWithDefense";

        public const string GraduatedWithDiploma = "graduatedWithDiploma";
        public const string GraduatedWithoutDiploma = "graduatedWithoutDiploma";
        // Id's must no change they are generated by us, not by the database
        public const int GraduatedWithDiplomaId = 501;
        public const int GraduatedWithoutDiplomaId = 502;
    }
}