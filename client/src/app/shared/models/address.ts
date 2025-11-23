export interface IAddress {
    firstName: string;
    lastName: string;
    addressLine1: string;
    addressLine2?: string;
    city: string;
    country: string;
    postalCode: string;
    phoneNumber?: string;
    isDefault?: boolean;
    isActive?: boolean;
}
