import { InstitutionDto } from "./institution/institution.dto";

export class UserDto {
  userId: number;
  userFullname: string;
  userType: UserType;
  permissions: string[] = [];

  institution: InstitutionDto;
}

export enum UserType {
  ems = 1,
  rnd = 2,
  rsd = 3,
  unauthorized = 4,
  publicUser = 5
}