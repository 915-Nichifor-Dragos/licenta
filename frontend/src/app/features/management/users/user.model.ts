export interface UserManagementUserListing {
  id: string;
  firstName: string;
  lastName: string;
  role: string;
  email: string;
  birthDate: Date;
  registrationDate: Date;
}

export interface UserEditRole {
  username: string;
  role: string;
}
