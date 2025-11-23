// models/user.ts
export interface IUser {
  email: string;
  displayName: string;
  token?: string;
  roles?: string[];
}

export interface IAuthResponse {
  success: boolean;
  message: string;
  token: string;
  user: IUser;
}

// models/address.ts
export interface IAddress {
  firstName: string;
  lastName: string;
  addressLine1: string;
  addressLine2?: string;
  city: string;
  country: string;
  postalCode: string;
  phoneNumber: string;
  isActive?: boolean;
  isDefault?: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}