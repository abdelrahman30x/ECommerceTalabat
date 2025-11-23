import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  errors: string[] = []; 

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      username: [null, Validators.required],
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      email: [
        null,
        [
          Validators.required,
          Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')
        ],
        [this.validateEmailNotTaken()]
      ],
      alternativeEmail: [null],
      mobileNumber: [null, Validators.required],
      bio: [null],

      password: [
        null,
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ],
      confirmPassword: [null, Validators.required]
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/shop');
      },
      error: (error) => {
        const rawErrors: any = error?.error?.errors;

        if (rawErrors && typeof rawErrors === 'object') {
          this.errors = Object.values(rawErrors as Record<string, string[]>)
            .reduce((acc, val) => acc.concat(val as string[]), [] as string[]);
        } else {
          this.errors = ['Registration failed.'];
        }
      }
    });
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) return of(null);

          return this.accountService.checkEmailExists(control.value).pipe(
            map((res: any) => {
              return res.available === false ? { emailExists: true } : null;
            })
          );
        })
      );
    };
  }
}
