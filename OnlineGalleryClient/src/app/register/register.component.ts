import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  formGroup: FormGroup

  constructor(private authService: AuthService,) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.formGroup = new FormGroup({
      username: new FormControl("", [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
        Validators.pattern(new RegExp("^([a-zA-Z1-9\d._@+-])*$"))
      ]),
      firstName: new FormControl("", [
        Validators.maxLength(30)
      ]),
      lastName: new FormControl("", [
        Validators.maxLength(30)
      ]),
      password: new FormControl("", [
        Validators.required,
        Validators.maxLength(255), 
        Validators.pattern(new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=\-_!*()@%&]).{6,}$"))
      ])
    });
  }

  register(){
    if(this.formGroup.valid){
      this.authService.register(this.formGroup.value);
    }
  }
}