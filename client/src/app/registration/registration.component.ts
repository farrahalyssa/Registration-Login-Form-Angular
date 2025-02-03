import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  imports: [CommonModule, FormsModule],
  standalone:true,
  styleUrls: ['./registration.component.css']
})

export class RegistrationComponent {
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  password: string = '';

  constructor(private router: Router) {}

  register(event: Event) {
    event.preventDefault();
    this.router.navigate(['home']);  
  }
}
