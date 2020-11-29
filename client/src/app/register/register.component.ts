import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Member } from '../_models/member';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  validationErrors: string[] = [];
  // userParams: UserParams;
  // areaList = [{ value: 'Fitness & Health', display: 'Fitness & Health' }, { value: 'IT', display: 'IT' },
  //   { value: 'Spa and Beauty', display: 'Spa and Beauty' }, { value: 'Home and Repair', display: 'Home and Repairs' },
  //   { value: 'Courier', display: 'Couriers' }  ]

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder,
  private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  // selectChangeHandler(event: any) {
  //   //this.member = event.target.value;
  //   //this.area = event.target.value;
  // }

  initializeForm() {
    this.registerForm = this.fb.group({
      area: ['Fitness & Health'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(10)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null: {isMatching: true}
    }
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, error => {
        this.validationErrors = error;
        
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
