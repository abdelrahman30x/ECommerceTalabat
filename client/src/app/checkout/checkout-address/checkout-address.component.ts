import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/shared/models/address';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

saveUserAddress() {
  const v = this.checkoutForm.get('addressForm').value;

  const addressToSend: IAddress = {
    firstName: v.firstName,
    lastName: v.lastName,
    addressLine1: v.addressLine1,
    addressLine2: v.addressLine2,
    city: v.city,
    country: v.country,
    postalCode: v.postalCode,
    phoneNumber: v.phoneNumber,
    isDefault: true,
    isActive: true
  };

  this.accountService.updateUserAddress(addressToSend).subscribe((address: IAddress) => {
    this.toastr.success('Address saved');
    this.checkoutForm.get('addressForm').reset(address);
  }, error => {
    this.toastr.error(error.message);
    console.log(error);
  });
}

}